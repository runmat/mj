using AutoAct.Entities;
using AutoAct.Enums;
using AutoAct.Rest;

namespace AutoAct.Interfaces
{
    /// <summary>
    /// Kommunikation mit AutoAct via REST Implementierungen
    /// </summary>
    public interface IAutoActRest
    {
        bool IsAlive();
        void SetDiegstAuthenticator(string anmeldeName, string passwort);
        Result<VehiclesResult> GetVehicles();
        Result<AutoActMakesResult> GetMakes(VehicleType vehicleType);
        Result<AutoActModelsResult> GetModels(VehicleType vehicleType, string make);
        Result<Vehicle> PostVehicle(Vehicle vehicle);
        Result<bool> DeleteVehicle(long id);
        Result<bool> DeletePictures(long id);
        Result<AttachmentsResult> PostAttachment(string vehicleId, AttachmentType attachmentType, string fileName, string pathAndfileName);
        Result<PicturesResult> PostPictures(string vehicleId, string[] fileName);
    }
}
