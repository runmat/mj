<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="JahresArchivManagement.aspx.vb" Inherits="Admin.JahresArchivManagement"  MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../JScript/Jquery/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../JScript/Jquery/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
     <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkUserManagement" runat="server" ToolTip="Benutzer" NavigateUrl="UserManagement.aspx"
                            Text="Benutzer | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkContact" runat="server" ToolTip="Ansprechpartner" NavigateUrl="Contact.aspx"
                            Text="Ansprechpartner | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkGroupManagement" runat="server" ToolTip="Gruppen" NavigateUrl="GroupManagement.aspx"
                            Text="Gruppen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>                            
                        <asp:HyperLink ID="lnkCustomerManagement" ToolTip="Kunden" runat="server" NavigateUrl="CustomerManagement.aspx"
                            Text="Kunden | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkArchivManagement" ToolTip="Archive" runat="server" NavigateUrl="ArchivManagement.aspx"
                            Text="Archive | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkJahresArchivManagement" ToolTip="Archive" runat="server" NavigateUrl="JahresArchivManagement.aspx"
                            Text="Jahres-Archive | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkAppManagement" runat="server" ToolTip="Anwendungen" NavigateUrl="AppManagement.aspx"
                            Text="Anwendungen"></asp:HyperLink>                    
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                            </h1>
                        </div>

                        <br/><br/><br/><br/>
                        <div ID="divStart" runat="server">
                            <asp:Label ID="Label1" runat="server" ForeColor="Black" Text="Jahres-Archive kopieren in Jahr:" />
                            <asp:TextBox runat="server" ID="tbDestinationYear" Width="40" style="text-align: center;" />
                            <asp:Label ID="lblDestinationYearHint" runat="server" ForeColor="Gray" Text=" (Hinweis: Kopiert wird jeweils aus dem Vorjahr!)" />

                            <br/>
                            <br/>
                            <asp:Button runat="server" ID="btnCopyYearArchives" Text="Jahres-Archive jetzt kopieren!" />
                            
                            <br/>
                        </div>
                        <div ID="divResults" runat="server">
                            
                            <asp:Label runat="server" ID="lblResult" ForeColor="Black" />

                        </div>
                        <br/><br/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>