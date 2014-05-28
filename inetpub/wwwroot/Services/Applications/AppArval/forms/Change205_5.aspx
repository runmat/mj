<%@ Page Language="vb"   EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Change205_5.aspx.vb" Inherits="AppArval.Change205_5"
     MasterPageFile="../../../MasterPage/Services.Master" %>
     <%@ Register TagPrefix="uc1" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
     
     <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        
                     
                     
                     <div id="pagination">
                    
                                     <uc1:GridNavigation id="GridNavigation1" runat="server" ></uc1:GridNavigation>
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
                                                 <HeaderStyle CssClass="GridTableHead" ></HeaderStyle>
                                                                              <AlternatingItemStyle CssClass="GridTableAlternate" />
                                                                <ItemStyle CssClass="ItemStyle" />
                                                                <Columns>
														<asp:BoundColumn Visible="False" DataField="id" SortExpression="id" HeaderText="ID"></asp:BoundColumn>
														<asp:BoundColumn DataField="Erstellt" SortExpression="Erstellt" HeaderText="Erstellt"></asp:BoundColumn>
														<asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Equipment" SortExpression="Equipment" HeaderText="Equi"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnr" SortExpression="Fahrgestellnr" HeaderText="Fahrg.Nr."></asp:BoundColumn>
														<asp:BoundColumn DataField="Lvnr" SortExpression="Lvnr" HeaderText="LV-Nr."></asp:BoundColumn>
														<asp:BoundColumn DataField="Versandadresse" SortExpression="Versandadresse" HeaderText="Versandadr."></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadresseName1" SortExpression="VersandadresseName1" HeaderText="VersandadresseName1"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadresseName2" SortExpression="VersandadresseName2" HeaderText="VersandadresseName2"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadresseStr" SortExpression="VersandadresseStr" HeaderText="VersandadresseStr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadresseNr" SortExpression="VersandadresseNr" HeaderText="VersandadresseNr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadressePlz" SortExpression="VersandadressePlz" HeaderText="VersandadressePlz"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadresseOrt" SortExpression="VersandadresseOrt" HeaderText="VersandadresseOrt"></asp:BoundColumn>
														<asp:BoundColumn DataField="VersandadresseLand" HeaderText="VersandadresseLand" 
                                                            SortExpression="VersandadresseLand" Visible="False"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Haendlernummer" SortExpression="Haendlernummer" HeaderText="H&#228;nderlnummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="TIDNr" SortExpression="TIDNr" HeaderText="TIDNr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="LIZNr" SortExpression="LIZNr" HeaderText="LIZNr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Materialnummer" SortExpression="Materialnummer" HeaderText="Materialnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="VersandartShow" SortExpression="VersandartShow" HeaderText="Versandart"></asp:BoundColumn>
														<asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundColumn>
														<asp:TemplateColumn>
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:LinkButton id=btnFreigeben runat="server" CssClass="Tablebutton" Height="16px" Width="78px"   Enabled='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull %>' CommandName="Freigeben">Freigeben</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:LinkButton id=LinkButton3 runat="server" CssClass="Tablebutton" Height="16px" Width="78px"  Enabled='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull %>' CommandName="delete">Storno</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													</asp:DataGrid>
													</td>
													</tr>
													</table>
                                                                

                     
                     
                     
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdSave" Text="Weiter" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"  Visible="false" OnClientClick="Show_ctl00_ContentPlaceHolder1_BusyBox1();"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>