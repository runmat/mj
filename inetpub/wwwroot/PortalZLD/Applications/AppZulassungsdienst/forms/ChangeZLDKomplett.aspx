<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeZLDKomplett.aspx.cs" Inherits="AppZulassungsdienst.forms.ChangeZLDKomplett" MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="DialogErfassungDLBezeichnung" Src="../Controls/DialogErfassungDLBezeichnung.ascx" %>
<%@ Register TagPrefix="uc" TagName="BankdatenAdresse" Src="../Controls/BankdatenAdresse.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
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
                                            Height="22px" onclick="cmdSaveBank_Click" >» Speichern/Erfassung </asp:LinkButton>
                                        <asp:LinkButton ID="cmdCancelBank" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                            Height="22px" onclick="cmdCancelBank_Click" >» Abbrechen</asp:LinkButton>
                                                    
                                    </div>                                     
                                </asp:Panel>                                   
                                <asp:Panel ID="Panel1" runat="server" >
                                    <table id="tab1" cellpadding="0" cellspacing="0">
                                        <tbody>
                                        <tr>
                                            <td colspan="3" 
                                                style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="true"></asp:Label>         
                                            </td>
                                        </tr>           
                                                                 
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:Label ID="lblPflichtfelder" runat="server" Height="16px">* Pflichtfelder</asp:Label>
                                                    <asp:HiddenField ID="hfKunnr" runat="server" /><asp:HiddenField ID="hfMatnr" runat="server" />
                                                </td>

                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="lblKunde0" runat="server" Height="16px">DAD-Auftrag: Barcode</asp:Label>
                                                </td>
                                                <td class="firstLeft active" >
                                                    <asp:TextBox ID="txtBarcode" runat="server" CssClass="TextBoxNormal" 
                                                        Width="75px" TabIndex="1"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="fteBarcode" 
                                                        TargetControlID="txtBarcode" InvalidChars="|}" FilterMode="InvalidChars" />
                                                </td>
                                                <td class="firstLeft active" style="width:100%" >
                                                    <asp:LinkButton ID="cmdGetData" runat="server" CssClass="TablebuttonMiddle" 
                                                        Height="16px" Width="100px" onclick="cmdGetData_Click" >» Daten holen</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKunde" runat="server">Kunde*:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtKunnr" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="8" Width="75px" TabIndex="2"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active" style="width: 100%; vertical-align:top; margin-top:3px">
                                                    <asp:DropDownList ID="ddlKunnr" runat="server" EnableViewState="False" 
                                                         Style="width: auto; position:absolute;" TabIndex="3">
                                                    </asp:DropDownList>
                                                    <label  style="float:right; padding-right:35px" runat="server" id="Pauschal" title=""></label>
                                                   
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblPLZ" runat="server">Referenzen:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:TextBox ID="txtReferenz1" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="20" Width="230px" TabIndex="4"  style="text-transform:uppercase;"></asp:TextBox>
                                                    &nbsp;<asp:TextBox ID="txtReferenz2" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="20" Width="230px" TabIndex="5"  style="text-transform:uppercase;"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="vertical-align:top;" >
                                                <div  style="padding-top:25px" />

                                                    <div>
                                                        <asp:Label ID="Label1" runat="server">Dienstleistungen:</asp:Label>
                                                    </div>
                                                    <div style="vertical-align:top; padding-top:25px">
                                                        <asp:Label ID="Label2" runat="server">Weitere Artikel:</asp:Label>
                                                    </div>

                                                </td>
                                                <td colspan="2" style="vertical-align:top;" > 
                                                    <asp:GridView ID="GridView1" style="border: none;" runat="server" AutoGenerateColumns="False"
                                                        ShowHeader="true" onrowcommand="GridView1_RowCommand" Width="100%" >
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
                                                                        CssClass="TextBoxNormal" runat="server"/>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" CssClass="TablePadding"/>
                                                                <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="GridDetail active"  BorderColor="#ffffff" BorderWidth="0px"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Gebühr">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGebPreis"  Visible='<%# proofGebMat(Eval("Value").ToString()) %>'
                                                                                 Enabled='<%# proofGebPak(Eval("ID_POS").ToString()) %>' 
                                                                                 onKeyPress="return numbersonly(event, true)" Width="55px"  Text='<%# Eval("GebPreis", "{0:F}") %>' CssClass="TextBoxNormal" runat="server"/>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" CssClass="TablePadding"/>
                                                                <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="GridDetail active" BorderColor="#ffffff" BorderWidth="0px"/>
                                                            </asp:TemplateField>   
                                                            <asp:TemplateField HeaderText="Gebühr Amt">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGebAmt" Visible='<%# proofGebMat(Eval("Value").ToString()) %>' 
                                                                                 onKeyPress="return numbersonly(event, true)" Width="55px" Text='<%# Eval("GebAmt", "{0:F}") %>'
                                                                                 CssClass="TextBoxNormal" runat="server"/>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderStyle="None" CssClass="TablePadding" />
                                                                <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="GridDetail active"
                                                                    BorderColor="#ffffff" BorderWidth="0px" />
                                                            </asp:TemplateField>                                                                                                                                                                                 
                                                            <asp:TemplateField >
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtnDel" CommandName="Del" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                                                        Visible='<%# ((GridViewRow)Container).RowIndex > 0 %>' ImageUrl="/PortalZLD/Images/RecycleBin.png"
                                                                        TabIndex="-1" Height="16px" Width="16px" runat="server" />
                                                                </ItemTemplate>
                                                              <ItemStyle BorderStyle="None"  CssClass="TablePadding" />
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
                                                <td class="firstLeft active"  >
                                                   &nbsp; </td>
                                                <td class="firstLeft active" colspan="2" >
                                                <div style="float:left">
                                                <asp:LinkButton ID="cmdCreate1" runat="server" CssClass="TablebuttonSmall" 
                                                             OnClick="cmdCreate1_Click" Style="font-size: 12px;" Width="50px">+</asp:LinkButton>
                                                    <asp:Label ID="lblArtikel"  runat="server" Text="Label">Zum Erfassen weiterer Artikel</asp:Label>
                                                     </div>
                                                    <div style="padding-left:246px">
                                                        <asp:Label ID="lblPreisKennz" Visible = "false" Height="15px" runat="server" Text="Label">Kennzeichen(Netto/Stk.):&nbsp;</asp:Label> 
                                                        <asp:TextBox ID="txtPreisKennz" Visible = "false" runat="server" CssClass="TextBoxNormal" 
                                                           onKeyPress="return numbersonly(event, true)" Width="55px"></asp:TextBox>
                                                         
                                                    </div>
                                                </td>

                                            </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                    </td>
                                                    <td class="active" colspan="2">
                                                        <div style="padding-left:360px">
                                                            <asp:Label ID="lblSteuer" runat="server" Height="15px" Text="" Visible="false">Steuern:&nbsp;</asp:Label>
                                                            <asp:TextBox ID="txtSteuer" runat="server" CssClass="TextBoxNormal" 
                                                                onKeyPress="return numbersonly(event, true)" Visible="false" Width="55px"></asp:TextBox>
                                                        </div>
                                                    </td>
                                            </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        &nbsp;</td>
                                                    <td class="firstLeft active" colspan="2">
                                                        <asp:RadioButton ID="rbECGeb" runat="server" Checked="true" 
                                                            GroupName="Bezahlung" Text="Gebühr bezahlt per EC" />
                                                        <asp:RadioButton ID="rbECBar" runat="server" GroupName="Bezahlung" 
                                                            style="padding-left:5px" Text="Gebühr bezahlt Bar" />
                                                        <asp:RadioButton ID="rbRE" runat="server" GroupName="Bezahlung" 
                                                            style="padding-left:5px" Text="Gebühr per RE" />
                                                        <asp:CheckBox ID="chkBar" runat="server" Text="Barkunde" />
                                                    </td>
                                                </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                    <asp:Label ID="lblStva" runat="server">StVA*:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" >
                                                    <asp:TextBox ID="txtStVa" runat="server" CssClass="TextBoxNormal" MaxLength="8" 
                                                        Width="65px" TabIndex="7" ></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active" >
                                                    <asp:DropDownList ID="ddlStVa" runat="server"  
                                                         Style="width: 375px" TabIndex="8" EnableViewState="False">
                                                    </asp:DropDownList>
                                                    <asp:LinkButton ID="lbtnReservierung" runat="server" 
                                                        CssClass="TablebuttonMiddle" Height="18px" OnClick="lbtnReservierung_Click" 
                                                        style="margin-left:15px" Width="100px">» Reservierung</asp:LinkButton>
                                                </td>

                                            </tr>                                                
                                            <tr class="formquery">
                                                <td class="firstLeft active"  >
                                                </td>
                                                <td class="firstLeft active" colspan="2"  >
                                                        <asp:CheckBox ID="chkWunschKZ" runat="server" Text="Wunsch-Kennzeichen" />
                                                        <span style="padding-right: 3px"></span>
                                                        <asp:CheckBox ID="chkReserviert" runat="server" Text="Reserviert, Nr" />
                                                        <span style="padding-right: 3px"></span>
                                                        <asp:TextBox ID="txtNrReserviert" runat="server" CssClass="TextBoxNormal" 
                                                            EnableTheming="True" Width="195px" MaxLength="10"></asp:TextBox>
                                                </td>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                </td>
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:LinkButton runat="server" ID="lbtnFeinstaub" CssClass="TablebuttonXSmall" Width="20px" Height="16px" Text="+" OnClick="lbtnFeinstaub_Click"></asp:LinkButton>
                                                    &nbsp;&nbsp;Feinstaubplakette vom Amt
                                                </td>

                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblDatum" runat="server">Datum der Zulassung:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:TextBox ID="txtZulDate" runat="server"  CssClass="TextBoxNormal" 
                                                        Width="65px" MaxLength="6" TabIndex="9"></asp:TextBox>
                                                    <asp:Label ID="txtZulDateFormate" runat="server" Height="15px" 
                                                        Style="padding-left: 2px; font-weight: normal">(ttmmjj)</asp:Label>
                                                    <asp:LinkButton ID="lbtnGestern" runat="server" Height="15px" 
                                                        Style="padding-left: 10px; font-weight: normal" 
                                                        Text="Gestern |" Width="60px" />
                                                    <asp:LinkButton ID="lbtnHeute" runat="server" Height="15px" 
                                                        Style="font-weight: normal" Text="Heute |" 
                                                        Width="50px" />
                                                    <asp:LinkButton ID="lbtnMorgen" runat="server" Height="15px" 
                                                         Style="font-weight: normal" Text="Morgen" 
                                                        Width="60px" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKennz" runat="server">Kennzeichen:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2" >
                                                    <asp:TextBox ID="txtKennz1" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="3" Width="30px" TabIndex="10"  style="text-transform:uppercase;"></asp:TextBox>
                                                    <span style="padding-right: 2px; padding-left: 2px">-</span>
                                                    <asp:TextBox ID="txtKennz2" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="6" Width="100px" TabIndex="11" style="text-transform:uppercase;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                </td>
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:CheckBox ID="chkEinKennz" runat="server" Text="Nur ein Kennzeichen" />
                                                    <span style="padding-right: 3px"></span>
                                                    <asp:CheckBox ID="chkKennzSonder" runat="server" AutoPostBack="True" 
                                                        oncheckedchanged="chkKennzSonder_CheckedChanged" 
                                                        Text="Kennzeichen-Sondergöße" />
                                                    <span style="padding-right: 3px"></span>
                                                    <asp:DropDownList ID="ddlKennzForm" runat="server" Enabled="false" 
                                                        Style="width: 150px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblBemerk" runat="server">Bemerkung:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:TextBox ID="txtBemerk" MaxLength="120" CssClass="TextBoxNormal" 
                                                        Width="465px" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="3">
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
                                <asp:LinkButton ID="cmdNewDLPrice" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px" Style="padding-right: 5px" onclick="cmdNewDLPrice_Click">» Preis ergänzte DL </asp:LinkButton>
                                <asp:LinkButton 
                                            ID="cmdFindPrize" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px"  style="padding-right:5px" onclick="cmdFindPrize_Click" 
                                    TabIndex="12"  >» Preis finden </asp:LinkButton>                            
                                <asp:LinkButton ID="lbtnBank" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px"  style="padding-right:5px" onclick="lbtnBank_Click" TabIndex="13" 
                                     >» Bankdaten/Adresse </asp:LinkButton>
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px" style="padding-right:5px" onclick="cmdCreate_Click" OnClientClick="checkZulassungsdatum();" 
                                    TabIndex="14" >» Speichern/Neu </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px" onclick="LinkButton1_Click">» Eingabeliste </asp:LinkButton>
                           
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
