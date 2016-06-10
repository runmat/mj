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
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="5" style="vertical-align:top">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#269700"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width: 100px" >
                                            <asp:Label ID="lblDatum" runat="server">Zulassungdatum:</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="width: 150px" >
                                            <asp:TextBox ID="txtZulDate" runat="server" CssClass="TextBoxNormal" 
                                                Width="75px" MaxLength="6"></asp:TextBox>
                                            <asp:Label ID="txtZulDateFormate" Style="padding-left: 2px; font-weight: normal"
                                                Height="15px" runat="server" Width="48px">(ttmmjj)</asp:Label>                                                      
                                        </td>
                                        <td class="firstLeft active" style="width: 170px" >
                                            <asp:LinkButton ID="lbtnGestern" runat="server" Height="15px" 
                                                Style="font-weight: normal" 
                                                Text="Gestern |" Width="60px" />
                                            <asp:LinkButton ID="lbtnHeute" runat="server" Height="15px" 
                                                Style="font-weight: normal" Text="Heute |" 
                                                Width="50px" />
                                            <asp:LinkButton ID="lbtnMorgen" runat="server" Height="15px" 
                                                Style="font-weight: normal" Text="Morgen" 
                                                Width="60px" />
                                        </td>
                                        <td class="firstLeft active" style="width: 370px" >
                                            <asp:RadioButton ID="rbNichtDisponiert" runat="server" Checked="true" 
                                                GroupName="Modus" Text="nicht disponiert" AutoPostBack="True" OnCheckedChanged="rbModusChanged" />
                                            <asp:RadioButton ID="rbBereitsDisponiert" runat="server" GroupName="Modus" 
                                                style="padding-left:5px" Text="bereits disponiert" AutoPostBack="True" OnCheckedChanged="rbModusChanged" />
                                            <asp:RadioButton ID="rbBereitsInArbeit" runat="server" GroupName="Modus" 
                                                style="padding-left:5px" Text="bereits in Arbeit" AutoPostBack="True" OnCheckedChanged="rbModusChanged" />
                                        </td>
                                        <td class="firstLeft active" >
                                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px" 
                                                onclick="cmdSearch_Click">» Suchen </asp:LinkButton>
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
                                                    OnRowDataBound="gvDispositionen_RowDataBound" >
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Amt">
                                                            <HeaderStyle Width="45px"/>
                                                            <ItemStyle HorizontalAlign="Center"/>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblAmt" Text='<%# Eval("Amt") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="KreisBezeichnung" HeaderText="">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AnzahlVorgaenge" HeaderText="Anzahl Vorgänge">
                                                            <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                                            <ItemStyle HorizontalAlign="Center"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Fahrer">
                                                            <HeaderStyle Width="280px"/>
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlFahrer" Style="width: 275px" runat="server"/>
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
