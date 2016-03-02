<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report17.aspx.cs" Inherits="AppRemarketing.forms.Report17"  MasterPageFile="../Master/AppMaster.Master" %>

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
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="tr_Inventarnummer" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Inventarnummer:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtInventarnummer" runat="server" CssClass="TextBoxNormal" MaxLength="10"
                                            Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_SelVermieter" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap" style="height: 22px">
                                        <asp:Label ID="lbl_Vermieter" runat="server">lbl_Vermieter</asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 88%; height: 22px;">
                                        <asp:DropDownList ID="ddlVermieter" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="tr_Auswahl" runat="server" class="formquery" >
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Auswahl" runat="server">lbl_Auswahl</asp:Label></td>
                                    <td class="firstLeft active" style="width: 100%">
                                        <span>
                                        <asp:RadioButton ID="rbAlle" runat="server" 
                                          Checked="True" Text="Alle" GroupName="Leasing" Width="70px" />
                                        <asp:RadioButton ID="rbOhneLeasing" runat="server" 
                                            Text="Kauf" GroupName="Leasing" Width="110px" />
                                        <asp:RadioButton ID="rbLeasing" runat="server" Text="Leasing" 
                                            GroupName="Leasing" Width="100px" />
                                        </span>

                                    </td>
                                </tr>     
                                <tr id="tr_Status" runat="server" class="formquery" >
                                    <td class="firstLeft active" style="height: 19px;">
                                        <asp:Label ID="lbl_Status" runat="server">lbl_Status</asp:Label>  
                                    </td>
                                    <td class="firstLeft active" style="width: 100%; height: 19px;">
                                        <span>
                                        <asp:RadioButton ID="rbAlleStatus" runat="server" 
                                          Checked="True" Text="Alle" GroupName="grpStatus" Width="70px" />
                                        <asp:RadioButton ID="rbZugelassen" runat="server" Text="Zugelassen" 
                                            GroupName="grpStatus" Width="110px" />
                                        <asp:RadioButton ID="rbNichtZugelassen" runat="server" Text="Nicht zugelassen" 
                                            GroupName="grpStatus" />
                                        </span>

                                    </td>
                                </tr>
                                <tr id="tr_Ereignisart" runat="server" class="formquery" >
                                    <td class="firstLeft active" style="height: 19px;">
                                        <asp:Label ID="lbl_Ereignisart" runat="server">lbl_Ereignisart</asp:Label>  
                                    </td>
                                    <td class="firstLeft active" style="width: 100%; height: 19px;">
                                        <span>
                                        <asp:RadioButton ID="rbAlleArt" runat="server" 
                                          Checked="True" Text="Alle" GroupName="grpEreignis" Width="70px" />
                                        <asp:RadioButton ID="rbArt1" runat="server" Text="Verlust" 
                                            GroupName="grpEreignis" Width="110px" />
                                        <asp:RadioButton ID="rbArt2" runat="server" Text="Selbstvermarkter" 
                                            GroupName="grpEreignis" />
                                        </span>

                                    </td>
                                </tr>
                                <tr id="tr_DatumVon" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Auslieferungsdatum von:
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
                                        Auslieferungsdatum bis:
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
                                <tr id="tr_Vertragsjahr" runat="server" class="formquery" >
                                    <td class="firstLeft active">
                                         <asp:Label ID="lbl_Vertragsjahr" runat="server">lbl_Vertragsjahr</asp:Label> 
                                        </td>
                                    <td class="firstLeft active">
                                    <asp:TextBox ID="txtVertragsjahr" CssClass="TextBoxNormal" runat="server" 
                                        MaxLength="4"></asp:TextBox></td>
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
                                            OnItemDataBound="rgGrid1_ItemDataBound" ShowGroupPanel="True" >
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
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="POS_TEXT" SortExpression="POS_TEXT" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="AUSLDAT" SortExpression="AUSLDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZUL_FRIST" SortExpression="ZUL_FRIST" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZULDAT" SortExpression="ZULDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="HCNAME" SortExpression="HCNAME" >
                                                        <HeaderStyle Width="140px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="MODELLGRP" SortExpression="MODELLGRP" >
                                                        <HeaderStyle Width="140px" />
                                                        <ItemStyle Wrap="false" />                                                        
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="MODELL" SortExpression="MODELL" >
                                                        <HeaderStyle Width="50px" />
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

