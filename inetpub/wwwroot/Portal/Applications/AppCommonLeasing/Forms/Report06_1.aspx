<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report06_1.aspx.vb" Inherits="AppCommonLeasing.Report06_1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">

    <script language="javascript" id="ScrollPosition">
<!--
		
	function sstchur_SmartScroller_GetCoords()
   {
		
      var scrollX, scrollY;
      
      if (document.all)
      {
         if (!document.documentElement.scrollLeft)
            scrollX = document.body.scrollLeft;
         else
            scrollX = document.documentElement.scrollLeft;
               
         if (!document.documentElement.scrollTop)
            scrollY = document.body.scrollTop;
         else
            scrollY = document.documentElement.scrollTop;
      }   
      else
      {
         scrollX = window.pageXOffset;
         scrollY = window.pageYOffset;
      }
      
      
		
      document.forms["Form1"].xCoordHolder.value = scrollX;
      document.forms["Form1"].yCoordHolder.value = scrollY;
     
   }
   
   function sstchur_SmartScroller_Scroll()
   {
  
			
      var x = document.forms["Form1"].xCoordHolder.value;
      var y = document.forms["Form1"].yCoordHolder.value;
      window.scrollTo(x, y);
     
   }
   
   window.onload = sstchur_SmartScroller_Scroll;
   window.onscroll = sstchur_SmartScroller_GetCoords;
   window.onkeypress = sstchur_SmartScroller_GetCoords;
   window.onclick = sstchur_SmartScroller_GetCoords;
  
// -->
    </script>

    <input type="hidden" id="xCoordHolder" runat="server" />
    <input type="hidden" id="yCoordHolder" runat="server" />
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120px">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="100%">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:LinkButton ID="lb_zurueck" Visible="True" runat="server">lb_zurueck</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td>
                                                    <p>
                                                        <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp;<asp:Label
                                                            ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;</p>
                                                    <p>
                                                        <asp:Label ID="lblInfo" Font-Bold="true" runat="server"></asp:Label></p>
                                                </td>
                                                <td align="right">
                                                    <asp:ImageButton ID="imgbExcel" runat="server" Height="20px" ImageUrl="../../../Images/excel.gif"
                                                        Visible="True" Width="20px" />
                                                    &nbsp;Ergebnisse/Seite:&nbsp;<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table id="TableKleinerAbstandVorGrid" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" PageSize="50" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                            <asp:BoundColumn DataField="EQUNR" Visible="false"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="col_Leasingvertragsnummer" SortExpression="Leasingvertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingvertragsnummer" runat="server" CommandName="Sort"
                                                            CommandArgument="Leasingvertragsnummer">col_Leasingvertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingvertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkFahrgestellnummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                            ToolTip="Anzeige Fahrzeughistorie" Target="_blank">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Kennzeichen" >
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21qwqwe1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Versanddatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versanddatum" runat="server" CommandName="Sort" CommandArgument="Versanddatum">col_Versanddatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21px113" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versanddatum","{0:dd.MM.yyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Versandadresse">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandadresse" runat="server" CommandName="Sort" CommandArgument="Versandadresse">col_Versandadresse</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_KundennummerAuftraggeber" >
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_KundennummerAuftraggeber" runat="server" CommandName="Sort"
                                                            CommandArgument="KundennummerAuftraggeber">col_KundennummerAuftraggeber</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21p" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KundennummerAuftraggeber") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_KundennummerHalteranschrift">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_KundennummerHalteranschrift" runat="server" CommandName="Sort"
                                                            CommandArgument="KundennummerHalteranschrift">col_KundennummerHalteranschrift</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21paxevw" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KundennummerHalteranschrift") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_KundennummerHaendler">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_KundennummerHaendler" runat="server" CommandName="Sort" CommandArgument="KundennummerHaendler">col_KundennummerHaendler</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label991" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KundennummerHaendler") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Equipmenttyp">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Equipmenttyp" runat="server" CommandName="Sort" CommandArgument="Equipmenttyp">col_Equipmenttyp</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label9911" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Equipmenttyp") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_ZBIINummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ZBIINummer" runat="server" CommandName="Sort" CommandArgument="ZBIINummer">col_ZBIINummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label9912" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZBIINummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Inventarnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Inventarnummer" runat="server" CommandName="Sort" CommandArgument="Inventarnummer">col_Inventarnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label9912a" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Inventarnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Versandart">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandart" runat="server" CommandName="Sort" CommandArgument="Versandart">col_Versandart</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="L1abel9912a21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandart") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Raum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Raum" runat="server" CommandName="Sort" CommandArgument="Raum">col_Raum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label99122a21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Raum") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Anlagestandort">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Anlagestandort" runat="server" CommandName="Sort" CommandArgument="Anlagestandort">col_Anlagestandort</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label902a21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anlagestandort") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                               
                                                <asp:TemplateColumn HeaderText="col_Mahnstufe">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Mahnstufe" runat="server" CommandName="Sort" CommandArgument="Mahnstufe">col_Mahnstufe</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labe29912b21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Mahnstufe") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Mahndatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Mahndatum" runat="server" CommandName="Sort" CommandArgument="Mahndatum">col_Mahndatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labe299112b21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Mahndatum","{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Sachbearbeiter">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Sachbearbeiter" runat="server" CommandName="Sort" CommandArgument="Sachbearbeiter">col_Sachbearbeiter</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label299112b21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Sachbearbeiter") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Versandgrund">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandgrund" runat="server" CommandName="Sort" CommandArgument="Versandgrund">col_Versandgrund</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="XLabel299112b21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandgrund") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Adressnummer1">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Adressnummer1" runat="server" CommandName="Sort" CommandArgument="Adressnummer1">col_Adressnummer1</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="XLabel456" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Adressnummer1") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Adressnummer2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Adressnummer2" runat="server" CommandName="Sort" CommandArgument="Adressnummer2">col_Adressnummer2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="XLa12bel456" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Adressnummer2") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Konzernschluessel">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Konzernschluessel" runat="server" CommandName="Sort" CommandArgument="Konzernschluessel">col_Konzernschluessel</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="XLa12bel456123" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Konzernschluessel") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" valign="top" align="left">
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
