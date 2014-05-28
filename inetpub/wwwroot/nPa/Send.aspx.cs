using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using org.fokus.npa.connector;
using org.fokus.npa.connector.Response;

namespace nPa
{
    public partial class Send : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessNpaResponse();
        }

        /// <summary>
        /// Processes a response from the eID Identity Provider 
        /// </summary>
        protected void ProcessNpaResponse()
        {

            //get the post Form
            NameValueCollection nvc = this.Request.Form;


            //check if we got a "SAMLResponse" parameter 
            if (!string.IsNullOrEmpty(nvc["SAMLResponse"]))
            {
                //if true then it means the request came probably from the eid server
                //lets start decoding it 

                //lets get the handler we previosly stored in the user session 
                // this will help us us the profile that originated the request 
                eIDHandler handler = Session["eIDHandler"] as eIDHandler;



                if (handler == null)
                {
                    //if the handler that originated the request is not stored 
                    //then we should creat another one based on the profile that we use 

                    //NOTE : if using more than one profile then you should try to store the profile that originated the request
                    //because it also stores the certificates that will be used to decript the message and if those are diffrent 
                    //you will not be able to get the server response.
                    handler = eIDConnector.getInstance().createHandler("basicProfile");
                }

                //initialize a handler to parse the response
                eIDResponseHandler myResponseHandler = handler.handleResponse(HttpContext.Current);

                //get the status of the request from the server 
                StatusInformation statusInformation = myResponseHandler.ResponseStatus;

                //check to see if the request has been sucessfull
                if (statusInformation.getStatus() == Status.Success)
                {
                    //get all the attributes 
                    //NOTE: this container will have only the requested attributes filled 
                    ResponseAttributes responseAttributes = myResponseHandler.getResponseAttributes;

                    //foreach (string responseAttribute in responseAttributes)
                    //{
                    //    var attributeValue = responseAttributes.Attribute(responseAttribute);

                    //    //do something with the values
                    //}

                    //do something with the attribtues 
                    //lets set the name to te control  
                    //lblName.Text = string.Format("{0} {1}", responseAttributes.FamilyNames, responseAttributes.GivenNames) + "(" + Session["ReturnURL"].ToString() + ")";

                    string URL = Session["ReturnURL"].ToString();

                    string Parameter;

                    string pr = "," + "," + responseAttributes.PlaceOfResidence.City + "," + responseAttributes.PlaceOfResidence.Street + "," + responseAttributes.PlaceOfResidence.ZipCode;


                    Parameter = "&eIdentityResponse=1" + "&GivenNames=" + responseAttributes.GivenNames + "&FamilyNames=" + responseAttributes.FamilyNames +
                                "&PlaceOfResidence=" + pr + "&DateOfBirth=" + responseAttributes.DateOfBirth +
                                "&PlaceOfBirth=" + responseAttributes.PlaceOfBirthFreetextPlace;


                    URL += Parameter;

                    Response.Redirect(URL, true);

                }
            }
        }

    }
}
