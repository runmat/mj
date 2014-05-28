<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Report_002_01.aspx.vb"
    Inherits="AppArval.Report_002_01" MasterPageFile="../../../MasterPage/Services.Master" %>
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
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>&nbsp;
                                <asp:Label ID="lblPageTitle" runat="server">(Anzeige Report)</asp:Label>
                            </h1>
                            <div>
                                <asp:LinkButton ID="lnkExcel" runat="server" Visible="True"><img alt="Excel"   src="../../../Images/iconXLS.gif"  />&nbsp;Excel herunterladen</asp:LinkButton>
                            </div>
                        </div>
                        <div id="TableQuery">
                            <div id="statistics">
                                <table id="tblBanner" cellpadding="0" runat="server" cellspacing="0">
                                    <tr>
                                        <td>
                                            KF1
                                        </td>
                                        <td>
                                         =
                                        </td>
                                        <td>
                                        Leasingnehmer unterschreibt nicht auf dem Sicherungsschein oder 
										Mahnstufe 4
                                        </td>
                                    </tr>
                                    
                                     <tr>
                                        <td>
                                          KF2
                                        </td>
                                        <td>
                                         =
                                        </td>
                                        <td>
                                       Versicherung unterschreibt nicht, bzw. kein Stempel auf dem 
										Sicherungsschein oder Mahnstufe 4
                                        </td>
                                    </tr>
                                    
                                      <tr>
                                        <td>
                                          KF3
                                        </td>
                                        <td>
                                         =
                                        </td>
                                        <td>
                                       Leasingnehmer hält sich nicht an das Gültigkeitsschema zum 
										Ausfüllen des Sicherungsscheins
                                        </td>
                                    </tr>
                                      <tr>
                                        <td>
                                          KF4
                                        </td>
                                        <td>
                                         =
                                        </td>
                                        <td>
                                      Versicherung hält sich nicht an das Gültigkeitsschema zum 
										Ausfüllen des Sicherungsscheins
                                        </td>
                                    </tr>
                                    
                                     <tr>
                                        <td>
                                         KF5
                                        </td>
                                        <td>
                                         =
                                        </td>
                                        <td>
                                      Leasingnehmer und Versicherungsnehmer sind nicht identisch
                                        </td>
                                    </tr>
                                    
                                    
                                     <tr>
                                        <td>
                                         KF6
                                        </td>
                                        <td>
                                         =
                                        </td>
                                        <td>
                                    §38/§39-Schreiben erhalten (Nichtzahlung der 
										Versicherungsprämie)
                                        </td>
                                    </tr>
                                    
                                     <tr>
                                        <td>
                                         KF7
                                        </td>
                                        <td>
                                         =
                                        </td>
                                        <td>
                                    Kündigung der Versicherung
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
                                            AutoGenerateColumns="True" AllowPaging="True" AllowSorting="True" ShowFooter="False"
                                            GridLines="None">
                                            <PagerStyle Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingItemStyle CssClass="GridTableAlternate" />
                                            <ItemStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <table id="Table10" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td nowrap>
                                                                    <asp:HyperLink ID="Hyperlink3" runat="server" Target='<%# "_blank" %>' Height="16px"
                                                                        Width="78px"  CssClass="Tablebutton" NavigateUrl='<%# "Report_002_02.aspx?equipment=" &amp; DataBinder.Eval(Container.DataItem, "Equipment") &amp; "&amp;kf=" &amp; DataBinder.Eval(Container.DataItem, "Klärfall") %>'>Details</asp:HyperLink>&nbsp;
                                                                </td>
                                                                <td nowrap>
                                                                    <asp:HyperLink ID="Hyperlink4" runat="server" Visible='<%# DataBinder.Eval(Container.DataItem, "Klärfall")<>"" %>'
                                                                        Target='<%# "_blank" %>' Height="16px" Width="78px" CssClass="Tablebutton" NavigateUrl='<%# "Report_03.aspx?e=" &amp; DataBinder.Eval(Container.DataItem, "Equipment") %>'>Formular</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="Kl&#228;rfall" SortExpression="Kl&#228;rfall"
                                                    HeaderText="Kl&#228;rfall"></asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>
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
</asp:Content>
