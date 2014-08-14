<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Zulassungsstatistik_02.aspx.cs"
    Inherits="AutohausPortal.forms.Zulassungsstatistik_02" MasterPageFile="/AutohausPortal/MasterPage/FormBig.Master" %>

<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/Form.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">
    function onRequestStart(sender, args) {
        if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                    args.get_eventTarget().indexOf("lnkPrint") >= 0) {
            args.set_enableAjax(false);
        }
    }
    </script>
    <div>
        <h4>
            <asp:Label ID="lblError" runat="server" Style="color: #B54D4D" Text=""> </asp:Label>
            <asp:Label ID="lblMessage" runat="server" Style="color: #269700" Text=""> </asp:Label>
        </h4>
    </div>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <!-- content start -->
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="legendcontentSmall clear">
        <div class="legenditemSmall">
            <b>O</b> = Offen</div>
        <div class="legenditemSmall">
            <b>A</b> = Angenommen</div>
        <div class="legenditemSmall">
            <b>DIS</b> = Disponiert</div>
        <div class="legenditemSmall">
            <b>GO</b> = in Arbeit</div>
        <div class="legenditemSmall">
            <b>D</b> = Durchgeführt</div>
        <div class="legenditemSmall">
            <b>F</b> = Fehlgeschlagen</div>
        <div class="legenditemSmall">
            <b>AR</b> = Abgerechnet</div>
        <div class="legenditemSmall">
            <b>L</b> = Gelöscht</div>
    </div>
    <div id="divExcelExport" runat="server" style="padding-right: 4px">
        <div align="right">
            <span style="float:right">
                <asp:LinkButton ID="lnkCreateExcel" runat="server" OnClick="lnkCreateExcel_Click" >Excel herunterladen</asp:LinkButton>
            </span>
            <img src="../Images/iconXLS.gif" alt="Excel herunterladen" style="float:right"/>
        </div>
    </div>
    <telerik:RadGrid ID="RadGrid1" runat="server" Width="95%" AutoGenerateColumns="False"
        PageSize="50" AllowSorting="True" AllowMultiRowSelection="False" AllowPaging="True" 
        OnDetailTableDataBind="RadGrid1_DetailTableDataBind" CssClass="GridView" OnNeedDataSource="RadGrid1_NeedDataSource"
        OnPreRender="RadGrid1_PreRender" EnableEmbeddedSkins="false" OnItemCommand="RadGrid1_ItemCommand"  
        OnItemDataBound="RadGrid1_ItemDataBound" Culture="de-DE">
        <PagerStyle Mode="NumericPages"></PagerStyle>
        <MasterTableView Width="100%" DataKeyNames="KUNNR" AllowMultiColumnSorting="True" ExpandCollapseColumn-Display="true"
            RowIndicatorColumn-Display="false">
            <ItemStyle CssClass="ItemStyle" />
            <AlternatingItemStyle CssClass="ItemStyle" />
            <HeaderStyle CssClass="GridTableHead" />
                <DetailTables>
                        <telerik:GridTableView DataKeyNames="KUNNR" Name="Orders" Width="100%" AllowPaging="True" PagerStyle-Wrap="True" 
                        RetrieveAllDataFields="True" PagerStyle-Mode="NextPrevAndNumeric" PagerStyle-PagerTextFormat="Seite wechseln: {4} &nbsp;Seite <strong>{0}</strong> von <strong>{1}</strong>, Vorgänge <strong>{2}</strong> bis <strong>{3}</strong> von <strong>{5}</strong>.">
                        <ItemStyle CssClass="ItemStyle"  />
                        <AlternatingItemStyle CssClass="ItemStyle" />
                        <HeaderStyle CssClass="GridTableHead"/>
                            <Columns>
                                <telerik:GridBoundColumn SortExpression="ZULBELN" HeaderText="ID" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="ZULBELN">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="BEB_STATUS" HeaderText="Status" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="BEB_STATUS" UniqueName="BEB_STATUS">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="ZZREFNR1" HeaderText="Referenz1" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="ZZREFNR1" UniqueName="ZZREFNR1">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="ZZREFNR2" HeaderText="Referenz2" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="ZZREFNR2" UniqueName="ZZREFNR2">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="ZZREFNR3" HeaderText="Referenz3" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="ZZREFNR3" UniqueName="ZZREFNR3">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="ZZREFNR4" HeaderText="Referenz4" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="ZZREFNR4" UniqueName="ZZREFNR4">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="ZZEVB" HeaderText="eVB-Nummer" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="ZZEVB" UniqueName="ZZEVB">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="ZZKENN" HeaderText="Kennzeichen" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="ZZKENN" UniqueName="ZZKENN">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="VE_ERDAT" HeaderStyle-Font-Bold="true" HeaderText="Beauftr.- Datum"
                                    HeaderButtonType="LinkButton" DataField="VE_ERDAT" UniqueName="VE_ERDAT" DataFormatString="{0:d}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="ZZZLDAT" HeaderStyle-Font-Bold="true" HeaderText="Zulassungsdatum"
                                    HeaderButtonType="LinkButton" DataField="ZZZLDAT" UniqueName="ZZZLDAT" DataFormatString="{0:d}">
                                </telerik:GridBoundColumn>
                                <telerik:GridNumericColumn SortExpression="MAKTX" DataField="MAKTX"  HeaderText="Dienstleistung" UniqueName="MAKTX">
                                </telerik:GridNumericColumn>
                                <telerik:GridBoundColumn SortExpression="PREIS_DL" HeaderStyle-Font-Bold="true" HeaderButtonType="LinkButton" DataField="PREIS_DL"  HeaderText="Preis" UniqueName="PREIS_DL">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="PREIS_GB" HeaderStyle-Font-Bold="true" HeaderButtonType="LinkButton" DataField="PREIS_GB"  HeaderText="Gebühr" UniqueName="PREIS_GB">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="PREIS_ST" HeaderStyle-Font-Bold="true" HeaderButtonType="LinkButton" DataField="PREIS_ST"  HeaderText="Steuer" UniqueName="PREIS_ST">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="PREIS_KZ" HeaderStyle-Font-Bold="true" HeaderButtonType="LinkButton" DataField="PREIS_KZ"  HeaderText="Preis Kennz." UniqueName="PREIS_KZ">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="FEINSTAUBAMT" HeaderText="F" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="FEINSTAUBAMT" UniqueName="FEINSTAUBAMT">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="RESWUNSCH" HeaderText="R/W" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="RESWUNSCH" UniqueName="RESWUNSCH">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="VK_KUERZEL" HeaderText="VK-Kürzel" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="VK_KUERZEL" UniqueName="VK_KUERZEL">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="KUNDEN_REF" HeaderText="interne Ref." HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="KUNDEN_REF" UniqueName="KUNDEN_REF">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="KUNDEN_NOTIZ" HeaderText="Notiz" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="KUNDEN_NOTIZ" UniqueName="KUNDEN_NOTIZ">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="VE_ERNAM" HeaderText="erfasst durch" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="VE_ERNAM" UniqueName="VE_ERNAM">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="VE_ERZEIT" HeaderText="abgesendet um" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="VE_ERZEIT" UniqueName="VE_ERZEIT">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemTemplate> 
                                        <asp:ImageButton runat="server" ID="lnkPrint"  CommandName="print" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.AH_DOKNAME") %>'
                                         ImageUrl="../images/logoPDF.png" Visible='<%# DataBinder.Eval(Container, "DataItem.AH_DOKNAME").ToString() != "" %>' ></asp:ImageButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                        </Columns>
                    </telerik:GridTableView>
                </DetailTables>
                    <columns>
                        <telerik:GridBoundColumn SortExpression="KUNNR"  HeaderText="Kundennr."  HeaderStyle-Font-Bold ="true" HeaderButtonType="LinkButton"
                            DataField="KUNNR">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn SortExpression="NAME1"  HeaderText="Kundenname"  HeaderStyle-Font-Bold ="true" HeaderButtonType="LinkButton"
                            DataField="NAME1">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn >
                    
                        </telerik:GridTemplateColumn>
                    </columns>
        </MasterTableView>
    </telerik:RadGrid>
    <div class="formbuttons">
        <asp:Button ID="cmdExport" runat="server" CssClass="button" Text="exportieren" 
            onclick="cmdExport_Click" />
    </div>
    <asp:UpdatePanel ID="upnlJsRunner" UpdateMode="Always" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="phrJsRunner" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
