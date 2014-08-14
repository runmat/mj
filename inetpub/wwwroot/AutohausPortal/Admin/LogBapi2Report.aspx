<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogBapi2Report.aspx.vb"
    Inherits="Admin.LogBapi2Report" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Reportname"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td nowrap="nowrap" class="firstLeft active">
                                            </td>
                                            <td nowrap="nowrap" class="firstLeft active" width="100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Kunde:
                                            </td>
                                            <td nowrap="nowrap" valign="top" class="firstLeft active">
                                                <asp:DropDownList ID="ddlFilterCustomer" runat="server" Font-Names="Verdana,sans-serif">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCustomer" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                BAPI:
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox ID="txtBapiName" runat="server" CssClass="InputTextbox">*</asp:TextBox>
                                                <asp:Label ID="lblBapiName" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Freundlicher Name:
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox ID="txtFriendName" runat="server" CssClass="InputTextbox">*</asp:TextBox>
                                                <asp:Label ID="lblFriendName" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Technischer Name:
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox ID="txtTechName" runat="server" CssClass="InputTextbox">*</asp:TextBox>
                                                <asp:Label ID="lblTechName" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                im Zeitraum von:
                                            </td>
                                            <td valign="top" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox CssClass="InputTextbox" runat="server" Width="83px" ToolTip="Datum von"
                                                    ID="txtDateVon" MaxLength="10"  />
                                                <ajaxToolkit:CalendarExtender ID="defaultCalendarExtenderVon" runat="server" TargetControlID="txtDateVon" />
                                                &nbsp;bis:&nbsp;
                                                <asp:TextBox CssClass="InputTextbox" runat="server" Width="83px" ToolTip="Datum bis"
                                                    ID="txtDateBis" MaxLength="10"  />
                                                <ajaxToolkit:CalendarExtender ID="defaultCalendarExtenderBis" runat="server" TargetControlID="txtDateBis" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="btnSuche" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="Images/iconXLS.gif" alt="Excel herunterladen" />
                                <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False" ForeColor="White">Excel herunterladen</asp:LinkButton>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanelNavi" runat="server">
                            <ContentTemplate>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvResult" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                                                        <asp:BoundField DataField="Kunnr" SortExpression="Kunnr" HeaderText="Kundennr." />
                                                        <asp:BoundField DataField="Kundenbezeichnung" SortExpression="Kundenbezeichnung"
                                                            HeaderText="Kunde" />
                                                        <asp:BoundField DataField="AppID" SortExpression="AppID" HeaderText="AppID" />
                                                        <asp:BoundField DataField="Freundlicher_Name" SortExpression="Freundlicher_Name"
                                                            HeaderText="Freundlicher Name" />
                                                        <asp:BoundField DataField="Technischer_Name" SortExpression="Technischer_Name" HeaderText="Tech. Name"
                                                            HeaderStyle-Wrap="False" />
                                                        <asp:BoundField DataField="Pfad" HeaderText="Pfad" SortExpression="Pfad" Visible="false" />
                                                        <asp:BoundField DataField="BAPI" HeaderText="BAPI" SortExpression="BAPI" />
                                                        <asp:BoundField DataField="LastUse" HeaderText="zuletzt genutzt" SortExpression="LastUse" />
                                                    </Columns>
                                                    <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div id="dataFooter">
                    &nbsp;</div>
            </div>
        </div>
    </div>
</asp:Content>
