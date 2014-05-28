using System;
using System.Collections.Generic;
using System.Reflection;
using GeneralTools.Models;

namespace CkgDomainLogic.CoC.Models
{
    public class CocMetaProperty
    {
        public string GroupName { get { return CocLayoutAttribute.Group; } }

        public string GroupFriendlyName { get { return GroupName.NotNullOrEmpty().StartsWith("~") ? "" : GroupName; } }

        public string GroupNameAsID { get { return GroupName.Replace('.', '_'); } }

        public string GroupLabel { get { return CocLayoutAttribute.GroupLabel; } }

        public bool IsCocOrderEditable { get { return CocLayoutAttribute.IsCocOrderEditable; } }

        public bool MultiLine { get { return CocLayoutAttribute.MultiLine; } }

        public int MultiLineRows { get { return CocLayoutAttribute.MultiLineRows; } }

        public string Name { get { return PropertyInfo.Name; } }

        public string Label { get { return CocLayoutAttribute.Label; } }

        public string Measure { get { return CocLayoutAttribute.Measure; } }

        public int MaxLength { get { return CocLayoutAttribute.MaxLen == 0 ? 999 : CocLayoutAttribute.MaxLen; } }

        public Type Type { get { return PropertyInfo.PropertyType; } }

        public bool IsBoolean { get { return Type == typeof(bool); } }

        public bool IsDateTime { get { return Type == typeof(DateTime?); } }

        public string WrapperCssClass { get { return IsDateTime ? "input-append datepicker" : ""; } }

        public string Format { get { return IsDateTime ? "{0:dd.MM.yyyy}" : ""; } }

        public string SelectOptions { get { return CocLayoutAttribute.SelectOptions; } }

        public bool SelectOptionsAvailable { get { return CocLayoutAttribute.SelectOptions.IsNotNullOrEmpty(); } }

        public List<CocMetaProperty> AllPropertiesOfThisGroup { get { return GroupPropertiesFunc(GroupName); } }

        
        #region private get
        
        public Func<string, List<CocMetaProperty>> GroupPropertiesFunc { private get; set; }

        public CocLayoutAttribute CocLayoutAttribute { private get; set; }

        public PropertyInfo PropertyInfo { private get; set; }

        #endregion
    }
}
