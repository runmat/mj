
namespace CkgDomainLogic.General.Contracts
{
    public interface IUploadItem
    {
        int DatensatzNr { get; set; }

        string ValidationStatus { get; }

        string ValidationErrorsJson { get; set; }

        string SaveStatus { get; set; }

        bool ValidationOk { get; set; }

        bool IsValid { get; }
    }
}
