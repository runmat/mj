using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace GeneralTools.Services
{
    /// <summary>
    /// A Utility class used to merge the properties of
    /// heterogenious objects
    /// </summary>
    public class TypeMerger
    {
        //assembly/module builders
        private static AssemblyBuilder _asmBuilder;
        private static ModuleBuilder _modBuilder;

        //object type cache
        private static readonly IDictionary<String, Type> AnonymousTypes = new Dictionary<String, Type>();

        //used for thread-safe access to Type Dictionary
        private static readonly Object SyncLock = new Object();


        private static string FormatHtmlDictionaryKey(string key)
        {
            return key.Replace("_", "-");
        }

        public static IDictionary<string, object> ToHtmlDictionary(Object o)
        {
            if (o == null)
                return new Dictionary<string, object>();

            var destinationDict = new Dictionary<string, object>();
            var dictObject = o as IDictionary<string, object>;
            if (dictObject != null)
            {
                foreach (var key in dictObject.Keys)
                {
                    object val;
                    if (dictObject.TryGetValue(key, out val))
                        destinationDict.Add(FormatHtmlDictionaryKey(key), val);
                }
                return destinationDict;
            }

            foreach (var property in o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var val = property.GetValue(o, null);
                destinationDict.Add(FormatHtmlDictionaryKey(property.Name), val);
            }
            return destinationDict;
        }

        /// <summary>
        /// Merge all property values of object 1 with the values of the same properties of object 2 - 
        ///     if both property 1 and property 2 do not exist => skip merging;
        ///     if property 1 does not exist => set to value of property 2;
        ///     if property 2 does not exist => set to value of property 1;
        ///     if property 1 and property 2 both exist => merge concatenation of both values (value1 + " " + value2);
        /// </summary>
        /// <param name="values1"></param>
        /// <param name="values2"></param>
        /// <param name="mergeSeparator">separator string used to concat the same property values of object 1 + 2</param>
        /// <returns></returns>
        public static IDictionary<string, object> StrictlyMergeProperties(Object values1, Object values2, string mergeSeparator = " ")
        {
            Type type1 = null;
            if (values1 != null)
                type1 = values1.GetType();
            var type2 = values2.GetType();

            var dict = new Dictionary<string, object>();

            foreach (var property2 in type2.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                PropertyInfo property1 = null;

                if (type1 != null)
                    property1 = type1.GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p1 => p1.Name.ToLower() == property2.Name.ToLower());

                var val2 = property2.GetValue(values2, null);
                if (property1 == null)
                {
                    dict.Add(property2.Name, val2);
                    continue;
                }

                var val1 = property1.GetValue(values1, null);

                val1 = val1 == null ? val2 : val1 + mergeSeparator + val2;

                dict.Add(property1.Name, val1);
            }

            if (type1 == null)
                return dict;

            foreach (var property1 in type1.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (dict.ContainsKey(property1.Name))
                    continue;

                var val1 = property1.GetValue(values1, null);
                dict.Add(property1.Name, val1);
            }

            return dict;
        }

        /// <summary>
        /// Replace all property values of object 1 with the value of the same properties of object 2 - 
        /// </summary>
        /// <param name="values1"></param>
        /// <param name="values2"></param>
        /// <param name="replaceOnlyIfExists">true => Replace only if property exists in object 1; false => set property value even if property does not exist in object 1</param>
        /// <returns></returns>
        public static IDictionary<string, object> StrictlyReplaceProperties(Object values1, Object values2, bool replaceOnlyIfExists)
        {
            Type type1 = null;
            if (values1 != null)
                type1 = values1.GetType();
            var type2 = values2.GetType();

            var dict = new Dictionary<string, object>();

            foreach (var property2 in type2.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                PropertyInfo property1 = null;

                if (type1 != null)
                    property1 = type1.GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p1 => p1.Name.ToLower() == property2.Name.ToLower());

                var val2 = property2.GetValue(values2, null);
                if (property1 == null)
                {
                    if (!replaceOnlyIfExists)
                        dict.Add(property2.Name, val2);

                    continue;
                }

                var val1 = property1.GetValue(values1, null);

                if (val2 != null)
                    val1 = val2;

                if (val1 != null)
                    dict.Add(property1.Name, val1);
            }

            if (type1 == null)
                return dict;

            foreach (var property1 in type1.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (dict.ContainsKey(property1.Name))
                    continue;

                var val1 = property1.GetValue(values1, null);
                dict.Add(property1.Name, val1);
            }

            return dict;
        }


        /// <summary>
        /// Merge two different object instances into a single
        /// </summary>
        public static Object MergeTypes(Object values1, Object values2)
        {
            //StrictlyMergeProperties(values1, values2);

            if (values1 == null) values1 = new object();
            if (values2 == null) values2 = new object();

            //create a name from the names of both Types
            var name = String.Format("{0}_{1}", values1.GetType(), values2.GetType());
            var name2 = String.Format("{0}_{1}", values2.GetType(), values1.GetType());

            var newValues = CreateInstance(name, values1, values2);
            if (newValues != null)
                return newValues;

            newValues = CreateInstance(name2, values2, values1);
            if (newValues != null)
                return newValues;

            //lock for thread safe writing
            lock (SyncLock)
            {
                //now that we're inside the lock - check one more time
                newValues = CreateInstance(name, values1, values2);
                if (newValues != null)
                    return newValues;

                //merge list of PropertyDescriptors for both objects
                var pdc = GetProperties(values1, values2);

                //make sure static properties are properly initialized
                InitializeAssembly();

                //create the type definition
                var newType = CreateType(name, pdc);

                //add it to the cache
                AnonymousTypes.Add(name, newType);

                //return an instance of the new Type
                return CreateInstance(name, values1, values2);
            }
        }

        /// <summary>
        /// Instantiates an instance of an existing Type from cache
        /// </summary>
        private static Object CreateInstance(String name, Object values1, Object values2)
        {
            Object newValues = null;

            //merge all values together into an array
            var allValues = MergeValues(values1, values2);

            //check to see if type exists
            if (AnonymousTypes.ContainsKey(name))
            {
                //get type
                var type = AnonymousTypes[name];

                //make sure it isn't null for some reason
                if (type != null)
                {
                    //create a new instance
                    newValues = Activator.CreateInstance(type, allValues);
                }
                else
                {
                    //remove null type entry
                    lock (SyncLock)
                        AnonymousTypes.Remove(name);
                }
            }

            //return values (if any)
            return newValues;
        }

        /// <summary>
        /// Merge PropertyDescriptors for both objects
        /// </summary>
        private static PropertyDescriptor[] GetProperties(Object values1,
            Object values2)
        {
            //dynamic list to hold merged list of properties
            var properties = new List<PropertyDescriptor>();

            //get the properties from both objects
            var pdc1 = TypeDescriptor.GetProperties(values1);
            var pdc2 = TypeDescriptor.GetProperties(values2);

            //add properties from values1
            for (var i = 0; i < pdc1.Count; i++)
                properties.Add(pdc1[i]);

            //add properties from values2
            for (var i = 0; i < pdc2.Count; i++)
                properties.Add(pdc2[i]);

            //return array
            return properties.ToArray();
        }

        /// <summary>
        /// Get the type of each property
        /// </summary>
        private static Type[] GetTypes(PropertyDescriptor[] pdc)
        {
            var types = new List<Type>();

            for (var i = 0; i < pdc.Length; i++)
                types.Add(pdc[i].PropertyType);

            return types.ToArray();
        }

        /// <summary>
        /// Merge the values of the two types into an object array
        /// </summary>
        private static Object[] MergeValues(Object values1,
            Object values2)
        {
            var pdc1 = TypeDescriptor.GetProperties(values1);
            var pdc2 = TypeDescriptor.GetProperties(values2);

            var values = new List<Object>();
            for (var i = 0; i < pdc1.Count; i++)
                values.Add(pdc1[i].GetValue(values1));

            for (var i = 0; i < pdc2.Count; i++)
                values.Add(pdc2[i].GetValue(values2));

            return values.ToArray();
        }

        /// <summary>
        /// Initialize static objects
        /// </summary>
        private static void InitializeAssembly()
        {
            //check to see if we've already instantiated
            //the static objects
            if (_asmBuilder == null)
            {
                //create a new dynamic assembly
                var assembly = new AssemblyName {Name = "AnonymousTypeExentions"};

                //get the current application domain
                var domain = Thread.GetDomain();

                //get a module builder object
                _asmBuilder = domain.DefineDynamicAssembly(assembly,
                    AssemblyBuilderAccess.Run);
                _modBuilder = _asmBuilder.DefineDynamicModule(
                    _asmBuilder.GetName().Name, false
                    );
            }
        }

        /// <summary>
        /// Create a new Type definition from the list
        /// of PropertyDescriptors
        /// </summary>
        private static Type CreateType(String name,
            PropertyDescriptor[] pdc)
        {
            //create TypeBuilder
            var typeBuilder = CreateTypeBuilder(name);

            //get list of types for ctor definition
            var types = GetTypes(pdc);

            //create priate fields for use w/in the ctor body and properties
            var fields = BuildFields(typeBuilder, pdc);

            //define/emit the Ctor
            BuildCtor(typeBuilder, fields, types);

            //define/emit the properties
            BuildProperties(typeBuilder, fields);

            //return Type definition
            return typeBuilder.CreateType();
        }

        /// <summary>
        /// Create a type builder with the specified name
        /// </summary>
        private static TypeBuilder CreateTypeBuilder(string typeName)
        {
            //define class attributes
            var typeBuilder = _modBuilder.DefineType(typeName,
                        TypeAttributes.Public |
                        TypeAttributes.Class |
                        TypeAttributes.AutoClass |
                        TypeAttributes.AnsiClass |
                        TypeAttributes.BeforeFieldInit |
                        TypeAttributes.AutoLayout,
                        typeof(object));

            //return new type builder
            return typeBuilder;
        }

        /// <summary>
        /// Define/emit the ctor and ctor body
        /// </summary>
        private static void BuildCtor(TypeBuilder typeBuilder,
            FieldBuilder[] fields, Type[] types)
        {
            //define ctor()
            var ctor = typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                types
                );

            //build ctor()
            var ctorGen = ctor.GetILGenerator();

            //create ctor that will assign to private fields
            for (var i = 0; i < fields.Length; i++)
            {
                //load argument (parameter)
                ctorGen.Emit(OpCodes.Ldarg_0);
                ctorGen.Emit(OpCodes.Ldarg, (i + 1));

                //store argument in field
                ctorGen.Emit(OpCodes.Stfld, fields[i]);
            }

            //return from ctor()
            ctorGen.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Define fields based on the list of PropertyDescriptors
        /// </summary>
        private static FieldBuilder[] BuildFields(TypeBuilder typeBuilder, PropertyDescriptor[] pdc)
        {
            var fields = new List<FieldBuilder>();

            //build/define fields
            for (var i = 0; i < pdc.Length; i++)
            {
                var pd = pdc[i];

                //define field as '_[Name]' with the object's Type
                var field = typeBuilder.DefineField(
                    String.Format("_{0}", pd.Name),
                    pd.PropertyType,
                    FieldAttributes.Private
                    );

                //add to list of FieldBuilder objects
                fields.Add(field);
            }

            return fields.ToArray();
        }

        /// <summary>
        /// Build a list of Properties to match the list of private fields
        /// </summary>
        private static void BuildProperties(TypeBuilder typeBuilder, FieldBuilder[] fields)
        {
            //build properties
            for (var i = 0; i < fields.Length; i++)
            {
                //remove '_' from name for public property name
                var propertyName = fields[i].Name.Substring(1);

                //define the property
                var property = typeBuilder.DefineProperty(propertyName,
                    PropertyAttributes.None, fields[i].FieldType, null);

                //define 'Get' method only (anonymous types are read-only)
                var getMethod = typeBuilder.DefineMethod(
                    String.Format("Get_{0}", propertyName),
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                    fields[i].FieldType,
                    Type.EmptyTypes
                    );

                //build 'Get' method
                var methGen = getMethod.GetILGenerator();

                //method body
                methGen.Emit(OpCodes.Ldarg_0);
                //load value of corresponding field
                methGen.Emit(OpCodes.Ldfld, fields[i]);
                //return from 'Get' method
                methGen.Emit(OpCodes.Ret);

                //assign method to property 'Get'
                property.SetGetMethod(getMethod);
            }
        }
    }
}
