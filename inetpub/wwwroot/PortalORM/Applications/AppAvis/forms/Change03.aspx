<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03.aspx.vb" Inherits="AppAvis.Change03" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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

<script language="JavaScript" type="text/javascript">
    function openinfo(url) {
        fenster = window.open(url, "AVIS", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=500,height=200");
        fenster.focus();
    }
</script>

<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    
    <uc1:BusyIndicator runat="server" />

    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager runat="server" ID="Scriptmanager1" EnableScriptLocalization="true"
        EnableScriptGlobalization="true">
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
                    <tbody>
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TaskTitle" valign="top" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="120">
                                <table id="Table2" cellspacing="0" cellpadding="0" bgcolor="white" border="0" runat="server">
                                    <tr id="trCreate" runat="server">
                                        <td valign="center" width="120">
                                            <asp:LinkButton ID="lb_Weiter" Text="Weiter" runat="server" 
                                                CssClass="StandardButton"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="center" width="120">
                                            <asp:LinkButton ID="lbBack" runat="server" CssClass="StandardButton" CausesValidation="false"> •&nbsp;Zurück</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="100">
                            </td>
                            <td>
                                <table id="Table3" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0"
                                   >
                                    <tr runat="server" id="tr_Auswahl">
                                        <td valign="top" colspan="1" >
                                            <table id="Table9" cellspacing="0" cellpadding="0" width="25%"
                                                border="0" style="border-color: #dfdfdf; border-style: solid; border-width: 1;">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbWI" runat="server" Checked="True" GroupName="Ort" Text="WI" />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td width="25%">
                                                        <asp:RadioButton ID="rbDez" runat="server" GroupName="Ort" Text="Dezentral:" />
                                                    </td>
                                                            </tr>
                                                </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">
                                             &nbsp;
                                        </td>
                                    </tr>                                     
                                    <tr  runat="server" id="tr_Text" visible="false">
                                        <td colspan="1" align="left" class="TaskTitle">
                                            <asp:Label ID="Label1" Text="Einzelzulassung" runat="server"></asp:Label>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td colspan="2" style="border-color: #f5f5f5; border-style: solid; border-width: 3;">
                                            <table id="tbl_Query" cellspacing="0" cellpadding="5" width="100%" border="0" runat="server" >
                                                <tr>
                                                    <td class="TextLarge" nowrap align="left">
                                                        Fahrgestellnummer:
                                                    </td>
                                                    <td class="TextLarge" width="100%">
                                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" MaxLength="17"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="StandardTableAlternate" nowrap align="left">
                                                        MVA-Nummer:
                                                    </td>
                                                    <td class="StandardTableAlternate">
                                                        <asp:TextBox ID="txtMVANummer" runat="server" MaxLength="8"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextLarge" nowrap align="left">
                                                        Zulassungsdatum:
                                                    </td>
                                                    <td class="TextLarge">
                                                        <asp:TextBox ID="txtZulassungsdatum" ToolTip="Zulassungsdatum" runat="server"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtZulassungsdatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                            PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtZulassungsdatum">
                                                        </cc1:CalendarExtender>
                                                        <cc1:MaskedEditExtender ID="meeZulassungsdatum" runat="server" TargetControlID="txtZulassungsdatum"
                                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                        </cc1:MaskedEditExtender>
                                                        <cc1:MaskedEditValidator ID="mevZulassungsdatum" runat="server" ControlToValidate="txtZulassungsdatum"
                                                            ControlExtender="meeZulassungsdatum" Display="none" IsValidEmpty="True" Enabled="true"
                                                            EmptyValueMessage="Bitte geben Sie ein gültiges Zulassungsdatum ein" InvalidValueMessage="Bitte geben Sie ein gültiges Zulassungsdatum ein">
                                                         
                                                                                                                     
                                                        </cc1:MaskedEditValidator>
                                                        <cc1:ValidatorCalloutExtender Enabled="true" ID="vceZulassungsdatum" Width="350px"
                                                            runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="mevZulassungsdatum">
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
                            <td width="100">
                            </td>
                            <td valign="top">
                                <table id="tbl_Upload" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0"
                                    runat="server" >
                                    <tr>
                                        <td colspan="1" align="left" class="TaskTitle">
                                            <asp:Label ID="lbl_Info" Text="Zulassungs-Excel-Datei-Auswahl" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border-color: #f5f5f5; border-style: solid; border-width: 3;">
                                            <table id="tbl0001" cellspacing="0" cellpadding="5" width="100%" border="0">
                                                <tr>
                                                    <td class="TextLarge" nowrap align="right">
                                                        Dateiauswahl <a href="javascript:openinfo('Info01.htm');">
                                                            <img src="../../../images/fragezeichen.gif" border="0"></a>:&nbsp;&nbsp;
                                                    </td>
                                                    <td class="TextLarge" width="100%">
                                                        <input id="upFile" type="file" size="49" name="File1" runat="server">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextLarge" nowrap align="right">
                                                        &nbsp;
                                                    </td>
                                                    <td class="TextLarge">
                                                        &nbsp;
                                                        <asp:Label ID="lblExcelfile" runat="server" EnableViewState="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="100">
                                &nbsp;
                            </td>
                            <td align="left" colspan="2">
                                &nbsp;
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
    </form>
</body>
</html>
