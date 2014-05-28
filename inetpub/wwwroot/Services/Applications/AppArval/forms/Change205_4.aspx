<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Change205_4.aspx.vb"
    Inherits="AppArval.Change205_4" MasterPageFile="../../../MasterPage/Services.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                <asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx" CssClass="TaskTitle">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx"  Visible="False">Fahrzeugauswahl</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkAdressAuswahl" runat="server" NavigateUrl="Change04_3.aspx" Visible="False">Adressauswahl</asp:hyperlink><asp:label id="lblAddress" runat="server" Visible="False"></asp:label><asp:label id="lblMaterialNummer" runat="server" Visible="False"></asp:label>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                                <asp:label id="lblPageTitle" runat="server"> (Bestätigung)</asp:label>
                                </h1>
                        </div>
                        <div id="TableQuery">
                            <div id="statistics">
                                <table cellpadding="0" cellspacing="0">
                                    <tr id="trVersandTemp" runat="server">
                                        <td>
                                            <asp:Label ID="lblVersandAdresse" runat="server">Versandadresse:</asp:Label>
                                        </td>
                                        <td>
                                        <asp:Label ID="lblVersand" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trVersandArt" runat="server">
                                        <td>
                                            <asp:Label ID="lblVersandartTxt" runat="server">Versandart:</asp:Label>
                                        </td>
                                        <td>
                                          <asp:Label ID="lblVersandart" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblVersandgrundText" runat="server">Versandgrund:</asp:Label>
                                        </td>
                                        <td>
                                          <asp:Label ID="lblVersandGrund" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
                                                <asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kennzeichen">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="Leasing-&lt;br&gt;vertrags-Nr.">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="TIDNR" SortExpression="TIDNR" HeaderText="KFZ-Briefnummer">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1"
                                                    HeaderText="Ordernummer"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="STATUS" SortExpression="STATUS" HeaderText="Status">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn Visible="False" HeaderText="Anfordern">
                                                    
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk0001" runat="server" Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                            <div id="dataFooter">
                                <asp:LinkButton ID="cmdSave" Text="Absenden" Height="16px" Width="78px" runat="server"
                                    CssClass="Tablebutton"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
