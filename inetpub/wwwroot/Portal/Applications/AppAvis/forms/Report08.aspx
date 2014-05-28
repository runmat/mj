<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report08.aspx.vb" Inherits="AppAvis.Report08" %>
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
                                                    Zulassungsdatum von:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtZuldatumVon" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtZuldatumVon_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtZuldatumVon">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                    <asp:CompareValidator ID="cv_txtZuldatumVon" runat="server" 
                                                    ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txtZuldatumVon"
                                                    ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>                                                      
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="130">
                                                    Zulassungsdatum bis:
                                                </td>
                                                <td class="StandardTableAlternate" valign="center">
                                                    <asp:TextBox ID="txtZuldatumBis" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtZuldatumBis_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtZuldatumBis">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                    <asp:CompareValidator ID="cv_txtZuldatumBis" runat="server" 
                                                    ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txtZuldatumBis"
                                                    ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>                                                   
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td width="150">
                                                    Freisetzungsdatum von:</td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtFreisetdatvon" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtFreisetdatvon_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtFreisetdatvon">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                <asp:CompareValidator ID="cv_txtFreisetdatvon" runat="server" 
                                                ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txtFreisetdatvon"
                                                ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>  </td>
                                            </tr>
                                            <tr>
                                                <td width="150">
                                                    Freisetzungsdatum bis:</td>
                                                <td class="StandardTableAlternate" valign="center">
                                                    <asp:TextBox ID="txtFreisetdatbis" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtFreisetdatbis_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtFreisetdatbis">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                 <asp:CompareValidator ID="cv_txtFreisetdatbis" runat="server" 
                                                ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txtFreisetdatbis"
                                                ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>                                                      
                                                 </td>
                                            </tr>
                                            <tr>
                                                <td width="130">
                                                    Verwendungszweck:</td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:DropDownList ID="ddlVerwendung" runat="server" Width="128px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            </table>
                                        <br />
                                    <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="'Zulassungsdatum von' kann darf nicht größer als 'Zulassungsdatum bis' sein!"
                                        Type="Date" ControlToValidate="txtZuldatumVon" ControlToCompare="txtZuldatumBis"
                                        Operator="LessThanEqual" Font-Bold="True"></asp:CompareValidator>
                                    <br /><asp:CompareValidator ID="CompareValidator6" runat="server" ErrorMessage="'Freisetzungsdatum von' kann darf nicht größer als 'Freisetzungsdatum bis' sein!"
                                        Type="Date" ControlToValidate="txtFreisetdatvon" ControlToCompare="txtFreisetdatBis"
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

