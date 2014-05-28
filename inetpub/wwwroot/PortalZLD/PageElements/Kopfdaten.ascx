<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Kopfdaten.ascx.vb"  Inherits="CKG.PortalZLD.PageElements.Kopfdaten" %>


<div id="kopfdaten">
    <table cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr>
            <td>
                <asp:Label ID="lbl_HaendlerNummer" runat="server">Händlernummer:</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblHaendlerNummer" runat="server"></asp:Label>
            </td>
            <td rowspan="3">
                 <asp:DataGrid ID="DataGrid1" runat="server" BorderWidth="0px" AutoGenerateColumns="False"
                    CellPadding="0" class="Kontingente" GridLines="None" HeaderStyle-Font-Bold="true" Width="100%">
                    <Columns>
                        <asp:TemplateColumn Visible="False" HeaderText="ZeigeKontingentart">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
                                </asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Kontingent">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
                                </asp:Label>
                                <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Richtwert_Alt") %>'
                                    Visible="False">
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Ausschoepfung" HeaderText="Inanspruchnahme">
                        </asp:BoundColumn>
                        <asp:TemplateColumn  HeaderText="Freies Kontingent" HeaderStyle-Wrap="False">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>

<HeaderStyle Wrap="False"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Gesperrt">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'>
                                </asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>

<HeaderStyle Font-Bold="True"></HeaderStyle>
                </asp:DataGrid>
                </td>
            </tr>
        <tr>
            <td>
                Name:
            </td>
            <td nowrap="nowrap">
                <asp:Label ID="lblHaendlerName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Adresse:
            </td>
            <td>
                <asp:Label ID="lblAdresse" runat="server"></asp:Label>
            </td>
        </tr>      
           <tr>
            <td id="tdKontingent" rowspan="3" align="left">


<asp:Label ID="lblMessage" runat="server"></asp:Label>


            </td>
        </tr>

    </table>
</div>

