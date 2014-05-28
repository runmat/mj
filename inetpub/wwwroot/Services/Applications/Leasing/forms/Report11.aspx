<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report11.aspx.cs" Inherits="Leasing.forms.Report11"
    MasterPageFile="../Master/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>

                    <!-- Umschalten der Panelansicht--> 
                    <div id="paginationQuery" >
                    <table cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr>
                                <td class="active">
                                    <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False"></asp:Label>
                                </td>
                                <td align="right">
                                    <div id="queryImage">
                                        <asp:ImageButton ID="ibtNewSearch" runat="server" 
                                            ImageUrl="../../../Images/queryArrow.gif" onclick="ibtNewSearch_Click" 
                                            style="width: 17px" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:Panel ID="divTrenn" runat="server" visible="false" ><div id="PlaceHolderDiv" ></div>   </asp:Panel>
                </div>

                    <div id="TableQuery">


                 <asp:Panel ID="divSelection" runat="server" Visible="true"  >
              

                        <table width="100%" id="Table1" runat="server" cellpadding="0" cellspacing="0" >
                        <tbody>


                            <tr class="formquery" >
                                <td class="firstLeft active">
                                 <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false" Visible="false" />
                                <asp:Label ID="lblNoData" runat="server" Font-Bold="true" EnableViewState="false" Visible="false" />

                                </td>
                            </tr>

                        <tr class="formquery" >
                        <td align="right" style="width:100%; vertical-align:top;padding-right:5px">
                        
                        <asp:Panel align="left" ID="pnInfo" runat="server" Visible="true" width="300px" Height="50px">
                                         <div class="new_layout">
                                        <div id="infopanel" class="infopanel">
                                        <label style="margin-left:0; margin-right:0; width:275px" >Information!</label>
                                        <div>
                                        Bitte zur Ausführung des Reports auf 'Weiter'<br/>klicken.
                                 </div>
                                </div> 
                                </div>
                                </asp:Panel>
                                <br/>
                        </td> 
                        </tr>
                                                <tr><td>&nbsp;</td></tr>
                        </tbody> 
                        </table>
                             
                         <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>            
                    </asp:Panel>
                 </div>

                    <telerik:RadAjaxManager runat="server">
                        <ClientEvents OnRequestStart="onRequestStart" />
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="fzgGrid">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="fzgGrid" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
    
                    <telerik:RadGrid ID="fzgGrid" AllowSorting="true" AllowPaging="true"              
                        AutoGenerateColumns="False" runat="server" GridLines="None" Width="100%" BorderWidth="0px" 
                        Culture="de-DE" Visible="false" EnableHeaderContextMenu="false" 

                          OnExcelMLExportRowCreated="fzgGrid_ExcelMLExportRowCreated"
                          OnExcelMLExportStylesCreated="fzgGrid_ExcelMLExportStylesCreated"

                        OnGridExporting="FzgGridExporting"
                        OnItemCommand="FzgGridItemCommand" OnNeedDataSource="FzgGridNeedDataSource"
                        OnPageIndexChanged="FzgGridPageChanged" OnPageSizeChanged="FzgGridPageSizeChanged"
                        OnSortCommand="FzgGridSortCommand" CellSpacing="0"  
                        ClientSettings-AllowColumnsReorder="false" VirtualItemCount="2">  
                         <HeaderContextMenu>
           
                        </HeaderContextMenu> 
                        <ExportSettings HideStructureColumns="true" IgnorePaging="True" OpenInNewWindow="True" >
                            <Excel Format="ExcelML"></Excel>
                            </ExportSettings>
                            <ExportSettings HideStructureColumns="True">
                            <Excel Format="ExcelML"></Excel>
                            <Pdf AllowCopy="True" />
                            <Excel Format="ExcelML" />
                        </ExportSettings>

                        <ClientSettings allowkeyboardnavigation="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="true"></Scrolling>
                            <Resizing AllowColumnResize="true" ClipCellContentOnResize="true"></Resizing>
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" Summary="Lizenzfahrzeuge" TableLayout="Auto" Width="100%" >
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
                                <%--Fahrgestellnummer--%>
                                <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM"
                                UniqueName="CHASSIS_NUM" Display="True" HeaderStyle-Width="20%" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Fahrzeugnummer (laut SAP:DAD Referenzfeld)--%>
                                <telerik:GridBoundColumn DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1"  
                                UniqueName="ZZREFERENZ1" Display="True" HeaderStyle-Width="20%" Groupable="False" ItemStyle-Wrap="false" />  
                                <%--ZBII/Fahrzeugbriefnummer (laut SAP:Technische Identnummer)--%>
                                <telerik:GridBoundColumn DataField="TIDNR" SortExpression="TIDNR" 
                                UniqueName="TIDNR" Display="True" HeaderStyle-Width="20%" Groupable="False" ItemStyle-Wrap="false" />  
                                <%--Kfz-Kennzeichen--%>
                                <telerik:GridBoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" 
                                UniqueName="LICENSE_NUM" Display="True" HeaderStyle-Width="20%" Groupable="False" ItemStyle-Wrap="false" />  
                                <%--Erstzulassungsdatum (laut SAP:Datum, an dem das Fahrzeug ersetzt werden soll)--%>
                                <telerik:GridBoundColumn DataField="REPLA_DATE" SortExpression="REPLA_DATE" DataFormatString="{0:dd.MM.yyyy}" 
                                UniqueName="REPLA_DATE" Display="True" HeaderStyle-Width="20%" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Equipmentnummer--%>
                                <telerik:GridBoundColumn DataField="EQUNR" SortExpression="EQUNR" 
                                UniqueName="EQUNR" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--DAD Referenzfeld--%>
                                <telerik:GridBoundColumn DataField="ZZREFERENZ2" SortExpression="ZZREFERENZ2" 
                                UniqueName="ZZREFERENZ2" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Lizenznummer des Equipments--%>
                                <telerik:GridBoundColumn DataField="LIZNR" SortExpression="LIZNR" 
                                UniqueName="LIZNR" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Angaben zum Hubraum--%>
                                <telerik:GridBoundColumn DataField="ZZHUBRAUM" SortExpression="ZZHUBRAUM" 
                                UniqueName="ZZHUBRAUM" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Typdaten Nennleistung in KW--%>
                                <telerik:GridBoundColumn DataField="ZZNENNLEISTUNG" SortExpression="ZZNENNLEISTUNG" 
                                UniqueName="ZZNENNLEISTUNG" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Leistung in kW/PS--%>
                                <telerik:GridBoundColumn DataField="LEISTUNG_PS" SortExpression="LEISTUNG_PS" 
                                UniqueName="LEISTUNG_PS" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Farbziffer des Fahrzeugs--%>
                                <telerik:GridBoundColumn DataField="ZFARBE" SortExpression="ZFARBE" 
                                UniqueName="ZFARBE" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Typdaten Handelsname--%>
                                <telerik:GridBoundColumn DataField="ZZHANDELSNAME" SortExpression="ZZHANDELSNAME" 
                                UniqueName="ZZHANDELSNAME" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Typdaten Hersteller Schlüssel--%>
                                <telerik:GridBoundColumn DataField="ZZ_HERSTELLER_SCHL" SortExpression="ZZ_HERSTELLER_SCHL" 
                                UniqueName="ZZ_HERSTELLER_SCHL" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Typdaten Hersteller Kurzbezeichnung--%>
                                <telerik:GridBoundColumn DataField="ZZHERST_TEXT" SortExpression="ZZHERST_TEXT" 
                                UniqueName="ZZHERST_TEXT" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Typdaten Typ Schlüssel--%>
                                <telerik:GridBoundColumn DataField="ZZTYP_SCHL" SortExpression="ZZTYP_SCHL" 
                                UniqueName="ZZTYP_SCHL" Display="False" Groupable="False" ItemStyle-Wrap="false" />  
                                <%--Fahrzeugtyp (P = PKW oder L = LKW)--%>
                                <telerik:GridBoundColumn DataField="TYP" SortExpression="TYP" 
                                UniqueName="TYP" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--Typdaten Kraftstoffart Text--%>
                                <telerik:GridBoundColumn DataField="ZZ_KRAFTSTOFF_TXT" SortExpression="ZZ_KRAFTSTOFF_TXT"  
                                UniqueName="ZZ_KRAFTSTOFF_TXT" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                                <%--MVA-Nummer--%>
                                <telerik:GridBoundColumn DataField="MVA_NUMMER" SortExpression="MVA_NUMMER" 
                                UniqueName="MVA_NUMMER" Display="False" Groupable="False" ItemStyle-Wrap="false" /> 
                            </Columns>
                        </MasterTableView>
                         <HeaderStyle CssClass="RadGridHeader" Wrap="False" />
                    
                    <PagerStyle AlwaysVisible="True" />

                <FilterMenu EnableImageSprites="False"></FilterMenu>
                    </telerik:RadGrid>

                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                            Height="16px" CausesValidation="False" Font-Underline="False" OnClick="SearchClick">» Weiter</asp:LinkButton>
                    </div>
                    
                    <br/>

                    <div id="dataFooter">&nbsp;</div>
                </div>
            </div>
        </div>
        </div>
</asp:Content>
