<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report17.aspx.vb" Inherits="AppF2.Report17"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/Services/PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" CssClass="firstLeft active"
                    Text="Zurück" OnClick="lb_zurueck_Click"></asp:LinkButton>
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
                                    <td class="active">
                                        <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False" />
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="ibtNewSearch" runat="server" ImageUrl="../../../Images/queryArrowUp.gif"
                                                OnClick="ibtNewSearch_Click" Visible="true" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <asp:Panel ID="divSelection" runat="server" Visible="true">
                        <div id="TableQuery" style="margin-bottom: 10px">
                            <table id="Table1" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="3">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server" Font-Bold="True" Visible="true"
                                                EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">                                        
                                        <td width="100px" class="firstLeft active" style="height: 22px">
                                            <asp:Label ID="lbl_FIN" runat="server" Text="Paidnummer:" AssociatedControlID="txt_FIN" />
                                        </td>
                                        <td class="firstLeft active" width="140px" style="height: 22px">
                                            <asp:TextBox ID="txt_FIN" runat="server" Width="120px" AutoPostBack="True" />
                                        </td>
                                        <td style="padding-right: 10Px" width="100%" rowspan="4">
                                            <div class="new_layout">
                                                <div id="infopanel" class="infopanel">
                                                    <label>
                                                        Tipp!</label>
                                                    <div>
                                                        Zum Anzeigen der Liste bitte auf die Schaltfläche<br />
                                                        'Abrufen' klicken
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td width="100px" class="firstLeft active" style="height: 22px">
                                            <asp:Label ID="lbl_Kontonr" runat="server" Text="Kontonummer:" AssociatedControlID="txt_Kontonr" />
                                        </td>
                                        <td class="firstLeft active" width="140px" style="height: 22px">
                                            <asp:TextBox ID="txt_Kontonr" runat="server" Width="120px" AutoPostBack="True" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td width="100px" class="firstLeft active" style="height: 22px;">
                                            <asp:Label ID="lbl_MahnstopVon" runat="server" Text="Mahnstop von:" AssociatedControlID="txt_MahnstopVon" />
                                        </td>
                                        <td class="firstLeft active" width="140px" style="height: 22px">
                                            <asp:TextBox ID="txt_MahnstopVon" runat="server" Width="120px" AutoPostBack="True" />
                                            <ajaxToolkit:CalendarExtender ID="ce_MahnstopVon" runat="server" Enabled="True" TargetControlID="txt_MahnstopVon"
                                                Animated="False" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td width="100px" class="firstLeft active" style="height: 22px;">
                                            <asp:Label ID="lbl_MahnstopBis" runat="server" Text="Mahnstop bis:" AssociatedControlID="txt_MahnstopBis" />
                                        </td>
                                        <td class="firstLeft active" width="140px" style="height: 22px">
                                            <asp:TextBox ID="txt_MahnstopBis" runat="server" Width="120px" AutoPostBack="True" />
                                            <ajaxToolkit:CalendarExtender ID="ce_MahnstopBis" runat="server" Enabled="True" TargetControlID="txt_MahnstopBis"
                                                Animated="False" />
                                        </td>                                        
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="3">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="footer" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                    </asp:Panel>
                    <div>
                        &nbsp;</div>
                    <asp:Panel ID="PnResult" Visible="false" runat="server">
                        <script type="text/javascript">

                            function onRequestStart(sender, args) {
                                if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToPdfButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                                    args.set_enableAjax(false);
                                }
                            }

                            function CheckTextLength(Textbox) {
                               
                                var txtBox = $("#" + Textbox.id);
                                var Text = txtBox.val();
                                if (Text.length > 133) {
                                    txtBox.val(Text.substring(0, 132));
                                    ShowMessage("Der Text darf nicht länger als 132 Zeichen sein!", true);
                                }
                            }

                            function ShowMessage(Text, Error) {
                                $("#ctl00_ContentPlaceHolder1_ErrorGrid").text("");
                                $("#ctl00_ContentPlaceHolder1_Success").text("");

                                if (Error) {
                                    $("#ctl00_ContentPlaceHolder1_ErrorGrid").text(Text);
                                } else {
                                    $("#ctl00_ContentPlaceHolder1_Success").text(Text);
                                }
                            }

                            function SetChange(sender) { 
                               // übergeordnetes <TR> suchen 
                               var row = $(sender).parent().parent();
                               // Child Checkbox suchen und Checked setzen
                               row.find("input:checkbox").attr('checked', true);
                            }
                        </script>
                        <div style="margin: 10px 5px 10px 10px;">
                            <span id="ErrorGrid" runat="server" class="TextError" style="font-weight: bold;"></span>
                            <span id="Success" runat="server" style="font-weight: bold; color:Green;"></span>
                        </div>
                        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                            <ClientEvents OnRequestStart="onRequestStart" />
                            <AjaxSettings>
                                <telerik:AjaxSetting AjaxControlID="rgMahnstop">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="rgTelefon" />                                        
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                            </AjaxSettings>
                        </telerik:RadAjaxManager>
                        <telerik:RadGrid ID="rgMahnstop" runat="server" GridLines="None" Width="100%" BorderWidth="0px"
                            Culture="de-DE" Visible="true" CellSpacing="0" AllowSorting="True" ShowFooter="True" 
                            AutoGenerateColumns="False" OnNeedDataSource="rgMahnstopNeedDataSource" AllowPaging="True">
                            <ClientSettings AllowKeyboardNavigation="True">
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="0" />
                                <Resizing AllowColumnResize="true" AllowResizeToFit="True" ResizeGridOnColumnResize="False" />
                            </ClientSettings>
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True"
                                ExportOnlyData="True">
                                <Excel Format="ExcelML"></Excel>
                            </ExportSettings>
                            <FooterStyle ForeColor="DarkBlue" Font-Bold="true" />
                            <HeaderStyle CssClass="RadGridHeader" Wrap="true"/>
                            <MasterTableView DataKeyNames="" CommandItemDisplay="Top" Summary="Mahnungen" TableLayout="Auto"
                                Width="100%" AllowAutomaticUpdates="True">
                                <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge"
                                    AlwaysVisible="True" />
                                <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                    ShowExportToCsvButton="false" ShowExportToPdfButton="false" ExportToWordText="Export to Word"
                                    ExportToExcelText="Export to XLS" ExportToCsvText="Export to CSV" ExportToPdfText="Export to PDF"
                                    ShowAddNewRecordButton="false" />
                                <Columns>
                                    <%--Kundennummer--%>
                                    <telerik:GridBoundColumn DataField="KUNNR_AG" HeaderText="Kundennummer" ReadOnly="true"
                                        SortExpression="KUNNR_AG" UniqueName="KUNNR_AG" />
                                    <%--Paidnummer/Fahrgestellnummer/FIN--%>
                                    <telerik:GridBoundColumn DataField="CHASSIS_NUM" HeaderText="Paidnummer" ReadOnly="true"
                                        SortExpression="CHASSIS_NUM" UniqueName="CHASSIS_NUM" />
                                    <%--Kontonummer/ZDPM_RETAIL--%>
                                      <telerik:GridBoundColumn DataField="KONTONR" HeaderText="Kontonummer" ReadOnly="true"
                                        SortExpression="KONTONR" UniqueName="KONTONR" />
                                    <%--Material--%>
                                    <telerik:GridTemplateColumn ReadOnly="true" HeaderText="Material" SortExpression="MATNR" UniqueName="MATNR" DataField="MATNR">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("MAKTX")%>'></asp:Label>
                                            <asp:Label runat="server" Text='<%# Eval("MATNR")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                <%--    <telerik:GridBoundColumn DataField="MATNR" HeaderText="Materialnummer" ReadOnly="true"
                                        SortExpression="MATNR" UniqueName="MATNR" />--%>
                                    <%--Materialname--%>
                                   <%-- <telerik:GridBoundColumn DataField="MAKTX" HeaderText="Materialname" ReadOnly="true"
                                        SortExpression="MAKTX" UniqueName="MAKTX" />--%>
                                    <%--Bezugsdatum--%>
                                    <%-- TODO: im BAPI nachrüsten lassen bzw. passende Spalte aussuchen.. --%>
                                    <%--Mahndatum ab--%>
                                    <telerik:GridTemplateColumn DataField="MAHNDATUM_AB" HeaderText="Mahnstop ab" ReadOnly="true"
                                        SortExpression="MAHNDATUM_AB" UniqueName="MAHNDATUM_AB">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="MAHNDATUM_AB" Text='<%# Eval("MAHNDATUM_AB") %>'
                                                Width="80px" onchange="javascript:SetChange(this);"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="MAHNDATUM_AB_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="MAHNDATUM_AB">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="meetxtMAHNDATUM_AB" runat="server" TargetControlID="MAHNDATUM_AB"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <asp:Label ID="lblMAHNDATUM_ABInfo" runat="server" CssClass="TextError"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <%--Mahnstop ab--%>
                                    <telerik:GridTemplateColumn DataField="BEM" HeaderText="Bemerkungstext" ReadOnly="true"
                                        SortExpression="BEM" UniqueName="BEM">
                                        <HeaderStyle width="240px" />
                                        <ItemStyle width="240px"/>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="BEM" Text='<%# Eval("BEM") %>' Width="100%" MaxLength="132"
                                                TextMode="MultiLine" Rows="3" onkeyup="javascript:CheckTextLength(this);" onchange="javascript:SetChange(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="EQUNR" HeaderText="EQUNR" ReadOnly="true" SortExpression="EQUNR"
                                        UniqueName="EQUNR" Visible="False" />                                 
                                    <telerik:GridTemplateColumn DataField="CHANGED" ReadOnly="true"
                                        SortExpression="CHANGED" UniqueName="CHANGED">
                                        <HeaderStyle Width="0px"/>
                                        <ItemStyle Width="0px"/>                                      
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="CHANGED" Checked='<%# Eval("CHANGED") %>' Visible="true" Enabled="true"></asp:CheckBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </asp:Panel>
                    <div id="DataFooter">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <br />
                                    <asp:LinkButton ID="lb_Execute" runat="server" CssClass="Tablebutton" Width="78px"
                                        Height="16px" CausesValidation="False" Font-Underline="False" OnClick="ExecuteClick"
                                        Visible="True">» Abrufen</asp:LinkButton>
                                    <asp:LinkButton ID="btnSave" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                        CausesValidation="False" Font-Underline="False" OnClick="btnSave_Click" Visible="False">» Speichern</asp:LinkButton>
                                </td>
                            </tr>                           
                        </table>
                    </div>
                    <div id="Div3">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
