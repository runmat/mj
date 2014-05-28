<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report101s.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Report101s"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>

                 

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="false" />
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ToolTip="Abfrage öffnen" 
                                                        ImageUrl="../../../Images/queryArrow.gif"
                                                        Visible="false" onclick="NewSearch_Click" />
                                                    <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen"
                                                        ImageUrl="../../../Images/queryArrowUp.gif" 
                                                        Visible="false" onclick="NewSearchUp_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                            <%--<asp:Panel runat="server" id="TableQuery" DefaultButton="cmdSearch">--%>
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr class="formquery">
                                        <td colspan="2" class="firstLeft active" style="width: 100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red"></asp:Label>
                                            <asp:Label ID="lblNoData" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="padding-top: 5px;" nowrap="nowrap">
                                            <asp:Label ID="lbl_SelPreselect" runat="server" />
                                        </td>
                                        <td class="active" nowrap="nowrap" style="width: 90%">
                                            <asp:DropDownList ID="ddlPreselect" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="padding-top: 5px;" nowrap="nowrap">
                                            <asp:Label runat="server" ID="lbl_SelKennzeichen" Text="Kennzeichen" />
                                        </td>
                                        <td class="active" nowrap="nowrap" style="width: 90%">
                                            <asp:TextBox ID="txtKennz" runat="server" MaxLength="10" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="padding-top: 5px;" nowrap="nowrap">
                                            <asp:Label runat="server" ID="lbl_SelFahrgestellnummer" Text="Fahrgestellnummer" />
                                        </td>
                                        <td class="active" nowrap="nowrap" style="width: 90%">
                                            <asp:TextBox ID="txtFin" runat="server" MaxLength="17" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="padding-top: 5px;" nowrap="nowrap">
                                            <asp:Label runat="server" ID="lbl_SelReferenz2" Text="Referenznummer 2" />
                                        </td>
                                        <td class="active" nowrap="nowrap" style="width: 90%">
                                            <asp:TextBox ID="txtRef2" runat="server" MaxLength="25" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="2" align="right" class="rightPadding" style="width: 100%">
                                            <div id="dataQueryFooter">
                                                <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                                    Height="16px" CausesValidation="False" Font-Underline="False">» Suchen</asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="2" class="firstLeft active" style="width: 100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            <%--</asp:Panel>--%>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="20" CssClass="GridView">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle Wrap="false" CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="Name_AG" HeaderText="col_Name_AG">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Name_AG" runat="server" CommandName="Sort" CommandArgument="Name_AG">col_Name_AG</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblName_AG" Text='<%# DataBinder.Eval(Container, "DataItem.Name_AG") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Name_TG" HeaderText="col_Name_TG">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Name_TG" runat="server" CommandName="Sort" CommandArgument="Name_TG">col_Name_TG</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblName_TG" Text='<%# DataBinder.Eval(Container, "DataItem.Name_TG") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="TG_NAME2" HeaderText="col_TG_NAME2">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_TG_NAME2" runat="server" CommandName="Sort" CommandArgument="Name_TG">col_TG_NAME2</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblTG_NAME2" Text='<%# DataBinder.Eval(Container, "DataItem.TG_NAME2") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="TG_STREET" HeaderText="col_TG_STREET">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_TG_STREET" runat="server" CommandName="Sort" CommandArgument="TG_STREET">col_TG_NAME2</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblTG_STREET" Text='<%# DataBinder.Eval(Container, "DataItem.TG_STREET") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="TG_POST_CODE1" HeaderText="col_TG_POST_CODE1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_TG_POST_CODE1" runat="server" CommandName="Sort" CommandArgument="TG_POST_CODE1">col_TG_POST_CODE1</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblTG_POST_CODE1" Text='<%# DataBinder.Eval(Container, "DataItem.TG_POST_CODE1") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="TG_CITY1" HeaderText="col_TG_CITY1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_TG_CITY1" runat="server" CommandName="Sort" CommandArgument="TG_CITY1">colTG_CITY1</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblTG_CITY1" Text='<%# DataBinder.Eval(Container, "DataItem.TG_CITY1") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="NummerZBII" HeaderText="col_NummerZBII">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_NummerZBII" runat="server" CommandName="Sort" CommandArgument="NummerZBII">col_NummerZBII</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblNummerZBII" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblVertragsnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Versandstatus" HeaderText="col_Versandstatus">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Versandstatus" runat="server" CommandName="Sort" CommandArgument="Versandstatus">col_Versandstatus</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblVersandstatus" Text='<%# DataBinder.Eval(Container, "DataItem.Versandstatus") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Versandadresse" HeaderText="col_Versandadresse">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Versandadresse" runat="server" CommandName="Sort" CommandArgument="Versandadresse">col_Versandadresse</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblVersandadresse" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="TG" HeaderText="col_TG">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_TG" runat="server" CommandName="Sort" CommandArgument="TG">col_TG</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblTG" Text='<%# DataBinder.Eval(Container, "DataItem.TG") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Referenznummer" HeaderText="col_Referenznummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenznummer" runat="server" CommandName="Sort" CommandArgument="Referenznummer">col_Referenznummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblReferenznummer" Text='<%# DataBinder.Eval(Container, "DataItem.Referenznummer") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:HiddenField ID="hField" runat="server" Value="0" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
