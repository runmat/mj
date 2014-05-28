<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportAuswertung_2.aspx.cs" 
    Inherits="AppZulassungsdienst.forms.ReportAuswertung_2" MasterPageFile="../MasterPage/Big.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
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
                <div id="innerContentRight" style="width: 100%; margin-bottom: 10px">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                    <asp:Panel ID="Panel1" runat="server">
                        <div id="TableQuery" runat="server">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="5" style="vertical-align:top">
                                        <span>
                                            <asp:RadioButtonList ID="rbAuswahl" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" 
                                                onselectedindexchanged="rbAuswahl_SelectedIndexChanged" Width="100%">
                                                <asp:ListItem Text="Übersicht" Value="0" Selected="true" ></asp:ListItem> 
                                                <asp:ListItem Text="Liste nach Kennzeichen" Value="1" ></asp:ListItem>
                                                <asp:ListItem Text="Liste nach Datum / Amt" Value="2" ></asp:ListItem>
                                                <asp:ListItem Text="Liste nach Datum / Amt / EC" Value="3" ></asp:ListItem>
                                                <asp:ListItem Text="Liste nach Kundennummer" Value="4" ></asp:ListItem>
                                                <asp:ListItem Text="Liste nach Dienstleistung" Value="5" ></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="width:100px">
                                        <asp:Label ID="Label1" runat="server" Text="Legende:"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width:200px">
                                        <div style="background-color: #FAFFBF; height:18px;width:18px; float:left"></div>
                                        <asp:Label ID="Label2" runat="server" style="padding-left:5px"  Text="Zwischensumme 1"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width:200px">
                                        <div style="background-color: #D2D7BF; height:18px;width:18px; float:left"></div>
                                        <asp:Label ID="Label3" runat="server" style="padding-left:5px"  Text="Zwischensumme 2"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width:200px">
                                        <div style="background-color: #FAFFA1; height:18px;width:18px; float:left"></div>
                                        <asp:Label ID="Label5" runat="server" style="padding-left:5px"  Text="Zwischensumme 3"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width:200px">
                                        <div style="background-color: #FFB27F; height:18px;width:18px; float:left"></div>
                                        <asp:Label ID="Label4" runat="server" style="padding-left:5px" Text="Summe Gesamt"></asp:Label>
                                    </td>    
                                </tr>
                                <tr class="formquery">
                                    <td colspan="5">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <div id="Result" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <div id="divExcelExport" runat="server" style="padding-right: 4px">
                                        <div align="right">
                                            <span style="float:right; margin-bottom: 2px; margin-left: 2px">
                                                <asp:LinkButton ID="lnkCreateExcel" runat="server" OnClick="lnkCreateExcel_Click" >Excel herunterladen</asp:LinkButton>
                                            </span>
                                            <img src="/PortalZLD/Images/iconXLS.gif" alt="Excel herunterladen" style="float:right; margin-bottom: 2px"/>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 1189px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE"  
                                            OnItemCommand="rgGrid1_ItemCommand" OnItemDataBound="rgGrid1_ItemDataBound"  
                                            OnNeedDataSource="rgGrid1_NeedDataSource" Skin="Default" 
                                            OnDataBound="rgGrid1_DataBound">
                                            <PagerStyle AlwaysVisible="True"/>
                                            <ClientSettings AllowKeyboardNavigation="true" >
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" />
                                            </ClientSettings>
                                            <GroupingSettings RetainGroupFootersVisibility="true" />
                                            <ItemStyle CssClass="ItemStyle" />
                                            <AlternatingItemStyle CssClass="ItemStyle" />
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                ShowGroupFooter="True" ShowFooter="True">
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="ZULBELN" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <FooterStyle BackColor="#FFB27F" HorizontalAlign="Right" Wrap="false" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="ZULBELN" SortExpression="ZULBELN" HeaderText="ID" UniqueName="ZULBELN" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KUNNR" SortExpression="KUNNR" HeaderText="Kundennr" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KUNDENNAME" SortExpression="KUNDENNAME" HeaderText="Kunde" >
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZZREFNR1" SortExpression="ZZREFNR1" HeaderText="Referenz1" >
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZZREFNR2" SortExpression="ZZREFNR2" HeaderText="Referenz2" >
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZZZLDAT" SortExpression="ZZZLDAT" HeaderText="Zulassungsdatum" 
                                                        DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZZKENN" SortExpression="ZZKENN" HeaderText="Kennzeichen" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KREISKZ" SortExpression="KREISKZ" HeaderText="StVA" >
                                                        <HeaderStyle Width="40px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="MENGE" SortExpression="MENGE" HeaderText="Menge" >
                                                        <HeaderStyle Width="45px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Visible='<%# (Eval("MENGE").ToString() != "0") %>' Text='<%# Eval("MENGE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="MAKTX" SortExpression="MAKTX" HeaderText="Dienstleistung" >
                                                        <HeaderStyle Width="140px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Bemerkung" SortExpression="Bemerkung" HeaderText="Bemerkung" >
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KSTATUS" SortExpression="KSTATUS" HeaderText="Status" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="PREIS_DL" SortExpression="PREIS_DL" HeaderText="Preis" DataFormatString="{0:c}" 
                                                        Aggregate="Sum" >
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="PREIS_GB" SortExpression="PREIS_GB" HeaderText="Gebühr" DataFormatString="{0:c}" 
                                                        Aggregate="Sum" >
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="GEB_AMT" SortExpression="GEB_AMT" HeaderText="Gebühr Amt" DataFormatString="{0:c}" 
                                                        Aggregate="Sum" >
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="PREIS_ST" SortExpression="PREIS_ST" HeaderText="Steuer" DataFormatString="{0:c}" 
                                                        Aggregate="Sum" >
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>                                                                                                                                                            
                                                    <telerik:GridBoundColumn DataField="PREIS_KZ" SortExpression="PREIS_KZ" HeaderText="Preis Kennz." 
                                                        DataFormatString="{0:c}" Aggregate="Sum" >
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn> 
                                                    <telerik:GridTemplateColumn DataField="FEINSTAUBAMT" SortExpression="FEINSTAUBAMT" HeaderText="Feinstaub" >
                                                        <HeaderStyle Width="30px" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Visible='<%# (Eval("FEINSTAUBAMT").ToString() == "X") %>' Text="F"></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn> 
                                                    <telerik:GridBoundColumn DataField="RESWUNSCH" SortExpression="RESWUNSCH" HeaderText="RW" >
                                                        <HeaderStyle Width="30px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="EC_JN" SortExpression="EC_JN" HeaderText="EC" >
                                                        <HeaderStyle Width="30px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="BAR_JN" SortExpression="BAR_JN" HeaderText="Bar" >
                                                        <HeaderStyle Width="30px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="RE_JN" SortExpression="RE_JN" HeaderText="RE" >
                                                        <HeaderStyle Width="30px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZAHLART" SortExpression="ZAHLART" HeaderText="Zahlungsart" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="BARQ_NR" SortExpression="BARQ_NR" >
                                                        <HeaderStyle Width="25px" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="cmdPrint" CommandName="Print" CommandArgument='<%# Eval("ZULBELN") %>'
                                                                runat="server" ToolTip="Barquittung" Visible='<%# (Eval("BARQ_NR").ToString().Length > 0) %>' 
                                                                    ImageUrl="/PortalZLD/Images/iconPDF.gif" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="VE_ERNAM" SortExpression="VE_ERNAM" HeaderText="Vorerfasser" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ERNAM" SortExpression="ERNAM" HeaderText="Erfasser" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="BA_VKBUR" SortExpression="BA_VKBUR" HeaderText="Verk.-Büro" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="CPD_ADRESSE" SortExpression="CPD_ADRESSE" HeaderText="Adresse" >
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="VE_ERDAT_ZEIT" SortExpression="VE_ERDAT_ZEIT" HeaderText="erfasst" 
                                                        DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="125px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="MOBUSER" SortExpression="MOBUSER" HeaderText="Mobiluser" >
                                                        <HeaderStyle Width="90px" />
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