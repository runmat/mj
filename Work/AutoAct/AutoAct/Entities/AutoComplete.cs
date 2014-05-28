using AutoAct.Resources;

namespace AutoAct.Entities
{
    /// <summary>
    /// REST API verwendet den Wert hier um zu entscheiden ob eine automatische Verfollständigung (!= null) stattfinden soll und wie diese stattfinden soll (AUTOCOMPLETIONTYPE == VIN)
    /// </summary>
    public class AutoComplete 
    {
        public AutoComplete()
        {
            CompletionType = "VIN";
            DescriptionSuffix = ApplicationStrings.AutoComplete_DescriptionSuffix;
        }

        public string CompletionType { get; set; }
        public string DescriptionSuffix { get; set; }
    }
}
