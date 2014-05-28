<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowBapis.aspx.vb" Inherits="CKG.Admin.ShowBapis" %>

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
    <style type="text/css">
        .style1
        {
            text-decoration: underline;
        }
    </style>
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
                        <td colspan="3" align="center">
                           Bapi Name:&nbsp;<asp:TextBox ID="txtFilter" runat="server" Text="**" Width="250px"></asp:TextBox>
                            &nbsp;<asp:ImageButton ID="imgbSetFilter" runat="server" Height="20px"
                                ImageUrl="../Images/refresh.gif" Visible="True" Width="20px" />
                            &nbsp; &nbsp;<asp:ImageButton ID="imgbLookSAP" runat="server"  ImageUrl="../Images/SAPLogo.gif"
                                Visible="True"  />
                                                           
                        </td>
                    </tr>
                  
                    <tr>
                        <td colspan="3" align="center">
                            <b>Web<span lang="de">-</span>Bapis
                                <asp:Label ID="lblWebBapisError" CssClass="TextError"  runat="server"></asp:Label>
                                <span lang="de">&nbsp;<asp:Label ID="lblWebBapisNoData" runat="server" Visible="False"></asp:Label></span>&nbsp;<asp:Label
                                    ID="lblWebBapisInfo" runat="server" Font-Bold="True"></asp:Label>
                            </b>
                                          
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:DataGrid ID="WebBapisDG" bodyHeight="250" CssClass="tableMain" bodyCSS="tableBody"
                                headerCSS="tableHeader"  Width="600" runat="server" BackColor="White" AutoGenerateColumns="False"
                                AllowSorting="True">
                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="BapiName" Visible="false"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="col_LookAt" ItemStyle-Wrap="false"  ItemStyle-VerticalAlign="Middle">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_LookAt" runat="server">col_LookAt</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbLookSAP" runat="server"  CommandArgument='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'
                                                CommandName="ShowSAP" >
																		<img src="../Images/SAPLogo.gif" border="0"></asp:LinkButton>
                                            &nbsp;&nbsp;<asp:LinkButton ID="lbLookWEB" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'
                                                CommandName="ShowWEB" >
																		<img src="../Images/dotNet.jpeg" border="0"></asp:LinkButton>
																		
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="col_BapiName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_BapiName" runat="server" CommandName="Sort" CommandArgument="BapiName">col_BapiName</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label21q" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                        HeaderText="col_BapiDate">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_BapiDate" runat="server" CommandName="Sort" CommandArgument="BapiDate">col_BapiDate</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label21qwqwe" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BapiDate","{0:dd.MM.yyyy}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="border-bottom-color: red; border-bottom-style: solid; border-width: 3;">
                            &nbsp;
                       </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr class="TextLarge">
                                    <td nowrap="nowrap" valign="bottom" width="15%">
                                        <asp:ImageButton runat="server" ID="imgbWebBapiVisible" 
                                            ImageUrl="../Images/minus.gif" />
                                        <span lang="de">&nbsp;<span class="style1"><strong>Web-Bapi Struktur</strong></span>&nbsp;&nbsp;&nbsp;</span>
                                    </td>
                                    <td align="left" valign="bottom">
                                        &nbsp;
                                        <asp:Label ID="lblWebBapiName" Font-Bold="True" runat="server" ForeColor="Red"></asp:Label>
                                        &nbsp;<asp:Label ID="lblWebBapiDatum" Font-Bold="True" runat="server" 
                                            ForeColor="Red"></asp:Label>
                                        <span lang="de">&nbsp;<asp:Label ID="lblWebBapiError" runat="server" EnableViewState="False"
                                            CssClass="TextError"></asp:Label></span>
                                    </td>
                                    <td align="right">
                                       </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="panelWebBapi" BorderColor="Gray" BorderStyle="Solid" BorderWidth="0" Width="100%"
                                runat="server" Height="100%">
                                
                            <table id="TableKleinerAbstandVorGrid" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr class="TextLarge">
                                                <td nowrap="nowrap" valign="bottom" width="15%">
                                                    <asp:ImageButton runat="server" ID="imgbWebImportVisible" 
                                                        ImageUrl="../Images/plus.gif" />
                                                    <span lang="de">&nbsp;<strong>Web-Bapi Import</strong>&nbsp;&nbsp;&nbsp;</span>
                                                </td>
                                                <td align="left" valign="bottom">
                                                    &nbsp;
                                                    <asp:Label ID="lblWebImportInfo" Font-Bold="True" runat="server"></asp:Label>
                                                    &nbsp;<asp:Label ID="lblWebImportNoData" runat="server" Visible="False"></asp:Label>
                                                    <span lang="de">&nbsp;<asp:Label ID="lblWebImportError" runat="server" EnableViewState="False"
                                                        CssClass="TextError"></asp:Label></span>
                                                </td>
                                                <td align="right">
                                                    <asp:ImageButton ID="imgbWebImportParameter" runat="server" Height="20px" ImageUrl="../Images/excel.gif"
                                                        Visible="True" Width="20px" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="panelWebImport"  Visible="false" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2"
                                            Width="100%" runat="server" Height="100%">
                                            <asp:DataGrid ID="WebImportDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                AllowSorting="True">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="imgbWebImportTabelleVisible" Visible='<%# DataBinder.Eval(Container, "DataItem.ParameterDATATYPE")="Tabelle" %>'
                                                                CommandName="Visible" ImageUrl="../Images/plus.gif" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn  ItemStyle-Width="34%" DataField="PARAMETER" HeaderText="Parameter Name"></asp:BoundColumn>
                                                    <asp:BoundColumn ItemStyle-Width="33%" DataField="ParameterDATATYPE" HeaderText="Parameter Datentyp"></asp:BoundColumn>
                                                    <asp:BoundColumn ItemStyle-Width="33%" DataField="ParameterLength" HeaderText="Parameter Länge">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate >
                                                            <asp:ImageButton runat="server" ID="imgbExcelFuerTabelle" Width="20px" Height="20px"
                                                                CommandName="Excel" Visible='<%# DataBinder.Eval(Container, "DataItem.ParameterDATATYPE")="Tabelle" %>'
                                                                ImageUrl="../Images/excel.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PARAMETER") %>' />
                                                            <asp:Literal Text='</TD></TR><TR align="center"><TD colspan="4">' ID="litHirarBeginn"
                                                                runat="server" EnableViewState="true">
                                                            </asp:Literal>
                                                            <asp:DataGrid ID="DataGridLvL2" runat="server" Visible="false" AutoGenerateColumns="False"
                                                                BorderWidth="1" BackColor="Transparent" Width="80%">
                                                                <ItemStyle CssClass="TextLarge"></ItemStyle>
                                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="SpaltenName" HeaderText="Spalten Name"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Laenge" HeaderText="Länge"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:Literal Text='</TD></TR>' ID="litHirarEnd" runat="server" EnableViewState="true">
																		
                                                            </asp:Literal>
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
                                <td></td>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr class="TextLarge">
                                                <td nowrap="nowrap" valign="bottom" width="15%">
                                                    <asp:ImageButton runat="server" ID="imgbWebExportVisible" ImageUrl="../Images/plus.gif" />
                                                    <span lang="de">&nbsp;<strong>Web-Bapi Export&nbsp;</strong>&nbsp;&nbsp;</span>
                                                </td>
                                                <td align="left" valign="bottom">
                                                    &nbsp;
                                                    <asp:Label ID="lblWebExportInfo" Font-Bold="True" runat="server"></asp:Label>
                                                    &nbsp;<asp:Label ID="lblWebExportNoData" runat="server" Visible="False"></asp:Label>
                                                    <span lang="de">&nbsp;<asp:Label ID="lblWebExportError" runat="server" EnableViewState="False"
                                                        CssClass="TextError"></asp:Label></span>
                                                </td>
                                                <td align="right">
                                                    <asp:ImageButton ID="imgbWebExportParameter" runat="server" Height="20px" ImageUrl="../Images/excel.gif"
                                                        Visible="True" Width="20px" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="panelWebExport" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                            BorderWidth="2" Width="100%" runat="server" Height="100%">
                                            <asp:DataGrid ID="WebExportDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                AllowSorting="True">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="imgbWebExportTabelleVisible" Visible='<%# DataBinder.Eval(Container, "DataItem.ParameterDATATYPE")="Tabelle" %>'
                                                                CommandName="Visible" ImageUrl="../Images/plus.gif" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn ItemStyle-Width="34%" DataField="PARAMETER" HeaderText="Parameter Name">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn ItemStyle-Width="33%" DataField="ParameterDATATYPE" HeaderText="Parameter Datentyp">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn ItemStyle-Width="33%" DataField="ParameterLength" HeaderText="Parameter Länge">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="imgbExcelFuerTabelle" Width="20px" Height="20px"
                                                                CommandName="Excel" Visible='<%# DataBinder.Eval(Container, "DataItem.ParameterDATATYPE")="Tabelle" %>'
                                                                ImageUrl="../Images/excel.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PARAMETER") %>' />
                                                            <asp:Literal Text='</TD></TR><TR align="center"><TD colspan="4">' ID="litHirarBeginn"
                                                                runat="server" EnableViewState="true">
                                                            </asp:Literal>
                                                            <asp:DataGrid ID="DataGridLvL2" Visible="false" runat="server" AutoGenerateColumns="False"
                                                                BorderWidth="1" BackColor="Transparent" Width="80%">
                                                                <ItemStyle CssClass="TextLarge"></ItemStyle>
                                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="SpaltenName" HeaderText="Spalten Name"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Laenge" HeaderText="Länge"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:Literal Text='</TD></TR>' ID="litHirarEnd" runat="server" EnableViewState="true">
																		
                                                            </asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid>
                                        </asp:Panel>
                                    </td>
                                
                                <td>
                                    &nbsp;&nbsp;</td>
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
                                                    <asp:ImageButton runat="server" ID="imgbWebTabellenVisible" ImageUrl="../Images/plus.gif" />
                                                    <span lang="de">&nbsp;<strong>Web-Bapi Tabellen</strong>&nbsp;&nbsp;&nbsp;</span>
                                                </td>
                                                <td align="left" valign="bottom">
                                                    &nbsp;
                                                    <asp:Label ID="lblWebTabellenInfo" Font-Bold="True" runat="server"></asp:Label>
                                                    &nbsp;<asp:Label ID="lblWebTabellenNoData" runat="server" Visible="False"></asp:Label>
                                                    <span lang="de">&nbsp;<asp:Label ID="lblWebTabellenError" runat="server" EnableViewState="False"
                                                        CssClass="TextError"></asp:Label></span>
                                                </td>
                                                <td align="right">
                                                    &nbsp;</td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="panelWebTabellen" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                            BorderWidth="2" Width="100%" runat="server" Height="100%">
                                            <asp:DataGrid ID="WebTabellenDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                AllowSorting="True">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="imgbWebTabellenTabelleVisible" 
                                                                CommandName="Visible" ImageUrl="../Images/plus.gif" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn ItemStyle-Width="50%" DataField="TabellenName" HeaderText="Tabellen Name">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn ItemStyle-Width="50%" DataField="Tabellengroesse" HeaderText="Tabellengröße">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="imgbExcelFuerTabelle" Width="20px" Height="20px"
                                                                CommandName="Excel" ImageUrl="../Images/excel.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.TabellenName") %>' />
                                                            <asp:Literal Text='</TD></TR><TR align="center"><TD colspan="4">' ID="litHirarBeginn"
                                                                runat="server" EnableViewState="true">
                                                            </asp:Literal>
                                                            <asp:DataGrid ID="DataGridLvL2" Visible="false" runat="server" AutoGenerateColumns="False"
                                                                BorderWidth="1" BackColor="Transparent" Width="80%">
                                                                <ItemStyle CssClass="TextLarge"></ItemStyle>
                                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="SpaltenName" HeaderText="Spalten Name"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Laenge" HeaderText="Länge"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:Literal Text='</TD></TR>' ID="litHirarEnd" runat="server" EnableViewState="true">
																		
                                                            </asp:Literal>
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
                                    <td colspan="3" >
                                        &nbsp;
                                    </td>
                                </tr>
                             
                               
                            </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="border-top-color: red; border-top-style: solid; border-width: 3;">
                            &nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="3" style="border-bottom-color: blue; border-bottom-style: solid; border-width: 3;">
                            &nbsp;
                        </td>
                    </tr>
                    
                    
                    
                    <tr>
                        <td colspan="3">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr class="TextLarge">
                                    <td nowrap="nowrap" valign="bottom" width="15%">
                                        <asp:ImageButton runat="server" ID="imgbSAPBapiVisible" ImageUrl="../Images/minus.gif" />
                                        <span lang="de">&nbsp;<strong><span class="style1">SAP-Bapi Struktur&nbsp;</span></strong>&nbsp;&nbsp;</span>
                                    </td>
                                    <td align="left" valign="bottom">
                                        &nbsp;
                                        <asp:Label ID="lblSAPBapiName" Font-Bold="True" runat="server" ForeColor="Blue"></asp:Label>
                                        &nbsp;<asp:Label ID="lblSAPBapiDatum" Font-Bold="True" runat="server" 
                                            ForeColor="Blue"></asp:Label>
                                        <span lang="de">&nbsp;</span><asp:Label ID="lblSAPBapiNoData" runat="server" Visible="False"></asp:Label>
                                        <span lang="de">&nbsp;<asp:Label ID="lblSAPBapiError" runat="server" EnableViewState="False"
                                            CssClass="TextError"></asp:Label></span>
                                    </td>
                                    <td align="right">
                                    </td>
                                </tr>
                                <tr><td colspan="3">&nbsp;</td></tr>
                            </table>
                            <asp:Panel ID="panelSAPBapi" BorderColor="Gray" BorderStyle="Solid" BorderWidth="0"
                                Width="100%" runat="server" Height="100%">
                                <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr class="TextLarge">
                                                    <td nowrap="nowrap" valign="bottom" width="15%">
                                                        <asp:ImageButton runat="server" ID="imgbSAPImportVisible" ImageUrl="../Images/plus.gif" />
                                                        <span lang="de">&nbsp;<strong>SAP-Bapi Import&nbsp;</strong>&nbsp;&nbsp;</span>
                                                    </td>
                                                    <td align="left" valign="bottom">
                                                        &nbsp;
                                                        <asp:Label ID="lblSAPImportInfo" Font-Bold="True" runat="server"></asp:Label>
                                                        &nbsp;<asp:Label ID="lblSAPImportNoData" runat="server" Visible="False"></asp:Label>
                                                        <span lang="de">&nbsp;<asp:Label ID="lblSAPImportError" runat="server" EnableViewState="False"
                                                            CssClass="TextError"></asp:Label></span>
                                                    </td>
                                                    <td align="right">
                                                        <asp:ImageButton ID="imgbSAPImportParameter" runat="server" Height="20px" ImageUrl="../Images/excel.gif"
                                                            Visible="True" Width="20px" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="panelSAPImport" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                                BorderWidth="2" Width="100%" runat="server" Height="100%">
                                                <asp:DataGrid ID="SAPImportDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                    AllowSorting="True">
                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                    <Columns>
                                                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imgbSAPImportTabelleVisible" Visible='<%# DataBinder.Eval(Container, "DataItem.ParameterDATATYPE")="Tabelle" %>'
                                                                    CommandName="Visible" ImageUrl="../Images/plus.gif" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn ItemStyle-Width="34%" DataField="PARAMETER" HeaderText="Parameter Name">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn ItemStyle-Width="33%" DataField="ParameterDATATYPE" HeaderText="Parameter Datentyp">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn ItemStyle-Width="33%" DataField="ParameterLength" HeaderText="Parameter Länge">
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imgbExcelFuerTabelle" Width="20px" Height="20px"
                                                                    CommandName="Excel" Visible='<%# DataBinder.Eval(Container, "DataItem.ParameterDATATYPE")="Tabelle" %>'
                                                                    ImageUrl="../Images/excel.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PARAMETER") %>' />
                                                                <asp:Literal Text='</TD></TR><TR align="center"><TD colspan="4">' ID="litHirarBeginn"
                                                                    runat="server" EnableViewState="true">
                                                                </asp:Literal>
                                                                <asp:DataGrid ID="DataGridLvL2" runat="server" Visible="false" AutoGenerateColumns="False"
                                                                    BorderWidth="1" BackColor="Transparent" Width="80%">
                                                                    <ItemStyle CssClass="TextLarge"></ItemStyle>
                                                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="SpaltenName" HeaderText="Spalten Name"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Laenge" HeaderText="Länge"></asp:BoundColumn>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                                <asp:Literal Text='</TD></TR>' ID="litHirarEnd" runat="server" EnableViewState="true">
																		
                                                                </asp:Literal>
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
                                                        <asp:ImageButton runat="server" ID="imgbSAPExportVisible" ImageUrl="../Images/plus.gif" />
                                                        <span lang="de"><strong>&nbsp;SAP-Bapi Export&nbsp;</strong>&nbsp;&nbsp;</span>
                                                    </td>
                                                    <td align="left" valign="bottom">
                                                        &nbsp;
                                                        <asp:Label ID="lblSAPExportInfo" Font-Bold="True" runat="server"></asp:Label>
                                                        &nbsp;<asp:Label ID="lblSAPExportNoData" runat="server" Visible="False"></asp:Label>
                                                        <span lang="de">&nbsp;<asp:Label ID="lblSAPExportError" runat="server" EnableViewState="False"
                                                            CssClass="TextError"></asp:Label></span>
                                                    </td>
                                                    <td align="right">
                                                        <asp:ImageButton ID="imgbSAPExportParameter" runat="server" Height="20px" ImageUrl="../Images/excel.gif"
                                                            Visible="True" Width="20px" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="panelSAPExport" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                                BorderWidth="2" Width="100%" runat="server" Height="100%">
                                                <asp:DataGrid ID="SAPExportDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                    AllowSorting="True">
                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                    <Columns>
                                                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imgbSAPExportTabelleVisible" Visible='<%# DataBinder.Eval(Container, "DataItem.ParameterDATATYPE")="Tabelle" %>'
                                                                    CommandName="Visible" ImageUrl="../Images/plus.gif" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn ItemStyle-Width="34%" DataField="PARAMETER" HeaderText="Parameter Name">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn ItemStyle-Width="33%" DataField="ParameterDATATYPE" HeaderText="Parameter Datentyp">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn ItemStyle-Width="33%" DataField="ParameterLength" HeaderText="Parameter Länge">
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imgbExcelFuerTabelle" Width="20px" Height="20px"
                                                                    CommandName="Excel" Visible='<%# DataBinder.Eval(Container, "DataItem.ParameterDATATYPE")="Tabelle" %>'
                                                                    ImageUrl="../Images/excel.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PARAMETER") %>' />
                                                                <asp:Literal Text='</TD></TR><TR align="center"><TD colspan="4">' ID="litHirarBeginn"
                                                                    runat="server" EnableViewState="true">
                                                                </asp:Literal>
                                                                <asp:DataGrid ID="DataGridLvL2" Visible="false" runat="server" AutoGenerateColumns="False"
                                                                    BorderWidth="1" BackColor="Transparent" Width="80%">
                                                                    <ItemStyle CssClass="TextLarge"></ItemStyle>
                                                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="SpaltenName" HeaderText="Spalten Name"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Laenge" HeaderText="Länge"></asp:BoundColumn>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                                <asp:Literal Text='</TD></TR>' ID="litHirarEnd" runat="server" EnableViewState="true">
																		
                                                                </asp:Literal>
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
                                                        <asp:ImageButton runat="server" ID="imgbSAPTabellenVisible" ImageUrl="../Images/plus.gif" />
                                                        <span lang="de"><strong>&nbsp;SAP-Bapi Tabellen&nbsp;</strong>&nbsp;&nbsp;</span>
                                                    </td>
                                                    <td align="left" valign="bottom">
                                                        &nbsp;
                                                        <asp:Label ID="lblSAPTabellenInfo" Font-Bold="True" runat="server"></asp:Label>
                                                        &nbsp;<asp:Label ID="lblSAPTabellenNoData" runat="server" Visible="False"></asp:Label>
                                                        <span lang="de">&nbsp;<asp:Label ID="lblSAPTabellenError" runat="server" EnableViewState="False"
                                                            CssClass="TextError"></asp:Label></span>
                                                    </td>
                                                    <td align="right">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="panelSAPTabellen" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                                BorderWidth="2" Width="100%" runat="server" Height="100%">
                                                <asp:DataGrid ID="SAPTabellenDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                    AllowSorting="True">
                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                    <Columns>
                                                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imgbSAPTabellenTabelleVisible" CommandName="Visible"
                                                                    ImageUrl="../Images/plus.gif" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn ItemStyle-Width="50%" DataField="TabellenName" HeaderText="Tabellen Name">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn ItemStyle-Width="50%" DataField="Tabellengroesse" HeaderText="Tabellengröße">
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imgbExcelFuerTabelle" Width="20px" Height="20px"
                                                                    CommandName="Excel" ImageUrl="../Images/excel.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.TabellenName") %>' />
                                                                <asp:Literal Text='</TD></TR><TR align="center"><TD colspan="4">' ID="litHirarBeginn"
                                                                    runat="server" EnableViewState="true">
                                                                </asp:Literal>
                                                                <asp:DataGrid ID="DataGridLvL2" Visible="false" runat="server" AutoGenerateColumns="False"
                                                                    BorderWidth="1" BackColor="Transparent" Width="80%">
                                                                    <ItemStyle CssClass="TextLarge"></ItemStyle>
                                                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="SpaltenName" HeaderText="Spalten Name"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Laenge" HeaderText="Länge"></asp:BoundColumn>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                                <asp:Literal Text='</TD></TR>' ID="litHirarEnd" runat="server" EnableViewState="true">
																		
                                                                </asp:Literal>
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
                                        <td colspan="3" >
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" valign="top" align="left" style="border-top-color: blue; border-top-style: solid;
                border-width: 3;">
                <!--#include File="../PageElements/Footer.html" -->
             </td>
        </tr>
    </table>
    </form>
</body>
</html>
