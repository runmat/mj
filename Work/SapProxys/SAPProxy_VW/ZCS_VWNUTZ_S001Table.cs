
//------------------------------------------------------------------------------
// 
//     This code was generated by a SAP. NET Connector Proxy Generator Version 1.0
//     Created at 14.05.2008
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

namespace SAPProxy_VW
{

  public class ZCS_VWNUTZ_S001Table : SAPTable 
  {
    public static Type GetElementType() 
    {
        return (typeof(ZCS_VWNUTZ_S001));
    }
 
    public override object CreateNewRow()
    { 
        return new ZCS_VWNUTZ_S001();
    }
     
    public ZCS_VWNUTZ_S001 this[int index] 
    {
        get 
        {
            return ((ZCS_VWNUTZ_S001)(List[index]));
        }
        set 
        {
            List[index] = value;
        }
    }
        
    public int Add(ZCS_VWNUTZ_S001 value) 
    {
        return List.Add(value);
    }
        
    public void Insert(int index, ZCS_VWNUTZ_S001 value) 
    {
        List.Insert(index, value);
    }
        
    public int IndexOf(ZCS_VWNUTZ_S001 value) 
    {
        return List.IndexOf(value);
    }
        
    public bool Contains(ZCS_VWNUTZ_S001 value) 
    {
        return List.Contains(value);
    }
        
    public void Remove(ZCS_VWNUTZ_S001 value) 
    {
        List.Remove(value);
    }
        
    public void CopyTo(ZCS_VWNUTZ_S001[] array, int index) 
    {
        List.CopyTo(array, index);
	}
  }
}
