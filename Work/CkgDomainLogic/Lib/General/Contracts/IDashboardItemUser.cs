namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardItemUser
    {
        int ID { get; set; }

        string UserName { get; set; }

        string ItemsXml { get; set; }
    }
}
