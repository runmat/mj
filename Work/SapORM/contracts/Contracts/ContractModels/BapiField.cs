using System;
using GeneralTools.Models;

namespace SapORM.Contracts
{
    [Serializable]
    public class BapiField
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public bool IsNullable { get { return (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(Nullable<>)); } }

        public string TypeAsString
        {
            get
            {
                var typeName = (IsNullable ? string.Format("{0}?", Type.GetGenericArguments()[0].Name) : Type.Name);

                return FormatTypeName(typeName);
            }
        }

        public int Length { get; set; }

        public int Decimals { get; set; }

        private static string FormatTypeName(string typeName)
        {
            return typeName.NotNullOrEmpty()
                            .Replace("Int32", "int")
                            .Replace("Decimal", "decimal")
                            .Replace("Byte", "byte")
                            .Replace("Double", "double")
                            .Replace("String", "string");
        }
    }
}
