using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("TranslatedResourceCustom")]
    public class TranslatedResourceCustom : IValidatableObject
    {
        [Column(Order = 0), Key]
        public string Resource { get; set; }

        [Column(Order = 1), Key]
        public int CustomerID { get; set; }


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

CREATE TABLE dbo.TranslatedResourceCustom
	(
	Resource nvarchar(50) NOT NULL,
	CustomerID int NOT NULL,
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

ALTER TABLE dbo.TranslatedResourceCustom ADD CONSTRAINT
	PK_TranslatedResourceCustom PRIMARY KEY CLUSTERED 
	(
	Resource,
	CustomerID 
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


ALTER TABLE dbo.TranslatedResourceCustom ADD CONSTRAINT
	FK_TranslatedResourceCustom_TranslatedResource FOREIGN KEY
	(
	Resource
	) REFERENCES dbo.TranslatedResource
	(
	Resource
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
ALTER TABLE dbo.TranslatedResourceCustom SET (LOCK_ESCALATION = TABLE)

COMMIT

*/