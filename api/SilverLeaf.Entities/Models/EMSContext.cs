using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using SilverLeaf.Common;
using SilverLeaf.Common.Extensions;
using SilverLeaf.Common.Helpers;
using SilverLeaf.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SilverLeaf.Entities.Models
{
    public class EMSContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IElasticSearchService _elastic;
        private readonly AppSettings _settings;
        private readonly IMapper _mapper;
        public EMSContext(DbContextOptions<EMSContext> options,
                            IHttpContextAccessor httpContext,
                            IElasticSearchService elastic,
                            IOptions<AppSettings> settings,
                            IMapper mapper) : base(options)
        {
            _httpContext = httpContext;
            _elastic = elastic;
            _settings = settings.Value;
            _mapper = mapper;
        }

        public DbSet<Student> Student { get; set; }

        public DbSet<Room> Room { get; set; }

        public DbSet<Fry> Fry { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Course> Course { get; set; }

        public DbSet<Feedback> Feedback { get; set; }

        public DbSet<Chat> Chat { get; set; }

        public DbSet<Staff> Staff { get; set; }

        public DbSet<Center> Center { get; set; }

        public DbSet<Class> Class { get; set; }

        public DbSet<OralScreener> OralScreener { get; set; }

        public DbSet<OralScreenerResult> OralScreenerResult { get; set; }

        public DbSet<PhonicsScreener> PhonicsScreener { get; set; }

        public DbSet<PhonicsScreenerSkill> PhonicsScreenerSkill { get; set; }

        public DbSet<PhonicsSkill> PhonicsSkill { get; set; }

        public DbSet<PhonicsScreenerResult> PhonicsScreenerResult { get; set; }

        public DbSet<ComprehensionScreener> ComprehensionScreener { get; set; }

        public DbSet<ComprehensionScreenerResult> ComprehensionScreenerResult { get; set; }

        public DbSet<ClassStaff> ClassStaff { get; set; }

        public DbSet<ClassStaff> ClassStudent { get; set; }

        public DbSet<StarReading> StarReading { get; set; }

        public DbSet<Screener> Screener { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            AddDefaultEntities(builder);

            builder.Entity<ClassStaff>().HasKey(sc => new { sc.StaffId, sc.ClassId });
            builder.Entity<ClassStudent>().HasKey(sc => new { sc.StudentId, sc.ClassId });

            builder.Entity<User>();
        }


#pragma warning disable 0809
        [Obsolete("Consider using BaseDomain's ValidateAndSaveChanges(), it does some basic error handling for common SQL errors, and will give you messages if your save is successful.", true)]
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            var x = base.SaveChanges(acceptAllChangesOnSuccess);
            OnAfterSaving();
            return x;
        }

        [Obsolete("Consider using BaseDomain's ValidateAndSaveChangesAsync(), it does some basic error handling for common SQL errors, and will give you messages if your save is successful.", true)]
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            var x = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await OnAfterSavingAsync();
            return x;
        }
#pragma warning restore 0809

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is Trackable trackable)
                {
                    var now = DateTime.UtcNow;
                    var user = GetCurrentUserName();
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdateDate = now;
                            trackable.UpdatedBy = user;
                            break;

                        case EntityState.Added:
                            trackable.CreateDate = now;
                            trackable.CreatedBy = user;
                            trackable.UpdateDate = now;
                            trackable.UpdatedBy = user;
                            break;
                    }
                }
                //if there's a FK to UserId, set it here to keep the BLL clean.
                var userIdFk = entry.Entity.GetType().GetProperty("UserId");
                if (userIdFk != null && userIdFk.GetValue(entry.Entity) == null)
                {
                    userIdFk.SetValue(entry.Entity, GetCurrentUserId());
                }
            }
        }

        private async Task OnAfterSavingAsync()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(a => a.GetName().Name == _settings.EntityAssemblyName);
            List<object> elasticDocuments = new List<object>();
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                var elasticType = assembly.GetTypes().SingleOrDefault(t => t.Name == $"Elastic{entry.Entity.GetType().Name}");
                if (elasticType != null)
                {
                    elasticDocuments.Add(MapToElastic(entry.Entity, elasticType));
                }
            }
            if (elasticDocuments.Any())
            {
                //TODO: find out why the entities aren't being 'let go' by the Change Tracker, 
                //this is resulting in this list of elastic documents to contain objects that should belong in different indexes
                //preventing index name inference (if the first elastic document is of type Reward, then it tries to send all documents
                //to the Reward index.
                var seperateLists = elasticDocuments.PartitionByType();
                foreach (var list in seperateLists)
                {
                    var result = await _elastic.IndexManyAsync(list, list.Safe().FirstOrDefault().GetType().ElasticIndexResolver());
                }
            }
        }


        private void OnAfterSaving()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(a => a.GetName().Name == _settings.EntityAssemblyName);
            List<object> elasticDocuments = new List<object>();
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                var elasticType = assembly.GetTypes().SingleOrDefault(t => t.Name == $"Elastic{entry.Entity.GetType().Name}");
                if (elasticType != null)
                {
                    elasticDocuments.Add(MapToElastic(entry.Entity, elasticType));
                }
            }
            if (elasticDocuments.Any())
            {
                //TODO: find out why the entities aren't being 'let go' by the Change Tracker, 
                //this is resulting in this list of elastic documents to contain objects that should belong in different indexes
                //preventing index name inference (if the first elastic document is of type Reward, then it tries to send all documents
                //to the Reward index.
                var seperateLists = elasticDocuments.PartitionByType();
                foreach (var list in seperateLists)
                {
                    var result = _elastic.IndexMany(elasticDocuments, list.Safe().FirstOrDefault().GetType().ElasticIndexResolver());
                }
            }
        }


        private object MapToElastic(object obj, Type elasticType)
        {
            var elasticObject = Activator.CreateInstance(elasticType);
            var mappedObject = _mapper.Map(obj, elasticObject, obj.GetType(), elasticType);
            return mappedObject;
        }

        private ClaimsPrincipal GetCurrentUser()
        {
            return _httpContext.HttpContext?.User;
        }

        private string GetCurrentUserName()
        {
            return GetCurrentUser()?.Identity?.Name ?? "System";
        }

        private string GetCurrentUserId()
        {
            return GetCurrentUser()?.Claims?.SingleOrDefault(c => c.Type == "sub")?.Value;
        }

        private void AddDefaultEntities(ModelBuilder builder)
        {
            builder.Entity<User>()
              .HasIndex(u => u.Id)
              .IsUnique();


            builder.Entity<Log>(log =>
            {
                log.HasKey(x => x.Id);
                log.Property(x => x.Level).HasMaxLength(128);
            });
        }
    }
}
