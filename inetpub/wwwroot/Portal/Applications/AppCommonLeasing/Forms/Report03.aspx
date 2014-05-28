<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report03.aspx.vb" Inherits="AppCommonLeasing.Report03" %>

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
                                                                                                                                                                                                                                    
                                                <asp:TemplateColumn HeaderText="col_EingangCarportliste" >
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_EingangCarportliste" runat="server" CommandName="Sort" CommandArgument="EingangCarportliste">col_EingangCarportliste</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EingangCarportliste","{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_CarportmeldungErfasst" >
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_CarportmeldungErfasst" runat="server" CommandName="Sort"
                                                            CommandArgument="CarportmeldungErfasst">col_CarportmeldungErfasst</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label22paxevwq" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.CarportmeldungErfasst") ="Nein" %>'
                                                            runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CarportmeldungErfasst") %>'>
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
                                                <asp:TemplateColumn HeaderText="col_AnzahlSchilder">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_AnzahlSchilder" runat="server" CommandName="Sort" CommandArgument="AnzahlSchilder">col_AnzahlSchilder</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21p" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AnzahlSchilder") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                               
                                                
                                                <asp:TemplateColumn HeaderText="col_Abmeldeauftrag">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Abmeldeauftrag" runat="server" CommandName="Sort" CommandArgument="Abmeldeauftrag">col_Abmeldeauftrag</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21pxy" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.Abmeldeauftrag") ="Nein" %>'
                                                            runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldeauftrag") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="col_ZBI">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ZBI" runat="server" CommandName="Sort" CommandArgument="ZBI">col_ZBI</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21px" runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.ZBI") ="Nein" %>'
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.ZBI") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                   
                                                <asp:TemplateColumn HeaderText="col_ZBII">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ZBII" runat="server" CommandName="Sort" CommandArgument="ZBII">col_ZBII</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21pax" runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.ZBII") ="Nein" %>'
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.ZBII") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_EingangPhysisch">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_EingangPhysisch" runat="server" CommandName="Sort" CommandArgument="EingangPhysisch">col_EingangPhysisch</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21paxe" runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.EingangPhysisch") ="Nein" %>'
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.EingangPhysisch") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_TreuhandpartnerVorhanden">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_TreuhandpartnerVorhanden" runat="server" CommandName="Sort"
                                                            CommandArgument="TreuhandpartnerVorhanden">col_TreuhandpartnerVorhanden</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21paxev" runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.TreuhandpartnerVorhanden") ="Nein" %>'
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.TreuhandpartnerVorhanden") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_VersendetAm">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_VersendetAm" runat="server" CommandName="Sort" CommandArgument="VersendetAm">col_VersendetAm</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21paxevw" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VersendetAm","{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="col_Sachbearbeiter">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Sachbearbeiter" runat="server" CommandName="Sort" CommandArgument="Sachbearbeiter">col_Sachbearbeiter</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label991" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Sachbearbeiter") %>'>
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
