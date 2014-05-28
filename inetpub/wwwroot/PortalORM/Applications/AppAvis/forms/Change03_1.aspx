<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03_1.aspx.vb" Inherits="AppAvis.Change03_1" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
                        <td valign="top">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" >
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="lbWeiter" OnClientClick="Show_BusyBox1();" Text="Daten ergänzen" runat="server"
                                            CssClass="StandardButton"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left" width="100%">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:LinkButton ID="lbBack" Visible="True" runat="server">&#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="LabelExtraLarge">
                                                    <p>
                                                        <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp;<asp:Label
                                                            ID="lblError" runat="server" EnableViewState="true" CssClass="TextError"></asp:Label>&nbsp;</p>
                                                    <p>
                                                        <asp:Label ID="lblInfo" runat="server" EnableViewState="False"></asp:Label></p>
                                                </td>
                                                <td align="right">
                                                    <asp:ImageButton ID="imgbExcel" runat="server" Height="20px" ImageUrl="../../../Images/excel.gif"
                                                        Visible="False" Width="20px" />
                                                    &nbsp;</td>
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
                                        <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" PageSize="50" bodyHeight="400"
                                            Width="100%" AutoGenerateColumns="False" AllowPaging="True" 
                                            AllowSorting="True" Visible="False">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                            <asp:BoundColumn DataField="Fahrgestellnummer" Visible="false"></asp:BoundColumn>
                                                 <asp:TemplateColumn HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxaq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_MVANummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_MVANummer" runat="server" CommandName="Sort" CommandArgument="MVANummer">col_MVANummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelx1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MVANummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Zulassungsdatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Zulassungsdatum" runat="server" CommandName="Sort" CommandArgument="Zulassungsdatum">col_Zulassungsdatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulassungsdatum","{0:dd.MM.yyyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Modell">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Modell" runat="server" CommandName="Sort" CommandArgument="Modell">col_Modell</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Modell") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Modellbezeichnung">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Modellbezeichnung" runat="server" CommandName="Sort" CommandArgument="Modellbezeichnung">col_Modellbezeichnung</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelq1q1a" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Modellbezeichnung") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                
                                                <asp:TemplateColumn HeaderText="col_Herstellernummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Herstellernummer" runat="server" CommandName="Sort" CommandArgument="Herstellernummer">col_Herstellernummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Herstellernummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_geplanterLiefertermin">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_geplanterLiefertermin" runat="server" CommandName="Sort"
                                                            CommandArgument="geplanterLiefertermin">col_geplanterLiefertermin</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.geplanterLiefertermin") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_istbezahlt">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_istbezahlt" CommandArgument="istbezahlt" CommandName="Sort"
                                                            runat="server">col_istbezahlt</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2d" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.istbezahlt") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Sperrdatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Sperrdatum" CommandArgument="Sperrdatum" CommandName="Sort"
                                                            runat="server">col_Sperrdatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2y" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Sperrdatum","{0:dd.MM.yyyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn >
                                                    <HeaderTemplate >
                                                        <asp:LinkButton ID="col_Sperrvermerk" CommandArgument="Sperrvermerk" CommandName="Sort"
                                                            runat="server">col_Sperrvermerk</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2y1sd" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Sperrvermerk") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="entfernen" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lbDelete" runat="server">entfernen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbDelete" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                            CommandName="Delete" Height="10px">
																		<img src="../../../Images/loesch.gif" border="0"></asp:LinkButton>
                                                      
                                                    </ItemTemplate>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Status"  ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton  CommandName="Sort" CommandArgument="Status" ID="col_Status" runat="server">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                    <div style="text-align:left">
                                                        <asp:Label ID="lblMessage"  EnableViewState="true" runat="server" >
                                                        </asp:Label>
                                                    </div>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
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
