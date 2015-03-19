﻿using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class KlaerfallVhc
    {
        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        public string HaendlerNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNo)]
        public string OrderNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime? Anlagedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDateClarificationCase)]
        public DateTime? AnlagedatumKlaerfall { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfZb2Receipt)]
        public DateTime? EingangsdatumZb2 { get; set; }
    }
}
