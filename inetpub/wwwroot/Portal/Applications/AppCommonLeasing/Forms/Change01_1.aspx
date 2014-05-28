<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_1.aspx.vb" Inherits="AppCommonLeasing.Change01_1" %>

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
                                          <EditItemStyle CssClass="GridTableEdit" />
                                          
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="EQUNR"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="col_Leasingvertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingvertragsnummer" runat="server" CommandName="Sort"
                                                            CommandArgument="Leasingvertragsnummer">col_Leasingvertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxaq1y" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingvertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                               
                                                <asp:TemplateColumn HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxaq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
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
                                                <asp:TemplateColumn HeaderText="col_SuchnameKunde">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_SuchnameKunde" runat="server" CommandName="Sort" CommandArgument="SuchnameKunde">col_SuchnameKunde </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SuchnameKunde") %>'>
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
                                                <asp:TemplateColumn HeaderText="col_Versandgrund">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandgrund" runat="server" CommandName="Sort" CommandArgument="Versandgrund">col_Versandgrund
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandgrund") %>'>
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
                                                <asp:TemplateColumn HeaderText="col_Faelligkeitsdatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Faelligkeitsdatum" runat="server">col_Faelligkeitsdatum
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton Visible='<%# DataBinder.Eval(Container, "DataItem.Faelligkeitsdatum","{0:dd.MM.yyyy}") = "01.01.1900" %>'
                                                            ID="lbChangeFaelligkeit" OnClientClick="Show_BusyBox1();" CssClass="StandardButton"
                                                            CommandName="Fristverlaengerung" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                            runat="server">Fristverlängerung
                                                        </asp:LinkButton>
                                                        <asp:Label Visible='<%# not DataBinder.Eval(Container, "DataItem.Faelligkeitsdatum","{0:dd.MM.yyyy}") = "01.01.1900" %>'
                                                            ID="lblFaelligkeitdatum" Text='<%# DataBinder.Eval(Container, "DataItem.Faelligkeitsdatum","{0:dd.MM.yyyy}") %>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Memo">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Memo" CommandArgument="Memo" CommandName="Sort" runat="server">col_Memo</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3qw" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.Memo") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtMemo" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.Memo")%>'
                                                            BorderStyle="Solid" BorderWidth="1" BorderColor="GrayText" TextMode="MultiLine"
                                                            MaxLength="120" Rows="4">
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_MemoEdit" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate >
                                                        <asp:LinkButton ID="col_MemoEdit" runat="server">col_MemoEdit</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton Visible="true" ID="lbMemoEdit" CssClass="StandardButton" CommandName="Edit"
                                                            CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' runat="server">Ändern</asp:LinkButton>
                                                                      </ItemTemplate>
                                                    <EditItemTemplate >
                                                    <p  style="vertical-align:middle;">
                                                        <asp:LinkButton Visible="true" ID="lbSpeichern" CssClass="StandardButton" CommandName="Speichern"
                                                            OnClientClick="Show_BusyBox1();" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                            runat="server">Speichern</asp:LinkButton>
                                                          <asp:LinkButton Visible="true" ID="lbAbbrechen" CssClass="StandardButton" CommandName="Abbrechen"
                                                              CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' runat="server">Abbrechen</asp:LinkButton>
                                                    </p>
                                                    </EditItemTemplate>
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
