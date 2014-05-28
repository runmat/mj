<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02.aspx.vb" Inherits="AppCommonCarRent.Report02" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <uc1:BusyIndicator runat="server" />
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
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
                            <asp:Label ID="lbl_PageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        <asp:LinkButton ID="lb_zurueck" Visible="True" runat="server">lb_zurueck</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="lb_weiter" runat="server" CssClass="StandardButton" OnClientClick="Show_BusyBox1();"
                                            Visible="True"> Erstellen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" width="100%">
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
                                                <td class="TextLarge" valign="center" width="150">
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150">
                                                    <asp:Label ID="lbl_DatumVon" runat="server"></asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ToolTip="Datum von" ID="txtDatumVon" Width="80px" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtDatumVon_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                        PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtDatumVon">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="meeZulassungsdatum" runat="server" TargetControlID="txtDatumVon"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </cc1:MaskedEditExtender>
                                                    <cc1:MaskedEditValidator ID="mevZulassungsdatum" runat="server" ControlToValidate="txtDatumVon"
                                                        ControlExtender="meeZulassungsdatum" Display="none" IsValidEmpty="True" Enabled="True"
                                                        EmptyValueMessage="Bitte geben Sie ein gültiges Datum ein" InvalidValueMessage="Bitte geben Sie ein gültiges Datum ein">                                                                      
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </cc1:MaskedEditValidator>
                                                    <cc1:ValidatorCalloutExtender Enabled="true" ID="vceZulassungsdatum" Width="350px"
                                                        runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="mevZulassungsdatum">
                                                    </cc1:ValidatorCalloutExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150">
                                                    <asp:Label ID="lbl_DatumBis" runat="server"></asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ToolTip="Datum bis" ID="txtDatumBis" Width="80px" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtDatumBis_CalenderExtender1" runat="server" Format="dd.MM.yyyy"
                                                        PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtDatumBis">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDatumBis"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </cc1:MaskedEditExtender>
                                                    <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtDatumBis"
                                                        ControlExtender="meeZulassungsdatum" Display="none" IsValidEmpty="True" Enabled="True"
                                                        EmptyValueMessage="Bitte geben Sie ein gültiges Datum ein" InvalidValueMessage="Bitte geben Sie ein gültiges Datum ein">
                                                                      
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                      
                                                    </cc1:MaskedEditValidator>
                                                    <cc1:ValidatorCalloutExtender Enabled="true" ID="ValidatorCalloutExtender1" Width="350px"
                                                        runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="MaskedEditValidator1">
                                                    </cc1:ValidatorCalloutExtender>
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
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td>
                           
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
