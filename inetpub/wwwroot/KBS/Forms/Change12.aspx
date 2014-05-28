<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change12.aspx.vb" Inherits="KBS.Change12"
    MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="JavaScript" type="text/javascript" src="../Java/JScript.js"></script>
    <style type="text/css">
    
        .RadioButton input
        {
            margin-right:2px;
        }
        
        .RadioButton label
        {
            vertical-align:top;
        }
    
    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label">Retoure zum Lieferanten</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <div id="TableQuery">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr class="formquery">
                                            <td colspan="4" class="firstLeft active">
                                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="4" class="firstLeft active">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblKst" runat="server" Width="80px">Kostenstelle</asp:Label>
                                            </td>
                                            <td class="active" colspan="3" style="white-space: nowrap;">
                                                <asp:TextBox ID="txtKST" runat="server" Enabled="false" AutoPostBack="true" Width="100px"
                                                    MaxLength="4"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="ftbeTxtKST" runat="server" FilterMode="ValidChars"
                                                    FilterType="Numbers" TargetControlID="txtKST">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:Label ID="lblKSTText" runat="server" TabIndex="0" Visible="false" Style="margin-left: 10px;"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery" style="vertical-align: top; text-align:left;">
                                            <td class="firstLeft active" style="vertical-align: top; width:80px;">
                                                <asp:Label ID="Label2" runat="server" width="80px">Lieferant:</asp:Label>
                                            </td>
                                            <td class="active" style="vertical-align: top;">
                                                <asp:DropDownList ID="ddlLieferant"  Style="width: auto; min-width:100px;" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>                                          
                                            <td style="vertical-align: top;">
                                                <table style="border-style: none; vertical-align: top; text-align:left;">
                                                    <tr>                                                        
                                                        <td style="padding-top:0px;">
                                                            <asp:RadioButton ID="rdbRückPost" runat="server" Checked="true" GroupName="Retoureweg" CssClass="active RadioButton"
                                                                BorderStyle="None" AutoPostBack="true" style="white-space: nowrap;" Text="Rückgabe per Post/Spedition"/>
                                                        </td>
                                                        <td width="100%" colspan="3">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trADM" runat="server" style="vertical-align: top;">                                                       
                                                        <td style="text-align: left; border-style: none;vertical-align:top;">
                                                            <asp:RadioButton ID="rdbADM" runat="server" Checked="false" AutoPostBack="true" GroupName="Retoureweg" class="active RadioButton"
                                                                BorderStyle="None" Text="Rückgabe an ADM" style="white-space: nowrap;"></asp:RadioButton>
                                                        </td>
                                                        <td class="active" width="100px">
                                                            <span id="spRückADM" runat="server"  style="white-space: nowrap; vertical-align: top;" visible="false">Lieferschein-Nr.:</span>&nbsp;
                                                        </td>
                                                        <td class="active" style="vertical-align: top;">
                                                            <asp:TextBox ID="txtLiefsNr" runat="server" visible="false"></asp:TextBox>&nbsp;
                                                        </td>
                                                       <td style="width:50%">
                                                        </td>
                                                    </tr>                                                    
                                                </table>
                                            </td>
                                            <td style="width:50%"> &nbsp;</td>
                                        </tr>
                                        <tr class="formquerry">
                                            <td colspan="5">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquerry">
                                            <td colspan="5" class="firstLeft">
                                                <table width="100%" style="text-align: center;" rules="none" border="outer" cellpadding="0"
                                                    cellspacing="0">
                                                    <thead class="Tablehead" style="background-color: #2B4C91; height: 22px;">
                                                        <tr style="border-style: none; border-spacing: 0px;">
                                                            <th style="padding: 0;">
                                                                Artikelbezeichnung
                                                            </th>
                                                            <th style="padding: 0;">
                                                                Stück
                                                            </th>
                                                            <th>
                                                                Grund der Retoure
                                                            </th>
                                                            <th>
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td style="padding: 2px;">
                                                                <asp:DropDownList ID="ddlArtikel" runat="server" Width="300px" AutoPostBack="false">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="padding: 2px;">
                                                                <asp:TextBox ID="txtStückzahl" runat="server" Width="50px" MaxLength="4"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeStückzahl" runat="server" TargetControlID="txtStückzahl"
                                                                    FilterType="Numbers">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                            <td style="padding: 2px;">
                                                                <asp:DropDownList ID="ddlRetouregrund" runat="server" />
                                                            </td>
                                                            <td style="padding: 2px;">
                                                                <asp:LinkButton ID="lnbAdd" Text="hinzufügen" Height="16px" Width="78px" runat="server"
                                                                    CssClass="Tablebutton"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="4" class="firstLeft active">
                                                <asp:Label ID="lblMessage" CssClass="TextError" runat="server"></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="data">
                        <asp:UpdatePanel runat="server" ID="upGrid">
                            <ContentTemplate>
                                <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView CssClass="GridView" ID="GridView3" runat="server" Width="100%" AutoGenerateColumns="False"
                                                AllowPaging="False" AllowSorting="True" ShowFooter="False" GridLines="None">
                                                <PagerSettings Visible="false" />
                                                <HeaderStyle CssClass="GridTableHead" HorizontalAlign="Left"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                <RowStyle CssClass="ItemStyle" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="Artikelbezeichnung" DataField="Artikelbezeichnung" />
                                                    <asp:TemplateField HeaderText="Menge">
                                                        <ItemTemplate>
                                                            <asp:ImageButton CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ArtIdx") %>'
                                                                CommandName="minusMenge" ID="imgbMinus" ImageUrl="~/Images/Minus.jpg" Width="15"
                                                                Height="15" runat="server" />
                                                            &nbsp;
                                                            <asp:TextBox MaxLength="3" runat="server" AutoPostBack="true" OnTextChanged="txtMenge_TextChanged"
                                                                Width="50px" onFocus="Javascript:this.select();" ID="txtMenge" Text='<%# DataBinder.Eval(Container, "DataItem.Menge") %>'></asp:TextBox>
                                                            &nbsp;<asp:ImageButton CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ArtIdx") %>'
                                                                CommandName="plusMenge" ID="imgbPlus" Width="15" Height="15" ImageUrl="~/Images/Plus.jpg"
                                                                runat="server" />
                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMenge" Display="None"
                                                                ErrorMessage="Bitte Menge eingeben" SetFocusOnError="true" ID="rfvMenge"> </asp:RequiredFieldValidator>
                                                            <asp:RangeValidator SetFocusOnError="true" ControlToValidate="txtMenge" Enabled="True"
                                                                ID="rvMenge" MinimumValue="1" MaximumValue="999" runat="server" Display="None"
                                                                ErrorMessage="Menge 1-999"></asp:RangeValidator><cc1:ValidatorCalloutExtender Enabled="True"
                                                                    ID="vceMenge" Width="350px" runat="server" HighlightCssClass="validatorCalloutHighlight"
                                                                    TargetControlID="rvMenge">
                                                                </cc1:ValidatorCalloutExtender>
                                                            <cc1:ValidatorCalloutExtender Enabled="True" ID="vceMenge2" Width="350px" runat="server"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="rfvMenge">
                                                            </cc1:ValidatorCalloutExtender>
                                                            <cc1:FilteredTextBoxExtender Enabled="True" ID="fteMenge2" runat="server" TargetControlID="txtMenge"
                                                                FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Grund der Retoure" DataField="Retouregrund" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lbDelete" runat="server" Width="32" Height="32" ImageUrl="~/Images/RecycleBin.png"
                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ArtIdx") %>' CommandName="entfernen" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <input id="Hidden1" type="hidden" runat="server" />

                                <script type="text/javascript" language="javascript">
                                    function onChangeSetHiddenField() {
                                        var hiddenInput = document.getElementById("ctl00_ContentPlaceHolder1_Hidden1");
                                        hiddenInput.value = 1;
                                    }
                                    function onSetFocusField(e, control1, control2) {
                                        var e = e ? e : window.event;
                                        var KeyCode = e.which ? e.which : e.keyCode;


                                        if (KeyCode == 40)
                                            control1.focus();
                                        else if (KeyCode == 38)
                                            control2.focus();
                                    }
                                    function onSetFocusFirstField(control1) {
                                        control1.focus();
                                    }                                                                            
                                </script>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                        <asp:Button ID="MPEDummy" Width="0" Height="0" runat="server" />
                        <asp:Button ID="MPEDummy1" Width="0" Height="0" runat="server" />
                        <asp:Button ID="MPEDummy2" Width="0" Height="0" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="mpeRetoureCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="RetoureCheck" TargetControlID="MPEDummy" BehaviorID="RetoureCheck">
                        </cc1:ModalPopupExtender>
                        <cc1:ModalPopupExtender runat="server" ID="MPE_ChangeLieferant" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="plChangeLief" TargetControlID="MPEDummy2">
                        </cc1:ModalPopupExtender>
                        <cc1:ModalPopupExtender runat="server" ID="MPERetourePrint" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="RetourePrint" TargetControlID="MPEDummy1">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="RetoureCheck" runat="server" Style="overflow: auto; height: 425px;
                            width: 600px; display: none"><%--DefaultButton="lbRetoureOk"--%>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                <ContentTemplate>
                                    <table cellspacing="0" id="tblRetourecheck" runat="server" bgcolor="white" cellpadding="0"
                                        style="overflow: auto; height: 425px; width: 583px; border: solid 1px #646464">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="firstLeft active">
                                                    <asp:Label ID="lblBedienError" runat="server" Text="Einscannen der Bedienerkarte!"
                                                        CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblStatus" Visible="false" runat="server">Retourestatus</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="txtBedienerkarte" Width="240px" runat="server" TextMode="Password"></asp:TextBox>
                                                    <asp:Label ID="lblRetoureMeldung" Visible="false" EnableViewState="true" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="trInfo" runat="server">
                                                <td align="center" class="firstLeft active">
                                                    <asp:Label ID="Info" runat="server">Bitte überprüfen Sie Ihre Retoure, ungewöhnliche Werte sind Rot markiert!<br />
                                                Bitte korrigieren Sie gegebenenfalls!<br /></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trGridview" runat="server">
                                                <td>
                                                    <asp:GridView ID="GridView2" runat="server" AllowPaging="False" AllowSorting="True"
                                                        AutoGenerateColumns="False" BackColor="White" CssClass="GridView" GridLines="None"
                                                        HorizontalAlign="Center" ShowFooter="False" Style="overflow: auto; width: 75%">
                                                        <PagerSettings Visible="false" />
                                                        <HeaderStyle CssClass="GridTableHead" />
                                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                        <RowStyle CssClass="ItemStyle" />
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblArtIdx" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ArtIdx") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Artikelbezeichnung" HeaderText="Artikelbezeichnung" />
                                                            <asp:BoundField DataField="Menge" HeaderText="Stück" />
                                                            <asp:BoundField DataField="Retouregrund" HeaderText="Retouregrund" />
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:LinkButton ID="lbRetoureFinalize" Visible="false" Text="Weiter" Height="16px" Width="78px"
                                                        runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                                    <asp:LinkButton ID="lbRetoureKorrektur" Text="Korrektur" Height="16px" Width="78px"
                                                        runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                                    &nbsp; &nbsp;
                                                    <asp:LinkButton ID="lbRetoureOk" Text="Weiter" Height="16px" Width="78px" runat="server"
                                                        CssClass="Tablebutton"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input id="SendTopSap" type="hidden" runat="server" />&nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>

                                    <script type="text/javascript" language="javascript">

                                        function ControlField(control1) {

                                            if (control1.value.length == 15) {
                                                if (control1.value.substring(control1.value.length - 1) == '}') {
                                                    theForm.__EVENTTARGET.value = '__Page';
                                                    theForm.__EVENTARGUMENT.value = 'MyCustomArgument';
                                                    theForm.submit();
                                                    var hiddenInput = document.getElementById("ctl00_ContentPlaceHolder1_SendTopSap");
                                                    hiddenInput.value = 1;
                                                }
                                                else {
                                                    control1.focus();
                                                }
                                            }
                                            else {
                                                control1.focus();
                                            }
                                        }                                                                
                                                           
                                    </script>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                        <asp:Panel ID="plChangeLief" HorizontalAlign="Center" runat="server" Style="display: none">
                            <table cellspacing="0" id="Table2" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                style="width: 50%; border: solid 1px #646464">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        <span>Beim Wechsel des Lieferanten werden Ihre Eingaben verloren gehen! Jetzt wechseln?</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lbtnOK" Text="OK" Height="16px" Width="78px" runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnCancel" Text="Abbrechen" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>                        
                        <asp:Panel ID="RetourePrint" HorizontalAlign="Center" runat="server" Style="display: none">
                                    <table cellspacing="0" id="Table1" runat="server" width="50%" bgcolor="white" cellpadding="0"
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
                                                <asp:Label ID="lblRetourePrint" runat="server" Text="Klicken Sie auf Drucken!"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                        <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lbCreatePDF"  Text="Drucken" Height="16px" Width="78px" runat="server"
                                                    CssClass="Tablebutton"></asp:LinkButton>                                                    
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
    </div>
</asp:Content>
