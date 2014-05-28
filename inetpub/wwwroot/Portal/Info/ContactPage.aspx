<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ContactPage.aspx.vb" Inherits="CKG.Portal.Info.ContactPage" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body topmargin="0" leftmargin="0">
    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="PageNavigation" width="150">
                                &nbsp;
                            </td>
                            <td class="PageNavigation">
                                &nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="150">
                                <table id="Table2" bordercolor="#ffffff" cellspacing="1" cellpadding="0" width="150"
                                    border="1">
                                    <tr>
                                        <td class="TextHeader" width="150">
                                            Kontakt
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" height="25">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" height="74">
                                            <asp:Label ID="lblCName" runat="server" Font-Bold="True">DAD Deutscher Auto Dienst GmbH</asp:Label><br />
                                            <asp:Label ID="lblCAddress" runat="server">Ladestraﬂe 1, 22926 Ahrensburg<br/>Hotline: +49 4102 804-0</asp:Label><br />
                                            <asp:Panel ID="pnlLinks" runat="server">
                                                <asp:HyperLink ID="lnkMail" runat="server" NavigateUrl="mailto:info@dad.de">info@dad.de</asp:HyperLink>
                                                <br/>
                                                <asp:HyperLink ID="lnkWeb" runat="server" NavigateUrl="http://www.dad.de">www.dad.de</asp:HyperLink>
                                            </asp:Panel>
                                            <br/>
                                            <asp:Panel ID="pnl2ndAddress" runat="server" Visible="true">
                                                <br/>
                                                <asp:Label ID="lblCName2" runat="server" Font-Bold="True">Christoph Kroschke GmbH</asp:Label>
                                                <br/>
                                                <asp:Label ID="lblCAddress2" runat="server">Ladestraﬂe 1, 22926 Ahrensburg<br/>Telefon: +49 4102 804-0 </asp:Label>
                                                <br/>
                                                <asp:Panel ID="pnlLinks2" runat="server">
                                                    <asp:HyperLink ID="lnkMail2" runat="server" NavigateUrl="mailto:kcl-dispo@kroschke.de"
                                                        Visible="False">kcl-dispo@kroschke.de</asp:HyperLink>
                                                    <br/>
                                                    <asp:HyperLink ID="lnkWeb2" runat="server" NavigateUrl="http://www.kroschke.de">www.kroschke.de</asp:HyperLink>
                                                </asp:Panel>
                                                <br/>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" height="25">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <!--#include File="../PageElements/Footer.html" -->
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
