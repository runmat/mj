<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02.aspx.vb" Inherits="AppCommonCarRent.Change02" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
    <meta content="JavaScript" name="vs_defaultClientScript"/>
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        #Table1
        {
        }
        .style1
        {
            height: 38px;
        }
        .style2
        {
            width: 150px;
        }
        </style>
</head>
<body >
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
                </asp:ScriptManager>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td class="PageNavigation"  colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2" valign="top">
                         <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" 
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="lb_weiter" runat="server" CssClass="StandardButton" Height="16px"
                                            Width="120px"> •&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="lb_Back" runat="server" CssClass="StandardButton" Visible="False"
                                            Height="16px" Width="120px" CausesValidation="False"> •&nbsp; Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        &nbsp;</td>
                                </tr>
                            </table>                                
                                
                            </td>
                            <td  valign="top">
                                <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white"
                                    border="0">
                                    <tr  class="TextLarge">
                                        <td align="left" class="TaskTitle" nowrap="nowrap">
                                            &nbsp;</td>
                                        <td align="left" class="TaskTitle" width="100%">
                                            &nbsp;</td>
                                    </tr>
                                    <tr  class="TextLarge">
                                        <td align="left" nowrap="nowrap">
                                            &nbsp;</td>
                                        <td align="left" width="100%">
                                            &nbsp;</td>
                                    </tr>
                                    <tr id="tr_Leasingvertragsnummer" class="TextLarge">
                                        <td align="left" nowrap="nowrap">
                                            <asp:Label ID="lbl_Haendlernr" runat="server"></asp:Label>
                                        </td>
                                        <td align="left" width="100%">
                                            <asp:TextBox ID="txtHaendlernr" runat="server" CssClass="TextBoxNormal" 
                                                Width="183px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="tr_Kennzeichen" class="StandardTableAlternate">
                                        <td align="left" nowrap="nowrap">
                                            <asp:Label ID="lbl_Kennzeichen" runat="server"></asp:Label>
                                            *</td>
                                        <td align="left" width="100%">
                                            <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" 
                                                Width="183px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="tr_Fahrgestellnummer" class="TextLarge">
                                        <td align="left" nowrap="nowrap">
                                            <asp:Label ID="lbl_Fahrgestellnummer" runat="server"></asp:Label>
                                            *</td>
                                        <td align="left" width="100%">
                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" 
                                                Width="183px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="tr_RetourDateVon" class="StandardTableAlternate" >
                                        <td align="left" nowrap="nowrap">
                                            <asp:Label ID="lbl_RetourDateVon" runat="server"></asp:Label>
                                        </td>
                                        <td align="left" width="100%">
                                            <asp:TextBox ID="txt_RetourDateVon" runat="server" Width="183px"></asp:TextBox>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                                ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txt_RetourDateVon"
                                                ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>                                            

                                            <cc1:CalendarExtender ID="txt_RetourDateVon_CalendarExtender" runat="server" 
                                                TargetControlID="txt_RetourDateVon">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr id="tr_RetourDateBis" class="TextLarge">
                                        <td align="left" nowrap="nowrap" class="style1">
                                            <asp:Label ID="lbl_RetourDateBis" runat="server"></asp:Label>
                                        </td>
                                        <td align="left" width="100%" class="style1">
                                            <asp:TextBox ID="txt_RetourDateBis" runat="server" Width="183px"></asp:TextBox>
                                            <asp:CompareValidator ID="CompareValidator2" runat="server" 
                                                ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txt_RetourDateBis"
                                                ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>                                            
                                            
                                            <cc1:CalendarExtender ID="txt_RetourDateBis_CalendarExtender" runat="server" 
                                                TargetControlID="txt_RetourDateBis" >
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr id="tr_AbmeldeDateVon" class="StandardTableAlternate">
                                        <td align="left" nowrap="nowrap">
                                            <asp:Label ID="lbl_AbmeldeDateVon" runat="server"></asp:Label>
                                        </td>
                                        <td align="left" width="100%">
                                            <asp:TextBox ID="txt_AbmeldeDateVon" runat="server" Width="183px"></asp:TextBox>
                                            <asp:CompareValidator ID="CompareValidator3" runat="server" 
                                                ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txt_AbmeldeDateVon"
                                                ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>                                             
                                            <cc1:CalendarExtender ID="txt_AbmeldeDateVon_CalendarExtender" runat="server" 
                                                TargetControlID="txt_AbmeldeDateVon">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr id="tr_AbmeldeDateBis" class="TextLarge">
                                        <td align="left" nowrap="nowrap">
                                            <asp:Label ID="lbl_AbmeldeDateBis" runat="server"></asp:Label>
                                        </td>
                                        <td align="left" width="100%">
                                            <asp:TextBox ID="txt_AbmeldeDateBis" runat="server" Width="183px"></asp:TextBox>
                                            <asp:CompareValidator ID="CompareValidator4" runat="server" 
                                                ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txt_AbmeldeDateBis"
                                                ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>                                            
                                            <cc1:CalendarExtender ID="txt_AbmeldeDateBis_CalendarExtender" runat="server" 
                                                TargetControlID="txt_AbmeldeDateBis">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr id="tr_Bezahlt" class="StandardTableAlternate">
                                        <td align="left" nowrap="nowrap">
                                            <asp:Label ID="lbl_Bezahlt" runat="server"></asp:Label>
                                        </td>
                                        <td align="left" width="100%" nowrap="nowrap">
                                            <asp:RadioButtonList ID="rbl_Bezahlt" runat="server" 
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="0" Selected="True">Alle</asp:ListItem>
                                                <asp:ListItem Value="1">Ja</asp:ListItem>
                                                <asp:ListItem Value="2">Nein</asp:ListItem>
                                            </asp:RadioButtonList>

                                        </td>
                                    </tr>
                                    <tr id="tr_Gesperrt" class="TextLarge">
                                        <td align="left" nowrap="nowrap">
                                            <asp:Label ID="lbl_Gesperrt" runat="server"></asp:Label>
                                        </td>
                                        <td align="left" width="100%" nowrap="nowrap">
                                            <asp:RadioButtonList ID="rbl_Gesperrt" runat="server" 
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="0" Selected="True">Alle</asp:ListItem>
                                                <asp:ListItem Value="1">Ja</asp:ListItem>
                                                <asp:ListItem Value="2">Nein</asp:ListItem>
                                            </asp:RadioButtonList>

                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td colspan="2">
                                            <asp:ImageButton ID="btnEmpty" runat="server"  ImageUrl="../../../images/empty.gif"  Height="1px" />
                                            <asp:Label ID="lbl_Info" CssClass="TextLarge" Text="Alle Eingaben mit mehrfacher Platzhalter-Suche (*) möglich (z.B. 'F*23Z*1*')"
                                                runat="server">Alle Eingaben mit mehrfacher Platzhalter-Suche (*) möglich (z.B. 'F*23Z*1*')</asp:Label>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="'Retourdatum von' kann darf nicht größer als 'Retourdatum bis' sein!"
                    Type="Date" ControlToValidate="txt_RetourDateVon" ControlToCompare="txt_RetourDateBis"
                    Operator="LessThanEqual" Font-Bold="True"></asp:CompareValidator>
                <br /><asp:CompareValidator ID="CompareValidator6" runat="server" ErrorMessage="'Abmeldung von' kann darf nicht größer als 'Abmeldung bis' sein!"
                    Type="Date" ControlToValidate="txt_AbmeldeDateVon" ControlToCompare="txt_AbmeldeDateBis"
                    Operator="LessThanEqual" Font-Bold="True"></asp:CompareValidator>

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap">
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" >
                &nbsp;<asp:TextBox id="TextBox2" runat="server" Visible="false"/>

                </td>
        </tr>
        <tr>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
