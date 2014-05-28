<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Grid.aspx.vb" Inherits="CKG.Services.Vorlage2" MasterPageFile="../MasterPage/Services.Master"%>
<%@ Register TagPrefix="uc1" TagName="menue" Src="../Start/MainMenu.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <body>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton runat="server" ID="lb_zurueck" Text="zurück"></asp:LinkButton></div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <div style="margin-top: 30px" >
                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True">No Data Label</asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </div>

                        <div id="Result" runat="Server" visible="false">
                            <div class="ExcelDiv">
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td nowrap="nowrap">
                                            <div id="CSVDownload" runat="Server">
                                                <img src="../Images/iconXLS.gif" alt="CSV herunterladen" />
                                                <span class="ExcelSpan">
                                                    <asp:HyperLink ID="lnkShowCSV" runat="server" ForeColor="White" Target="_blank">CSV-Datei</asp:HyperLink>&nbsp;
                                                    <asp:Label ID="lblDownloadTip" runat="server">rechte Maustaste => Ziel speichern unter...</asp:Label></span>
                                            </div>
                                        </td>
                                        <td nowrap="nowrap">
                                            <div align="right">
                                                <img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                                                <span class="ExcelSpan">
                                                    <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False" ForeColor="White">Excel herunterladen</asp:LinkButton>
                                                </span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        
                        <div id="pagination">
                            <uc2:GridNavigation id="GridNavigation1" runat="server" ></uc2:GridNavigation>
                        </div>
                        <div id="data">
                     
                            <table cellspacing="0" width="100%" cellpadding="0"  bgcolor="white" border="0">
                                <tr>
                                    <td>

                                        <asp:GridView ID="GridView1" Width="100%" runat="server" AllowSorting="True" AutoGenerateColumns="True"
                                            CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                            PageSize="50" EditRowStyle-Wrap="False" PagerStyle-Wrap="True" CssClass="GridView">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EditRowStyle Wrap="False"></EditRowStyle>
                                        </asp:GridView>

                                    </td>
                                </tr>
                                <tr >
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
                     
                                            <asp:HyperLink ID="lnkButton" Width="78px" Height="16px" runat="server" CssClass="Tablebutton"
                                            NavigateUrl="javascript:history.back()">Button&nbsp;»</asp:HyperLink>

                     
                     </div>
                        </div>
                 
                    </div>
                   
                </div>
            </div>
        </div>

        <script type="text/javascript">

<!--

            function changein(object) {

                object.cells[0].className = "bgstyleleft";

                count = object.cells.length;

                for (i = 1; i <= count - 1; i++) {

                    object.cells[i].className = "bgstyle";

                }

                object.cells[count - 1].className = "bgstyleright";

            }

            function changeout(object) {

                object.cells[0].className = "";

                count = object.cells.length;

                for (i = 1; i <= count - 1; i++) {

                    object.cells[i].className = "";

                }

                object.cells[count - 1].className = "";

            }

//-->

        </script>

    </body>
</asp:Content>
