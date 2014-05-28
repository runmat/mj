<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AutorisierungenLoeschen.aspx.vb"
    Inherits="Admin.AutorisierungenLoeschen" MasterPageFile="MasterPage/Admin.Master" %>

<%--<%@ Register TagPrefix="uc1" TagName="menue" Src="Sidebar/Menue.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="author" content="Christoph Kroschke AG" />
    <meta name="generator" content="Zend Development Environment" />
    <style type="text/css">
        #DoubleLogin
        {
            width: 73%;
        }
        #Table4
        {
            width: 549px;
        }
        .style1
        {
            width: 994px;
        }
    </style>
</asp:Content>--%>
<%--<asp:Content ID="ContentMenue" ContentPlaceHolderID="ContentPlaceHolderMenue" runat="server">
    <uc1:menue runat="server" />
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                    <div id="navigationSubmenu">
                    </div>
                    <%--<form id="Form1" runat="server" method="post">--%>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <div id="data" style="border-style: solid; border-width: 1px; border-color: #dfdfdf">
                            <table cellspacing="0" cellpadding="0" width="80%" align="center">
                                <tr>
                                    <td>
                                        <table cellpadding="0" style="border-color: #ffffff" id="tableSearch" runat="server"
                                            cellspacing="1">
                                            <tbody>
                                                <tr>
                                                    <td class="cellBorderGray active" align="center">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">
                                        <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" width="100%"
                                            align="left" border="0">
                                            <tbody>
                                                <tr id="trSearchSpacer" runat="server">
                                                    <td align="left" nowrap="nowrap" width="50%">
                                                    </td>
                                                </tr>
                                                <tr id="trSearchResult" runat="server">
                                                    <td align="left" class="style1">
                                                        <asp:GridView ID="dgSearchResult" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" PageSize="20"
                                                            HeaderStyle-CssClass="Tablehead" Width="100%" Font-Underline="False">
                                                            <PagerSettings Position="Bottom" Visible="True" FirstPageText="&lt;&lt;" LastPageText="&gt;&gt;"
                                                                Mode="Numeric" PageButtonCount="2" />
                                                            <Columns>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAuthID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AuthorizationID") %>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:ButtonField DataTextField="AppFriendlyName" SortExpression="AppFriendlyName"
                                                                    HeaderText="Anwendung" CommandName="Edit">
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="TableLinkHead" ForeColor="Black">
                                                                    </HeaderStyle>
                                                                    <ItemStyle CssClass="TableLink" Font-Underline="true" HorizontalAlign="Center" ForeColor="#595959">
                                                                    </ItemStyle>
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="InitializedBy" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    SortExpression="InitializedBy" HeaderText="Angelegt von" HeaderStyle-ForeColor="Black">
                                                                    <HeaderStyle HorizontalAlign="Center" ForeColor="Black"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="InitializedWhen" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    SortExpression="InitializedWhen" HeaderText="Angelegt am" HeaderStyle-ForeColor="Black">
                                                                    <HeaderStyle HorizontalAlign="Center" ForeColor="Black"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OrganizationName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    SortExpression="OrganizationName" HeaderText="Organisation" HeaderStyle-ForeColor="Black">
                                                                    <HeaderStyle HorizontalAlign="Center" ForeColor="Black"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CustomerReference" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-HorizontalAlign="Center" SortExpression="CustomerReference" HeaderText="Kunden-Referenz"
                                                                    HeaderStyle-ForeColor="Black">
                                                                    <HeaderStyle HorizontalAlign="Center" ForeColor="Black"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ProcessReference" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    SortExpression="ProcessReference" HeaderText="Prozess-Referenz" HeaderStyle-ForeColor="Black">
                                                                    <HeaderStyle HorizontalAlign="Center" ForeColor="Black"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:TemplateField SortExpression="TestUser" HeaderStyle-ForeColor="#000000" HeaderStyle-CssClass="TableLinkHead"
                                                                    HeaderStyle-Wrap="false" HeaderText="TestUser" ItemStyle-VerticalAlign="Middle"
                                                                    HeaderStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.TestUser") %>'>
                                                                        </asp:CheckBox>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:ButtonField HeaderStyle-ForeColor="#000000" HeaderStyle-HorizontalAlign="Center"
                                                                    ItemStyle-CssClass="TableLink" CommandName="Del" HeaderText="Löschen" HeaderStyle-CssClass="TableLinkHead"
                                                                    ItemStyle-ForeColor="#595959" ButtonType="Image" ImageUrl="../../Images/del.png"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="TableLinkHead" ForeColor="Black">
                                                                    </HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" CssClass="TableLink" ForeColor="#595959"></ItemStyle>
                                                                </asp:ButtonField>
                                                            </Columns>
                                                            <SelectedRowStyle BackColor="#F2E6AE" />
                                                            <HeaderStyle CssClass="Tablehead" ForeColor="White" />
                                                            <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table id="Tablex1" class="" runat="server" style="border-color: #ffffff" cellspacing="0"
                                            cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td>
                                                    <table id="Table2" cellspacing="0" style="border-color: #ffffff" runat="server" cellpadding="0"
                                                        border="0">
                                                        <tr id="trEditUser" runat="server">
                                                            <td align="left" width="33%" valign="top">
                                                                <table width="33%" id="tblLeft" border="0" style="border-color: #FFFFFF">
                                                                    <tr>
                                                                        <td class="InfoBoxFlat" align="left" colspan="2">
                                                                            <table id="tblGroupDaten" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                                                width="100%" border="0">
                                                                                <tr>
                                                                                    <td width="200" height="22" class="firstLeft active">
                                                                                        Anwendung:
                                                                                    </td>
                                                                                    <td align="left" width="160" height="22" class="cellBorderGray active">
                                                                                        <asp:TextBox ID="txtAppFriendlyName" runat="server" Width="160px" Height="14px" CssClass="InputTextbox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td width="200" height="22" class="firstLeft active">
                                                                                        Angelegt von:
                                                                                    </td>
                                                                                    <td align="left" width="160" height="22" class="cellBorderGray active">
                                                                                        <asp:TextBox ID="txtInitializedBy" runat="server" Width="160px" Height="14px" CssClass="InputTextbox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td width="200" height="22" class="firstLeft active">
                                                                                        Angelegt am:
                                                                                    </td>
                                                                                    <td align="left" width="160" height="22" class="cellBorderGray active">
                                                                                        <asp:TextBox ID="txtInitializedWhen" runat="server" Width="160px" Height="14px" CssClass="InputTextbox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td width="200" height="22px" class="firstLeft active">
                                                                                        <asp:TextBox ID="txtAuthorizationID" runat="server" Visible="False" Width="16px"
                                                                                            Height="0px" BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                                    </td>
                                                                                    <td align="right" width="160" height="22px" class="cellBorderGray active">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td valign="top" align="center" width="50%">
                                                                <table id="tblRight" style="border-color: #FFFFFF" cellspacing="2" cellpadding="0"
                                                                    bgcolor="white" border="0">
                                                                    <tr id="trPwdRules" runat="server">
                                                                        <td class="InfoBoxFlat" align="left" colspan="2">
                                                                            <table id="tblPwdRules" style="border-color: #FFFFFF" cellspacing="2" cellpadding="0"
                                                                                width="100%" border="0">
                                                                                <tr>
                                                                                    <td height="22" class="firstLeft active">
                                                                                        Organisation:
                                                                                    </td>
                                                                                    <td align="left" height="22" class="cellBorderGray active">
                                                                                        <asp:TextBox ID="txtOrganizationName" runat="server" Width="160px" Height="14px"
                                                                                            CssClass="InputTextbox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22" class="firstLeft active">
                                                                                        Kundenreferenz:
                                                                                    </td>
                                                                                    <td align="left" height="22" class="cellBorderGray active">
                                                                                        <asp:TextBox ID="txtCustomerReference" runat="server" Width="160px" Height="14px"
                                                                                            CssClass="InputTextbox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22" class="firstLeft active">
                                                                                        Prozessreferenz:
                                                                                    </td>
                                                                                    <td align="left" height="22" class="cellBorderGray active">
                                                                                        <asp:TextBox ID="txtProcessReference" runat="server" Width="160px" Height="14px"
                                                                                            CssClass="InputTextbox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="right" class="cellBorderGray active">
                                                    &nbsp;&nbsp;<asp:LinkButton class="Tablebutton" ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                                        CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;&nbsp;&nbsp;<asp:LinkButton
                                                            class="Tablebutton" ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                                            CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <%--</form>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
