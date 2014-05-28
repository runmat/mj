<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report14.aspx.vb" Inherits="AppF2.Report14"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
             <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" Visible="false"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="mahnGrid">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="mahnGrid" />
                                    <telerik:AjaxUpdatedControl ControlID="lblError" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                                    <telerik:AjaxUpdatedControl ControlID="cmdNext" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <asp:Label runat="server" ID="lblError" CssClass="TextError" EnableViewState="false"
                        Visible="false" />
                    <telerik:RadGrid ID="mahnGrid" AllowSorting="true" AllowPaging="true" AllowAutomaticInserts="false"
                        AutoGenerateColumns="false" PageSize="20" runat="server" GridLines="None" Width="100%"
                        BorderWidth="0" Culture="de-DE" Visible="true" OnNeedDataSource="GridNeedDataSource"
                        OnPageIndexChanged="GridPageIndexChanged">
                        <ExportSettings HideStructureColumns="true" />
                        <MasterTableView CommandItemDisplay="Top" Summary="Mahnungen">
                            <PagerStyle Mode="NextPrev" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                            <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="false"
                                ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                            <Columns>
                                <telerik:GridTemplateColumn DataField="MAHNART" SortExpression="MAHNART" UniqueName="MAHNART">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# MahnArtText(Eval("MAHNART")) %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="HAENDLER_EX" SortExpression="HAENDLER_EX" UniqueName="HAENDLER_EX" />
                                <telerik:GridBoundColumn DataField="ZZLSDAT" SortExpression="ZZLSDAT" UniqueName="ZZLSDAT"
                                    DataFormatString="{0:dd.MM.yyyy}" />
                                <telerik:GridBoundColumn DataField="AUGRU_TEXT" SortExpression="AUGRU_TEXT" UniqueName="AUGRU_TEXT"
                                    DataFormatString="<nobr>{0}</nobr>" />
                                <telerik:GridBoundColumn DataField="TIDNR" SortExpression="TIDNR" UniqueName="TIDNR" />
                                <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" UniqueName="CHASSIS_NUM" />
                                <telerik:GridBoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" UniqueName="LICENSE_NUM" />
                                <telerik:GridBoundColumn DataField="OFFENER_BETRAG" SortExpression="OFFENER_BETRAG"
                                    UniqueName="OFFENER_BETRAG" DataFormatString="{0:C}" />
                                <telerik:GridBoundColumn DataField="MELDUNG_AN_AG" SortExpression="MELDUNG_AN_AG"
                                    UniqueName="MELDUNG_AN_AG" DataFormatString="{0:dd.MM.yyyy}" />
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <div id="dataFooter">
                        <asp:LinkButton ID="cmdNext" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                            Enabled="False" CausesValidation="True" Font-Underline="False" OnClick="NextClick">» Weiter</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
