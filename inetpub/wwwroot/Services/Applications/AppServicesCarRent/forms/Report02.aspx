<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02.aspx.vb" Inherits="AppServicesCarRent.Report02"
    MasterPageFile="../MasterPage/App.Master" %>

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
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="ibtShowSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <asp:Panel ID="divTrenn" runat="server" Visible="false">
                                    <div id="PlaceHolderDiv">
                                    </div>
                                </asp:Panel>
                            </div>
                            <asp:Panel ID="divSelection" runat="server" DefaultButton="lbcreate">
                                <div id="TableQuery">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2" style="width: 100%">
                                                <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                <asp:Label ID="lblDateVon" runat="server" Text="Eingangsdatum von:"></asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox ID="txtDateVon" runat="server"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtDateVon_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtDateVon" Animated="False">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                <asp:Label ID="lblDateBis" runat="server" Text="Eingangsdatum bis:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtDateBis" runat="server"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtDateBis_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtDateBis" Animated="False">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap" colspan="2">
                                                Datumsbereich darf nicht größer als 2 Monate sein.
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2" style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>
                                <div id="dataQueryFooter">
                                    <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                                </div>
                            </asp:Panel>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server">
                                    </uc2:GridNavigation>
                                </div>
                                <div id="data" style="overflow-x: auto; overflow-y: hidden;">
                                    <div style="margin-bottom: 0; padding-bottom: 15px;">
                                        <table cellspacing="0" cellpadding="0" bgcolor="white" border="0" style="margin-bottom: 0;">
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                        CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                        PageSize="20" CssClass="GridView" Style="margin-bottom: 0px;">
                                                        <%--  --%>
                                                        <PagerSettings Visible="false" />
                                                        <HeaderStyle CssClass="GridTableHead" />
                                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                        <RowStyle Wrap="false" CssClass="ItemStyle" />
                                                        <EditRowStyle Wrap="False"></EditRowStyle>
                                                        <Columns>
                                                            <asp:TemplateField SortExpression="Eingangsdatum" HeaderText="col_Eingangsdatum">
                                                                <HeaderStyle Width="90px" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Eingangsdatum" runat="server" CommandArgument="Eingangsdatum"
                                                                        CommandName="sort">col_Eingangsdatum</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum","{0:d}") %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum","{0:d}") %>'
                                                                            ID="lblEingangsdatum" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="FahrgestellNr" HeaderText="col_FahrgestellNr">
                                                                <HeaderStyle Width="130px" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_FahrgestellNr" runat="server" CommandArgument="FahrgestellNr"
                                                                        CommandName="sort">col_FahrgestellNr</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FahrgestellNr") %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container, "DataItem.FahrgestellNr") %>' ID="lblFahrgestellNr" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="FahrzeugNr" HeaderText="col_FahrzeugNr">
                                                                <HeaderStyle Width="80px" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_FahrzeugNr" runat="server" CommandArgument="FahrzeugNr" CommandName="sort">col_FahrzeugNr</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FahrzeugNr") %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container, "DataItem.FahrzeugNr") %>' ID="lblFahrzeugNr" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Hersteller" HeaderText="col_Hersteller">
                                                                <HeaderStyle Width="90px" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Hersteller" runat="server" CommandArgument="Hersteller" CommandName="sort">col_Hersteller</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Hersteller") %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container, "DataItem.Hersteller") %>' ID="lblHersteller" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZBIINr" HeaderText="col_ZBIINr">
                                                                <HeaderStyle Width="70px" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZBIINr" runat="server" CommandArgument="ZBIINr" CommandName="sort">col_ZBIINr</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZBIINr") %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container, "DataItem.ZBIINr") %>' ID="lblZBIINr" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Handelsname" HeaderText="col_Handelsname">
                                                                <HeaderStyle Width="70px" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Handelsname" runat="server" CommandArgument="Handelsname"
                                                                        CommandName="sort">col_Handelsname</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Handelsname") %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container, "DataItem.Handelsname") %>' ID="lblHandelsname" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="AuftragsNr" HeaderText="col_AuftragsNr">
                                                                <HeaderStyle Width="110px" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_AuftragsNr" runat="server" CommandArgument="AuftragsNr" CommandName="sort">col_AuftragsNr</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AuftragsNr") %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container, "DataItem.AuftragsNr") %>' ID="lblAuftragsNr" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Absender" HeaderText="col_Absender">
                                                                <HeaderStyle Width="110px" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Absender" runat="server" CommandArgument="Absender" CommandName="sort">col_Absender</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Absender") %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container, "DataItem.Absender") %>' ID="lblAbsender" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="DarlehensNr" HeaderText="col_DarlehensNr">
                                                                <HeaderStyle Width="110px" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_DarlehensNr" runat="server" CommandArgument="DarlehensNr"
                                                                        CommandName="sort">col_DarlehensNr</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DarlehensNr") %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container, "DataItem.DarlehensNr") %>' ID="lblDarlehensNr" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                                <HeaderStyle Width="80px" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                                        CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>' ID="lblKennzeichen" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Erstzulassungsdatum" HeaderText="col_Erstzulassungsdatum">
                                                                <HeaderStyle Width="130px" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Erstzulassungsdatum" runat="server" CommandArgument="Erstzulassungsdatum"
                                                                        CommandName="sort">col_Erstzulassungsdatum</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Erstzulassungsdatum","{0:d}") %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container,  "DataItem.Erstzulassungsdatum","{0:d}") %>'
                                                                            ID="lblErstzulassungsdatum" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
