<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_1.aspx.vb" Inherits="AppCommonCarRent.Change01_1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:styles id="ucStyles" runat="server">
    </uc1:styles>
    
</head>
<body leftmargin="0" topmargin="0" text="1">
    
    <uc1:BusyIndicator ID="BusyIndicator1" runat="server" />

    <form id="Form1" method="post" runat="server">

    <script language="javascript">
<!--
var prm = Sys.WebForms.PageRequestManager.getInstance(); 

prm.add_initializeRequest(initializeRequest);

var postbackElement; 

function initializeRequest(sender, args) { 

if (prm.get_isInAsyncPostBack()) 
{

//debugger
args.set_cancel(true); 

}

}

// -->
    </script>

    <script language="javascript" id="ScrollPosition">
<!--
  
		
	function sstchur_SmartScroller_GetCoords()
   {
		
      var scrollX, scrollY;
      
      if (document.all)
      {
         if (!document.documentElement.scrollLeft)
            scrollX = document.body.scrollLeft;
         else
            scrollX = document.documentElement.scrollLeft;
               
         if (!document.documentElement.scrollTop)
            scrollY = document.body.scrollTop;
         else
            scrollY = document.documentElement.scrollTop;
      }   
      else
      {
         scrollX = window.pageXOffset;
         scrollY = window.pageYOffset;
      }
      
      
		
      document.forms["Form1"].xCoordHolder.value = scrollX;
      document.forms["Form1"].yCoordHolder.value = scrollY;
     
   }
   
   function sstchur_SmartScroller_Scroll()
   {
  
			
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
    <asp:ScriptManager ID="scriptmanager1" EnableScriptGlobalization="true" runat="server">
    </asp:ScriptManager>
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:header id="ucHeader" runat="server">
                </uc1:header>
            </td>
        </tr>
        <tr>
            <td colspan="3">
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
                                &nbsp;<asp:LinkButton ID="lb_zurueck" runat="server" Visible="True">lb_zurueck</asp:LinkButton>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
             
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="3">
                <table id="Table3" cellspacing="0" cellpadding="5" width="100%" border="0">
                    <tr class="TaskTitle2">
                        <td colspan="3" align="left" nowrap="nowrap">
                            <asp:UpdatePanel ID="upKopfSelektion" runat="server">
                                <ContentTemplate>
                                    <table runat="server" id="tableKopfselekton" cellpadding="3">
                                        <tr style="font-weight: bold">
                                            <td>
                                                <asp:Label ID="lbl_Lizensnehmer" runat="server">Lizensnehmer</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Deckungskarte" runat="server">Deckungskarte</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Branding" runat="server">Branding</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Mindesthaltedauer" runat="server">Mindesthaltedauer</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Zulassungsdatum" runat="server">Zulassungsdatum</asp:Label>
                                            </td>
                                            <td rowspan="2" valign="bottom">
                                                <asp:ImageButton runat="server" ToolTip="Daten in alle markierten Fahrzeuge eintragen" ID="imgInsertAll" Height="15" Width="15" ImageUrl="../../../Images/select.gif" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlLizensnehmer" AutoPostBack="true" runat="server" Width="200px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender PromptCssClass="ListSearchExt" ID="ListSearchExtender2" TargetControlID="ddlLizensnehmer"
                                                    IsSorted="true" PromptPosition="Top" PromptText="Namen-Suche" runat="server">
                                                </cc1:ListSearchExtender>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDeckungskarte" AutoPostBack="true" runat="server" Enabled="False"
                                                    Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBranding" runat="server" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMindesthaltedauer" MaxLength="3" Width="30px" runat="server">180</asp:TextBox>
                                                <asp:RangeValidator ControlToValidate="txtMindesthaltedauer" ID="rvMindesthaltedauer"
                                                    MinimumValue="1" MaximumValue="999" runat="server" Display="None" ErrorMessage="Mindesthaltedauer von 1-999 Tagen"></asp:RangeValidator>
                                                <cc1:ValidatorCalloutExtender Enabled="true" ID="vceMindesthaltedauer2" Width="350px"
                                                    runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="rvMindesthaltedauer">
                                                </cc1:ValidatorCalloutExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteMindesthaltedauer" runat="server" TargetControlID="txtMindesthaltedauer"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtZulassungsdatum" Width="80px" runat="server"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtZulassungsdatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtZulassungsdatum">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="meeZulassungsdatum" runat="server" TargetControlID="txtZulassungsdatum"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="mevZulassungsdatum" runat="server" ControlToValidate="txtZulassungsdatum"
                                                    ControlExtender="meeZulassungsdatum" Display="none" IsValidEmpty="True" Enabled="true"
                                                    EmptyValueMessage="Bitte geben Sie ein gültiges Zulassungsdatum ein" InvalidValueMessage="Bitte geben Sie ein gültiges Zulassungsdatum ein">
                                                                      
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                      
                                                </cc1:MaskedEditValidator>
                                                <cc1:ValidatorCalloutExtender Enabled="true" ID="vceZulassungsdatum" Width="350px"
                                                    runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="mevZulassungsdatum">
                                                </cc1:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table width="100%" cellpadding="3" style="border-color: #f5f5f5; border-style: solid;
                                border-width: 3;" runat="server" visible="true" id="tableAuswahl">
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="upGridAnzeige" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel Visible="false" ID="panelEquiDetails" runat="server" BorderColor="Black"
                                                    BorderStyle="Solid" BorderWidth="1" Width="100%">
                                                    <asp:UpdatePanel RenderMode="Inline" runat="server" ID="upEquiDetails" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table border="0" runat="server" cellpadding="3" align="left" width="100%" id="tableSelektionInfo">
                                                                <tr align="left">
                                                                    <td colspan="1">
                                                                        <asp:Label ID="lbl_infoSelektionHersteller" Text="lbl_infoSelektionHersteller" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="1" style="font-weight: bold">
                                                                        <asp:Label ID="lblDataSelektionHersteller" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="1">
                                                                        <asp:Label ID="lbl_infoSelektionTyp" Text="lbl_infoSelektionTyp" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="1" style="font-weight: bold">
                                                                        <asp:Label ID="lblDataSelektionTyp" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td colspan="1" width="50%">
                                                                        &nbsp;
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
                                                                        <asp:Label ID="lbl_infoSelektionAbsender" Text="lbl_infoSelektionAbsender" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="5" style="font-weight: bold">
                                                                        <asp:Label ID="lblDataSelektionAbsender" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1">
                                                                        <asp:Label ID="lbl_infoSelektionStandort" Text="lbl_infoSelektionStandort" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="5" style="font-weight: bold">
                                                                        <asp:Label ID="lblDataSelektionStandort" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1">
                                                                        <asp:Label ID="lbl_InfoLizensnehmer" Text="lbl_InfoLizensnehmer" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="1">
                                                                        <asp:DropDownList ID="ddlDataLinznehmer" AutoPostBack="true" runat="server" Width="200px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1">
                                                                        <asp:Label ID="lbl_InfoDeckungskarte" Text="lbl_InfoDeckungskarte" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="1">
                                                                        <asp:DropDownList ID="ddlDataDeckungskarte" AutoPostBack="true" runat="server" Enabled="False"
                                                                            Width="200px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1">
                                                                        <asp:Label ID="lbl_InfoBranding" Text="lbl_InfoBranding" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="1">
                                                                        <asp:DropDownList ID="ddlDataBranding" runat="server" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1">
                                                                        <asp:Label ID="lbl_InfoMindesthaltedauer" Text="lbl_InfoMindesthaltedauer" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="1">
                                                                        <asp:TextBox ID="txtDataMindesthaltedauer" MaxLength="3" Width="30px" runat="server">180</asp:TextBox>
                                                                        <asp:RangeValidator ControlToValidate="txtDataMindesthaltedauer" ID="RVtxtDataMindesthaltedauer"
                                                                            MinimumValue="1" MaximumValue="999" runat="server" Display="None" ErrorMessage="Mindesthaltedauer von 1-999 Tagen"></asp:RangeValidator>
                                                                        <cc1:ValidatorCalloutExtender Enabled="true" ID="ValidatorCalloutExtender1" Width="350px"
                                                                            runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="RVtxtDataMindesthaltedauer">
                                                                        </cc1:ValidatorCalloutExtender>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtDataMindesthaltedauer"
                                                                            FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1">
                                                                        <asp:Label ID="lbl_InfoZulassungsdatum" Text="lbl_InfoZulassungsdatum" runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="1">
                                                                        <asp:TextBox ID="txtDataZulassungsdatum" Width="80px" runat="server"></asp:TextBox>
                                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                                            Animated="true" Enabled="True" TargetControlID="txtDataZulassungsdatum">
                                                                        </cc1:CalendarExtender>
                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDataZulassungsdatum"
                                                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                                        </cc1:MaskedEditExtender>
                                                                        <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtDataZulassungsdatum"
                                                                            ControlExtender="meeZulassungsdatum" Display="none" IsValidEmpty="True" Enabled="true"
                                                                            EmptyValueMessage="Bitte geben Sie ein gültiges Zulassungsdatum ein" InvalidValueMessage="Bitte geben Sie ein gültiges Zulassungsdatum ein">
                                                                                                            
                                                
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                            
                                                
                                                                        </cc1:MaskedEditValidator>
                                                                        <cc1:ValidatorCalloutExtender Enabled="true" ID="ValidatorCalloutExtender2" Width="350px"
                                                                            runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="MaskedEditValidator1">
                                                                        </cc1:ValidatorCalloutExtender>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1">
                                                                        <asp:Label ID="lbl_InfoAbwVersandScheinSchilder" Text="lbl_InfoAbwVersandScheinSchilder"
                                                                            runat="server">
                                                
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="5">
                                                                        <asp:Label ID="lblEqunr" runat="server" Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblAbwScheinSchilderNR" runat="server" Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblDataAbwVersandScheinSchilder" Font-Bold="true" runat="server"></asp:Label>&nbsp;
                                                                        <asp:ImageButton runat="server" ID="imgbSucheAdresse" Height="14" CommandName="bearbeiten"
                                                                            Width="14" ImageUrl="../../../Images/lupe2.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' />
                                                                        <asp:ImageButton Height="14" Width="14" ID="imgbAbwScheinUndSchilderloeschen" ImageUrl="../../../Images/loesch.gif"
                                                                            runat="server"></asp:ImageButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="6">
                                                                        <div align="left">
                                                                            <asp:LinkButton CssClass="standardButton" runat="server" ID="lb_Uebernehmen" Text="übernehmen"></asp:LinkButton>
                                                                            <asp:LinkButton CssClass="standardButton" runat="server" ID="lb_Abbrechen" Text="abbrechen"></asp:LinkButton>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                                <asp:Panel ID="panelSucheAdresse" Visible="false" runat="server" BackColor="Silver"
                                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1" Width="100%">
                                                    <asp:UpdatePanel  ChildrenAsTriggers="true" ID="upSucheAdresse" UpdateMode="Conditional"
                                                        RenderMode="Inline" runat="server">
                                                        <ContentTemplate>
                                                            <table id="Table1" cellspacing="0" cellpadding="1" width="100%" bgcolor="white">
                                                                <tr id="Tr1" runat="server">
                                                                    <td class="TextLarge" width="200" height="32">
                                                                        <asp:Label ID="Label1" runat="server">Selektion&nbsp;zurücksetzen</asp:Label>&nbsp;<asp:ImageButton
                                                                            ID="btnLoesch"  ImageUrl="../../../../Images/loesch.gif" runat="server"></asp:ImageButton>
                                                                    </td>
                                                                    <td class="TextLarge" width="400" height="32">
                                                                    </td>
                                                                    <td class="TextLarge" width="*">
                                                                    </td>
                                                                </tr>
                                                                <tr id="tr_Name1" runat="server">
                                                                    <td class="TextLarge" height="29">
                                                                        <asp:Label runat="server" ID="lbl_Name1">Name:</asp:Label>&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="TextLarge" width="400" height="29">
                                                                        <asp:TextBox ID="txtName" runat="server" MaxLength="35" Width="200px">**</asp:TextBox>&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr id="tr_PLz" runat="server">
                                                                    <td class="TextLarge">
                                                                        PLZ:
                                                                    </td>
                                                                    <td class="TextLarge">
                                                                        <asp:TextBox ID="txtPLZ" runat="server" MaxLength="35" Width="200px">**</asp:TextBox>&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr id="Tr_Ort" runat="server">
                                                                    <td class="StandardTableAlternate">
                                                                        Ort:
                                                                    </td>
                                                                    <td class="StandardTableAlternate">
                                                                        <asp:TextBox ID="txtOrt" runat="server" MaxLength="35" Width="200px">**</asp:TextBox>&nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" width="100%" height="29">
                                                                    </td>
                                                                </tr>
                                                                <tr id="Tr_SelectionButton" runat="server">
                                                                    <td class="TextLarge">
                                                                        Anzahl Treffer:
                                                                        <asp:Label ID="lblAdrErgebnissAnzahl" runat="server" Width="40"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <p>
                                                                            <asp:LinkButton ID="lb_Suche" runat="server" Text="Suche" CssClass="StandardButton"></asp:LinkButton></p>
                                                                    </td>
                                                                </tr>
                                                                <tr id="tr_HaendlerAuswahl" runat="server">
                                                                    <td class="StandardTableAlternate" colspan="2">
                                                                        <asp:ListBox ID="lbHaendler" runat="server" Width="500px" Height="150px" AutoPostBack="True">
                                                                        </asp:ListBox>
                                                                    </td>
                                                                    <td>
                                                                        <p align="left">
                                                                            &nbsp;&nbsp;
                                                                            <asp:Label ID="lblHaendlerDetailsName1" runat="server" Font-Size="12"></asp:Label><br>
                                                                            &nbsp;&nbsp;
                                                                            <asp:Label ID="lblHaendlerDetailsName2" runat="server" Font-Size="12"></asp:Label><br>
                                                                            &nbsp;&nbsp;
                                                                            <asp:Label ID="lblHaendlerDetailsStrasse" runat="server" Font-Size="12"></asp:Label><br>
                                                                            <br>
                                                                            &nbsp;&nbsp;<b>
                                                                                <asp:Label ID="lblHaendlerDetailsPLZ" runat="server" Font-Size="12"></asp:Label>&nbsp;&nbsp;
                                                                                <asp:Label ID="lblHaendlerDetailsOrt" runat="server" Font-Size="12"></asp:Label></b><br>
                                                                        </p>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr id="tr_Message" runat="server">
                                                                    <td class="TextLarge" colspan="3">
                                                                        <p>
                                                                            <asp:Label ID="lbl_Info" runat="server">einfache / mehrfache Platzhaltersuche möglich z.B. 'PLZ= 9*', 'Name1=*Musterma*' </asp:Label><br>
                                                                            <asp:Label ID="lbl_error" runat="server" CssClass="TextError"></asp:Label></p>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="5">
                                                                        <div align="left">
                                                                            <asp:LinkButton CssClass="standardButton" runat="server" ID="lb_AdrUebernehmen" Text="übernehmen"></asp:LinkButton>
                                                                            <asp:LinkButton CssClass="standardButton" runat="server" ID="lb_AdrAbbrechen" Text="abbrechen"></asp:LinkButton>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                                <p align="left">
                                                    <asp:Label ID="lblGvFahrzeugeNoData" Font-Bold="true" runat="server" Visible="false"
                                                        Text="keine Daten vorhanden"></asp:Label>&nbsp;
                                                    <asp:Label ID="lblAnzeigeAnzahlAusgewaehlt" Font-Bold="true" runat="server">Anzahl ausgewählter Fahrzeuge:</asp:Label>
                                                    &nbsp;
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                </p>
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
                                                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="col_Auswahl" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <asp:ImageButton BackColor="White" runat="server" ID="imgbAllauswaehlen" Height="14"
                                                                    CommandName="alleAuwaehlen" ToolTip="alle Fahrzeuge markieren" Width="16" ImageUrl="../../../Images/Confirm_Mini2.gif" />
                                                                <asp:ImageButton BackColor="White" ToolTip="alle Markierungen aufheben" runat="server" ID="imgbAuswahlloeschen" Height="14"
                                                                    CommandName="alleLoeschen" Width="16" ImageUrl="../../../Images/Not_Confirm_Mini2.gif" />&nbsp;
                                                                <asp:LinkButton ID="col_Auswahl" runat="server" CommandName="Sort" CommandArgument="Ausgewaehlt">col_Auswahl</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkAuswahl" ToolTip="Dieses Fahrzeug markieren"  OnCheckedChanged="chk_Auswahl_CheckedChanged" CausesValidation="true"
                                                                    AutoPostBack="true" Checked='<%# DataBinder.Eval(Container, "DataItem.Ausgewaehlt")="X" %>'
                                                                    runat="server" />
                                                                &nbsp;&nbsp;
                                                                <asp:ImageButton runat="server" ToolTip="Dieses Fahrzeug bearbeiten" ID="imgbBearbeiten" Height="14" CommandName="bearbeiten"
                                                                    Width="14" ImageUrl="../../../Images/lupe2.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Lizensnehmer">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Lizensnehmer" runat="server" CommandName="Sort" CommandArgument="LizensnehmerTEXT ">col_Lizensnehmer</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Branding" runat="server" CommandName="Sort" CommandArgument="BrandingTEXT">col_Branding</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Zuldat" runat="server" CommandName="Sort" CommandArgument="Zuldat">col_Zuldat</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label21qa213"  Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LizensnehmerTEXT ") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label21qwqws13e" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BrandingTEXT") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label22paxevwq12a" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zuldat","{0:d}") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Laufzeit">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Laufzeit" runat="server" CommandName="Sort"
                                                                                CommandArgument="Laufzeit">col_Laufzeit</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_EVB" runat="server" CommandName="Sort" CommandArgument="EVBTEXT">col_EVB</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <%--<tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Sicherungsscheinpflichtig" runat="server" CommandName="Sort"
                                                                                CommandArgument="Sicherungsscheinpflichtig">col_Sicherungsscheinpflichtig</asp:LinkButton>
                                                                        </td>
                                                                    </tr>--%>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label2" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Laufzeit") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2q1ss1s21w" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EVBTEXT") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <%--<tr>
                                                                        <td>
                                                                            <asp:CheckBox OnCheckedChanged="chk_Sicherungsscheinpflichtig_CheckedChanged" CausesValidation="true"
                                                                                runat="server" ID="cbx_Sicherungsscheinpflichtig" AutoPostBack="true" Enabled="True"
                                                                                Checked='<%# DataBinder.Eval(Container, "DataItem.Sicherungsscheinpflichtig")="X" %>' />
                                                                        </td>
                                                                    </tr>--%>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Hersteller">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="Hersteller">col_Hersteller</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="Typ">col_Typ</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Farbe" runat="server" CommandName="Sort" CommandArgument="Farbe">col_Farbe</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label21q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Hersteller") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label21qwqwe" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Typ") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label22paxevwq" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Farbe") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                     <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Aufbauart" runat="server" CommandName="Sort" CommandArgument="Aufbauart">col_Aufbauart</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_ZBIINummer" runat="server" CommandName="Sort" CommandArgument="Navi">col_ZBIINummer</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                   
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:HyperLink ID="lnkFahrgestellnummer" Target="_blank" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                                                runat="server"></asp:HyperLink>
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2q1ss1w23" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Aufbauart") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2q1ss1w" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZBIINummer") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Eingangsdatum">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Eingangsdatum" runat="server" CommandName="Sort" CommandArgument="Eingangsdatum">col_Eingangsdatum</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Referenz" runat="server" CommandName="Sort" CommandArgument="Referenz">col_Referenz</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                    </tr>
                                                                    <td>
                                                                        <asp:Label ID="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum","{0:dd.MM.yyyy}") %>'>
                                                                        </asp:Label>
                                                                    </td>
                                                                    <tr>
                                                                    </tr>
                                                                    <td>
                                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz") %>'>
                                                                        </asp:Label>
                                                                    </td>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Getriebe" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Getriebe" runat="server" CommandName="Sort" CommandArgument="Getriebe">col_Getriebe</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Kraftstoffart" runat="server" CommandName="Sort" CommandArgument="Kraftstoffart">col_Kraftstoffart</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                              
                                                                
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aq" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Getriebe") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2qsw" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kraftstoffart") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Bereifung" ItemStyle-HorizontalAlign="left"
                                                            ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Bereifung" runat="server" CommandName="Sort" CommandArgument="Bereifung">col_Bereifung</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Reifengroesse" runat="server" CommandName="Sort" CommandArgument="Reifengroesse">col_Reifengroesse</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bereifung") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Reifengroesse") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
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
                                                        <asp:TemplateField HeaderText="col_CoC" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_CoC" runat="server" CommandName="Sort" CommandArgument="CoC">col_CoC</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="cbx_COC" Enabled="false" Checked='<%# DataBinder.Eval(Container, "DataItem.CoC")="X" %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="lvl2GridStart">
                                                        
                                                                </asp:Literal>
                                                                <table cellpadding="3" cellspacing="0" border="0" width="100%">
                                                                    <tr>
                                                                        <td width="93px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td width="100px">
                                                                            Standort:
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label12aqq2q1sw21a" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Standort") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="93px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td width="100px">
                                                                            Absender:
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Absender") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="93px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td width="100px">
                                                                            abw. Versand Schein/Schilder:
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label4" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AbwScheinSchilderTEXT") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Literal runat="server" ID="lbl3GridEnd" Text="</td></tr>">
                                                                </asp:Literal>
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
                                    <asp:UpdatePanel ID="upWeiter" runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lb_weiter" Text="Weiter" runat="server" CssClass="StandardButton"></asp:LinkButton>
                                    
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
        <tr>
            <td>
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
</body>
</html>
