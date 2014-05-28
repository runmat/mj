<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01s.aspx.vb" Inherits="CKG.Components.ComCommon.Change01s"
    MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


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
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP1">
                    
                        <Triggers>
                            <asp:PostBackTrigger ControlID="cmdSearch" />
                        </Triggers>
                    
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%" style="width: 100%">
                                    <tbody>
                                        <tr id="tr_Message" runat="server" class="formquery">
                                            <td colspan="2" class="firstLeft active">
                                                <p>
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Black" Font-Bold="True"></asp:Label>
                                                    <asp:Label ID="lblerror" runat="server" CssClass="TextError"></asp:Label></p>
                                            </td>
                                        </tr>
                                        
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                
                                                <asp:Label ID="lbl_Auswahl" runat="server">lbl_Auswahl</asp:Label>
                                            
                                            </td>
                                                     <td class="firstLeft active">
                                                <span><asp:RadioButton ID="rb_Einzelauswahl" runat="server" Text="rb_Einzelauswahl" 
                                                             GroupName="Auswahl" Checked="True"  /></span>
                                                <span><asp:RadioButton ID="rb_Upload" runat="server" Text="rb_Upload" 
                                                             GroupName="Auswahl" AutoPostBack="True" /></span>
                                                
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
                                                (XX-Y1234)
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
                                        <tr class="formquery" id="tr_NummerZB2" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_NummerZB2" runat="server">lbl_ZZREFERENZ1</asp:Label>
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
                                            <td class="active">
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
                                                                <td style="width:100%">
                                                                <div style="width:120px">
                                                                        Fahrgestellnummer
                                                                    </div>
                                                                    <div style="width:120px">
                                                                        1J8HCE8MX8Y178953
                                                                    </div>
                                                                    <div style="width:120px">
                                                                        1A8HSH4958B155255
                                                                    </div>
                                                                    <div style="width:120px">
                                                                        1A8HSH4948B152640
                                                                    </div>
                                                                    <div id="Last" style="width:120px">
                                                                        &nbsp;
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </span></img> </a>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="2" align="right" class="rightPadding" style="width: 100%">
                                                <div id="dataQueryFooter">
                                                    <asp:LinkButton ID="cmdSearch" runat="server" CssClass="TablebuttonLarge" Width="130px"
                                                        Height="16px">» Weiter</asp:LinkButton>
                                                    &nbsp;
                                                </div>
                                            </td>
                                        </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
