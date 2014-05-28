<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report06s.aspx.vb" Inherits="CKG.Components.ComCommon.Report06s"
    MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                                Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="/Services/Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                                <td style="width: 100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:RadioButton ID="rdbAlle" runat="server" AutoPostBack="True" Checked="True" Font-Bold="True"
                                                        Text="Alle" />
                                                </td>
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:RadioButton ID="rdbAnwendung" runat="server" AutoPostBack="True" Font-Bold="True"
                                                        Text="Anwendung" />
                                                </td>
                                                <td class="firstLeft active">
                                                    <span>
                                                        <asp:DropDownList ID="ddlAnwendung" runat="server" Enabled="False">
                                                        </asp:DropDownList>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:RadioButton ID="rdbGruppe" runat="server" AutoPostBack="True" Font-Bold="True"
                                                        Text="Gruppe" />
                                                </td>
                                                <td class="firstLeft active">
                                                   <span>
                                                    <asp:DropDownList ID="ddlGruppe" runat="server" Enabled="False">
                                                    </asp:DropDownList>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>
                            </asp:Panel>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen </asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="/Services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>&nbsp;</div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvBestand" Width="100%" runat="server" CellPadding="1" CellSpacing="1"
                                                    GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0" AllowSorting="True"
                                                    AllowPaging="True" PageSize="20" AutoGenerateColumns="False">
                                                    <HeaderStyle BackColor="#9b9b9b" ForeColor="White" Height="30px" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                                                        <asp:BoundField DataField="Firstname" HeaderText="Vorname" SortExpression="Firstname" />
                                                        <asp:BoundField DataField="LastName" HeaderText="Nachname" SortExpression="LastName" />
                                                        <asp:TemplateField HeaderText="Gruppe" SortExpression="GroupName">
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("GroupName") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" class="style3">
                                                                    <tr>
                                                                        <td width="100%">
                                                                            <asp:Label ID="lblGroup" runat="server" Text='<%# Bind("GroupName") %>'></asp:Label>
                                                                            <asp:Label ID="lblGroupID" runat="server" Text='<%# Bind("GroupID") %>' Visible="False"></asp:Label>
                                                                        </td>
                                                                        <td style="text-align: right">
                                                                            <asp:ImageButton ID="btnAnwendung" runat="server" OnClick="Button1_Click" ImageUrl="/services/images/Lupe_16x16.gif"
                                                                                CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' Height="16px"
                                                                                Width="16px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="LastLogin" DataFormatString="{0:d}" HeaderText="Datum letzte Anmeldung"
                                                            SortExpression="LastLogin">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LastPwdChange" HeaderText="Datum letzte Passwortänderung"
                                                            SortExpression="LastPwdChange" DataFormatString="{0:d}">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Gesperrt" SortExpression="AccountIsLockedOut">
                                                            <EditItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("AccountIsLockedOut") %>' />
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("AccountIsLockedOut") %>'
                                                                    Enabled="false" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="dataFooter">
                                &nbsp;</div>
                    <div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                                <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                                    PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnOK">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="mb" runat="server" BackColor="White" Style="display: none">
                                    <div style="padding: 20px 20px 20px 20px">
                                        <div style="text-align: center;">
                                            &nbsp;&nbsp;
                                            <asp:Label ID="lblGruppe" Text="Anwendungen der Gruppe " runat="server" Font-Bold="True"></asp:Label>
                                        </div>
                                        <br />
                                        <asp:GridView ID="grvAnwendungen" runat="server" CellPadding="1" CellSpacing="1"
                                            GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0" AllowSorting="True"
                                            AllowPaging="True" PageSize="20" AutoGenerateColumns="False">
                                            <HeaderStyle BackColor="#9b9b9b" ForeColor="White" Height="30px" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:BoundField DataField="AppFriendlyName" HeaderText="Zugeordnete Anwendungen">
                                                    <HeaderStyle Font-Bold="True" />
                                                    <ItemStyle ForeColor="Blue" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div style="text-align: center; padding-bottom: 10px">
                                        <asp:Button ID="btnOK" runat="server" CssClass="TablebuttonLarge" Font-Bold="true"
                                            Text="Schließen" />
                                    </div>
                                </asp:Panel>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
            </div>
        </div>
    </div>
</asp:Content>
