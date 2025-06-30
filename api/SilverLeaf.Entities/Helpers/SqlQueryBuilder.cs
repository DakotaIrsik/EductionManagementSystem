using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SilverLeaf.Common.Extensions;
using SilverLeaf.Common.Interfaces;
using SilverLeaf.Common.Models;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilverLeaf.Entities.Helpers
{
    public static class SqlQueryExtensions
    {
        public static async Task<AdjustableDTO<StudentDTO>> StudentSearchQuery(this IQueryable<Student> query, IMapper mapper, StudentSearchRequest request)
        {
            if (request.IsPhonicsScreenerComplete != null)
            {
                var response2 = await query
                                  .Include(s => s.PhonicsScreenerResults)
                                  .Where(s => s.PhonicsScreenerResults.Any(psr => psr.Assessor == "System"))
                                  .SortBy(request)
                                  .GetPaged(request);
                return new AdjustableDTO<StudentDTO>(request, mapper.Map<List<StudentDTO>>(response2.Data), response2.Total);
            }

            var response = await query.SortBy(request)
                                      .GetPaged(request);

            return new AdjustableDTO<StudentDTO>(request, mapper.Map<List<StudentDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<StudentDTO>> IncompleteScreenerSearchQueryAsync(this IQueryable<PhonicsScreenerResult> query, IMapper mapper, IAdjustable pagingRequest)
        {

            var studentsWithUnAnsweredScreenerQuestions = query.Where(q => q.Assessor == "System").Select(q => q.Student);

            var response = await studentsWithUnAnsweredScreenerQuestions
                                    .SortBy(pagingRequest)
                                    .GetPaged(pagingRequest);

            return new AdjustableDTO<StudentDTO>(pagingRequest, mapper.Map<List<StudentDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<TeacherDTO>> TeacherSearchQuery(this IQueryable<Staff> query, IMapper mapper, TeacherSearchRequest request)
        {
            if (request.Id > 0)
            {
                var response1 = await query
                                  .Where(s => s.Id == request.Id)
                                  .SortBy(request)
                                  .GetPaged(request);

                return new AdjustableDTO<TeacherDTO>(request, mapper.Map<List<TeacherDTO>>(response1.Data), response1.Total);
            }

            var response = await query.SortBy(request)
                                      .GetPaged(request);

            return new AdjustableDTO<TeacherDTO>(request, mapper.Map<List<TeacherDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<RoomDTO>> RoomSearchQuery(this IQueryable<Room> query, IMapper mapper, RoomSearchRequest request)
        {

            if (request.Id > 0)
            {
                var response1 = await query
                                  .Where(s => s.Id == request.Id)
                                  .SortBy(request)
                                  .GetPaged(request);

                return new AdjustableDTO<RoomDTO>(request, mapper.Map<List<RoomDTO>>(response1.Data), response1.Total);
            }

            if (!string.IsNullOrEmpty(request.UserId))
            {
                var response2 = await query
                                  .Where(c => c.Name == request.UserId)
                                  .SortBy(request)
                                  .GetPaged(request);
                return new AdjustableDTO<RoomDTO>(request, mapper.Map<List<RoomDTO>>(response2.Data), response2.Total);
            }


            var response = await query.SortBy(request)
                                      .GetPaged(request);

            return new AdjustableDTO<RoomDTO>(request, mapper.Map<List<RoomDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<CourseDTO>> CourseSearchQuery(this IQueryable<Course> query, IMapper mapper, CourseSearchRequest request)
        {
            if (request.Id > 0)
            {
                var response1 = await query.Where(s => s.Id == request.Id)
                                  .SortBy(request)
                                  .GetPaged(request);

                return new AdjustableDTO<CourseDTO>(request, mapper.Map<List<CourseDTO>>(response1.Data), response1.Total);
            }

            var response = await query.SortBy(request)
                                      .GetPaged(request);

            return new AdjustableDTO<CourseDTO>(request, mapper.Map<List<CourseDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<ClassDTO>> ClassSearchQuery(this IQueryable<Class> query, IMapper mapper, ClassSearchRequest request)
        {
            query = query.Include(q => q.Course);

            if (request.Id > 0)
            {
                var response1 = await query.Where(s => s.Id == request.Id)
                                  .SortBy(request)
                                  .GetPaged(request);

                return new AdjustableDTO<ClassDTO>(request, mapper.Map<List<ClassDTO>>(response1.Data), response1.Total);
            }

            if (request.CourseId > 0)
            {
                query = query.Where(c => c.CourseId == request.CourseId);
            }

            if (request.Lesson > 0)
            {
                query = query.Where(c => c.Lesson == request.Lesson);
            }

            if (request.Week > 0)
            {
                query = query.Where(c => c.Week == request.Week);
            }

            if (!string.IsNullOrWhiteSpace(request.Session.ToString()))
            {
                query = query.Where(c => c.Session.Contains(request.Session));
            }

            if (!string.IsNullOrWhiteSpace(request.Card.ToString()))
            {
                query = query.Where(c => c.Card.Contains(request.Card));
            }

            if (!string.IsNullOrWhiteSpace(request.Fictionality.ToString()))
            {
                query = query.Where(c => c.Fictionality.Contains(request.Fictionality));
            }

            if (!string.IsNullOrWhiteSpace(request.Genre.ToString()))
            {
                query = query.Where(c => c.Genre.Contains(request.Genre));
            }

            if (!string.IsNullOrWhiteSpace(request.Title.ToString()))
            {
                query = query.Where(c => c.Title.Contains(request.Title));
            }


            var response = await query.SortBy(request)
                                      .GetPaged(request);

            return new AdjustableDTO<ClassDTO>(request, mapper.Map<List<ClassDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<PhonicsScreenerDTO>> PhonicsScreenerSearchQuery(this IQueryable<PhonicsScreener> query, IMapper mapper, PhonicsScreenerSearchRequest request)
        {
            if (request.Order > 0)
            {
                var response1 = await query.Where(s => s.Order == request.Order)
                                  .SortBy(request)
                                  .GetPaged(request);

                return new AdjustableDTO<PhonicsScreenerDTO>(request, mapper.Map<List<PhonicsScreenerDTO>>(response1.Data), response1.Total);
            }

            if (request.CourseId != 0)
            {
                query = query.Where(c => c.CourseId == request.CourseId);
            }

            if (request.Task != 0)
            {
                query = query.Where(c => c.Task == request.Task);
            }

            var response = await query.SortBy(request)
                                      .GetPaged(request);

            return new AdjustableDTO<PhonicsScreenerDTO>(request, mapper.Map<List<PhonicsScreenerDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<OralScreenerDTO>> OralScreenerSearchQuery(this IQueryable<OralScreener> query, IMapper mapper, OralScreenerSearchRequest request)
        {
            if (request.Order > 0)
            {
                var response1 = await query.Where(s => s.Order == request.Order)
                                  .SortBy(request)
                                  .GetPaged(request);

                return new AdjustableDTO<OralScreenerDTO>(request, mapper.Map<List<OralScreenerDTO>>(response1.Data), response1.Total);
            }

            if (!string.IsNullOrWhiteSpace(request.Question))
            {
                query = query.Where(c => c.Question.Contains(request.Question));
            }

            if (!string.IsNullOrWhiteSpace(request.Image))
            {
                query = query.Where(c => c.Image.Contains(request.Image));
            }

            var response = await query.SortBy(request)
                                      .GetPaged(request);

            return new AdjustableDTO<OralScreenerDTO>(request, mapper.Map<List<OralScreenerDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<ComprehensionScreenerDTO>> ComprehensionScreenerSearchQuery(this IQueryable<ComprehensionScreener> query, IMapper mapper, ComprehensionScreenerSearchRequest request)
        {
            if (request.Order > 0)
            {
                var response1 = await query.Where(s => s.Order == request.Order)
                                  .SortBy(request)
                                  .GetPaged(request);

                return new AdjustableDTO<ComprehensionScreenerDTO>(request, mapper.Map<List<ComprehensionScreenerDTO>>(response1.Data));
            }

            if (!string.IsNullOrWhiteSpace(request.Preface))
            {
                query = query.Where(c => c.Preface.Contains(request.Preface));
            }

            if (!string.IsNullOrWhiteSpace(request.SecondPreface))
            {
                query = query.Where(c => c.SecondPreface.Contains(request.SecondPreface));
            }

            if (!string.IsNullOrWhiteSpace(request.Image))
            {
                query = query.Where(c => c.Image.Contains(request.Image));
            }

            if (!string.IsNullOrWhiteSpace(request.SecondImage))
            {
                query = query.Where(c => c.SecondImage.Contains(request.SecondImage));
            }

            if (!string.IsNullOrWhiteSpace(request.Answers))
            {
                query = query.Where(c => c.Answers.Contains(request.Answers));
            }

            if (!string.IsNullOrWhiteSpace(request.Question))
            {
                query = query.Where(c => c.Question.Contains(request.Question));
            }

            if (!string.IsNullOrWhiteSpace(request.Image))
            {
                query = query.Where(c => c.Image.Contains(request.Image));
            }

            var response = await query.SortBy(request)
                                      .GetPaged(request);

            return new AdjustableDTO<ComprehensionScreenerDTO>(request, mapper.Map<List<ComprehensionScreenerDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<PhonicsScreenerDTO>> PhonicsScreenerResultSearchQuery(this IQueryable<PhonicsScreenerResult> query, IMapper mapper, PhonicsScreenerSearchRequest request)
        {

            query = query.Where(s => s.StudentId == request.StudentId);
            if (request.Incomplete)
            {
                query = query.Where(s => s.IsCorrect == null);
            }

            var response = await query.SortBy(request)
                                .GetPaged(request);

            return new AdjustableDTO<PhonicsScreenerDTO>(request, mapper.Map<List<PhonicsScreenerDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<OralScreenerDTO>> OralScreenerResultSearchQuery(this IQueryable<OralScreenerResult> query, IMapper mapper, OralScreenerSearchRequest request)
        {

            query = query.Where(s => s.StudentId == request.StudentId);

            if (request.Incomplete)
            {
                query = query.Where(s => s.IsCorrect == null);
            }

            var response = await query.SortBy(request)
                              .GetPaged(request);

            return new AdjustableDTO<OralScreenerDTO>(request, mapper.Map<List<OralScreenerDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<ComprehensionScreenerDTO>> ComprehensionScreenerResultSearchQuery(this IQueryable<ComprehensionScreenerResult> query, IMapper mapper, ComprehensionScreenerSearchRequest request)
        {

            query = query.Where(s => s.StudentId == request.StudentId);

            if (request.Incomplete)
            {
                query = query.Where(s => s.IsCorrect == null);
            }

            var response = await query.Where(s => s.StudentId == request.StudentId)
                              .SortBy(request)
                              .GetPaged(request);

            return new AdjustableDTO<ComprehensionScreenerDTO>(request, mapper.Map<List<ComprehensionScreenerDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<StudentDTO>> PendingScreenerSearchQuery(this IQueryable<Student> query, IMapper mapper, PendingScreenerSearchRequest request)
        {
            query = query.Where(q => q.IsBeginnerOralScreenerComplete == false ||
                                     q.IsComprehensionScreenerComplete == false ||
                                     q.IsPhonicsScreenerComplete == false);

            var response = await query
                              .SortBy(request)
                              .GetPaged(request);

            return new AdjustableDTO<StudentDTO>(request, mapper.Map<List<StudentDTO>>(response.Data), response.Total);
        }

        public static async Task<AdjustableDTO<CompletionScreenerDTO>> CompletedScreenerSearchQuery(this IQueryable<Screener> query, IMapper mapper, ScreenerSearchRequest request)
        {
            if (request.StudentId > 0)
            {
                query = query.Where(s => s.StudentId == request.StudentId);
            }

            var response = await query
                  .SortBy(request)
                  .GetPaged(request);

            return new AdjustableDTO<CompletionScreenerDTO>(request, mapper.Map<List<CompletionScreenerDTO>>(response.Data), response.Total);
        }
    }
}
