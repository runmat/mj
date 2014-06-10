<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="MainPage.aspx.cs" Inherits="Kantine.MainPage"
    MasterPageFile="Kantine.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="PageElements/Zahlenfeld.ascx" TagName="Zahlenfeld" TagPrefix="Kan" %>
<asp:Content ContentPlaceHolderID="Main" runat="server">
    <div id="Main" style="text-align: center;">
        <br />
        <asp:UpdatePanel ID="upMessages" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <div style="text-align: left;">
                    <div>
                        <asp:Label ID="lblAusgabe" runat="server" Visible="false" Style="font-weight: bold;"></asp:Label></div>
                    <div>
                        <asp:Label ID="lblError" runat="server" class="Error" Visible="false"></asp:Label></div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <div class="Heading">
            Verkauf
        </div>
        <div class="Rahmen">
            <asp:UpdatePanel ID="UpKundendaten" ChildrenAsTriggers="true" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="txtKundennummer" EventName="TextChanged" />
                    <asp:PostBackTrigger ControlID="btnCancel" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuchen" EventName="Click" />
                    <asp:PostBackTrigger ControlID="zfKundennummer" />
                </Triggers>
                <ContentTemplate>
                    <table>
                        <tr style="vertical-align: top;" class="BigLetters">
                            <td class="BigLetters">
                                Kundennr.:&nbsp;
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <asp:TextBox ID="txtKundennummer" runat="server" OnTextChanged="txtKundennummer_Changed" onkeyup="javascript:TextChanged(this)"
                                                AutoPostBack="true" MaxLength="10" ToolTip="Die Kundennummer ist exakt 8+2 Zeichen lang und besteht nur aus Zahlen."
                                                CssClass="BigLetters"></asp:TextBox>
                                            <script type="text/javascript" language="javascript">
                                                function TextChanged(sender)
                                                {
                                                    if (sender.value.length >= 10) {
                                                        sender.blur();
                                                    }
                                                }
                                            </script>
                                            <cc1:FilteredTextBoxExtender ID="txtKundennummer_FilteredTextBoxExtender" runat="server"
                                                Enabled="True" TargetControlID="txtKundennummer" FilterMode="ValidChars" ValidChars="0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                            <div id="Layer1" style="position: absolute; top: 250px; left: 550px; width: 200px;
                                                height: 300px;">
                                                <Kan:Zahlenfeld ID="zfKundennummer" runat="server" Modus="Zahlen" OnCommitEvent="zfKundennummer_Commit" />
                                            </div>
                                            <asp:Label ID="lblKundennummer" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtRechner" runat="server" ImageUrl="./Images/calculator_big.png"
                                                ImageAlign="Middle" OnClick="ibtRechner_Click" Width="48px" Height="48px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="BigLetters">
                                Kunde:&nbsp;
                            </td>
                            <td style="vertical-align: top;">
                                <asp:Label ID="lblKundenname" runat="server"></asp:Label>
                            </td>
                            <td width="100%">
                            </td>
                            <td class="BigLetters">
                                Guthaben:&nbsp;
                            </td>
                            <td style="white-space: nowrap; vertical-align: top;" class="BigLetters">
                                <asp:Label ID="lblGuthaben" runat="server">0,00</asp:Label>&#8364;
                            </td>
                            <td id="tdKontoauszug" runat="server" visible="false">
                                <asp:ImageButton ID="ibtKontoauszug" runat="server" ImageUrl="./Images/bar_chart.png"
                                    ImageAlign="Middle" OnClick="ibtKontoauszug_Click" Width="48px" Height="48px" />
                                <div id="LayerKontoauszug" runat="server" >
                                    <div class="BackgroundPopUp"></div>
                                    <div class="Box Background" style="position: absolute; top: 200px; left: 450px; width: 600px;
                                        min-height: 200px; z-index: 15;">
                                        <div style="text-align: left;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblKontoauszug" runat="server" Text="Kontoauszug" CssClass="BigLetters"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="ibtKontoauszugClose" runat="server" ImageUrl="./Images/kreuz.jpg"
                                                            ImageAlign="right" OnClick="ibtKontoauszug_Click" Width="37px" Height="37px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="Trennlinie">
                                        </div>
                                        <div>
                                            <asp:GridView ID="gvKontoauszug" runat="server" RowStyle-CssClass="TableItems" HeaderStyle-CssClass="Tablehead"
                                                AutoGenerateColumns="false" AllowPaging="false">
                                                <RowStyle Font-Size="12px" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="Datum" DataField="Datum" />
                                                    <asp:BoundField HeaderText="Aktion" DataField="Aktion" />
                                                    <asp:BoundField HeaderText="Artikel" DataField="Artikel" />
                                                    <asp:BoundField HeaderText="Betrag in &#8364;" DataField="Betrag" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpArtikel" runat="server" OnLoad="UpArtikel_Load" OnUnload="UpArtikel_Unload">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                    <asp:PostBackTrigger ControlID="btnCancel" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuchen" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <table id="tblVerkauf" runat="server" visible="false">
                        <tr style="height: 100%;">
                            <td width="70%" valign="top" align="justify">
                                <cc1:TabContainer ID="tcWaren" runat="server" Width="100%" AutoPostBack="True" ActiveTabIndex="5"
                                    CssClass="Tab" Height="370">
                                    <cc1:TabPanel HeaderText="Mittag" ID="tpMittag" runat="server" CssClass="tcWaren"
                                        Enabled="false">
                                        <HeaderTemplate>
                                            Mittag
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:ListView ID="lstvArtikel" runat="server">
                                                <LayoutTemplate>
                                                    <ul style="list-style-type: none;">
                                                        <asp:PlaceHolder ID="ItemPlaceholder" runat="server"></asp:PlaceHolder>
                                                        <li style="float: left;">
                                                            <div class="ButtonTouchBox">
                                                                <asp:Button ID="btnSondergericht" runat="server" Text='Sondergericht' CssClass="ButtonTouchSmall Rahmen"
                                                                    OnClick="btnSondergericht_Click" /></div>
                                                            <div id="LayerSondergericht" runat="server" style="position: absolute; top: 380px;
                                                                left: 750px; width: 300px; height: 310px;">
                                                                <Kan:Zahlenfeld ID="zfSG" runat="server" OnCommitEvent="zfSondergericht_Commit" Modus="Rechner" />
                                                            </div>
                                                        </li>
                                                    </ul>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <li style="float: left;">
                                                        <div class="ButtonTouchBox">
                                                            <asp:Button runat="server" Text='<%# Eval("Artikelbezeichnung") %>' CssClass="ButtonTouchSmall Rahmen"
                                                                OnClick="Button_Click" />
                                                        </div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel ID="tpBrötchen" runat="server" HeaderText="Backwaren" CssClass="tcWaren">
                                        <ContentTemplate>
                                            <asp:ListView ID="lstvArtikel2" runat="server">
                                                <LayoutTemplate>
                                                    <ul style="list-style-type: none;">
                                                        <asp:PlaceHolder ID="ItemPlaceholder" runat="server"></asp:PlaceHolder>
                                                    </ul>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <li style="float: left;">
                                                        <div class="ButtonTouchBox">
                                                            <asp:Button runat="server" Text='<%# Eval("Artikelbezeichnung") %>' CssClass="ButtonTouchSmall Rahmen"
                                                                OnClick="Button_Click" /></div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel ID="tpSweets" runat="server" HeaderText="Süßwaren" CssClass="tcWaren">
                                        <HeaderTemplate>
                                            Süßwaren</HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:ListView ID="lstvArtikel3" runat="server">
                                                <LayoutTemplate>
                                                    <ul style="list-style-type: none;">
                                                        <asp:PlaceHolder ID="ItemPlaceholder" runat="server"></asp:PlaceHolder>
                                                    </ul>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <li style="float: left;">
                                                       <div class="ButtonTouchBox">
                                                            <asp:Button ID="Button1" runat="server" Text='<%# Eval("Artikelbezeichnung") %>'
                                                                CssClass="ButtonTouchSmall Rahmen" OnClick="Button_Click" /></div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel ID="tpEis" runat="server" HeaderText="Eis" CssClass="tcWaren">
                                        <ContentTemplate>
                                            <asp:ListView ID="lstvArtikel4" runat="server">
                                                <LayoutTemplate>
                                                    <ul style="list-style-type: none;">
                                                        <asp:PlaceHolder ID="ItemPlaceholder" runat="server"></asp:PlaceHolder>
                                                    </ul>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <li style="float: left;">
                                                       <div class="ButtonTouchBox">
                                                            <asp:Button ID="Button2" runat="server" Text='<%# Eval("Artikelbezeichnung") %>'
                                                                CssClass="ButtonTouchSmall Rahmen" OnClick="Button_Click" /></div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel ID="tpGetränke" runat="server" HeaderText="Getränke" CssClass="tcWaren">
                                        <ContentTemplate>
                                            <asp:ListView ID="lstvArtikel5" runat="server">
                                                <LayoutTemplate>
                                                    <ul style="list-style-type: none;">
                                                        <asp:PlaceHolder ID="ItemPlaceholder" runat="server"></asp:PlaceHolder>
                                                    </ul>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <li style="float: left;">
                                                        <div class="ButtonTouchBox">
                                                            <asp:Button ID="Button3" runat="server" Text='<%# Eval("Artikelbezeichnung") %>'
                                                                CssClass="ButtonTouchSmall Rahmen" OnClick="Button_Click" /></div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel ID="tpGuthaben" runat="server" Enabled="false" HeaderText="Guthaben" CssClass="tcWaren">
                                        <ContentTemplate>
                                                        <ul style="list-style-type: none;">
                                                            <li style="float: left;">
                                                                <div class="ButtonTouchBox">
                                                                    <asp:Button ID="Button4" runat="server" Text='5 &#8364;' CssClass="ButtonTouchSmall Rahmen" OnClick="btn5Euro_Click" />
                                                                </div>
                                                            </li>
                                                            <li style="float: left;">
                                                                <div class="ButtonTouchBox">
                                                                    <asp:Button ID="Button6" runat="server" Text='10 &#8364;' CssClass="ButtonTouchSmall Rahmen"
                                                                        OnClick="btn10Euro_Click" /></div>
                                                            </li>
                                                            <li style="float: left;">
                                                                <div class="ButtonTouchBox">
                                                                    <asp:Button ID="Button5" runat="server" Text='15 &#8364;' CssClass="ButtonTouchSmall Rahmen"
                                                                        OnClick="btn15Euro_Click" /></div>
                                                            </li>
                                                            <li style="float: left;">
                                                                <div class="ButtonTouchBox">
                                                                    <asp:Button ID="Button7" runat="server" Text='20 &#8364;' CssClass="ButtonTouchSmall Rahmen"
                                                                        OnClick="btn20Euro_Click" /></div>
                                                            </li>
                                                            <li style="float: left;">
                                                                <div class="ButtonTouchBox">
                                                                    <asp:Button ID="Button8" runat="server" Text='50 &#8364;' CssClass="ButtonTouchSmall Rahmen"
                                                                        OnClick="btn50Euro_Click" /></div>
                                                            </li>
                                                            <li style="float: left;">
                                                                <div class="ButtonTouchBox">
                                                                    <asp:Button ID="Button3" runat="server" Text='Freier Betrag' CssClass="ButtonTouchSmall Rahmen"
                                                                        OnClick="btnFreierBetrag_Click" /></div>
                                                                <div id="LayerFreierBetrag" style="position: absolute; top: 450px; left: 750px; width: 300px;
                                                                    height: 300px;">
                                                                    <Kan:Zahlenfeld ID="zfFreierBetrag" runat="server" OnCommitEvent="zfFreierBetrag_Commit"
                                                                        Modus="Rechner" />
                                                                </div>
                                                            </li>                                                            
                                                        </ul>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                </cc1:TabContainer>
                            </td>
                            <td width="30%" style="vertical-align: top; height: 100%;">
                                <div class="Box" style="height: 400px;">
                                    <table id="tblArtikelliste" runat="server">
                                        <tr>
                                            <td valign="top" style="margin-top: 20px;">
                                                <div style="vertical-align: bottom;">
                                                    <table>
                                                        <tr>
                                                            <td class="Beschriftung" style="font-size: medium; text-align: left;">
                                                                Artikel
                                                            </td>
                                                            <td class="Beschriftung" style="font-size: medium; text-align: right;">
                                                                Preis
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <hr />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <asp:ListView ID="lstvRechnung" runat="server" DataKeyNames="Artikel">
                                                    <LayoutTemplate>
                                                        <table id="itemPlaceholderContainer">
                                                            <tr runat="server" id="itemPlaceholder" class="active">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <%# Eval("Artikel") %>
                                                            </td>
                                                            <td style="text-align: right;">
                                                                <%# Eval("Preis") %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                                <div style="vertical-align: bottom;">
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <hr />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Beschriftung" style="font-size: medium; text-align: left;">
                                                                Summe:
                                                            </td>
                                                            <td class="Beschriftung" style="font-size: medium; text-align: right;">
                                                                <asp:Label ID="lblSumme" runat="server">0,00</asp:Label>&#8364;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="Buttoncontainer">
            <asp:UpdatePanel ID="upButtons" runat="server">
                <ContentTemplate>
                    <table id="tblButtons" runat="server" visible="false">
                        <tr>
                            <td width="100%">
                            </td>
                            <td>
                                <%--<div style="font-weight: Bold; font-size: 24;">
                                    <asp:ImageButton ID="btnBack" runat="server"  CssClass="ButtonTouch Rahmen Image" OnClick="btnBack_Click"
                                        Width="80" height="10" ImageUrl="Images/Pfeil2.png" BorderWidth="1px" ImageAlign="Middle" />                                                     
                                </div>--%>
                                <asp:Button ID="btnBack" runat="server" Text="&#8592;" CssClass="ButtonTouch Rahmen" OnClick="btnBack_Click"
                                    Width="100" style="font-size: 32pt;"/>
                            </td>
                            <td>
                                <asp:Button ID="btnCancel" runat="server" Text="Abbruch" CssClass="ButtonTouch Rahmen" OnClick="btnCancel_Click"
                                    Width="100" />
                            </td>
                            <td>
                                <asp:Button ID="btnBuchen" runat="server" Text="Buchen" CssClass="ButtonTouch Rahmen" OnClick="btnBuchen_Click"
                                    Width="100" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--<asp:UpdatePanel ID="UpMessagebox" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                <Triggers>                    
                </Triggers>
                <ContentTemplate>
                    <asp:LinkButton ID="lbnMPEhide" runat="server"></asp:LinkButton>
                    <cc1:ModalPopupExtender ID="mpeOK" runat="server" PopupControlID="divMessagebox"
                        TargetControlID="lbnMPEhide" OkControlID="lbtnOK" CancelControlID="lbtnAbbruch"
                        DropShadow="true" OnLoad="mpeOK_Load" OnInit="mpeOK_Init" RepositionMode="RepositionOnWindowResizeAndScroll" />
                    <div id="divMessagebox" runat="server" class="Box Background" style="position: absolute;
                        top: 300px; left: 450px; width: 500px; visibility: hidden;">
                        <div style="text-align: left;">
                            <table>
                                <tr>
                                    <td style="margin: 10px 10px 10px 10px">
                                        <asp:Label ID="lblMessageTitle" runat="server" Text="Achtung!" CssClass="BigLetters"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="Trennlinie">
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <td width="100%" colspan="2">
                                        <asp:Label ID="lblMessageboxText" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td width="50%">
                                        <asp:Button ID="lbtnOK" runat="server" CssClass="ButtonTouch Rahmen" Width="100px" Height="50px"
                                            OnClick="lbtnOK_Click" Text="OK" />
                                    </td>
                                    <td width="50%">
                                        <asp:Button ID="lbtnAbbruch" runat="server" CssClass="ButtonTouch Rahmen" Width="100px"
                                            Height="50px" OnClick="lbtnAbbruch_Click" Text="Abbruch" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
       </ContentTemplate></asp:UpdatePanel>--%>
        </div>
    </div>
</asp:Content>
