
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

  [RfcStructure(AbapName ="ZDAD_TAB_EQUI_AND_001" , Length = 30, Length2 = 60)]
  public class ZDAD_TAB_EQUI_AND_001 : SAPStructure
  {
    
    [RfcField(AbapName = "CHASSIS_NUM", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 0, Offset2 = 0)]
    [XmlElement("CHASSIS_NUM")]
    public string Chassis_Num
    { 
       get
       {
          return _Chassis_Num;
       }
       set
       {
          _Chassis_Num = value;
       }
    }
    private string _Chassis_Num;

  }

}
