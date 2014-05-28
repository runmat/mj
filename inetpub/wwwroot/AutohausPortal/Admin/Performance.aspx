<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Performance.aspx.vb" Inherits="Admin.Performance"
    MasterPageFile="MasterPage/Admin.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        <!--
        window.setInterval('window.location.reload()',15000);
        //-->
    </script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <asp:Label ID="lblError" runat="server" Text="Label"></asp:Label>
                    </div>
                        <div id="Result" runat="Server" visible="true">
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:Gridview id="DataGrid1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="20" CssClass="GridView">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle Wrap="false" CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
										    <Columns>
											    <asp:TemplateField SortExpression="CategoryName" HeaderText="Kategorie">
												    <ItemTemplate>
													    <asp:HyperLink id="HyperLink1" runat="server" NavigateUrl='<%# "Performance2.aspx?PerformanceCounterID=" &amp; DataBinder.Eval(Container, "DataItem.PerformanceCounterID") %>' Text='<%# DataBinder.Eval(Container, "DataItem.CategoryName") %>'>
													    </asp:HyperLink>
												    </ItemTemplate>
											    </asp:TemplateField>
											    <asp:BoundField DataField="CounterName" SortExpression="CounterName" HeaderText="Bezeichnung"></asp:BoundField>
											    <asp:BoundField DataField="InstanceName" SortExpression="InstanceName" HeaderText="Instanz"></asp:BoundField>
											    <asp:BoundField DataField="PerformanceCounterValue" SortExpression="PerformanceCounterValue" HeaderText="Wert">
											    </asp:BoundField>
											    <asp:BoundField DataField="CounterUnit" SortExpression="CounterUnit" HeaderText="Einheit"></asp:BoundField>
										     </Columns>
                                            </asp:Gridview>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
