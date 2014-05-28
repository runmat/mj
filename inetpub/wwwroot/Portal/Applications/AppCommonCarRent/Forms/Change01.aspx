<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01.aspx.vb" Inherits="AppCommonCarRent.Change01" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">

    <uc1:BusyIndicator runat="server" />

    <form id="Form1" method="post" runat="server">

    <script language="javascript" id="ScrollPosition">
<!--

        function sstchur_SmartScroller_GetCoords() {

            var scrollX, scrollY;

            if (document.all) {
                if (!document.documentElement.scrollLeft)
                    scrollX = document.body.scrollLeft;
                else
                    scrollX = document.documentElement.scrollLeft;

                if (!document.documentElement.scrollTop)
                    scrollY = document.body.scrollTop;
                else
                    scrollY = document.documentElement.scrollTop;
            }
            else {
                scrollX = window.pageXOffset;
                scrollY = window.pageYOffset;
            }



            document.forms["Form1"].xCoordHolder.value = scrollX;
            document.forms["Form1"].yCoordHolder.value = scrollY;

        }

        function sstchur_SmartScroller_Scroll() {


            var x = document.forms["Form1"].xCoordHolder.value;
            var y = document.forms["Form1"].yCoordHolder.value;
            window.scrollTo(x, y);

        }

        window.onload = sstchur_SmartScroller_Scroll;
        window.onscroll = sstchur_SmartScroller_GetCoords;
        window.onkeypress = sstchur_SmartScroller_GetCoords;
        window.onclick = sstchur_SmartScroller_GetCoords;
  
// -->
    </script>

    <input type="hidden" id="xCoordHolder" runat="server" />
    <input type="hidden" id="yCoordHolder" runat="server" />
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TaskTitle" valign="top" colspan="2">
                                <asp:LinkButton ID="lb_zurueck" runat="server" Visible="True">lb_zurueck</asp:LinkButton>
                            </td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table id="Table1" cellspacing="0" cellpadding="5" width="100%">
                    <tr class="TaskTitle1">
                        <td colspan="3" align="left">
                            <asp:LinkButton runat="server" CssClass="StandardButtonTable" ID="lb_AnzeigeSelektion" Font-Underline="true" 
                                Font-Bold="true" Font-Size="Larger" Text="Selektion"></asp:LinkButton>
                            
                            <asp:UpdatePanel runat="server" ID="upAnzeigeAnzahlErgebnisse">
                                <ContentTemplate>
                                    <asp:Label ID="lblAnzeigeAnzahlErgebnisse" runat="server">Anzahl Treffer:</asp:Label></ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" width="100%" nowrap="nowrap" style="border-color: #f5f5f5; border-style: solid;
                            border-width: 3; background: #FFFFCC;">
                            <asp:UpdatePanel ID="upSelektionInfo" runat="server">
                                <ContentTemplate>
                                    <table border="0" runat="server" cellpadding="3" align="left" width="80%" visible="false"
                                        id="tableSelektionInfo">
                                        <tr align="left">
                                            <td colspan="1">
                                                <asp:Label ID="lbl_infoSelektionHersteller" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                            <td colspan="1" style="font-weight: bold">
                                                <asp:Label ID="lblDataSelektionHersteller" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                            <td colspan="1">
                                                <asp:Label ID="lbl_infoSelektionTyp" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                            <td colspan="1" style="font-weight: bold">
                                                <asp:Label ID="lblDataSelektionTyp" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                            <td colspan="1">
                                                <asp:Label ID="lbl_infoSelektionAufbauart" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                            <td colspan="1" style="font-weight: bold">
                                                <asp:Label ID="lblDataSelektionAufbauart" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Liefermonat:
                                            </td>
                                            <td style="font-weight: bold">
                                                <asp:Label ID="lblDataSelektionLiefermonat" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                            <td>
                                                Bereifung:
                                            </td>
                                            <td style="font-weight: bold">
                                                <asp:Label ID="lblDataSelektionBereifung" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                            <td>
                                                Getriebe:
                                            </td>
                                            <td style="font-weight: bold">
                                                <asp:Label ID="lblDataSelektionGetriebe" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Kraftstoffart:
                                            </td>
                                            <td style="font-weight: bold">
                                                <asp:Label ID="lblDataSelektionKraftstoffart" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                            <td>
                                                Navi:
                                            </td>
                                            <td style="font-weight: bold">
                                                <asp:Label ID="lblDataSelektionNavi" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                            <td>
                                                Farbe:
                                            </td>
                                            <td style="font-weight: bold">
                                                <asp:Label ID="lblDataSelektionFarbe" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="1">
                                                <asp:Label ID="lbl_infoSelektionAbsender" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                            <td colspan="5" style="font-weight: bold">
                                                <asp:Label ID="lblDataSelektionAbsender" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="1">
                                                <asp:Label ID="lbl_infoSelektionStandort" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                            <td colspan="5" style="font-weight: bold">
                                                <asp:Label ID="lblDataSelektionStandort" runat="server">
                                                
                                                </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:UpdatePanel ID="upSelektion" runat="server">
                                <ContentTemplate>
                                    <table width="100%"  cellpadding="3" style="border-color: #f5f5f5; border-style: solid;
                                        border-width: 3; background: #FFFFCC;" runat="server" visible="true" id="tableSelektion">
                                        <tr>
                                            <td nowrap="nowrap" align="left" colspan="2">
                                                <asp:LinkButton ID="lb_SelektionsschrittBack" runat="server" CssClass="StandardButton">Selektionsschritt zurück</asp:LinkButton>
                                                &nbsp;
                                                <asp:LinkButton ID="lb_SelektionZurueckSetzen" runat="server" CssClass="StandardButton">neue Selektion</asp:LinkButton>
                                                &nbsp;
                                                <asp:CheckBox Text="übernommene Fahrzeuge ausschließen" ID="chkUebernahmeAusschluss"
                                                    AutoPostBack="true" runat="server" Enabled="false" Checked="true" />
                                            </td>
                                            <td colspan="4">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="BeschriftungsSpalte">
                                                Kraftstoffart:
                                            </td>
                                            <td>
                                                <asp:RadioButtonList RepeatDirection="Horizontal" TextAlign="left" AutoPostBack="true"
                                                    ID="rblKraftstoffart" runat="server">
                                                </asp:RadioButtonList>
                                            </td>
                                            <td class="BeschriftungsSpalte">
                                                Bereifung:
                                            </td>
                                            <td>
                                                <asp:RadioButtonList TextAlign="Left" RepeatDirection="Horizontal" AutoPostBack="true"
                                                    ID="rblBereifung" runat="server">
                                                </asp:RadioButtonList>
                                            </td>
                                            <td class="BeschriftungsSpalte">
                                                Liefermonat:
                                            </td>
                                            <td>
                                                <asp:DropDownList Width="150px" ID="ddlLiefermonat" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="BeschriftungsSpalte">
                                                Navi:
                                            </td>
                                            <td>
                                                <asp:RadioButtonList RepeatDirection="Horizontal" TextAlign="left" AutoPostBack="true"
                                                    ID="rblNavi" runat="server">
                                                </asp:RadioButtonList>
                                            </td>
                                            <td class="BeschriftungsSpalte">
                                                Getriebe:
                                            </td>
                                            <td>
                                                <asp:RadioButtonList RepeatDirection="Horizontal" TextAlign="left" AutoPostBack="true"
                                                    ID="rblGetriebe" runat="server">
                                                </asp:RadioButtonList>
                                            </td>
                                            <td class="BeschriftungsSpalte">
                                                Farbe:
                                            </td>
                                            <td>
                                                <asp:DropDownList Width="150px" ID="ddlFarbe" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="BeschriftungsSpalte">
                                                <asp:Label runat="server" ID="lbl_Standort"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:ListBox AutoPostBack="true" Rows="6" ID="lbxStandort" Width="100%" runat="server">
                                                </asp:ListBox>
                                                <cc1:ListSearchExtender Enabled="false" PromptCssClass="ListSearchExt" ID="ListSearchExtender1"
                                                    TargetControlID="lbxStandort" IsSorted="true" PromptPosition="Top" PromptText="PLZ-Suche"
                                                    runat="server">
                                                </cc1:ListSearchExtender>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="BeschriftungsSpalte">
                                                <asp:Label runat="server" ID="lbl_Hersteller"></asp:Label>
                                            </td>
                                            <td >
                                                 <table cellpadding="3">
                                                    <tr>
                                                    <td>
                                                <asp:ListBox Width="200" Rows="6" AutoPostBack="true" ID="lbxHersteller" runat="server">
                                                </asp:ListBox>
                                                </td>
                                                
                                                        <td class="BeschriftungsSpalte">
                                                              <asp:Label runat="server" ID="lbl_Typ"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:ListBox AutoPostBack="true" Rows="6" Width="200" ID="lbxTyp" runat="server">
                                                </asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                               
                                            </td>
                                            <td class="BeschriftungsSpalte">
                                                 <asp:Label runat="server" ID="lbl_Aufbauart"></asp:Label>
                                            </td>
                                            <td>
                                                 <asp:ListBox AutoPostBack="true" Rows="6" Width="200" ID="lbxAufbauart" runat="server">
                                                </asp:ListBox>
                                            </td>
                                           <td>
                                           &nbsp;
                                           </td>
                                           <td width="50%">
                                           &nbsp;
                                           </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="BeschriftungsSpalte">
                                                <asp:Label runat="server" ID="lbl_Absender"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:ListBox AutoPostBack="true" Rows="6" Width="100%" ID="lbxAbsender" runat="server">
                                                </asp:ListBox>
                                                <cc1:ListSearchExtender PromptCssClass="ListSearchExt" ID="ListSearchExtender2" TargetControlID="lbxAbsender"
                                                    IsSorted="true" PromptPosition="Top" PromptText="PLZ-Suche" runat="server">
                                                </cc1:ListSearchExtender>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table id="Table3" cellspacing="0" cellpadding="5" width="100%" border="0">
                    <tr class="TaskTitle2">
                        <td colspan="3" align="left" nowrap="nowrap">
                           
                            <asp:LinkButton cssClass="StandardButtonTable"  runat="server" ID="lb_AnzeigeAuswahl" Font-Underline="true" 
                                Font-Bold="true" Font-Size="Larger" Text="Auswahl" ></asp:LinkButton>
                           
                            <asp:UpdatePanel runat="server" ID="upAnzahlAuswahl">
                                <ContentTemplate>
                                    <asp:Label ID="lblAnzeigeAnzahlAuswahl" runat="server">Anzahl Treffer:</asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table width="100%" cellpadding="3" style="border-color: #f5f5f5; border-style: solid;
                                border-width: 3;" runat="server" visible="False" id="tableAuswahl">
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="upGridAnzeige" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblGvFahrzeugeNoData" runat="server" Visible="false" Text="keine Daten vorhanden"></asp:Label>
                                                <asp:GridView ID="gvFahrzeuge" runat="server" AutoGenerateColumns="False" Visible="false"
                                                    AllowSorting="True" CssClass="tableMain" AllowPaging="false" BackColor="White"
                                                    Width="100%">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="White" CssClass="GridTableHead" />
                                                    <PagerStyle Font-Size="12pt" Font-Bold="True" HorizontalAlign="Left" Wrap="False">
                                                    </PagerStyle>
                                                    <PagerSettings Mode="Numeric" Position="Top" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEQUNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Auswahl" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <asp:ImageButton BackColor="White" runat="server" ID="imgbAllauswaehlen" Height="14"
                                                                    CommandName="alleAuwaehlen" Width="16" ToolTip="alle markieren" ImageUrl="../../../Images/Confirm_Mini2.gif" />
                                                                <asp:ImageButton BackColor="White" runat="server" ToolTip="alle Markierungen aufheben" ID="imgbAuswahlloeschen" Height="14"
                                                                    CommandName="alleLoeschen" Width="16" ImageUrl="../../../Images/Not_Confirm_Mini2.gif" />&nbsp;
                                                                <asp:ImageButton runat="server" ID="imgInsertAll" ToolTip="alle markierten übernehmen" Height="15" CommandName="insertAll"
                                                                    Width="15" ImageUrl="../../../Images/select.gif" />
                                                                <asp:LinkButton ID="col_Auswahl" runat="server" CommandName="Sort" CommandArgument="Ausgewaehlt">col_Auswahl</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkAuswahl" OnCheckedChanged="chk_Auswahl_CheckedChanged" CausesValidation="true"
                                                                    AutoPostBack="true" ToolTip="dieses Fahrzeug markieren" Checked='<%# DataBinder.Eval(Container, "DataItem.Ausgewaehlt")="X" %>'
                                                                    runat="server" />
                                                                &nbsp;&nbsp;
                                                                <asp:ImageButton runat="server" ToolTip="dieses Fahrzeug übernehmen"  ID="imgbInsertOne" Height="14" CommandName="insertOne"
                                                                    Width="14" ImageUrl="../../../Images/select.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Hersteller">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="Hersteller">col_Hersteller</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label21q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Hersteller") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Typ">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="Typ">col_Typ</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label21qwqwe" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Typ") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:HyperLink Target="_blank" ID="lnkFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                                    runat="server"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Farbe">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Farbe" runat="server" CommandName="Sort" CommandArgument="Farbe">col_Farbe</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label22paxevwq" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Farbe") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Eingangsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Eingangsdatum" runat="server" CommandName="Sort" CommandArgument="Eingangsdatum">col_Eingangsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum","{0:dd.MM.yyyy}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Referenz">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz" runat="server" CommandName="Sort" CommandArgument="Referenz">col_Referenz</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Getriebe" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Getriebe" runat="server" CommandName="Sort" CommandArgument="Getriebe">col_Getriebe</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label12aq" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Getriebe") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Bereifung" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Bereifung" runat="server" CommandName="Sort" CommandArgument="Bereifung">col_Bereifung</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label12aqq2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bereifung") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Reifengroesse">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Reifengroesse" runat="server" CommandName="Sort" CommandArgument="Reifengroesse">col_Reifengroesse</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label12aqq2q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Reifengroesse") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Kraftstoffart">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kraftstoffart" runat="server" CommandName="Sort" CommandArgument="Kraftstoffart">col_Kraftstoffart</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label12aqq2qsw" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kraftstoffart") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Navi" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Navi" runat="server" CommandName="Sort" CommandArgument="Navi">col_Navi</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label12aqq2q1sw" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Navi") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_ZBIINummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_ZBIINummer" runat="server" CommandName="Sort" CommandArgument="Navi">col_ZBIINummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label12aqq2q1ss1w" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZBIINummer") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_CoC" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_CoC" runat="server" CommandName="Sort" CommandArgument="CoC">col_CoC</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="cbx_COC" Enabled="false" Checked='<%# DataBinder.Eval(Container, "DataItem.CoC")="X" %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <td valign="top">
            <table id="Table2" cellspacing="0" cellpadding="5" width="100%" border="0">
                <tr class="TaskTitle3">
                    <td colspan="3" align="left">
                        <asp:LinkButton runat="server" CssClass="StandardButtonTable" ID="lb_Uebernommen" Font-Underline="true" 
                            Font-Bold="true" Font-Size="Larger" Text="Auswahl"></asp:LinkButton>
                       
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                                               <asp:Label ID="lblAnzeigeAnzahlUebernommen" runat="server">Anzahl Treffer:</asp:Label></ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="3" style="border-color: #f5f5f5; border-style: solid;
                            border-width: 3;" runat="server" visible="False" id="tableUebernommen">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblGvUebernommenNoData" runat="server" Visible="false" Text="keine Daten vorhanden"></asp:Label>
                                    <asp:UpdatePanel ID="upUebernommen" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvUebernommen" runat="server" AutoGenerateColumns="False" Visible="false"
                                                AllowSorting="True" CssClass="tableMain" AllowPaging="false" BackColor="White"
                                                Width="100%">
                                                <HeaderStyle HorizontalAlign="Left" ForeColor="White" CssClass="GridTableHead" />
                                                <PagerStyle Font-Size="12pt" Font-Bold="True" HorizontalAlign="Left" Wrap="False">
                                                </PagerStyle>
                                                <PagerSettings Mode="Numeric" Position="Top" />
                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEQUNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Loeschen" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Loeschen" runat="server">col_Loeschen</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ToolTip="Dieses Fahrzeug aus Übernahmetabelle entfernen" ID="imgDelete" Height="14" CommandName="delete" Width="14"
                                                                ImageUrl="../../../Images/loesch.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Hersteller">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="Hersteller">col_Hersteller</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label21q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Hersteller") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Typ">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="Typ">col_Typ</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label21qwqwe" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Typ") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Fahrgestellnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:HyperLink Target="_blank" ID="lnkFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                                runat="server"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Farbe">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Farbe" runat="server" CommandName="Sort" CommandArgument="Farbe">col_Farbe</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label22paxevwq" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Farbe") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Eingangsdatum">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Eingangsdatum" runat="server" CommandName="Sort" CommandArgument="Eingangsdatum">col_Eingangsdatum</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum","{0:dd.MM.yyyy}") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Referenz">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Referenz" runat="server" CommandName="Sort" CommandArgument="Referenz">col_Referenz</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Getriebe" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Getriebe" runat="server" CommandName="Sort" CommandArgument="Getriebe">col_Getriebe</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label12aq" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Getriebe") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Bereifung" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-VerticalAlign="Middle">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Bereifung" runat="server" CommandName="Sort" CommandArgument="Bereifung">col_Bereifung</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label12aqq2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bereifung") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Reifengroesse">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Reifengroesse" runat="server" CommandName="Sort" CommandArgument="Reifengroesse">col_Reifengroesse</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label12aqq2q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Reifengroesse") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Kraftstoffart">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Kraftstoffart" runat="server" CommandName="Sort" CommandArgument="Kraftstoffart">col_Kraftstoffart</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label12aqq2qsw" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kraftstoffart") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_Navi" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Navi" runat="server" CommandName="Sort" CommandArgument="Navi">col_Navi</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label12aqq2q1sw" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Navi") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_ZBIINummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZBIINummer" runat="server" CommandName="Sort" CommandArgument="Navi">col_ZBIINummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label12aqq2q1ss1w" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZBIINummer") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="col_CoC" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_CoC" runat="server" CommandName="Sort" CommandArgument="CoC">col_CoC</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="cbx_COC" Enabled="false" Checked='<%# DataBinder.Eval(Container, "DataItem.CoC")="X" %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:LinkButton ID="lb_weiter" OnClientClick="Show_BusyBox1();" Text="Weiter" runat="server"
                                        CssClass="StandardButton"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        </tr>
        <tr>
            <td align="left" colspan="1">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
    </table> </form>
</body>
</html>
