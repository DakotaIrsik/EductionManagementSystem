using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SilverLeaf.Common.Models;
using SilverLeaf.Common.Services;
using SilverLeaf.Core.BusinessLogic;
using SilverLeaf.common.Services;
using SilverLeaf.Core.Services;

namespace SilverLeaf.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddSingleton<IImageService, ImageService>();
            services.AddTransient<IStudentDomain, StudentDomain>();
            services.AddTransient<ICourseDomain, CourseDomain>();
            services.AddTransient<IClassDomain, ClassDomain>();
            services.AddTransient<IUserDomain, UserDomain>();
            services.AddTransient<IDiagnosticsDomain, DiagnosticsDomain>();
            services.AddTransient<IFeedbackDomain, FeedbackDomain>();
            //services.AddTransient<IChatDomain, ChatDomain>();
            services.AddTransient<IComprehensionScreenerDomain, ComprehensionScreenerDomain>();
            services.AddTransient<IPhonicsScreenerDomain, PhonicsScreenerDomain>();
            services.AddTransient<IOralScreenerDomain, OralScreenerDomain>();
            services.AddTransient<IPendingScreenerDomain, PendingScreenerDomain>();
            services.AddTransient<IFryDomain, FryDomain>();
            services.AddTransient<IPhonicsSkillDomain, PhonicsSkillDomain>();
            services.AddTransient<ICompletionScreenerDomain, CompletionScreenerDomain>();

            services.AddTransient<IPDFService, PDFService>();

            services.AddTransient<IMapper, Mapper>();
            services.AddTransient<Message, Message>();
            services.AddTransient<Error, Error>();

            services.AddTransient<IBaseDomain, BaseDomain>();
            
            return services;
        }
    }
}
