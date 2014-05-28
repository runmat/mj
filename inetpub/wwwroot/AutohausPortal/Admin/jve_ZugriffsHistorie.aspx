<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="jve_ZugriffsHistorie.aspx.vb"
    Inherits="Admin.jve_ZugriffsHistorie" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Zugriffshistorie"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table id="TblSearch" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                                    <tr>
                                        <td class="firstLeft active" colspan="2" style="width: 100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false"></asp:Label>
                                            <asp:Label ID="lblinfo" runat="server" visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trSearch" runat="server">
                                        <td align="left">
                                            <table bgcolor="white" border="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="40">
                                                        ab Datum:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtAbDatum" runat="server" Width="130px"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtAbDatum_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtAbDatum">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                    <td class="firstLeft active" width="50">
                                                        Kunde:
                                                    </td>
                                                    <td class="active" width="160">
                                                        <asp:DropDownList ID="ddlCustomer" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="100%">
                                                    &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="40">
                                                        bis Datum:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtBisDatum" runat="server" Width="130px"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtBisDatum_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtBisDatum">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                    <td class="firstLeft active" width="50">
                                                        Benutzer:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtFilterUserName" runat="server" Width="160px" Height="20px">*</asp:TextBox>
                                                    </td>
                                                    <td width="100%">
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="40">
                                                        Filter:
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbAll" runat="server" Text="Alle Vorgänge" Checked="True" GroupName="grpKriterium"
                                                            AutoPostBack="True" BorderWidth="0"></asp:RadioButton><br />
                                                        <asp:RadioButton ID="rbOnline" runat="server" Text="Nur Online-Benutzer" GroupName="grpKriterium"
                                                            AutoPostBack="True" BorderWidth="0"></asp:RadioButton><br />
                                                        <asp:RadioButton ID="rbError" runat="server" Text="nur fehlerhafte Vorgänge"
                                                            GroupName="grpKriterium" AutoPostBack="True" BorderWidth="0"></asp:RadioButton>
                                                    </td>
                                                    <td colspan="2">
                                                        <div>
                                                            <font color="#ff0000">&nbsp;&nbsp;&nbsp;Länger als&nbsp;</font>
                                                            <asp:DropDownList ID="ddbZeit" runat="server" Width="51px">
                                                            </asp:DropDownList>
                                                            <font color="#ff0000">&nbsp;Stunde(n) online.</font>
                                                        </div>
                                                    </td>
                                                    <td width="100%"></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» 
                                 Erstellen</asp:LinkButton>
                            </div>
                            <div id="Result" runat="server" visible="false">                                
                                    <div class="ExcelDiv">
                                        <div align="right" class="rightPadding" id="ExcelDiv" runat="server">
                                            &nbsp;<img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                                            <span class="ExcelSpan">
                                                <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel 
                                                                herunterladen</asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                    <div id="pagination">
                                        <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                    </div>
                                    <div id="data">
                                        <table cellspacing="0" cellpadding="0" bgcolor="white">
                                            <tr id="trdata1" runat="server">
                                                <td>
                                                    <DBWC:HierarGrid ID="HGZ" runat="server" Width="100%" BorderStyle="solid" BorderColor="#999999"
                                                        CellPadding="0" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                                        TemplateDataMode="Table" LoadControlMode="UserControl" TemplateCachingBase="Tablename"
                                                        BorderWidth="1px" AllowSorting="True">
                                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="ItemStyle"></ItemStyle>
                                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="userName" SortExpression="userName" HeaderText="Benutzer">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Customername" SortExpression="Customername" HeaderText="Kunde">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="GroupName" HeaderText="Gruppe" SortExpression="Groupname">
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn SortExpression="TestUser" HeaderText="Test">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.TestUser") %>'
                                                                        Enabled="False"></asp:CheckBox>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TestUser") %>'>
                                                                    </asp:TextBox>
                                                                </EditItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="hostName" SortExpression="hostName" HeaderText="Abmeldestatus">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="requestType" SortExpression="requestType" HeaderText="Anfrageart">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="browser" SortExpression="browser" HeaderText="Browser">
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn SortExpression="startTime" HeaderText="&lt;u&gt;Startzeit&lt;/u&gt;">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.startTime") %>'
                                                                        ForeColor='<%# DataBinder.Eval(Container, "DataItem.StartColor") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="Textbox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.startTime") %>'
                                                                        ForeColor="Red">
                                                                    </asp:TextBox>
                                                                </EditItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="endTime" SortExpression="endTime" HeaderText="Endzeit">
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                    </DBWC:HierarGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
