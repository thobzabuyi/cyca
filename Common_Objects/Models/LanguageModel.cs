using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class LanguageModel
    {
        public Language GetSpecificLanguage(int languageId)
        {
            Language language;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var languageList = (from r in dbContext.Languages
                                    where r.Language_Id.Equals(languageId)
                                    select r).ToList();

                language = (from r in languageList
                            select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return language;
        }

        public List<Language> GetListOfLanguages()
        {
            List<Language> languages;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var languageList = (from r in dbContext.Languages
                                        select r).ToList();

                    languages = (from r in languageList
                                 select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return languages;
        }
    }
}
