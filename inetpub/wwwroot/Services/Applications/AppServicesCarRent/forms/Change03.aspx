<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03.aspx.vb" Inherits="AppServicesCarRent.Change03"
    MasterPageFile="../MasterPage/App.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text=""></asp:Label>
                        </h1>
                    </div>
                    <asp:Panel ID="Panel2" DefaultButton="btndefault" runat="server">
                        <div id="TableQuery">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr id="tr_Message" runat="server" class="formquery" >
                                    <td colspan="2" class="firstLeft active" style="width: 100%">
                                        <p>
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Black" Font-Bold="True"></asp:Label>
                                            <asp:Label ID="lblerror" runat="server" CssClass="TextError"></asp:Label></p>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Auswahl" runat="server">lbl_Auswahl</asp:Label>
                                    </td>
                                    <td class="active">
                                        <span>
                                            <asp:RadioButton ID="rb_Einzelauswahl" runat="server" 
                                            Text="rb_Einzelauswahl" GroupName="Auswahl"
                                                Checked="True" AutoPostBack="True" /></span> <span>
                                                    <asp:RadioButton ID="rb_Upload" runat="server" Text="rb_Upload" GroupName="Auswahl"
                                                        AutoPostBack="True" /></span>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Leasingvertragsnummer" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Leasingvertragsnummer" runat="server">lbl_Leasingvertragsnummer</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtOrdernummer" runat="server" MaxLength="11"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Kennzeichen" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Kennzeichen" runat="server"></asp:Label>:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtAmtlKennzeichen" runat="server" MaxLength="35"></asp:TextBox>

                                        <asp:Label ID="lbl_FilterInfo" runat="server" Text="lbl_FilterInfo"></asp:Label>

                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_KennzeichenZusatz" runat="server">
                                    <td class="firstLeft active">
                                    </td>
                                    <td class="active">
                                        * Eingabe von nachgestelltem Platzhalter möglich. Mindestens Kreis und ein Buchstabe
                                        (z.B. XX-Y*)
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_ZZREFERENZ1" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_ZZREFERENZ1" runat="server">lbl_ZZREFERENZ1</asp:Label>
                                    </td>
                                    <td class="active"  style="width: 100%">
                                        <asp:TextBox ID="txtZZREFERENZ1" runat="server" MaxLength="20"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_NummerZB2" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_NummerZB2" runat="server">lbl_NummerZB2</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtNummerZB2" runat="server" MaxLength="20"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Fahrgestellnummer" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" MaxLength="17"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_FahrgestellnummerZusatz" runat="server">
                                    <td class="firstLeft active">
                                    </td>
                                    <td class="active">
                                        ** Eingabe von vorangestelltem Platzhalter möglich. Mindestens fünf Zeichen (z.B.
                                        *12345)
                                    </td>
                                </tr>
                                <tr id="tr_Alle" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_alle" runat="server">kompletter Bestand</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:CheckBox ID="chk_alle" runat="server" />
                                    </td>
                                </tr>
                                <tr id="tr_Platzhaltersuche" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Platzhaltersuche" runat="server">lbl_Platzhaltersuche</asp:Label>&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td class="active">
                                        <asp:RadioButton ID="cbxPlatzhaltersuche" runat="server" Checked="True" GroupName="grpWeicheSuche"
                                            Text=" Platzhaltersuche möglich. Nur verwendbare Vorgänge werden angezeigt.">
                                        </asp:RadioButton><br>
                                        <asp:RadioButton ID="cbxOhnePlatzhalter" runat="server" GroupName="grpWeicheSuche"
                                            Text=" Platzhalter werden ignoriert. Informationen zu dem Vorgang werden angezeigt, insofern er im System gefunden wurde.">
                                        </asp:RadioButton>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_upload" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Upload" runat="server">lbl_Upload</asp:Label>
                                    </td>
                                    <td class="active" style="width: 100%">
                                        <input id="upFile1" type="file" size="35" name="File1" runat="server" />
                                        <a class="tip" href="#">
                                            <img alt="" border="0" height="18px" src="/Services/images/Ausrufezeichen01_10.jpg"
                                                width="18px" />
                                            <span>
                                                <table id="InfoTab" cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100%" nowrap="nowrap">
                                                            Upload von Fahrgestellnummern
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" nowrap="nowrap">
                                                            Erwarteter Dateityp für den Upload: Excel-Datei (*.xls)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%">
                                                            Erwartetes Dateiformat (<strong>mit</strong> Spaltenüberschriften)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%">
                                                            <div style="width: 120px">
                                                                Fahrgestellnummer
                                                            </div>
                                                            <div style="width: 120px">
                                                                1J8HCE8MX8Y178953
                                                            </div>
                                                            <div style="width: 120px">
                                                                1A8HSH4958B155255
                                                            </div>
                                                            <div style="width: 120px">
                                                                1A8HSH4948B152640
                                                            </div>
                                                            <div id="Last" style="width: 120px">
                                                                &nbsp;
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </span></img> </a>
                                    </td>
                                </tr>
                                <tr class="formquery" >
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery"  style="background-color: #dfdfdf; width: 100%">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>                                
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                                >» Suchen</asp:LinkButton>
                            <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btndefault" runat="server"
                                Text="Button" />
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
