<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateRadGrid.aspx.cs" Inherits="Vermieter.forms.TemplateRadGrid" MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" onclick="lbBack_Click"></asp:LinkButton>
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
                                        <asp:Label ID="lblSelection" runat="server" Text="Selektion ausblenden..."></asp:Label>
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="NewSearch" runat="server" 
                                                ImageUrl="/services/Images/queryArrow.gif" onclick="NewSearch_Click" />
                                            
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    
                    <div id="TableQuery">
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false" Visible="false" />
                    <asp:Label ID="lblNoData" runat="server" Font-Bold="true" EnableViewState="false" Visible="false" />
                   
                    <asp:Panel ID="pnlSelection" runat="server">
                    <table width="100%" id="tabOuter" runat="server" cellpadding="0" cellspacing="0" >
                            <tbody>
                                   
                                <tr class="formquery" >
                                    <td class="firstLeft active">
     
                            <table id="tabSelektion" runat="server" cellpadding="0" cellspacing="0">
                                <tbody>
                                        <tr class="formquery">
                                        <td class="firstLeft active" style="width:150px">
                                            <asp:Label ID="lbl_1" runat="server">Ein Label</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:DropDownList ID="dd_1" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="dd_1_SelectedIndexChanged" Width="350px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_2" runat="server">Ein Label</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txt_1" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="17" Width="200px" ontextchanged="txt_1_TextChanged"></asp:TextBox>
                                            <asp:ImageButton ID="ibtn_1" runat="server" Height="16px" 
                                                ImageUrl="/Services/Images/haken_gruen.gif" onclick="ibtn_1_Click" 
                                                ToolTip="Ein Image button" Width="16px" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Datum1" runat="server">Datum1</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txt_Datum1" runat="server"  AutoPostBack="true"
                                        Width="200px" ontextchanged="txt_Datum1_TextChanged"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="extDatum1" runat="server" TargetControlID="txt_Datum1" WatermarkText="Auswahl nur über Kalender." WatermarkCssClass="Watermarked"></cc1:TextBoxWatermarkExtender>
                                         <cc1:CalendarExtender ID="CE_Datum1" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                            Animated="false" Enabled="True" TargetControlID="txt_Datum1" PopupButtonID="btnCal1"></cc1:CalendarExtender>
                                            <asp:ImageButton ID="btnCal1" runat="server" 
                                                ImageUrl="/Services/Images/calendar.jpg" onclick="btnCal1_Click" />
                                            <asp:ImageButton ID="ibtnDelEingVon" runat="server" Height="16px" 
                                                ImageUrl="/Services/Images/del.png" ToolTip="Datum entfernen!" 
                                                Width="16px" style="padding-left:5px" onclick="ibtnDelEingVon_Click" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="height: 22px">
                                            <asp:Label ID="lbl_Datum2" runat="server">Datum2</asp:Label>
                                        </td>
                                     <td class="firstLeft active" style="height: 22px">
                                        <asp:TextBox ID="txt_Datum2" runat="server"  AutoPostBack="true"
                                      Width="200px" ontextchanged="txt_Datum2_TextChanged"></asp:TextBox>
                                      <cc1:CalendarExtender ID="txtDatum2_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txt_Datum2" Animated="False" >
                                                </cc1:CalendarExtender>
                                    </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_3" runat="server">Radiobutton</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <span>
                                            <asp:RadioButtonList ID="rbl_1" runat="server" 
                                                RepeatDirection="Horizontal" style="width:250px;" AutoPostBack="True" 
                                                onselectedindexchanged="rbl_1_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="0" style="white-space:nowrap;margin:0 0 0 0;">Item0</asp:ListItem>
                                                <asp:ListItem Value="1" style="white-space:nowrap;margin:0 0 0 0;">Item1</asp:ListItem>
                                                <asp:ListItem Value="2" style="white-space:nowrap;margin:0 0 0 0;">Item2</asp:ListItem>
                                            </asp:RadioButtonList>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl4" runat="server">Checkbox</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                           <span>
                                            <asp:CheckBox ID="chk_1" runat="server" AutoPostBack="True" oncheckedchanged="chk_1_CheckedChanged" 
                                                 />
                                            </span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>

                                                    </td>
                        <td align="right" style="width:100%;vertical-align:top;padding-right:5px;" >
                            &nbsp</td> 

                                    <td style="width:100%;vertical-align:top;padding-right:5px;">
                                        <asp:Panel ID="Panel1" runat="server" style="text-align:left" Visible="true" width="350px">
                                            <div class="new_layout">
                                                <div id="infopanel" class="infopanel">
                                                    <label style="margin-left:0; margin-right:0; padding-right:0; width:275px">
                                                    Tipp!</label>
                                                    <div>
                                                        Um in der Tabelle zusätzliche Spalten einzublenden<br/> 
                                                        klicken Siemit der rechten Maustaste auf die<br/>
                                                        Spaltenüberschriften (« Spalten auswählen »).
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </td>

                        </tr>
                            <tr class="formquery" >
                            <td>&nbsp</td>  
                            <td>&nbsp;</td> 
                            <td>&nbsp;</td> 
                         </tr>
                        </tbody> 
                        </table>


                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>      

                    </asp:Panel>
                    </div>
                               
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
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                    <script type="text/javascript">
                    <!--

                        function ShowColumnHeaderMenu(ev, columnName) {
                            alert("");
                            var grid = $find("<%=fzgGrid.ClientID %>");
                            var columns = grid.get_masterTableView().get_columns();
                            for (var i = 0; i < columns.length; i++) {
                                if (columns[i].get_uniqueName() == columnName) {
                                    columns[i].showHeaderMenu(ev, 75, 20);
                                }
                            }
                        }
                        -->
                    </script>
                </telerik:RadCodeBlock>
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                    <ClientEvents OnRequestStart="onRequestStart" />
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="fzgGrid">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="fzgGrid" LoadingPanelID="RadAjaxLoadingPanel1" />
                                <telerik:AjaxUpdatedControl ControlID="lblError" />
                                <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                <telerik:radgrid ID="fzgGrid" AllowSorting="True" AllowPaging="True" 
                     AutoGenerateColumns="False" runat="server" GridLines="None" Width="100%" BorderWidth="0px" 
                    Culture="de-DE" Visible="False"
                    EnableHeaderContextMenu="True" 

                    OnExcelMLExportRowCreated="fzgGrid_ExcelMLExportRowCreated"
                    OnExcelMLExportStylesCreated="fzgGrid_ExcelMLExportStylesCreated"
                    OnItemCommand="FzgGridItemCommand" OnNeedDataSource="FzgGridNeedDataSource"
                    OnPageIndexChanged="FzgGridPageChanged" OnPageSizeChanged="FzgGridPageSizeChanged"
                    OnSortCommand="FzgGridSortCommand" CellSpacing="0" 
                    ClientSettings-AllowColumnsReorder="True" VirtualItemCount="2">

                    <ExportSettings HideStructureColumns="true" IgnorePaging="True" 
                        OpenInNewWindow="True" >
<Pdf AllowCopy="True"></Pdf>

                    <Excel Format="ExcelML"></Excel>
                    </ExportSettings>
                    <ExportSettings HideStructureColumns="True">
                    <Excel Format="ExcelML"></Excel>
                    <Pdf AllowCopy="True" />
                    <Excel Format="ExcelML" />
                    </ExportSettings>
                    <ClientSettings allowkeyboardnavigation="True">
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="2"></Scrolling>
                        <Resizing AllowColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" Summary="KFZ Steuer Avisierung" TableLayout="Auto" Width="100%" >
                        <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                        </EditFormSettings>

                        <PagerStyle Mode="NextPrevAndNumeric" 
                            PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" 
                            AlwaysVisible="True" />
                        <CommandItemSettings 
                            ShowExportToWordButton="false" 
                            ShowExportToExcelButton="true"
                            ShowExportToCsvButton="false" 
                            ShowExportToPdfButton="false"
                            ExportToWordText="Export to Word"
                            ExportToExcelText="Export to XLS"
                            ExportToCsvText="Export to CSV"
                            ExportToPdfText="Export to PDF"
                            ShowAddNewRecordButton="false" 
                            />

                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                         <Columns>

                            <%--Fahrzeugart--%>
                            <telerik:GridBoundColumn DataField="FZG_ART"
                                FilterControlAltText="Filter FZG_ART column" SortExpression="FZG_ART" 
                                UniqueName="FZG_ART" Groupable="False" HeaderText="FZG_ART">
                            </telerik:GridBoundColumn>
                            <%--Fahrgestellnummer--%>
                            <telerik:GridBoundColumn DataField="CHASSIS_NUM" 
                                FilterControlAltText="Filter CHASSIS_NUM column" SortExpression="CHASSIS_NUM" 
                                UniqueName="CHASSIS_NUM" Groupable="False" HeaderText="CHASSIS_NUM">
                            </telerik:GridBoundColumn>
                            <%--Kfz-Kennzeichen--%>
                            <telerik:GridBoundColumn DataField="LICENSE_NUM" 
                                FilterControlAltText="Filter LICENSE_NUM column" SortExpression="LICENSE_NUM" 
                                UniqueName="LICENSE_NUM" Groupable="False" HeaderText="LICENSE_NUM">
                                
                            </telerik:GridBoundColumn>
                            <%--Zulassungsdatum --%>
                            <telerik:GridBoundColumn DataField="ZZZLDAT" DataFormatString="{0:dd.MM.yyyy}" 
                                DataType="System.DateTime" FilterControlAltText="Filter ZZZLDAT column" 
                                SortExpression="ZZZLDAT" UniqueName="ZZZLDAT" Groupable="False" HeaderText="ZZZLDAT">
                                
                            </telerik:GridBoundColumn>
                            <%--AbmeldeDatum --%>
                            <telerik:GridBoundColumn DataField="EXPIRY_DATE" 
                                DataFormatString="{0:dd.MM.yyyy}" DataType="System.DateTime" 
                                FilterControlAltText="Filter EXPIRY_DATE column" SortExpression="EXPIRY_DATE" 
                                UniqueName="EXPIRY_DATE" Groupable="False" HeaderText="EXPIRY_DATE">
                                
                            </telerik:GridBoundColumn>
                            <%--KFZ Steuer Gesamt --%>
                            <telerik:GridBoundColumn DataField="KFZ_STEU_GES" 
                                FilterControlAltText="Filter KFZ_STEU_GES column" SortExpression="KFZ_STEU_GES" 
                                UniqueName="KFZ_STEU_GES" Groupable="False" HeaderText="KFZ_STEU_GES">
                                
                            </telerik:GridBoundColumn>

                            <%--Equipmentnummer --%>
                            <telerik:GridBoundColumn DataField="EQUNR" SortExpression="EQUNR" 
                                FilterControlAltText="Filter EQUNR column" 
                                UniqueName="EQUNR" Display="False" Groupable="False" HeaderText="EQUNR" > 
                                
                            </telerik:GridBoundColumn>
                            <%--DAD Reverenz 1 --%>
                            <telerik:GridBoundColumn DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1" 
                                FilterControlAltText="Filter ZZREFERENZ1 column" 
                                UniqueName="ZZREFERENZ1" Display="False" Groupable="False" HeaderText="ZZREFERENZ1" > 
                                
                            </telerik:GridBoundColumn> 
				            <%--DAD Reverenz 2 --%>
				            <telerik:GridBoundColumn DataField="ZZREFERENZ2" SortExpression="ZZREFERENZ2" 
                                FilterControlAltText="Filter ZZREFERENZ2 column" 
                                UniqueName="ZZREFERENZ2" Display="False" Groupable="False" HeaderText="ZZREFERENZ2" > 
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="REPLA_DATE" SortExpression="REPLA_DATE" 
                                DataType="System.DateTime" 
                                FilterControlAltText="Filter REPLA_DATE column" UniqueName="REPLA_DATE" 
                                Display="False" Groupable="False" HeaderText="REPLA_DATE" >
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="EXPIRY_DATE_BER" SortExpression="EXPIRY_DATE_BER" 
                                DataType="System.DateTime" 
                                FilterControlAltText="Filter EXPIRY_DATE_BER column" 
                                UniqueName="EXPIRY_DATE_BER" Display="False" Groupable="False" HeaderText="EXPIRY_DATE_BER" >
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ZZCODE_KRAFTSTOF" 
                                FilterControlAltText="Filter ZZCODE_KRAFTSTOF column" 
                                SortExpression="ZZCODE_KRAFTSTOF" UniqueName="ZZCODE_KRAFTSTOF" 
                                Display="False" Groupable="False" HeaderText="ZZCODE_KRAFTSTOF">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ZZKRAFTSTOFF_TXT" 
                                FilterControlAltText="Filter ZZKRAFTSTOFF_TXT column" 
                                SortExpression="ZZKRAFTSTOFF_TXT" UniqueName="ZZKRAFTSTOFF_TXT" 
                                Display="False" Groupable="False" HeaderText="ZZKRAFTSTOFF_TXT">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ZZHUBRAUM" 
                                FilterControlAltText="Filter ZZHUBRAUM column" SortExpression="ZZHUBRAUM" 
                                UniqueName="ZZHUBRAUM" Display="False" Groupable="False" HeaderText="ZZHUBRAUM">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ZZCO2KOMBI" 
                                FilterControlAltText="Filter ZZCO2KOMBI column" SortExpression="ZZCO2KOMBI" 
                                UniqueName="ZZCO2KOMBI" Display="False" Groupable="False" HeaderText="ZZCO2KOMBI">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ZZSLD" 
                                FilterControlAltText="Filter ZZSLD column" SortExpression="ZZSLD" 
                                UniqueName="ZZSLD" Display="False" Groupable="False" HeaderText="ZZSLD">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ZZNATIONALE_EMIK" 
                                FilterControlAltText="Filter ZZNATIONALE_EMIK column" 
                                SortExpression="ZZNATIONALE_EMIK" UniqueName="ZZNATIONALE_EMIK" 
                                Display="False" Groupable="False" HeaderText="ZZNATIONALE_EMIK">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SCHADSTOFF_KLA" 
                                FilterControlAltText="Filter SCHADSTOFF_KLA column" 
                                SortExpression="SCHADSTOFF_KLA" UniqueName="SCHADSTOFF_KLA" 
                                Display="False" Groupable="False" HeaderText="SCHADSTOFF_KLA">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SCHADSTOFF_KLA_TXT" 
                                FilterControlAltText="Filter SCHADSTOFF_KLA_TXT column" 
                                SortExpression="SCHADSTOFF_KLA_TXT" UniqueName="SCHADSTOFF_KLA_TXT" 
                                Display="False" Groupable="False" HeaderText="FZG_ART">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ZZZULGESGEW" 
                                FilterControlAltText="Filter ZZZULGESGEW column" SortExpression="ZZZULGESGEW" 
                                UniqueName="ZZZULGESGEW" Display="False" Groupable="False" HeaderText="ZZZULGESGEW">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ANZ_TAGE" DataType="System.Int32" 
                                FilterControlAltText="Filter ANZ_TAGE column" SortExpression="ANZ_TAGE" 
                                UniqueName="ANZ_TAGE" Display="False" Groupable="False" HeaderText="ANZ_TAGE">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ZEIT_MON" DataType="System.Int32" 
                                FilterControlAltText="Filter ZEIT_MON column" SortExpression="ZEIT_MON" 
                                UniqueName="ZEIT_MON" Display="False" Groupable="False" HeaderText="ZEIT_MON">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="KFZ_STEU_GEW" 
                                FilterControlAltText="Filter KFZ_STEU_GEW column" SortExpression="KFZ_STEU_GEW" 
                                UniqueName="KFZ_STEU_GEW" Display="False" Groupable="False" HeaderText="KFZ_STEU_GEW">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="KFZ_STEU_HUBR" 
                                FilterControlAltText="Filter KFZ_STEU_HUBR column" 
                                SortExpression="KFZ_STEU_HUBR" UniqueName="KFZ_STEU_HUBR" 
                                Display="False" Groupable="False" HeaderText="KFZ_STEU_HUBR">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="KFZ_STEU_CO2" 
                                FilterControlAltText="Filter KFZ_STEU_CO2 column" SortExpression="KFZ_STEU_CO2" 
                                UniqueName="KFZ_STEU_CO2" Display="False" Groupable="False" HeaderText="KFZ_STEU_CO2">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="KFZ_STEU_PRO_TAG" 
                                FilterControlAltText="Filter KFZ_STEU_PRO_TAG column" 
                                SortExpression="KFZ_STEU_PRO_TAG" UniqueName="KFZ_STEU_PRO_TAG" 
                                Display="False" Groupable="False" HeaderText="KFZ_STEU_PRO_TAG">
                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BEMERKUNG" 
                                FilterControlAltText="Filter BEMERKUNG column" SortExpression="BEMERKUNG" 
                                UniqueName="BEMERKUNG" Display="False" Groupable="False" HeaderText="BEMERKUNG">
                                
                            </telerik:GridBoundColumn>
                        </Columns>
             
                    </MasterTableView>

                    <HeaderStyle CssClass="RadGridHeader" Wrap="False" />
                    <PagerStyle AlwaysVisible="True" />

                <FilterMenu EnableImageSprites="False"></FilterMenu>

                </telerik:radgrid> 

                <div id="dataQueryFooter">
                <asp:LinkButton ID="lb_Weiter" runat="server" CssClass="Tablebutton" Width="78px"
                    Height="16px" CausesValidation="False" Font-Underline="False" 
                        OnClick="lb_Weiter_Click">» Weiter</asp:LinkButton>
                <asp:LinkButton ID="lb_ResetView" runat="server" CssClass="TablebuttonLarge" Width="128px"
                    Height="16px" CausesValidation="False" Font-Underline="False" 
                        OnClick="cmdResetView_Click" Visible="False">» Sonstiges</asp:LinkButton>
                    </div>

                 <div id="dataFooter">&nbsp;</div>
                 
                 </div>

            </div>
        </div>
    </div>
</asp:Content>
