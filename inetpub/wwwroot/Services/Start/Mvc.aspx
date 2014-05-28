<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Mvc.aspx.vb" Inherits="CKG.Services.Mvc"
    MasterPageFile="../MasterPage/Services.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function hideiFrameLoading() {
            document.getElementById('diviFrameLoading').style.display = "none";
            document.getElementById('diviFrame').style.display = "block";
        } 
    </script> 
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="diviFrameLoading" align="center"> 
                <img src="../Images/ajax-loading.gif" alt="" style="margin-top: 250px; margin-bottom: 250px"/> 
            </div> 
            <div id="innerContent">
                <div id="diviFrame" style="display: none">
                    <iframe runat="server" ID="ifrMvcApp" scrolling="auto" width="910" height="720" frameBorder="0"></iframe>
                </div>
            </div>
        </div>
    </div>
</asp:Content>