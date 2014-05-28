<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report13.aspx.vb" Inherits="AppF2.Report13"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" CausesValidation="false"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <uc1:Kopfdaten ID="kopfdaten" runat="server" />
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
                            <telerik:AjaxSetting AjaxControlID="versendungenGrid">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="versendungenGrid" />
                                    <telerik:AjaxUpdatedControl ControlID="lblError" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="cmdSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="versendungenGrid" />
                                    <telerik:AjaxUpdatedControl ControlID="lblError" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                                    <telerik:AjaxUpdatedControl ControlID="tab1" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearchUp" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="NewSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="tab1" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearchUp" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="NewSearchUp">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="tab1" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearchUp" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <div id="TableQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="false" />
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="NewSearch" runat="server" ToolTip="Abfrage öffnen" ImageUrl="../../../Images/queryArrow.gif"
                                                Visible="false" OnClick="NewSearch_Click" CausesValidation="false" />
                                            <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                                                Visible="false" OnClick="NewSearchUp_Click" CausesValidation="false" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr class="formquery">
                                <td class="firstLeft active" style="padding-right: 70px; white-space: nowrap">
                                    <asp:Label ID="lbl_DatumVon" runat="server">Datum von:</asp:Label>
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txt_DatumVon" runat="server" Width="100px" />
                                    <ajaxToolkit:CalendarExtender ID="ce_DatumVon" runat="server" Format="dd.MM.yyyy"
                                        PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txt_DatumVon" />
                                    <ajaxToolkit:MaskedEditExtender ID="mee_DatumVon" runat="server" TargetControlID="txt_DatumVon"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight" />
                                </td>
                                <td class="active" style="width: 100%">
                                    <asp:RequiredFieldValidator ControlToValidate="txt_DatumVon" ID="rfv_DatumVon" runat="server"
                                        ErrorMessage="Bitte wählen Sie ein Datum" />
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_DatumBis" runat="server">Datum bis:</asp:Label>
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txt_DatumBis" runat="server" Width="100px" />
                                    <ajaxToolkit:CalendarExtender ID="ce_DatumBis" runat="server" Format="dd.MM.yyyy"
                                        PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txt_DatumBis" />
                                    <ajaxToolkit:MaskedEditExtender ID="mee_DatumBis" runat="server" TargetControlID="txt_DatumBis"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight" />
                                </td>
                                <td class="active">
                                    <asp:RequiredFieldValidator ControlToValidate="txt_DatumBis" ID="rfv_DatumBis" runat="server"
                                        ErrorMessage="Bitte wählen Sie ein Datum" />
                                    <asp:CompareValidator ControlToValidate="txt_DatumBis" ControlToCompare="txt_DatumVon"
                                        Operator="GreaterThan" Type="Date" ID="cmp_Datum" runat="server" Text="'Datum bis' muss größer als 'Datum von' sein" />
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_Bezahlt" runat="server">Bezahlt:</asp:Label>
                                </td>
                                <td class="active" colspan="2">
                                    <asp:RadioButtonList ID="rbtnl_Bezahlt" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow">
                                        <asp:ListItem Text="Bezahlt" Value="X" />
                                        <asp:ListItem Text="Unbezahlt" Value="U" />
                                        <asp:ListItem Text="Alle" Selected="True" Value="" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_Abrufgrund" runat="server">Abrufgrund:</asp:Label>
                                </td>
                                <td class="active">
                                    <asp:DropDownList ID="ddl_Abrufgrund" runat="server" DataValueField="SapWert" DataTextField="WebBezeichnung"
                                        DataSourceID="augruSource" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Alle" Value="END" />
                                        <asp:ListItem Text="Nachträglich endgültig" Value="NAE" />
                                    </asp:DropDownList>
                                </td>
                                <td class="active">
                                    <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                        Height="16px" CausesValidation="True" Font-Underline="False" OnClick="SearchClick"
                                        Style="margin-left: 20px">» Weiter</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 2px;">
                        </div>
                    </div>
                    <div id="dataQueryFooter">
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false"
                            Visible="false" />
                        <asp:Label ID="lblNoData" runat="server" Font-Bold="true" EnableViewState="false"
                            Visible="false" Text="Keine Versendungen für die angegebenen Kriterien gefunden" />
                    </div>
                    <telerik:RadGrid ID="versendungenGrid" AllowSorting="true" AllowPaging="true" AllowAutomaticInserts="false"
                        AutoGenerateColumns="false" PageSize="20" runat="server" GridLines="None" Width="100%"
                        BorderWidth="0" Culture="de-DE" Visible="false" OnItemCommand="GridItemCommand"
                        OnNeedDataSource="GridNeedDataSource" OnExcelMLExportRowCreated="GridExportRowCreated"
                        OnExcelMLExportStylesCreated="GridExportStylesCreated">
                        <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" />
                        <ClientSettings>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" Summary="Versendungen je Abrufgrund">
                            <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                            <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="HAENDLER" SortExpression="HAENDLER" UniqueName="HAENDLER"
                                    DataFormatString="<nobr>{0}</nobr>" />
                                <telerik:GridBoundColumn DataField="LIZNR" SortExpression="LIZNR" UniqueName="LIZNR"
                                    DataFormatString="<nobr>{0}</nobr>" />
                                <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" UniqueName="CHASSIS_NUM"
                                    DataFormatString="<nobr>{0}</nobr>" />
                                <telerik:GridBoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" UniqueName="LICENSE_NUM"
                                    DataFormatString="<nobr>{0}</nobr>" />
                                <telerik:GridBoundColumn DataField="TIDNR" SortExpression="TIDNR" UniqueName="TIDNR"
                                    DataFormatString="<nobr>{0}</nobr>" />
                                <telerik:GridBoundColumn DataField="NAME1" SortExpression="NAME1" UniqueName="NAME1"
                                    DataFormatString="<nobr>{0}</nobr>" />
                                <telerik:GridBoundColumn DataField="CITY1" SortExpression="CITY1" UniqueName="CITY1"
                                    DataFormatString="<nobr>{0}</nobr>" />
                                <telerik:GridBoundColumn DataField="ZZTMPDT" SortExpression="ZZTMPDT" UniqueName="ZZTMPDT"
                                    DataFormatString="<nobr>{0:dd.MM.yyyy}</nobr>" />
                                <telerik:GridBoundColumn DataField="ZZVGRUND_TEXT" SortExpression="ZZVGRUND_TEXT"
                                    UniqueName="ZZVGRUND_TEXT" DataFormatString="<nobr>{0}</nobr>" />
                                <telerik:GridBoundColumn DataField="ZZANFDT" SortExpression="ZZANFDT" UniqueName="ZZANFDT"
                                    DataFormatString="<nobr>{0:dd.MM.yyyy}</nobr>" />
                                <telerik:GridBoundColumn DataField="TEXT200" SortExpression="TEXT200" UniqueName="TEXT200"
                                    DataFormatString="<nobr>{0}</nobr>" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <div id="dataFooter">
                        &nbsp;</div>
                    <asp:SqlDataSource ID="augruSource" runat="server" CancelSelectOnNullParameter="true"
                        DataSourceMode="DataSet" EnableCaching="true" SelectCommand="SELECT GrundID, WebBezeichnung, SapWert, MitZusatzText, Zusatzbemerkung, VersandadressArt, Eingeschraenkt FROM CustomerAbrufgruende WHERE CustomerID=@cID AND GroupID=@gID AND AbrufTyp='endg';">
                        <SelectParameters>
                            <asp:Parameter Name="cID" />
                            <asp:Parameter Name="gID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
