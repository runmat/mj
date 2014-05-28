<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BapiOverView.aspx.vb"
    Inherits="CKG.Admin.BapiOverView" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:styles id="ucStyles" runat="server">
    </uc1:styles>
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
                <uc1:header id="ucHeader" runat="server">
                </uc1:header>
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
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center" nowrap="nowrap">
                                                     
                                        Filter&nbsp;<asp:RadioButtonList   ID="rblFilterFor" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Text="all" Value="all"></asp:ListItem>
                                            <asp:ListItem Text="RAM" Value="RAM"></asp:ListItem>
                                            <asp:ListItem Text="DB" Value="DB"></asp:ListItem>
                                            <asp:ListItem Text="Update DB" Value="UpdateDB"></asp:ListItem>
                                        </asp:RadioButtonList>
                                            
                        </td>
        </tr>
        <tr>
            <td colspan="3" align="center" nowrap="nowrap">
                &nbsp; Bapi Name:&nbsp;<asp:TextBox ID="txtFilter" runat="server" Text="**" Width="250px"></asp:TextBox>
                <span lang="de">&nbsp;<asp:ImageButton ID="imgbSetFilter" runat="server" Height="20px"
                    ImageUrl="../Images/refresh.gif" Visible="True" Width="20px" />
                </span>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table id="TableKleinerAbstandVorGrid" cellspacing="0" cellpadding="0" width="100%"
                    border="0">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr class="TextLarge">
                                    <td nowrap="nowrap" valign="bottom" width="15%">
                                        <asp:ImageButton runat="server" ID="imgbRamVisible" ImageUrl="../Images/minus.gif" />
                                        <span lang="de">&nbsp;RAM-Overview&nbsp;&nbsp;&nbsp;</span>
                                    </td>
                                    <td align="left" valign="bottom">
                                        <asp:Label ID="lblRamInfo" Font-Bold="True" runat="server"></asp:Label>&nbsp;
                                        <asp:Label ID="lblRamTimeInfo" Font-Bold="True" runat="server"></asp:Label>
                                        <span lang="de">&nbsp;<strong>Uhr</strong>&nbsp;</span>
                                        <asp:Label ID="lblSpeicherbedarf" Font-Bold="True" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" nowrap="nowrap">
                                        <asp:ImageButton ID="imgbRamAktulisieren" runat="server" Height="20px" ImageUrl="../Images/refresh.gif"
                                            Visible="True" Width="20px" />&nbsp;
                                        <asp:LinkButton ID="lbSpeicherbedarf" runat="server" CssClass="StandardButton">Speicherbedarf 
                                                        / clear all</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="panelRam" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2" Width="100%"
                                runat="server" Height="100%">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <p>
                                                <asp:Label ID="lblRamNoData" runat="server" Visible="False"></asp:Label>&nbsp;<asp:Label
                                                    ID="lblRamError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;</p>
                                            <p>
                                                &nbsp;</p>
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="imgbRamExcel" runat="server" Height="20px" ImageUrl="../Images/excel.gif"
                                                Visible="True" Width="20px" />
                                            &nbsp;Ergebnisse/Seite:&nbsp;<asp:DropDownList ID="ddlRamPageSize" runat="server"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:DataGrid ID="RamDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                    AllowPaging="True" AllowSorting="True">
                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="BapiName" Visible="false"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="col_BapiName">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_BapiName" runat="server" CommandName="Sort" CommandArgument="BapiName">col_BapiName</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label21q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_BapiDate">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_BapiDate" runat="server" CommandName="Sort" CommandArgument="BapiDate">col_BapiDate</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label21qwqwe" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BapiDate","{0:d}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_BapiLoadet">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_BapiLoadet" runat="server" CommandName="Sort" CommandArgument="BapiLoadet">col_BapiLoadet</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label22paxevwq" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BapiLoadet") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Delete" runat="server">col_Delete</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbDelete" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'
                                                    CommandName="Delete" Height="10px">
																		<img src="../Images/loesch.gif" border="0"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                        HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                            </asp:Panel>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr class="TextLarge">
                                    <td nowrap="nowrap" valign="bottom" width="15%">
                                        <asp:ImageButton runat="server" ID="imgbDBVisible" ImageUrl="../Images/minus.gif" />
                                        <span lang="de">&nbsp;DB-Overview&nbsp;&nbsp;&nbsp;</span>
                                    </td>
                                    <td align="left" valign="bottom">
                                        <asp:Label ID="lblDBInfo" Font-Bold="True" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" nowrap="nowrap">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="PanelDB" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2" Width="100%"
                                runat="server" Height="100%">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <p>
                                                <asp:Label ID="lblDBNoData" runat="server" Visible="False"></asp:Label>&nbsp;<asp:Label
                                                    ID="lblDBError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;</p>
                                            <p>
                                                &nbsp;</p>
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="imgbDBExcel" runat="server" Height="20px" ImageUrl="../Images/excel.gif"
                                                Visible="True" Width="20px" />
                                            &nbsp;Ergebnisse/Seite:&nbsp;<asp:DropDownList ID="ddlDBPageSize" runat="server"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:DataGrid ID="DBDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                    AllowPaging="True" AllowSorting="True">
                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="BapiName" Visible="false"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="col_BapiName">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_BapiName" runat="server" CommandName="Sort" CommandArgument="BapiName">col_BapiName</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_BapiDate">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_BapiDate" runat="server" CommandName="Sort" CommandArgument="BapiDate">col_BapiDate</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BapiDate","{0:d}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_inserted">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_inserted" runat="server" CommandName="Sort" CommandArgument="inserted">col_inserted</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.inserted") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_updated">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_updated" runat="server" CommandName="Sort" CommandArgument="inserted">col_updated</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.updated") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_Details" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Details" runat="server">col_Details</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbDetails" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'
                                                    runat="server" Width="10px" CommandName="Details" Height="10px">
																		<img  src="../Images/lupe2.gif" border="0"/></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Delete" runat="server">col_Delete</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbDelete" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'
                                                    CommandName="Delete" Height="10px">
																		<img src="../Images/loesch.gif" border="0"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                        HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                            </asp:Panel>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr class="TextLarge">
                                    <td nowrap="nowrap" valign="bottom" width="15%">
                                        <asp:ImageButton runat="server" ID="imgbUpdateDBVisible" ImageUrl="../Images/minus.gif" />
                                        <span lang="de">&nbsp;UpdateDB-Overview&nbsp;&nbsp;&nbsp;</span>
                                    </td>
                                    <td align="left" valign="bottom">
                                        <asp:Label ID="lblUpdateDBInfo" Font-Bold="True" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" nowrap="nowrap">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel BorderColor="Gray" BorderStyle="Solid" BorderWidth="2" ID="PanelUpdateDB"
                                Width="100%" runat="server" Height="100%">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <p>
                                                <asp:Label ID="lblUpdateDBNoData" runat="server" Visible="False"></asp:Label>&nbsp;<asp:Label
                                                    ID="lblUpdateDBError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;</p>
                                            <p>
                                                <asp:TextBox ID="txtInsertBapiName" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                                                &nbsp;<asp:ImageButton ImageAlign="Middle" ID="imgbInsertUpdateDG" runat="server"
                                                    Height="20px" ImageUrl="../Images/Insert.gif" Visible="True" Width="20px" />
                                            </p>
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="imgbUpdateDBExcel" runat="server" Height="20px" ImageUrl="../Images/excel.gif"
                                                Visible="True" Width="20px" />
                                            &nbsp;Ergebnisse/Seite:&nbsp;<asp:DropDownList ID="ddlUpdateDBPageSize" runat="server"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:DataGrid ID="updateDBDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                    AllowPaging="True" AllowSorting="True">
                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="BapiName" Visible="false"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="col_BapiName">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_BapiName" runat="server" CommandName="Sort" CommandArgument="BapiName">col_BapiName</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_BapiLastChangeSap">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_BapiLastChangeSap" runat="server" CommandName="Sort" CommandArgument="BapiLastChangeSap">col_BapiLastChangeSap</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BapiLastChangeSap") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_inserted">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_inserted" runat="server" CommandName="Sort" CommandArgument="inserted">col_inserted</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.inserted") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_updated">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_updated" runat="server" CommandName="Sort" CommandArgument="updated">col_updated</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.updated") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_LastSapDeveloper">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_LastSapDeveloper" runat="server" CommandName="Sort" CommandArgument="LastSapDeveloper">col_LastSapDeveloper</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LastSapDeveloper") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_LastDBChangeWebBy">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_LastDBChangeWebBy" runat="server" CommandName="Sort" CommandArgument="LastDBChangeWebBy">col_LastDBChangeWebBy</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LastDBChangeWebBy") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="col_Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Delete" runat="server">col_Delete</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbDelete" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'
                                                    CommandName="Delete" Height="10px">
																		<img src="../Images/loesch.gif" border="0"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                        HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                            </asp:Panel>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </td> </tr>
    <tr>
        <td colspan="3" valign="top" align="left">
            <!--#include File="../PageElements/Footer.html" -->
        </td>
    </tr>
    </table>
    </form>
</body>
</html>
