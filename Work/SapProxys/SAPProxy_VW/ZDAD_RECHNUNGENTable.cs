
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

  public class ZDAD_RECHNUNGENTable : SAPTable 
  {
    public static Type GetElementType() 
    {
        return (typeof(ZDAD_RECHNUNGEN));
    }
 
    public override object CreateNewRow()
    { 
        return new ZDAD_RECHNUNGEN();
    }
     
    public ZDAD_RECHNUNGEN this[int index] 
    {
        get 
        {
            return ((ZDAD_RECHNUNGEN)(List[index]));
        }
        set 
        {
            List[index] = value;
        }
    }
        
    public int Add(ZDAD_RECHNUNGEN value) 
    {
        return List.Add(value);
    }
        
    public void Insert(int index, ZDAD_RECHNUNGEN value) 
    {
        List.Insert(index, value);
    }
        
    public int IndexOf(ZDAD_RECHNUNGEN value) 
    {
        return List.IndexOf(value);
    }
        
    public bool Contains(ZDAD_RECHNUNGEN value) 
    {
        return List.Contains(value);
    }
        
    public void Remove(ZDAD_RECHNUNGEN value) 
    {
        List.Remove(value);
    }
        
    public void CopyTo(ZDAD_RECHNUNGEN[] array, int index) 
    {
        List.CopyTo(array, index);
	}
  }
}
