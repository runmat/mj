<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report13.aspx.cs" Inherits="AppRemarketing.forms.Report13"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu" style="margin-top: 10px; margin-bottom: 10px">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
                    <ClientEvents OnRequestStart="onRequestStart" />
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="innerContentRight">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="innerContentRight" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <script type="text/javascript">
                    function onRequestStart(sender, args) {
                        if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0) {
                            args.set_enableAjax(false);
                        }
                    }
                </script>
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading" style="float: none">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                    <div id="paginationQuery" style="float: none">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="active">
                                    Neue Abfrage
                                </td>
                                <td align="right">
                                    <div id="queryImage">
                                        <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                            ToolTip="Abfrage öffnen" Visible="false" OnClick="NewSearch_Click" />
                                        <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                                            Visible="false" OnClick="NewSearchUp_Click" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="lbCreate">
                        <div id="TableQuery" runat="server" style="margin-bottom: 10px">
                            <table cellpadding="0" cellspacing="0">
                                <tr id="tr_Fahrgestellnummer" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Fahrgestellnummer:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                            Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_Kennzeichen" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Kennzeichen:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" MaxLength="10"
                                            Width="200px"></asp:TextBox>
                                        </td>
                                </tr>
                                <tr id="tr_Inventarnummer" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Inventarnummer:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtInventarnummer" runat="server" CssClass="TextBoxNormal" MaxLength="10"
                                            Width="200px"></asp:TextBox>
                                        &nbsp;</td>
                                </tr>
                                <tr id="tr_Rechnungsnummer" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Rechnungsnummer:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtRechnungsnummer" runat="server" CssClass="TextBoxNormal" MaxLength="8"
                                            Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_Vermieter" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Autovermieter:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:DropDownList ID="ddlVermieter" runat="server" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="tr_Hereinnahmecenter" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap" style="height: 22px" width="150px">
                                        Hereinnahmecenter
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:DropDownList ID="ddlHC" runat="server" Width="200px">
                                        </asp:DropDownList>          
                                    </td>
                                </tr>
                                <tr id="tr_Status" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Status:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="200px">
                                            <asp:ListItem Value="A">- alle -</asp:ListItem>
                                            <asp:ListItem Value="R">Rechnungen</asp:ListItem>
                                            <asp:ListItem Value="G">Gutschrift</asp:ListItem>
                                            <asp:ListItem Value="N">Nachbelastung</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="tr_DatumVon" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Erstellungsdatum von:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtDatumVon" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr id="tr_DatumBis" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Erstellungsdatum bis:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtDatumBis" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr id="tr_Vertragsjahr" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Vertragsjahr:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtVertragsjahr" runat="server" CssClass="TextBoxNormal" MaxLength="4"
                                            Width="200px"></asp:TextBox>
                                        </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClientClick="Show_BusyBox1();" OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" EnableHeaderContextMenu="true" 
                                            OnExcelMLExportRowCreated="rgGrid1_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid1_ItemCommand" OnItemCreated="rgGrid_ItemCreated" 
                                            OnNeedDataSource="rgGrid1_NeedDataSource" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="550px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="2" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top"  >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="RENNR" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn Groupable="false" UniqueName="Bearbeiten" >
                                                        <HeaderStyle Width="15px" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnEdit" runat="server" Visible='<%# (Eval("STATUS")).ToString()== "Rechnung" %>'
                                                                CommandName="Info" ImageUrl="/services/images/info.gif"
                                                                ToolTip="Belastungsanzeigen anzeigen." CommandArgument='<%# Eval("RENNR") %>' />
                                                            <asp:ImageButton ID="ibtBemerkung" runat="server" Visible='<%# (Eval("STATUS")).ToString()!= "Rechnung" %>'
                                                                CommandName="Bemerkung" ImageUrl="/services/images/comment.png"
                                                                ToolTip="Bemerkungstext anzeigen." CommandArgument='<%# Eval("RENNR") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="NAME1" SortExpression="NAME1" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="STATUS" SortExpression="STATUS" >
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="RENNR" SortExpression="RENNR" Groupable="false">
                                                        <HeaderStyle Width="60px" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnpdf" runat="server" CommandArgument='<%# Eval("RENNR") + "|" + ((string)Eval("STATUS")).Substring(0, 1).ToUpperInvariant() %>'
                                                                CommandName="PDF" ImageUrl="/services/images/pdf-logo.png" Height="20" Width="20"
                                                                SortExpression="ibtnEdit" ToolTip="Rechnung als PDF" />&nbsp;
                                                            <asp:Label runat="server" ID="lblRechnungsnummer" Text='<%# Eval("RENNR") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="REDAT" SortExpression="REDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="NETWR" SortExpression="NETWR" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="REFNR" SortExpression="REFNR" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="REFIN" SortExpression="REFIN" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid2" Visible="false" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" EnableHeaderContextMenu="true" 
                                            OnExcelMLExportRowCreated="rgGrid2_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid2_ItemCommand" OnItemCreated="rgGrid_ItemCreated" 
                                            OnNeedDataSource="rgGrid2_NeedDataSource" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true" >
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="2" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top"  >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="FIN" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="REDAT" SortExpression="REDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="FIN" SortExpression="FIN" >
                                                        <HeaderStyle Width="140px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KENNZEICHEN" SortExpression="KENNZEICHEN" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KMSTAND" SortExpression="KMSTAND" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KMWRT" SortExpression="KMWRT" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="OPWRT" SortExpression="OPWRT" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="FBOPT" SortExpression="FBOPT" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="FLWRT" SortExpression="FLWRT" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="RPWRT" SortExpression="RPWRT" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="UWWRT" SortExpression="UWWRT" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="MAEWERT_AV" SortExpression="MAEWERT_AV" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="MINWERT_AV" SortExpression="MINWERT_AV" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SUMME" SortExpression="SUMME" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: right; width: 100%; text-align: right; padding-top: 15px">
                        <asp:LinkButton ID="lbtnBack" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="lbtnBack_Click" Visible="false">» Zurück </asp:LinkButton>
                    </div>
                    <div id="dataFooter">
                        <asp:Literal id="Literal1" runat="server"></asp:Literal>
                    </div>
                </div>
                <div>
                    <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                    <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                    <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel"
                        X="450" Y="200">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="mb" runat="server" Width="500px" Height="180px" BackColor="#F4F7FC" style="display:none">
                        <div style="padding-left: 110px; padding-bottom: 5px; padding-top: 7px; background-color: #64759E;
                            height: 17px;">
                            <asp:Label ID="lblAdressMessage" runat="server" Text="Bemerkungstext "
                                Font-Bold="True" ForeColor="white"></asp:Label>
                        </div>
                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; padding-bottom: 10px;height:70px">
                            <table>
                                <tbody>
                                    
                                    <tr>
                                        <td style="color: #4C4C4C; font-weight: bold; width: 90px">
                                            Bemerkung:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBemerkung" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <table width="100%" style="text-align: center">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnOK" runat="server" Text="Übernehmen" CssClass="TablebuttonLarge"
                                        Font-Bold="True" Width="90px" Height="25px" OnClick="btnOK_Click" Style="vertical-align: middle" visible="false" />
                                    <asp:Button ID="btnCancel" runat="server" Text="OK" CssClass="TablebuttonLarge"
                                        Font-Bold="true" Width="90px" Height="25px" Style="vertical-align: middle" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

