using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SilverLeaf.Common.Models;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Screener.Admin.Models;
using SilverLeaf.Screener.Admin.Objects;
using System;
using System.Collections.Generic;

namespace SilverLeaf.Screener.Admin.Services
{
    public static class CacheService
    {
        // Fields
        private static string _loginUsername;
        private static DateTime? _userProfileCachedOn;
        private static User _user;
        private static StudentDTO _student;
        private static List<PhonicsScreenerViewModel> _incompletePhonicsScreenerQuestions;
        private static List<ComprehensionScreenerViewModel> _incompleteComprehensionScreenerQuestions;
        private static List<OralScreenerViewModel> _incompleteOralScreenerQuestions;
        private static string _currentLanguage = "EN";

        // Events
        public static event EventHandler DeviceChanged = delegate { };
        public static event EventHandler<int> AlertBadgeChanged = delegate { };

        // Cache Keys
        private const string UserNameCacheKey = "UserNameCacheKey";
        private const string UserKey = "UserKey";
        private const string UserProfileCachedOnKey = "UserProfileCachedOn";
        private const string StudentKey = "StudentKey";
        private const string CurrentLanguageKey = "CurrentLanguageKey";
        private const string PhonicsScreenerQuestionsKey = "PhonicsScreenerQuestionsKey";
        private const string ComprehensionScreenerQuestionsKey = "ComprehensionScreenerQuestionsKey";
        private const string OralScreenerQuestionsKey = "OralScreenerQuestionsKey";

        // Properties
        public static string LoginUserName
        {
            get => _loginUsername ?? (_loginUsername = GetValueOrDefault<string>(UserNameCacheKey) ?? "hangzhou");
            set => SetAndStoreProperty(ref _loginUsername, UserNameCacheKey, value);
        }

        public static User User
        {
            get => _user ?? (_user = GetValueOrDefault<User>(UserKey));
            set
            {
                SetAndStoreProperty(ref _user, UserKey, value);
                UserProfileCachedOn = DateTime.UtcNow;
            }
        }

        public static string CurrentLanguage
        {
            get => _currentLanguage ?? (_currentLanguage = GetValueOrDefault<string>(CurrentLanguageKey) ?? "EN");
            set
            {
                SetAndStoreProperty(ref _currentLanguage, CurrentLanguageKey, value);
            }
        }

        public static StudentDTO Student
        {
            get => _student ?? (_student = GetValueOrDefault<StudentDTO>(StudentKey));
            set
            {
                SetAndStoreProperty(ref _student, StudentKey, value);
                UserProfileCachedOn = DateTime.UtcNow;
            }
        }


        public static AdjustableDTO<StudentDTO> PendingScreenerStudents;
        public static List<PhonicsScreenerViewModel> IncompletePhonicsScreenerQuestions
        {
            get => _incompletePhonicsScreenerQuestions ?? (_incompletePhonicsScreenerQuestions = GetValueOrDefault<List<PhonicsScreenerViewModel>>(PhonicsScreenerQuestionsKey));
            set
            {
                SetAndStoreProperty(ref _incompletePhonicsScreenerQuestions, PhonicsScreenerQuestionsKey, value);
                UserProfileCachedOn = DateTime.UtcNow;
            }
        }

        public static List<ComprehensionScreenerViewModel> IncompleteComprehensionScreenerQuestions
        {
            get => _incompleteComprehensionScreenerQuestions ?? (_incompleteComprehensionScreenerQuestions = GetValueOrDefault<List<ComprehensionScreenerViewModel>>(ComprehensionScreenerQuestionsKey));
            set
            {
                SetAndStoreProperty(ref _incompleteComprehensionScreenerQuestions, ComprehensionScreenerQuestionsKey, value);
                UserProfileCachedOn = DateTime.UtcNow;
            }
        }

        public static List<OralScreenerViewModel> IncompleteOralScreenerQuestions
        {
            get => _incompleteOralScreenerQuestions ?? (_incompleteOralScreenerQuestions = GetValueOrDefault<List<OralScreenerViewModel>>(OralScreenerQuestionsKey));
            set
            {
                SetAndStoreProperty(ref _incompleteOralScreenerQuestions, OralScreenerQuestionsKey, value);
                UserProfileCachedOn = DateTime.UtcNow;
            }
        }



        public static DateTime? UserProfileCachedOn
        {
            get => _userProfileCachedOn ?? (_userProfileCachedOn =
                       GetValueOrDefault(UserProfileCachedOnKey, DateTime.UtcNow));

            private set => SetAndStoreProperty(ref _userProfileCachedOn, UserProfileCachedOnKey, value);
        }


        private static ISettings Current => CrossSettings.Current;

        // Methods
        public static T GetValueOrDefault<T>(string key, T defaultValue = default(T))
        {
            var value = Current.GetValueOrDefault(key, string.Empty);

            // To maintain backwards compatibility, handle primitives and strings separately from json objects,
            // and then store them as json objects
            if (string.IsNullOrEmpty(value))
            {
                AddOrUpdateValue(key, defaultValue);
                return GetValueOrDefault(key, defaultValue);
            }

            if (!value.IsValidJson())
            {
                AddOrUpdateValue(key, value);
                return GetValueOrDefault(key, defaultValue);
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public static bool AddOrUpdateValue<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            return Current.AddOrUpdateValue(key, json);
        }

        public static void Remove(string key)
        {
            Current.Remove(key);
        }

        public static void Clear()
        {
            Current.Clear();
        }

        public static bool Contains(string key)
        {
            return Current.Contains(key);
        }

        /// <summary>
        /// Remove any cached, user-specific settings and
        /// reset the corresponding properties
        /// </summary>
        public static void RemoveUserSettings()
        {
            User = null;
            Remove(UserNameCacheKey);

            UserProfileCachedOn = null;
            Remove(UserProfileCachedOnKey);
        }

        public static void RemoveStudentSettings()
        {
            Student = null;
        }

        private static bool IsValidJson(this string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        /// <summary>
        /// Set the property backing store and store the value in cache
        /// </summary>
        /// <typeparam name="T">The Type of the property</typeparam>
        /// <param name="backingStore">The property's backing store</param>
        /// <param name="key">The cache key</param>
        /// <param name="value">The value</param>
        private static void SetAndStoreProperty<T>(ref T backingStore, string key, T value)
        {
            backingStore = value;
            AddOrUpdateValue(key, value);
        }
    }
}
