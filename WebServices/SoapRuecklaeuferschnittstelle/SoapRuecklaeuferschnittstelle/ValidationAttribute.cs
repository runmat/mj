//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Services.Protocols;

//namespace SoapRuecklaeuferschnittstelle
//{
//    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)] 
//    public class ValidationAttribute : SoapExtensionAttribute 
//    { int priority = 0; 
//    // used by soap extension to get the type 
//    // of object to be created 
//    public override System.Type ExtensionType { get { return typeof(ValidationExtension); } } 
//    public override int Priority { get { return priority; } set { priority = value; } } }
//} 