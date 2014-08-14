<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowBapis.aspx.vb" Inherits="Admin.ShowBapis" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
            </div>
            <div id="innerContent" style="width:95%">
                <div id="innerContentRight" >
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                       <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1"  runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td nowrap="nowrap" class="firstLeft active">
                                                </td>
                                                <td class="firstLeft active" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">

                                                <td colspan="3" align="center">
                                                   Bapi Name:&nbsp;<asp:TextBox ID="txtFilter" runat="server" Text="**" Width="250px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="imgbSetFilter" runat="server" Height="20px"
                                                        ImageUrl="Images/Filter.gif" Visible="True" Width="20px" />
                                                    &nbsp; &nbsp;<asp:ImageButton ID="imgbLookSAP" runat="server"  ImageUrl="Images/SAPLogo.gif"
                                                        Visible="True"  />
                                                           
                                                </td>

                                            </tr>

                                            <tr class="formquery">
                                            <td colspan="3" align="center">
                                                <b>Web<span lang="de">-</span>Bapis
                                                    <asp:Label ID="lblWebBapisError" CssClass="TextError"  runat="server"></asp:Label>
                                                    <span lang="de">&nbsp;<asp:Label ID="lblWebBapisNoData" runat="server" Visible="False"></asp:Label></span>&nbsp;<asp:Label
                                                        ID="lblWebBapisInfo" runat="server" Font-Bold="True"></asp:Label>
                                                </b>
                                          
                                            </td>
                                            </tr><tr class="formquery">
                        <td colspan="3" align="center">
                        <div style="height: 250px; overflow: auto; width:618px;">
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
																		<img src="Images/SAPLogo.gif" alt="SAP" border="0"></asp:LinkButton>
                                            &nbsp;&nbsp;<asp:LinkButton ID="lbLookWEB" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'
                                                CommandName="ShowWEB" >
																		<img src="Images/dotNet.jpeg" alt="NET" border="0"></asp:LinkButton>
																		
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
                                    <asp:TemplateColumn HeaderText="col_TestSap">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_TestSap" runat="server" CommandName="Sort" CommandArgument="TestSap">col_TestSap</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TestSap") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="col_SourceModule">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_SourceModule" runat="server" CommandName="Sort" CommandArgument="SourceModule">col_SourceModule</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SourceModule") %>'>
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
                            </asp:DataGrid></div>
                        </td></tr></tbody>
                        </table>
                                                        </div>
                    <div id="data">
                    <table id="tab2" runat="server" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="3">
                            <table cellspacing="0" cellpadding="0" width="90%" border="0">
                                <tr class="TextLarge">
                                    <td nowrap="nowrap" valign="bottom" width="15%">
                                        <asp:ImageButton runat="server" ID="imgbWebBapiVisible" 
                                            ImageUrl="Images/minus.gif" />
                                        <span lang="de">&nbsp;<span class="style1"><strong>Web-Bapi Struktur</strong></span>&nbsp;&nbsp;&nbsp;</span>
                                    </td>
                                    <td align="left" valign="bottom" style="width:100%">
                                        &nbsp;
                                        <asp:Label ID="lblWebBapiName" Font-Bold="True" runat="server" ForeColor="Red"></asp:Label>
                                        &nbsp;<asp:Label ID="lblWebBapiDatum" Font-Bold="True" runat="server" 
                                            ForeColor="Red"></asp:Label>
                                        <span lang="de">&nbsp;<asp:Label ID="lblWebBapiError" runat="server" EnableViewState="False"
                                            CssClass="TextError"></asp:Label></span>
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
                                
                            <table id="TableKleinerAbstandVorGrid" cellspacing="0" cellpadding="0" width="90%"
                                border="0">
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr class="TextLarge">
                                                <td nowrap="nowrap" valign="bottom" width="15%">
                                                    <asp:ImageButton runat="server" ID="imgbWebImportVisible" 
                                                        ImageUrl="Images/Plus1.gif" />
                                                    <span lang="de">&nbsp;<strong>Web-Bapi Import</strong>&nbsp;&nbsp;&nbsp;</span>
                                                </td>
                                                <td align="left" valign="bottom" style="width:65%">
                                                    &nbsp;
                                                    <asp:Label ID="lblWebImportInfo" Font-Bold="True" runat="server"></asp:Label>
                                                    &nbsp;<asp:Label ID="lblWebImportNoData" runat="server" Visible="False"></asp:Label>
                                                    <span lang="de">&nbsp;<asp:Label ID="lblWebImportError" runat="server" EnableViewState="False"
                                                        CssClass="TextError"></asp:Label></span>
                                                </td>
                                                <td align="right">
                                                    <asp:ImageButton ID="imgbWebImportParameter" runat="server" Height="20px" ImageUrl="Images/iconxls.gif"
                                                        Visible="True" Width="20px" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="panelWebImport"  Visible="false" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2"
                                            Width="80%" runat="server" Height="100%">
                                            <asp:DataGrid ID="WebImportDG" runat="server" CssClass="GridView" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                AllowSorting="True">
                                                <ItemStyle CssClass ="ItemStyle" Width="100px" />
                                                <AlternatingItemStyle CssClass="GridTableAlternate" Width="100px"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead" Width="100px"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center"  ItemStyle-Width ="100px"  ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate >
                                                            <asp:ImageButton runat="server" ID="imgbWebImportTabelleVisible" Visible='<%# DataBinder.Eval(Container, "DataItem.ParameterDATATYPE")="Tabelle" %>'
                                                                CommandName="Visible" ImageUrl="Images/Plus1.gif" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn ItemStyle-Width ="100px"  DataField="PARAMETER" HeaderText="Parameter Name"></asp:BoundColumn>
                                                    <asp:BoundColumn   ItemStyle-Width ="100px" DataField="ParameterDATATYPE" HeaderText="Parameter Datentyp"></asp:BoundColumn>
                                                    <asp:BoundColumn  ItemStyle-Width ="100px"  DataField="ParameterLength" HeaderText="Parameter Länge">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn Visible="true"  ItemStyle-Width ="100px"  ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate   >
                                                            <asp:ImageButton runat="server" ID="imgbExcelFuerTabelle" Width="20px" Height="20px"
                                                                CommandName="Excel" Visible='<%# DataBinder.Eval(Container, "DataItem.ParameterDATATYPE")="Tabelle" %>'
                                                                ImageUrl="Images/iconxls.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PARAMETER") %>' />
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
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr class="TextLarge">
                                                <td nowrap="nowrap" valign="bottom" width="15%">
                                                    <asp:ImageButton runat="server" ID="imgbWebExportVisible" ImageUrl="Images/Plus1.gif" />
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
                                                    <asp:ImageButton ID="imgbWebExportParameter" runat="server" Height="20px" ImageUrl="Images/iconxls.gif"
                                                        Visible="True" Width="20px" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="panelWebExport" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                            BorderWidth="2" Width="80%" runat="server" Height="100%">
                                            <asp:DataGrid ID="WebExportDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                AllowSorting="True">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="imgbWebExportTabelleVisible" Visible='<%# DataBinder.Eval(Container, "DataItem.ParameterDATATYPE")="Tabelle" %>'
                                                                CommandName="Visible" ImageUrl="Images/Plus1.gif" />
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
                                                                ImageUrl="Images/iconxls.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PARAMETER") %>' />
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
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:80%">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr class="TextLarge">
                                                <td nowrap="nowrap" valign="bottom" width="15%">
                                                    <asp:ImageButton runat="server" ID="imgbWebTabellenVisible" ImageUrl="Images/Plus1.gif" />
                                                    <span lang="de">&nbsp;<strong>Web-Bapi Tabellen</strong>&nbsp;&nbsp;&nbsp;</span>
                                                </td>
                                                <td align="left" valign="bottom" colspan="3">
                                                    &nbsp;
                                                    <asp:Label ID="lblWebTabellenInfo" Font-Bold="True" runat="server"></asp:Label>
                                                    &nbsp;<asp:Label ID="lblWebTabellenNoData" runat="server" Visible="False"></asp:Label>
                                                    <span lang="de">&nbsp;<asp:Label ID="lblWebTabellenError" runat="server" EnableViewState="False"
                                                        CssClass="TextError"></asp:Label></span>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="panelWebTabellen" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                            BorderWidth="2" Width="80%" runat="server" Height="100%">
                                            <asp:DataGrid ID="WebTabellenDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                AllowSorting="True">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="imgbWebTabellenTabelleVisible" 
                                                                CommandName="Visible" ImageUrl="Images/Plus1.gif" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn ItemStyle-Width="50%" DataField="TabellenName" HeaderText="Tabellen Name">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn ItemStyle-Width="50%" DataField="Tabellengroesse" HeaderText="Tabellengröße">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="imgbExcelFuerTabelle" Width="20px" Height="20px"
                                                                CommandName="Excel" ImageUrl="Images/iconxls.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.TabellenName") %>' />
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
                                    <td colspan="2" >
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
                                        <asp:ImageButton runat="server" ID="imgbSAPBapiVisible" ImageUrl="Images/minus.gif" />
                                        <span lang="de">&nbsp;<strong><span class="style1">SAP-Bapi Struktur&nbsp;</span></strong>&nbsp;&nbsp;</span>
                                    </td>
                                    <td align="left" valign="bottom" colspan="3" Style="width:100%">
                                        &nbsp;
                                        <asp:Label ID="lblSAPBapiName" Font-Bold="True" runat="server" ForeColor="Blue"></asp:Label>
                                        &nbsp;<asp:Label ID="lblSAPBapiDatum" Font-Bold="True" runat="server" 
                                            ForeColor="Blue"></asp:Label>
                                        <span lang="de">&nbsp;</span><asp:Label ID="lblSAPBapiNoData" runat="server" Visible="False"></asp:Label>
                                        <span lang="de">&nbsp;<asp:Label ID="lblSAPBapiError" runat="server" EnableViewState="False"
                                            CssClass="TextError"></asp:Label></span>
                                    </td>
       
                                </tr>
                                <tr><td colspan="3">&nbsp;</td></tr>
                            </table>
                            <asp:Panel ID="panelSAPBapi" BorderColor="Gray" BorderStyle="Solid" BorderWidth="0"
                                Width="80%" runat="server" Height="100%">
                                <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                         <td>
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr class="TextLarge">
                                                    <td nowrap="nowrap" valign="bottom" width="15%">
                                                        <asp:ImageButton runat="server" ID="imgbSAPImportVisible" ImageUrl="Images/Plus1.gif" />
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
                                                        <asp:ImageButton ID="imgbSAPImportParameter" runat="server" Height="20px" ImageUrl="Images/iconxls.gif"
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
                                                                    CommandName="Visible" ImageUrl="Images/Plus1.gif" />
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
                                                                    ImageUrl="Images/iconxls.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PARAMETER") %>' />
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
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr class="TextLarge">
                                                    <td nowrap="nowrap" valign="bottom" width="15%">
                                                        <asp:ImageButton runat="server" ID="imgbSAPExportVisible" ImageUrl="Images/Plus1.gif" />
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
                                                        <asp:ImageButton ID="imgbSAPExportParameter" runat="server" Height="20px" ImageUrl="Images/iconxls.gif"
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
                                                                    CommandName="Visible" ImageUrl="Images/Plus1.gif" />
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
                                                                    ImageUrl="Images/iconxls.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PARAMETER") %>' />
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
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr class="TextLarge">
                                                    <td nowrap="nowrap" valign="bottom" width="15%">
                                                        <asp:ImageButton runat="server" ID="imgbSAPTabellenVisible" ImageUrl="Images/Plus1.gif" />
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
                                                                    ImageUrl="Images/Plus1.gif" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn ItemStyle-Width="50%" DataField="TabellenName" HeaderText="Tabellen Name">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn ItemStyle-Width="50%" DataField="Tabellengroesse" HeaderText="Tabellengröße">
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imgbExcelFuerTabelle" Width="20px" Height="20px"
                                                                    CommandName="Excel" ImageUrl="Images/iconxls.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.TabellenName") %>' />
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
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>

                            </asp:Panel>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">Suchen »</asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div> 
            </div>
        </div>
    </div>
</asp:Content>
