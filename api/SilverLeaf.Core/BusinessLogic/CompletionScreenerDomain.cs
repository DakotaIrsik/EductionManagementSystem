using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.LookUps;
using SilverLeaf.Common.Models;
using SilverLeaf.Common.Services;
using SilverLeaf.Core.Services;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.ElasticSearch;
using SilverLeaf.Entities.Helpers;
using SilverLeaf.Entities.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilverLeaf.Core.BusinessLogic
{
    public interface ICompletionScreenerDomain
    {
        Task<CompletionScreenerDTO> SaveScreenerAsync(CompletionScreenerDTO model);
        Task<AdjustableDTO<ScreenerDTO>> GetAsync(CompletedScreenerRequest model, bool useNoSql = true);

    }
    public class CompletionScreenerDomain : BaseDomain, ICompletionScreenerDomain
    {
        private readonly IPhonicsScreenerDomain _phonicsScreener;
        private readonly IStudentDomain _student;
        private readonly ICourseDomain _course;
        private readonly IPDFService _pdf;

        List<CourseDTO> Courses { get; set; }
        List<PhonicsScreenerDTO> PhonicsScreeners { get; set; }
        StudentDTO Student { get; set; }

        public CompletionScreenerDomain(EMSContext context,
                              IMapper mapper,
                              IElasticSearchService elastic,
                              ILogger logger,
                              IHttpContextAccessor httpContextAccessor,
                              IOptions<AppSettings> settings,
                              ICacheService cache,
                              IPhonicsScreenerDomain phonicsScreenerDomain,
                              IStudentDomain studentDomain,
                              ICourseDomain courseDomain,
                              IPDFService pdfService) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _phonicsScreener = phonicsScreenerDomain;
            _student = studentDomain;
            _course = courseDomain;
            _pdf = pdfService;
        }

        public async Task<AdjustableDTO<ScreenerDTO>> GetAsync(CompletedScreenerRequest model, bool useNoSql = true)
        {
            AdjustableDTO<ScreenerDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticScreener>(new ScreenerSearchRequest(model) { StudentId = model.StudentId }, "screener");
                result = new AdjustableDTO<ScreenerDTO>(model, _mapper.Map<List<ScreenerDTO>>(response.Data));
            }
            else
            {
                var response = await _context.Screener.CompletedScreenerSearchQuery(_mapper, new ScreenerSearchRequest(model) { StudentId = model.StudentId });
                result = new AdjustableDTO<ScreenerDTO>(model, _mapper.Map<List<ScreenerDTO>>(response.Data));
            }

            return result;
        }

        public async Task<CompletionScreenerDTO> SaveScreenerAsync(CompletionScreenerDTO model)
        {
            await LoadScreenerInformation(model.Student.Id);
            AssociateScreenerResults(ref model);
            model.PrimaryRecommendation = await CourseRecommendation(model.Phonics.PhonicsTasks);
            model.Student = Student;
            model.Assessor = Student.Assessor;

            var starReading = _mapper.Map<StarReading>(model.StarReading);
            starReading.StudentId = model.Student.Id;
            var starReadingDTO = _mapper.Map<StarReadingDTO>(starReading);

            model.StarReading = starReadingDTO;
            model.Url = _pdf.GenerateScreenerEvaluationForm(model);

            var screener = _mapper.Map<Screener>(model);
            var student = _context.Student.Find(model.Student.Id);
            student.IsBeginnerOralScreenerComplete = true;
            student.IsComprehensionScreenerComplete = true;
            student.IsPhonicsScreenerComplete = true;

            _context.Screener.Add(screener);
            _context.StarReading.Add(starReading);
            await ValidateAndSaveChangesAsync();
            var x = Errors;
            return model;
        }

        private void AssociateScreenerResults(ref CompletionScreenerDTO model)
        {
            var phonics1 = new Dictionary<int, int> { { 1, 13 }, { 2, 14 }, { 3, 14 }, { 4, 18 } };
            var phonics2 = new Dictionary<int, int> { { 1, 12 }, { 2, 12 }, { 3, 11 }, { 4, 11 } };
            var phonics3 = new Dictionary<int, int> { { 1, 13 }, { 2, 11 }, { 3, 13 }, { 4, 11 } };
            var phonics4 = new Dictionary<int, int> { { 1, 9 }, { 2, 8 }, { 3, 8 }, { 4, 9 } };

            model.Phonics.PhonicsTasks.AddRange(GetPhonicsTask(1, phonics1));
            model.Phonics.PhonicsTasks.AddRange(GetPhonicsTask(2, phonics2));
            model.Phonics.PhonicsTasks.AddRange(GetPhonicsTask(3, phonics3));
            model.Phonics.PhonicsTasks.AddRange(GetPhonicsTask(4, phonics4));
        }

        private async Task LoadScreenerInformation(int studentId)
        {
            Student = await _student.GetAsync(studentId, false);
            PhonicsScreeners = await _phonicsScreener.GetByStudentAsync(studentId, new PagingRequest(true), false); // TODO Find out why I can't use ElasticSearch.
        }

        private List<PhonicsTask> GetPhonicsTask(int courseId, Dictionary<int, int> taskThreshhold)
        {
            var result = new List<PhonicsTask>();
            foreach (var task in taskThreshhold)
            {
                result.Add(new PhonicsTask(courseId, task.Key, task.Value)
                {
                    Correct = PhonicsScreeners.Where(ps => ps.CourseId == courseId && ps.Task == task.Key && ps.IsCorrect == true).Count(),
                    Total = PhonicsScreeners.Where(ps => ps.CourseId == courseId && ps.Task == task.Key).Count()
                }); ;
            }
            return result;
        }

        private async Task<string> CourseRecommendation(List<PhonicsTask> phonics)
        {
            var result = await _course.GetAsync();
            Courses = result.Data.ToList();
            var course = Courses.Where(c => c.Name.Contains("Phonics")).OrderBy(c => c.Name).FirstOrDefault();
            foreach (var p in phonics)
            {
                if (p.Correct >= p.PassThreshhold)
                {
                    course = Courses.FirstOrDefault(c => c.Name.ToLower() == $"{nameof(phonics)}{p.CourseId}");
                }
                else
                {
                    break;
                }
            }

            return course.Name;
        }
    }
}
