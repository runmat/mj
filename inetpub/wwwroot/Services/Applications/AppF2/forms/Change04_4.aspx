<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_4.aspx.vb" Inherits="AppF2.Change04_4"       MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx">Suche</asp:HyperLink>&nbsp;<asp:HyperLink
                        ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx">| Fahrzeugauswahl</asp:HyperLink>
                    <asp:HyperLink ID="lnkAdressAuswahl" runat="server" NavigateUrl="Change04_3.aspx"
                        > | Adressauswahl</asp:HyperLink>
                    <a class="active">| Abrufgründe</a>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>                        
                        <div id="TableQuery">

                                    <table id="tab1" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trMemo" runat="server" class="formquery">
                                                <td class="firstLeft active">
                                                    Nachricht für Empfänger:&nbsp;
                                                    <asp:TextBox ID="txtKopf" runat="server" Width="247px" MaxLength="200"></asp:TextBox>
                                                </td>
                                                <td class="active" width="100%">
                                                    <asp:LinkButton ID="cmdSaveMemo" runat="server" CssClass="Tablebutton" Width="78px">» Speichern</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr id="ShowScript" runat="server" class="formquery">
                                                <td  class="firstLeft active" style="padding-bottom:10px;">
                                                    Finanzierungsart: &nbsp;
                                                    <asp:DropDownList ID="ddlFinanzierungsart" runat="server" style="margin-left:42px" Width="150px" AutoPostBack="true">
                                                    </asp:DropDownList> &nbsp;
                                                    
                                                </td>
                                                <td class="active" width="100%" style="padding-left:7px">
                                                    <asp:Label ID="lblAbrufgrund" runat="server" Visible="false" Text="Abrufgrund:"></asp:Label>
                                                    &nbsp;
                                                    <asp:DropDownList ID="ddlAbrufgrund" Visible="false" runat="server"
                                                        Width="250px" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>

                        </div>
                        <div id="Result" runat="Server">
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                    <PagerSettings Visible="False" />
                                    <RowStyle CssClass="ItemStyle" />
                                    <EmptyDataRowStyle BackColor="#DFDFDF" />
                                    <Columns>
                                        <asp:TemplateField SortExpression="MANDT" Visible="False" HeaderText="col_MANDT">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_MANDT" runat="server" CommandName="Sort" CommandArgument="MANDT">col_Kontonummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MANDT") %>'
                                                    ID="lblMandt">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="LIZNR" HeaderText="col_Kontonummer">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Kontonummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Kontonummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'
                                                    ID="Label1">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ZZREFERENZ1" HeaderText="col_Vertragsnummer">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Vertragsnummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'
                                                    ID="Label2">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="TIDNR" HeaderText="col_NummerZB2">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'
                                                    ID="Label3">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>'
                                                    ID="lblFahrg">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="LICENSE_NUM"
                                                    CommandName="Sort">col_Kennzeichen</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ZZFINART_BEZ" HeaderText="col_Finart">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Finart" runat="server" CommandArgument="ZZFINART_BEZ"
                                                    CommandName="Sort">col_Finart</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblFinart" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFINART_BEZ") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                        <asp:BoundField Visible="False" DataField="TEXT50" SortExpression="TEXT50" HeaderText="Kopftext">
                                        </asp:BoundField>
                                        <asp:TemplateField Visible="False" SortExpression="ZZBEZAHLT" HeaderText="Bezahlt">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkBezahlt" runat="server" Enabled="False"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ZZCOCKZ">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_COC" runat="server" CommandArgument="LICENSE_NUM" CommandName="Sort">col_COC</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Checkbox2" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'
                                                    Enabled="False"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField  HeaderStyle-ForeColor="White" Visible="False" HeaderText="Versandart">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTemp" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="1" %>'>Temporär</asp:Label>
                                                <asp:Label ID="lblEndg" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="2" %>'>Endgültig</asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderStyle-ForeColor="White" HeaderText="Abrufgrund">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="cmbAbrufgrund" runat="server" DataSource='<%# cmbAbrufgrund_ItemDataBound1( DataBinder.Eval(Container, "DataItem.MANDT"), DataBinder.Eval(Container, "DataItem.ZZFINART_GRP"))%>'
                                                    DataValueField="SapWert" DataTextField="WebBezeichnung" Width="300px" 
                                                    onselectedindexchanged="cmbAbrufgrund_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderStyle-ForeColor="White" HeaderText="Abrufgrund-Info/Text">
                                            <ItemTemplate>
                                                <asp:Label ID="lblZusatzinfo" runat="server" Visible="True" EnableViewState="True"></asp:Label>
                                                <asp:TextBox BorderStyle="Solid" BorderWidth="1" ID="txtZusatztext"
                                                    runat="server" Visible="False" EnableViewState="True" MaxLength="50" Width="250px"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="White" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            *<font face="Arial" size="1">max.&nbsp;23 Zeichen</font></div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdSave2" runat="server" CssClass="Tablebutton" Width="78px">» Weiter</asp:LinkButton></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>                    
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>