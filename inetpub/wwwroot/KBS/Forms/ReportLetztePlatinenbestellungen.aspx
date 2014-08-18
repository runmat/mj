<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportLetztePlatinenbestellungen.aspx.vb"
    Inherits="KBS.ReportLetztePlatinenbestellungen" MasterPageFile="~/KBS.Master" %>
    
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Letzte Platinenbestellungen"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" Text="" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <asp:Panel runat="server" DefaultButton="lbSearch" style="border: solid 1px #DFDFDF;">
                        <div id="TableQuery" runat="server">
                            <table>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="3">
                                        <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblKst" runat="server">Kostenstelle</asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtKST" runat="server" Width="100px" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td class="firstLeft active">
                                        <div style="margin-left: 10px">
                                            <asp:Label ID="lblKSTText" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblLieferant" runat="server">Lieferant</asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:DropDownList ID="ddlLieferant" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="firstLeft active">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="lbSearch" Text="Suchen" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                    </asp:Panel>
                    <div id="data" runat="server">   
                        <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblNoData" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="rgGrid1" runat="server"
                                        AutoGenerateColumns="False" GridLines="None" Culture="de-DE" Skin="Default" Visible="false" AllowPaging="True" AllowSorting="True">
                                        <ClientSettings AllowKeyboardNavigation="true" >
                                            <Scrolling ScrollHeight="480px" AllowScroll="True" />
                                        </ClientSettings>
                                        <ItemStyle CssClass="ItemStyle" />
                                        <AlternatingItemStyle CssClass="ItemStyle" />
                                        <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" DataKeyNames="BSTNR">
                                            <SortExpressions>
                                                <telerik:GridSortExpression FieldName="Bestelldatum" SortOrder="Descending" />
                                            </SortExpressions>
                                            <HeaderStyle ForeColor="#595959" />
                                            <DetailTables>
                                                <telerik:GridTableView DataKeyNames="BSTNR" Width="100%" runat="server">
                                                    <SortExpressions>
                                                        <telerik:GridSortExpression FieldName="MAKTX" SortOrder="Ascending" />
                                                    </SortExpressions>
                                                    <ParentTableRelation>
                                                        <telerik:GridRelationFields DetailKeyField="BSTNR" MasterKeyField="BSTNR">
                                                        </telerik:GridRelationFields>
                                                    </ParentTableRelation>
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="MAKTX" SortExpression="MAKTX" HeaderText="Artikel" >
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ARTLIF" SortExpression="ARTLIF" HeaderText="Artikel-Nr." >
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="MENGE" SortExpression="MENGE" HeaderText="Menge" >
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ELIKZ" SortExpression="ELIKZ" HeaderText="Geliefert" >
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ZUSINFO_TXT" SortExpression="ZUSINFO_TXT" HeaderText="Zusatzinfo" >
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </telerik:GridTableView>
                                            </DetailTables>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="Bestelldatum" SortExpression="Bestelldatum" HeaderText="Datum" DataFormatString="{0:dd.MM.yyyy}" >
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="BSTNR" SortExpression="BSTNR" HeaderText="Bestellung Nr." >
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="LIEFERSNR" SortExpression="LIEFERSNR" HeaderText="Lieferschein Nr." >
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Lieferdatum" SortExpression="Lieferdatum" HeaderText="Lieferdatum" DataFormatString="{0:dd.MM.yyyy}" >
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </td>
                            </tr>
                        </table>
                        <div>
                            &nbsp;
                        </div>
                    </div>
                    <div id="dataFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
