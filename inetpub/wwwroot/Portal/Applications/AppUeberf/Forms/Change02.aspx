<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02.aspx.vb" Inherits="AppUeberf.Change02" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <title></title>
    <style type="text/css">
        .style2
        {
            width: 101px;
        }
        .style3
        {
            font-size: x-small;
        }
        #TableForm
        {
            border: 4px solid #C0C0C0;
            padding: 1px 4px;
        }
        .style4
        {
            width: 64px;
        }
        .style5
        {
            width: 131px;
        }
        .style6
        {
            width: 148px;
        }
        .style7
        {
            width: 49px;
        }
        .style8
        {
            font-size: xx-small;
            font-weight: bold;
            color: #FF0000;
        }
        .style9
        {
            font-size: xx-small;
        }
    </style>
</head>
<body>
    
    <uc1:BusyIndicator ID="BusyIndicator1" runat="server" />

    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td class="PageNavigation" height="19">
                <asp:Label ID="lblHead" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td valign="top" class="style2">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtGetData" runat="server" Text="Daten holen" OnClientClick="Show_BusyBox1();"
                                CssClass="StandardButton" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtBack" runat="server" Text="Zurück" CssClass="StandardButton" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtSave" runat="server" Text="Speichern" CssClass="StandardButton"
                                Visible="False" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                <asp:Label ID="lblInfo" runat="server" Font-Bold="True"></asp:Label>
                <asp:GridView ID="grvAusgabe" runat="server" AllowSorting="True" AllowPaging="True"
                    AutoGenerateColumns="False" Width="100%" PageSize="50" BackColor="White" CssClass="tableMain">
                    <PagerSettings Position="Top" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtVbeln" runat="server" CommandName="Edit" OnClick="ibtVbeln_Click"
                                    ToolTip="Bearbeiten" ImageUrl="../../../Images/EditTableHS.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Auftragsnummer" SortExpression="Auftragsnummer">
                            <HeaderTemplate>
                                <asp:LinkButton ID="Auftragsnummer" runat="server" CommandArgument="Auftragsnummer"
                                    CommandName="Sort">Auftragsnummer</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAuftrag" runat="server" Text='<%# Bind("VBELN") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fahrt" SortExpression="Fahrt">
                            <HeaderTemplate>
                                <asp:LinkButton ID="Fahrt" runat="server" CommandArgument="Fahrt" CommandName="Sort">Fahrt</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("FAHRT") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kennzeichen" SortExpression="Kennzeichen">
                            <HeaderTemplate>
                                <asp:LinkButton ID="Kennzeichen" runat="server" CommandArgument="Kennzeichen" CommandName="Sort">Kennzeichen</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="LabelKennz" runat="server" Text='<%# Bind("KENNZEICHEN") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fahrgestellnummer" SortExpression="Fahrgestellnummer">
                            <HeaderTemplate>
                                <asp:LinkButton ID="Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                    CommandName="Sort">Fahrgestellnummer</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("FAHRG_NR") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Wunschlieferdatum" SortExpression="Wunschlieferdatum">
                            <HeaderTemplate>
                                <asp:LinkButton ID="Wunschlieferdatum" runat="server" CommandArgument="Wunschlieferdatum"
                                    CommandName="Sort">Wunschlieferdatum</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VDATU", "{0:d}")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="StartPLZ" SortExpression="StartPLZ">
                            <HeaderTemplate>
                                <asp:LinkButton ID="StartPLZ" runat="server" CommandArgument="StartPLZ" CommandName="Sort">Start-PLZ</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("POST_CODE1_START") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="StartOrt" SortExpression="StartOrt">
                            <HeaderTemplate>
                                <asp:LinkButton ID="StartOrt" runat="server" CommandArgument="StartOrt" CommandName="Sort">Start-Ort</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("CITY1_START") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ZielPLZ" SortExpression="ZielPLZ">
                            <HeaderTemplate>
                                <asp:LinkButton ID="ZielPLZ" runat="server" CommandArgument="ZielPLZ" CommandName="Sort">Ziel-PLZ</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblZielPLZ" runat="server" Text='<%# Bind("POST_CODE1_ZIEL")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ZielOrt" SortExpression="ZielOrt">
                            <HeaderTemplate>
                                <asp:LinkButton ID="ZielOrt" runat="server" CommandArgument="ZZTYP_SCHL" CommandName="Sort">Ziel-Ort</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblZielOrt" runat="server" Text='<%# Bind("CITY1_ZIEL")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="TextExtraLarge" />
                    <HeaderStyle CssClass="GridTableHead" />
                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                </asp:GridView>
                <table id="TableForm" runat="server" visible="false" width="910px" bgcolor="White"
                    cellspacing="0">
                    <tr>
                        <td bgcolor="Silver">
                            <b>Auftrag</b>
                            <asp:Label ID="lblAuftrag" runat="server"></asp:Label>
                        </td>
                        <td bgcolor="Silver">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblKennzeichen" runat="server" Text="Kennzeichen " Style="font-weight: 700"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtKennzeichen" runat="server" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <b>Fzg.-Ident-Nr.</b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" Width="200px" MaxLength="17"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td nowrap="nowrap">
                            <span class="style3">Das Fahrzeug wurde im Rahmen der Übernahme fotografisch dokumentiert.
                            </span>
                            <br class="style3" />
                            <asp:RadioButtonList ID="rblFoto" runat="server" RepeatDirection="Horizontal" BorderColor="White"
                                BorderStyle="Solid" BorderWidth="1px">
                                <asp:ListItem Value="1">ja</asp:ListItem>
                                <asp:ListItem Value="0">nein</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" bgcolor="Silver">
                            Erschwerte Übernahmebedingungen:
                            <asp:CheckBoxList ID="cblUebernahmebedingungen" runat="server" RepeatDirection="Horizontal"
                                BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                <asp:ListItem>Keine</asp:ListItem>
                                <asp:ListItem>Verschmutzung</asp:ListItem>
                                <asp:ListItem Value="Regen">Regen/Nässe</asp:ListItem>
                                <asp:ListItem>Dunkelheit</asp:ListItem>
                                <asp:ListItem>Parkhaus</asp:ListItem>
                                <asp:ListItem Value="Schnee">Schnee/Eis</asp:ListItem>
                                <asp:ListItem Value="Zeitdruck">Zeitdruck des Übergebenden</asp:ListItem>
                                <asp:ListItem>Sonstiges</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label8" runat="server" Text="Unfall + Technik: " Font-Bold="True"
                                Font-Underline="True"></asp:Label>
                            <asp:Label ID="Label9" runat="server" Text="Sind Vorschäden oder technische Mängel bekannt? (Bei Logistikgutachten immer korrekt ausfüllen!)"></asp:Label>
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblUnfall" runat="server" RepeatDirection="Horizontal" BorderColor="White"
                                            BorderStyle="Solid" BorderWidth="1px" TextAlign="Left">
                                            <asp:ListItem Value="1">Unfall: ja</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="true">nein</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblTechMaengel" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="White" BorderStyle="Solid" BorderWidth="1px" TextAlign="Left">
                                            <asp:ListItem Value="1">Technische Mängel: ja</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="true">nein</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Font-Bold="True" Text="Austausch?"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblAustauschMotor" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="White" BorderStyle="Solid" BorderWidth="1px" TextAlign="Left">
                                            <asp:ListItem Value="1">Motor: ja</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="true">nein</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblAustauschGetriebe" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="White" BorderStyle="Solid" BorderWidth="1px" TextAlign="Left">
                                            <asp:ListItem Value="1">Getriebe: ja</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="true">nein</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblTacho" runat="server" RepeatDirection="Horizontal" BorderColor="White"
                                            BorderStyle="Solid" BorderWidth="1px" TextAlign="Left">
                                            <asp:ListItem Value="1">Tacho: ja</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="true">nein</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="Silver" colspan="2">
                            <table width="100%">
                                <tr>
                                    <td class="style4">
                                        <b>Fehlfahrt</b>
                                    </td>
                                    <td class="style6">
                                        <asp:RadioButtonList ID="rblFehlfahrt" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1">ja</asp:ListItem>
                                            <asp:ListItem Value="0">nein</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td bgcolor="Silver" class="style5">
                                        Wartezeit in Minuten
                                    </td>
                                    <td class="style7">
                                        <asp:TextBox ID="txtWartezeit" runat="server" MaxLength="4" Width="40px"></asp:TextBox>
                                    </td>
                                    <td>
                                        min
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td valign="bottom">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td>
                                        <b>Tachostand&nbsp; bei Übernahme </b>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtTachostandUebernahme" runat="server" MaxLength="6" 
                                            Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <b>km</b></td>
                                    <td style="padding-left: 10px">
                                        <b>Tachostand am Zielort</b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTachostand" runat="server" MaxLength="6" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <b>km</b>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td class="style3">
                                        Hiermit bestätige ich, dass während der von mir durchgeführten Überführungsfahrt
                                        keine für mich sichtbaren oder anderweitig wahrnehmbaren Schäden entstanden sind.
                                        Ich habe das Fahrzeug nach Beendigung der Fahrt eingehend untersucht und keine während
                                        der Fahrt verursachten Schäden feststellen können. <b>(Logistikpartner DAD)</b>
                                    </td>
                                    <td valign="bottom">
                                        <asp:RadioButtonList ID="rblSchaedBeiUeb" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="White" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1">ja</asp:ListItem>
                                            <asp:ListItem Value="0">nein</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td  colspan="2" bgcolor="Silver">
                            <b>Funktionschecks nach Übergabe </b>(Bei Logistikgutachten immer korrekt ausfüllen!)
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" bgcolor="Silver">
                            <table>
                                <tr>
                                    <td>
                                        Ölstand
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblOelstand" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="padding-left:10px">
                                        Probefahrt
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblProbefahrt" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="padding-left: 10px">
                                        Fensterheber mech.
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblFensterheber" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Motorlauf Otto/Diesel
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblMotorlauf" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="padding-left: 10px">
                                        Radlager
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblRadlager" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="padding-left: 10px">
                                        Verdeck mech./elektr.
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblVerdeck" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Getriebe manuell/auto
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblGetriebe" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="padding-left: 10px">
                                        Gebläse
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblGeblaese" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="padding-left: 10px">
                                        Schlüssel
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblSchluessel" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Anfahrtest
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblAnfahrtest" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="padding-left: 10px">
                                        Klimaanlage/-automatik
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblKlima" runat="server" RepeatDirection="Horizontal" BorderColor="Silver"
                                            BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Kupplung
                                    </td>
                                    <td>
                                        
                                        <asp:RadioButtonList ID="rblKupplung" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                        
                                    </td>
                                    <td style="padding-left: 10px">
                                        Radio/Navi
                                    </td>
                                    <td>
                                    
                                        <asp:RadioButtonList ID="rblRadioNavi" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1" Selected="True">i.O.</asp:ListItem>
                                            <asp:ListItem Value="0">nicht i.O.</asp:ListItem>
                                        </asp:RadioButtonList>
                                    
                                    </td>
                                    <td>
                                    
                                    </td>
                                    <td>
                                    
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b>Weitere Dienstleistungen/Auslagen</b><span class="style9"> </span>
                            <span class="style8">(Alle Beträge sind ohne MwSt. zu erfassen!)</span></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td style="padding-right:40px">
                                        Betankung
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBetankung" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        €
                                    </td>
                                    <td style="padding-left:190px">
                                        Kennzeichen De-/Montage
                                    </td>
                                    <td>
                                        
                                        <asp:RadioButtonList ID="rblKennzMontage" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="White" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1">ja</asp:ListItem>
                                            <asp:ListItem Value="0">nein</asp:ListItem>
                                        </asp:RadioButtonList>
                                        
                                    </td>
                                    
                                </tr>
                            <tr>
                                    <td style="padding-right:40px">
                                        Fahrzeugwäsche</td>
                                    <td>
                                        <asp:TextBox ID="txtFahrzeugwaesche" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        €
                                    </td>
                                    <td style="padding-left:190px">
                                        Reifenhandling</td>
                                    <td>
                                        
                                        <asp:RadioButtonList ID="rblReifenhandling" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="White" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1">ja</asp:ListItem>
                                            <asp:ListItem Value="0">nein</asp:ListItem>
                                        </asp:RadioButtonList>
                                        
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td style="padding-right:40px">
                                        Innenreinigung                                     </td>
                                    <td>
                                        <asp:TextBox ID="txtInnenreinigung" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        €
                                    </td>
                                    <td style="padding-left:190px">
                                        Dispokontakt notwendig</td>
                                    <td>
                                        
                                        <asp:RadioButtonList ID="rblDispokontakt" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="White" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1">ja</asp:ListItem>
                                            <asp:ListItem Value="0">nein</asp:ListItem>
                                        </asp:RadioButtonList>
                                        
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td style="padding-right:40px">
                                        Maut                                     </td>
                                    <td>
                                        <asp:TextBox ID="txtMaut" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        €
                                    </td>
                                    <td style="padding-left:190px">
                                        Vorholung/Nachbringung</td>
                                    <td>
                                        
                                        <asp:RadioButtonList ID="rblVorholung" runat="server" RepeatDirection="Horizontal"
                                            BorderColor="White" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:ListItem Value="1">ja</asp:ListItem>
                                            <asp:ListItem Value="0">nein</asp:ListItem>
                                        </asp:RadioButtonList>
                                        
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td style="padding-right:40px">
                                        Öl</td>
                                    <td>
                                        <asp:TextBox ID="txtOel" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        €</td>
                                    <td style="padding-left:190px">
                                        &nbsp;</td>
                                    <td>
                                        
                                        &nbsp;</td>
                                    
                                </tr>
                                <tr>
                                    <td style="padding-right:40px">
                                        Hotel/Übernachtung ohne Frühstück</td>
                                    <td>
                                        <asp:TextBox ID="txtUebernachtung" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        €</td>
                                    <td style="padding-left:190px">
                                        &nbsp;</td>
                                    <td>
                                        
                                        &nbsp;</td>
                                    
                                </tr>
                                <tr>
                                    <td style="padding-right:40px">
                                        sonstige Auslagen</td>
                                    <td>
                                        <asp:TextBox ID="txtSonstigeAuslagen" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        €</td>
                                    <td style="padding-left:190px">
                                        &nbsp;</td>
                                    <td>
                                        
                                        &nbsp;</td>
                                    
                                </tr>
                            </table>
                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" bgcolor="silver">
                            <table>
                                <tr>
                                <td>
                                    <b>Übernahme Uhrzeit</b>
                                </td>
                                <td style="padding-left: 8px">
                                    <asp:TextBox ID="txtUebernahmeUhrzeit1" runat="server" Width="25px" 
                                        MaxLength="2"></asp:TextBox>
                                </td>
                                <td>
                                
                                    <b>:</b></td>
                                <td>
                                
                                    <asp:TextBox ID="txtUebernahmeUhrzeit2" runat="server" Width="25px" 
                                        MaxLength="2"></asp:TextBox>
                                
                                </td>
                                <td style="padding-left:220px">
                                
                                    <b>Übergabe Datum</b></td>
                                <td>
                                
                                    <asp:TextBox ID="txtUebergabeDatum" runat="server" MaxLength="10" 
                                        ToolTip="Format: TT.MM.JJJJ" Width="80px"></asp:TextBox>
                                
                                </td>
                                <td>
                                
                                    <b>Übergabe Uhrzeit</b></td>
                                <td>
                                
                                    <asp:TextBox ID="txtUebergabeZeit1" runat="server" MaxLength="2" Width="25px"></asp:TextBox>
                                
                                </td>
                                <td>
                                
                                    <b>:</b></td>
                                <td>
                                
                                    <asp:TextBox ID="txtUebergabeZeit2" runat="server" MaxLength="2" Width="25px"></asp:TextBox>
                                
                                </td>
                                </tr>
                               
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td style="padding-right: 40px">
                                        <b>Bemerkungen</b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBemerkungen" runat="server" MaxLength="79" Width="600px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
