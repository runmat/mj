<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Disposition.aspx.cs"
    Inherits="AppZulassungsdienst.forms.Disposition" MasterPageFile="../MasterPage/Big.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" onclick="lb_zurueck_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="rbNichtDisponiert" />
                            <asp:PostBackTrigger ControlID="rbBereitsDisponiert" />
                            <asp:PostBackTrigger ControlID="rbBereitsInArbeit" />
                            <asp:PostBackTrigger ControlID="chkAlleAemter" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="6" style="vertical-align:top">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#269700"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width: 100px" >
                                            <asp:Label ID="lblDatum" runat="server">Zulassungdatum:</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="width: 140px" >
                                            <asp:TextBox ID="txtZulDate" runat="server" CssClass="TextBoxNormal" 
                                                Width="75px" MaxLength="6"></asp:TextBox>
                                            <asp:Label ID="txtZulDateFormate" Style="padding-left: 2px; font-weight: normal"
                                                Height="15px" runat="server" Width="48px">(ttmmjj)</asp:Label>                                                      
                                        </td>
                                        <td class="firstLeft active" style="width: 140px" >
                                            <asp:LinkButton ID="lbtnGestern" runat="server" Height="15px" 
                                                Style="font-weight: normal" Text="Gestern |" />
                                            <asp:LinkButton ID="lbtnHeute" runat="server" Height="15px" 
                                                Style="font-weight: normal" Text="Heute |" />
                                            <asp:LinkButton ID="lbtnMorgen" runat="server" Height="15px" 
                                                Style="font-weight: normal" Text="Morgen" />
                                        </td>
                                        <td class="firstLeft active" style="width: 370px" >
                                            <asp:RadioButton ID="rbNichtDisponiert" runat="server" Checked="true" 
                                                GroupName="Modus" Text="nicht disponiert" AutoPostBack="True" OnCheckedChanged="OnSelectionChanged" />
                                            <asp:RadioButton ID="rbBereitsDisponiert" runat="server" GroupName="Modus" 
                                                style="padding-left:5px" Text="bereits disponiert" AutoPostBack="True" OnCheckedChanged="OnSelectionChanged" />
                                            <asp:RadioButton ID="rbBereitsInArbeit" runat="server" GroupName="Modus" 
                                                style="padding-left:5px" Text="bereits in Arbeit" AutoPostBack="True" OnCheckedChanged="OnSelectionChanged" />
                                        </td>
                                        <td class="firstLeft active" style="width: 90px" >
                                            <asp:CheckBox ID="chkAlleAemter" runat="server" Text="alle Ämter" AutoPostBack="True" OnCheckedChanged="OnSelectionChanged" />
                                        </td>
                                        <td class="firstLeft active" >
                                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px" 
                                                onclick="cmdSearch_Click">» Suchen </asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="Result" runat="Server">
                                <div id="data">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvDispositionen" runat="server" AutoGenerateColumns="False" 
                                                    CellPadding="0" CellSpacing="0" GridLines="None" 
                                                    AllowSorting="false" AllowPaging="false" CssClass="GridView" 
                                                    OnRowDataBound="gvDispositionen_RowDataBound" ShowFooter="True">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <FooterStyle CssClass="GridTableFoot" ForeColor="White" Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Amt">
                                                            <HeaderStyle Width="45px"/>
                                                            <FooterStyle HorizontalAlign="Center"/>
                                                            <ItemStyle HorizontalAlign="Center"/>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblAmt" Text='<%# Eval("Amt") %>' Width="40px" style="text-align: left"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblKreisBezeichnung" Text='<%# Eval("KreisBezeichnung") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <HeaderStyle Width="16px"/>
                                                            <FooterStyle HorizontalAlign="Center"/>
                                                            <ItemStyle HorizontalAlign="Center"/>
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgAmtHinweis" runat="server" Visible='<%# (!string.IsNullOrEmpty(Eval("Hinweis").ToString())) %>' 
                                                                    ImageUrl="/PortalZLD/images/info.gif" ToolTip='<%# Eval("Hinweis") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="AnzahlVorgaenge" HeaderText="Anzahl Vorgänge">
                                                            <HeaderStyle Width="105px"/>
                                                            <FooterStyle HorizontalAlign="Center"/>
                                                            <ItemStyle HorizontalAlign="Center"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Fahrer">
                                                            <HeaderStyle Width="270px"/>
                                                            <ItemTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlFahrer" Width="275px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="GebuehrAmt" HeaderText="Summe Gebühr">
                                                            <HeaderStyle Width="90px"/>
                                                            <FooterStyle HorizontalAlign="Right"/>
                                                            <ItemStyle HorizontalAlign="Right"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Vorschuss">
                                                            <HeaderStyle Width="70px"/>
                                                            <FooterStyle HorizontalAlign="Center"/>
                                                            <ItemStyle HorizontalAlign="Center"/>
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkVorschuss" Checked='<%# Eval("Vorschuss") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Höhe Vorschuss">
                                                            <HeaderStyle Width="100px"/>
                                                            <FooterStyle HorizontalAlign="Center"/>
                                                            <ItemStyle HorizontalAlign="Center"/>
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txtVorschussBetrag" Width="65px" Text='<%# Eval("VorschussBetrag", "{0:f2}") %>' style="text-align: right"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px" 
                            onclick="cmdSave_Click">» Speichern </asp:LinkButton>
                        <asp:LinkButton ID="cmdSend" runat="server" CssClass="Tablebutton" Width="78px"
                           onclick="cmdSend_Click">» Absenden</asp:LinkButton>      
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
