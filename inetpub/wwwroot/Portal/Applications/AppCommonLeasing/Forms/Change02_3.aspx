<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_3.aspx.vb" Inherits="AppCommonLeasing.Change02_3" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

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
                                        &nbsp;<asp:LinkButton ID="lb_zurueck" Visible="True" runat="server">lb_zurueck</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="lb_weiter" CssClass="StandardButton" runat="server">Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="100%">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;</td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table  width="100%" border="0">
                                            <tr>
                                                <td colspan="2">
                                                    <table id="tblAnzeigeVersandDaten" cellspacing="0" cellpadding="5" width="100%" border="0">
                                                        <tr id="tr_VersandAdressArt">
                                                            <td align="left">
                                                                <asp:Label CssClass="TextLarge" ID="lbl_VersandAdressArt" runat="server"></asp:Label>
                                                            </td> 
                                                                                                  <td align="left" width="100%">
                                                                <asp:Label CssClass="TextXLarge" ID="lblVersandAdressArtAnzeige" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_Versandadresse">
                                                            <td align="left">
                                                                <asp:Label ID="lbl_Versandadresse" CssClass="TextLarge" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left" width="100%">
                                                                <asp:Label CssClass="TextXLarge" ID="lblVersandadresseAnzeige" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_Versandart">
                                                            <td align="left">
                                                                <asp:Label ID="lbl_Versandart" CssClass="TextLarge" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left" width="100%">
                                                                <asp:Label CssClass="TextXLarge" ID="lblVersandartAnzeige" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_Versandgrund">
                                                            <td align="left">
                                                                <asp:Label ID="lbl_Versandgrund" CssClass="TextLarge" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left" width="100%">
                                                                <asp:Label CssClass="TextXLarge" ID="lblVersandgrundAnzeige" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                       
                                                    </table>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td>
                                                    <p>
                                                        <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp;<asp:Label
                                                            ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;</p>
                                                    <p>
                                                        <asp:Label ID="lblInfo" Font-Bold="true" runat="server"></asp:Label></p>
                                                </td>
                                                <td align="right">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                   
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
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
                                                <asp:BoundColumn Visible="False" DataField="EQUNR"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="col_Leasingvertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingvertragsnummer" runat="server" CommandName="Sort"
                                                            CommandArgument="Leasingvertragsnummer">col_Leasingvertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingvertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="col_Suchname">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Suchname" runat="server" CommandName="Sort" CommandArgument="Suchname">col_Suchname </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Suchname") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="col_Leasingnehmer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingnehmer" runat="server" CommandName="Sort" CommandArgument="Leasingnehmer">col_Leasingnehmer </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelq11" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingnehmer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                              
                                                <asp:TemplateColumn HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
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
                                                <asp:TemplateColumn HeaderText="col_NummerZBII">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZBII" CommandArgument="NummerZBII" CommandName="Sort"
                                                            runat="server">col_NummerZBII</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2y" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                               
                                                <asp:TemplateColumn HeaderText="col_Abmeldedatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandName="Sort" CommandArgument="Abmeldedatum">col_Abmeldedatum
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelc1X" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum","{0:dd.MM.yyyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_CoCvorhanden" ItemStyle-VerticalAlign="Middle"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_CoCvorhanden" runat="server" CommandName="Sort" CommandArgument="CoCvorhanden">col_CoCvorhanden
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chbCoCvorhanden"  Enabled="false" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.CoCvorhanden") ="X" %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Entfernen" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Entfernen" runat="server">col_Entfernen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate >
                                                        <asp:LinkButton Visible="true" ID="lbEntfernen" 
                                                            CommandName="entfernen" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                            runat="server"><img src=../../../Images/loesch.gif border=0> </asp:LinkButton>
                                                        <asp:Label Visible="false" ID="lblMessage" runat="server"></asp:Label>
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
