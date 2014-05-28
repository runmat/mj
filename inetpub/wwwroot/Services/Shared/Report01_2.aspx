<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01_2.aspx.vb" Inherits="CKG.Services.Report01_2"
    MasterPageFile="../MasterPage/Services.Master" %>

<%@ Register TagPrefix="uc1" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:HyperLink runat="server" NavigateUrl="javascript:history.back()">zurück</asp:HyperLink>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        <div>
                            <asp:HyperLink ID="lnkShowCSV" runat="server" ForeColor="White" Target="_blank">CSV-Datei</asp:HyperLink>
                            <asp:Label ID="lblDownloadTip" runat="server">&nbsp;rechte Maustaste => Ziel speichern unter...</asp:Label>
                            <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False"><img alt="Excel"   src="../Images/iconXLS.gif"  />&nbsp;Excel herunterladen</asp:LinkButton>
                        </div>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div id="pagination">
                            <uc1:GridNavigation ID="GridNavigation1" runat="server" />
                        </div>
                        <div id="data">
                            <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="lblError" runat="server" Visible="False" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridView1" Width="100%" runat="server" AllowSorting="True" AutoGenerateColumns="True"
                                            CellPadding="0" AllowPaging="True" GridLines="None"
                                            PageSize="20" CssClass="GridView">
                                            <PagerSettings Visible="False" />
                                            <PagerStyle Wrap="True" />
                                            <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate" BackColor="#DEE1E0" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EditRowStyle Wrap="False" />
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
                                <tr id="ShowScript" runat="server" visible="False">
                                    <td>
                                        <script type="text/javascript">
                 
						                <!--                                            //
                                            function FreigebenConfirm(Fahrgest, Vertrag, BriefNr, Kennzeichen) {
                                                var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
                                                return (Check);
                                            }
						                //-->
                                        </script>
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
</asp:Content>
