<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report09.aspx.vb" Inherits="AppF2.Report09" MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="BusyBoxDotNet" Namespace="BusyBoxDotNet" TagPrefix="busyboxdotnet" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="paginationQuery" style="width: 100%; display: block;">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" cellpadding="0" cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td nowrap="nowrap" width="100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:LinkButton ID="btnConfirm" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                                    Width="130px">» Abfrage starten</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="DivPlaceholder" runat="server" style="height: 550px;">
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lbCreateExcel" runat="server" Text="Excel herunterladen" ForeColor="White"></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="20" CssClass="gridview">
                                                    <PagerSettings Visible="false" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Händlernummer" SortExpression="Händlernummer" HeaderText="Händlernummer"
                                                            ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Kontingent temp Versand" SortExpression="Kontingent temp Versand" HeaderText="Kontingent temp. Versand"
                                                            ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField" DataFormatString="{0:N0}">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Inanspruchnahme" SortExpression="Inanspruchnahme"
                                                            HeaderText="Inanspruchnahme" ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField" DataFormatString="{0:N0}">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Kontingent ist gesperrt" SortExpression="Kontingent ist gesperrt" HeaderText="Kontingent ist gesperrt"
                                                            ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        
                                                    </Columns>
                                                    <HeaderStyle CssClass="Tablehead" ForeColor="White" />
                                                    <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lbCreateExcel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
        </div>
    </div>
</asp:Content>