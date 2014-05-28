<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04.aspx.vb" Inherits="AppServicesCarRent.Change04"
    MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
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
                            <asp:Label ID="lblHead" runat="server" Text=""></asp:Label>
                        </h1>
                    </div>
                    <asp:Panel ID="Panel2" DefaultButton="btndefault" runat="server">
                        <div id="TableQuery">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr id="tr_Message" runat="server" class="formquery">
                                    <td colspan="2" class="firstLeft active" style="width: 100%">
                                        <p>
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Black" Font-Bold="True"></asp:Label>
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label></p>
                                    </td>
                                </tr>
                                <tr class="formquery" id="trupload" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Upload" runat="server">lbl_Upload</asp:Label>
                                    </td>
                                    <td class="active" style="width: 100%">
                                        <input id="upFile1" type="file" size="35" name="File1" runat="server" />
                                        &nbsp; <a href="javascript:openinfo('Info.htm');">
                                            <img src="/Services/Images/info.gif" border="0" height="16px" width="16px" alt="Struktur Uploaddatei"
                                                title="Struktur Uploaddatei Fahrgestllnummern" /></a> &nbsp;
                                    </td>
                                </tr>
                                <tr id="trAktion" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Aktion" runat="server">lbl_Aktion</asp:Label>
                                    </td>
                                    <td class="active" style="width: 100%">
                                        <span>
                                            <asp:RadioButtonList ID="rdo_list" runat="server">
                                                <asp:ListItem Selected="True">Sperren</asp:ListItem>
                                                <asp:ListItem>Ensperren</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </span>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        &nbsp;
                                    </td>
                                    <td class="active" style="width: 100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2">
                                        &nbsp;
                                        <asp:Label ID="lblExcelfile" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery" style="background-color: #dfdfdf; width: 100%">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbCreate" runat="server" CssClass="TablebuttonLarge" Width="130px" height="16px"
                                OnClick="lbCreate_Click" Style="margin-bottom: 0px">» Upload & Senden</asp:LinkButton>
                            <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btndefault" runat="server"
                                Text="Button" />
                        </div>
                    </asp:Panel>
                    <div id="Queryfooter" runat="server">
                        &nbsp;
                    </div>

                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" runat="server">Excel herunterladen</asp:LinkButton>
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
                                    <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                        CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                        Width="100%">
                                        <PagerSettings Visible="False" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="CHASSIS_NUM" HeaderText="Fahrgestellnummer" />
                                            <asp:BoundField DataField="SPERRVERMERK" HeaderText="Sperrvermerk" />
                                            <asp:BoundField DataField="BEM_RETURN" HeaderText="Status" />
                                        </Columns>
                                    </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            </div></div>
        </div>
    </div>

    <script type="text/javascript">
        function openinfo(url) {
            fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=350");
            fenster.focus();
        }
 
    </script>

</asp:Content>
