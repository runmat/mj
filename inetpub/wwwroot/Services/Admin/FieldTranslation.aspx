<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FieldTranslation.aspx.vb"
    Inherits="Admin.FieldTranslation" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>&nbsp;
                                <asp:Label ID="lblAppURL" runat="server"></asp:Label>&nbsp;-
                                <asp:Label ID="lblKundeSprache" runat="server"></asp:Label></h1>
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
                                                Sprache:
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:DropDownList ID="ddlLanguage" runat="server" Width="275px">
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
                                                <asp:Label ID="lblAppID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ApplicationFieldID") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFieldType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FieldType") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FieldName") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Field" SortExpression="Field" HeaderText="Seitenelement">
                                        </asp:BoundField>
                                        <asp:TemplateField SortExpression="Visibility" HeaderText="Sichtbar">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Visibility") %>'
                                                    Enabled="False"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Content" SortExpression="Content" HeaderText="&#220;bersetzung">
                                        </asp:BoundField>
                                        <asp:ButtonField Text="Anlegen" CausesValidation="False" CommandName="Create" />
                                        <asp:ButtonField Text="Ändern" CausesValidation="False" CommandName="Edit" />
                                        <asp:ButtonField CommandName="Del" ItemStyle-HorizontalAlign="Center" HeaderText="Löschen"
                                            ButtonType="Image" ImageUrl="../../Images/del.png" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div id="adminInput">
                            <table id="Tablex1" class="" runat="server" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tbody>
                                    <tr id="trEditUser" runat="server">
                                        <td align="left">
                                            <div style="background-color: #dfdfdf; height: 22px; padding-left: 15px; padding-top: 7px;">
                                            </div>
                                            <table id="tblLeft" style="border-color: #ffffff" cellspacing="0" cellpadding="0"
                                                width="100%" bgcolor="white" border="0">
                                                <tr class="formquery">
                                                    <td height="22" class="firstLeft active">
                                                        Feld:
                                                    </td>
                                                    <td align="left" height="22" class="active" width="100%">
                                                        <asp:Label ID="lblField" runat="server"></asp:Label><asp:Label ID="lblFieldID" runat="server"
                                                            Visible="False"></asp:Label><asp:Label ID="lblFieldIDSave" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td height="22" class="firstLeft active">
                                                        Feldtyp:
                                                    </td>
                                                    <td align="left" height="22" nowrap="nowrap" class="active" width="100%">
                                                        <span>
                                                            <asp:RadioButton ID="rbLabel" runat="server" GroupName="grpFieldType" Checked="True"
                                                                Text="Label" AutoPostBack="True"></asp:RadioButton></span>&nbsp;&nbsp; <span>
                                                                    <asp:RadioButton ID="rbLinkButton" runat="server" Text="LinkButton" GroupName="grpFieldType"
                                                                        AutoPostBack="True"></asp:RadioButton></span>&nbsp;&nbsp; <span>
                                                                            <asp:RadioButton ID="rbRadioButton" runat="server" Text="RadioButton" GroupName="grpFieldType"
                                                                                AutoPostBack="True"></asp:RadioButton></span>&nbsp;&nbsp;
                                                        <span>
                                                            <asp:RadioButton ID="rbTableRow" runat="server" Text="Tabellenzeile" GroupName="grpFieldType"
                                                                AutoPostBack="True"></asp:RadioButton></span>&nbsp;&nbsp; <span>
                                                                    <asp:RadioButton ID="rbGridColumn" runat="server" Text="Grid-Spalte" GroupName="grpFieldType"
                                                                        AutoPostBack="True"></asp:RadioButton></span>&nbsp;&nbsp; <span>
                                                                            <asp:RadioButton ID="rbTextBox" runat="server" Text="TextBox" GroupName="grpFieldType"
                                                                                AutoPostBack="True"></asp:RadioButton></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td height="22" class="firstLeft active">
                                                        Feldname:
                                                    </td>
                                                    <td align="left" height="22" class="active" width="100%">
                                                        <asp:TextBox ID="txtFieldName" runat="server" Width="350px" MaxLength="50" Height="20px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td height="22" class="firstLeft active">
                                                        Sichtbar:
                                                    </td>
                                                    <td align="left" height="22" class="active" width="100%">
                                                        <asp:CheckBox ID="cbxVisible" runat="server"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td height="22" class="firstLeft active">
                                                        Übersetzung:
                                                    </td>
                                                    <td align="left" height="22" class="active" width="100%">
                                                        <asp:TextBox ID="txtContent" runat="server" Width="350px" MaxLength="50" Height="20px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td height="22" class="firstLeft active">
                                                        <asp:Label ID="lbl_TextTooltip" runat="server" Visible="False">ToolTip:</asp:Label>
                                                    </td>
                                                    <td align="left" height="22" class="active" width="100%">
                                                        <asp:TextBox ID="txt_Tooltip" Visible="False" runat="server" Width="350px" MaxLength="50"
                                                            Height="20px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trStandard" runat="server">
                                                    <td height="22" class="firstLeft active">
                                                        Standard:
                                                    </td>
                                                    <td align="left" height="22" class="cellBorderGray active" width="100%">
                                                        <asp:Label ID="lblStandard" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="dataFooter">
                            &nbsp;&nbsp;<asp:LinkButton class="Tablebutton" ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                            &nbsp;<asp:LinkButton class="Tablebutton" ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                            &nbsp;<asp:LinkButton class="Tablebutton" ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
