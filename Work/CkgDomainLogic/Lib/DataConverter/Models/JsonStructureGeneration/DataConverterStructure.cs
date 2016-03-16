using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CkgDomainLogic.DataConverter.Models
{
    public class DataConverterStructure : Dictionary<string, List<string>>
    {
        public DataConverterStructure()
        {
        }

        public DataConverterStructure(Type sourceType)
        {
            GenerateStructure(sourceType);
        }

        public DataConverterStructure(object sourceObject)
        {
            GenerateStructure(sourceObject);
        }

        private void GenerateStructure(Type sourceType)
        {
            if (sourceType != null)
            {
                Add("", new List<string>());

                var properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite).ToList();

                foreach (var property in properties)
                {
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType == typeof(string))
                    {
                        this[""].Add(property.Name);
                    }
                    else if (property.PropertyType.IsClass)
                    {
                        var classProperties = property.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite).ToList();

                        if (classProperties.Any())
                            Add(property.Name, new List<string>());

                        foreach (var classProperty in classProperties)
                        {
                            if (classProperty.PropertyType.IsValueType || classProperty.PropertyType.IsEnum || classProperty.PropertyType == typeof(string))
                            {
                                this[property.Name].Add(classProperty.Name);
                            }
                        }
                    }
                }
            }
        }

        private void GenerateStructure(object sourceObject)
        {
            if (sourceObject != null)
                GenerateStructure(sourceObject.GetType());
        }
    }
}
