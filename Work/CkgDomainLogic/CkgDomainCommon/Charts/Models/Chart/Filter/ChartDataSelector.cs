using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Linq;
using GeneralTools.Models;

namespace CkgDomainLogic.Charts.Models
{
    public class ChartDataSelector : IValidatableObject
    {
        public string ChartID { get; set; }

        public string ChartGroup { get; set; }

        
        int[] _jahrItems = new int[0];
        [XmlIgnore]
        public int[] JahrItems
        {
            get { return _jahrItems; } 
            set
            {
                _jahrItems = value; 
                JahrBools = new bool[value.Length];
                for (var i = 0; i < JahrBools.Length; i++)
                    JahrBools[i] = true;
            }
        }

        public bool[] JahrBools { get; set; }

        [XmlIgnore]
        public int[] SelectedJahrItems { get { return JahrItems.Where(i => JahrBools[Array.IndexOf(JahrItems, i)]).ToArray(); } }


        string[] _key1Items = new string[0];
        [XmlIgnore]
        public string[] Key1Items 
        {
            get { return _key1Items; } 
            set
            {
                _key1Items = value;
                Key1Bools = new bool[value.Length];
                for (var i = 0; i < Key1Bools.Length; i++)
                    Key1Bools[i] = true;
            }
        }
        
        public bool[] Key1Bools { get; set; }

        [XmlIgnore]
        public string[] SelectedKey1Items { get { return Key1Items.Where(i => Key1Bools[Array.IndexOf(Key1Items, i)]).ToArray(); } }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (JahrBools.None(b => b))
                yield return new ValidationResult("Bitte wählen Sie mindestens ein Jahr!");

            if (Key1Bools.None(b => b))
                yield return new ValidationResult("Bitte wählen Sie mindestens eine Gruppe!");
        }

        public void Apply(ChartDataSelector selector)
        {
            JahrBools = selector.JahrBools;
            Key1Bools = selector.Key1Bools;
        }
    }
}
