<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Change03_4.aspx.vb"
    Inherits="AppArval.Change03_4" MasterPageFile="../../../MasterPage/Services.Master" %>
   
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change03_2.aspx">Fahrzeugsuche/Fahrzeugauswahl</asp:HyperLink>&nbsp;
                    <asp:HyperLink ID="lnkAdressAuswahl" runat="server" NavigateUrl="Change03_3.aspx"
                        Visible="False">Fahrzeugliste</asp:HyperLink>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                                &nbsp;
                                <asp:Label ID="lblPageTitle" runat="server">(Bestätigung)</asp:Label>
                            </h1>
                        </div>
                      
                        <div id="TableQuery">
                        <div id="statistics">
                            <table cellpadding="0" cellspacing="0">
                              
                                <tbody>
                                <tr >
                                        <td colspan="3" >
                                             <u><b>Sie haben folgenden Zulassungsauftrag erstellt</b></u>
                                        </td>
                                    </tr>
                                    <tr >
                                        <td colspan="3">
                                            <asp:Label ID="lblNoData" runat="server" CssClass="TextError"></asp:Label><asp:Label
                                                ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr >
                                        <td >
                                            <asp:Label ID="Label3" runat="server">Versicherung (optional):</asp:Label>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtVersicherung" runat="server" Width="250px" MaxLength="125"></asp:TextBox>
                                        </td>
                                        <td  width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        </div>
                       
                       
                        <div id="data">
                            <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                             <tfoot>
                                    <tr>
                                        <td >
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tr>
                                    <td>
                                        <asp:DataGrid CssClass="GridView" ID="DataGrid1" runat="server" PageSize="50" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="False" AllowSorting="True" ShowFooter="False"
                                            GridLines="None">
                                            <PagerStyle Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingItemStyle CssClass="GridTableAlternate" />
                                            <ItemStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="EquipmentNummer" SortExpression="EquipmentNummer"
                                                    HeaderText="Equipment"></asp:BoundColumn>
                                                <asp:TemplateColumn Visible="False" SortExpression="Ausgewaehlt" HeaderText="Auswahl">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="False" HeaderText="VA">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'>
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="FUnterlagen" SortExpression="FUnterlagen" HeaderText="Fehl.&lt;br&gt;Unterl.">
                                                    <ItemStyle ForeColor="Red"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="LeasingNummer" HeaderText="LV-Nr.">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" ToolTip="Leasingvertrags-Nr." CommandArgument="LeasingNummer"
                                                            CommandName="sort">LV-Nr.</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LeasingNummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Halter" SortExpression="Halter" HeaderText="Halter">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="FhgstNummer" HeaderText="Fahrgestellnr.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="FhgstNummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FhgstNummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Evbnummer" HeaderText="EVB-Nummer">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evbnummer" runat="server" Enabled="False" Width="60px" Text='<%# DataBinder.Eval(Container, "DataItem.Evbnummer") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Evb_Von" HeaderText="EVB von">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evb_Von" runat="server" Enabled="False" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.Evb_Von", "{0:dd.MM.yyyy}" ) %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Evb_Bis" HeaderText="EVB bis">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evb_Bis" runat="server" Enabled="False" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.Evb_Bis", "{0:dd.MM.yyyy}" ) %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="DatumZulassung" HeaderText="Zulassungsdat.">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="DatumZulassung" runat="server" Enabled="False" Width="80px" Text='<%# DataBinder.Eval(Container, "DataItem.DatumZulassung", "{0:dd.MM.yyyy}") %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Wunschkennzeichen"
                                                    HeaderText="Wunschkennzeichen">
                                                    
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Vorreserviert" HeaderText="Vorreserviert">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Vorreserviert" runat="server" Enabled="False" Checked='<%# (Not DataBinder.Eval(Container, "DataItem.Kennzeichenserie")) AND DataBinder.Eval(Container, "DataItem.Vorreserviert") %>'>
                                                        </asp:CheckBox>&nbsp;
                                                        <asp:TextBox ID="Reservierungsdaten" runat="server" Visible="False" Enabled="False"
                                                            Width="75px" Text='<%# DataBinder.Eval(Container, "DataItem.Reservierungsdaten") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Kennzeichenserie" HeaderText="AS">
                                                                                                       <ItemTemplate>
                                                        <asp:CheckBox ID="Kennzeichenserie" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Kennzeichenserie") AND (Not DataBinder.Eval(Container, "DataItem.Vorreserviert")) %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Zulstelle" SortExpression="Zulstelle" HeaderText="OrtsKZ">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" Height="16px" Width="78px" CssClass="Tablebutton" CommandName="delete">Verwerfen</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </div>
                         <div id="dataFooter">
                            <asp:LinkButton ID="cmdSave" Text="Absenden" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>&nbsp;
                            <asp:HyperLink ID="btnPrint" runat="server" Height="16px" Width="78px" CssClass="Tablebutton"
                                NavigateUrl="Change03_5.aspx" Target="_blank" Enabled="False">Druckansicht</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
