<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change08.aspx.vb" Inherits="KBS.Change08"
    MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <script type='text/javascript'>
            function CreateHandler()
            {
                $addHandler($get('ctl00_ContentPlaceHolder1_rbEinzugJa'), "onclick", ShowEinzug);
            }
            
            function ShowEinzug(){
                if ($get('ctl00_ContentPlaceHolder1_rbEinzugJa').checked && $get('ctl00_ContentPlaceHolder1_rbLieferscheinKunde').checked) {
                        window.open(EinzugPath,"Einzugsermächtigung");
                    } 
                }
        </script>
        <div id="site" >
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label">Neukundenanlage</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel runat="server" ID="upEingabe">
                            <Triggers>
                                <asp:PostBackTrigger ControlID = "lbParken" />
                                <asp:PostBackTrigger ControlID = "lbAusparken" />
                                <asp:PostBackTrigger ControlID = "lbAbsenden" />
                            </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <div class="paginationQuery">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tbody>
                                                        <tr>
                                                            <td class="firstLeft active" style="font-size: 12px">
                                                                <asp:Label ID="lblNoData" runat="server" Text="Bitte füllen Sie alle Pflichtfelder(*) aus!"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="border: none">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 140px">
                                                        Mitarbeiter/in*:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtMitarbeiter" TextMode="Password" runat="server" MaxLength="15" TabIndex="1"></asp:TextBox>
                                                        <asp:HiddenField ID="HiddenMitarbeiter" runat="server" />
                                                    </td>
                                                    <td class="firstLeft active" style="width: 60%">
                                                        <asp:LinkButton ID="lb_Neu" Visible="false" Text="Neuer Kunde" Height="16px" Width="78px"
                                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="paginationQuery">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tbody>
                                                        <tr>
                                                            <td class="firstLeft active" style="font-size: 12px">
                                                                Anlagetyp
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="border: none">
                                                <tr class="formRadioButton">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px">
                                                        Typ*:
                                                    </td>
                                                    <td class="firstLeft active" style="font-size: 12px; width: 100px">
                                                        <span>
                                                            <asp:RadioButton ID="rbBarkunde" GroupName="Kunde" Text="Barkunde" runat="server"
                                                                TabIndex="2" AutoPostBack="True" Checked="True" />
                                                        </span>
                                                    </td>
                                                    <td class="firstLeft active" style="font-size: 12px;" colspan="3">
                                                        <span>
                                                            <asp:RadioButton ID="rbLieferscheinKunde" GroupName="Kunde" runat="server" Text="Lieferscheinkunde"
                                                                TabIndex="3" AutoPostBack="True" />
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formRadioButton">
                                                    <td class="firstLeft active">
                                                    </td>
                                                    <td class="active">
                                                    </td>
                                                    <td class="firstLeft active" valign="middle" style="width: 165px; height: 25px;">
                                                        <img alt="" style="width: 25px; height: 25px; padding-right: 5px;" src="../Images/Pfeil_nachrechts.jpg" />
                                                        <asp:Label ID="Label1" Height="15px" runat="server" Text="Einzugsermächtigung"></asp:Label>
                                                    </td>
                                                    <td class="firstLeft active" style="width: 50px">
                                                        <span>
                                                            <asp:RadioButton ID="rbEinzugJa" GroupName="Einzug" Text="Ja" runat="server" TabIndex="4" OnClientClick="ShowEinzug()"/>                                                            
                                                        </span>
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <span>
                                                            <asp:RadioButton ID="rbEinzugNein" GroupName="Einzug" Text="Nein" runat="server"
                                                                TabIndex="5" />
                                                        </span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="height: 19px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="paginationQuery">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tbody>
                                                        <tr>
                                                            <td class="firstLeft active" style="font-size: 12px">
                                                                Kundendaten
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formRadioButton">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px">
                                                        Anrede:*
                                                    </td>
                                                    <td class="firstLeft active" style="font-size: 12px;" colspan="2">
                                                        <span style="padding-right: 50px">
                                                            <asp:RadioButton ID="rbFirma" GroupName="Anrede" Text="Firma" runat="server" TabIndex="6" />
                                                        </span><span style="padding-right: 55px">
                                                            <asp:RadioButton ID="rbHerr" GroupName="Anrede" Text="Herr" runat="server" TabIndex="7" />
                                                        </span><span>
                                                            <asp:RadioButton ID="rbFrau" GroupName="Anrede" Text="Frau" runat="server" TabIndex="8" />
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px">
                                                        Branche*:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 215px">
                                                        <asp:DropDownList ID="ddlBranche" runat="server" AutoPostBack="True" TabIndex="9">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblBrancheFrei" Font-Bold="true" Font-Size="12px" runat="server" Visible="false"
                                                            Text="*"></asp:Label>
                                                        <asp:TextBox ID="txtBrancheFrei" Width="315px" Visible="false" runat="server" TabIndex="10"
                                                            MaxLength="20"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px">
                                                        Name 1 / Firma*:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtName1" Width="565px" runat="server" MaxLength="40" TabIndex="11"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        Name 2 / Zusatz:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtName2" Width="565px" runat="server" MaxLength="40" TabIndex="12"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 127px">
                                                        Straße*:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtStrasse" Width="400" runat="server" MaxLength="60" TabIndex="13"></asp:TextBox>
                                                    </td>
                                                    <td class="firstLeft active" style="font-size: 12px">
                                                        Haus-Nr.*:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 230px">
                                                        <asp:TextBox ID="txtHausnr" Width="49" runat="server" MaxLength="10" TabIndex="14"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px">
                                                        Postleitzahl*:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 90px">
                                                        <asp:TextBox ID="txtPlz" Width="80" MaxLength="5" runat="server" TabIndex="15"></asp:TextBox>
                                                    </td>
                                                    <td class="firstLeft active" style="font-size: 12px; width: 35px">
                                                        Ort*:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 575px">
                                                        <asp:TextBox ID="txtOrt" Width="399" runat="server" MaxLength="40" TabIndex="16"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px">
                                                        Land*:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 175px">
                                                        <asp:DropDownList ID="ddLand" Width="150" runat="server" TabIndex="17" 
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px">
                                                        Ust-ID Nummer<asp:Label ID="lblStar" runat="server" Visible="false" Text="*"></asp:Label>:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtUIDNummer" Width="225" runat="server" MaxLength="20" TabIndex="18"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="paginationQuery">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tbody>
                                                        <tr>
                                                            <td class="firstLeft active" style="font-size: 12px">
                                                                Kommunikation
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" colspan="2" style="font-size: 12px;" tabindex="19">
                                                        Ansprechpartner
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px; height: 22px;">
                                                        Vorname:
                                                    </td>
                                                    <td class="firstLeft active" style="height: 22px">
                                                        <asp:TextBox ID="txtVornameAnPartner" Width="270" MaxLength="35" runat="server" TabIndex="20"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; height: 22px;">
                                                        Name:
                                                    </td>
                                                    <td class="firstLeft active" style="height: 22px;">
                                                        <asp:TextBox ID="txtNameAnPartner" Width="270" MaxLength="35" runat="server" TabIndex="21"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; height: 22px;">
                                                        Funktion:
                                                    </td>
                                                    <td class="firstLeft active" style="height: 22px;">
                                                        <asp:DropDownList ID="ddlFunktion" Width="273" runat="server" TabIndex="22">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px">
                                                        Telefon:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtTelefon" Width="270" MaxLength="30" runat="server" TabIndex="23"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px">
                                                        Mobil:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtMobil" Width="270" MaxLength="30" runat="server" TabIndex="24"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px">
                                                        Fax:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtFax" Width="270" MaxLength="30" runat="server" TabIndex="25"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 125px">
                                                        E-Mail:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtMail" Width="270" MaxLength="100" runat="server" TabIndex="26"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        <asp:Button ID="MPEDummy" style="display: none" runat="server" style="display:none" />
                        
                        <cc1:ModalPopupExtender runat="server" ID="MPENeukundeResultat" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="NeukundeResultat" TargetControlID="MPEDummy">
                        </cc1:ModalPopupExtender>        
                        <asp:Panel ID="NeukundeResultat" HorizontalAlign="Center" runat="server" style="display:none">
                            <table cellspacing="0" id="Table1" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                  style="border: solid 1px #646464">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lblNeukundeResultatMeldung" runat="server"></asp:Label></td></tr><tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lbNeukundeResultat" Text="OK" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton" ></asp:LinkButton></td></tr><tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                                
                            </table>
                        </asp:Panel>
                            <div id="dataFooter">
                                <asp:LinkButton ID="lbParken" Text="Parken" Height="16px" Width="78px" runat="server"
                                    CssClass="Tablebutton" TabIndex="27"></asp:LinkButton>                                
                                <asp:LinkButton ID="lbAusparken" Text="Ausparken" Height="16px" Width="78px" runat="server"
                                    CssClass="Tablebutton" TabIndex="27"></asp:LinkButton>
                                <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                                    CssClass="Tablebutton" TabIndex="27"></asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                                <asp:Button ID="MPEDummy3" style="display: none" runat="server" style="display:none" />
                                <cc1:ModalPopupExtender runat="server" ID="MPEAusparken" BackgroundCssClass="divProgress"
                                    Enabled="true" PopupControlID="Ausparken" TargetControlID="MPEDummy3">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="Ausparken" HorizontalAlign="Center" runat="server" Style="display: none">
                                    <table cellspacing="0" id="Table3" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                        style="width: 50%; border: solid 1px #646464">
                                        <tr>
                                            <td colspan="3">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                geparkte Neukunden:
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblErrorAusparken" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:GridView ID="gvAusparken" runat="server" AllowPaging="False" AllowSorting="True"
                                                    AutoGenerateColumns="False" BackColor="White" CssClass="GridView" GridLines="None"
                                                    HorizontalAlign="Center" ShowFooter="False" Width="100%">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="NAME1" HeaderText="Kundenname" />
                                                        <asp:BoundField DataField="CITY1" HeaderText="Ort" />
                                                        <asp:BoundField DataField="ERDAT" HeaderText="erfasst" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="ibAusparkenTable" runat="server" Text="Ausparken" Height="16px"
                                                                    Width="78px" CssClass="Tablebutton" CommandName="ausparken" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.VKUNNR") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibAusparkenDelete" runat="server" Width="32" Height="32" ImageUrl="~/Images/RecycleBin.png"
                                                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.VKUNNR") %>' CommandName="löschen" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lbAusparkenClose" Text="Schließen" Height="16px" Width="78px"
                                                    runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
