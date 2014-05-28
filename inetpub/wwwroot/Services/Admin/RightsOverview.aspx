<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RightsOverview.aspx.vb"
    Inherits="Admin.RightsOverview" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<script src="../JScript/Jquery/jquery.js" type="text/javascript"></script>--%>
    <script src="../JScript/Jquery/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../JScript/Jquery/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../JScript/Jquery/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../JScript/Jquery/jquery.ui.mouse.js" type="text/javascript"></script>
    <script src="../JScript/Jquery/jquery.ui.resizable.js" type="text/javascript"></script>
    <%--<script src="../JScript/Jquery/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>--%>
    <script src="../JScript/Jquery/colResizable-1.3.min.js" type="text/javascript"></script>
    <link href="../styles/colResizable.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {

            var onResized = function (e) {

            };

            $("#<%  = RightsOverview.ClientID %>").colResizable({
                liveDrag: true,
                gripInnerHtml: "<div class='ItemStyle'></div>",
                draggingClass: "dragging",
                onResize: onResized
            });

            //$("#resizeBox").resizable();

        });	
    </script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="Result" runat="Server">
                        <asp:Panel ID="panelRam" runat="server">
                            <div class="ExcelDiv">
                                <div align="right" class="rightPadding">
                                    <img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                                    <span class="ExcelSpan">
                                        <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div><br />
                            <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <%--<div id="resizeBox">--%>
                                            <asp:GridView ID="RightsOverview" Width="100%" runat="server" AutoGenerateColumns="True"
                                                CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                                <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                <PagerSettings Visible="False" />
                                                <RowStyle CssClass="ItemStyle" Font-Size="8px" />
                                            </asp:GridView>
                                        <%--</div>--%>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
