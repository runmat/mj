using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.Logs.Models
{
    public enum SapBapiDurationLevel
    {
        VeryQuick = 0,
        Quick = 1,
        Normal = 2,
        Long = 3,
        VeryLong = 4,
        Unacceptable = 5
    };

    public class SapBapiDuration
    {
        public SapBapiDurationLevel Level { get; set; }

        [SelectListKey]
        public int LevelAsInt { get { return Int32.Parse(Level.ToString("d")); } }

        [SelectListText]
        public string LevelAsString { get { return Localize.TranslateResourceKey(Level.ToString("F")); } }

        public string LevelCssClass { get { return "duration-level-" + Level.ToString("F").ToLowerAndHyphens(); } }

        public int ThresholdUpFromSeconds { get; set; }

        public static List<SapBapiDuration> Presets
        {
            get
            {
                return new List<SapBapiDuration>
                    {
                        new SapBapiDuration { Level = SapBapiDurationLevel.VeryQuick, ThresholdUpFromSeconds = -1 },
                        new SapBapiDuration { Level = SapBapiDurationLevel.Quick, ThresholdUpFromSeconds = 3 },
                        new SapBapiDuration { Level = SapBapiDurationLevel.Normal, ThresholdUpFromSeconds = 6 },
                        new SapBapiDuration { Level = SapBapiDurationLevel.Long, ThresholdUpFromSeconds = 20 },
                        new SapBapiDuration { Level = SapBapiDurationLevel.VeryLong, ThresholdUpFromSeconds = 60 },
                        new SapBapiDuration { Level = SapBapiDurationLevel.Unacceptable, ThresholdUpFromSeconds = 120 }
                    };
            }
        }

        public static List<SapBapiDuration> LegendPresets
        {
            get { return Presets; }
        }

        public string ThresholdUpFromSecondsAsString
        {
            get
            {
                if (ThresholdUpFromSeconds > 0)
                {
                    
                    return string.Format(Localize.DauerSAPAufruf, ThresholdUpFromSeconds);
                }
                return string.Empty;
            }
        }

        public static SapBapiDuration GetSapBapiDuration(int levelAsInt)
        {
            return Presets.FirstOrDefault(d => d.LevelAsInt == levelAsInt) ?? Presets.FirstOrDefault(d => d.Level == SapBapiDurationLevel.VeryQuick);
        }

        public static SapBapiDuration GetSapBapiDuration(double? dauer)
        {
            return Presets.LastOrDefault(d => dauer.GetValueOrDefault() >= d.ThresholdUpFromSeconds) ?? Presets.FirstOrDefault(d => d.Level == SapBapiDurationLevel.Normal);
        }
    }
}
