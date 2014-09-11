<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04.aspx.vb" Inherits="AppARVAL.Change04" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style8
        {
            width: 438px;
        }
        .style9
        {
            width: 154px;
        }
        .style11
        {
            text-decoration: underline;
            font-weight: bold;
        }
    </style>
</head>
<body>
    
    <uc1:BusyIndicator runat="server" />

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ScriptMode="Release">
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
                            <asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="120">
                                        <asp:LinkButton ID="cmdCreate" runat="server" OnClientClick="Show_BusyBox1();" CssClass="StandardButton" Width="120px"> •&nbsp;Suche</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="120">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton" Visible="False"
                                            Width="120px"> •&nbsp;Speichern</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="120">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton" Width="120px"> •&nbsp;Zurück</asp:LinkButton>
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
                                                <td class="TextLarge" valign="center" width="150" height="31">
                                                    Vertragsnummer:
                                                </td>
                                                <td valign="center" height="31">
                                                    <asp:TextBox ID="txtVertragsnummer" runat="server" MaxLength="7"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" valign="center" width="150" height="31">
                                                    Kfz-Kennzeichen:&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate" valign="center" height="31">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server"></asp:TextBox>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td valign="center">
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" MaxLength="17"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <br>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        
                                    <table id="tblAdressenAnzeige" runat="server" visible="false" cellspacing="0" cellpadding="5"
                                            width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td class="TaskTitle">
                                                    Details
                                                </td>
                                                <td class="TaskTitle" colspan="4">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge">
                                                    Vetragsnummer:</td>
                                                <td>
                                                    <asp:Label ID="lblVetragsnummer" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" >
                                                    Kfz-Kennzeichen:
                                                </td>
                                                <td class="StandardTableAlternate" colspan="3">
                                                    <asp:Label ID="lblKennzeichen" runat="server"></asp:Label>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" width="150">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFahrgestellnummer" runat="server" MaxLength="17"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate">
                                                    <span class="style11">Halter</span>
                                                </td>
                                                <td class="StandardTableAlternate">
                                                
                                                </td>
                                                <td width="150" class="StandardTableAlternate" colspan="2">
                                                    <span class="style11">Leasingnehmer</span>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge">
                                                    Name1:
                                                </td>
                                                <td class="style8">
                                                    <asp:Label ID="lblHalterName1" runat="server"></asp:Label>
                                                </td>
                                                <td class="TextLarge" width="150">
                                                    Name1:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblName1" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate">
                                                    
                                                </td>
                                                <td class="StandardTableAlternate">
                                                   
                                                </td>
                                                <td class="StandardTableAlternate" width="150">
                                                    Name2:
                                                </td>
                                                <td class="StandardTableAlternate">
                                                    <asp:Label ID="lblName2" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style9">
                                                    &nbsp;
                                                </td>
                                                <td class="style8">
                                                    &nbsp;
                                                </td>
                                                <td class="TextLarge" width="150">
                                                    Strasse:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblStrasse" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate">
                                                    Ort:
                                                </td>
                                                <td class="StandardTableAlternate" style="font-weight: normal">
                                                    <asp:Label ID="lblHalterOrt" runat="server"></asp:Label>
                                                </td>
                                                <td class="StandardTableAlternate" width="150">
                                                    PLZ und Ort:
                                                </td>
                                                <td class="StandardTableAlternate" style="font-weight: normal">
                                                    <asp:Label ID="lblPlzOrt" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td valign="top" align="left">
                                     <table id="tblEdit" runat="server" visible="false" cellspacing="0" cellpadding="5"
                                            width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td class="TaskTitle" valign="top" align="left" colspan="2">
                                                    Vertragsdaten ändern</td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150" height="31">
                                                    Neue
                                                    Vertragsnummer:</td>
                                                <td class="TextLarge" valign="center" height="31">
                                                    <asp:TextBox ID="txtNewVertragsnummer" runat="server" MaxLength="7"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150" height="31">
                                                    Neuer
                                                    Leasingnehmer:</td>
                                                <td class="TextLarge" valign="center" height="31">
                                                    <div>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtLeasingnehmer" runat="server" Width="650px" AutoPostBack="true"></asp:TextBox>
                                                                <cc1:TextBoxWatermarkExtender ID="txtLeasingnehmer_TextBoxWatermarkExtender" 
                                                                    runat="server" Enabled="True" TargetControlID="txtLeasingnehmer" 
                                                                    WatermarkCssClass="watermarked" 
                                                                    WatermarkText="Geben Sie mindestens 3 Zeichen ein um eine Auswahlliste zu erhalten.">
                                                                </cc1:TextBoxWatermarkExtender>
                                                                <cc1:AutoCompleteExtender ID="txtLeasingnehmer_AutoCompleteExtender" runat="server"
                                                                    DelimiterCharacters="" Enabled="True" ServiceMethod="GetAdressList" ServicePath="ArvalService.asmx"
                                                                    TargetControlID="txtLeasingnehmer" UseContextKey="True">
                                                                </cc1:AutoCompleteExtender>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtLeasingnehmer" EventName="TextChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                    
                                   
                                    
                                    </td>
                                
                                </tr>
                                
                                <tr>
                                    <td valign="top" align="left">
                                       
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
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                            <asp:Label ID="lblInfo" runat="server" EnableViewState="False" 
                                ForeColor="#0033CC" Visible="False"></asp:Label>
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
