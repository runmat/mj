<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report12.aspx.cs" Inherits="AppRemarketing.forms.Report12"
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
                    function openinfo(url) {
                        fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=350");
                        fenster.focus();
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
                                <tr id="tr_SearchFin" runat="server" visible="true" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <asp:TextBox ID="txtFahrgestellnummer" CssClass="TextBoxNormal" runat="server" MaxLength="17"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtKennzeichen" CssClass="TextBoxNormal" runat="server" MaxLength="17"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_Inventarnr" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_Inventarnr" runat="server">lbl_Inventarnr</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtInventarnr" CssClass="TextBoxNormal" runat="server" MaxLength="17"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_Selection" runat="server" visible="false" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Selektionsauswahl:
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <span>
                                            <asp:RadioButton ID="rb_Einzelselektion" Text="Einzelselektion" GroupName="Auswahl"
                                                runat="server" Checked="True" AutoPostBack="True" OnCheckedChanged="rb_Einzelselektion_CheckedChanged" />
                                            &nbsp;
                                            <asp:RadioButton ID="rbFin" Text="Fahrgestellnummer Upload" GroupName="Auswahl" runat="server"
                                                Checked="False" AutoPostBack="True" OnCheckedChanged="rbFin_CheckedChanged" />
                                            &nbsp; </span>
                                    </td>
                                </tr>
                                <tr id="tr_SelVermieter" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_Vermieter" runat="server">lbl_Vermieter</asp:Label>
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <asp:DropDownList ID="ddlVermieter" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="tr_DatumVon" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_DatumVon" runat="server">lbl_DatumVon</asp:Label>
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <asp:TextBox ID="txtDatumVon" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_Datumvon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtDatumvon">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr id="tr_DatumBis" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_DatumBis" runat="server">lbl_DatumBis</asp:Label>
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <asp:TextBox ID="txtDatumBis" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Wählen Sie 'Datum bis' größer als 'Datum von'!"
                                            ControlToCompare="txtDatumvon" ControlToValidate="txtDatumBis" Operator="GreaterThanEqual"
                                            Type="Date"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr id="tr_Vertragsjahr" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Vertragsjahr" runat="server">lbl_Vertragsjahr</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtVertragsjahr" CssClass="TextBoxNormal" runat="server" MaxLength="4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_UploadFin" runat="server" visible="false" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_UploadFin" runat="server">lbl_UploadFin</asp:Label>
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <input id="upFileFin" type="file" size="49" name="upFileFin" runat="server" />&nbsp;
                                        <a href="javascript:openinfo('InfoFin.htm');">
                                            <img src="/Services/Images/info.gif" border="0" height="16px" width="16px" alt="Struktur Uploaddatei"
                                                title="Struktur Uploaddatei Fahrgestellnummern" /></a> &nbsp; * max. 900
                                        Datensätze
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
                                            OnNeedDataSource="rgGrid1_NeedDataSource" 
                                            OnItemDataBound="rgGrid_ItemDataBound" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top"  >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="FAHRGNR" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn DataField="FAHRGNR" SortExpression="FAHRGNR" GroupByExpression="FAHRGNR GROUP BY FAHRGNR" >
                                                        <HeaderStyle Width="140px" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkHistorie" Target="_blank" ToolTip="Zur Fahrzeughistorie" runat="server"
                                                                Text='<%# Eval("FAHRGNR") %>'>
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="KENNZ" SortExpression="KENNZ" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="INVENTAR" SortExpression="INVENTAR" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZULDAT" SortExpression="ZULDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SOLLZULDAT" SortExpression="SOLLZULDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="POS_TEXT" SortExpression="POS_TEXT" >
                                                        <HeaderStyle Width="75px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="HCNAME" SortExpression="HCNAME" >
                                                        <HeaderStyle Width="115px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="MODELLGRP" SortExpression="MODELLGRP" >
                                                        <HeaderStyle Width="125px" />
                                                        <ItemStyle Wrap="false" />                                                        
                                                    </telerik:GridBoundColumn>
                                                     <telerik:GridBoundColumn DataField="MODELL" SortExpression="MODELL" >
                                                        <HeaderStyle Width="75px" />
                                                        <ItemStyle Wrap="false" />
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
                                        <telerik:RadGrid ID="rgGrid2" Visible="false" runat="server" PageSize="15"  
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE"  
                                            OnNeedDataSource="rgGrid2_NeedDataSource" OnItemDataBound="rgGrid_ItemDataBound" >
                                            <ClientSettings AllowKeyboardNavigation="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" TableLayout="Auto" AllowPaging="true" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="CHASSIS_NUM" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" GroupByExpression="CHASSIS_NUM GROUP BY CHASSIS_NUM" >
                                                        <HeaderStyle Width="140px" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkHistorie" Target="_blank" ToolTip="Zur Fahrzeughistorie" runat="server"
                                                                Text='<%# Eval("CHASSIS_NUM") %>'>
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="BEM" SortExpression="BEM" >
                                                        <ItemStyle Wrap="false" />
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
            </div>
        </div>
    </div>
</asp:Content>

