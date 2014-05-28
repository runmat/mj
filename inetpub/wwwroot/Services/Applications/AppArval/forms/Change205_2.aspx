<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Change205_2.aspx.vb"
    Inherits="AppArval.Change205_2" MasterPageFile="../../../MasterPage/Services.Master" %>
<%@ Register TagPrefix="uc1" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkKreditlimit" runat="server" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:HyperLink>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <div id="pagination">
                            <uc1:GridNavigation ID="GridNavigation1" runat="server"></uc1:GridNavigation>
                        </div>
                        <div id="data">
                            <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblError" runat="server" Visible="false" CssClass="TextError" EnableViewState="False"></asp:Label>
                                        <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid CssClass="GridView" ID="DataGrid1" runat="server" PageSize="50" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ShowFooter="False"
                                            GridLines="None">
                                            <PagerStyle Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingItemStyle CssClass="GridTableAlternate" />
                                            <ItemStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="Equnr" SortExpression="Equnr" HeaderText="Equipment">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Anfordern">
                                                  
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk0000" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="Leasingvertrags-Nr.">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="TIDNR" SortExpression="TIDNR" HeaderText="Kfz-Briefnummer">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kfz-Kennzeichen">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1"
                                                    HeaderText="Ordernummer"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="STATUS" SortExpression="STATUS" HeaderText="Status">
                                                </asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr><td class="firstLeft active" >
                                * Dieser Brief wurde bereits angefordert und befindet sich in der Autorisierung
                                </td></tr>
                                
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdSave" Text="Weiter" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
