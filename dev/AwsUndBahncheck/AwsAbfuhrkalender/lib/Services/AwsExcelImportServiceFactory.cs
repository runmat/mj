using AwsAbfuhrkalender.Contracts;

namespace AwsAbfuhrkalender.Services
{
    public class AwsExcelImportServiceFactory
    {
        public static IAwsExceImportService Create()
        {
            return new AwsExcelImportService();
        }
    }
}
