<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BapiOverView.aspx.vb"
    Inherits="Admin.BapiOverView" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    OnClick="responseBack" Text="Zurück" />
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label" />
                        </h1>
                    </div>
                    <script type="text/javascript">
                        function onRequestStart(sender, args) {
                            if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                                            args.get_eventTarget().indexOf("ExportToPdfButton") >= 0 ||
                                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                                args.set_enableAjax(false);
                            }
                        }
                    </script>
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                        <ClientEvents OnRequestStart="onRequestStart" />
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="DBDG" />
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false" />
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False" />
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Bapi Name:
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtFilter" runat="server" CssClass="InputTextbox" Text="**"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../images/empty.gif"
                                                        Width="1px" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>
                            </asp:Panel>
                            <div id="dataQueryFooter" style="margin-bottom: 10px">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">Suchen »</asp:LinkButton>
                                <asp:LinkButton ID="lbClearBapiStruktur" runat="server" 
                                    CssClass="TablebuttonXXXLarge" Width="175px" Height="16px">BapiStruktur-Tabelle leeren »</asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server">
                                <div class="divBapiOverView">
                                    <table cellspacing="0" cellpadding="0" style="width: 100%" border="0">
                                        <tr>
                                            <td nowrap="nowrap" valign="bottom" style="width: 25%">
                                                <asp:ImageButton runat="server" ID="imgbDBVisible" Height="16px" Width="16px" ImageUrl="../Images/minus.gif" />
                                                <span lang="de">&nbsp;DB-Overview&nbsp;&nbsp;&nbsp;</span>
                                            </td>
                                            <td align="left" valign="bottom">
                                                <asp:Label ID="lblDBInfo" Font-Bold="True" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="active" align="left" colspan="2" valign="bottom">
                                                <asp:Label ID="lblDBNoData" Font-Bold="True" runat="server" />&nbsp;
                                                <asp:Label ID="lblDBError" Font-Bold="True" runat="server" EnableViewState="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Panel ID="PanelDB" runat="server">
                                    <div>
                                        <telerik:RadGrid ID="DBDG" runat="server" Width="100%" CellPadding="0" AllowSorting="True"
                                            Visible="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                            BorderStyle="None" Culture="de-DE" OnNeedDataSource="DBDG_NeedDataSource" OnItemCommand="DBDG_ItemCommand"
                                            PageSize="20">
                                            <ClientSettings>
                                                <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" />
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="600" />
                                            </ClientSettings>
                                            <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" ExportOnlyData="true"
                                                IgnorePaging="true" FileName="DB-Overview" />
                                            <ItemStyle Wrap="false" />
                                            <FooterStyle CssClass="RADGridFooter" />
                                            <PagerStyle CssClass="RADGridFooter" AlwaysVisible="true" Mode="NextPrevAndNumeric"
                                                PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="BapiName">
                                                <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                                    ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                                <%-- TODO: re-Enable Export To Excel --%>
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="BapiName" SortExpression="BapiName" UniqueName="BapiName"
                                                        HeaderText="BapiName" />
                                                    <telerik:GridBoundColumn DataField="SourceModule" SortExpression="SourceModule" UniqueName="SourceModule"
                                                        HeaderText="Modul" />
                                                    <telerik:GridBoundColumn DataField="BapiDate" SortExpression="BapiDate" UniqueName="BapiDate"
                                                        HeaderText="BapiDate" DataFormatString="{0:d}" />
                                                    <telerik:GridBoundColumn DataField="inserted" SortExpression="inserted" UniqueName="inserted"
                                                        HeaderText="inserted" />
                                                    <telerik:GridBoundColumn DataField="updated" SortExpression="updated" UniqueName="updated"
                                                        HeaderText="updated" />
                                                    <telerik:GridTemplateColumn SortExpression="BapiName" HeaderText="Details" DataField="Details">
                                                        <HeaderStyle Width="50px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbDetails" CommandArgument='<%# Eval("BapiName") %>'
                                                                runat="server" Width="16px" CommandName="Details" Height="16px">
                                                                <img src="../Images/Lupe_01.gif" width="16px" height="16px" alt="Details" border="0"/>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn SortExpression="BapiName" HeaderText="Delete" DataField="Delete">
                                                        <HeaderStyle Width="50px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbDelete" runat="server" Width="10px" CommandArgument='<%# Eval("BapiName") & "," & Eval("SourceModule") %>'
                                                                CommandName="Delete" Height="10px">
                                                                <img src="../Images/Papierkorb_01.gif" width="16px" height="16px" alt="Löschen" border="0" />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </asp:Panel>
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
