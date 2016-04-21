<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeZLDVorVersand.aspx.cs"
    Inherits="AppZulassungsdienst.forms.ChangeZLDVorVersand" MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="DialogErfassungDLBezeichnung" Src="../Controls/DialogErfassungDLBezeichnung.ascx" %>
<%@ Register TagPrefix="uc" TagName="BankdatenAdresse" Src="../Controls/BankdatenAdresse.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?26082013"></script>
    <script language="javascript" type="text/javascript">
        function checkZulassungsdatum() {
            var tb = document.getElementById('<%= txtZulDate.ClientID %>');
            document.getElementById('<%= ihDatumIstWerktag.ClientID %>').value = nurWerktage(tb.value)[0];
            return true;
        }
        function pageLoad() {
            var mpe = $find('bDLBezeichnung');
            if (mpe != null) {
                mpe.add_shown(function () {
                    SetDLBezeichnung("");
                });
            }
        }
    </script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenuInput">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
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
                            <asp:PostBackTrigger ControlID="lbtnReservierung" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <asp:Panel ID="pnlBankdaten" runat="server" style="display:none">
                                    <uc:BankdatenAdresse runat="server" ID="ucBankdatenAdresse" />
                                    <div id="divButtons" class="dataQueryFooter" style="display: block">
                                        <asp:LinkButton ID="cmdSaveBank" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                            Height="22px" OnClick="cmdSaveBank_Click">» Speichern/Erfassung </asp:LinkButton>
                                        <asp:LinkButton ID="cmdCancelBank" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                            Height="22px" OnClick="cmdCancelBank_Click">» Abbrechen</asp:LinkButton>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="Panel1" runat="server">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td colspan="4" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="4">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblPflichtfelder" runat="server" Height="16px">* Pflichtfelder</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2" style="width: 100%;">
                                                    &nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="height: 30px">
                                                    <asp:Label ID="lblKunde" runat="server">Kunde*:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" style="height: 30px">
                                                    <asp:TextBox ID="txtKunnr" runat="server" CssClass="TextBoxNormal" MaxLength="8"
                                                        Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active" colspan="2" style="width: 100%; vertical-align: top;
                                                    padding-top: 14px">
                                                    <asp:DropDownList ID="ddlKunnr" runat="server" Style="width: auto; position: absolute;" EnableViewState="False">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblPLZ" runat="server">Referenzen:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:TextBox ID="txtReferenz1" runat="server" CssClass="TextBoxNormal" MaxLength="20"
                                                        Width="230px" style="text-transform:uppercase;"></asp:TextBox>
                                                    &nbsp;<asp:TextBox ID="txtReferenz2" runat="server" CssClass="TextBoxNormal" MaxLength="20"
                                                        Width="230px" style="text-transform:uppercase;" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="vertical-align:top;" >
                                                <div  style="padding-top:25px" />

                                                    <div>
                                                        <asp:Label ID="Label2" runat="server">Dienstleistungen:</asp:Label>
                                                    </div>
                                                    <div style="vertical-align:top; padding-top:25px">
                                                        <asp:Label ID="Label3" runat="server">Weitere Artikel:</asp:Label>
                                                    </div>

                                                </td>
                                                <td colspan="3" style="vertical-align:top;" > 
                                                    <asp:GridView ID="GridView1" style="border: none 0px #ffffff" runat="server" AutoGenerateColumns="False"
                                                        ShowHeader="True" OnRowCommand="GridView1_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID_POS" runat="server" Text='<%# Eval("ID_POS") %>'/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtSearch" CssClass="TextBoxNormal" runat="server" Width="75px"/>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" CssClass="firstLeft active" Width="75px" />
                                                                <HeaderStyle BorderStyle="None" BorderColor="#ffffff" BorderWidth="0px"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlItems" Style="width: 375px"  runat="server"/>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" CssClass="firstLeft active" Width="375px" />
                                                                <HeaderStyle BorderStyle="None" BorderColor="#ffffff" BorderWidth="0px"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblMenge" runat="server" Text="Stk."/>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtMenge" CssClass="TextBoxNormal" runat="server" Width="40px" 
                                                                                 onKeyPress="return numbersonly(event, true)" Text='<%# Eval("Menge") %>'/>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" CssClass="TablePadding" Width="55px"/>
                                                                <HeaderStyle BorderStyle="None" BorderColor="#ffffff" BorderWidth="0px"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-BorderStyle="None" ItemStyle-Width="380px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtnDel" CommandName="Del" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                                                        Visible='<%# ((GridViewRow)Container).RowIndex > 0 %>' ImageUrl="/PortalZLD/Images/RecycleBin.png"
                                                                        TabIndex="-1" Height="16px" Width="16px" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" CssClass="TablePadding" />
                                                                <HeaderStyle BorderStyle="None" BorderColor="#ffffff" BorderWidth="0px"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDLBezeichnung" runat="server" Text='<%# Eval("DLBezeichnung") %>'/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>

                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active" style="padding-left: 12px">
                                                    <asp:LinkButton ID="cmdCreate1" runat="server" CssClass="TablebuttonSmall" OnClick="cmdCreate1_Click"
                                                        Style="font-size: 12px;" Width="50px">+</asp:LinkButton>
                                                </td>
                                                <td colspan="2" style="width: 100%">
                                                    Zum Erfassen weiterer Artikel
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblStva" runat="server">StVA*:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtStVa" runat="server" CssClass="TextBoxNormal" MaxLength="8" Width="65px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:DropDownList ID="ddlStVa" runat="server" Style="width: 375px" AutoPostBack="false" EnableViewState="False">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="firstLeft active" style="width: 100%;">
                                                    <asp:LinkButton ID="lbtnReservierung" runat="server" CssClass="TablebuttonMiddle"
                                                        Width="100px" Height="16px" OnClick="lbtnReservierung_Click">» Reservierung</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active" colspan="3" style="width: 100%">
                                                    <asp:CheckBox ID="chkWunschKZ" runat="server" Text="Wunsch-Kennzeichen" />
                                                    <span style="padding-right: 3px"></span>
                                                    <asp:CheckBox ID="chkReserviert" runat="server" Text="Reserviert, Nr" />
                                                    <span style="padding-right: 3px"></span>
                                                    <asp:TextBox ID="txtNrReserviert" runat="server" CssClass="TextBoxNormal" Width="195px"
                                                        MaxLength="10"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active" colspan="3" style="width: 100%">
                                                    <asp:LinkButton runat="server" ID="lbtnFeinstaub" CssClass="TablebuttonXSmall" Width="20px" Height="16px" Text="+" OnClick="lbtnFeinstaub_Click"></asp:LinkButton>
                                                    &nbsp;&nbsp;Feinstaubplakette vom Amt
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="height: 36px">
                                                    <asp:Label ID="lblDatum" runat="server">Datum der Zulassung:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3" style="height: 36px">
                                                    <asp:TextBox ID="txtZulDate" runat="server" CssClass="TextBoxNormal" Width="65px"
                                                        MaxLength="6"></asp:TextBox>
                                                    <asp:Label ID="txtZulDateFormate" Style="padding-left: 2px; font-weight: normal"
                                                        Height="15px" runat="server">(ttmmjj)</asp:Label>
                                                    <asp:LinkButton runat="server" Style="padding-left: 10px; font-weight: normal" Height="15px"
                                                        ID="lbtnGestern" Text="Gestern |" Width="60px" />
                                                    <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnHeute"
                                                        Width="50px" Text="Heute |" />
                                                    <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnMorgen"
                                                        Width="60px" Text="Morgen" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKennz" runat="server">Kennzeichen:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:TextBox ID="txtKennz1" MaxLength="3" CssClass="TextBoxNormal" Width="30px" runat="server"
                                                       style="text-transform:uppercase;"></asp:TextBox>
                                                    <span style="padding-right: 2px; padding-left: 2px">-</span>
                                                    <asp:TextBox ID="txtKennz2" MaxLength="6" CssClass="TextBoxNormal" Width="100px"
                                                        runat="server" style="text-transform:uppercase;"></asp:TextBox>
                                                    <span style="padding-right: 2px;"></span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                </td>
                                                <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                    <asp:CheckBox ID="chkEinKennz" runat="server" Text="Nur ein Kennzeichen"></asp:CheckBox>
                                                    <span style="padding-right: 3px"></span>
                                                    <asp:CheckBox ID="chkKennzSonder" runat="server" Text="Kennzeichen-Sondergöße" AutoPostBack="True"
                                                        OnCheckedChanged="chkKennzSonder_CheckedChanged"></asp:CheckBox>
                                                    <span style="padding-right: 3px"></span>
                                                    <asp:DropDownList ID="ddlKennzForm" runat="server" Enabled="false" Style="width: 150px;">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblBemerk" runat="server">Bemerkung:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                    <asp:TextBox ID="txtBemerk" runat="server" CssClass="TextBoxNormal" MaxLength="120"
                                                        Width="465px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    </div>
                                </asp:Panel>
                            </div>
                            <div id="ButtonFooter" runat="server" class="dataQueryFooter">
                                <asp:LinkButton ID="lbtnBank" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px" Style="padding-right: 5px" OnClick="lbtnBank_Click">» Bankdaten/Adresse </asp:LinkButton>
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px" Style="padding-right: 5px" OnClick="cmdCreate_Click" OnClientClick="checkZulassungsdatum();">» Speichern </asp:LinkButton>
                            </div>
                            <input type="hidden" runat="server" id="ihDatumIstWerktag" value="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                    <ajaxToolkit:ModalPopupExtender ID="mpeDLBezeichnung" runat="server" TargetControlID="btnFake"
                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" X="600" Y="400" BehaviorID="bDLBezeichnung" >
                    </ajaxToolkit:ModalPopupExtender>           
                    <asp:Panel ID="mb" runat="server" Width="385px" Height="140px" 
                        BackColor="White" style="display:none">
                        <uc2:DialogErfassungDLBezeichnung ID="dlgErfassungDLBez" runat="server" 
                        OnTexteingabeBestaetigt="dlgErfassungDLBez_TexteingabeBestaetigt" />                               
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
