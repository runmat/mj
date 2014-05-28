<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01s_2.aspx.vb" Inherits="CKG.Components.ComCommon.Beauftragung2.Report01s_2"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register TagPrefix="uc1" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
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
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        <div>
                            <asp:HyperLink ID="lnkShowCSV" runat="server" ForeColor="White" Target="_blank">CSV-Datei</asp:HyperLink>
                            <asp:Label ID="lblDownloadTip" runat="server">&nbsp;rechte Maustaste => Ziel speichern unter...</asp:Label>
                            <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False"><img alt="Excel"   src="../../../Images/iconXLS.gif"  />&nbsp;Excel herunterladen</asp:LinkButton>
                        </div>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div id="pagination">
                            <uc1:GridNavigation ID="GridNavigation1" runat="server"></uc1:GridNavigation>
                        </div>
                        <div id="data">
                            <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNoData" runat="server" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridView1" Width="2000px" runat="server" AllowSorting="True" AutoGenerateColumns="True"
                                            CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                            PageSize="20" EditRowStyle-Wrap="False" PagerStyle-Wrap="True" CssClass="GridView">
                                            <PagerSettings Visible="False" />
                                            <EmptyDataRowStyle Wrap="False" />
                                            <FooterStyle Wrap="False" />
                                            <PagerStyle Wrap="True"></PagerStyle>
                                            <SelectedRowStyle Wrap="False" />
                                            <HeaderStyle CssClass="GridTableHead" ForeColor="White" Wrap="False"></HeaderStyle>
                                            <AlternatingRowStyle Wrap="false" CssClass="GridTableAlternate" />
                                            <RowStyle Wrap="false" CssClass="ItemStyle" />
                                            <EditRowStyle Wrap="False"></EditRowStyle>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblInfo" runat="server" EnableViewState="False" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHidden" runat="server" Visible="False" Width="39px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="dataFooter">
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
