<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_2.aspx.vb" Inherits="CKG.Components.ComCommon.Change04_2" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:styles id="ucStyles" runat="server">
    </uc1:styles>
    <style type="text/css">
        #Table12 {
            width: 496px;
        }
        .style1
        {
            width: 256px;
        }
        .style2
        {
            width: 256px;
            font-weight: bold;
        }
        .style3
        {
            width: 332px;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">

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

    <input id="xCoordHolder" type="hidden" runat="server" />
    <input id="yCoordHolder" type="hidden" runat="server" />
    <p>
        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
            <tr>
                <td colspan="3">
                    <uc1:header id="ucHeader" runat="server">
                    </uc1:header>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" colspan="3">
                    <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="PageNavigation" colspan="3">
                                <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server">(Erfassung)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TaskTitle" valign="top" colspan="3">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" width="117">
                    <table id="Table12" bordercolor="#ffffff" cellspacing="0" cellpadding="0"
                        border="0">
                        <tr>
                            <td width="157">
                                <p>
                                    <asp:LinkButton ID="lb_Speichern" runat="server" CssClass="StandardButton"> &#149;&nbsp;Speichern</asp:LinkButton><br />
                                    <asp:LinkButton ID="lb_Verwerfen" runat="server" CssClass="StandardButton"> &#149;&nbsp;Verwerfen</asp:LinkButton></p>
                            </td>
                        </tr>
                        <tr>
                            <td width="157">
                                * = Pflichtfelder</td>
                        </tr>
                    </table>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False">
                    </asp:Label>
                 </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="0" valign="top">
                    <table id="blubb" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                        <tr>
                            <td>
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" BorderWidth="1" BorderStyle="Solid" BorderColor="#8399b1"
                                    Width="1000px" Height="1253px">
                                    <table id="tbl_EingabeMaske" height="100%" cellspacing="0" cellpadding="0" width="100%"
                                        border="0" runat="server">
                                <tr>
                                            <td class="style1">
                                                <b>Team</b>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_Team" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                
                                            </td>
                                            <td>
                                                
                                            </td>
                                        </tr>                                        
                                        <tr >
                                            <td colspan="5" class="sektion">
                                               1) Leasingnehmer
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Kundennummer*</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LN_KUNNR"  CssClass="TextboxNormal" runat="server" 
                                                    MaxLength="12"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>Name*</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LN_NAME1"  CssClass="TextboxNormal" runat="server"
                                                    ToolTip="Vorname" MaxLength="40"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Strasse und Hausnummer*</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LN_STRAS" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="60"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>PLZ*</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LN_PSTLZ" runat="server" CssClass="TextboxShort" 
                                                    MaxLength="10"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Ort*</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LN_ORT01" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="40"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr >
                                            <td colspan="5" class="sektion">
                                                2) Vorgaben Zulassung
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Zulassung auf</b></td>
                                            <td>
                                                <asp:DropDownList ID="ddl_SH_KUNDART" runat="server">
                                                    <asp:ListItem Value="Auswahl">-Auswahl-</asp:ListItem>
                                                    <asp:ListItem Value="LN">Leasingnehmer</asp:ListItem>
                                                    <asp:ListItem Value="LG">Leasinggeber</asp:ListItem>
                                                    <asp:ListItem Value="SH">spezieller Halter</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                <b>Zulassungsort</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_SH_ZKFZKZ" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="3"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Firma</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_SH_Firma"  CssClass="TextboxNormal" runat="server" 
                                                    MaxLength="40"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>Strasse und Hausnummer</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SH_Strasse"  CssClass="TextboxNormal" runat="server" 
                                                    MaxLength="60"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>PLZ</b> </td>
                                            <td>
                                                <asp:TextBox ID="txt_SH_PLZ" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="10"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                                </td>
                                            <td class="style3">
                                                <b>Ort</b> </td>
                                            <td>
                                                <asp:TextBox ID="txt_SH_ORT" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="40"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Dauerwunschkennzeichen</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_SH_DWK" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="11"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                <b>Zulassung durch</b></td>
                                            <td>
                                                <asp:DropDownList ID="ddl_ZUL_DURCH" runat="server">
                                                    <asp:ListItem Value="Auswahl">-Auswahl-</asp:ListItem>
                                                    <asp:ListItem Value="H">Händler</asp:ListItem>
                                                    <asp:ListItem>DAD</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td class="style1">
                                                <b>KFZ-Steuerzahler</b></td>
                                            <td>
                                                <asp:DropDownList ID="ddl_KFZ_Steuer" runat="server">
                                                    <asp:ListItem Value="Auswahl">-Auswahl-</asp:ListItem>
                                                    <asp:ListItem Value="L">Leasing</asp:ListItem>
                                                    <asp:ListItem Value="H">Halter</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="sektion" colspan="5">
                                                3) Versicherungsdaten</td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Versichert durch</b>
                                            </td>
                                            <td>
                                                    <asp:DropDownList ID="ddl_Versichert" runat="server">
                                                    <asp:ListItem Value="Auswahl">-Auswahl-</asp:ListItem>
                                                    <asp:ListItem Value="1">LHS</asp:ListItem>
                                                    <asp:ListItem Value="2">Eigenversichert</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                <b>Versicherungsanstalt</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_Versicherung" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="40"></asp:TextBox>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="style1">
                                                <b>Deckungskarte oder eVB</b></td>
                                            <td>
                                                <asp:DropDownList ID="ddl_VersDeckArt" runat="server" AutoPostBack="True">
                                                    <asp:ListItem Value="Auswahl">-Auswahl-</asp:ListItem>
                                                    <asp:ListItem Value="1">Deckungskarte</asp:ListItem>
                                                    <asp:ListItem Value="2">Dauer-EVB</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3" visible="false" id="td_dauerEVB1">
                                                <b>Dauer-EVB</b></td>
                                            <td visible="false"  id="td_dauerEVB2">
                                                <asp:TextBox ID="txt_DauerEVB" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="7"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="sektion" colspan="5">
                                                4) Auslieferung
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Firma</b>&nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="txt_AL_Name1" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="40"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>Ansprechpartner</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_AL_Ansprechpartner" runat="server" 
                                                    CssClass="TextboxNormal" MaxLength="40"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td class="style1">
                                                <b>Strasse und Hausnummer</b>&nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="txt_AL_Strasse" runat="server" CssClass="TextboxNormal"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>PLZ</b>&nbsp;
                                                
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_AL_PLZ" runat="server" CssClass="TextboxShort" 
                                                    MaxLength="10"></asp:TextBox>
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Ort</b>&nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="txt_AL_Ort" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="40"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>Telefonnummer</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_AL_Telefon" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Fax</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_AL_Fax" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="20"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>E-Mail</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_AL_EMAIL" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Auslieferungsinformationen</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_AL_AUSLINF" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="60"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="sektion" colspan="5">
                                                5) Fahrzeugrücknahme
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Firma</b>&nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="txt_RL_NAME1" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="40"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>Ansprechpartner</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_RL_ANSPRECHP" runat="server" 
                                                    CssClass="TextboxNormal" MaxLength="40"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td class="style1">
                                                <b>Strasse und Hausnummer</b>&nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="txt_RL_Strasse" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="60"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>PLZ</b>&nbsp;
                                                
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_RL_PLZ" runat="server" CssClass="TextboxShort" 
                                                    MaxLength="10"></asp:TextBox>
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Ort</b>&nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="txt_RL_Ort" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="40"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>Telefonnummer</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_RL_Telefon" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Fax</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_RL_Fax" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="20"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>E-Mail</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_RL_EMAIL" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="sektion" colspan="5">
                                                6) Sonstiges
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Sonderaufgaben</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_SONDAG" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="60"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td class="style3">
                                                <b>Servicekarte</b>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rbl_SVCKART" runat="server" 
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="X">Ja</asp:ListItem>
                                                    <asp:ListItem Selected="True">Nein</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <b>Tankkarten</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_Tankkarte" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                <b>Tankkartenbesonderheiten</b></td>
                                            <td>
                                                <asp:TextBox ID="txt_Tankkarte_BSH" runat="server" CssClass="TextboxNormal" 
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="style1">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                Service Voreinstellungen</td>
                                            <td>
                                                &nbsp;</td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                Navi-CD/DVD</td>
                                            <td>
                                                <asp:RadioButtonList ID="rbl_SV_NAVICD" runat="server" 
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="X">Ja</asp:ListItem>
                                                    <asp:ListItem Selected="True">Nein</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                Handy-Adapter</td>
                                            <td>
                                                <asp:RadioButtonList ID="rbl_SV_HANDY_ADAPTER" runat="server" 
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="X">Ja</asp:ListItem>
                                                    <asp:ListItem Selected="True">Nein</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                Verbandskasten</td>
                                            <td>
                                                <asp:RadioButtonList ID="rbl_SV_Verbandskasten" runat="server" 
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="X">Ja</asp:ListItem>
                                                    <asp:ListItem Selected="True">Nein</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                Warndreieck</td>
                                            <td>
                                                <asp:RadioButtonList ID="rbl_SV_Warndreieck" runat="server" 
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="X">Ja</asp:ListItem>
                                                    <asp:ListItem Selected="True">Nein</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                Warnweste</td>
                                            <td>
                                                <asp:RadioButtonList ID="rbl_SV_Warnweste" runat="server" 
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="X">Ja</asp:ListItem>
                                                    <asp:ListItem Selected="True">Nein</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                Fußmatten</td>
                                            <td>
                                                <asp:RadioButtonList ID="rbl_SV_Fussmatten" runat="server" 
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="X">Ja</asp:ListItem>
                                                    <asp:ListItem Selected="True">Nein</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                Führerscheinkontrolle</td>
                                            <td>
                                                <asp:RadioButtonList ID="rbl_SV_FScheinkontrolle" runat="server" 
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="X">Ja</asp:ListItem>
                                                    <asp:ListItem Selected="True">Nein</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td width="20">
                                                &nbsp;</td>
                                            <td class="style3">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="sektion" colspan="5">
                                                <span lang="de">7</span>) Kundenansprechpartner
                                            </td>
                                        </tr>
                                       <tr>
                                            <td class="sektionGrid" colspan="5"> 
                                                <asp:GridView ID="grdAnsprechpartner" runat="server" 
                                                    AutoGenerateColumns="False">
                                                    <PagerSettings Visible="False" />
                                                    <Columns>
                                                        <asp:BoundField DataField="AG" Visible="False" />
                                                        <asp:BoundField DataField="MANDT" Visible="False" />
                                                        <asp:BoundField DataField="EXKUNNR_ZL" HeaderText="EXKUNNR_ZL" 
                                                            Visible="False" />
                                                        <asp:TemplateField HeaderText="Name*">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtName" runat="server" Height="22px" MaxLength="40" 
                                                                    Text='<%# Eval("NAME1") %>' Width="212px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Telefon">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtTelefon" runat="server" Height="22px" MaxLength="20" 
                                                                    Text='<%# Eval("TELF1") %>' Width="150px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="E-Mail">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEMAIL" runat="server" Height="22px" MaxLength="50" 
                                                                    Text='<%# Eval("EMAIL") %>' Width="219px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Löschen">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                     <tr>
                                            <td class="sektion" colspan="5">
                                                <span lang="de">8</span>) Versandbedingungen
                                            </td>
                                        </tr>
                                       <tr>
                                            <td class="sektionGrid" colspan="5"> 
                                                <asp:GridView ID="grdVersandbedingungen" runat="server" 
                                                    AutoGenerateColumns="False">
                                                    <PagerSettings Visible="False" />
                                                    <Columns>
                                                        <asp:BoundField DataField="AG" Visible="False" />
                                                        <asp:BoundField DataField="MANDT" Visible="False" />
                                                        <asp:BoundField DataField="EXKUNNR_ZL" HeaderText="EXKUNNR_ZL" 
                                                            Visible="False" />
                                                       <asp:TemplateField HeaderText="E-Mail">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEMAIL" runat="server" Height="22px" MaxLength="50" 
                                                                    Text='<%# Eval("EMAIL") %>' Width="219px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                            
                                                        <asp:TemplateField HeaderText="Funktion des Adressinhabers">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtFAdressinhaber" runat="server" Height="22px" MaxLength="40" 
                                                                    Text='<%# Eval("ZPOSITION") %>' Width="212px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Übernahmebestätigung">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkUebernahmebest" runat="server" 
                                                                    Checked='<%# IIf(IsDBNull(Eval("UEBNAHMBEST")), False, Eval("UEBNAHMBEST")) %>'/>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ZB2 aktiv">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkZB2" runat="server" 
                                                                    Checked='<%# IIf(IsDBNull(Eval("ZB2_VERSENDEN")), False, Eval("ZB2_VERSENDEN")) %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rücknahmeprotokoll">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkRuecknahmeprot" runat="server" 
                                                                    Checked='<%# IIf(IsDBNull(Eval("RUECKNAHMEPROTK")), False, Eval("RUECKNAHMEPROTK")) %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ZB1 nach Abmeldung">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkZB1" runat="server" 
                                                                    Checked='<%# IIf(IsDBNull(Eval("ZB1_VERSENDEN")), False, Eval("ZB1_VERSENDEN")) %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Löschen">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>                                        
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                   
                </td>
                <td>
                </td>
            </tr>
        </table>
    </p>
    </form>
</body>
</html>