<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Change03_2.aspx.vb"
    Inherits="AppArval.Change03_2" MasterPageFile="../../../MasterPage/Services.Master" %>
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
                        <div id="paginationQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="firstLeft active">
                                            bitte geben Sie ihre Suchkriterien ein
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="TableQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tfoot>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr class="formquery">
                                        <td colspan="3"  class="firstLeft active">
                                        <asp:Label ID="lblNoData" runat="server" CssClass="TextError"></asp:Label><asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                     </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Leasingvertrag-Nr.:
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtLeasingnummerErfassung" runat="server" ></asp:TextBox>
                                            <asp:TextBox ID="txtFahrgestellnr" runat="server" CssClass="TextBoxNormal" Visible="false"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td >
                                            <asp:CheckBox ID="CheckBox1" runat="server" Text="Nach ARVAL filtern" TextAlign="Left"
                                                Visible="False"></asp:CheckBox>
                                        </td>
                                        <td  width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdSearch" Text="Suchen" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="cmdSave" Text="Weiter" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                        <div id="pagination" >
                            <uc1:gridnavigation id="GridNavigation1" runat="server"></uc1:gridnavigation>
                        </div>
                        <div id="data">
                            <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                
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
                                                <asp:TemplateColumn SortExpression="Ausgewaehlt" HeaderText="Ausw.">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lbtnAuswahl" runat="server" ToolTip="Zur Auswahl markieren" CommandName="sort"
                                                            CommandArgument="Ausgewaehlt">Ausw.</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Auswahl" runat="server" Visible='<%# NOT (CStr(DataBinder.Eval(Container, "DataItem.Zulstelle")).Length=0) %>'
                                                            Checked='<%# DataBinder.Eval(Container, "DataItem.Ausgewaehlt") %>'></asp:CheckBox>
                                                        <asp:Label ID="Label2" runat="server" Visible='<%# (CStr(DataBinder.Eval(Container, "DataItem.Zulstelle")).Length=0) %>'>unvollst.</asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="FUnterlagen" SortExpression="FUnterlagen" HeaderText="Fehl.&lt;br&gt;Unterl.*">
                                                    <ItemStyle Font-Size="XX-Small" ForeColor="Red"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn Visible="False" HeaderText="FUFlag">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="EquipmentNummer" SortExpression="EquipmentNummer"
                                                    HeaderText="EquipmentNummer"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="HaendlerNummer" SortExpression="HaendlerNummer"
                                                    HeaderText="HaendlerNummer"></asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="LeasingNummer" HeaderText="LV-Nr.">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton0" runat="server" ToolTip="Leasingvertrags-Nr." CommandName="sort"
                                                            CommandArgument="LeasingNummer">LV-Nr.</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLvNr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LeasingNummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="FhgstNummer" HeaderText="Fahrgestell-Nr.">
                                                    <ItemStyle Font-Size="XX-Small"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="FhgstNummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FhgstNummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="DatumBriefeingang" SortExpression="DatumBriefeingang"
                                                    HeaderText="Datum&lt;br&gt;Briefeingang" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Halter" SortExpression="Halter" HeaderStyle-Width="150px"
                                                    HeaderText="Halter">
                                                    <ItemStyle Font-Size="XX-Small" Width="150px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="Standort" SortExpression="Standort" HeaderText="Standort">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="Evbnummer" HeaderText="EVB-Nummer">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evbnummer" runat="server" Width="60px" Text='<%# DataBinder.Eval(Container, "DataItem.Evbnummer") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Evb_Von" HeaderText="EVB von">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evb_Von" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.Evb_Von", "{0:dd.MM.yyyy}" ) %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Evb_Bis" HeaderText="EVB bis">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evb_Bis" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.Evb_Bis", "{0:dd.MM.yyyy}" ) %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="DatumZulassung" HeaderText="Datum&lt;br&gt;Zulassung">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="DatumZulassung" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container, "DataItem.DatumZulassung", "{0:dd.MM.yyyy}") %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Zulstelle" HeaderText="OrtsKZ">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton5" runat="server" ToolTip="Ortskennzeichen" CommandName="sort"
                                                            CommandArgument="Zulstelle">OrtsKZ</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulstelle") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulstelle") %>'>
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Wuk01_Buchstaben" HeaderText="WK.1">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Wunschkennzeichen 1: Buchstabenkombination (max. 2-Stellig), Zahl 4-Stellig"
                                                            CommandArgument="Wuk01_Buchstaben" CommandName="sort">WK.1</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Wuk01_Buchstaben" runat="server" Width="25px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk01_Buchstaben") %>'
                                                            MaxLength="2">
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="Wuk01_Ziffern" runat="server" Width="40px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk01_Ziffern") %>'
                                                            MaxLength="4">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Wuk02_Buchstaben" HeaderText="WK.2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" ToolTip="Wunschkennzeichen 2" CommandArgument="Wuk02_Buchstaben"
                                                            CommandName="sort">WK.2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Wuk02_Buchstaben" runat="server" Width="25px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk02_Buchstaben") %>'
                                                            MaxLength="2">
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="Wuk02_Ziffern" runat="server" Width="40px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk02_Ziffern") %>'
                                                            MaxLength="4">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="False" SortExpression="Wuk03_Buchstaben" HeaderText="WK.3">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" ToolTip="Wunschkennzeichen 3" CommandArgument="Wuk03_Buchstaben"
                                                            CommandName="sort">WK.3</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Wuk03_Buchstaben" runat="server" Width="25px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk03_Buchstaben") %>'
                                                            MaxLength="2">
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="Wuk03_Ziffern" runat="server" Width="40px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk03_Ziffern") %>'
                                                            MaxLength="4">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Vorreserviert" HeaderText="Vorreserviert">
                                                    <ItemStyle Width="100px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="Vorreserviert" runat="server" TextAlign="Left" Checked='<%# (Not DataBinder.Eval(Container, "DataItem.Kennzeichenserie")) AND DataBinder.Eval(Container, "DataItem.Vorreserviert") %>'
                                                            GroupName="A1"></asp:RadioButton>
                                                        <asp:TextBox ID="Reservierungsdaten" runat="server" Width="75px" Text='<%# DataBinder.Eval(Container, "DataItem.Reservierungsdaten") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Kennzeichenserie" HeaderText="AS">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton9" runat="server" CommandArgument="Kennzeichenserie"
                                                            CommandName="sort" ToolTip="Aus Kennzeichen-Serie">AS</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="Kennzeichenserie" runat="server" GroupName="A1" Checked='<%# DataBinder.Eval(Container, "DataItem.Kennzeichenserie") AND (Not DataBinder.Eval(Container, "DataItem.Vorreserviert")) %>'>
                                                        </asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="KA">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" ToolTip="Keine Auswahl">KA</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="Radiobutton1" runat="server" GroupName="A1" Checked='<%# (Not DataBinder.Eval(Container, "DataItem.Kennzeichenserie")) AND (Not DataBinder.Eval(Container, "DataItem.Vorreserviert")) %>'>
                                                        </asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblLegende" runat="server" Visible="False">*(V)ollmacht, (H)andelsregistereintrag,(P)ersonalausweis,(G)ewerbeanmeldung,(E)inzugsermächtigung</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
