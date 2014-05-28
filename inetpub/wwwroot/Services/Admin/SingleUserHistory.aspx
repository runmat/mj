<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SingleUserHistory.aspx.vb"
    Inherits="Admin.SingleUserHistory" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Benutzerhistorie - "></asp:Label>
                            <asp:Label ID="lblUsername" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" ></asp:Label>
                            <div id="TableQuery">
                                <table id="tblSearch" runat="server" cellpadding="0" cellspacing="0" width="100%"
                                    border="0">
                                    <tr class="formquery">
                                        <td class="firstLeft active" >
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Datum von:
                                        </td>
                                        <td class="firstLeft">
                                            <asp:TextBox ID="txtDatumVon" runat="server" Width="80px" Height="20px" MaxLength="10"></asp:TextBox>
                                            <cc1:CalendarExtender ID="ceTxtDatumVon" runat="server" TargetControlID="txtDatumVon">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="meeTxtDatumVon" runat="server" TargetControlID="txtDatumVon"
                                                UserDateFormat="DayMonthYear" Mask="99/99/9999" MaskType="Date">
                                            </cc1:MaskedEditExtender>
                                        </td>
                                        <td class="firstLeft active">
                                            Datum bis:
                                        </td>
                                        <td class="firstLeft">
                                            <asp:TextBox ID="txtDatumBis" runat="server" Width="80px" Height="20px" MaxLength="10"></asp:TextBox>
                                            <cc1:CalendarExtender ID="ceTxtDatumBis" runat="server" TargetControlID="txtDatumBis">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="meeTxtDatumBis" runat="server" TargetControlID="txtDatumBis"
                                                UserDateFormat="DayMonthYear" Mask="99/99/9999" MaskType="Date">
                                            </cc1:MaskedEditExtender>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft" colspan="5" align="right">
                                            <asp:LinkButton ID="btnCreate" runat="server" Class="Tablebutton" Width="78px">»Erstellen</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="firstLeft" colspan="5">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="dataQueryFooter" runat="server">
                                &nbsp;
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel 
                                        herunterladen</asp:LinkButton>
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
                                                <asp:DataGrid ID="dgSearchResult" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AllowPaging="True" GridLines="None" PageSize="20" CssClass="GridView">
                                                    <SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"
                                                        Wrap="false"></HeaderStyle>
                                                    <PagerStyle Visible="False" />
                                                    <AlternatingItemStyle CssClass="GridTableAlternate" />
                                                    <ItemStyle Wrap="false" CssClass="ItemStyle" />
                                                    <EditItemStyle Wrap="False"></EditItemStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="ID" SortExpression="ID" HeaderText="ID" Visible="false">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Aktion" SortExpression="Aktion" HeaderText="Aktion">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Änderer" SortExpression="Änderer" HeaderText="Änderer">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Alter Wert" SortExpression="Alter Wert" HeaderText="Alter Wert">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Neuer Wert" SortExpression="Neuer Wert" HeaderText="Neuer Wert">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Zeitpunkt" SortExpression="Zeitpunkt" HeaderText="Zeitpunkt">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Typ" SortExpression="Typ" HeaderText="Typ"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>                                                
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="Div7" runat="server">
                                &nbsp;
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
