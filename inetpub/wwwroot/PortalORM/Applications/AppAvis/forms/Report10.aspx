<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report10.aspx.vb" Inherits="AppAvis.Report10" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>

</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager runat="server" ID="Scriptmanager1" EnableScriptLocalization="true" EnableScriptGlobalization="true"></asp:ScriptManager>
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
                            <asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton" 
                                            Height="16px" Width="100px"> &#149;&nbsp;Erstellen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton" 
                                            Height="16px" Width="100px" CausesValidation="false"> •&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td width="150">
                                                    Rechnungsnummer:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtRechnNr" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="150">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtFahrgestNr" runat="server"></asp:TextBox>
                                                    <asp:Image runat="server" ID="iFahrgestellTooltip" ImageUrl="/PortalORM/images/fragezeichen.gif" ToolTip="Selektion nach Ausgabeart nur mit Fahrgestellnr. möglich" />
                                                    &nbsp;
                                                    <asp:CheckBox ID="cbRechnungsausgabe" runat="server" Text="Rechnungsausgabe" Checked="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="150">
                                                    Rechnungsdatum von:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtRechnDatVon" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtRechnDatVon_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtRechnDatVon">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                    <asp:CompareValidator ID="cv_txtRechnDatVon" runat="server" 
                                                    ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txtRechnDatVon"
                                                    ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>                                                      
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="130">
                                                    Rechnungsdatum bis:
                                                </td>
                                                <td class="StandardTableAlternate" valign="center">
                                                    <asp:TextBox ID="txtRechnDatBis" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtRechnDatBis_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtRechnDatBis">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                    <asp:CompareValidator ID="cv_txtRechnDatBis" runat="server" 
                                                    ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txtRechnDatBis"
                                                    ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>                                                   
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td width="130">
                                                    Leistungsdatum von:
                                                </td>
                                                <td class="StandardTableAlternate" valign="center">
                                                    <asp:TextBox ID="txtLeistDatVon" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtLeistDatVon_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtLeistDatVon">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                    <asp:CompareValidator ID="cv_txtLeistDatVon" runat="server" 
                                                    ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txtLeistDatVon"
                                                    ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>                                                   
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td width="150">
                                                    Leistungsdatum bis:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtLeistDatBis" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtLeistDatBis_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtLeistDatBis">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                    <asp:CompareValidator ID="cv_txtLeistDatBis" runat="server" 
                                                    ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txtLeistDatBis"
                                                    ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>                                                      
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="150">
                                                    Leistungsart:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtLeistArt" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="150">
                                                    Spediteur:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtSpediteur" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="'Rechnungsdatum von' darf nicht größer als 'Rechnungsdatum bis' sein!"
                                        Type="Date" ControlToValidate="txtRechnDatVon" ControlToCompare="txtRechnDatBis"
                                        Operator="LessThanEqual" Font-Bold="True"></asp:CompareValidator>
                                    <br /><asp:CompareValidator ID="CompareValidator6" runat="server" ErrorMessage="'Leistungsdatum von' darf nicht größer als 'Leistungsdatum bis' sein!"
                                        Type="Date" ControlToValidate="txtLeistDatVon" ControlToCompare="txtLeistDatBis"
                                        Operator="LessThanEqual" Font-Bold="True"></asp:CompareValidator>                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;<asp:TextBox id="TextBox2" runat="server" Visible="false"/>
                        </td>

                        <td valign="top">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
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
