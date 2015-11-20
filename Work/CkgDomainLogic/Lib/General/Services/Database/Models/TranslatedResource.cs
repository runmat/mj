using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("TranslatedResource")]
    public class TranslatedResource : IValidatableObject
    {
        private const string Kurz = "_kurz";

        [Key]
        public string Resource { get; set; }

        public string Format { get; set; }

        [RequiredConditional]
        public string en { get; set; }
        public string en_kurz { get; set; }

        [RequiredConditional]
        public string de { get; set; }
        public string de_kurz { get; set; }

        public string de_de { get; set; }
        public string de_de_kurz { get; set; }

        public string de_ch { get; set; }
        public string de_ch_kurz { get; set; }

        public string de_at { get; set; }
        public string de_at_kurz { get; set; }

        public string fr { get; set; }
        public string fr_kurz { get; set; }


        public void MergeTranslatedResourceCustom(TranslatedResourceCustom translatedResourceCustom)
        {
            if (!string.IsNullOrEmpty(translatedResourceCustom.en))
            {
                en = translatedResourceCustom.en;    
            }

            if (!string.IsNullOrEmpty(translatedResourceCustom.en_kurz))
            {
                en_kurz = translatedResourceCustom.en_kurz;    
            }

            if (!string.IsNullOrEmpty(translatedResourceCustom.de))
            {
                de = translatedResourceCustom.de;    
            }

            if (!string.IsNullOrEmpty(translatedResourceCustom.de_kurz))
            {
                de_kurz = translatedResourceCustom.de_kurz;    
            }

            if (!string.IsNullOrEmpty(translatedResourceCustom.de_at))
            {
                de_at = translatedResourceCustom.de_at;    
            }

            if (!string.IsNullOrEmpty(translatedResourceCustom.de_at_kurz))
            {
                de_at_kurz = translatedResourceCustom.de_at_kurz;    
            }

            if (!string.IsNullOrEmpty(translatedResourceCustom.de_ch))
            {
                de_ch = translatedResourceCustom.de_ch;    
            }

            if (!string.IsNullOrEmpty(translatedResourceCustom.de_ch_kurz))
            {
                de_ch_kurz = translatedResourceCustom.de_ch_kurz;    
            }

            if (!string.IsNullOrEmpty(translatedResourceCustom.de_de))
            {
                de_de = translatedResourceCustom.de_de;    
            }

            if (!string.IsNullOrEmpty(translatedResourceCustom.de_de_kurz))
            {
                de_de_kurz = translatedResourceCustom.de_de_kurz;    
            }

            if (!string.IsNullOrEmpty(translatedResourceCustom.fr))
            {
                fr = translatedResourceCustom.fr;
            }

            if (!string.IsNullOrEmpty(translatedResourceCustom.fr_kurz))
            {
                fr_kurz = translatedResourceCustom.fr_kurz;
            }
        }

        /// <summary>
        /// Anhand der aktuellen UiCulture ermittle die passende Übersetzung aus dieser Resource
        /// </summary>
        /// <returns></returns>
        public string GetTranslation()
        {
            // Zu erst versuche die exakte Sprache-Kultur Kombination zu finden
            var culture = Thread.CurrentThread.CurrentUICulture.Name.ToLower();

            if (culture.Contains("-"))
            {
                culture = culture.Replace("-", "_");
            }

            var propertyInfo = typeof(TranslatedResource).GetProperty(culture);

            if (propertyInfo != null && propertyInfo.GetValue(this, null) != null && string.IsNullOrEmpty(propertyInfo.GetValue(this, null).ToString()) == false)
            {
                return propertyInfo.GetValue(this, null).ToString();
            }


            // Versuche die Sprache falls die Kombination Sprache-Kultur nicht zu finden ist
            var parts = culture.Split("_".ToCharArray()[0]);
            if (parts.Length == 2)
            {
                propertyInfo = typeof(TranslatedResource).GetProperty(parts[0]);

                if (propertyInfo != null && propertyInfo.GetValue(this, null) != null && string.IsNullOrEmpty(propertyInfo.GetValue(this, null).ToString()) == false)
                {
                    return propertyInfo.GetValue(this, null).ToString();
                }
            }

            // Weder die Sprache-Kultur noch die Sprache habe ich finden können, Default zurückgeben
            return de;
        }

        /// <summary>
        /// Anhand der aktuellen UiCulture ermittle die passende Kurz-Übersetzung aus dieser Resource
        /// Zu erst werden die Kurz Übersetzungen ermittelt, falls kein Wert ermittelt wurde, wird die normale Übersetzung genommen
        /// </summary>
        public string GetTranslationKurz()
        {
            // Zu erst versuche die exakte Sprache-Kultur Kombination zu finden
            var culture = Thread.CurrentThread.CurrentUICulture.Name.ToLower();

            if (culture.Contains("-"))
            {
                culture = culture.Replace("-", "_");
            }

            //fr-FR_Kurz
            var propertyInfo = typeof(TranslatedResource).GetProperty(string.Concat(culture, Kurz));

            // Property vorhanden && Hat einen Wert
            if (propertyInfo != null && propertyInfo.GetValue(this, null) != null && string.IsNullOrEmpty(propertyInfo.GetValue(this, null).ToString()) == false)
            {
                // Diesen Wert verwenden
                return propertyInfo.GetValue(this, null).ToString();
            }

            //fr_Kurz
            var parts = culture.Split("_".ToCharArray()[0]);
            if (parts.Length == 2)
            {
                propertyInfo = typeof(TranslatedResource).GetProperty(string.Concat(parts[0],Kurz));

                // Property vorhanden && Hat einen Wert
                if (propertyInfo != null && propertyInfo.GetValue(this, null) != null && string.IsNullOrEmpty(propertyInfo.GetValue(this, null).ToString()) == false)
                {
                    return propertyInfo.GetValue(this, null).ToString();
                }
            }

            // fr -> Ich kann keine kurze Bezeichnung finden, daher die "normale" Bezeichnung aus der Sprache verwenden
            propertyInfo = typeof(TranslatedResource).GetProperty(parts[0]);
            if (propertyInfo != null && propertyInfo.GetValue(this, null) != null && string.IsNullOrEmpty(propertyInfo.GetValue(this, null).ToString()) == false)
            {
                // Diesen Wert verwenden
                return propertyInfo.GetValue(this, null).ToString();
            }

            // Weder die Sprache-Kultur noch die Sprache habe ich finden können, Default zurückgeben
            if (!string.IsNullOrEmpty(de_kurz))
            {
                return de_kurz;    
            }

            return GetTranslation();
        }

        private bool IsEmpty
        {
            get
            {
                return (de.IsNullOrEmpty() && de_kurz.IsNullOrEmpty() &&
                        en.IsNullOrEmpty() && en_kurz.IsNullOrEmpty() &&
                        fr.IsNullOrEmpty() && fr_kurz.IsNullOrEmpty());
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsEmpty && de.IsNullOrEmpty())
                yield return new ValidationResult(Localize.Required, new[] { "de" });

            if (!IsEmpty && en.IsNullOrEmpty())
                yield return new ValidationResult(Localize.Required, new[] { "en" });
        }
    }
}


/*

--Überprüfen Sie das Skript ausführlich, bevor Sie es außerhalb des Datenbank-Designer-Kontexts ausführen, um potenzielle Datenverluste zu vermeiden.
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION

CREATE TABLE dbo.TranslatedResource
	(
	Resource nvarchar(50) NOT NULL,
	Format nvarchar(50) NULL,
	en nvarchar(256) NOT NULL,
	en_kurz nvarchar(256) NULL,
    de nvarchar(256) NOT NULL,
    de_kurz nvarchar(256) NULL,
	de_de nvarchar(256) NULL,
	de_de_kurz nvarchar(256) NULL,
	de_at nvarchar(256) NULL,
	de_at_kurz nvarchar(256) NULL,
	de_ch nvarchar(256) NULL,
	de_ch_kurz nvarchar(256) NULL
	)  ON [PRIMARY]

ALTER TABLE dbo.TranslatedResource ADD CONSTRAINT
	PK_TranslatedResource PRIMARY KEY CLUSTERED 
	(
	Resource
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


ALTER TABLE dbo.TranslatedResource SET (LOCK_ESCALATION = TABLE)

COMMIT

*/

/*

// Code für den Export der Resx Einträge als SQL Insert Statements
void Main()
{
	const string insert = "INSERT INTO [dbo].[TranslatedResource] ([Resource], [Format], [en], [de], [de_de], [de_at], [de_ch]) VALUES (N'{0}', NULL, N'{1}', N'{2}', N'{3}', N'{4}', N'{5}');";
	
	System.Text.StringBuilder sb = new System.Text.StringBuilder();
	var invariantResourceSet = CkgDomainLogic.Resources.DomainCommonResources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, true);
	var deutschResourceSet = CkgDomainLogic.Resources.DomainCommonResources.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo("de-de"), true, true);
	foreach (DictionaryEntry invariantEntry in invariantResourceSet)
	{
		var s = deutschResourceSet.GetString(invariantEntry.Key.ToString()); // de un de-de Wert
		if (string.IsNullOrEmpty(s))
		{
			sb.AppendLine(string.Format(insert, invariantEntry.Key.ToString(), invariantEntry.Value.ToString().Replace("'", "''"), "", "", "", ""));
			continue;
		}
		
		sb.AppendLine(string.Format(insert, invariantEntry.Key.ToString(), invariantEntry.Value.ToString().Replace("'", "''"), s, "", "", ""));
	}
	
	sb.ToString().Dump();
}

*/