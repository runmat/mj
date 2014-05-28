using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneralTools.Models;

namespace GeneralTools.UnitTests.ModelMappings
{
    public class AppModelMappings : Models.ModelMappings
    {
        static public ModelMapping<AdvancedTestClass1, AdvancedTestClass2> MapAdvancedClasses
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<AdvancedTestClass1, AdvancedTestClass2>(
                    new Dictionary<string, string> {
                        { "IntProperty", "IntPropertyDifferentName" },
                        { "BoolProperty", "BoolPropertyDifferentName" },
                        { "StringProperty", "StringPropertyDifferentName" },
                        { "DateTimeProperty", "DateTimePropertyDifferentName" },

                        { "StringPropertyCopyChangedByAppCode", "StringPropertyCopyChangedByAppCode" },
                        { "StringPropertyCopyChangedByUserCode", "StringPropertyCopyChangedByUserCode" },
                        { "StringPropertyCopyBackChangedByAppCode", "StringPropertyCopyBackChangedByAppCode" },
                        { "StringPropertyCopyBackChangedByUserCode", "StringPropertyCopyBackChangedByUserCode" },
                    },
                    // AppCode Init Copy:
                    (source, destination) =>
                        {
                            destination.StringPropertyCopyChangedByAppCode = "changed by Copy AppCode";
                        },
                    // AppCode Init CopyBack:
                    (source, destination) =>
                        {
                            destination.StringPropertyCopyBackChangedByAppCode = "changed by CopyBack AppCode";
                        } 
                ));
            }
        }
    }
}
