<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ForbiddenUserNameManagement.aspx.vb" Inherits="Admin.ForbiddenUserNameManagement"  MasterPageFile="MasterPage/Admin.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkUserManagement" runat="server" ToolTip="Benutzer" NavigateUrl="UserManagement.aspx"
                            Text="Benutzer | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkCustomerManagement" ToolTip="Kunden" CssClass="IMGCust" runat="server"
                            NavigateUrl="CustomerManagement.aspx" Text="Kunden | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkArchivManagement" ToolTip="Archive" runat="server" CssClass="IMGArchiv"
                            NavigateUrl="ArchivManagement.aspx" Text="Archive | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkAppManagement" runat="server" ToolTip="Anwendungen" CssClass="IMGApp"
                            NavigateUrl="AppManagement.aspx" Text="Anwendungen"></asp:HyperLink>
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                      <asp:Panel ID="DivSearch1" runat="server" DefaultButton="btnEmpty">
                            <div id="TableQuery">
                                <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                &nbsp;
                                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trSelectOrganization" runat="server">
                                            <td class="firstLeft active">
                                                Benutzername:
                                            </td>
                                            <td class="firstLeft active" width="100%">
                                                <asp:TextBox ID="txtFilterForbiddenUserNameName" runat="server" CssClass="InputTextbox" Width="257px">*</asp:TextBox>
                                                <asp:Label ID="lblForbiddenUserNameName" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtEmpty" runat="server" CssClass="InputTextbox" Width="160px" Visible="False">*</asp:TextBox>
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../images/empty.gif"
                                                    Width="1px" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                            </td>
                                            <td style="width: 35%">
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td align="right" nowrap="nowrap" class="rightPadding">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr style="background-color: #dfdfdf; height: 22px">
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div id="QueryFooter" runat="server">
                                    <div id="dataQueryFooter">
                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton class="Tablebutton" ID="btnSuche" runat="server"
                                            Text="&amp;nbsp;&amp;#187; Suchen" CssClass="TablebuttonXLarge" Height="16px"
                                            Width="155px" Font-Names="Verdana,sans-serif" Font-Size="10px"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="&amp;nbsp;&amp;#187; Neuer Eintrag"
                                            CssClass="TablebuttonXLarge" Height="16px" Width="155px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="Result" runat="Server" visible="false">
                            <div class="ExcelDiv">
                                <div align="right" id="trSearchSpacer1" runat="server">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <table cellspacing="0" style="border-width: 0px; border-color: #ffffff;" 
                                    cellpadding="0" width="100%"
                                    align="left">
                                    <tbody>
                                        <tr id="trSearchResult" runat="server">
                                            <td align="left">
                                                <asp:GridView ID="dgSearchResult" Width="100%" runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowPaging="True" GridLines="None" PageSize="20" EditRowStyle-Wrap="False" PagerStyle-Wrap="True"
                                                    CssClass="GridView">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Benutzername">
     														<ItemTemplate>
															<asp:LinkButton id="btnSelect" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UserName") %>'  CommandName="Select" ></asp:LinkButton>
														</ItemTemplate>
                                                        </asp:TemplateField>                                                            
                                                        <asp:TemplateField HeaderText="l&#246;schen">
     														<ItemTemplate>
															<asp:ImageButton id="ibtnSRDelete" runat="server" CausesValidation="false"  CommandName="Del" ImageUrl="../Images/Papierkorb_01.gif" Width="16px" Height="16px"></asp:ImageButton>
														</ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="dataFooter">
                                &nbsp;
                            </div>
                        </div>
                        <div id="Input" runat="server" visible="False">
                            <div id="adminInput">
                                <table id="Tablex1" runat="server" cellspacing="0" cellpadding="0" width="100%"
                                    border="0">
                                    <tr>
                                        <td width="100%">
                                            <table style="border-color: #ffffff" width="100%">
                                                <tr id="trEditUser" runat="server">
                                                    <td align="left" width="50%" valign="top">
                                                        <table id="tblLeft" style="border-color: #ffffff; padding-right: 50px;" cellspacing="0"
                                                            cellpadding="0">
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <table id="tblGroupDaten" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                                        width="100%" border="0">
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Benutzername:
                                                                            </td>
                                                                            <td class="active" style="width:100%">
                                                                                <asp:TextBox ID="txtForbiddenUserNameName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                                <asp:textbox id="txtID" runat="server" Visible="False" Width="10px" Height="10px" BorderWidth="0px" BorderStyle="None" >-1</asp:textbox>
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
                                </table>
                                <div style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                                <div id="dataFooter">
                                    &nbsp;&nbsp;<asp:LinkButton class="Tablebutton" ID="lbtnDelete" runat="server" Text="&amp;nbsp;&amp;#187; Löschen"
                                        CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;<asp:LinkButton
                                            class="Tablebutton" ID="lbtnConfirm" runat="server" Text="&amp;nbsp;&amp;#187; Bestätigen"
                                            CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;<asp:LinkButton
                                                class="Tablebutton" ID="lbtnSave" runat="server" Text="&amp;nbsp;&amp;#187; Speichern"
                                                CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;<asp:LinkButton
                                                    class="Tablebutton" ID="lbtnCancel" runat="server" Text="&amp;nbsp;&amp;#187; Verwerfen"
                                                    CssClass="Tablebutton" Height="16px" Width="78px" 
                                        Visible="False"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>