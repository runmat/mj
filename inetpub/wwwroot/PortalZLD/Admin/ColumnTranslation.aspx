<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ColumnTranslation.aspx.vb"
    Inherits="Admin.ColumnTranslation" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <div id="navigationSubmenu">
                         <asp:HyperLink ID="lnkUserManagement" runat="server" ToolTip="Benutzer" NavigateUrl="UserManagement.aspx"
                            Text="Benutzer | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkGroupManagement" runat="server" ToolTip="Gruppen" NavigateUrl="GroupManagement.aspx"
                            Text="Gruppen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkCustomerManagement" ToolTip="Kunden" runat="server" NavigateUrl="CustomerManagement.aspx"
                            Text="Kunden | "></asp:HyperLink>
                                                    <asp:HyperLink ID="lnkAppManagement" runat="server" ToolTip="Anwendungen" NavigateUrl="AppManagement.aspx"
                            Text="Anwendungen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkArchivManagement" ToolTip="Archive" runat="server" NavigateUrl="ArchivManagement.aspx"
                            Text="Archive | "></asp:HyperLink>
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>&nbsp;
                                <asp:Label ID="lblAppName" runat="server"></asp:Label>&nbsp;-
                                <asp:Label ID="lblAppFriendlyName" runat="server"></asp:Label></h1>
                        </div>
                        <div id="DivSearch" runat="server">
                            <div id="TableQuery">
                                <table id="table1" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                &nbsp;
                                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>

                            </div>
                                 <div id="QueryFooter" runat="server">
                                    <div id="dataQueryFooter">
                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton class="Tablebutton" ID="lnkBack" runat="server"
                                            Text="zurück&amp;nbsp;&amp;#187; " CssClass="TablebuttonXLarge" Height="16px"
                                            Width="155px" Font-Names="Verdana,sans-serif" Font-Size="10px"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="Neue Übersetzung&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="16px" Width="155px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                    </div>
                                </div>                            
                        </div>
                        <div id="Result" runat="Server">
                           <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                        <div id="data">

                                    <asp:GridView ID="dgSearchResult" Width="100%" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                    <PagerSettings Visible="False" />
                                    <RowStyle CssClass="ItemStyle" />
                                    <EmptyDataRowStyle BackColor="#DFDFDF" />
                                                <Columns>
                                                    <asp:TemplateField Visible="False" HeaderText="AppID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAppID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppID") %>'>
                                                            </asp:Label>
                                                            <asp:Label ID="lbOrgname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrgName") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ButtonField DataTextField="OrgName" SortExpression="OrgName" CommandName="Edit"
                                                        HeaderText="SAP-Name" />
                                                    <asp:BoundField DataField="OrgName" SortExpression="OrgName" HeaderText="SAP-Name">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NewName" SortExpression="NewName" HeaderText="&#220;bersetzung">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DisplayOrder" SortExpression="DisplayOrder" HeaderText="Reihenfolge Nr">
                                                    </asp:BoundField>
                                                    <asp:TemplateField SortExpression="NULLENENTFERNEN" HeaderText="Nullen entfernen">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.NULLENENTFERNEN") %>'
                                                                Enabled="False"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="TEXTBEREINIGEN" HeaderStyle-Wrap="True" HeaderText="Text bereinigen">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.TEXTBEREINIGEN") %>'
                                                                Enabled="False"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ISTDATUM" HeaderStyle-Wrap="True" HeaderText="ist Datum">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox3" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ISTDATUM") %>'
                                                                Enabled="False"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ISTZEIT" HeaderStyle-Wrap="True" HeaderText="ist Zeit">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox5" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ISTZEIT") %>'
                                                                Enabled="False"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ABEDaten" HeaderText="ABE-Daten">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="Checkbox4" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ABEDaten") %>'
                                                                Enabled="False"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ButtonField CommandName="Del" HeaderText="Löschen" ButtonType="Image" ImageUrl="/PortalZLD/Images/del.png" />
                                                </Columns>
                                            </asp:GridView>
                                    </div>
                        </div>

                        <div id="adminInput">
                            <table id="Tablex1" class="" runat="server" style="border-color: #ffffff" cellspacing="0"
                                cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table id="Table2" cellspacing="0" style="border-color: #ffffff" runat="server" cellpadding="0"
                                            width="120" border="0">
                                            <tbody>
                                                <tr id="trEditUser" runat="server">
                                                    <td align="left">
                                                        <table width="740" style="border-color: #ffffff" border="0">
                                                            <tr>
                                                                <td valign="top" align="left">
                                                                    <table id="tblLeft" style="border-color: #ffffff" cellspacing="2" cellpadding="0"
                                                                        width="345" bgcolor="white" border="0">
                                                                        <tr>
                                                                            <td height="22" class="firstLeft active">
                                                                                SAP-Name:
                                                                            </td>
                                                                            <td height="22" class="cellBorderGray active">
                                                                                <asp:TextBox ID="txtAppID" runat="server" Visible="False" BorderStyle="None" BorderWidth="0px"
                                                                                    Width="0px" BackColor="#CEDBDE" ForeColor="#CEDBDE" Height="0px">-1</asp:TextBox><asp:TextBox
                                                                                        ID="txtOrgNameAlt" runat="server" Visible="False" BorderStyle="None" BorderWidth="0px"
                                                                                        Width="0px" BackColor="#CEDBDE" ForeColor="#CEDBDE" Height="0px">-1</asp:TextBox>
                                                                                <asp:TextBox ID="txtOrgNameNeu" runat="server" Width="160px" Height="14px" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trAnrede" runat="server">
                                                                            <td height="22" class="firstLeft active">
                                                                                Übersetzung:
                                                                            </td>
                                                                            <td height="22" class="cellBorderGray active">
                                                                                <asp:TextBox ID="txtNewName" runat="server" Width="160px" Height="14px" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trVorname" runat="server">
                                                                            <td height="22" class="firstLeft active">
                                                                                Reihenfolge Nr.:
                                                                            </td>
                                                                            <td height="22" class="cellBorderGray active">
                                                                                <asp:TextBox ID="txtDisplayOrder" runat="server" Width="160px" Height="14px" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td height="22" class="firstLeft active">
                                                                                Ausrichtung:
                                                                            </td>
                                                                            <td height="22" class="cellBorderGray active">
                                                                                <asp:DropDownList ID="ddlAlignment" runat="server" Width="160px" AutoPostBack="True"
                                                                                    Height="20px" CssClass="DropDowns">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td height="22" class="firstLeft active">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td height="22" class="cellBorderGray active">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td valign="top" align="right" width="50%">
                                                                    <table id="tblRight" cellspacing="2" cellpadding="0" width="345" border="0" bgcolor="white"
                                                                        style="border-color: #FFFFFF">
                                                                        <tr>
                                                                            <td valign="top" width="185" height="22" class="cellBorderGray active">
                                                                                Nullen entfernen:
                                                                            </td>
                                                                            <td align="left" height="22" class="cellBorderGray active">
                                                                                <asp:CheckBox ID="cbxNullenEntfernen" runat="server"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" height="22" class="cellBorderGray active">
                                                                                Text bereinigen:
                                                                            </td>
                                                                            <td align="left" height="22" class="cellBorderGray active">
                                                                                <asp:CheckBox ID="cbxTextBereinigen" runat="server"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td height="22" class="cellBorderGray active">
                                                                                ist Datum:
                                                                            </td>
                                                                            <td align="left" height="22" class="cellBorderGray active">
                                                                                <asp:CheckBox ID="cbxIstDatum" runat="server"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td height="22" class="cellBorderGray active">
                                                                                ist Zeit:
                                                                            </td>
                                                                            <td align="left" height="22" class="cellBorderGray active">
                                                                                <asp:CheckBox ID="cbxIstZeit" runat="server"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td height="22" class="cellBorderGray active">
                                                                                ABE-Daten:
                                                                            </td>
                                                                            <td align="left" height="22" class="cellBorderGray active">
                                                                                <asp:CheckBox ID="cbxABEDaten" runat="server"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="left" class="cellBorderGray active">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="top" align="right" class="cellBorderGray active">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="left" class="cellBorderGray active">
                                                                </td>
                                                                <td valign="top" align="right" class="cellBorderGray active">
                                                                    &nbsp;&nbsp;<asp:LinkButton class="Tablebutton" ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                                                    &nbsp;<asp:LinkButton class="Tablebutton" ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                                                    &nbsp;<asp:LinkButton class="Tablebutton" ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" height="25">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
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
