<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TaggleicheMeldungDAD.aspx.vb" Inherits="KBS.TaggleicheMeldungDAD"
    MasterPageFile="~/KBS.Master" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../Java/JScript.js"></script>
    <script type="text/javascript">
        function checkZulassungsdatum() {
            var tb = document.getElementById('<%= txtZulassungsdatum.ClientID %>');
            document.getElementById('<%= ihDatumIstWerktag.ClientID %>').value = nurWerktage(tb.value)[0];
            return true;
        }
    </script>
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                   <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Taggleiche Meldung DAD"></asp:Label>
                            </h1>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="paginationQuery">
                                    <table cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td class="active">
                                                    Neue Abfrage starten
                                                </td>
                                                <td align="right">
                                                    <div id="queryImage">
                                                        <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="~/Images/queryArrow.gif" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div style="margin-top: 25px">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                </div>
                                <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                    <div id="TableQuery" style="margin-bottom: 10px">
                                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                            <tbody>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" >
                                                        <asp:Label runat="server" Height="16px">DAD-Auftrag: Barcode</asp:Label>
                                                    </td>
                                                    <td class="firstLeft active" >
                                                        <asp:TextBox ID="txtBarcode" runat="server" CssClass="TextBoxNormal" 
                                                            Width="110px" TabIndex="1"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="fteBarcode" 
                                                            TargetControlID="txtBarcode" InvalidChars="|}" FilterMode="InvalidChars" />
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label runat="server">Fahrgestellnummer:</asp:Label>
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" 
                                                            MaxLength="20" Width="230px" TabIndex="3" style="text-transform:uppercase;" />
                                                    </td>
                                                </tr>   
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label runat="server">ZBII-Nummer/Brief:</asp:Label>
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtBriefnummer" runat="server" CssClass="TextBoxNormal" 
                                                            MaxLength="10" Width="110px" TabIndex="4" style="text-transform:uppercase;" />
                                                    </td>
                                                </tr>                                     
                                                <tr class="formquery">
                                                    <td colspan="2">
                                                        <asp:ImageButton
                                                            ID="btnEmpty" runat="server" Height="16px" ImageUrl="~/Images/empty.gif" Width="1px" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                            &nbsp;
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div id="dataQueryFooter">
                                    <asp:LinkButton ID="cmdCreate" runat="server" CssClass="TablebuttonMiddle" 
                                        Height="16px" Width="100px">» Daten holen</asp:LinkButton>
                                </div>
                                <asp:Panel ID="Panel2" DefaultButton="btnEmpty2" runat="server" Visible="False">
                                    <div id="Result" runat="Server">
                                        <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:Label runat="server" Height="16px">* Pflichtfelder</asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label runat="server">ID:</asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="lblIDDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label runat="server">Bestellnr.:</asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="lblBestellnummerDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label runat="server">Fahrgestellnummer:</asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="lblFahrgestellnummerDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label runat="server">ZBII-Nummer/Brief:</asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="lblBriefnummerDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label runat="server">Datum der Zulassung*:</asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtZulassungsdatum" onKeyPress="return numbersonly(event, false)" runat="server" CssClass="TextBoxNormal" 
                                                        Width="75px" MaxLength="6" />
                                                    <asp:Label Style="padding-left: 2px; font-weight: normal"
                                                        Height="15px" runat="server">(ttmmjj)</asp:Label>
                                                    <asp:LinkButton runat="server" Style="padding-left: 10px; font-weight: normal" Height="15px"
                                                        ID="lbtnGestern" Text="Gestern |" Width="60px" />
                                                    <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnHeute"
                                                        Width="50px" Text="Heute |" />
                                                    <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnMorgen"
                                                        Width="60px" Text="Morgen" />
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label runat="server">Kennzeichen*:</asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtKennz1" MaxLength="3" CssClass="TextBoxNormal" Width="30px" runat="server"
                                                        style="text-transform:uppercase;" />
                                                    <span style="padding-right: 2px; padding-left: 2px">-</span>
                                                    <asp:TextBox ID="txtKennz2" MaxLength="6" CssClass="TextBoxNormal" Width="100px"
                                                        runat="server" style="text-transform:uppercase;"></asp:TextBox>
                                                    <span style="padding-right: 2px;"></span>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label runat="server">Gebühr*:</asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtGebuehr" CssClass="TextBoxNormal" Width="75px" runat="server"
                                                        style="text-transform:uppercase;" MaxLength="11" onKeyPress="return numbersonly(event, true)" />
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label runat="server">Auslieferung:</asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <asp:CheckBox ID="cbxAuslieferung" runat="server" Text=""/>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="cmdFrachtbriefnummer" runat="server" CssClass="TablebuttonLarge" 
                                                        Width="128px" Height="16px">» Frachtbrief ändern </asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px" runat="server" ID="trFrachtbriefnummer" Visible="False">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label runat="server">Frachtbriefnummer:</asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtFrachtbriefnummer" CssClass="TextBoxNormal" Width="150px" runat="server"
                                                        style="text-transform:uppercase;" MaxLength="20" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="3">
                                                    <asp:ImageButton
                                                        ID="btnEmpty2" runat="server" Height="16px" ImageUrl="~/Images/empty.gif" Width="1px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                                <div id="dataFooter">
                                    <asp:LinkButton ID="cmdSend" runat="server" CssClass="Tablebutton" 
                                        Width="78px" Visible="False">» Absenden </asp:LinkButton>
                                </div>
                                <input type="hidden" runat="server" id="ihDatumIstWerktag" value="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
