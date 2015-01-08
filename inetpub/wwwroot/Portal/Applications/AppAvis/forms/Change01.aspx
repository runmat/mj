<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01.aspx.vb" Inherits="AppAvis.Change01" %>
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
        #Table2
        {
            width: 131px;
        }
          
        .style1
        {
            width: 50%;
        }
      
        .style6
        {
            width: 35px;
        }
      
        .style5
        {
            width: 140px;
            height: 16px;
        }
      
        .style2
        {
            width: 151px;
        }
      
        .style4
        {
            width: 140px;
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
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"> •&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:Calendar ID="calVon" runat="server" BackColor="White" BorderColor="#999999"
            CellPadding="1" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Width="120px" 
                                Visible="False">
                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <SelectorStyle BackColor="#CCCCCC" />
                                <WeekendDayStyle BackColor="#FFFFCC" />
                                <OtherMonthDayStyle ForeColor="#808080" />
                                <NextPrevStyle VerticalAlign="Bottom" />
                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                            </asp:Calendar>
                            <asp:Calendar ID="calBis" runat="server" BackColor="White" BorderColor="#999999"
            CellPadding="1" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Width="120px" 
                                Visible="False">
                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <SelectorStyle BackColor="#CCCCCC" />
                                <WeekendDayStyle BackColor="#FFFFCC" />
                                <OtherMonthDayStyle ForeColor="#808080" />
                                <NextPrevStyle VerticalAlign="Bottom" />
                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                            </asp:Calendar>
                            <asp:Calendar ID="calBisBereit" runat="server" BackColor="White" BorderColor="#999999"
            CellPadding="1" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Width="120px" 
                                Visible="False">
                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <SelectorStyle BackColor="#CCCCCC" />
                                <WeekendDayStyle BackColor="#FFFFCC" />
                                <OtherMonthDayStyle ForeColor="#808080" />
                                <NextPrevStyle VerticalAlign="Bottom" />
                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                            </asp:Calendar>
                            <asp:Calendar ID="calVonBereit" runat="server" BackColor="White" BorderColor="#999999"
            CellPadding="1" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Width="120px" 
                                Visible="False">
                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <SelectorStyle BackColor="#CCCCCC" />
                                <WeekendDayStyle BackColor="#FFFFCC" />
                                <OtherMonthDayStyle ForeColor="#808080" />
                                <NextPrevStyle VerticalAlign="Bottom" />
                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                            </asp:Calendar>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="right">
                                        <table id="Table7" cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                        <td  valign="top">
                            <table id="Table8" cellspacing="0" cellpadding="0" width="75%"  border="0" >                               
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
                                <tr runat="server" id="SelectionRow" >
                                    <td valign="top" align="left" class="style1" >
                                        <table id="Table9" cellspacing="0" cellpadding="0" border="0"  width="100%">
                                            <tr>
                                                <td valign="top">
                                                <div id="Suche1" 
                                                        style="border-color:#CC0033; width:100%; background:#FFFFCC; border: groove;">
                                                    <table  id="Table10" cellspacing="1" cellpadding="1"  width="50%">
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
                                                            <td nowrap>
                                                                Eingangsdatum von:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtEingangsdatumVon"  runat="server" Width="80px" 
                                                                    Enabled="False" Height="20px" Wrap="False"></asp:TextBox>
                                                                &nbsp;<asp:ImageButton ID="btnCal1" runat="server" ImageUrl="../../../Images/calendar_red.jpg" />
                                                                 &nbsp;<asp:ImageButton ID="ibtnDelEingVon" runat="server" 
                                                                    ImageUrl="../../../Images/loesch.gif" Height="16px" Width="16px" 
                                                                    ToolTip="Datum entfernen!" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Eingangsdatum bis:
                                                            </td>
                                                            <td valign="middle">
                                                                <asp:TextBox ID="txtEingangsdatumBis" runat="server" Width="80px" 
                                                                    Enabled="False" Height="20px" Wrap="False"></asp:TextBox>
                                                                &nbsp;<asp:ImageButton ID="btnCal2" runat="server" 
                                                                    ImageUrl="../../../Images/calendar_red.jpg" ImageAlign="AbsMiddle" />
                                                            &nbsp;<asp:ImageButton ID="ibtnDelEingBis" runat="server" 
                                                                    ImageUrl="../../../Images/loesch.gif" Height="16px" Width="16px" 
                                                                    ToolTip="Datum entfernen!" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap>
                                                                Bereitmeldung von:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMeldungsdatumVon" runat="server" Width="80px" 
                                                                    Enabled="False" Height="20px"></asp:TextBox>
                                                                &nbsp;<asp:ImageButton ID="btnCalBereit2" runat="server" 
                                                                    ImageUrl="../../../Images/calendar_red.jpg" ImageAlign="AbsMiddle" />
                                                            &nbsp;<asp:ImageButton ID="ibtnDelBereitVon" runat="server" 
                                                                    ImageUrl="../../../Images/loesch.gif" Height="16px" Width="16px" 
                                                                    ToolTip="Datum entfernen!" />
                                                            </td>
                                                        </tr>
                                                        <tr valign="middle">
                                                            <td>
                                                                Bereitmeldung bis:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMeldungsdatumBis" runat="server" Width="80px" 
                                                                    Enabled="False" Height="20px"></asp:TextBox>
                                                                &nbsp;<asp:ImageButton  ID="btnCalBereit1" runat="server" 
                                                                    ImageUrl="../../../Images/calendar_red.jpg" Width="16px" ImageAlign="AbsMiddle" />
                                                            &nbsp;<asp:ImageButton ID="ibtnDelBereitBis" runat="server" 
                                                                    ImageUrl="../../../Images/loesch.gif" Height="16px" Width="16px" 
                                                                    ToolTip="Datum entfernen!" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Vorlage ZBII:
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdo_Alle" runat="server" AutoPostBack="True" Checked="True"
                                                                    GroupName="Erledigt" Text="Alle" />
                                                                &nbsp;<asp:RadioButton ID="rdo_Ja" runat="server" AutoPostBack="True" GroupName="Erledigt"
                                                                    Text="Ja" />
                                                                &nbsp;<asp:RadioButton ID="rdo_Nein" runat="server" AutoPostBack="True" GroupName="Erledigt"
                                                                    Text="Nein" />
                                                            </td>
                                                            </tr>
                                                            <tr>
                                                            <td>
                                                                Gesperrt:
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdo_AlleSperr" runat="server" AutoPostBack="True" Checked="True"
                                                                    GroupName="Sperre" Text="Alle" />
                                                                &nbsp;<asp:RadioButton ID="rdo_JaSperr" runat="server" AutoPostBack="True" GroupName="Sperre"
                                                                    Text="Ja" />
                                                                &nbsp;<asp:RadioButton ID="rdo_NeinSperr" runat="server" AutoPostBack="True" GroupName="Sperre"
                                                                    Text="Nein" />
                                                            </td>
                                                            </tr>
                                                            <tr>
                                                            <td>
                                                                Zulassungsbereit:</td>
                                                             <td>
                                                                 <asp:RadioButton ID="rdo_AlleBereit" runat="server" AutoPostBack="True" Checked="True"
                                                                     GroupName="Bereit" Text="Alle" />
                                                                 &nbsp;<asp:RadioButton ID="rdo_JaBereit" runat="server" AutoPostBack="True" GroupName="Bereit"
                                                                     Text="Ja" />
                                                                 &nbsp;<asp:RadioButton ID="rdo_NeinBereit" runat="server" AutoPostBack="True" GroupName="Bereit"
                                                                     Text="Nein" />
                                                             </td>
                                                            </tr>
                                                            <tr>
                                                            <td>
                                                                Planzulassung:</td>
                                                             <td>
                                                                 <asp:RadioButton ID="rdo_AllePlanzulassung" runat="server" AutoPostBack="True" Checked="True"
                                                                     GroupName="Planzulassung" Text="Alle" />
                                                                 &nbsp;<asp:RadioButton ID="rdo_JaPlanzulassung" runat="server" AutoPostBack="True" GroupName="Planzulassung"
                                                                     Text="Ja" />
                                                                 &nbsp;<asp:RadioButton ID="rdo_NeinPlanzulassung" runat="server" AutoPostBack="True" GroupName="Planzulassung"
                                                                     Text="Nein" />
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
                                    <table id="Table11" cellspacing="0" cellpadding="0" border="0" width="100%">
                                            <tr>
                                                <td valign="top">
                                                <div id="Suche2" 
                                                        style=" display:none;  overflow:hidden;  width:100%; height:306px; background:#FFFFCC; border: groove;" >
                                                    <table id="Table12" cellspacing="1" cellpadding="1" width="100%">

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
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                    </td>
                                    <td valign="top" align="right">

                                    </td>
                                </tr>
                                <tr ID="ButtonRow" runat="server">
                                    <td valign="top"  class="style1">
                                    <table id="Table13" cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tr>
                                        <td align="left">
                                            &nbsp;</td>
                                     <td align="right" style="padding-top:10px"> 
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
                                        </table>
                                    </td>
                                    <td valign="top" align="right">
                                        &nbsp;
                                    </td>
                                    <td valign="top" align="right">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="right">
                                        &nbsp;
                                    </td>
                                    <td valign="top" align="right">
                                        &nbsp;
                                    </td>
                                    <td valign="top" align="right">
                                        &nbsp;&nbsp; &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td valign="top"  align="left">
                                                    <table id="Table5" cellspacing="1" cellpadding="1" width="100%" bgcolor="white" border="0">
                                                        <tr id="rowResults" runat="server">
                                                            <td class="TextLarge" valign="center">
                                                                <asp:Label ID="lblResults" runat="server"></asp:Label>&nbsp; &nbsp;<asp:ImageButton
                                                                    ID="lnkCreateExcel" ImageUrl="~/Images/excel.gif" runat="server" Height="16px"
                                                                    Width="16px" ImageAlign="Top" Visible="False"></asp:ImageButton>
                                                                &nbsp;<asp:LinkButton ID="lnkCreateExcel2" class="TextLarge" Visible="False" runat="server">Excelformat</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr id="rowResultate" runat="server">
                                                            <td class="TextLarge" valign="center">
                                                                &nbsp;<br />
                                                                <asp:DataGrid ID="DataGrid1" runat="server" Width="740px" AllowPaging="True" AllowSorting="True"
                                                                    AutoGenerateColumns="False">
                                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                                    <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="Carportnr" SortExpression="Carportnr" HeaderText="Carport Nummer">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Carport Name" SortExpression="Carport Name" HeaderText="Carport Name">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Fahrzeuge" SortExpression="Fahrzeuge" HeaderText="Fahrzeuge">
                                                                        </asp:BoundColumn>
                                                                    </Columns>
                                                                    <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                                        HorizontalAlign="Center" CssClass="TextExtraLarge" Wrap="False"></PagerStyle>
                                                                </asp:DataGrid>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:LinkButton ID="cmdDetails" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Detailanzeige&nbsp;&#187;</asp:LinkButton>
                                        <br>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        &nbsp;
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
 <%--                                                   <table class="BorderLeftBottom" id="Table8" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:Label ID="Label1" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Carport:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPDI" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap>
                                                                Eingangsdatum von:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEingangsdatumVon" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnCal1" runat="server" ImageUrl="../../../Images/calendar_red.jpg" />&nbsp;(TT.MM.JJJJ)&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Eingangsdatum bis:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEingangsdatumBis" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnCal2" runat="server" ImageUrl="../../../Images/calendar_red.jpg" />&nbsp;(TT.MM.JJJJ)&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap>
                                                                Bereitmeldung von:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMeldungsdatumVon" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnCalBereit1" runat="server" ImageUrl="../../../Images/calendar_red.jpg" />&nbsp;(TT.MM.JJJJ)&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Bereitmeldung bis:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMeldungsdatumBis" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnCalBereit2" runat="server" ImageUrl="../../../Images/calendar_red.jpg" />&nbsp;(TT.MM.JJJJ)&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Fahrzeugmodell:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtModell" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Vorlage ZBII
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdo_Alle" runat="server" AutoPostBack="True" Checked="True"
                                                                    GroupName="Erledigt" Text="Alle" />
                                                                &nbsp;<asp:RadioButton ID="rdo_Ja" runat="server" AutoPostBack="True" GroupName="Erledigt"
                                                                    Text="Ja" />
                                                                &nbsp;<asp:RadioButton ID="rdo_Nein" runat="server" AutoPostBack="True" GroupName="Erledigt"
                                                                    Text="Nein" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
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
                                                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton"> •&nbsp;Erstellen</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>--%>
                                                        <INPUT id="SelOpen2" type="hidden" size="1" runat="server" />
    </form>
</body>
</html>
