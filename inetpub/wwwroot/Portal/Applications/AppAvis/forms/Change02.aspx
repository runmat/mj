<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02.aspx.vb" Inherits="AppAvis.Change02" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <script language="JavaScript"  type="text/javascript" src="../Javascript/Slideup_Down.js"></script>
    <style type="text/css">
      
        .style1
        {
            width: 50%;
        }
      
        .style2
        {
            width: 151px;
        }
      
        .style4
        {
            width: 140px;
        }
        .style5
        {
            width: 140px;
            height: 16px;
        }
      
        .style6
        {
            width: 35px;
        }
      
        .style7
        {
            height: 11px;
        }
      
    </style>
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
                            <asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="2">
                            &nbsp;&nbsp;</td>
                    </tr>
                    <tr>
                        <td  valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td valign="middle" width="150">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"> •&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" width="150">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" width="150">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                        <td  valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="75%"  border="0" >                               
                             <tr runat="server" id="ZulkreisSel">
                                    <td valign="top" class="style1" >
                                      <table id="Table9" class="BorderLeftBottom" cellspacing="0" cellpadding="0" width="100%" border="0" >
                                      <tr>
                                        <td>
                                            <asp:RadioButton ID="rbWI" runat="server" Checked="True" GroupName="Ort" 
                                                Text="WI" />
                                        </td>
                                          <td>
                                        </td>
                                      </tr>   
                                      <tr>
                                        <td width="25%">
                                            <asp:RadioButton ID="rbDez" runat="server" GroupName="Ort" Text="Dezentral:" />
                                        </td>
                                          <td align="left">
                                           &nbsp;
                                           <asp:DropDownList ID="ddlZulKreis" runat="server">
                                                               </asp:DropDownList></td></tr> 
                                      <tr>
                                        <td >
                                            &nbsp;</td>
                                          <td align="right">
                                            <asp:LinkButton ID="lbtn_Weiter" runat="server" CssClass="ButtonUp"> •&nbsp;Weiter</asp:LinkButton></td>
                                         </tr>
                                      </table>
                                    </td>
                                    
                                    <td  valign="top" class="style6">
                                        &nbsp;</td>
                                    
                                    <td  valign="top">
                                        &nbsp;</td>
                                    <td  valign="top">
                                        </td>   
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                            <asp:LinkButton ID="lbtn_backZul" runat="server" Visible="false" 
                                                CssClass="ButtonUp" Width="100%"></asp:LinkButton></td>
                                    <td valign="top" align="left" class="style6">
                                        &nbsp;</td>
                                    <td valign="top" align="left">
                                        &nbsp;</td>
                                    <td valign="top" align="right">

                                        &nbsp;</td>
                                </tr>
                                <tr runat="server" id="SelectionRow" visible="false" >
                                    <td valign="top" align="left" class="style1" >
                                        <table id="Table7" cellspacing="0" cellpadding="0" border="0"  width="100%">
                                            <tr>
                                                <td valign="top">
                                                <div id="Suche1" 
                                                        style="height:235px; border-color:#CC0033; width:100%; background:#FFFFCC; border: groove;">
                                                    <table  id="Table8" cellspacing="1" cellpadding="1"  width="50%">
                                                        <tr>
                                                            <td class="style5">
                                                                Carport:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCarports" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap class="style5">
                                                                Hersteller:
                                                            </td>
                                                            <td >
                                                                <asp:DropDownList ID="ddlHersteller" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                Liefermonat:
                                                            </td>
                                                            <td >
                                                                <asp:DropDownList ID="ddlLiefermonat" runat="server" AutoPostBack="True" 
                                                                    style="height: 22px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap class="style5">
                                                                Bereifung:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlBereifung" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                Getriebe:
                                                            </td>
                                                            <td >
                                                                <asp:DropDownList ID="ddlGetriebe" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5" >
                                                                Kraftstoffart:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlKraftstoff" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                Navi:</td>
                                                            <td >
                                                                <asp:DropDownList ID="ddlNavi" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td bgcolor="#FFFFCC" class="style5">
                                                                Farbe</td>
                                                            <td class="style2">
                                                                <asp:DropDownList ID="ddlFarbe" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                Vermietgruppe:</td>
                                                            <td >
                                                                <asp:DropDownList ID="ddlVermiet" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        </table>
                                                 </div>
                                                </td>
                                            </tr>
                                            
                                        </table>

                                    </td>
                                    <td valign="top" align="left" class="style6">
                                        &nbsp;</td>
                                    <td valign="top" align="left">
                                    <table id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
                                            <tr>
                                                <td valign="top">
                                                <div id="Suche2" 
                                                        style=" display:none; overflow:hidden;  width:100%; height:235px; background:#FFFFCC; border: groove;" >
                                                    <table id="Table3" cellspacing="1" cellpadding="1" width="100%">
                                                        <tr>
                                                            <td class="style4">
                                                                Fahrzeugart:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlFzgArt" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap class="style4">
                                                                Aufbauart:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlAufbauArt" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style4">
                                                                Händler-Nr.:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlHaendlernr" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap class="style4">
                                                                Händler Kurzname:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlHaendlername" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style4">
                                                                Einkaufsindikator:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlEKIndikator" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style4">
                                                                Verwendungszweck:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlVerwZweck" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style4">
                                                                Owner Code:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOwnerCode" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style4">
                                                                Sperre bis:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlSperrdat" runat="server" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style4">
                                                                &nbsp;&nbsp;</td>
                                                            <td>&nbsp;&nbsp;</td>
                                                        </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                    </td>
                                    <td valign="top" align="right">

                                    </td>
                                </tr>
                                <tr ID="ButtonRow" runat="server" visible="false">
                                    <td valign="top"  class="style1">
                                    <table id="Table5" cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tr>
                                        <td align="left">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="ButtonUp"> •&nbsp;Suchen</asp:LinkButton>
                                        </td>
                                     <td align="right"> 
                                        <a href="javascript:;" id="Down"  class="ButtonUp" onmousedown="slidedown('Suche2');">Weitere Kriterien</a>                                        
                                    </td>
                                                                           
                                    </tr>
                                    </table>

                                                            </td>
                                    <td valign="top" align="left" class="style6">
                                        &nbsp;</td>
                                    <td valign="top" align="left">
                                      <a href="javascript:;" id="UP" style=" display:none;" class="ButtonUp" onmousedown="slideup('Suche2');">Schließen</a>
                                    </td>
                                    <td valign="top" align="right">
                                        </td>
                                </tr>
                                </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="style7">
                            &nbsp;</td>
                        <td valign="top" align="left" class="style7">
                                        &nbsp;&nbsp;</td>
                    </tr>
                    <tr>
                        <td valign="top" class="style7">
                            </td>
                        <td valign="top" align="left" class="style7">
                            <asp:Label ID="lblMessage" runat="server" CssClass="TextLarge" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
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
                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <INPUT id="SelOpen2" type="hidden" size="1" runat="server" />
    </form>
</body>
</html>

