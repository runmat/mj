using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfComboboxAutocomplete
{
    public interface IAutoCompleteTagCloudConsumer
    {
        void OnDropDownTabKey(string tagText);
    }
}
