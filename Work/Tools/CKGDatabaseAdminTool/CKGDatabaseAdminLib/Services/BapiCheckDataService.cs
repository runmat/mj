using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models;
using System.Linq;
using SapORM.Contracts;

namespace CKGDatabaseAdminLib.Services
{
    public class BapiCheckBapiDataService : CkgGeneralDataService, IBapiCheckDataService
    {
        public List<BapiCheckAbweichung> BapiCheckAbweichungen { get; set; }

        private DatabaseContext _dataContext;

        private bool _testSap;

        public BapiCheckBapiDataService(string connectionName, bool testSap)
        {
            _testSap = testSap;
            InitDataContext(connectionName);
        }

        public void InitDataContext(string connectionName)
        {
            var sectionData = (NameValueCollection)ConfigurationManager.GetSection("dbConnections");
            _dataContext = new DatabaseContext(sectionData.Get(connectionName));

            _dataContext.Bapis.Load();
            _dataContext.BapiCheckItems.Load();
        }

        public string PerformBapiCheck()
        {
            byte[] impNeu = new byte[] { };
            byte[] expNeu = new byte[] { };

            BapiCheckAbweichungen = new List<BapiCheckAbweichung>();

            var strukturen = _dataContext.GetBapiCheckItemsForCheck(_testSap);

            foreach (var bapi in _dataContext.BapisSorted)
            {
                var tmpAbw = new BapiCheckAbweichung { BapiName = bapi.BAPI };

                try
                {
                    S.AP.GetSerializedBapiStructuresForBapiCheck(bapi.BAPI, ref impNeu, ref expNeu);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("FUNCTION_NOT_EXIST Execution failed"))
                    {
                        tmpAbw.DoesNotExistInSap = true;
                    }
                    else
                    {
                        return ex.Message;
                    }
                }

                var strukturAlt = strukturen.Find(s => s.BapiName.ToUpper() == bapi.BAPI.ToUpper());

                if (strukturAlt == null)
                {
                    tmpAbw.IsNew = true;
                }
                else
                {
                    var impAlt = strukturAlt.ImportStruktur;
                    var expAlt = strukturAlt.ExportStruktur;

                    if (impAlt != null || expAlt != null)
                    {
                        if (!tmpAbw.DoesNotExistInSap)
                        {
                            if ((impAlt != null && impNeu != null && !impAlt.SequenceEqual(impNeu))
                                || (expAlt != null && expNeu != null && !expAlt.SequenceEqual(expNeu)))
                            {
                                tmpAbw.HasChanged = true;
                            }
                        }
                    }
                }

                if (tmpAbw.HasChanged || tmpAbw.IsNew || tmpAbw.DoesNotExistInSap)
                {
                    BapiCheckAbweichungen.Add(tmpAbw);

                    if (!tmpAbw.DoesNotExistInSap)
                    {
                        _dataContext.SaveBapiCheckItem(bapi.BAPI, impNeu, expNeu, tmpAbw.IsNew, _testSap);
                    }
                }
            }

            return "";
        }
    }
}