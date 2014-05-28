<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Translation_Start.aspx.vb" Inherits="Admin.Translation_Start"  MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <div id="navigationSubmenu">
                        &nbsp;
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <asp:Panel id="DivSearch" runat="server" DefaultButton="btnEmpty">
                            <div id="TableQuery">
                                <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                
                                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Anwendung:
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap" width="100%">
                                                <asp:TextBox ID="txtFilterAppName" runat="server" CssClass="InputTextbox">*</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Freundlicher Name:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterAppFriendlyName" runat="server" 
                                                    CssClass="InputTextbox">*</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Art:</td>
                                            <td class="firstLeft active">
                                               <span> <asp:RadioButton ID="rbField" runat="server" GroupName="Art" 
                                                    Checked="True" Text="Feldübersetztung" />
                                                <asp:RadioButton ID="rbColumn" runat="server" Visible="false" GroupName="Art" 
                                                    Text="Spaltenübersetzung" /></span>
                                                </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtEmpty" runat="server" CssClass="InputTextbox" 
                                                    Visible="False" Width="160px">*</asp:TextBox>
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" 
                                                    ImageUrl="../images/empty.gif" Width="1px" />
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
                                            <td align="right" class="rightPadding" nowrap="nowrap">
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
                                        <asp:LinkButton class="Tablebutton" ID="btnSuche" runat="server"
                                            Text="&amp;nbsp;&amp;#187; Suchen" CssClass="Tablebutton" Height="16px"
                                            Width="78px" Font-Names="Verdana,sans-serif" Font-Size="10px"></asp:LinkButton>
                                       <asp:LinkButton ID="cmdback" runat="server" Text="&amp;nbsp;&amp;#187; zurück"
                                            CssClass="Tablebutton" Height="16px" Width="78px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="Result" runat="Server" visible="false">
                            <div class="ExcelDiv">
                                <div align="right" id="trSearchSpacer" runat="server">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" width="100%"
                                    align="left" border="0">
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
                                                        <asp:BoundField DataField="AppID" SortExpression="AppID" HeaderText="AppID"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Anwendung" SortExpression="AppName">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ForeColor="#595959" Font-Underline="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppName")%>' CommandName="goToFieldTranslation"  ID="btnSelect" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.AppUrl")%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Freundlicher Name" />
                                                        <asp:BoundField DataField="AppType" SortExpression="AppType" HeaderText="Typ" />

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
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>