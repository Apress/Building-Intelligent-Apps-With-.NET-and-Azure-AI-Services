using Azure.AI.Translation.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter3_LanguageTranslator.Business
{
    public class TranslatorBusiness : ITranslatorBusiness
    {
        private readonly TextTranslationClient _textTranslationClient;
        public TranslatorBusiness(TextTranslationClient textTranslationClient)
        {
            _textTranslationClient = textTranslationClient;
        }
        public async Task<string> TranslateMessageAsync(string message, string targetLanguage)
        {
            string languageCode = String.Empty;

            switch (targetLanguage.ToLower())
            {
                case "french":
                    languageCode = "fr";
                    break;
                case "spanish":
                    languageCode = "es";
                    break;
                case "hindi":
                    languageCode = "hi";
                    break;
                case "german":
                    languageCode = "de";
                    break;
                default:
                    throw new Exception(message: "Invalid target language");
            }

            var result = await _textTranslationClient.TranslateAsync(languageCode, message);

            IReadOnlyList<TranslatedTextItem> translations = result.Value;
            TranslatedTextItem translation = translations.FirstOrDefault();

            return translation.Translations.FirstOrDefault().Text;
        }
    }
}
