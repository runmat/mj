using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services; 

namespace Leasing.forms
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für ArvalService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf. 
    [System.Web.Script.Services.ScriptService]
    public class ArvalService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string[] GetAdressList(string prefixText, int count)
        {
            string[] NewArr = new string[1];
            string[] BaseArr = null;
            int i = 0;

            BaseArr = (string[])Session["LnAdressen"];

            try
            {

                if (prefixText.Length > 2)
                {
                    for (i = 0; i <= BaseArr.Length - 1; i++)
                    {

                        if (BaseArr[i].ToUpper().Contains(prefixText.ToUpper()))
                        {
                            if (NewArr[0] == null)
                            {
                                NewArr[0] = BaseArr[i];

                            }
                            else
                            {
                                Array.Resize(ref NewArr, NewArr.Length + 1);

                                NewArr[NewArr.Length - 1] = BaseArr[i];

                            }

                            if (count > 0 )
                            {
                                if (NewArr.Length >= count)
                                {
                                    break;
                                }
                            }


                        }
                    }
                }

                if (NewArr[0] == null)
                {
                    NewArr = null;
                }

            }
            catch (Exception ex)
            {
                NewArr = null;
                return NewArr;
            }


            return NewArr;

        }



    }
}
