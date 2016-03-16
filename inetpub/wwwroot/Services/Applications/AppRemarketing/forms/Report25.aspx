<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report25.aspx.cs" Inherits="AppRemarketing.forms.Report25"
    MasterPageFile="../Master/AppMaster.Master" %>

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
                                <tr class="formquery" id="tr_Fahrgestellnummer" runat="server" >
                                    <td class="firstLeft active">
                                        Fahrgestellnummer:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                            Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Kennzeichen" runat="server" >
                                    <td class="firstLeft active">
                                        Kennzeichen:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" MaxLength="10"
                                            Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr visible="false" class="formquery" id="tr_Vermieter" runat="server" >
                                    <td class="firstLeft active">
                                        Autovermieter:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:DropDownList ID="ddlVermieter" runat="server" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr visible="true" class="formquery" id="tr_HC" runat="server" >
                                    <td class="firstLeft active" nowrap="nowrap" style="height: 22px" width="150px">
                                        Hereinnahmecenter
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:DropDownList ID="ddlHC" runat="server" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Status" runat="server" >
                                    <td class="firstLeft active">
                                        Status:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_DatumVon" runat="server" >
                                    <td class="firstLeft active">
                                        Eingangsdatum von:
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
                                <tr class="formquery" id="tr_DatumBis" runat="server" >
                                    <td class="firstLeft active">
                                        Eingangsdatum bis:
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
                                <tr class="formquery" id="tr_Vertragsjahr" runat="server" >
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
                        <div class="ExcelDiv" id="divGutachtenHeader" runat="server" Visible="false">
                            <div align="right" class="rightPadding">
                                <span style="float: left; color: White; font-weight: bold; padding-left: 15px">
                                    <asp:Label ID="lblGutachten" runat="server">Gutachten</asp:Label>
                                </span>
                            </div>
                        </div>
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
                                                <Scrolling ScrollHeight="550px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="3" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="FAHRGNR" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn Groupable="false">
                                                        <HeaderStyle Width="27px" Font-Underline="false" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkAuswahl" Checked='<%# (Eval("Auswahl")).ToString()!= "0"%>'
                                                                runat="server" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn Groupable="false">
                                                        <HeaderStyle Width="27px" Font-Underline="false" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnRekla" runat="server" SortExpression="ibtnRekla" CommandName="Rekla"
                                                                ImageUrl="/services/images/del.png" ToolTip="Reklamieren" CommandArgument='<%# Eval("FAHRGNR") %>'
                                                                Visible='<%# Convert.ToInt32(Eval("STATU"))== 0 || Convert.ToInt32(Eval("STATU"))== 3 %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn Groupable="false">
                                                        <HeaderStyle Width="135px" Font-Underline="false" />
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgEmpty" runat="server" ImageUrl="/Services/images/blank.gif" Height="16"
                                                                Width="16" Visible='<%# (Eval("DDTEXT")).ToString()!="Widersprochen" && (Eval("DDTEXT")).ToString()!="Blockiert" %>' />
                                                            <asp:ImageButton ID="imgRekla" runat="server" ImageUrl="/services/images/comment.png"
                                                                CommandName="ShowReklamation" CommandArgument='<%# Eval("FAHRGNR") %>'
                                                                Visible='<%# (Eval("DDTEXT")).ToString()=="Widersprochen" %>'
                                                                ToolTip="Widerspruch" />
                                                            <asp:ImageButton ID="ibtnblocktext" runat="server" ImageUrl="/services/images/comment.png"
                                                                CommandName="BlockText" CommandArgument='<%# Eval("FAHRGNR") %>'
                                                                Visible='<%# (Eval("DDTEXT")).ToString()=="Blockiert" %>'
                                                                ToolTip="Blockadegrund" />
                                                            <asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="/services/images/info.gif"
                                                                CommandName="Show" CommandArgument='<%# Eval("FAHRGNR") %>'
                                                                ToolTip="Gutachten anzeigen." />
                                                            <asp:ImageButton ID="ibtnpdf" runat="server" ImageUrl="/services/images/pdf-logo.png"
                                                                Height="20" Width="20" CommandName="PDF" CommandArgument='<%# Eval("FAHRGNR") %>'
                                                                ToolTip="Belastungsanzeige als PDF" />
                                                            <asp:HyperLink ID="lnkTuev" runat="server" Target="_blank" ImageUrl="/services/images/TUEV.png"
                                                                Height="20px" Width="20px" ToolTip="Zum TÜV-Gutachten" 
                                                                Visible='<%# Eval("GUTA").ToString()=="TUEV" && Eval("FAHRGNR")!=null %>' 
                                                                NavigateUrl='<%# "http://audi-ruecknahme.autoplus-portal.de/getDADFile?fin=" + Eval("FAHRGNR").ToString() %>' />
                                                            <asp:ImageButton ID="ibtnRepKalk" runat="server" ImageUrl="/services/images/Tool.png"
                                                                Height="20" Width="20" CommandName="RepKalk" CommandArgument='<%# Eval("FAHRGNR").ToString() + "_" + Eval("REPKALK").ToString() %>'
                                                                ToolTip="Reparaturkostenkalkulation" Visible='<%# !String.IsNullOrEmpty(Eval("REPKALK").ToString()) %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
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
                                                        <HeaderStyle Width="110px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="AVNAME" SortExpression="AVNAME" >
                                                        <HeaderStyle Width="95px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="HCNAME" SortExpression="HCNAME" >
                                                        <HeaderStyle Width="125px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="HCEINGDAT" SortExpression="HCEINGDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="118px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="GUTAUFTRAGDAT" SortExpression="GUTAUFTRAGDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="132px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="GUTADAT" SortExpression="GUTADAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SOLLFREI" SortExpression="SOLLFREI" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="95px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="DDTEXT" SortExpression="DDTEXT" UniqueName="DDTEXT" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SUMME" SortExpression="SUMME" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="75px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="AZGUT" SortExpression="AZGUT" >
                                                        <HeaderStyle Width="112px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="RENNR" SortExpression="RENNR" >
                                                        <HeaderStyle Width="128px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="FLAG_MIETFZG" UniqueName="FLAG_MIETFZG" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="STATU" UniqueName="STATU" Visible="false">
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
                                        <telerik:RadGrid ID="rgGrid2" Visible="false" runat="server" PageSize="10" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" 
                                            OnNeedDataSource="rgGrid2_NeedDataSource" >
                                            <ClientSettings AllowColumnsReorder="false" AllowKeyboardNavigation="true">
                                                <Scrolling ScrollHeight="200px" AllowScroll="True" UseStaticHeaders="True" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
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
                                                    <telerik:GridBoundColumn DataField="GUTA" SortExpression="GUTA" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="GUTAID" SortExpression="GUTAID" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="GUTADAT" SortExpression="GUTADAT" DataFormatString="{0:d}">
                                                        <HeaderStyle Width="105px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="BESCHAED_AV" SortExpression="BESCHAED_AV" DataFormatString="{0:c}">
                                                        <HeaderStyle Width="125px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="FEHLTBETR_AV" SortExpression="FEHLTBETR_AV" DataFormatString="{0:c}">
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="VORSCHAED_REP_AV" SortExpression="VORSCHAED_REP_AV" DataFormatString="{0:c}">
                                                        <HeaderStyle Width="150px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SCHAED_UNREP_AV" SortExpression="SCHAED_UNREP_AV" DataFormatString="{0:c}">
                                                        <HeaderStyle Width="145px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="VORSCHAED_WERTMIND" SortExpression="VORSCHAED_WERTMIND" DataFormatString="{0:c}">
                                                        <HeaderStyle Width="170px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SCHAED_UNREP_WMIND" SortExpression="SCHAED_UNREP_WMIND" DataFormatString="{0:c}">
                                                        <HeaderStyle Width="220px" />
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
                    <div id="dataFooter" runat="server" style="margin-bottom: 10px;">
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        <asp:LinkButton ID="cmdBlock" runat="server" Visible="False" CssClass="TablebuttonMiddle"
                            Height="16px" Width="100px" OnClick="cmdBlock_Click">» Blockieren</asp:LinkButton>
                        <asp:LinkButton ID="cmdNoBlock" runat="server" CssClass="TablebuttonMiddle" Height="16px"
                            Width="100px" Visible="False" OnClick="cmdNoBlock_Click">» Aufheben</asp:LinkButton>
                        <asp:LinkButton ID="cmdEdit" Visible="False" runat="server" CssClass="TablebuttonMiddle"
                            Height="16px" Width="100px" OnClick="cmdEdit_Click">» In Bearbeitung</asp:LinkButton>
                        <asp:LinkButton ID="cmdSetOpen" runat="server" CssClass="TablebuttonLarge" Height="16px"
                            Width="128px" Visible="False" OnClick="cmdSetOpen_Click">» auf offen setzen</asp:LinkButton>
                    </div>
                </div>
                <div>
                    <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                    <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                    <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel"
                        X="450" Y="200">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="mb" runat="server" Width="500px" Height="290px" BackColor="White"
                        Style="display: none">
                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; text-align: center">
                            <asp:Label ID="lblAdressMessage" runat="server" Text="Bitte geben Sie hier eine Beschreibung ein:"
                                Font-Bold="True"></asp:Label>
                        </div>
                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; padding-bottom: 10px;
                            margin-left: 10px">
                            <asp:TextBox ID="txtBeschreibung" runat="server" Height="70px" TextMode="MultiLine"
                                Width="450px"></asp:TextBox>
                        </div>
                        <table cellpadding="0" cellspacing="0" style="margin-left: 10px;">
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Sachbearbeiter:
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtSachbearbeiter" runat="server" CssClass="TextBoxNormal" MaxLength="25"
                                        Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Telefon:
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtTelefon" runat="server" CssClass="TextBoxNormal" MaxLength="15"
                                        Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    E-Mail:
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtMail" runat="server" CssClass="TextBoxNormal" MaxLength="50"
                                        Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <div style="text-align: center; padding-bottom: 10px">
                            <asp:Label ID="lblSaveInfo" runat="server" Visible="false" Style="margin-bottom: 15px"></asp:Label>
                        </div>
                        <table width="100%" style="text-align: center">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnOK" runat="server" Text="Übernehmen" CssClass="TablebuttonLarge"
                                        Font-Bold="True" Width="90px" OnClick="btnOK_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Ablehnen" CssClass="TablebuttonLarge"
                                        Font-Bold="true" Width="90px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div>
                    <asp:Button ID="InfoPopupPosition" runat="server" Text="Fake" Style="display: none" />
                    <asp:Button ID="InfoPopupOpener" runat="server" Text="BUTTON" OnClick="InfoPopupOpener_Click"
                        Visible="False" />
                    <cc1:ModalPopupExtender ID="InfoPopup" runat="server" TargetControlID="InfoPopupPosition"
                        PopupControlID="InfoPopupContent" BackgroundCssClass="modalBackground" DropShadow="true"
                        CancelControlID="InfoPopupCancel" X="450" Y="200">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="InfoPopupContent" runat="server" Width="500px" Height="180px" BackColor="#F4F7FC"
                        Style="display: none">
                        <div class="Titlebar">
                            <asp:Literal ID="InfoPopupHeader" runat="server" Text="Reklamationstext" />
                        </div>
                        <div style="padding-left: 10px; margin-bottom: 10px; padding-bottom: 10px; height: 100px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="color: #4C4C4C; font-weight: bold; padding-left: 10px; padding-right: 10px;
                                            padding-top: 10px">
                                            <asp:Label ID="InfoPopupText" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <table width="100%" style="text-align: center">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="InfoPopupCancel" runat="server" Text="OK" CssClass="TablebuttonLarge"
                                        Font-Bold="true" Width="90px" Height="25px" Style="vertical-align: middle" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div>
                    <asp:Button ID="BlockPopupPosition" runat="server" Text="Fake" Style="display: none;" />
                    <asp:Button ID="BlockPopupOpener" runat="server" Text="BUTTON" OnClick="OpenBlockPopup_Click"
                        Visible="False" />
                    <cc1:ModalPopupExtender ID="BlockPopup" runat="server" TargetControlID="BlockPopupPosition"
                        PopupControlID="BlockPopupPanel" BackgroundCssClass="modalBackground" DropShadow="true"
                        CancelControlID="BlockPopupCancel" X="450" Y="200">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="BlockPopupPanel" runat="server" Width="500px" Height="180px" BackColor="#F4F7FC"
                        Style="display: none">
                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; text-align: center">
                            <asp:Label ID="BlockHeader" runat="server" Font-Bold="True"></asp:Label>
                        </div>
                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; padding-bottom: 10px;
                            margin-left: 10px">
                            <asp:TextBox ID="BlockText" runat="server" Height="70px" TextMode="MultiLine" Width="450px"></asp:TextBox>
                        </div>
                        <table width="100%" style="text-align: center">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="BlockAccept" runat="server" Text="Übernehmen" CssClass="TablebuttonLarge"
                                        Font-Bold="True" Width="90px" Height="25px" Style="vertical-align: middle" OnClick="BlockAcceptClick" />
                                    <asp:Button ID="BlockPopupCancel" runat="server" Text="Abbrechen" CssClass="TablebuttonLarge"
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
