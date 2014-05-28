<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UeberfDADMenu.ascx.vb" Inherits="AppUeberf.UeberfDADMenu" %>
            
        <table>
            <tr>
                <td>
				    <asp:Menu ID="Menu1" runat="server" Width="245px" 
                        BackColor="Silver" 
                        Orientation="Horizontal" Height="35px">
                        <Items>
                            <asp:MenuItem Selectable="False" Text="Start" Value="Start" 
                                SeparatorImageUrl="/Portal/Images/arrow.gif"></asp:MenuItem>
                            <asp:MenuItem Selectable="False" Text="Zulassungsauftrag" 
                                Value="Zulassungsauftrag" SeparatorImageUrl="/Portal/Images/arrow.gif"></asp:MenuItem>
                            <asp:MenuItem Text="Versand Schein u. Schilder" 
                                Value="VSS" Selectable="False" 
                                SeparatorImageUrl="/Portal/Images/arrow.gif"></asp:MenuItem>
                            <asp:MenuItem Text="Auslieferung" Value="Auslieferung" 
                                Selectable="False" SeparatorImageUrl="/Portal/Images/arrow.gif"></asp:MenuItem>
                            <asp:MenuItem Text="Rücklieferung" Value="Rücklieferung" 
                                Selectable="False" SeparatorImageUrl="/Portal/Images/arrow.gif"></asp:MenuItem>
                            <asp:MenuItem Text="Auftragsübersicht" 
                                Value="Auftragsübersicht" Selectable="False"></asp:MenuItem>
                        </Items>
                        <StaticItemTemplate>
                            <%# Eval("Text") %>
                        </StaticItemTemplate>
                    </asp:Menu>
                </td>
            </tr>
        </table>
 
								

