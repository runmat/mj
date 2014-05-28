<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change11.aspx.vb" Inherits="AppAvis.Change11" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>    
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
     <style type="text/css">
        .SelectTable tr td{
            padding: 3px 0px 5px 0px;            
            }
        .SearchTable tbody{
            border: solid 1px black;        
        }
        .SearchTable tr td{
            padding: 5px 0px 5px 10px;
            background-color: #cccccc;
            font-size: 14px;
            }      
    </style>  
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <uc1:BusyIndicator ID="BusyIndicator1" runat="server" />

    <form id="Form1" method="post" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
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
                            <asp:Label ID="lblPageTitle" runat="server"> (Auswahl)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <thead>
                                    <th class="TaskTitle" width="150">
                                        &nbsp;
                                    </th>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td valign="top" width="150">
                                            <asp:LinkButton ID="lbtnInsert" runat="server" CssClass="StandardButton"
                                                Width="120" Enabled="false">Anlegen</asp:LinkButton>
                                            <asp:LinkButton ID="lbtnChange" runat="server" CssClass="StandardButton"
                                                Width="120" Enabled="false">Ändern</asp:LinkButton>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="tblSelect" cellspacing="0" cellpadding="0" width="100%" border="0" class="SelectTable">
                                <thead>
                                    <tr>
                                        <th class="TaskTitle" valign="top" align="left" width="100%">
                                            &nbsp;
                                        </th>                                      
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td valign="top" align="left" style="padding-top:0;">
                                            <table id="tblSearch" cellspacing="0" cellpadding="0" class="SearchTable">
                                                <tbody>
                                                    <tr>
                                                        <td style="white-space: nowrap;">
                                                            Stationscode:
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0" >
                                                                <tr>
                                                                    <td style="padding-left: 0;">
                                                                        <telerik:RadTextBox ID="rtbStationscode" runat="server" Width="140px" MaxLength="12"
                                                                            Font-Size="14px" AutoPostBack="True">
                                                                        </telerik:RadTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCheckStationExists" runat="server" Text="Pr&uuml;fen" CssClass="StandardButton"
                                                                            BorderStyle="Solid" BorderWidth="1" BorderColor="Black" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="100%" height="100%" valign="top" rowspan="10">
                                                            <table id="tblAdresseSAP" runat="server">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblStationsnummer" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblName1" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblName2" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblStrasse" runat="server"></asp:Label>
                                                                        <asp:Label ID="lblHausnummer" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblPlz" runat="server"></asp:Label>
                                                                        <asp:Label ID="lblOrt" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblLand" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblTelefon" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblFax" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                       <asp:Label ID="lblÄnderung" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="trName1" runat="server">
                                                        <td style="white-space: nowrap;">
                                                            Name1:
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="rtbName1" runat="server" Width="300px" MaxLength="40" Font-Size="14px">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trName2" runat="server">
                                                        <td style="white-space: nowrap;">
                                                            Name2:
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="rtbName2" runat="server" Width="300px" MaxLength="40" Font-Size="14px">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trStrasse" runat="server">
                                                        <td style="white-space: nowrap;">
                                                            Straße/ Nr.:
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="rtbStrasse" runat="server" Width="240px" MaxLength="60" Font-Size="14px">
                                                            </telerik:RadTextBox>
                                                            <telerik:RadTextBox ID="rtbNummer" runat="server" Width="52px" MaxLength="10" Font-Size="14px">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trPLZ" runat="server">
                                                        <td style="white-space: nowrap;">
                                                            Plz./ Ort
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="rtbPLZ" runat="server" Width="60px" MaxLength="10" Font-Size="14px"></telerik:RadTextBox>                                                       
                                                            <telerik:RadTextBox ID="rtbOrt" runat="server" Width="232px" MaxLength="40" Font-Size="14px"></telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trLand" runat="server">
                                                        <td style="white-space:nowrap;">
                                                            Länderkürzel:
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="rtbLand" runat="server" MaxLength="3" Width="60px" Font-Size="14px"></telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trTelefon" runat="server">
                                                        <td style="white-space: nowrap;">
                                                            Telefon:
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="rtbTelefon" runat="server" Width="300px" MaxLength="30" Font-Size="14px">
                                                            </telerik:RadTextBox>                                                            
                                                        </td>
                                                    </tr>
                                                    <tr id="trFax" runat="server">
                                                        <td style="white-space: nowrap;">
                                                            Fax:
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="rtbFax" runat="server" Width="300px" MaxLength="30" Font-Size="14px">
                                                            </telerik:RadTextBox>                                                            
                                                        </td>
                                                    </tr>
                                                    <tr id="trEmail" runat="server">
                                                        <td style="white-space: nowrap;" valign="top">
                                                            E-Mail*:
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="rtbEmail" runat="server" Width="350px" Rows="5" TextMode="MultiLine" Font-Size="14px">
                                                            </telerik:RadTextBox><br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td valign="top">
                                                            <table cellpadding="0px" cellspacing="0">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <label style="color: #000000; font-size: 12px;">
                                                                            *</label>
                                                                    </td>
                                                                    <td valign="top">
                                                                        <label style="color: #000000; font-size: 12px;">Geben Sie bis zu 5 E-Mail Adressen durch Semikolon(;) getrennt ein.</label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                              
                                                            
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>                                       
                                    </tr>                                    
                                </tbody>
                            </table>                            
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label><br />
                            <asp:Label ID="lblSuccess" runat="server" CssClass="TextSuccess" EnableViewState="False"></asp:Label>
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