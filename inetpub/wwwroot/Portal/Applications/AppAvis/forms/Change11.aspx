<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change11.aspx.vb" Inherits="AppAvis.Change11" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>    
    <uc1:Styles ID="ucStyles" runat="server"/>
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
    <uc1:BusyIndicator ID="BusyIndicator1" runat="server"/>

    <form id="Form1" method="post" runat="server">

    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"/>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trSelection" runat="server">
                        <td valign="top" width="120">
                            <table bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120" border="0">
                                <tr>
                                    <td valign="top" width="150">
                                        <asp:LinkButton ID="lbtnStationen" runat="server" CssClass="StandardButton"
                                            Width="120">Stationen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="150">
                                        <asp:LinkButton ID="lbtnSpediteure" runat="server" CssClass="StandardButton"
                                            Width="120">Spediteure</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trEdit" runat="server" Visible="False">
                        <td valign="top" width="120">
                            <table bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120" border="0">
                                <tr>
                                    <td valign="top" width="150">
                                        <asp:LinkButton ID="lbtnInsert" runat="server" CssClass="StandardButton"
                                            Width="120" Enabled="false">Anlegen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="150">
                                        <asp:LinkButton ID="lbtnChange" runat="server" CssClass="StandardButton"
                                            Width="120" Enabled="false">Ändern</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="150">
                                        <asp:LinkButton ID="lbtnZurueck" runat="server" CssClass="StandardButton"
                                            Width="120">Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="tblSelect" cellspacing="0" cellpadding="0" width="100%" border="0" class="SelectTable">
                                <tr>
                                    <td valign="top" align="left" style="padding-top:0;">
                                        <table id="tblSearch" cellspacing="0" cellpadding="0" class="SearchTable">
                                            <tbody>
                                                <tr>
                                                    <td style="white-space: nowrap;">
                                                        <asp:Label ID="lblSearchFieldName" runat="server" Text="Stationscode:"/>
                                                    </td>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0" >
                                                            <tr>
                                                                <td style="padding-left: 0;">
                                                                    <asp:TextBox ID="txtStationscode" runat="server" Width="140px" MaxLength="12"
                                                                        Font-Size="14px" style="text-transform: uppercase" AutoPostBack="True"/>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCheckStationExists" runat="server" Text="Pr&uuml;fen" CssClass="StandardButton"
                                                                        BorderStyle="Solid" BorderWidth="1" BorderColor="Black"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblStationsnummer" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap;">
                                                        Name1:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName1" runat="server" Width="300px" MaxLength="40" Font-Size="14px"/>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblName1" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap;">
                                                        Name2:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName2" runat="server" Width="300px" MaxLength="40" Font-Size="14px"/>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblName2" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap;">
                                                        Straße/ Nr.:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtStrasse" runat="server" Width="240px" MaxLength="60" Font-Size="14px"/>
                                                        <asp:TextBox ID="txtNummer" runat="server" Width="52px" MaxLength="10" Font-Size="14px"/>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblStrasse" runat="server"/>
                                                        &nbsp;
                                                        <asp:Label ID="lblHausnummer" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap;">
                                                        Plz./ Ort
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPLZ" runat="server" Width="60px" MaxLength="10" Font-Size="14px"/>                                                       
                                                        <asp:TextBox ID="txtOrt" runat="server" Width="232px" MaxLength="40" Font-Size="14px"/>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPlz" runat="server"/>
                                                        &nbsp;
                                                        <asp:Label ID="lblOrt" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space:nowrap;">
                                                        Länderkürzel:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLand" runat="server" MaxLength="3" Width="60px" Font-Size="14px"/>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLand" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap;">
                                                        Telefon:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTelefon" runat="server" Width="300px" MaxLength="30" Font-Size="14px"/>                                                            
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTelefon" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap;">
                                                        Fax:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFax" runat="server" Width="300px" MaxLength="30" Font-Size="14px"/>                                                            
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFax" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td rowspan="5" style="white-space: nowrap;" valign="top">
                                                        E-Mail*:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmail1" runat="server" Width="350px" Font-Size="14px"/>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEmail1" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtEmail2" runat="server" Width="350px" Font-Size="14px"/>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEmail2" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtEmail3" runat="server" Width="350px" Font-Size="14px"/>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEmail3" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtEmail4" runat="server" Width="350px" Font-Size="14px"/>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEmail4" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtEmail5" runat="server" Width="350px" Font-Size="14px"/>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEmail5" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span style="font-size: 12px">*bis zu 5 Adressen</span>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblÄnderung" runat="server"/>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>                                       
                                </tr>                                    
                            </table>                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"/>
                            <br />
                            <asp:Label ID="lblSuccess" runat="server" CssClass="TextSuccess" EnableViewState="False"/>
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