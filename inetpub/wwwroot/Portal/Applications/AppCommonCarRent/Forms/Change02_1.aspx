<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_1.aspx.vb" Inherits="AppCommonCarRent.Change02_1" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>


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
    
    <uc1:BusyIndicator runat="server" />

    <form id="Form1" method="post" runat="server">

    <script language="javascript" id="ScrollPosition">
		
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
                                    <td class="TaskTitle">
                                                                              <asp:LinkButton ID="lb_zurueck" Visible="True" runat="server">lb_zurueck</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        &nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="lbtnweiter" CssClass="StandardButton" runat="server">Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="100%"  valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle"  >
                                        &nbsp;
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
                                                        &nbsp;</td>
                                                <td  >
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                        <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label><asp:Label
                                                            ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>
                                                </td>
                                                <td align="right"  >
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td valign="baseline">
                                                    <asp:Label ID="lblInfo" Font-Bold="true" runat="server"></asp:Label>
                                                </td>
                                                <td align="right"  >
                                                  <asp:ImageButton ID="imgbExcel" runat="server" Height="20px" ImageUrl="../../../Images/excel.gif"
                                                    Visible="False" Width="20px" /> &nbsp;
                                                    &nbsp;<b>Ergebnisse/Seite:</b>&nbsp;<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
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
                        <td valign="top" width="120px">
                            &nbsp;</td>
                        <td width="100%">

                <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
                </asp:ScriptManager>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                         <asp:DataGrid ID="DataGrid1" runat="server"  PageSize="50" Width="100%"
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
                                                <asp:TemplateColumn HeaderText="col_Hersteller">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="Hersteller">col_Hersteller</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHersteller" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Hersteller") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Typ">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="Fahrzeugtyp">col_Typ</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTyp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrzeugtyp") %>'>
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
                                                <asp:TemplateColumn HeaderText="col_Retourdatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Retourdatum" runat="server" CommandName="Sort" CommandArgument="Retourdatum">col_Retourdatum
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRetour" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Retourdatum","{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Abmeldedatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandName="Sort" CommandArgument="Abmeldedatum">col_Abmeldedatum
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelc1X" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum","{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Lagerort">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Lagerort" runat="server" CommandName="Sort" CommandArgument="Lagerort">col_Lagerort
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLagerort" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Lagerort") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Leasingnehmer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingnehmer" runat="server" CommandName="Sort" CommandArgument="Leasingnehmer">col_Leasingnehmer
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLeasingnehmer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingnehmer") %>'>
                                                        </asp:Label>  
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Datumbezahlt">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Datumbezahlt" runat="server" CommandName="Sort" CommandArgument="Datumbezahlt">col_Datumbezahlt
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDatumbezahlt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Datumbezahlt","{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_CoCvorhanden" ItemStyle-VerticalAlign="Middle"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_CoCvorhanden" runat="server" CommandName="Sort" CommandArgument="CoCvorhanden">col_CoCvorhanden
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chbCoCvorhanden" Enabled="false" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.CoCvorhanden") ="X" %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Bezahltkennzeichen" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Bezahltkennzeichen" runat="server" CommandName="Sort" CommandArgument="BzKennz">col_Bezahltkennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chbBzKennz" runat="server" 
                                                            Checked='<%# DataBinder.Eval(Container, "DataItem.Bezahltkennzeichen")="X" %>' 
                                                             AutoPostBack="True" CommandName="Update" CommandArgument="chbBzKennz" 
                                                            oncheckedchanged="chbBzKennz_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Sperren" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Sperren" runat="server" CommandName="Sort" CommandArgument="Versandsperre">col_Sperren</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chbSperren" runat="server" 
                                                            Checked='<%# DataBinder.Eval(Container, "DataItem.Versandsperre")="X" %>' 
                                                            AutoPostBack="True" oncheckedchanged="chbSperren_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Anfordern" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Anfordern" runat="server" CommandName="Sort" CommandArgument="Anfordern">col_Anfordern</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chbAnfordern" AutoPostBack="True" runat="server" 
                                                            Enabled="false" 
                                                            Checked='<%# DataBinder.Eval(Container, "DataItem.Anfordern")="X" %>' 
                                                            oncheckedchanged="chbAnfordern_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Abmeld" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Abmeld" runat="server" CommandName="Sort" CommandArgument="Abmelden">col_Abmeld</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chbAbmeld" runat="server"  
                                                            Checked= '<%# DataBinder.Eval(Container, "DataItem.Abmelden")="" %>' 
                                                            AutoPostBack="True" oncheckedchanged="chbAbmeld_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle BackColor="White" NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>                           
                            
                            </ContentTemplate>
                            </asp:UpdatePanel>

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
                                        &nbsp;</td>
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
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
