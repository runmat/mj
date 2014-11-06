using System;
using System.Reflection;
using System.Reflection.Emit;

namespace GeneralTools.Services
{
    public static class DynamicTypeBuilder
    {
        public static TypeBuilder CreateTypeBuilder(string assemblyName, string moduleName, string typeName)
        {
            var typeBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName)
                .DefineType(typeName, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        public static void CreateAutoImplementedProperty(TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string privateFieldPrefix = "m_";
            const string getterPrefix = "get_";
            const string setterPrefix = "set_";

            // Generate the field.
            var fieldBuilder = builder.DefineField(string.Concat(privateFieldPrefix, propertyName), propertyType, FieldAttributes.Private);

            // Generate the property
            var propertyBuilder = builder.DefineProperty(propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType, null);

            // Property getter and setter attributes.
            var propertyMethodAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define the getter method.
            var getterMethod = builder.DefineMethod(
                string.Concat(getterPrefix, propertyName),
                propertyMethodAttributes, propertyType, Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            var getterIlCode = getterMethod.GetILGenerator();
            getterIlCode.Emit(OpCodes.Ldarg_0);
            getterIlCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterIlCode.Emit(OpCodes.Ret);

            // Define the setter method.
            var setterMethod = builder.DefineMethod(string.Concat(setterPrefix, propertyName), propertyMethodAttributes, null, new[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            var setterIlCode = setterMethod.GetILGenerator();
            setterIlCode.Emit(OpCodes.Ldarg_0);
            setterIlCode.Emit(OpCodes.Ldarg_1);
            setterIlCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterIlCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }
    }
}
