<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Mvc.aspx.vb" Inherits="CKG.PortalZLD.Mvc"
    MasterPageFile="../MasterPage/Services.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                
                <iframe runat="server" ID="ifrMvcApp" scrolling="auto" width="910" height="720" frameBorder="0"></iframe>
                
            </div>
        </div>
    </div>
</asp:Content>