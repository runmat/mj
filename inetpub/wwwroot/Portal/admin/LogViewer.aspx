<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogViewer.aspx.vb" Inherits="CKG.Admin.LogViewer" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server">Administration</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"> (Datenselektion)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table5" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" width="150">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton">&#149;&nbsp;Erstellen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="TblSearch" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                                <tr id="trSearch" runat="server">
                                    <td align="left">
                                        <table border="0" bgcolor="white">
                                            <tr>
                                                <td valign="bottom" width="100">
                                                    Firma:
                                                </td>
                                                <td valign="bottom" width="160">
                                                    <asp:Label ID="lblCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:Label>
                                                    <asp:DropDownList ID="ddlFilterCustomer" runat="server" Width="160px" Height="20px" AutoPostBack="True" Visible="False"></asp:DropDownList>
                                                </td>
                                                <td valign="bottom">
                                                </td>
                                            </tr>
                                            <tr id="trGruppe" runat="server">
                                                <td valign="bottom" width="100" height="10">
                                                    Gruppe:
                                                </td>
                                                <td valign="bottom" width="160" height="10">
                                                    <asp:Label ID="lblGroup" runat="server" CssClass="InfoBoxFlat" Width="160px" Visible="False"></asp:Label><asp:DropDownList
                                                        ID="ddlFilterGroup" runat="server" Width="160px" Height="20px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td valign="bottom">
                                                </td>
                                            </tr>
                                            <tr id="trOrganisation" runat="server">
                                                <td valign="bottom" width="100" height="10">
                                                    Organisation:
                                                </td>
                                                <td valign="bottom" width="160" height="10">
                                                    <asp:Label ID="lblOrganisation" runat="server" CssClass="InfoBoxFlat" Width="160px"
                                                        Visible="False"></asp:Label><asp:DropDownList ID="ddlFilterOrganization" runat="server"
                                                            Width="160px" Height="20px">
                                                        </asp:DropDownList>
                                                </td>
                                                <td valign="bottom">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="bottom" width="100">
                                                    Benutzername:
                                                </td>
                                                <td valign="bottom" width="160">
                                                    <p>
                                                        <asp:TextBox ID="txtUserID" runat="server" Width="0px" Height="0px" Visible="False"
                                                            ForeColor="#CEDBDE" BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None">-1</asp:TextBox><asp:Label
                                                                ID="lblUserName" runat="server" CssClass="InfoBoxFlat" Width="160px" Visible="False"></asp:Label><asp:TextBox
                                                                    ID="txtFilterUserName" runat="server" Width="160px" Height="20px">*</asp:TextBox></p>
                                                </td>
                                                <td valign="bottom" height="10">
                                                    <asp:Button ID="btnSuche" runat="server" CssClass="StandardButton" Text="Benutzer suchen">
                                                    </asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="bottom" width="100">
                                                    ab Datum:
                                                </td>
                                                <td valign="bottom" width="160">
                                                    <asp:TextBox ID="txtAbDatum" runat="server" Width="130px"></asp:TextBox><asp:Button
                                                        ID="btnOpenSelectAb" runat="server" Width="30px" Height="22px" Text="..." CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                                <td valign="bottom">
                                                    <asp:Calendar ID="calAbDatum" runat="server" Width="160px" Visible="False" BorderStyle="Solid"
                                                        BorderColor="Black" CellPadding="0">
                                                        <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                                        <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                                        <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                                        <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                                        <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                                        <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                                        <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                                    </asp:Calendar>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="bottom" width="100">
                                                    bis Datum:
                                                </td>
                                                <td valign="bottom" width="160">
                                                    <asp:TextBox ID="txtBisDatum" runat="server" Width="130px"></asp:TextBox><asp:Button
                                                        ID="btnOpenSelectBis" runat="server" Width="30px" Height="22px" Text="..." CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                                <td valign="bottom">
                                                    <asp:Calendar ID="calBisDatum" runat="server" Width="160px" Visible="False" BorderStyle="Solid"
                                                        BorderColor="Black" CellPadding="0">
                                                        <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                                        <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                                        <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                                        <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                                        <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                                        <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                                        <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                                    </asp:Calendar>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="bottom" width="100">
                                                    Aufgabe:
                                                </td>
                                                <td valign="bottom" width="160" colspan="2">
                                                    <asp:DropDownList ID="ddlAction" runat="server" Width="320px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trSearchResult" runat="server">
                                    <td align="left">
                                        <asp:DataGrid ID="dgSearchResult" runat="server" Width="100%" BorderWidth="1px" BorderStyle="Solid"
                                            AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White">
                                            <SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top">
                                            </HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="UserID" SortExpression="UserID" HeaderText="UserID">
                                                </asp:BoundColumn>
                                                <asp:ButtonColumn DataTextField="UserName" SortExpression="UserName" HeaderText="Benutzername"
                                                    CommandName="Edit"></asp:ButtonColumn>
                                                <asp:BoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Kunden-&lt;br&gt;referenz">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="GroupName" SortExpression="GroupName" HeaderText="Gruppe">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="CustomerAdmin" HeaderText="Firmen-&lt;br&gt;Administrator">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbxSRCustomerAdmin" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "CustomerAdmin") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="TestUser" HeaderText="Testzugang">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbxSRTestUser" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "TestUser") %>'
                                                            Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                            <table id="TblLog" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                        <asp:Label ID="Label2" runat="server"> Datenanzeige</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:HyperLink>&nbsp;
                                        <asp:Label ID="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" CellPadding="0" AllowSorting="True"
                                            AllowPaging="True" AutoGenerateColumns="False" BackColor="White">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButton1" runat="server"></asp:ImageButton>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" Visible="False"></asp:CheckBox>
                                                        <asp:Label ID="lblID" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="UserName" SortExpression="UserName" HeaderText="Benutzername">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Inserted" SortExpression="Inserted" HeaderText="Angelegt">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Task" SortExpression="Task" HeaderText="Anwendung"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Identification" SortExpression="Identification" HeaderText="Identifikation">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="Description" HeaderText="Beschreibung">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>'
                                                            NAME="Label1">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="Textbox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>'
                                                            NAME="Textbox1">
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="&amp;gt;&amp;gt;&amp;gt;" PrevPageText="&amp;lt;&amp;lt;&amp;lt;"
                                                Position="Top"></PagerStyle>
                                        </asp:DataGrid>                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            &nbsp;
                        </td>
                        <td valign="top" align="left">
                            <asp:Label ID="lblError" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            &nbsp;
                        </td>
                        <td valign="top" align="left">
                            <!--#include File="../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
