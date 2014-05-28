<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SelektionmitGrid.aspx.vb"
    Inherits="CKG.Services.Vorlage" MasterPageFile="../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton runat="server" ID="lb_zurueck" Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div>
                    <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                </div>
                <div>
                    <asp:Label ID="lblError" runat="server" Visible="False" CssClass="TextError" EnableViewState="False"></asp:Label>
                </div>
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
                                                    <input type="button" id="NewSearch" onclick="ShowHide('tab1');" alt="Neue Abfrage"
                                                        style="border: none; background-image: url(../../../Images/queryArrow.gif); width: 16px;
                                                        height: 16px; cursor: hand;" runat="server" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" cellpadding="0" cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                &nbsp;
                                            </td>
                                            <td nowrap="nowrap" class="firstLeft active" width="100%">
                                                <asp:Label ID="Label1" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                Dropdown&nbsp;
                                            </td>
                                            <td nowrap="nowrap" valign="top" class="firstLeft active">
                                                &nbsp;
                                                <asp:DropDownList ID="drpVJahr" runat="server" Font-Names="Verdana,sans-serif">
                                                </asp:DropDownList>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                <span lang="de">Text</span>:
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                &nbsp;
                                                <asp:TextBox ID="txtOrgNr" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                &nbsp;
                                                <asp:Label ID="lblPlatzhaltersuche" runat="server" Height="16px">*(mit Platzhaltersuche)</asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                Datum :
                                            </td>
                                            <td valign="top" nowrap="nowrap" class="firstLeft active">
                                                &nbsp;
                                                <asp:TextBox CssClass="InputTextbox" runat="server" Width="83px" ToolTip="Datum von"
                                                    ID="txtDateVon" MaxLength="10" autocomplete="off" />
                                                <ajaxToolkit:CalendarExtender ID="defaultCalendarExtenderVon" runat="server" TargetControlID="txtDateVon" />
                                                &nbsp;bis:&nbsp;
                                                <asp:TextBox CssClass="InputTextbox" runat="server" Width="83px" ToolTip="Datum bis"
                                                    ID="txtDateBis" MaxLength="10" autocomplete="off" />
                                                <ajaxToolkit:CalendarExtender ID="defaultCalendarExtenderBis" runat="server" TargetControlID="txtDateBis" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td nowrap="nowrap">
                                                &nbsp;
                                            </td>
                                            <td valign="middle" align="right" nowrap="nowrap">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="btnSuche" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                    </div>

                    <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False" ForeColor="White">Excel herunterladen</asp:LinkButton>
                                </span>
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
                                                <asp:GridView ID="GridView1" Width="100%" runat="server" AllowSorting="True" AutoGenerateColumns="True"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="50" EditRowStyle-Wrap="False" PagerStyle-Wrap="True" CssClass="GridView">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="VD-Bezirk" SortExpression="VD-Bezirk" HeaderText="Name">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField" Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Name1" SortExpression="Name1" HeaderText="Name 1">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Name2" SortExpression="Name2" HeaderText="Name 2">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Auftragsnummer" SortExpression="Auftragsnummer" HeaderText="Auftragsnummer">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Versand am" DataFormatString="{0:dd.MM.yyyy}" SortExpression="Versand am"
                                                            HeaderText="Versand am">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Stueckzahl" SortExpression="Stueckzahl" HeaderText="Stueckzahl">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Kennz. anzeigen" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkKennzAnzeigen" runat="server"></asp:CheckBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Adresse anzeigen" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="RadioButton1" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="Tablehead" ForeColor="White" />
                                                    <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
