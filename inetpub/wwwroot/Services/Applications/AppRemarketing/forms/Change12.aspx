<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change12.aspx.cs" Inherits="AppRemarketing.forms.Change12" MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Titlebar
        {
            background-image: url(/Services/Images/overflow.png);
            line-height: 22px;
            color: #ffffff;
            font-weight: bold;
            float: left;
            height: 22px;
            width: 100%;
            background-color: #576b96;
            text-align: center;
            white-space: nowrap;
        }
    </style>
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
                                        &nbsp;</td>
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
                                        Vermieter:
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
                                <tr id="tr_Gutachter" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap" style="height: 22px" width="150px">
                                        Gutachter
                                    </td>
                                    <td class="firstLeft active">                                        
                                        <asp:DropDownList ID="ddlGutachter" runat="server" Width="200px">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem>DEKRA</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="tr_DatumVon" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Datum von:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtDatumVon" runat="server" 
                                            CssClass="TextBoxNormal"  Width="200px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtDatumVon"></cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr id="DatumBis" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Datum bis:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtDatumBis" runat="server" 
                                            CssClass="TextBoxNormal"  Width="200px"></asp:TextBox>     
                                        <cc1:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtDatumBis"></cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
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
                                            OnExcelMLExportStylesCreated="rgGrid1_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid1_ItemCommand" OnItemCreated="rgGrid1_ItemCreated" 
                                            OnNeedDataSource="rgGrid1_NeedDataSource" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="3" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="FIN" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn Groupable="false" >
                                                        <HeaderStyle Width="75px" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnGutschrift" runat="server" Height="16px" Width="70px" Style="background-image: url(/services/images/button.jpg);
                                                                text-align: center; vertical-align: middle; font-weight: bold; padding-top: 2px"
                                                                Text="Gutschrift" CommandName="Gutschrift" CommandArgument='<%# Eval("FIN") + "|" + Eval("RENNR") %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn Groupable="false" >
                                                        <HeaderStyle Width="95px" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnNachbelastung" runat="server" Height="16px" Width="90px"
                                                                Style="background-image: url(/services/images/buttonXLarge.jpg); text-align: center;
                                                                vertical-align: middle; font-weight: bold; padding-top: 2px" Text="Nachbelastung"
                                                                CommandName="Nachbelastung" 
                                                                CommandArgument='<%# Eval("FIN") + "|" + Eval("RENNR") %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="FIN" SortExpression="FIN" >
                                                        <HeaderStyle Width="140px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KENNZEICHEN" SortExpression="KENNZEICHEN" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="INVENTAR" SortExpression="INVENTAR" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="NAME1" SortExpression="NAME1" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="RENNR" SortExpression="RENNR" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="REDAT" SortExpression="REDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SUMME" SortExpression="SUMME" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="100px" />
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
                    <div id="dataFooter">
                    </div>
                </div>
                <div>
                    <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                    <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                    <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel"
                        X="450" Y="200">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="mb" runat="server" Width="520px" Height="300px"
                        BackColor="#E7E7E7" style="display:none">
                        <div class="Titlebar">
                            <asp:Label ID="lblAdressMessage" runat="server"
                                Font-Bold="True" ForeColor="white"></asp:Label>
                        </div>
                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; padding-bottom: 10px;">
                        
                            <table>
                                <tbody>
                                <tr>
                                    <td style="color:#4C4C4C;font-weight:bold;width:90px;height:25px">
                                        Rechnungsnummer:</td>
                                    <td>
                                        <asp:Label ID="lblRechnr" runat="server" Font-Bold="True" ForeColor="#4C4C4C"></asp:Label>
                                    </td>
                                </tr>
                                    <tr>
                                        <td style="color:#4C4C4C;font-weight:bold;width:90px;height:25px">
                                            Fahrgestellnummer:</td>
                                        <td>
                                            <asp:Label ID="lblFin" runat="server" Font-Bold="True" ForeColor="#4C4C4C"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color:#4C4C4C;font-weight:bold;width:90px">
                                            Betrag in &#8364;:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBetrag" runat="server" MaxLength="9" Width="100px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilterExtender" runat="server" 
                                                FilterType="Custom" TargetControlID="txtBetrag" ValidChars="0123456789,">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                <tr>
                                    <td style="color:#4C4C4C;font-weight:bold;width:90px;vertical-align: top">
                                        Bemerkung:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBemerkung" runat="server" MaxLength="200" Width="350px" 
                                            TextMode="MultiLine" Height="52px"></asp:TextBox>
                                    </td>
                                </tr>
                                    <tr id="trEmpfaenger" runat="server" visible="false">
                                        <td style="color:#4C4C4C;font-weight:bold;width:90px">
                                            Empfänger:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlEmpfaenger" runat="server" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color:#4C4C4C;font-weight:bold;width:90px">
                                            Merkantiler Minderwert:</td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="cbxMinderwert"/>
                                            </td>
                                    </tr>
                                </tbody>
                            </table>       
                        </div>
                        <div style="text-align: center;padding-bottom:10px">
                            <asp:Label ID="lblSaveInfo" runat="server" Visible="false" Style="margin-bottom: 15px" EnableViewState="false"></asp:Label>
                        </div>
                        <table width="100%" style="text-align: center">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnOK" runat="server" Text="Übernehmen" CssClass="TablebuttonLarge"
                                        Font-Bold="True" Width="90px" Height="25px" onclick="btnOK_Click" style="vertical-align:middle" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Abbrechen" CssClass="TablebuttonLarge"
                                        Font-Bold="true" Width="90px" Height="25px" style="vertical-align:middle" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
