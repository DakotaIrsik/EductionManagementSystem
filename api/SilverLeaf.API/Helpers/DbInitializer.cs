using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SilverLeaf.Common;
using SilverLeaf.Common.Extensions;
using SilverLeaf.Common.LookUps;
using SilverLeaf.Common.Services;
using SilverLeaf.Entities.ElasticSearch;
using SilverLeaf.Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SilverLeaf.API.Helpers
{
    public static class DbInitializer
    {
        private static List<PhonicsScreener> PhonicsScreenerQuestions;
        private static List<OralScreener> OralScreenerQuestions;
        private static List<ComprehensionScreener> ComprehensionScreenerQuestions;
        private static List<Student> Students;
        private static List<Fry> FryWords;
        private static List<Staff> Teachers;
        private static List<Staff> Operations;
        private static List<Center> Centers;
        private static List<Course> Courses;
        private static List<Room> Rooms;
        private static List<Class> GuidedReading1Classes;
        private static List<PhonicsSkill> PhonicsSkills;

        private static string DeleteMessage(string indexName)
        {
            return $"Creating {indexName} ElasticSearch Index";
        }

        private static string CreateMessage(string indexName)
        {
            return $"Deleting {indexName} ElasticSearch Index";
        }

        private static string MappingFile(string elasticClassName)
        {
            return $"MappingFiles/{elasticClassName}.json";
        }

        private static string DeletingMSSQLDatabaseMessage => "Deleting MSSQL Database";
        private static string CreatingMSSSQLDatabaseMessage => "Creating MSSQL Database";
        private static string DataSuccessfullySeededMessage => "Data Successfully Seeded";

        private static string SeedingMessage(string indexName)
        {
            var dataStructureName = indexName.Replace("Elastic", "", StringComparison.InvariantCulture);
            dataStructureName = (dataStructureName.ToUpperInvariant().EndsWith('S')) ? dataStructureName + "es" : dataStructureName + "s";
            return $"Seeding {dataStructureName}";
        }

        public static async Task Seed(IWebHost host, bool shouldRecreateDatabase, bool shouldSeed)
        {
            using (var serviceScope = host?.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    using (var elasticSearchService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>())
                    {
                        if (shouldRecreateDatabase)
                        {
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticCenter));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticChat));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticClass));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticComprehensionScreener));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticComprehensionScreenerResult));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticCourse));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticFeedback));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticFry));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticOralScreener));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticOralScreenerResult));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticPhonicsScreener));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticPhonicsScreenerResult));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticRoom));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticStudent));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticTeacher));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticPhonicsSkill));
                            await RecreateElasticIndex(elasticSearchService, typeof(ElasticScreener));
                        }
                    }

                    using (var context = scope.ServiceProvider.GetRequiredService<EMSContext>())
                    {
                        if (shouldRecreateDatabase)
                        {
                            Console.WriteLine(DeletingMSSQLDatabaseMessage);
                            await context.Database.EnsureDeletedAsync().ConfigureAwait(false);

                            Console.WriteLine(CreatingMSSSQLDatabaseMessage);
                            await context.Database.EnsureCreatedAsync().ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticCenter)));
                            await SeedCentersAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticCourse)));
                            await SeedCoursesAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticRoom)));
                            await SeedRoomsAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticClass)));
                            await SeedGGR1ClassesAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticFry)));
                            await SeedFryWordsAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticComprehensionScreener)));
                            await SeedComprehensionScreenersAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticOralScreener)));
                            await SeedOralScreenersAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticPhonicsScreener)));
                            await SeedPhonicsScreenersAsync(context).ConfigureAwait(false);
                        }

                        if (shouldSeed)
                        {
                            Console.WriteLine(SeedingMessage(nameof(ElasticStudent)));
                            await SeedStudentsAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticTeacher)));
                            await SeedStaffAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticComprehensionScreenerResult)));
                            await SeedComprehensionScreenerAnswersAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticOralScreenerResult)));
                            await SeedOralScreenerAnswersAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticPhonicsScreenerResult)));
                            await SeedPhonicsScreenerAnswersAsync(context).ConfigureAwait(false);

                            Console.WriteLine(SeedingMessage(nameof(ElasticPhonicsSkill)));
                            await SeedPhonicsSkillsAsync(context).ConfigureAwait(false);

                            Console.WriteLine(DataSuccessfullySeededMessage);
                        }
                    }
                }
            }

        }

        private static async Task RecreateElasticIndex(IElasticSearchService service, Type type)
        {
            var indexName = type.Name.Replace("Elastic", "").ToLower();
            Console.WriteLine(DeleteMessage(indexName));
            await service.DeleteIndexAsync(indexName).ConfigureAwait(false);
            Console.WriteLine(CreateMessage(indexName));
            await service.CreateIndexAsync(indexName, File.ReadAllText(MappingFile(type.Name))).ConfigureAwait(false);
        }

        private static async Task SeedRoomsAsync(EMSContext context)
        {
            var rooms = LoadRooms();
            context.Room.AddRange(rooms);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedCentersAsync(EMSContext context)
        {
            var centers = LoadCenters();
            context.Center.AddRange(centers);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedCoursesAsync(EMSContext context)
        {
            var courses = LoadCourses().OrderBy(c => c.Order);
            foreach (var course in courses)
            {
                context.Course.Add(course);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private static async Task SeedGGR1ClassesAsync(EMSContext context)
        {
            var courses = LoadGuidedReading1Classes();
            await context.Class.AddRangeAsync(courses).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedPhonicsScreenersAsync(EMSContext context)
        {
            var screeners = LoadPhonicsScreeners();
            await context.PhonicsScreener.AddRangeAsync(screeners).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedOralScreenersAsync(EMSContext context)
        {
            var screeners = LoadOralScreeners();
            await context.OralScreener.AddRangeAsync(screeners).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedComprehensionScreenersAsync(EMSContext context)
        {
            var screeners = LoadComprehensionScreeners();
            await context.ComprehensionScreener.AddRangeAsync(screeners).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedStudentsAsync(EMSContext context)
        {
            LoadStudents();
            await context.Student.AddRangeAsync(Students).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedStaffAsync(EMSContext context)
        {
            LoadStaff();
            await context.Staff.AddRangeAsync(Teachers.Concat(Operations)).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedPhonicsScreenerAnswersAsync(EMSContext context)
        {
            var answers = LoadPhonicsScreenerResults();
            int i = 10;
            var split = answers.Split(i);
            foreach (var chunk in split)
            {
                await context.PhonicsScreenerResult.AddRangeAsync(chunk).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                Console.WriteLine($"{i}%");
                i = i + 10;
            }
        }

        private static async Task SeedOralScreenerAnswersAsync(EMSContext context)
        {
            var answers = LoadOralScreenerResults();
            int i = 10;
            var split = answers.Split(i);

            foreach (var chunk in split)
            {
                await context.OralScreenerResult.AddRangeAsync(chunk).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                Console.WriteLine($"{i}%");
                i = i + 10;
            }
        }

        private static async Task SeedComprehensionScreenerAnswersAsync(EMSContext context)
        {
            var answers = LoadComprehensionScreenerResults();
            int i = 10;
            var split = answers.Split(i);

            foreach (var chunk in split)
            {
                await context.ComprehensionScreenerResult.AddRangeAsync(chunk).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                i = i + 10;
            }
        }

        private static async Task SeedFryWordsAsync(EMSContext context)
        {
            var result = LoadFryWords();
            await context.Fry.AddRangeAsync(result).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedPhonicsSkillsAsync(EMSContext context)
        {
            var result = LoadPhonicsSkills();
            await context.PhonicsSkill.AddRangeAsync(result).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static List<Center> LoadCenters()
        {
            string json = File.ReadAllText(@"Data\Centers.json");
            Centers = JsonConvert.DeserializeObject<List<Center>>(json);
            return Centers;
        }

        private static List<Room> LoadRooms()
        {
            string json = File.ReadAllText(@"Data\Rooms.json");
            Rooms = JsonConvert.DeserializeObject<List<Room>>(json);
            return Rooms;
        }

        private static List<Course> LoadCourses()
        {
            string json = File.ReadAllText(@"Data\Courses.json");
            Courses = JsonConvert.DeserializeObject<List<Course>>(json);
            return Courses;
        }

        private static List<Class> LoadGuidedReading1Classes()
        {
            string json = File.ReadAllText(@"Data\GuidedReading1Classes.json");
            GuidedReading1Classes = JsonConvert.DeserializeObject<List<Class>>(json);
            return GuidedReading1Classes;
        }

        private static List<Fry> LoadFryWords()
        {
            string json = File.ReadAllText(@"Data\FryWords.json");
            FryWords = JsonConvert.DeserializeObject<List<Fry>>(json);
            return FryWords;
        }

        private static List<Staff> LoadStaff()
        {
            string json = File.ReadAllText(@"Data\Staff.json");
            var staffs = JsonConvert.DeserializeObject<List<Staff>>(json);
            foreach (var staff in staffs)
            {
                staff.CenterId = 1;
            }

            Teachers = staffs.Where(s => s.Type == "Teacher").ToList();
            Operations = staffs.Where(s => s.Type == "Operations").ToList();

            return staffs;
        }

        private static List<Student> LoadStudents()
        {
            string json = File.ReadAllText(@"Data\Students.json");
            Students = JsonConvert.DeserializeObject<List<Student>>(json);

            //just for load testing
            //while (Students.Count() < 1000)
            //{
            //Students.AddRange(JsonConvert.DeserializeObject<List<Student>>(json));
            //}
            return Students;
        }

        private static List<PhonicsScreenerResult> LoadPhonicsScreenerResults()
        {
            var phonicsResults = new List<PhonicsScreenerResult>();
            foreach (var student in Students)
            {
                var howManyAreAnswered = new Random().Next(0, PhonicsScreenerQuestions.Count());
                var randomTeacher = Teachers.PickRandom();
                for (int i = 0; i < PhonicsScreenerQuestions.Count(); i++)
                {
                    bool? correct = null;
                    if (i < howManyAreAnswered)
                    {
                        correct = new Random().Next(2) == 0;
                    }

                    phonicsResults.Add(new PhonicsScreenerResult
                    {
                        Assessor = randomTeacher.EnglishName,
                        IsCorrect = correct,
                        Order = PhonicsScreenerQuestions[i].Order,
                        PhonicsScreenerId = PhonicsScreenerQuestions[i].Id,
                        Prefix = PhonicsScreenerQuestions[i].Prefix,
                        ZH_Prefix = PhonicsScreenerQuestions[i].ZH_Prefix,
                        StudentId = student.Id,
                        Task = PhonicsScreenerQuestions[i].Task,
                        Test = PhonicsScreenerQuestions[i].Test,
                        UpdateDate = DateTime.Now,
                        UpdatedBy = randomTeacher.EnglishName,
                        CourseId = PhonicsScreenerQuestions[i].CourseId
                    });
                }
            }
            return phonicsResults;
        }

        private static List<OralScreenerResult> LoadOralScreenerResults()
        {
            var oralResults = new List<OralScreenerResult>();
            foreach (var student in Students)
            {
                var howManyAreAnswered = new Random().Next(0, OralScreenerQuestions.Count());
                var randomTeacher = Teachers.PickRandom();

                for (int i = 0; i < OralScreenerQuestions.Count(); i++)
                {
                    bool? correct = null;
                    if (i < howManyAreAnswered)
                    {
                        correct = new Random().Next(2) == 0;
                    }

                    oralResults.Add(new OralScreenerResult
                    {
                        Assessor = randomTeacher.EnglishName,
                        IsCorrect = correct,
                        Order = OralScreenerQuestions[i].Order,
                        OralScreenerId = OralScreenerQuestions[i].Id,
                        Question = OralScreenerQuestions[i].Question,
                        StudentId = student.Id,
                        UpdateDate = DateTime.Now,
                        UpdatedBy = randomTeacher.EnglishName,
                        Image = OralScreenerQuestions[i].Image,
                        ZH_Question = OralScreenerQuestions[i].ZH_Question
                    });
                }
            }

            return oralResults;
        }

        private static List<ComprehensionScreenerResult> LoadComprehensionScreenerResults()
        {
            var comprehensionResults = new List<ComprehensionScreenerResult>();
            foreach (var student in Students)
            {
                var howManyAreAnswered = new Random().Next(0, ComprehensionScreenerQuestions.Count());
                var randomTeacher = Teachers.PickRandom();

                for (int i = 0; i < ComprehensionScreenerQuestions.Count(); i++)
                {

                    bool? correct = null;
                    if (i < howManyAreAnswered)
                    {
                        correct = new Random().Next(2) == 0;
                    }

                    comprehensionResults.Add(new ComprehensionScreenerResult
                    {
                        Assessor = randomTeacher.EnglishName,
                        IsCorrect = correct,
                        Preface = ComprehensionScreenerQuestions[i].Preface,
                        SecondPreface = ComprehensionScreenerQuestions[i].SecondPreface,
                        Order = ComprehensionScreenerQuestions[i].Order,
                        ComprehensionScreenerId = ComprehensionScreenerQuestions[i].Id,
                        Question = ComprehensionScreenerQuestions[i].Question,
                        StudentId = student.Id,
                        UpdateDate = DateTime.Now,
                        UpdatedBy = randomTeacher.EnglishName,
                        Image = ComprehensionScreenerQuestions[i].Image,
                        SecondImage = ComprehensionScreenerQuestions[i].SecondImage,
                        Answers = ComprehensionScreenerQuestions[i].Answers,
                        CorrectAnswer = ComprehensionScreenerQuestions[i].CorrectAnswer
                    });
                }
            }

            return comprehensionResults;
        }

        private static List<PhonicsScreener> LoadPhonicsScreeners()
        {
            var json = File.ReadAllText(@"Data\PhonicsScreeners.json");
            PhonicsScreenerQuestions = JsonConvert.DeserializeObject<List<PhonicsScreener>>(json);
            return PhonicsScreenerQuestions;
        }

        private static List<PhonicsSkill> LoadPhonicsSkills()
        {
            var json = File.ReadAllText(@"Data\PhonicsSkills.json");
            PhonicsSkills = JsonConvert.DeserializeObject<List<PhonicsSkill>>(json);
            return PhonicsSkills;
        }

        private static List<OralScreener> LoadOralScreeners()
        {
            string json = File.ReadAllText(@"Data\OralScreeners.json");
            OralScreenerQuestions = JsonConvert.DeserializeObject<List<OralScreener>>(json);
            return OralScreenerQuestions;
        }

        private static List<ComprehensionScreener> LoadComprehensionScreeners()
        {
            string json = File.ReadAllText(@"Data\ComprehensionScreeners.json");
            ComprehensionScreenerQuestions = JsonConvert.DeserializeObject<List<ComprehensionScreener>>(json);
            return ComprehensionScreenerQuestions;
        }
    }
}
