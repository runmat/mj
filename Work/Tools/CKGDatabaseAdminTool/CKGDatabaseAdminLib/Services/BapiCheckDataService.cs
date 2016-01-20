using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models;
using System.Linq;
using GeneralTools.Models;

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
            var sectionData = Config.GetAllDbConnections();
            _dataContext = new DatabaseContext(sectionData.Get(connectionName));

            _dataContext.Bapis.Load();
            _dataContext.BapiCheckItems.Load();
        }

        public string PerformBapiCheck()
        {
            var bapiStrukturNeu = new byte[] { };

            BapiCheckAbweichungen = new List<BapiCheckAbweichung>();

            var strukturen = _dataContext.GetBapiCheckItemsForCheck(_testSap);

            foreach (var bapi in _dataContext.BapisSorted)
            {
                var bapiName = bapi.BAPI.NotNullOrEmpty().ToUpper().Trim();

                var tmpAbw = new BapiCheckAbweichung { BapiName = bapiName };

                try
                {
                    bapiStrukturNeu = S.AP.GetSerializedBapiStructuresForBapiCheck(bapiName);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("FU_NOT_FOUND"))
                    {
                        tmpAbw.DoesNotExistInSap = true;
                    }
                    else
                    {
                        return ex.Message;
                    }
                }

                var strukturAlt = strukturen.FirstOrDefault(s => s.BapiName == bapiName);

                if (strukturAlt == null)
                {
                    tmpAbw.IsNew = true;
                }
                else
                {
                    var bapiStrukturAlt = strukturAlt.BapiStruktur;

                    if (bapiStrukturAlt != null)
                    {
                        if (!tmpAbw.DoesNotExistInSap)
                        {
                            if (bapiStrukturNeu != null && !bapiStrukturAlt.SequenceEqual(bapiStrukturNeu))
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
                        _dataContext.SaveBapiCheckItem(bapiName, bapiStrukturNeu, tmpAbw.IsNew, _testSap);
                    }
                }
            }

            return "";
        }
    }
}