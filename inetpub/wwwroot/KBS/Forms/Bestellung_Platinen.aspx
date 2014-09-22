<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Bestellung_Platinen.aspx.vb"
    Inherits="KBS.Bestellung_Platinen" MasterPageFile="~/KBS.Master" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.30930.28736, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="../Java/JScript.js"></script>
    <script language="JavaScript" type="text/javascript" src="../Java/jquery-ui-1.9.2.custom.min.js"></script>
    <script language="JavaScript" type="text/javascript" src="../Java/jquery.ui.datepicker-de.min.js"></script>
    <script language="JavaScript" type="text/javascript" src="../Java/jquery.ui.dialog.min.js"></script>
    <link href="../Styles/jquery-ui-1.9.2.custom.min.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/jquery.ui.datepicker.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            // Dialog 1 Konfiguration
            var dial = $("#ctl00_ContentPlaceHolder1_BestellungsCheck").dialog({
                resizable: false,
                width: 600,
                modal: true,
                draggable: false,
                buttons: {
                    Korrektur: function () {
                        $(this).dialog("close");
                        $("#ctl00_ContentPlaceHolder1_BestellCheckHidden").val(1);
                    },
                    Weiter: function () {
                        SendArtikel("ctl00_ContentPlaceHolder1_lbBestellungOk");
                    }
                }
            });

            if ($("#ctl00_ContentPlaceHolder1_BestellCheckHidden").val() == 0) {
                dial.ready(function () {
                    var vis = $("#ctl00_ContentPlaceHolder1_txtBedienerkarte").attr("visible");
                    if (vis == undefined || vis == "true") {
                        $("#ctl00_ContentPlaceHolder1_txtBedienerkarte").select();
                    }
                });
                dial.dialog("open");
            } else {
                dial.dialog("close");
            }

            // Dialog 2 Konfiguration
            var dial2 = $("#plChangeLief").dialog({
                resizable: false,
                width: 600,
                modal: true,
                draggable: false,
                buttons: {
                    Ok: function () {
                        // OK setzen
                        $("#OKClicked").val(1);
                        // Ausblenden vor Postback
                        $(this).dialog("close");
                        // Änderung absenden
                        ChangeLieferant();
                    },
                    Abbrechen: function () {
                        // Dialog ausblenden
                        $(this).dialog("close");
                    }
                },
                close: function () {
                    // Dialog ausblenden
                    $("#ctl00_ContentPlaceHolder1_ChangeLiefHidden").val(1);

                    // Ok Button abfragen
                    if ($("#OKClicked").val() != 1) {
                        var lValue = $("#lastValue").val();
                        if (lValue != "") {
                            $("#ctl00_ContentPlaceHolder1_ddlLieferant").val(lValue);
                        }
                    } else {
                        $("#OKClicked").val(0);
                    }
                }
            });

            if ($("#ctl00_ContentPlaceHolder1_ChangeLiefHidden").val() == 0 && $("#ctl00_ContentPlaceHolder1_Hidden1").val() == 1) {
                dial2.dialog("open");
            } else {
                dial2.dialog("close");
            }

            // Dialog 3 Konfiguration
            var dial3 = $("#ctl00_ContentPlaceHolder1_plMessage").dialog({
                resizable: false,
                width: 600,
                modal: true,
                draggable: false,
                buttons: {
                    OK: function () {
                        $("#ctl00_ContentPlaceHolder1_MessageHidden").val(1);
                        $(this).dialog("close");
                    }
                }
            });

            if ($("#ctl00_ContentPlaceHolder1_MessageHidden").val() == 0) {
                dial3.dialog("open");
            } else {
                dial3.dialog("close");
            }

            // Optionen Panel
            var AccOptional = $("#divOptionen").accordion({
                collapsible: true,
                active: false,
                change: function (event, ui) {
                    var index = AccOptional.accordion("option", "active");
                    if (index === false) {
                        // alle geschlossen
                        $("#ctl00_ContentPlaceHolder1_txtLieferdatum").val("");
                    }
                }
            });
            if ($("#ctl00_ContentPlaceHolder1_txtLieferdatum").val() != "") {
                $("#divOptionen").accordion('activate', 0);
            }
        });

        function pageLoad() {
            // Datepicker an Textbox binden
            $("#ctl00_ContentPlaceHolder1_txtLieferdatum").unbind();
            $("#ctl00_ContentPlaceHolder1_txtLieferdatum").datepicker();

            // Feld löschen bei Change-Event
            $("#ctl00_ContentPlaceHolder1_chkGeliefert").change(function () {
                ShowLieferscheinnummer();
            });

            // Lieferscheinnummer initial ausblenden
            ShowLieferscheinnummer();

            // Lieferantenwechsel prüfen
            $("#ctl00_ContentPlaceHolder1_ddlLieferant").change(
            function () {
                if ($("#ctl00_ContentPlaceHolder1_ddlLieferant").val() != $("#lastValue").val() &&
                    $("#ctl00_ContentPlaceHolder1_Hidden1").val() == 1) {
                    $("#ctl00_ContentPlaceHolder1_ChangeLiefHidden").val(0);
                    $("#plChangeLief").dialog("open");
                } else {
                    ChangeLieferant();
                }
            });

            // letzter gewählter Lieferant setzen
            var value = $("#ctl00_ContentPlaceHolder1_ddlLieferant").val();
            $("input[id=lastValue]").val(value);
        }

        // Formular Absenden
        function SendArtikel() {
            var Textbox = $("#ctl00_ContentPlaceHolder1_txtBedienerkarte");
            if (Textbox.val().length == 15) {
                var text = Textbox.val();
                text = text.substr(Textbox.val().length - 1, 1);
                if (Textbox.val().substr(Textbox.val().length - 1, 1) == "}" || Textbox.val().substr(Textbox.val().length - 1, 1) == ".") {
                    __doPostBack("<%=txtBedienerkarte.UniqueID %>", Textbox.val());
                }
            }
            else if (Textbox.val().length == 0) {
                $("#ctl00_ContentPlaceHolder1_lblBedienError").text("Bitte lesen Sie die Bedienerkarte ein!");
            } 
            else {
                $("#ctl00_ContentPlaceHolder1_lblBedienError").text("Fehler beim Einlesen der Bedienerkarte. Barcode hat die falsche Länge!");
            }
        }

        // Ansichtssteuerung der Lieferscheinnummer
        function ShowLieferscheinnummer() {
            if ($("#ctl00_ContentPlaceHolder1_chkGeliefert").is(":checked")) {
                $("#ctl00_ContentPlaceHolder1_trLieferscheinnummer").show();
            } else {
                $("#ctl00_ContentPlaceHolder1_trLieferscheinnummer").hide();
                $("#ctl00_ContentPlaceHolder1_txtLieferscheinnummer").val("");
            }
        }

        //Lieferanten wechseln
        function ChangeLieferant() {
            var index = $("#ctl00_ContentPlaceHolder1_ddlLieferant option:selected").index();
            $("#ctl00_ContentPlaceHolder1_Hidden1").val(0);
            __doPostBack("ChangeLiefOk", index);
        }
    </script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label">Bestellung Platinen und Zubehör</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <div id="TableQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tfoot>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tfoot>
                            <tbody>
                                <tr class="formquery">
                                    <td colspan="2" class="firstLeft active">
                                        <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2" class="firstLeft active">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="white-space: nowrap;">
                                        <asp:Label ID="lblKst" runat="server" Width="80px">Kostenstelle:</asp:Label>
                                        <asp:TextBox ID="txtKST" runat="server" Enabled="false" AutoPostBack="true" Width="100px"></asp:TextBox>
                                        <asp:Label ID="lblKSTText" runat="server" Visible="false" Style="margin-left: 10px;
                                            margin-top: 5px;"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="padding-bottom: 5px;">
                                        <table cellpadding="0" cellspacing="0" style="border-width: 0px; border-style: none;">
                                            <tr>
                                                <td width="80px">
                                                    <asp:Label ID="lblLieferant" runat="server" Width="80px">Lieferant:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:DropDownList ID="ddlLieferant" Style="width: auto" runat="server" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label1" runat="server" Visible="true">bereits geliefert:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:CheckBox ID="chkGeliefert" runat="server" Style="border-style: none;" AutoPostBack="false" />
                                                </td>
                                            </tr>
                                            <tr id="trLieferscheinnummer" runat="server" class="formquery">
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active">
                                                    Lieferscheinnummer:
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtLieferscheinnummer" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div id="divOptionen" class="firstLeft">
                                            <h3>
                                                Optional</h3>
                                            <div style="white-space: nowrap;">
                                                <table style="border-width: 0px; border-style: none;">
                                                    <tr class="formquery" id="trLiefertermin" runat="server" visible="true">
                                                        <td class="active" style="font-size: 10px; vertical-align: top;">
                                                            gewünschter
                                                            <br />
                                                            Liefertermin:
                                                        </td>
                                                        <td class="active" style="vertical-align: top;">
                                                            <asp:TextBox ID="txtLieferdatum" runat="server" Width="70px" MaxLength="10"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 100%; vertical-align: top;">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="data">
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
                                <td id="tabContainer" runat="server">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView CssClass="GridView" ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                                        AllowPaging="False" AllowSorting="True" ShowFooter="False" GridLines="None">
                                        <PagerSettings Visible="false" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMatnr" runat="server" Text='<%# Eval("ARTLIF") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Topseller" DataField="ARTBEZ" HeaderStyle-Width="315px" />
                                            <asp:TemplateField HeaderText="Bild" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Image ID="Image1" runat="server" Visible='<%# proofFileExist(Eval("ImageUrl").toString)  %>'
                                                        ImageUrl='<%# Eval("ImageUrl") %>' Height="28px"
                                                        Width="100px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stück">
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="6" runat="server" Width="50px" onChange="Javascript:onChangeSetHiddenField();"
                                                        onFocus="Javascript:this.select();" ID="txtMenge" Text='<%# Eval("MENGE") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Verpackungseinheit" DataField="VMEINS" />
                                            <asp:TemplateField HeaderText="Beschreibung">
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="30" runat="server" Width="185px" onFocus="Javascript:this.select();"
                                                        Visible='<%# Not Eval("ZUSINFO")= "" %>' ID="txtBeschreibung"
                                                        Text='<%# Eval("Beschreibung") %>'></asp:TextBox>
                                                    <asp:Label ID="Label1" runat="server" Font-Size="9px" Text="*Pflichtfeld, max. 30 Zeichen"
                                                        Visible='<%# Not Eval("ZUSINFO")= "" %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
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
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMatnr" runat="server" Text='<%# Eval("ARTLIF") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Artikelbezeichnung" DataField="ARTBEZ" HeaderStyle-Width="315px" />
                                            <asp:TemplateField HeaderText="Bild" Visible="false">
                                                <ItemTemplate>
                                                    <a class="tip" style="text-decoration: none; color: #595959;" href="#">
                                                        <asp:Image ID="Image2" runat="server" Visible='<%# proofFileExist(Eval("ImageUrl").toString)  %>'
                                                            ImageUrl="~/Images/Lupe_01.gif" Height="18px" Width="16px" />
                                                        <span style="width: 215px; background-color: WhiteSmoke; height: auto">
                                                            <div style="border: none; height: auto; width: 200px;">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ImageUrl") %>'
                                                                    Height="56px" Width="200px" />
                                                            </div>
                                                        </span></a>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                                <ItemStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stück">
                                                <ItemTemplate>
                                                    <asp:TextBox onChange="Javascript:onChangeSetHiddenField();" MaxLength="6" runat="server"
                                                        Width="50px" onFocus="Javascript:this.select();" ID="txtMenge" Text='<%# Eval("Menge") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Verpackungseinheit" DataField="VMEINS" />
                                            <asp:TemplateField HeaderText="Beschreibung">
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="30" runat="server" Width="185px" onFocus="Javascript:this.select();"
                                                        Visible='<%# Not Eval("ZUSINFO")= "" %>' ID="txtBeschreibung"
                                                        Text='<%# Eval("Beschreibung") %>'></asp:TextBox>
                                                    <asp:Label ID="Label1" runat="server" Font-Size="9px" Text="*Plichtfeld, max. 30 Zeichen"
                                                        Visible='<%# Not Eval("ZUSINFO")= "" %>'></asp:Label>
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
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                            <asp:LinkButton ID="lbParken" Text="Parken" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                            <asp:LinkButton ID="lbAusparken" Text="Ausparken" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                        <input id="BestellCheckHidden" value="1" runat="server" type="hidden" />
                        <input id="ChangeLiefHidden" value="1" runat="server" type="hidden" />
                        <input id="MessageHidden" value="1" runat="server" type="hidden" />
                        <div id="BestellungsCheck" runat="server" style="overflow: auto; max-height: 425px;
                            width: 600px;" title="Bestellung bestätigen">
                            <div class="firstLeft active" style="text-align: center; padding: 10px 5px 15px 5px;">
                                <asp:Label ID="lblBedienError" runat="server" Text="Einscannen der Bedienerkarte!"
                                    CssClass="TextError"></asp:Label>
                                <asp:Label ID="lblStatus" Visible="false" runat="server">Bestellstatus</asp:Label>
                            </div>
                            <div style="text-align: center; padding: 0px 5px 15px 5px;">
                                <asp:TextBox ID="txtBedienerkarte" Width="240px" runat="server" AutoPostBack="false"
                                    TextMode="Password"></asp:TextBox><br />
                                <asp:Label ID="lblBestellMeldung" runat="server" Style="padding: 0px 5px 15px 5px;"></asp:Label>
                            </div>
                            <div id="trInfo" runat="server" class="firstLeft active" style="text-align: center;
                                padding: 0px 5px 15px 5px;">
                                <asp:Label ID="Info" runat="server">Bitte überprüfen Sie Ihre Bestellung, ungewöhnliche Werte sind Rot markiert!<br /> Bitte korrigieren Sie gegebenenfalls!<br /></asp:Label>
                            </div>
                            <div id="trGridview" runat="server" style="padding: 0px 5px 15px 5px;">
                                <asp:GridView ID="GridView2" runat="server" AllowPaging="False" AllowSorting="True"
                                    AutoGenerateColumns="False" BackColor="White" CssClass="GridView" GridLines="None"
                                    HorizontalAlign="Center" ShowFooter="False" Style="overflow: auto; width: 75%;">
                                    <PagerSettings Visible="false" />
                                    <HeaderStyle CssClass="GridTableHead" />
                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                    <RowStyle CssClass="ItemStyle" />
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMatnr" runat="server" Text='<%# Eval("ARTLIF") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ARTBEZ" HeaderText="Artikelbezeichnung" />
                                        <asp:BoundField DataField="Menge" HeaderText="Stück" />
                                        <asp:BoundField DataField="Beschreibung" HeaderText="Beschreibung" />
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <input type="submit" style="opacity: 0; width: 1px; height: 1px" onclick='SendArtikel("ctl00_ContentPlaceHolder1_lbBestellungOk")'/>
                        </div>
                        <div id="plMessage" runat="server" horizontalalign="Center" title="Hinweis!">
                            <asp:Label ID="lblMeldung" runat="server" Style="padding: 10px 5px 15px 5px; text-align: center;
                                width: 75%;"></asp:Label>
                        </div>
                        <div id="plChangeLief" title="Hinweis!" style="width: 50%; text-align: center;">
                            <div class="firstLeft active" style="padding: 10px 5px 15px 5px;">
                                Beim Wechsel des Lieferanten werden Ihre Eingaben verloren gehen! Jetzt wechseln!
                                <input type="hidden" id="lastValue" value="" />
                                <input type="hidden" id="OKClicked" value="0"/>
                            </div>
                        </div>
                        <asp:Button ID="MPEDummy" style="display: none" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="MPEAusparken" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="Ausparken" TargetControlID="MPEDummy">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="Ausparken" HorizontalAlign="Center" runat="server" Style="display: none">
                            <table cellspacing="0" id="Table3" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                style="width: 50%; border: solid 1px #646464">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        geparkte Bestellungen:
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblErrorAusparken" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="margin-left: 40px">
                                        <asp:GridView ID="gvAusparken" runat="server" AllowPaging="False" AllowSorting="False"
                                            AutoGenerateColumns="False" BackColor="White" CssClass="GridView" GridLines="None"
                                            HorizontalAlign="Center" ShowFooter="False" Width="100%">
                                            <PagerSettings Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:BoundField DataField="Lieferant" HeaderText="Lieferant" ItemStyle-Wrap="False" />
                                                <asp:BoundField DataField="ERDAT" HeaderText="Erfasst" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="ibAusparkenTable" runat="server" Text="Ausparken" Height="16px"
                                                            Width="78px" CssClass="Tablebutton" CommandName="ausparken" CommandArgument='<%# Eval("BSTNR") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibAusparkenDelete" runat="server" Width="32" Height="32" ImageUrl="~/Images/RecycleBin.png"
                                                            CommandArgument='<%# Eval("BSTNR") %>' CommandName="löschen" />
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
                                    <td>
                                        <asp:LinkButton ID="lbAusparkenClose" Text="Schließen" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <input type="hidden" id="ihIsSaving" runat="server" value="0"/>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
