using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using Loader;
using ToolboxLibrary;

namespace Host
{
    /// <summary>
    /// This is responsible for naming the components as they are created.
    /// This is added as a servide by the HostSurfaceManager
    /// </summary>
	public class NameCreationService : INameCreationService
	{


		public NameCreationService()
		{
		}

		string INameCreationService.CreateName(IContainer container, Type type)
		{
            const string separator = BasicHostLoader.NameNumberSeparator;
			var cc = container.Components;
			var max = 0;
		    var componentName = type.Name;

		    if (type == typeof (PdfLabel))
		    {
                componentName = Toolbox.LastLabelName;
		    }

			for (var i = 0; i < cc.Count; i++)
			{
				var comp = cc[i] as Component;

			    if (comp == null || comp.GetType() != type) continue;
			    
                var name = comp.Site.Name;
                if (!name.StartsWith(componentName) || !name.Contains(separator)) continue;

                var compNumber = Int32.Parse(name.Substring(name.IndexOf(separator, StringComparison.InvariantCulture) + 1));
			    if (compNumber > max)
			        max = compNumber;
			}

            return componentName + separator + (max + 1);
		}

		bool INameCreationService.IsValidName(string name)
		{
			return true;
		}
		void INameCreationService.ValidateName(string name)
		{
			return;
		}

	}// class
}// namespace
