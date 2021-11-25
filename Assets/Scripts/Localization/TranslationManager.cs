using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UI;
using Utils;

namespace Localization
{
    public class TranslationManager
    {
        public static TranslationManager Instance { get; private set; }
        public static Action OnLanguageChanged { get; set; }

        private readonly Dictionary<string, Dictionary<string, string>> _translations;

        private Dictionary<string, string> _selectedTranslations;
        private string _selectedLanguage = "en";

        public TranslationManager(string json)
        {
            Instance = this;
            try
            {
                _translations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Couldn't deserialize: {json} Caught exception: {ex}");
            }
        }
        
        public void SelectLanguage(string language)
        {
            if (_translations.ContainsKey(language))
            {
                _selectedLanguage = language;
                _selectedTranslations = _translations[_selectedLanguage];
                OnLanguageChanged?.Invoke();
            }
            else
            {
                Logger.LogError($"Language {language} does not exist");
            }
        }

        public string GetTranslation(string id)
        {
            if (_selectedTranslations.ContainsKey(id))
            {
                return _selectedTranslations[id];
            }

            Logger.LogError($"Id {id} not found in translations");
            return "";
        }
    }
    
}