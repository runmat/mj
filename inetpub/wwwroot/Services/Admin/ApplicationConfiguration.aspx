<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ApplicationConfiguration.aspx.vb"
    Inherits="Admin.ApplicationConfiguration" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
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
                                <asp:Label ID="lblHead" runat="server" Text="Label" />
                                <asp:HiddenField ID="hiddenAppID" runat="server" />
                            </h1>
                        </div>
                        <div id="DivSearch" runat="server">
                            <div id="TableQuery">
                                <table id="table1" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Kunde:
                                            </td>
                                            <td class="active">
                                                <asp:DropDownList ID="ddlCustomer" runat="server" Width="275px" 
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Gruppe:
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:DropDownList ID="ddlGroup" runat="server" Width="275px" 
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                &nbsp;
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
                                    &nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="Neue Einstellung&amp;nbsp;&amp;#187; "
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
                                <asp:GridView ID="dgSearchResult" Width="100%" runat="server" AllowSorting="True" AllowPaging="true"
                                    AutoGenerateColumns="False" CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0"
                                    GridLines="None" PageSize="20" EditRowStyle-Wrap="False" CssClass="GridView">
                                    <PagerSettings Visible="False" />
                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                    <RowStyle CssClass="ItemStyle" />
                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                    <Columns>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblConfigID" runat="server" Text='<%# Eval("ConfigID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ConfigType" HeaderText="Typ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblConfigType" runat="server" Text='<%# Eval("ConfigType") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ConfigKey" HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblConfigKey" runat="server" Text='<%# Eval("ConfigKey") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ConfigValue" HeaderText="Wert">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" Checked='<%# Eval("ConfigValue") IsNot Nothing AndAlso Eval("ConfigValue").ToString().ToUpper() = "TRUE" %>' 
                                                    Visible='<%# Eval("ConfigType") = "bool" %>' Enabled="False" />
                                                <asp:Label runat="server" Text='<%# Eval("ConfigValue") %>' 
                                                    Visible='<%# Eval("ConfigType") = "string" %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Beschreibung" />
                                        <asp:ButtonField Text="Anlegen" CausesValidation="False" CommandName="Create" />
                                        <asp:ButtonField Text="Ändern" CausesValidation="False" CommandName="Edit" />
                                        <asp:ButtonField CommandName="Del" ItemStyle-HorizontalAlign="Center" HeaderText="Löschen"
                                            ButtonType="Image" ImageUrl="../../Images/del.png" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div id="Input" runat="server" Visible="False">
                            <div id="adminInput">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td align="left">
                                                <div style="background-color: #dfdfdf; height: 22px; padding-left: 15px; padding-top: 7px;">
                                                </div>
                                                <table id="tblLeft" style="border-color: #ffffff" cellspacing="0" cellpadding="0"
                                                    width="100%" bgcolor="white" border="0">
                                                    <tr class="formquery">
                                                        <td height="22" class="firstLeft active">
                                                            Einstellung:
                                                        </td>
                                                        <td align="left" height="22" class="active" width="100%">
                                                            <asp:Label ID="lblConfigKey" runat="server" />
                                                            <asp:HiddenField ID="hiddenConfigID" runat="server"/>
                                                            <asp:HiddenField ID="hiddenConfigIDSave" runat="server"/>
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td height="22" class="firstLeft active">
                                                            Typ:
                                                        </td>
                                                        <td align="left" height="22" nowrap="nowrap" class="active" width="100%">
                                                            <span>
                                                                <asp:RadioButton ID="rbBool" runat="server" GroupName="grpConfigType" Text="Ja/Nein" AutoPostBack="True" Checked="True" />
                                                            </span>
                                                            &nbsp;&nbsp;
                                                            <span>
                                                                <asp:RadioButton ID="rbString" runat="server" GroupName="grpConfigType" Text="Text" AutoPostBack="True" />
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td height="22" class="firstLeft active">
                                                            Name:
                                                        </td>
                                                        <td align="left" height="22" class="active" width="100%">
                                                            <asp:TextBox ID="txtConfigKey" runat="server" Width="350px" MaxLength="50" Height="20px" />
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td height="22" class="firstLeft active">
                                                            Wert:
                                                        </td>
                                                        <td align="left" height="22" class="active" width="100%">
                                                            <asp:CheckBox ID="cbxBoolValue" runat="server" />
                                                            <asp:TextBox ID="txtStringValue" runat="server" Width="350px" MaxLength="200" Height="20px" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td height="22" class="firstLeft active">
                                                            Beschreibung:
                                                        </td>
                                                        <td align="left" height="22" class="active" width="100%">
                                                            <asp:TextBox ID="txtDescription" runat="server" Width="350px" MaxLength="500" Height="20px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187;" CssClass="Tablebutton" Height="16px" Width="78px" />
                            &nbsp;
                            <asp:LinkButton ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187;" CssClass="Tablebutton" Height="16px" Width="78px" />
                            &nbsp;
                            <asp:LinkButton ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187;" CssClass="Tablebutton" Height="16px" Width="78px" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
