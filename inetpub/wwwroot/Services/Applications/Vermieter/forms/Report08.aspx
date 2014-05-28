<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report08.aspx.cs" Inherits="Vermieter.forms.Report08"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active" style="width: 25px;">
                                        <asp:ImageButton ID="NewSearch" runat="server" Width="17px" OnClick="NewSearch_Click" />
                                    </td>
                                    <td align="left" class="active" style="vertical-align: middle;">
                                        Abfrageoptionen
                                    </td>
                                    <td class="active" style="width: 25px;" align="right">
                                        <asp:ImageButton ID="NewSearch2" runat="server" Width="17px" OnClick="NewSearch_Click" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="divError" runat="server" enableviewstate="false" style="padding: 10px 0px 10px 15px;
                        margin-top: 10px;">
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        <asp:Label ID="lblNoData" runat="server" ForeColor="Blue" EnableViewState="False"></asp:Label>
                    </div>
                    <asp:Panel ID="Panel1" runat="server">
                        <div id="TableQuery">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Kunde" runat="server">lbl_Kunde</asp:Label>
                                    </td>
                                    <td class="active" style="width: 100%">
                                        <span>
                                            <asp:DropDownList runat="server" ID="ddlCustomer" Width="400px"/>
                                        </span>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        &nbsp;
                                    </td>
                                    <td class="active" style="width: 100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2" align="right" style="width: 100%">
                                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                            &nbsp;
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                    Height="16px" CausesValidation="False" Font-Underline="False" OnClick="cmdSearch_Click">» Suchen</asp:LinkButton>
                                <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btndefault" runat="server"
                                    Text="Button" />
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="/services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel" runat="server" OnClick="lnkCreateExcel_Click">Excel herunterladen</asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server" />
                        </div>
                        <div id="data">
                            <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                Width="970px" OnSorting="GridView1_Sorting">
                                <PagerSettings Visible="False" />
                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                <RowStyle CssClass="ItemStyle" />
                                <Columns>
                                    <asp:TemplateField SortExpression="Loeschen" HeaderText="col_Loeschen">
                                        <HeaderTemplate>
                                            <asp:Linkbutton ID="col_Loeschen" runat="server" Text="col_Loeschen" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbxDelete" runat="server" Visible='<%# Bind("DeleteEnable") %>'
                                                Checked='<%# Bind("Delete") %>' />
                                            <asp:Label ID="lblKennzeichnung" runat="server" Visible="false" Text='<%# Bind("KENNZEICHNG") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Typ" HeaderText="col_Typ">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="Typ"
                                                Text="col_Typ" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTyp" Text='<%# DataBinder.Eval(Container, "DataItem.Typ") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Komponentenbezeichnung" HeaderText="col_Komponentenbezeichnung">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Komponentenbezeichnung" runat="server" CommandName="Sort"
                                                CommandArgument="Komponentenbezeichnung" Text="col_Komponentenbezeichnung" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblKomponentenbezeichnung" Text='<%# DataBinder.Eval(Container, "DataItem.Komponentenbezeichnung") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Komponentennummer" HeaderText="col_Komponentennummer">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Komponentennummer" runat="server" CommandName="Sort" CommandArgument="Komponentennummer"
                                                Text="col_Komponentennummer" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblKomponentennummer" Text='<%# DataBinder.Eval(Container, "DataItem.Komponentennummer") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="true" SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer"
                                                Text="col_Vertragsnummer" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblVertragsnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen"
                                                Text="col_Kennzeichen" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer"
                                                Text="col_Fahrgestellnummer" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Versandadresse" HeaderText="col_Versandadresse">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Versandadresse" runat="server" CommandName="Sort" CommandArgument="Versandadresse"
                                                Text="col_Versandadresse" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblVersandadresse" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Beauftragung" HeaderText="col_Beauftragung">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Beauftragung" runat="server" CommandName="Sort" CommandArgument="Beauftragung"
                                                Text="col_Beauftragung" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblBeauftragung" Text='<%# DataBinder.Eval(Container, "DataItem.Beauftragung","{0:dd.MM.yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Fehlertext" HeaderText="col_Fehlertext">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Fehlertext" runat="server" CommandName="Sort" CommandArgument="Fehlertext"
                                                Text="col_Fehlertext" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFehlertext" Text='<%# DataBinder.Eval(Container, "DataItem.Fehlertext") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Treuhandgeber" HeaderText="col_Treuhandgeber">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Treuhandgeber" runat="server" CommandName="Sort" CommandArgument="Treuhandgeber"
                                                Text="col_Treuhandgeber" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTreuhandgeber" Text='<%# DataBinder.Eval(Container, "DataItem.Treuhandgeber") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flag_Briefversand" Visible="False">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Flag_Briefversand") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Flag_Briefversand") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flag_Schluesselversand" Visible="False">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Flag_Schluesselversand") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Flag_Schluesselversand") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Status" HeaderText="Status" Visible="False" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAnforderungsnummer" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.Anforderungsnummer") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="Div1">
                        </div>
                    </div>
                    <div id="dataFooter">
                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="Tablebutton" Width="78px"
                            Visible="false" OnClick="btnDelete_Click" Text="» Speichern" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
