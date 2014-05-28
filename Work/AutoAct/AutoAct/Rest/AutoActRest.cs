using System;
using System.Linq;
using System.Net;
using AutoAct.Entities;
using AutoAct.Enums;
using AutoAct.Interfaces;
using AutoAct.Resources;
using GeneralTools.Contracts;
using RestSharp;

namespace AutoAct.Rest
{
    public class AutoActRest : IAutoActRest
    {
        #region Privates and Constructor

        private readonly ILogService _logService;
        private readonly string _baseurl;
        private readonly IRestClient _client;
        private readonly IFileHelper _fileHelper;
        private readonly CustomConverter _serializer = new CustomConverter { ContentType = "application/json" };

        public AutoActRest(ILogService logService, string baseurl, IRestClient client, IFileHelper fileHelper)
        {
            _logService = logService;
            _baseurl = baseurl;
            _client = client;
            _fileHelper = fileHelper;
            _client.BaseUrl = _baseurl;
        }

        #endregion

        #region Authentification & utilities

        public void SetDiegstAuthenticator(string anmeldeName, string passwort)
        {
            var digestAuthenticator = new DigestAuthenticator(anmeldeName, passwort);
            _client.Authenticator = digestAuthenticator;
        }

        public bool IsAlive()
        {
            var request = CreateRequestWithBody(ApplicationStrings.IsAliveResource, Method.GET, null);
            var response = _client.Execute(request);

            return string.IsNullOrEmpty(response.Content);
        }

        #endregion

        #region Vehicle

        /// <summary>
        /// Ermittelt alle eingestellte Fahrzeuge eines Kunden
        /// Keine Kundenummer Übergabe da die Wahl der Fahrzeuge durch die Anmeldung beschränkt ist
        /// </summary>
        /// <returns></returns>
        public Result<VehiclesResult> GetVehicles()
        {
            var request = CreateRequestWithBody(ApplicationStrings.GetVehicleResource, Method.GET, null);
            var response = _client.Execute<VehiclesResult>(request);

            return EvaluateResponse(response);
        }

        public Result<Vehicle> PostVehicle(Vehicle vehicle)
        {
            VehiclePayLoad payload = new VehiclePayLoad { Vehicle = vehicle };
            var request = CreateRequestWithBody(ApplicationStrings.PostVehicleResource, Method.POST, payload);
            var response = _client.Execute<Vehicle>(request);

            return EvaluateResponse(response);
        }

        /// <summary>
        /// Entfernt Fahrzeug samt Dokumente aus dem Bestand der mobile.de
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result<bool> DeleteVehicle(long id)
        {
            string resource = string.Format(ApplicationStrings.DeleteVehicleResource, id.ToString());

            var request = CreateRequestWithBody(resource, Method.DELETE, null);
            var response = _client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return new Result<bool>{ Value = true};
            }

            var ex = new ApplicationException(response.ErrorMessage, response.ErrorException);

            _logService.LogError(ex, null, null); // Optional Paramter angeben da ich sonst keine Mocks schreiben kann!
            var result =  new Result<bool>{ Value = false };
            result.Errors.errors.Add(new Error{ code = response.StatusCode.ToString(), message = new Message{ de = ex.Message } });
            return result;
        }

        #endregion

        #region Make & Model

        public Result<AutoActMakesResult> GetMakes(VehicleType vehicleType)
        {
            var resource = string.Format(ApplicationStrings.GetMakeResource, vehicleType.ToString());
            var request = CreateRequestWithBody(resource, Method.GET, null);
            var response = _client.Execute<AutoActMakesResult>(request);

            return EvaluateResponse(response);
        }

        public Result<AutoActModelsResult> GetModels(VehicleType vehicleType, string make)
        {
            var resource = string.Format(ApplicationStrings.GetModelResourcce, vehicleType.ToString(), make.ToUpper());
            var request = CreateRequestWithBody(resource, Method.GET, null);
            var response = _client.Execute<AutoActModelsResult>(request);

            return EvaluateResponse(response);
        }

        #endregion

        #region Pictures

        /// <summary>
        /// Entfernt alle Fahrzeuge eines Fahrzeugs aus dem Bestand der mobile.de
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result<bool> DeletePictures(long id)
        {
            string resource = string.Format(ApplicationStrings.PostImageResource, id.ToString());

            var request = CreateRequestWithBody(resource, Method.PUT, null);
            var response = _client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new Result<bool> { Value = true };
            }

            var ex = new ApplicationException(response.ErrorMessage, response.ErrorException);

            _logService.LogError(ex, null, null); // Optional Paramter angeben da ich sonst keine Mocks schreiben kann!
            var result = new Result<bool> { Value = false };
            result.Errors.errors.Add(new Error { code = response.StatusCode.ToString(), message = new Message { de = ex.Message } });
            return result;
        }

        /// <summary>
        /// Image wird per Post zuz AutoAct übertragen
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Result<PicturesResult> PostPictures(string vehicleId, string[] fileName)
        {
            if (fileName.Any() == false)
            {
                return new Result<PicturesResult>();
            }

            string resource = string.Format(ApplicationStrings.PostImageResource, vehicleId);
            var request = CreateRequestWithBody(resource, Method.POST, null);
            
            foreach (var s in fileName)
            {
                request.AddFile("image", _fileHelper.ReadAllBytes(s), string.Empty, ApplicationStrings.ContentTypeImageJpg); 
            }
            
            var response = _client.Execute<PicturesResult>(request);

            return EvaluateResponse(response);
        }

        #endregion

        #region Attachment

        /// <summary>
        /// Ein Anhang wird einem bereits gespeicherten Fahrzeug hinzugefügt
        /// </summary>
        /// <param name="vehicleId">AutoAct Id des Fahrzeug</param>
        /// <param name="attachmentType"></param>
        /// <param name="fileName"></param>
        /// <param name="pathAndfileName"></param>
        /// <returns></returns>
        public Result<AttachmentsResult> PostAttachment(string vehicleId, AttachmentType attachmentType, string fileName, string pathAndfileName)
        {
            string resource = string.Format(ApplicationStrings.PostAttachmentResource, vehicleId);

            var request = CreateRequestWithBody(resource, Method.POST, null);
            request.AddParameter(ApplicationStrings.PostAttachmentFormPartAttachmentType, attachmentType.ToString());
            request.AddFile(ApplicationStrings.PostAttachmentFormPartFile, _fileHelper.ReadAllBytes(pathAndfileName), fileName, ApplicationStrings.ContentTypePdf);
            var response = _client.Execute<AttachmentsResult>(request);

            return EvaluateResponse(response);
        }

        #endregion

        #region Privates

        /// <summary>
        /// Einheitliche Behandlung für Restoperation die einen  der Rückgabewerte aus einer  mit AutoAct
        /// AutoAct 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private Result<T> EvaluateResponse<T>(IRestResponse<T> response)
        {
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == 0)
            {
                return new Result<T> { Value = response.Data };
            }

            var result = new Result<T> { Errors = _serializer.Deserialize<ErrorRootObject>(response) };

            if (result.Errors == null)
            {
                result.Errors = new ErrorRootObject();
                result.Errors.errors.Add(new Error{ code = response.StatusCode.ToString(), field = "", message = new Message{ de = "Fehler bei einem Rest Aufruf"}});
            }

            return result;
        }

        /// <summary>
        /// Request mit einem PayLoad in Body lassen sich nicht direkt durch RestSharp erzeugen
        /// - Verwende den CusotmConverter
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private IRestRequest CreateRequestWithBody(string uri, Method method, object body)
        {
            IRestRequest request = new RestRequest(uri, method);
            request.Resource = uri;
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new CustomConverter { ContentType = "application/json" };
            request.AddHeader("Accept-Language", ApplicationStrings.LanguageHeader);

            if (body == null)
            {
                return request;
            }

            request.AddHeader("Content-Type", "application/json");
            request.AddBody(body);

            return request;
        }

        #endregion

    }
}
