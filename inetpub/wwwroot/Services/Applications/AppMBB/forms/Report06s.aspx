<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report06s.aspx.cs" Inherits="AppMBB.forms.Report06s"
  MasterPageFile="../Master/AppMaster.Master" %>
  
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        table.test > tbody > tr > td
        {
            padding: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
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
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lbl_Head" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="divAbfrage" runat="server">
                        <div id="paginationQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="active">
                                            Neue Abfrage starten
                                        </td>
                                        <td align="right">
                                            <div id="queryImage">
                                                <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                                    ToolTip="Abfrage öffnen" OnClick="OnNewSearch" Visible="false" />
                                                <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                                                    Visible="false" OnClick="OnNewSearchUp" />
                                            </div>
                                        </td>   
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div id="divError" runat="server" enableviewstate="false" style="padding: 0px 0px 10px 15px;">
                        <asp:Label ID="lbl_Error" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        <asp:Label ID="lbl_Info" runat="server" ForeColor="Blue" EnableViewState="False"></asp:Label>
                    </div>
                    <asp:Panel ID="QueryParameterContainer" runat="server" DefaultButton="lbSearch">
                        <div id="TableQuery">
                            <table id="tblEinzel" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Abmeldestatus" runat="server">lbl_Abmeldestatus</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left: 4px;" colspan="3">
                                            <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" TabIndex="1">
                                                <asp:ListItem Value="B" Text="to do Kunde" Selected="True" onclick="rbtnSelected();"/>
                                                <asp:ListItem Value="D" Text="to do DAD" onclick="rbtnSelected();"/>
                                                <asp:ListItem Value="V" Text="Versand" onclick="rbtnSelected();"/>
                                                <asp:ListItem Value="Z" Text="ZLS" onclick="rbtnSelected();"/>
                                                <asp:ListItem Value="S" Text="Standard" onclick="rbtnSelected();"/>
                                                <asp:ListItem Value="K" Text="Klärfälle" onclick="rbtnSelected();"/>
                                                <asp:ListItem Value="H" Text="Historie" onclick="rbtnSelected();"/>
                                                <asp:ListItem Value="W" Text="WDV DAD" onclick="rbtnSelected();"/>
                                                <asp:ListItem Value="C" Text="WDV Kunde" onclick="rbtnSelected();"/>
                                            </asp:RadioButtonList>
                                        </td>   
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Abteilung" runat="server">lbl_Abteilung</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left: 4px">
                                            <asp:DropDownList runat="server" ID="ddlAbteilung" Width="150px" TabIndex="7"/>
                                        </td>
                                        <td class="firstLeft active" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Rechnungsempfaenger" runat="server">lbl_Rechnungsempfaenger</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left: 4px">
                                            <asp:DropDownList runat="server" ID="ddlRechnungsempfaenger" Width="150px" TabIndex="7"/>
                                        </td>
                                        <td class="firstLeft active" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Vertragsnummer" runat="server">lbl_Vertragsnummer</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left: 4px">
                                            <asp:TextBox ID="txtVertragsnummer" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="17" Width="150px" TabIndex="2"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active" style="width:100%;" colspan="2">
                                            <asp:Label ID="lbl_Wiedervorlage" runat="server">Wiedervorlage</asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left: 4px">
                                            <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="17" Width="150px" TabIndex="3"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active"><asp:Label ID="lbl_WiedervorlageVon" runat="server">von</asp:Label></td>
                                        <td class="firstLeft active" style="padding-left: 4px">
                                            <asp:TextBox ID="txtWiedervorlageVon" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="10" Width="150px" TabIndex="5"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CE_WiedervorlageVon" runat="server" TargetControlID="txtWiedervorlageVon" Format="dd.MM.yyyy" PopupPosition="BottomLeft" Animated="false" Enabled="True" ></ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="MEE_WiedervorlageVon" runat="server" TargetControlID="txtWiedervorlageVon" Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight"></ajaxToolkit:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left: 4px;">
                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="17" Width="150px" TabIndex="4"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active"><asp:Label ID="lbl_WiedervorlageBis" runat="server">bis</asp:Label></td>
                                        <td class="firstLeft active" style="padding-left: 4px">
                                            <asp:TextBox ID="txtWiedervorlageBis" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="10" Width="150px" TabIndex="6"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CE_WiedervorlageBis" runat="server" TargetControlID="txtWiedervorlageBis" Format="dd.MM.yyyy" PopupPosition="BottomLeft" Animated="false" Enabled="True" ></ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="MEE_WiedervorlageBis" runat="server" TargetControlID="txtWiedervorlageBis" Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight"></ajaxToolkit:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td class="firstLeft active" style="padding-left: 4px" colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div id="Queryfooter" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbSearch" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="OnSearch" TabIndex="9">&#187; Suchen </asp:LinkButton>
                    </div>
                    <div id="Result" runat="server" visible="false">
                        <table cellspacing="0" cellpadding="0" width="100%" style="background-color: white;" border="0">
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
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true" ColumnsReorderMethod="Reorder">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top"  >
                                                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="True"></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="AUFTRAG_DAT" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn Groupable="false" UniqueName="ShowStatus" >
                                                        <HeaderStyle Width="25px" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtStatus" CommandArgument='<%# Eval("CHASSIS_NUM") +";"+ Eval("LICENSE_NUM") %>' CommandName="ShowStatus"
                                                                runat="server" ImageUrl="/services/images/EditTableHS.png" ToolTip="Status" Width="16" Height="16" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn Groupable="false" UniqueName="ShowHistory" >
                                                        <HeaderStyle Width="25px" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtHistory" CommandArgument='<%# Eval("CHASSIS_NUM") %>' CommandName="ShowHistory"
                                                                runat="server" ImageUrl="/services/images/History_Bullet.gif" ToolTip="Historie" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn Groupable="false" UniqueName="ShowRechercheprotokoll" >
                                                        <HeaderStyle Width="25px" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtRechercheProtokoll" CommandArgument='<%# Eval("VORGANGS_NR") %>' CommandName="ShowRechercheprotokoll"
                                                                runat="server" ImageUrl="/services/images/note.png" Visible='<%# Eval("FLAG_RECHERCHE").ToString() == "X" %>' ToolTip="Rechercheprotokoll" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn Groupable="false" UniqueName="ImageBemerkung" >
                                                        <HeaderStyle Width="30px" />
                                                        <ItemTemplate>
                                                            <asp:Image ID="Image1" runat="server" ImageUrl="/services/images/blank.gif" Visible='<%# Eval("TEXT_FLG").ToString() != "X" %>' />
                                                            <asp:Image ID="Image2" runat="server" ImageUrl="/services/images/comment.png" Visible='<%# Eval("TEXT_FLG").ToString() == "X" %>' ToolTip="Bemerkungstext vorhanden" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="AUFTRAG_DAT" SortExpression="AUFTRAG_DAT" DataFormatString="{0:dd.MM.yyyy}" >
                                                        <HeaderStyle Width="110px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="VERTRAGSNR" SortExpression="VERTRAGSNR" >
                                                        <HeaderStyle Width="110px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="WDVDAT" SortExpression="WDVDAT" DataFormatString="{0:dd.MM.yyyy}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="DAT_WV_KUNDE" SortExpression="DAT_WV_KUNDE" DataFormatString="{0:dd.MM.yyyy}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZB2_STATUS" SortExpression="ZB2_STATUS" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZB1_STATUS" SortExpression="ZB1_STATUS" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KV_STATUS" SortExpression="KV_STATUS" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KH_STATUS" SortExpression="KH_STATUS" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZLS_STATUS" SortExpression="ZLS_STATUS" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="FORM_STATUS" SortExpression="FORM_STATUS" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" >
                                                        <HeaderStyle Width="150px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZB2_NR" SortExpression="ZB2_NR" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="STORT_TXT" SortExpression="STORT_TXT" >
                                                        <HeaderStyle Width="140px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn Groupable="false" UniqueName="ShowDokument" >
                                                        <HeaderStyle Width="70px" />
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtSIP" CommandArgument='<%# Eval("CHASSIS_NUM") + ";" + Eval("PDF_SIP") %>' 
                                                                CommandName="ShowSIP" runat="server" Visible='<%# !String.IsNullOrEmpty(Eval("PDF_SIP").ToString()) %>' 
                                                                ImageUrl="/services/images/iconPDF.gif" ToolTip='<%# "Sicherstellungsprotokoll vom " + Eval("PDF_SIP") %>' />
                                                            <asp:ImageButton ID="ibtVER" CommandArgument='<%# Eval("CHASSIS_NUM") + ";" + Eval("PDF_VER") %>' 
                                                                CommandName="ShowVER" runat="server" Visible='<%# !String.IsNullOrEmpty(Eval("PDF_VER").ToString()) %>' 
                                                                ImageUrl="/services/images/iconPDF.gif" ToolTip='<%# "Vertragsdaten vom " + Eval("PDF_VER") %>' />
                                                            <asp:ImageButton ID="ibtDOK" CommandArgument='<%# Eval("CHASSIS_NUM") + ";" + Eval("PDF_DOK") %>' 
                                                                CommandName="ShowDOK" runat="server" Visible='<%# !String.IsNullOrEmpty(Eval("PDF_DOK").ToString()) %>' 
                                                                ImageUrl="/services/images/iconPDF.gif" ToolTip='<%# "allgemeines Dokument vom " + Eval("PDF_DOK") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                        <telerik:RadGrid ID="rgGrid2" Visible="false" runat="server" PageSize="10" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" EnableHeaderContextMenu="true" 
                                            OnExcelMLExportRowCreated="rgGrid2_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid2_ItemCommand" OnItemCreated="rgGrid_ItemCreated" 
                                            OnNeedDataSource="rgGrid2_NeedDataSource" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true" >
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="360px" AllowScroll="True" UseStaticHeaders="True" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top"  >
                                                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="True"></PagerStyle>
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
                                                    <telerik:GridBoundColumn DataField="KENNUNG" SortExpression="KENNUNG" >
                                                        <HeaderStyle Width="50px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="STATUS" SortExpression="STATUS" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ERDAT" SortExpression="ERDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ERNAM" SortExpression="ERNAM" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="TEXT" SortExpression="TEXT" >
                                                        <HeaderStyle Width="250px" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                        <telerik:RadGrid ID="rgGrid3" Visible="false" runat="server" PageSize="10" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" EnableHeaderContextMenu="true" 
                                            OnExcelMLExportRowCreated="rgGrid3_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid3_ItemCommand" OnItemCreated="rgGrid_ItemCreated"
                                            OnNeedDataSource="rgGrid3_NeedDataSource" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true" >
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="360px" AllowScroll="True" UseStaticHeaders="True" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top"  >
                                                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="True"></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="DATUM_1" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="KENNUNG_AP" SortExpression="KENNUNG_AP" >
                                                        <HeaderStyle Width="120px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="NAME" SortExpression="NAME" >
                                                        <HeaderStyle Width="120px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="DATUM_1" SortExpression="DATUM_1" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZB1_KNZ_1" SortExpression="ZB1_KNZ_1" >
                                                        <HeaderStyle Width="120px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="DATUM_2" SortExpression="DATUM_2" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZB1_KNZ_2" SortExpression="ZB1_KNZ_2" >
                                                        <HeaderStyle Width="120px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="VERMERK" SortExpression="VERMERK" >
                                                        <HeaderStyle Width="200px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                    <div runat="server" id="divDetails" style="width: 100%">
                                        <asp:FormView ID="fvDetails" runat="server" CssClass="test" Width="100%">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td style="background-color: #dfdfdf; height: 22px; vertical-align: middle" colspan="4">
                                                            <span style="padding-left: 15px; padding-top: 5px; font-weight: bold;">Statusanzeige</span>
                                                        </td>   
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td style="width: 30px">
                                                            &nbsp;
                                                        </td>
                                                        <td class="firstLeft active" style="width: 150px">
                                                            <asp:Label ID="lblFahrgestellummerStatus" runat="server">Fahrgestellnummer:</asp:Label>
                                                        </td>
                                                        <td colspan="2" style="padding-left: 4px;">
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("CHASSIS_NUM") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td>
                                                        </td>
                                                        <td class="firstLeft active">
                                                            <asp:Label ID="lblKennzeichenStatus" runat="server">Kennzeichen:</asp:Label>
                                                        </td>
                                                        <td colspan="2" style="padding-left: 4px;">
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("LICENSE_NUM") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td style="padding-left: 5px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:ImageButton ID="ibtNewZBI" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                                                ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnCommand="OnNewStatus"
                                                                CommandArgument="ZB1" CommandName="Status ZBI:" />
                                                        </td>
                                                        <td class="firstLeft active" style="vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="lblZBI" runat="server">Status ZBI:</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 4px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("ZB1_STATUS") %>'></asp:Label>
                                                        </td>
                                                        <td style="padding-left:4px;vertical-align:top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("TEXT_ZB1") %>' />&nbsp;
                                                        </td>   
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td style="padding-left: 5px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:ImageButton ID="ibtNewZBII" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                                                ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnCommand="OnNewStatus"
                                                                CommandArgument="ZB2" CommandName="Status ZBII:" />
                                                        </td>
                                                        <td class="firstLeft active" style="vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="lblZBII" runat="server">Status ZBII:</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 4px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("ZB2_STATUS") %>'></asp:Label>
                                                        </td>
                                                        <td style="padding-left:4px;vertical-align:top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("TEXT_ZB2") %>' />&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td style="padding-left: 5px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:ImageButton ID="ibtNewKNZV" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                                                ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnCommand="OnNewStatus"
                                                                CommandArgument="KV" CommandName="Status KNZV:" />
                                                        </td>
                                                        <td class="firstLeft active" style="vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="lblKNZV" runat="server">Status KNZV:</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 4px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("KV_STATUS") %>'></asp:Label>
                                                        </td>
                                                        <td style="padding-left:4px;vertical-align:top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label8" runat="server" Text='<%# Eval("TEXT_KV") %>' />&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td style="padding-left: 5px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:ImageButton ID="ibtNewKNZH" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                                                ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnCommand="OnNewStatus"
                                                                CommandArgument="KH" CommandName="Status KNZH:" />
                                                        </td>
                                                        <td class="firstLeft active" style="vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="lblKNZH" runat="server">Status KNZH:</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 4px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label9" runat="server" Text='<%# Eval("KH_STATUS") %>'></asp:Label>
                                                        </td>
                                                        <td style="padding-left:4px;vertical-align:top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label10" runat="server" Text='<%# Eval("TEXT_KH") %>' />&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td style="padding-left: 5px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:ImageButton ID="ibtNewForm" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                                                ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnCommand="OnNewStatus"
                                                                CommandArgument="F" CommandName="Status Form:" />
                                                        </td>
                                                        <td class="firstLeft active" style="vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="lblForm" runat="server">Status Form:</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 4px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label11" runat="server" Text='<%# Eval("FORM_STATUS") %>'></asp:Label>
                                                        </td>
                                                        <td style="padding-left:4px;vertical-align:top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label12" runat="server" Text='<%# Eval("TEXT_F") %>' />&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trZLS" runat="server" class="formquery" visible='<%# this.rblStatus.SelectedValue == "D" %>'>
                                                        <td style="padding-left: 5px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:ImageButton ID="ibtNewZlsStatus" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                                                ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnCommand="OnNewStatus"
                                                                CommandArgument="ZLS" CommandName="Zulassungsstatus:" />
                                                        </td>
                                                        <td class="firstLeft active" style="vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="lblZlsStatus" runat="server">Zulassungsstatus:</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 4px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label13" runat="server" Text='<%# Eval("ZLS_STATUS") %>'></asp:Label>
                                                        </td>
                                                        <td style="padding-left:4px;vertical-align:top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label14" runat="server" Text='<%# Eval("TEXT_ZLS") %>' />&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trWVD" runat="server" class="formquery" >
                                                        <td style="padding-left: 5px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:ImageButton ID="ibtNewWvd" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                                                ToolTip="Neues Wiedervorlagedatum setzen" Style="padding-right: 5px; padding-left: 5px" OnCommand="OnNewStatus"
                                                                CommandArgument="WVD" CommandName="Wiedervorlage:" />
                                                        </td>
                                                        <td class="firstLeft active" style="vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="lblWvd" runat="server">Wiedervorlage:</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 4px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label15" runat="server" Text='<%# Eval("WDVDAT", "{0:dd.MM.yyyy}") %>'></asp:Label><br />
                                                        </td>
                                                        <td style="padding-left:4px;vertical-align:top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label16" runat="server" Text='<%# Eval("TEXT_WVD") %>' />&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trWVK" runat="server" class="formquery" >
                                                        <td style="padding-left: 5px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:ImageButton ID="ibtNewWvk" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                                                ToolTip="Neues Wiedervorlagedatum Kunde setzen" Style="padding-right: 5px; padding-left: 5px" OnCommand="OnNewStatus"
                                                                CommandArgument="WVK" CommandName="Wiedervorlage Kunde:" />
                                                        </td>
                                                        <td class="firstLeft active" style="vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="lblWvk" runat="server">Wiedervorlage Kunde:</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 4px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label18" runat="server" Text='<%# Eval("DAT_WV_KUNDE", "{0:dd.MM.yyyy}") %>'></asp:Label><br />
                                                        </td>
                                                        <td style="padding-left:4px;vertical-align:top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="Label19" runat="server" Text='<%# Eval("TEXT_WVK") %>' />&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td style="background-color: #dfdfdf; height: 22px; vertical-align: middle" colspan="3">
                                                            <span style="padding-left: 15px; padding-top: 5px; font-weight: bold;">Statusanzeige</span>
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td style="width: 8%">
                                                            &nbsp;
                                                        </td>
                                                        <td class="firstLeft active" style="width: 150px">
                                                            <asp:Label ID="lblFahrgestellummerStatus" runat="server">Fahrgestellnummer:</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 4px">
                                                            <asp:Label ID="lblFinShow" runat="server" Text='<%# Eval("CHASSIS_NUM") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td>
                                                        </td>
                                                        <td class="firstLeft active">
                                                            <asp:Label ID="lblKennzeichenStatus" runat="server">Kennzeichen:</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 4px;">
                                                            <asp:Label ID="lblKennzeichenShow" runat="server" Text='<%# Eval("LICENSE_NUM") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td style="padding-left: 5px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            &nbsp;
                                                        </td>
                                                        <td class="firstLeft active" style="vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:Label ID="lblEditStatus" runat="server"></asp:Label>
                                                        </td>
                                                        <td style="padding-left: 4px; vertical-align: top; border-top: 1px solid #dfdfdf">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="200px" Style="vertical-align: top"></asp:DropDownList>
                                                            <asp:TextBox ID="txtStatus" runat="server" Width="200px" CssClass="TextBoxNormal" MaxLength="10" Style="vertical-align: top" Visible="false"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CE_Status" runat="server" TargetControlID="txtStatus" Format="dd.MM.yyyy" PopupPosition="BottomLeft" Animated="false" Enabled="True" ></ajaxToolkit:CalendarExtender>
                                                            <ajaxToolkit:MaskedEditExtender ID="MEE_Status" runat="server" TargetControlID="txtStatus" Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight"></ajaxToolkit:MaskedEditExtender>
                                                            &nbsp;<asp:TextBox ID="txtComment" runat="server" Height="60px" TextMode="MultiLine" Width="300px" ></asp:TextBox>
                                                            &nbsp;<asp:ImageButton ID="ibtSaveStatus" runat="server" ImageUrl="/portal/images/savehs.png" ToolTip="Speichern" Style="padding-right: 5px" OnCommand="OnSaveStatus" />
                                                            &nbsp;<asp:ImageButton ID="ibtCancelStatus" runat="server" ImageUrl="/services/images/del.png" ToolTip="Abbrechen" Style="padding-right: 5px" OnCommand="OnCancelStatus" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EditItemTemplate>
                                        </asp:FormView>
                                        <table width="100%" style="margin-top: 5px">
                                            <tr>
                                                <td style="background-color: #dfdfdf; height: 22px; vertical-align: middle" colspan="2">
                                                    <span style="padding-left: 15px; padding-top: 5px; font-weight: bold;">Dokumentenupload</span>
                                                </td>   
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" style="width: 180px; vertical-align:top">
                                                    Dokumentart:
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblDokumentart" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow">
                                                        <asp:ListItem Value="SIP" Text="Sicherstellungsprotokoll" Selected="True"/>
                                                        <asp:ListItem Value="VER" Text="Vertragsdaten"/>
                                                        <asp:ListItem Value="DOK" Text="allgemeines Dokument"/>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <telerik:RadAsyncUpload runat="server" ID="radUploadDokument" AllowedFileExtensions="pdf"
                                                        AllowedMimeTypes="pdf" MaxFileInputsCount="1" MultipleFileSelection="Disabled"
                                                        OnClientFileUploadFailed="onUploadFailed" OnClientValidationFailed="validationFailed"
                                                        DisablePlugins="true" Width="100%" InputSize="50" Localization-Cancel="Abbrechen"
                                                        Localization-Remove="Löschen" Localization-Select="Wählen" 
                                                        EnableInlineProgress="false" MaxFileSize="10485760">
                                                    </telerik:RadAsyncUpload>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" style="width: 180px; vertical-align:top">
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lbtUpload" runat="server" CssClass="Tablebutton" Width="78px" OnClick="OnUpload">&#187; Hochladen </asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #dfdfdf; height: 22px; vertical-align: middle" colspan="2">
                                                    <span style="padding-left: 15px; padding-top: 5px; font-weight: bold;">Vorgang stornieren</span>
                                                </td>   
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" style="width: 180px; vertical-align:top">
                                                    Vermerk:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStornoVermerk" runat="server" Height="60px" TextMode="MultiLine" Width="300px" ></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" style="width: 180px; vertical-align:top">
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lbtStorno" runat="server" CssClass="Tablebutton" Width="78px" OnClick="OnStorno">&#187; Storno </asp:LinkButton>
                                                </td>
                                            </tr>
                                         </table>
                                    </div>
                                    <asp:HiddenField ID="hField" runat="server" Value="0" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="BackContainer" runat="server" Visible="false">
                        <div style="height: 22px;">
                            &nbsp;
                        </div>
                        <div style="float: right; margin-top: 10px; margin-bottom: 31px;">
                            <asp:LinkButton ID="lbtBack" runat="server" CssClass="Tablebutton" Width="78px" OnClick="OnBack">&#187; Zurück </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        rbtnSelected = function () {
            var list = document.getElementById('<%=rblStatus.ClientID %>');
            var items = list.childNodes;
            var display = "none";
            for (var i = 0; i < items.length; i++) {
                var node = items[i];
                if (node.nodeName.toLowerCase() === 'input') {
                    if (node.checked) {
                        if ((node.value === 'W') || (node.value === 'C')) {
                            display = "block";
                        }
                        break;
                    }
                }
            }

            var wiedervorlage = [
            "<%=lbl_Wiedervorlage.ClientID %>",
            "<%=lbl_WiedervorlageVon.ClientID %>", "<%=lbl_WiedervorlageBis.ClientID %>",
            "<%=txtWiedervorlageVon.ClientID %>", "<%=txtWiedervorlageBis.ClientID %>"];
            for (var w = 0; w < wiedervorlage.length; w++) {
                document.getElementById(wiedervorlage[w]).style.display = display;
            }
        };

        function validationFailed(sender, eventArgs) {
            alert("Es werden nur *.PDF Dateien unterstützt.");
        }

        function onUploadFailed(sender, args) {
            alert(args.get_message());
        }

    </script>
</asp:Content>
