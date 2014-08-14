<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SapMonitoring.aspx.vb"
    Inherits="Admin.SAPMonitoring" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
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
                            <asp:Label ID="lblHead" runat="server" Text="SAPMonitoring"></asp:Label>
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
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trSearch" runat="server">
                                        <td align="left">
                                            <table bgcolor="white" border="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="70">
                                                        ab Datum:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtAbDatum" runat="server" Width="130px"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtAbDatum_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtAbDatum">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                    <td width="100%">
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="70">
                                                        bis Datum:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtBisDatum" runat="server" Width="130px"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtBisDatum_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtBisDatum">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                    <td width="100%">
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="70">
                                                        Kriterium
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbBAPI" runat="server" Text="BAPI" GroupName="grpKriterium"
                                                            AutoPostBack="True" BorderWidth="0"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:RadioButton ID="rbASPX" runat="server" Text="ASPX-Seite" Checked="True" GroupName="grpKriterium"
                                                            AutoPostBack="True" BorderWidth="0"></asp:RadioButton>
                                                    </td>
                                                    <td width="100%">
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="70">
                                                        Auswahl:
                                                    </td>
                                                    <td width="160">
                                                        <asp:DropDownList ID="ddlAuswahl" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="100%">
                                                    </td>
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
                            <div id="Result">
                                <div id="Resultshow" runat="server" visible="false">
                                    <div class="ExcelDiv">
                                        <div align="right" class="rightPadding" id = "ExcelDiv" runat="server">
                                            &nbsp;<img src="/PortalZLD/Images/iconXLS.gif" alt="Excel herunterladen" />
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
                                        <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                            <tr id="trdata1" runat="server">
                                                <td>
                                                    <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" CellPadding="0" BackColor="White"
                                                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" CssClass="GridView"
                                                        Border="0">
                                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                        <ItemStyle Wrap="false" CssClass="ItemStyle" />
                                                        <Columns>
                                                            <asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer">
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn SortExpression="Testbenutzer" HeaderText="Testbenutzer">
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Testbenutzer") %>'>
                                                                    </asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="BAPI" SortExpression="BAPI" HeaderText="BAPI"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Parameter">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Start" SortExpression="Start" HeaderText="Start"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Ende" SortExpression="Ende" HeaderText="Ende"></asp:BoundColumn>
                                                            <asp:TemplateColumn SortExpression="Dauer" HeaderText="Dauer">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer"))) %>'
                                                                        Height="10px" BackColor="#8080FF">
                                                                    </asp:Label>
                                                                    <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn SortExpression="Erfolg" HeaderText="Erfolg">
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox2" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Erfolg") %>'>
                                                                    </asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="Fehlermeldung" SortExpression="Fehlermeldung" HeaderText="Fehlermeldung">
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        <PagerStyle NextPageText="&amp;gt;&amp;gt;&amp;gt;" PrevPageText="&amp;lt;&amp;lt;&amp;lt;"
                                                            Mode="NumericPages"></PagerStyle>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                            <tr id="trdata2" runat="server">
                                            <td>
                                            <DBWC:HierarGrid ID="HGZ" runat="server" Width="100%" BorderStyle="None" BorderColor="#999999"
                                                    CellPadding="0" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                                    TemplateDataMode="Table" LoadControlMode="UserControl" TemplateCachingBase="Tablename"
                                                    BorderWidth="1px" AllowSorting="True">
                                                    <PagerStyle Mode="NumericPages"></PagerStyle>
                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                    <ItemStyle CssClass="ItemStyle"></ItemStyle>
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Seite" SortExpression="Seite" HeaderText="Seite"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Anwendung" SortExpression="Anwendung" HeaderText="Anwendung">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Zugriffe SAP" SortExpression="Zugriffe SAP" HeaderText="Zugriffe&lt;br&gt;SAP">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Kunnr" SortExpression="Kunnr" HeaderText="Kundennr.">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Customername" SortExpression="Customername" HeaderText="Kundenname">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AccountingArea" SortExpression="AccountingArea" HeaderText="AccountingArea">
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn SortExpression="Testbenutzer" HeaderText="Testbenutzer">
                                                            <HeaderStyle Width="100px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Checkbox4" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Testbenutzer") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn SortExpression="Start ASPX" HeaderText="Start ASPX">
                                                            <HeaderStyle Width="150px"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="Hyperlink4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Start ASPX") %>'
                                                                    ToolTip='<%# DataBinder.Eval(Container, "DataItem.Ende ASPX") %>'>
                                                                </asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn SortExpression="Dauer ASPX" HeaderText="Dauer">
                                                            <HeaderStyle HorizontalAlign="Center" Width="205px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            <ItemTemplate>
                                                                <table id="Table18" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td width="50">
                                                                            ASPX
                                                                        </td>
                                                                        <td width="30" align="right">
                                                                            <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer ASPX") %>'>
                                                                            </asp:Label>&nbsp;
                                                                        </td>
                                                                        <td width="125">
                                                                            <asp:Label ID="Label5" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer ASPX"))) %>'
                                                                                Height="10px" BackColor="#8080FF">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="25">
                                                                            SAP
                                                                        </td>
                                                                        <td width="30" align="right">
                                                                            <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer SAP") %>'>
                                                                            </asp:Label>&nbsp;
                                                                        </td>
                                                                        <td width="125">
                                                                            <asp:Label ID="Label7" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer SAP"))) %>'
                                                                                Height="10px" BackColor="Highlight">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                </DBWC:HierarGrid>
                                                </td>
                                                </tr>
                                        </table>
                                    </div>
                                </div>
                                
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
