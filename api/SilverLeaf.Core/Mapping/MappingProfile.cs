using AutoMapper;
using Newtonsoft.Json;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.ElasticSearch;
using SilverLeaf.Entities.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using System.Linq;
using CommonModels = SilverLeaf.Common.Models;

namespace SilverLeaf.Core.Mapping
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Please try to keep these organized by region and also sorted alphabetically, else it will grow unwieldly quick!!!
        /// </summary>
        public MappingProfile()
        {
            #region SQL to DTO (Sort Alphabetic ASC)
            CreateMap<ApplicationFile, ApplicationFileDTO>();
            CreateMap<Center, CenterDTO>();
            CreateMap<Chat, ChatDTO>();
            CreateMap<Class, ClassDTO>();
            CreateMap<Course, CourseDTO>();
            CreateMap<Feedback, FeedbackDTO>();
            CreateMap<Fry, FryDTO>();
            CreateMap<HoursOfOperation, HoursOfOperationDTO>();
            CreateMap<ComprehensionScreener, ComprehensionScreenerDTO>();
            CreateMap<ComprehensionScreenerResult, ComprehensionScreenerDTO>();
            CreateMap<OralScreener, OralScreenerDTO>();
            CreateMap<OralScreenerResult, OralScreenerDTO>();
            CreateMap<PhonicsScreener, PhonicsScreenerDTO>();
            CreateMap<PhonicsScreenerResult, PhonicsScreenerDTO>();
            CreateMap<PhonicsSkill, PhonicsSkillDTO>();
            CreateMap<Student, StudentDTO>();
            CreateMap<StarReading, StarReadingDTO>();
            CreateMap<User, UserDTO>();

            #endregion

            #region SQL to Elastic (Sort Alphabetic ASC)
            CreateMap<Center, ElasticCenter>();
            CreateMap<Course, ElasticCourse>();
            CreateMap<Chat, ElasticChat>();
            CreateMap<Class, ElasticClass>();
            CreateMap<ComprehensionScreener, ElasticComprehensionScreener>();
            CreateMap<ComprehensionScreenerResult, ElasticComprehensionScreenerResult>();
            CreateMap<Feedback, ElasticFeedback>();
            CreateMap<Fry, ElasticFry>();
            CreateMap<OralScreener, ElasticOralScreener>();
            CreateMap<OralScreenerResult, ElasticOralScreenerResult>();
            CreateMap<PhonicsScreener, ElasticPhonicsScreener>();
            CreateMap<PhonicsScreenerResult, ElasticPhonicsScreenerResult>();
            CreateMap<PhonicsSkill, ElasticPhonicsSkill>();
            CreateMap<Screener, ElasticScreener>();
            CreateMap<Student, ElasticStudent>();
            CreateMap<Room, ElasticRoom>();
            #endregion

            #region DTO to SQL (Sort Alphabetic ASC)
            CreateMap<ApplicationFileDTO, ApplicationFile>();
            CreateMap<CenterDTO, Center>();
            CreateMap<ChatDTO, Chat>();
            CreateMap<ClassDTO, Class>();
            CreateMap<FeedbackDTO, Feedback>();
            CreateMap<FryDTO, Fry>();
            CreateMap<HoursOfOperationDTO, HoursOfOperation>();
            CreateMap<ComprehensionScreenerDTO, ComprehensionScreener>();
            CreateMap<ComprehensionScreenerDTO, ComprehensionScreenerResult>()
                .ForMember(x => x.ComprehensionScreenerId, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Id, y => y.MapFrom(z => 0));
            CreateMap<CompletionScreenerDTO, Screener>()
              .ForMember(x => x.StudentId, y => y.MapFrom(z => z.Student.Id))
              .ForMember(x => x.Student, opt => opt.Ignore())
              .ForMember(x => x.Course1, y => y.MapFrom(z => JsonConvert.SerializeObject(z.Phonics.PhonicsTasks.Where(t => t.CourseId == 1))))
              .ForMember(x => x.Course2, y => y.MapFrom(z => JsonConvert.SerializeObject(z.Phonics.PhonicsTasks.Where(t => t.CourseId == 2))))
              .ForMember(x => x.Course3, y => y.MapFrom(z => JsonConvert.SerializeObject(z.Phonics.PhonicsTasks.Where(t => t.CourseId == 3))))
              .ForMember(x => x.Course4, y => y.MapFrom(z => JsonConvert.SerializeObject(z.Phonics.PhonicsTasks.Where(t => t.CourseId == 4))));
            CreateMap<CompletionScreenerDTO, StarReading>()
                .ForMember(x => x.StudentId, y => y.MapFrom(z => z.Student.Id))
                .ForMember(x => x.Student, opt => opt.Ignore())
                .ForMember(x => x.GradeEquivalentLevel, y => y.MapFrom(z => z.StarReading.GradeEquivalentLevel))
                .ForMember(x => x.InstructionalReadingLevel, y => y.MapFrom(z => z.StarReading.InstructionalReadingLevel))
                .ForMember(x => x.PracticeQuestionsAnswered, y => y.MapFrom(z => z.StarReading.PracticeQuestionsAnswered))
                .ForMember(x => x.ZoneOfProximalDevelopment, y => y.MapFrom(z => z.StarReading.ZoneOfProximalDevelopment))
                .ForMember(x => x.TimeTaken, y => y.MapFrom(z => z.StarReading.TimeTaken));
            CreateMap<OralScreenerDTO, OralScreener>();
            CreateMap<OralScreenerDTO, OralScreenerResult>()
                .ForMember(x => x.OralScreenerId, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Id, y => y.MapFrom(z => 0));
            CreateMap<PhonicsScreenerDTO, PhonicsScreener>();
            CreateMap<PhonicsScreenerDTO, PhonicsScreenerResult>()
                 .ForMember(x => x.PhonicsScreenerId, y => y.MapFrom(z => z.Id))
                 .ForMember(x => x.Id, y => y.MapFrom(z => 0));
            CreateMap<PhonicsSkillDTO, PhonicsSkill>();
            CreateMap<RoomDTO, Room>().ReverseMap();
            CreateMap<StarReadingDTO, StarReading>();
            CreateMap<StudentDTO, Student>();
            CreateMap<UserDTO, User>();
          
            #endregion

            #region DTO to Elastic (Sort Alphabetic ASC)
            CreateMap<CenterDTO, ElasticCenter>();
            CreateMap<ChatDTO, ElasticChat>();
            CreateMap<ClassDTO, ElasticClass>();
            CreateMap<StudentDTO, ElasticStudent>();
            CreateMap<FeedbackDTO, ElasticFeedback>();
            CreateMap<Fry, ElasticFry>();
            CreateMap<ComprehensionScreenerDTO, ElasticComprehensionScreener>();
            CreateMap<ComprehensionScreenerDTO, ElasticComprehensionScreenerResult>();
            CreateMap<OralScreenerDTO, ElasticOralScreener>();
            CreateMap<OralScreenerDTO, ElasticOralScreenerResult>();
            CreateMap<PhonicsScreenerDTO, ElasticPhonicsScreener>();
            CreateMap<PhonicsScreenerDTO, ElasticPhonicsScreenerResult>();
            CreateMap<PhonicsSkill, ElasticPhonicsSkill>();
            CreateMap<StudentDTO, ElasticStudent>();
            CreateMap<ScreenerDTO, ElasticScreener>();
            #endregion

            #region Elastic to DTO (Sort Alphabetic ASC)
            CreateMap<ElasticCenter, CenterDTO>();
            CreateMap<ElasticCourse, CourseDTO>();
            CreateMap<ElasticChat, ChatDTO>();
            CreateMap<ElasticClass, ClassDTO>();
            CreateMap<ElasticFeedback, FeedbackDTO>();
            CreateMap<ElasticFry, FryDTO>();
            CreateMap<ElasticComprehensionScreener, ComprehensionScreenerDTO>();
            CreateMap<ElasticComprehensionScreenerResult, ComprehensionScreenerDTO>();
            CreateMap<ElasticOralScreener, OralScreenerDTO>();
            CreateMap<ElasticOralScreenerResult, OralScreenerDTO>();
            CreateMap<ElasticPhonicsScreener, PhonicsScreenerDTO>();
            CreateMap<ElasticPhonicsScreenerResult, PhonicsScreenerDTO>();
            CreateMap<ElasticPhonicsSkill, PhonicsSkillDTO>();
            CreateMap<ElasticStudent, StudentDTO>();
            CreateMap<ElasticScreener, ScreenerDTO>();
            #endregion

            #region ViewModel to DTO (Sort Alphabetic ASC)
            CreateMap<FeedbackRequest, FeedbackDTO>();
            CreateMap<PhonicsScreenerSubmissionRequest, PhonicsScreenerDTO>();
            CreateMap<OralScreenerSubmissionRequest, OralScreenerDTO>();
            CreateMap<ComprehensionScreenerSubmissionRequest, ComprehensionScreenerDTO>();
            CreateMap<StudentRegistrationRequest, StudentDTO>();
            CreateMap<ClassSearchRequest, ClassDTO>();
            CreateMap<CompleteScreenerRequest, CompletionScreenerDTO>();
            #endregion

            #region Miscellaneous (Sort Alphabetic ASC)
            CreateMap<CommonModels.ApplicationFile, ApplicationFile>();
            #endregion
        }
    }
}