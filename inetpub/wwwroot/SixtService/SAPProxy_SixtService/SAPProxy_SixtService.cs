
//------------------------------------------------------------------------------
// 
//     This code was generated by a SAP. NET Connector Proxy Generator Version 1.0
//     Created at 17.04.2008
//     Created from Windows 2000
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// 
//------------------------------------------------------------------------------
using System;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using SAP.Connector;

namespace SAPProxy_SixtService
{

  // Client SAP proxy class
  [WebServiceBinding(Name="", Namespace="urn:sap-com:document:sap:rfc:functions")]
  public class SAPProxy_SixtService : SAPClient
  {
    // Constructors
    public SAPProxy_SixtService(){}
    public SAPProxy_SixtService(string ConnectionString) : base(ConnectionString){}

  
    // Exception constants 
    public const string Error_Imp = "ERROR_IMP";
   
    public const string No_Data = "NO_DATA";
   
    public const string Transform_Error = "TRANSFORM_ERROR";
   
    // Client call Methods
    

    // Generated method used to call the remote function module Z_M_IMP_ZUL_AUFTR_1    
    // Throws:  Error_Imp 
    [RfcMethod(AbapName = "Z_M_IMP_ZUL_AUFTR_1")]
    [SoapDocumentMethodAttribute("http://tempuri.org/Z_M_IMP_ZUL_AUFTR_1",
     RequestNamespace = "urn:sap-com:document:sap:rfc:functions",
     RequestElementName = "Z_M_IMP_ZUL_AUFTR_1",
     ResponseNamespace = "urn:sap-com:document:sap:rfc:functions",
     ResponseElementName = "Z_M_IMP_ZUL_AUFTR_1.Response")]
    public void Z_M_Imp_Zul_Auftr_1 (

     [RfcParameter(AbapName = "I_KUNNR",RfcType=RFCTYPE.RFCTYPE_CHAR, Optional = false, Direction = RFCINOUT.IN, Length = 10, Length2 = 20)]
     [XmlElement("I_KUNNR", IsNullable=false)]
     string I_Kunnr,
     [RfcParameter(AbapName = "GT_WEB",RfcType=RFCTYPE.RFCTYPE_ITAB, Optional = true, Direction = RFCINOUT.INOUT)]
     [XmlArray("GT_WEB", IsNullable=false)]
     [XmlArrayItem("ZDAD_ZUL_AUFTR_1", IsNullable=false)]
     ref ZDAD_ZUL_AUFTR_1Table Gt_Web)
    {
        object[]results = null;
        results = this.SAPInvoke("Z_M_Imp_Zul_Auftr_1",new object[] {
                            I_Kunnr,Gt_Web });
        Gt_Web = (ZDAD_ZUL_AUFTR_1Table) results[0];

        return;
    }


    // Generated method used to call the remote function module Z_M_EXP_BRIEFDATEN    
    // Throws:  No_Data Transform_Error 
    [RfcMethod(AbapName = "Z_M_EXP_BRIEFDATEN")]
    [SoapDocumentMethodAttribute("http://tempuri.org/Z_M_EXP_BRIEFDATEN",
     RequestNamespace = "urn:sap-com:document:sap:rfc:functions",
     RequestElementName = "Z_M_EXP_BRIEFDATEN",
     ResponseNamespace = "urn:sap-com:document:sap:rfc:functions",
     ResponseElementName = "Z_M_EXP_BRIEFDATEN.Response")]
    public void Z_M_Exp_Briefdaten (

     [RfcParameter(AbapName = "IMP_ERDATB",RfcType=RFCTYPE.RFCTYPE_DATE, Optional = true, Direction = RFCINOUT.IN, Length = 8, Length2 = 16)]
     [XmlElement("IMP_ERDATB", IsNullable=false)]
     string Imp_Erdatb,
     [RfcParameter(AbapName = "IMP_ERDATV",RfcType=RFCTYPE.RFCTYPE_DATE, Optional = true, Direction = RFCINOUT.IN, Length = 8, Length2 = 16)]
     [XmlElement("IMP_ERDATV", IsNullable=false)]
     string Imp_Erdatv,
     [RfcParameter(AbapName = "IMP_FAHRG",RfcType=RFCTYPE.RFCTYPE_CHAR, Optional = true, Direction = RFCINOUT.IN, Length = 30, Length2 = 60)]
     [XmlElement("IMP_FAHRG", IsNullable=false)]
     string Imp_Fahrg,
     [RfcParameter(AbapName = "IMP_KUNNR",RfcType=RFCTYPE.RFCTYPE_CHAR, Optional = true, Direction = RFCINOUT.IN, Length = 10, Length2 = 20)]
     [XmlElement("IMP_KUNNR", IsNullable=false)]
     string Imp_Kunnr,
     [RfcParameter(AbapName = "EXP_XML_STRING",RfcType=RFCTYPE.RFCTYPE_STRING, Optional = true, Direction = RFCINOUT.OUT)]
     [XmlElement("EXP_XML_STRING", IsNullable=false)]
     out string Exp_Xml_String)
    {
        object[]results = null;
        results = this.SAPInvoke("Z_M_Exp_Briefdaten",new object[] {
                            Imp_Erdatb,Imp_Erdatv,Imp_Fahrg,Imp_Kunnr });
        Exp_Xml_String = (string) results[0];

        return;
    }


    // Generated method used to call the remote function module Z_M_READ_TAB_EQUI_AND_001    
    // Throws:  No_Data 
    [RfcMethod(AbapName = "Z_M_READ_TAB_EQUI_AND_001")]
    [SoapDocumentMethodAttribute("http://tempuri.org/Z_M_READ_TAB_EQUI_AND_001",
     RequestNamespace = "urn:sap-com:document:sap:rfc:functions",
     RequestElementName = "Z_M_READ_TAB_EQUI_AND_001",
     ResponseNamespace = "urn:sap-com:document:sap:rfc:functions",
     ResponseElementName = "Z_M_READ_TAB_EQUI_AND_001.Response")]
    public void Z_M_Read_Tab_Equi_And_001 (

     [RfcParameter(AbapName = "I_KUNNR",RfcType=RFCTYPE.RFCTYPE_CHAR, Optional = true, Direction = RFCINOUT.IN, Length = 10, Length2 = 20)]
     [XmlElement("I_KUNNR", IsNullable=false)]
     string I_Kunnr,
     [RfcParameter(AbapName = "E_XML",RfcType=RFCTYPE.RFCTYPE_STRING, Optional = true, Direction = RFCINOUT.OUT)]
     [XmlElement("E_XML", IsNullable=false)]
     out string E_Xml,
     [RfcParameter(AbapName = "TAB_IN",RfcType=RFCTYPE.RFCTYPE_ITAB, Optional = false, Direction = RFCINOUT.INOUT)]
     [XmlArray("TAB_IN", IsNullable=false)]
     [XmlArrayItem("ZDAD_TAB_EQUI_AND_001", IsNullable=false)]
     ref ZDAD_TAB_EQUI_AND_001Table Tab_In)
    {
        object[]results = null;
        results = this.SAPInvoke("Z_M_Read_Tab_Equi_And_001",new object[] {
                            I_Kunnr,Tab_In });
        E_Xml = (string) results[0];
        Tab_In = (ZDAD_TAB_EQUI_AND_001Table) results[1];

        return;
    }


    // Generated method used to call the remote function module Z_M_EXPORT_ZUL_001    
    // Throws:  No_Data 
    [RfcMethod(AbapName = "Z_M_EXPORT_ZUL_001")]
    [SoapDocumentMethodAttribute("http://tempuri.org/Z_M_EXPORT_ZUL_001",
     RequestNamespace = "urn:sap-com:document:sap:rfc:functions",
     RequestElementName = "Z_M_EXPORT_ZUL_001",
     ResponseNamespace = "urn:sap-com:document:sap:rfc:functions",
     ResponseElementName = "Z_M_EXPORT_ZUL_001.Response")]
    public void Z_M_Export_Zul_001 (

     [RfcParameter(AbapName = "I_ERDAT",RfcType=RFCTYPE.RFCTYPE_DATE, Optional = false, Direction = RFCINOUT.IN, Length = 8, Length2 = 16)]
     [XmlElement("I_ERDAT", IsNullable=false)]
     string I_Erdat,
     [RfcParameter(AbapName = "I_KUNNR",RfcType=RFCTYPE.RFCTYPE_CHAR, Optional = false, Direction = RFCINOUT.IN, Length = 10, Length2 = 20)]
     [XmlElement("I_KUNNR", IsNullable=false)]
     string I_Kunnr,
     [RfcParameter(AbapName = "E_XML",RfcType=RFCTYPE.RFCTYPE_STRING, Optional = true, Direction = RFCINOUT.OUT)]
     [XmlElement("E_XML", IsNullable=false)]
     out string E_Xml)
    {
        object[]results = null;
        results = this.SAPInvoke("Z_M_Export_Zul_001",new object[] {
                            I_Erdat,I_Kunnr });
        E_Xml = (string) results[0];

        return;
    }


  } // rfm client proxy


} // Namespace
