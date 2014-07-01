using System.Collections.Generic;

namespace CkgDomainLogic.General.Contracts
{
    public interface IApplicationUserMenuProvider
    {
        List<IApplicationUserMenuItem> GetMenuItemGroups();

        List<IApplicationUserMenuItem> GetMenuItems(string appType = null);

        bool AppFavoritesEditMode { get; set; }

        bool AppFavoritesEditSwitchOneFavorite(int appID);
    }
}
