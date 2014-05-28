<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report18_1.aspx.vb" Inherits="CKG.Components.ComCommon.Report18_1" %>

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
    
    <uc1:BusyIndicator ID="BusyIndicator1" runat="server" />

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
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                        <td width="100%" >
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
                                                        <asp:Label ID="lblInfo"  Font-Bold="true" runat="server"></asp:Label></p>
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
                                        <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" PageSize="50" 
                                            Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="col_Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxaq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="col_Vertragsnummer2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer2" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer2">col_Vertragsnummer2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxaq13" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer2") %>'>
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
                                               
                                                <asp:TemplateColumn HeaderText="col_Hersteller">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="Hersteller">col_Hersteller
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya1yqxa" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Hersteller") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                               
                                                <asp:TemplateColumn HeaderText="col_Typ">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="Typ">col_Typ
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya1yqxqwaw" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Typ") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                               
                                                <asp:TemplateColumn HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="col_Modellbezeichnung">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Modellbezeichnung" runat="server" CommandName="Sort" CommandArgument="Modellbezeichnung">col_Modellbezeichnung
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya12" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Modellbezeichnung") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="col_Label">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Label" runat="server" CommandName="Sort" CommandArgument="Label">col_Label
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya123" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Label") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                
                                                <asp:TemplateColumn HeaderText="col_Suchname">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Suchname" runat="server" CommandName="Sort" CommandArgument="Suchname">col_Suchname</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Suchname") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Versanddatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versanddatum" CommandArgument="Versanddatum" CommandName="Sort"
                                                            runat="server">col_Versanddatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2y" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versanddatum","{0:dd.MM.yyyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Abrufgrund">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Abrufgrund" runat="server" CommandName="Sort" CommandArgument="Abrufgrund">col_Abrufgrund
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abrufgrund") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Anforderer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Anforderer" runat="server" CommandName="Sort" CommandArgument="Anforderer">col_Anforderer
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelc1X" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anforderer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Anforderer2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Anforderer2" runat="server" CommandName="Sort" CommandArgument="Anforderer2">col_Anforderer2
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelc1132X" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anforderer2") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Freigeber">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Freigeber" runat="server" CommandName="Sort" CommandArgument="Freigeber">col_Freigeber
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelc1132Xas" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Freigeber") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                
                                                
                                                <asp:TemplateColumn HeaderText="col_VertriebseinheitZuKunde">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_VertriebseinheitZuKunde" runat="server" CommandName="Sort"
                                                            CommandArgument="VertriebseinheitZuKunde">col_VertriebseinheitZuKunde
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VertriebseinheitZuKunde") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_NummerZBII">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZBII" runat="server" CommandName="Sort" CommandArgument="NummerZBII">col_NummerZBII
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcu1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Mahnstufe">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Mahnstufe" runat="server" CommandName="Sort" CommandArgument="Mahnstufe">col_Mahnstufe
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Mahnstufe") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Mahndatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Mahndatum" runat="server" CommandName="Sort" CommandArgument="Mahndatum">col_Mahndatum
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcq2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Mahndatum","{0:dd.MM.yyyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="col_Versandart">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandart" runat="server" CommandName="Sort" CommandArgument="Versandart">col_Versandart
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcq3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandart") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                                                               
                                                <asp:TemplateColumn HeaderText="col_CoC">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_CoC" runat="server" CommandName="Sort" CommandArgument="CoC">col_CoC
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CoC") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="col_Leasingnehmer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingnehmer" runat="server" CommandName="Sort" CommandArgument="Leasingnehmer">col_Leasingnehmer
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya1q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingnehmer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Eingangsdatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Eingangsdatum" runat="server" CommandName="Sort" CommandArgument="Eingangsdatum">col_Eingangsdatum
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya1yq" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum","{0:dd.MM.yyyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                             
                                                
                                                <asp:TemplateColumn HeaderText="col_Standort">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Standort" runat="server" CommandName="Sort" CommandArgument="Standort">col_Standort
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya1yqxqwa" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Standort") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Inbetriebnahme">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Inbetriebnahme" runat="server" CommandName="Sort" CommandArgument="Inbetriebnahme">col_Inbetriebnahme
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya1yqxdwqwa" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Inbetriebnahme","{0:dd.MM.yyyy}") %>'>
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
