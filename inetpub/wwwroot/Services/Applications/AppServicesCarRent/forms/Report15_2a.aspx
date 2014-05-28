<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report15_2a.aspx.vb" Inherits="AppServicesCarRent.Report15_2a"
    MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI"  TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<telerik:RadScriptManager ID="RadScriptManager1" runat="server" />--%>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgLebenslaufTueteStkl">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgLebenslaufTueteStkl" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
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
                                        <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="ibtNewSearch" runat="server" ImageUrl="../../../Images/queryArrowUp.gif" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:Panel ID="divTrenn" runat="server" Visible="false">
                            <div id="PlaceHolderDiv">
                            </div>
                        </asp:Panel>
                    </div>
                    <div>
                        <asp:Label ID="lblError" runat="server" CssClass="TextError">&nbsp;</asp:Label>
                    </div>
                    <!--div der Klasse HistTabPanel ist erforderlich, damit die css-Einstellungen für den table greifen-->
                    <div class="HistTabPanel">
                        <table id="Table6" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td class="First" style="width: 150px">
                                    <asp:Label ID="lblFahrzeugIdTitel" runat="server"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <asp:Label ID="lblFahrzeugId" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="Label5" runat="server">Komponenten</asp:Label>
                                </td>
                            </tr>
                            <tr id="trLebenslaufTueteStkl" runat="server">
                                <td colspan="5" style="padding: 0">
                                    <telerik:RadGrid ID="rgLebenslaufTueteStkl" runat="server" PageSize="20" AllowPaging="True" 
                                        AutoGenerateColumns="False" GridLines="None">
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                        <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="False">
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldAlias="Nr." FieldName="STLKN"></telerik:GridGroupByField>
                                                        <telerik:GridGroupByField FieldAlias="Typ" FieldName="MAKTX"></telerik:GridGroupByField>
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="STLKN"></telerik:GridGroupByField>
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                            <HeaderStyle ForeColor="White" />
                                            <Columns>
                                                <telerik:GridBoundColumn SortExpression="STLKN" HeaderText="Stücklistenknotennummer" HeaderButtonType="TextButton"
                                                    DataField="STLKN" Visible="false" UniqueName="STLKN">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="MAKTX" HeaderText="Bezeichnung" HeaderButtonType="TextButton"
                                                    DataField="MAKTX" UniqueName="MAKTX" Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="ZAKT_NR" HeaderText="Aktivitätsnummer" HeaderButtonType="TextButton"
                                                    DataField="ZAKT_NR" Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="AKTI_BEZEI" HeaderText="Aktivität" HeaderButtonType="TextButton"
                                                    DataField="AKTI_BEZEI">
                                                    <HeaderStyle Width="20%" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="UNAME" HeaderText="Sachbearbeiter" HeaderButtonType="TextButton"
                                                    DataField="UNAME">
                                                    <HeaderStyle Width="16%" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="DATUM" HeaderText="Datum" HeaderButtonType="TextButton" 
                                                    DataField="DATUM" DataFormatString="{0:dd.MM.yyyy}">
                                                    <HeaderStyle Width="12%" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Uhrzeit">
                                                    <HeaderStyle ForeColor="#ffffff" Width="12%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="LabelUhrzeit" runat="server" Text='<%# getTimeString(DataBinder.Eval(Container, "DataItem.UZEIT"), DataBinder.Eval(Container, "DataItem.AKTI_BEZEI")) %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Versandadresse">
                                                    <HeaderStyle ForeColor="#ffffff" Width="40%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1") & " " & DataBinder.Eval(Container, "DataItem.NAME2") %>'>
                                                        </asp:Label>
                                                        <asp:Literal ID="Literal1" runat="server" Text="<br>">
                                                        </asp:Literal>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STREET") & " " & DataBinder.Eval(Container, "DataItem.HOUSE_NUM1") %>'>
                                                        </asp:Label>
                                                        <asp:Literal ID="Literal2" runat="server" Text="<br>">
                                                        </asp:Literal>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.POST_CODE1") & " " & DataBinder.Eval(Container, "DataItem.CITY1") & " " & DataBinder.Eval(Container, "DataItem.COUNTRY") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="dataQueryFooter">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
