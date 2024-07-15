using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter3_LanguageTranslator.Business
{
    public interface ITranslatorBusiness
    {
        Task<string> TranslateMessageAsync(string message, string targetLanguage);
    }
}
