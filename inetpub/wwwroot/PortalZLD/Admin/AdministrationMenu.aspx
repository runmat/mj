<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdministrationMenu.aspx.vb" Inherits="Admin.AdministrationMenu" MasterPageFile="MasterPage/Admin.Master" %>
<%@ Register TagPrefix="uc1" TagName="menue" Src="../Start/Menue.ascx" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;
            </div>
            <div id="innerContent" >
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            Verwaltung</h1>
                    </div>
                    <div id="dataVerwaltung">
                        <table cellspacing="0" cellpadding="0" border="0">
                            <tfoot><tr><td colspan="3">&nbsp;</td></tr></tfoot>
                            <tr id="trUserManagement" runat="server" >
                                <td align="left">
                                    <asp:HyperLink ID="lnkUserManagementPic" runat="server" NavigateUrl="UserManagement.aspx"
                                        ImageUrl="/PortalZLD/Images/User01_06.jpg" Width="60px">Benutzer</asp:HyperLink>
                                </td>
                                <td align="left">
                                    <asp:HyperLink ID="lnkUserManagement" CssClass="LinksVerwaltung" Width="180px" Height="20px"
                                        runat="server" Text="Benutzer" NavigateUrl="UserManagement.aspx" />
                                </td>
                                <td width="100%">
                                </td>
                            </tr>
                            <tr id="trOrganizationManagement" runat="server" visible="false">
                                <td align="left">
                                    <asp:HyperLink ID="lnkOrganizationManagement0" runat="server" NavigateUrl="OrganizationManagement.aspx"
                                        Width="60px" ImageUrl="/PortalZLD/Images/Diagramm02_08.jpg" >Organisationen</asp:HyperLink>
                                </td>
                                <td align="left">
                                    <asp:HyperLink ID="lnkOrganizationManagement" runat="server" CssClass="LinksVerwaltung"
                                        NavigateUrl="OrganizationManagement.aspx" Width="180px" Height="20"
                                        Text="Organisationen" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr  id="trGroupManagement" runat="server" visible="false">
                                <td align="left">
                                    <asp:HyperLink ID="lnkGroupManagement0" runat="server" Width="60px" NavigateUrl="GroupManagement.aspx"
                                      Text="asdasd" ImageUrl="/PortalZLD/Images/User02_06.jpg">Gruppen</asp:HyperLink>
                                </td>
                                <td class="" align="left">
                                    <asp:HyperLink ID="lnkGroupManagement" runat="server" CssClass="LinksVerwaltung"
                                        Width="180px" Height="20" NavigateUrl="GroupManagement.aspx" 
                                        Text="Gruppen" />
                                </td>
                                <td class="" align="left">
                                </td>
                            </tr>
                            <tr id="trCustomerManagement" runat="server" visible="false">
                                <td align="left">
                                    <asp:HyperLink ID="lnkCustomerManagement0" runat="server" Width="60px" NavigateUrl="CustomerManagement.aspx"
                                      ImageUrl="/PortalZLD/Images/customer11.png" >Kunden</asp:HyperLink>
                                </td>
                                <td class="" align="left">
                                    <asp:HyperLink ID="lnkCustomerManagement" runat="server" CssClass="LinksVerwaltung"
                                        Width="180px" Height="20" NavigateUrl="CustomerManagement.aspx"
                                        Text="Kunden" ></asp:HyperLink>
                                </td>
                                <td class="" align="center">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr  id="trAppManagement" runat="server" visible="false">
                                <td align="left" height="32px">
                                    <asp:HyperLink ID="lnkAppManagement0" Width="60px" runat="server" NavigateUrl="AppManagement.aspx"
                                        Text="Anwendungen" ImageUrl="/PortalZLD/Images/Fenster_max.jpg" >Anwendungen</asp:HyperLink>
                                </td>
                                <td nowrap="nowrap" align="left">
                                    <asp:HyperLink ID="lnkAppManagement" CssClass="LinksVerwaltung" Width="180px" Height="20"
                                        runat="server" NavigateUrl="AppManagement.aspx" Text="Anwendungen" />
                                </td>
                                <td class="">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="trArchivManagement" runat="server" visible="false">
                                <td align="left" height="32px" valign="baseline">
                                    <asp:HyperLink ID="lnkArchivManagement0" Width="60px" runat="server" NavigateUrl="AppManagement.aspx"
                                        ImageUrl="/PortalZLD/Images/Buch_08.jpg" >Archive</asp:HyperLink>
                                </td>
                                <td class="" nowrap="nowrap" align="left" valign="middle">
                                    <asp:HyperLink ID="lnkArchivManagement" runat="server" NavigateUrl="ArchivManagement.aspx"
                                       Text="Archive" Width="180px" Height="20px" CssClass="LinksVerwaltung" />
                                </td>
                                <td class="">
                                    &nbsp;
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3">

                                </td>
                            </tr>

                        </table>
                    </div>
                    <div id="dataFooter">&nbsp;</div>
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal><asp:Literal ID="litAlert"
                        runat="server"></asp:Literal>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>