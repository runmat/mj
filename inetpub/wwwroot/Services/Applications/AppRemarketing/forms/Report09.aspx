<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report09.aspx.cs" Inherits="AppRemarketing.forms.Report09"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="EditVorschaden" Src="../PageElements/EditVorschaden.ascx" %>
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
                                    <td class="firstLeft active" width="150px">
                                        Fahrgestellnummer
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtFin" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_Kennzeichen" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Kennzeichen
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_Inventarnummer" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Inventarnummer
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtInventarnummer" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_Autovermieter" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap" style="height: 22px" width="150px">
                                        Autovermieter
                                    </td>
                                    <td class="active">
                                        <span>
                                            <asp:DropDownList ID="ddlVermieter" runat="server">
                                            </asp:DropDownList>
                                        </span>
                                    </td>
                                </tr>
                                <tr id="tr_DatumVon" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Schadensdatum von
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtDatumVon" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_Datumvon" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_DatumVon" runat="server" TargetControlID="txtDatumVon"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                        <asp:Label ID="lblDatVonError" runat="server" EnableViewState="False" Font-Bold="True"
                                            Font-Names="Verdana" Font-Size="10px" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="tr_DatumBis" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Schadensdatum bis
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtDatumBis" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_Datumbis" runat="server" Animated="false" Enabled="True"
                                            Format="dd.MM.yyyy" PopupPosition="BottomLeft" TargetControlID="txtDatumBis">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" InputDirection="LeftToRight"
                                            Mask="99/99/9999" MaskType="Date" TargetControlID="txtDatumBis">
                                        </cc1:MaskedEditExtender>
                                        <asp:Label ID="lblDatBisError" runat="server" EnableViewState="False" Font-Bold="True"
                                            Font-Names="Verdana" Font-Size="10px" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="tr_Vertragsjahr" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Vertragsjahr" runat="server">lbl_Vertragsjahr</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtVertragsjahr" runat="server" MaxLength="4" CssClass="TextBoxNormal"></asp:TextBox>
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
                                        <telerik:RadGrid ID="rgGrid1" runat="server" Width="100%" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" EnableHeaderContextMenu="true" 
                                            OnExcelMLExportRowCreated="rgGrid1_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid1_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid1_ItemCommand" OnItemCreated="rgGrid1_ItemCreated" 
                                            OnNeedDataSource="rgGrid1_NeedDataSource" 
                                            OnItemDataBound="rgGrid1_ItemDataBound" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true" >
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="550px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top"  >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="LFDNR" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="LFDNR" SortExpression="LFDNR" >
                                                        <HeaderStyle Width="50px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="FAHRGNR" SortExpression="FAHRGNR" GroupByExpression="FAHRGNR GROUP BY FAHRGNR" >
                                                        <HeaderStyle Width="140px" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkHistorie" Target="_blank" ToolTip="Zur Fahrzeughistorie" runat="server"
                                                                Text='<%# Eval("FAHRGNR") %>'>
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="KENNZ" SortExpression="KENNZ" >
                                                        <HeaderStyle Width="75px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="INVENTAR" SortExpression="INVENTAR" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="AVNR" SortExpression="AVNR" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="PREIS" SortExpression="PREIS" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SCHAD_DAT" SortExpression="SCHAD_DAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="BESCHREIBUNG" SortExpression="BESCHREIBUNG" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="HCEINGDAT" SortExpression="HCEINGDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>                                                                                                                                                            
                                                    <telerik:GridBoundColumn DataField="HCNAME" SortExpression="HCNAME" >
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>                                                    
                                                    <telerik:GridBoundColumn DataField="MODELL" SortExpression="MODELL" >
                                                        <HeaderStyle Width="75px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="MODELLGRP" SortExpression="MODELLGRP" >
                                                        <HeaderStyle Width="140px" />
                                                        <ItemStyle Wrap="false" />                                                        
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn Groupable="false" UniqueName="Bearbeiten" >
                                                        <HeaderStyle Width="40px" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnEdit" runat="server" SortExpression="ibtnEdit" CommandName="EditData"
                                                                ImageUrl="/services/images/EditTableHS.png" ToolTip="Meldung bearbeiten." />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divEditVorschaden" runat="server" visible="false">
                        <uc2:EditVorschaden ID="EditVorschaden1" runat="server" />
                    </div>
                    <div id="dataFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
