<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeZLDNach.aspx.cs"
    Inherits="AppZulassungsdienst.forms.ChangeZLDNach" MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="DialogErfassungDLBezeichnung" Src="../Controls/DialogErfassungDLBezeichnung.ascx" %>
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
                    Text="Zur¸ck" OnClick="lb_zurueck_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading"><h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <asp:Panel ID="pnlBankdaten" runat="server" Style="display: none">
                                    <table cellpadding="0" runat="server" id="TableBank" cellspacing="0">
                                        <tr>
                                            <td colspan="3" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trError" runat="server">
                                            <td colspan="3" class="firstLeft active">
                                                <asp:Label ID="lblErrorBank" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblZulDateBank" runat="server">Datum der Zulassung:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="2">
                                                <asp:TextBox ID="txtZulDateBank" Enabled="false" runat="server" CssClass="TextBoxNormal"
                                                    Width="65px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="Label3" runat="server">Kunde*:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="height: 30px">
                                                <asp:TextBox ID="txtKundeBankSuche" onfocus="javascript:this.select();" Enabled="false"
                                                    runat="server" CssClass="TextBoxNormal" MaxLength="8" Width="65px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="width: 100%; height: 30px;">
                                                <asp:TextBox ID="txtKundebank" runat="server" CssClass="TextBoxNormal" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblReferenzen" runat="server">Referenzen:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="2">
                                                <asp:TextBox ID="txtRef1Bank" Enabled="false" runat="server" CssClass="TextBoxNormal"
                                                    MaxLength="20"></asp:TextBox>
                                                &nbsp;<asp:TextBox ID="txtRef2Bank" Enabled="false" runat="server" CssClass="TextBoxNormal"
                                                    MaxLength="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" class="firstLeft active" style="background-color: #dfdfdf; height: 22px;
                                                padding-left: 15px">
                                                Abweichende Anschrift
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblName" runat="server">Name:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="2" style="width: 100%;">
                                                <asp:TextBox ID="txtName1" Width="400px" CssClass="TextBoxNormal" runat="server"
                                                    MaxLength="40"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active" colspan="2" style="width: 100%; height: 30px;">
                                                <asp:TextBox ID="txtName2" Width="400px" CssClass="TextBoxNormal" runat="server"
                                                    MaxLength="40"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblStrasse" runat="server">Straﬂe:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="2" style="width: 100%;">
                                                <asp:TextBox ID="txtStrasse" Width="400px" CssClass="TextBoxNormal" runat="server"
                                                    MaxLength="60"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblPLZBank" runat="server">PLZ/Ort*:</asp:Label>
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtPlz" runat="server"  onKeyPress="return numbersonly(event, false)" CssClass="TextBoxNormal" MaxLength="5" Width="65px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="width: 100%;">
                                                <asp:TextBox ID="txtOrt" Width="400px" CssClass="TextBoxNormal" runat="server" MaxLength="40"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                            </td>
                                            <td class="firstLeft active" colspan="2" style="width: 100%;">
                                                    <asp:CheckBox ID="chkEinzug" runat="server" Text="Einzugserm‰chtigung" />
                                                    <asp:CheckBox ID="chkRechnung" runat="server" Text="Rechnung" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="3">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" class="firstLeft active" style="background-color: #dfdfdf; height: 22px;
                                                padding-left: 15px">
                                                Bankverbindung
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblKontoinhaber" runat="server">Kontoinhaber:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="2" style="width: 100%;">
                                                <asp:TextBox ID="txtKontoinhaber" Width="400px" CssClass="TextBoxNormal" runat="server"
                                                    MaxLength="40"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblIBAN" runat="server">IBAN:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="2" style="width: 100%;">
                                                <asp:TextBox ID="txtIBAN" Width="400px" CssClass="TextBoxNormal TextUpperCase" runat="server"
                                                    MaxLength="34"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblSWIFT" runat="server">SWIFT-BIC:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="2" style="width: 100%;">
                                                <asp:TextBox ID="txtSWIFT" Width="400px" CssClass="TextBoxNormal" runat="server"
                                                    MaxLength="11" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblGeldinstitut" runat="server">Geldinstitut:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="2" style="width: 100%;">
                                                <asp:TextBox ID="txtGeldinstitut" Width="400px" CssClass="TextBoxNormal" runat="server"
                                                    MaxLength="40" Text="Wird automatisch gef¸llt!" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="3">
                                                &nbsp;
                                                <asp:HiddenField runat="server" ID="hfBankleitzahl"/>
                                                <asp:HiddenField runat="server" ID="hfKontonummer"/>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="divButtons" class="dataQueryFooter" style="display: block">
                                        <asp:LinkButton ID="cmdSaveBank" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                            Height="22px" OnClick="cmdSaveBank_Click">ª Speichern/Erfassung </asp:LinkButton>
                                        <asp:LinkButton ID="cmdCancelBank" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                            Height="22px" OnClick="cmdCancelBank_Click">ª Abbrechen</asp:LinkButton>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="Panel1" runat="server">
                                    <table id="tab1" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td colspan="4" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="4">
                                                    <asp:Label ID="lblPflichtfelder" runat="server" Height="16px">* Pflichtfelder</asp:Label>
                                                    <asp:HiddenField ID="hfKunnr" runat="server" /><asp:HiddenField ID="hfMatnr" runat="server" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKunde0" runat="server" Height="16px">DAD-Auftrag: Barcode</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:TextBox ID="txtBarcode" runat="server" CssClass="TextBoxNormal" Width="75px"
                                                        Enabled="False"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="fteBarcode"
                                                        TargetControlID="txtBarcode" InvalidChars="|}" FilterMode="InvalidChars" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKunde" runat="server">Kunde*:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtKunnr" runat="server" CssClass="TextBoxNormal" MaxLength="8"
                                                        Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active" style="width: 100%; vertical-align: top; margin-top: 3px" colspan="2">
                                                    <asp:DropDownList ID="ddlKunnr" runat="server" Style="width: auto; position: absolute;" EnableViewState="False">
                                                    </asp:DropDownList>
                                                    <label style="float: right; padding-right: 35px" runat="server" id="Pauschal" title="">
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblPLZ" runat="server">Referenzen:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                    <asp:TextBox ID="txtReferenz1" runat="server" CssClass="TextBoxNormal" MaxLength="20"
                                                        Width="230px"  style="text-transform:uppercase;"></asp:TextBox>
                                                    &nbsp;<asp:TextBox ID="txtReferenz2" runat="server" CssClass="TextBoxNormal" MaxLength="20"
                                                        Width="230px"  style="text-transform:uppercase;"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="vertical-align: top;">
                                                    <div style="padding-top: 25px" />
                                                    <div>
                                                        <asp:Label ID="Label1" runat="server">Dienstleistungen:</asp:Label>
                                                    </div>
                                                    <div style="vertical-align: top; padding-top: 25px">
                                                        <asp:Label ID="Label2" runat="server">Weitere Artikel:</asp:Label>
                                                    </div>
                                                </td>
                                                <td colspan="3" style="vertical-align: top; width: 100%;">
                                                    <asp:GridView ID="GridView1" Style="border: none;" runat="server" AutoGenerateColumns="False"
                                                        ShowHeader="true" AllowSorting="false" OnRowCommand="GridView1_RowCommand" OnDataBound="GridView1_DataBound">
                                                        <HeaderStyle />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtSearch" CssClass="TextBoxNormal" runat="server" Width="55px"/>
                                                                    <asp:Label ID="lblID_POS" runat="server" Text='<%# Eval("ID_POS") %>' style="display:none"/>
                                                                    <asp:Label ID="lblOldMatnr" runat="server" Text='<%# Eval("OldValue") %>' style="display:none"/>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" Width="80px" CssClass="firstLeft active"/>
                                                                <HeaderStyle BorderStyle="None" BorderColor="#ffffff" BorderWidth="0px"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlItems" Style="width: 325px" runat="server"/>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" CssClass="active"/>
                                                                <HeaderStyle BorderStyle="None" CssClass="active" BorderColor="#ffffff" BorderWidth="0px"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblMenge" runat="server" Text="Stk."/>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtMenge" CssClass="TextBoxNormal" runat="server" Width="30px" onKeyPress="return numbersonly(event, true)"
                                                                                 Text='<%# Eval("Menge") %>'/>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" CssClass="TablePadding" Width="55px" />
                                                                <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="GridDetail active"
                                                                    BorderColor="#ffffff" BorderWidth="0px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField  HeaderText="Preis">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPreis" Width="55px" onKeyPress="return numbersonly(event, true)" Text='<%# Eval("Preis", "{0:F}") %>' 
                                                                        CssClass="TextBoxNormal" runat="server" Enabled='<%# proofBlTypOKPreisEditable(Eval("ID_POS").ToString()) %>'/>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" CssClass="TablePadding"/>
                                                                <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="GridDetail active"  BorderColor="#ffffff" BorderWidth="0px"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Geb¸hr">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGebPreis"  Visible='<%# proofGebMat(Eval("Value").ToString()) %>'  
                                                                                 Enabled='<%# proofGebPak(Eval("ID_POS").ToString()) %>' 
                                                                                 onKeyPress="return numbersonly(event, true)" Width="55px"  Text='<%# Eval("GebPreis", "{0:F}") %>' CssClass="TextBoxNormal" runat="server"/>
                                                                </ItemTemplate>
                                                               <ItemStyle BorderStyle="None" CssClass="TablePadding"/>
                                                               <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="GridDetail active" BorderColor="#ffffff" BorderWidth="0px"/>
                                                            </asp:TemplateField>   
                                                            <asp:TemplateField HeaderText="Geb¸hr Amt">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGebAmt" Visible='<%# proofGebMat(Eval("Value").ToString()) %>'
                                                                                 onKeyPress="return numbersonly(event, true)" Width="55px" Text='<%# Eval("GebAmt", "{0:F}") %>'
                                                                                 CssClass="TextBoxNormal" runat="server"/>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" CssClass="TablePadding" />
                                                                <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="TablePadding"
                                                                    BorderColor="#ffffff" BorderWidth="0px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField >
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtnDel" CommandName="Del" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                                                        Visible='<%# ((GridViewRow)Container).RowIndex > 0 %>' ImageUrl="/PortalZLD/Images/RecycleBin.png"
                                                                        TabIndex="-1" Height="16px" Width="16px" runat="server" />
                                                                </ItemTemplate>
                                                              <ItemStyle BorderStyle="None"  CssClass="GridDetail active" />
                                                              <HeaderStyle BorderStyle="None" CssClass="GridDetail active" BorderColor="#ffffff" BorderWidth="0px"/>
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
                                                <td class="firstLeft active" colspan="3">
                                                    <div style="float: left">
                                                        <asp:LinkButton ID="cmdCreate1" runat="server" CssClass="TablebuttonSmall" OnClick="cmdCreate1_Click"
                                                            Style="font-size: 12px;" Width="50px">+</asp:LinkButton>
                                                        Zum Erfassen weiterer Artikel
                                                    </div>
                                                    <div style="float: right; padding-right: 263px">
                                                        Kennzeichen(Netto/Stk.):&nbsp;
                                                        <asp:TextBox ID="txtPreisKennz" runat="server" CssClass="TextBoxNormal" onKeyPress="return numbersonly(event, true)"
                                                            Width="60px"></asp:TextBox>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                </td>
                                                <td class="active" colspan="3">
                                                    <div style="padding-left: 372px">
                                                        Steuern:&nbsp;
                                                        <asp:TextBox ID="txtSteuer" runat="server" CssClass="TextBoxNormal" onKeyPress="return numbersonly(event, true)"
                                                            Width="60px"></asp:TextBox>
                                                        <asp:CheckBox ID="chkBar" runat="server" Text="Barkunde" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblStva" runat="server">StVA*:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtStVa" runat="server" CssClass="TextBoxNormal" MaxLength="8" Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="active" style="width: 100%;">
                                                    <asp:DropDownList ID="ddlStVa" runat="server" Style="width: 375px" EnableViewState="False" 
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlStVa_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="firstLeft active" style="width: 100%;">
                                                    <asp:LinkButton ID="lbtnReservierung" runat="server" CssClass="TablebuttonMiddle"
                                                        Width="100px" Height="16px" OnClick="lbtnReservierung_Click">ª Reservierung</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:CheckBox ID="chkWunschKZ" runat="server" Text="Wunsch-Kennzeichen" />
                                                    <span style="padding-right: 3px"></span>
                                                    <asp:CheckBox ID="chkReserviert" runat="server" Text="Reserviert, Nr" />
                                                    <span style="padding-right: 3px"></span>
                                                    <asp:TextBox ID="txtNrReserviert" runat="server" CssClass="TextBoxNormal" Width="195px"
                                                        Enabled="False" EnableTheming="True" MaxLength="10"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:LinkButton runat="server" ID="lbtnFeinstaub" CssClass="TablebuttonXSmall" Width="20px" Height="16px" Text="+" OnClick="lbtnFeinstaub_Click"></asp:LinkButton>
                                                    &nbsp;&nbsp;Feinstaubplakette vom Amt
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblDatum" runat="server">Datum der Zulassung:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:TextBox ID="txtZulDate" runat="server" CssClass="TextBoxNormal" Width="65px"
                                                        EnableTheming="True" MaxLength="6"></asp:TextBox>
                                                    <asp:Label ID="txtZulDateFormate" Style="padding-left: 2px; font-weight: normal"
                                                        Height="15px" runat="server">(ttmmjj)</asp:Label>
                                                    <asp:LinkButton runat="server" Visible="false" Style="padding-left: 10px; font-weight: normal"
                                                        Height="15px" ID="lbtnGestern" Text="Gestern |" Width="60px" />
                                                    <asp:LinkButton runat="server" Visible="false" Style="font-weight: normal" Height="15px"
                                                        ID="lbtnHeute" Width="50px" Text="Heute |" />
                                                    <asp:LinkButton runat="server" Visible="false" Style="font-weight: normal" Height="15px"
                                                        ID="lbtnMorgen" Width="60px" Text="Morgen" />
                                                    <asp:CheckBox ID="chkFlieger" Text="Flieger" runat="server" />

                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKennz" runat="server">Kennzeichen:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:TextBox ID="txtKennz1" MaxLength="3" CssClass="TextBoxNormal" Width="30px" runat="server"  style="text-transform:uppercase;"></asp:TextBox>
                                                    <span style="padding-right: 2px; padding-left: 2px">-</span>
                                                    <asp:TextBox ID="txtKennz2" MaxLength="6" CssClass="TextBoxNormal" Width="100px"
                                                        runat="server" style="text-transform:uppercase;"></asp:TextBox>
                                                    <span style="padding-right: 2px;"></span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                </td>
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:CheckBox ID="chkEinKennz" runat="server" Text="Nur ein Kennzeichen"></asp:CheckBox><span
                                                        style="padding-right: 3px"></span>
                                                    <asp:CheckBox ID="chkKennzSonder" runat="server" Text="Kennzeichen-Sondergˆﬂe" OnCheckedChanged="chkKennzSonder_CheckedChanged"
                                                        AutoPostBack="true"></asp:CheckBox>
                                                    <span style="padding-right: 3px"></span>
                                                    <asp:DropDownList ID="ddlKennzForm" runat="server" Enabled="false" Style="width: 150px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblBemerk" runat="server">Bemerkung:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:TextBox ID="txtBemerk" runat="server" CssClass="TextBoxNormal" MaxLength="120"
                                                        Width="465px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblInfotext" runat="server">Infotext:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:TextBox ID="txtInfotext" runat="server" CssClass="TextBoxNormal" 
                                                        Width="465px" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trFreitext" runat="server" >
                                                <td class="firstLeft active" >
                                                   <div style="margin-bottom:105px"> <asp:Label ID="lblFreitext"  runat="server">Freitext Autohaus:</asp:Label></div>
                                                </td>
                                                <td class="firstLeft active" colspan="3">
                                                <asp:TextBox ID="txtService" TextMode="MultiLine" 
                                                style="width: 600px; height: 120px;" 
                                                 runat="server" ReadOnly="true"  ></asp:TextBox>
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
                            <div id="dataQueryFooter" runat="server" class="dataQueryFooter">
                                <asp:LinkButton ID="cmdNewDLPrice" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px" Style="padding-right: 5px" onclick="cmdNewDLPrice_Click">ª Preis erg‰nzte DL </asp:LinkButton>
                                <asp:LinkButton ID="cmdFindPrize" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px" Style="padding-right: 5px" OnClick="cmdFindPrize_Click">ª Preis finden </asp:LinkButton>
                                <asp:LinkButton ID="lbtnBank" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px" Style="padding-right: 5px" OnClick="lbtnBank_Click">ª Bankdaten/Adresse </asp:LinkButton>
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px" Style="padding-right: 5px" OnClick="cmdCreate_Click" OnClientClick="checkZulassungsdatum();">ª Speichern/Liste </asp:LinkButton>
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
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
