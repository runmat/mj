<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportTagList_2.aspx.cs" 
    Inherits="AppZulassungsdienst.forms.ReportTagList_2" MasterPageFile="../MasterPage/App.Master" %>
    
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?26082013"></script>
    <style type="text/css">
        .RadGrid_Default .rgHeader a {
            color: white !important;
        }
    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" onclick="lb_zurueck_Click"></asp:LinkButton>
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
                        if (args.get_eventTarget().indexOf("cmdCreate") >= 0) {
                            args.set_enableAjax(false);
                        }
                    }
                </script>
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" Height="35px"></asp:Label>
                    <div id="Result" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                            <tr class="formquery">
                                <td class="firstLeft active" style="height: 35px">
                                    <div style="width:25px; height:25px; background-color:#EA7272"></div>
                                </td>
                                <td class="firstLeft active" style="height: 35px">
                                    Hier wurden bereits Tageslisten gedruckt!</td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <div style="width:25px; height:25px; background-color:#FFDB6D"></div>
                                </td>
                                <td class="firstLeft active" style="width:100%" >
                                    Diese Datensätze wurden bereits in einer Tagesliste gedruckt!<br />
                                    Bei erneutem Druck muss die alte Tagesliste vernichtet werden! Soll keine 
                                    erneute Ausgabe erfolgen, dann Liste nicht auswählen!
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="width: 100%; max-width: 1189px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="10" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" Skin="Default" 
                                            OnItemDataBound="rgGrid1_ItemDataBound" OnNeedDataSource="rgGrid1_NeedDataSource" >
                                            <PagerStyle AlwaysVisible="True"/>
                                            <ClientSettings AllowKeyboardNavigation="true" EnableAlternatingItems="False" >
                                                <Scrolling ScrollHeight="440px" AllowScroll="True" UseStaticHeaders="True" />
                                            </ClientSettings>
                                            <GroupingSettings RetainGroupFootersVisibility="true" />
                                            <ItemStyle CssClass="ItemStyle" />
                                            <AlternatingItemStyle CssClass="ItemStyle" />
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true">
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <GroupByExpressions>
                                                    <telerik:GridGroupByExpression>
                                                        <SelectFields>
                                                            <telerik:GridGroupByField FieldName="KREISKZ" HeaderText="Kreis"></telerik:GridGroupByField>
                                                            <telerik:GridGroupByField FieldName="ZZZLDAT" FormatString="{0:dd.MM.yyyy}" HeaderText="Zulassungsdatum"></telerik:GridGroupByField>
                                                        </SelectFields>
                                                        <GroupByFields>
                                                            <telerik:GridGroupByField FieldName="KREISKZ"></telerik:GridGroupByField>
                                                        </GroupByFields>
                                                    </telerik:GridGroupByExpression>
                                                </GroupByExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <FooterStyle BackColor="#FFB27F" HorizontalAlign="Right" Wrap="false" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="KREISKZ" SortExpression="KREISKZ" HeaderText="Kreis" AllowSorting="False" >
                                                        <HeaderStyle Width="50px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZZZLDAT" SortExpression="ZZZLDAT" HeaderText="Zulassungsdatum" DataFormatString="{0:dd.MM.yyyy}" AllowSorting="False" >
                                                        <HeaderStyle Width="110px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="DRUKZ" SortExpression="DRUKZ" Visible="False" >
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="BLTYP" SortExpression="BLTYP" Visible="False" >
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KUNNR" SortExpression="KUNNR" Visible="False" >
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZULBELN" SortExpression="ZULBELN" Visible="False" >
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZULPOSNR" SortExpression="ZULPOSNR" Visible="False" >
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="NAME1" SortExpression="NAME1" HeaderText="Kundenname" >
                                                        <HeaderStyle Width="200px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZZREFNR1" SortExpression="ZZREFNR1" HeaderText="Referenz1" >
                                                        <HeaderStyle Width="180px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZZKENN" SortExpression="ZZKENN" HeaderText="Kennzeichen" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="MAKTX" SortExpression="MAKTX" HeaderText="Dienstleistung" >
                                                        <HeaderStyle Width="200px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="FLAG" SortExpression="FLAG" Visible="False" >
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdCreatePdf" runat="server" CssClass="TablebuttonLarge" Height="22px" Width="128px" 
                                onclick="cmdCreatePdf_Click">» PDF erzeugen </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
