<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report07.aspx.vb" Inherits="AppAvis.Report07" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="2">
                            Bitte geben Sie die Auswahlkriterien ein.
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton" 
                                Height="15px" Width="100px"> •&nbsp;Zurück</asp:LinkButton>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="" valign="top">
                                        <table class="BorderLeftBottom" id="Table5" cellspacing="1" cellpadding="1" width="300"
                                            border="0">
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td colspan="3">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <p>
                                                        Amtl. Kennzeichen:</p>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAmtlKennzeichen" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    Fahrgestellnummer:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    Briefnummer:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBriefnummer" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    MVA-Nr.
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrdernummer" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    Produktionskennziffer:</td>
                                                <td>
                                                    <asp:TextBox ID="txtProduktionskennziffer" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td valign="middle" width="150" style="margin: 10px;">
                                                    <asp:CheckBox ID="cbWhat"  Height="15px" Width="100px" runat="server" Text="ORM" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td valign="middle" width="150" >
                                                    <asp:LinkButton ID="cmdCreate"  Height="15px" Width="100px" runat="server" CssClass="StandardButton"  >•&nbsp;Erstellen</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
