namespace CkgDomainLogic.General.Contracts
{
    public interface IApplicationUserMenuItem 
    {
        int AppID { get; set; }

        string AppName { get; set; }

        string AppFriendlyName { get; set; }

        string AppURL { get; set; }

        int AppRank { get; set; }

        bool AppIsNoMvcFavorite { get; set; }

        
        #region Menu Group
        
        string AppType { get; set; }

        int AppTypeRank { get; set; }

        string AppTypeFriendlyName { get; set; }

        #endregion
    }
}
