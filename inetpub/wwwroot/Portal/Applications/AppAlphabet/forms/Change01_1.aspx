<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_1.aspx.vb" Inherits="AppAlphabet.Change01_1" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
        .style1 {
            font-weight: bold;
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
                    <table id="Table12" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
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
                            </td>
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
                    <br />
                    <span lang="de"><b>&nbsp;&nbsp; * = Pflichtfelder</b></span></td>
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
                                            <td>
                                                <span><b>Vertragsnummer</b></span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LIZNR"  CssClass="TextboxShort" 
                                                    runat="server"></asp:TextBox>
                                            </td>
                                            <td width="20"></td>
                                                <td>
                                                    <b>Vertragsstatus</b>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_D_STATUS" runat="server">
                                                        <asp:ListItem Text="Anlauf" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Aktiv" Value="2">
                                                    </asp:ListItem>
                                                     <asp:ListItem Text="Auslauf" Value="3">
                                                    </asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                        </tr>
                                        <tr >
                                            <td colspan="5" class="sektion">
                                               1) Leasingnehmer
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Kundennummer</b>
                                            <span lang="de">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LN_KUNNR"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                                <span lang="de">&nbsp;</span><asp:ImageButton ID="ibtRefresh" runat="server" 
                                                    Height="16px" ImageUrl="/Portal/images/refresh.gif" ToolTip="Aktualisieren" 
                                                    Width="16px" />
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Name1</b>
                                            <span lang="de">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LN_NAME1"  CssClass="TextboxNormal" runat="server"
                                                    ToolTip="Vorname">&#160;</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Name2</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LN_NAME2"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Strasse und Hausnummer</b>
                                            <span lang="de">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LN_STRAS"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>PLZ</b>
                                            <span lang="de">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LN_PSTLZ"  CssClass="TextboxShort"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Ort</b>
                                            <span lang="de">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LN_ORT01"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Land</b>
                                            <span lang="de">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddl_D_LN_LAND1" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td colspan="5" class="sektion">
                                               2) Nutzer
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Vorname</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_FR_VNAME" CssClass="TextboxNormal"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Nachname</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_FR_NNAME"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>E-mail</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_FR_EMAIL"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Telefonnummer</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_FR_TELEF"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td colspan="5" class="sektion">
                                               3) Händler
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Händlernummer</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_HD_KUNNR"  CssClass="TextboxShort" runat="server">
                                                </asp:TextBox>
                                                <span lang="de">&nbsp;</span><asp:ImageButton ID="ibtSearchLN" runat="server" 
                                                    ImageUrl="/Portal/images/lupe2.gif" ToolTip="Adresssuche" />
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Name1</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_HD_NAME1"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Name2</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_HD_NAME2"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Strasse und Hausnummer</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_HD_STRAS"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>PLZ</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_HD_PSTLZ"  CssClass="TextboxShort" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Ort</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_HD_ORT01"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Land</b>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_D_HD_LAND1" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Telefonnummer</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_HD_TELEF" CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td colspan="5" class="sektion">
                                               4) Fahrzeug
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Hersteller</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_HERST"  CssClass="TextboxNormal" 
                                                    runat="server"></asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Modell</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_MODELL"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Farbe</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_FARBE"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Polster</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_POLSTER"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Hubraum</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_HUBRAUM"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Leistung</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LEISTUNG"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Aufbauart</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_AUFBAUART"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Anzahl Türen</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_ANZ_TUER"  CssClass="TextboxShort" 
                                                    runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Fahrgestellnummer</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_CHASSIS_NUM"  MaxLength="17" CssClass="TextboxNormal"
                                                    runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Kennzeichen</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_LICENSE_NUM"  CssClass="TextboxShort" 
                                                    runat="server" MaxLength="8"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" class="sektion">
                                                
                                                    5) Geschäftsstelle
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="style1" lang="de">Auswahl Geschäftsstelle</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_S_Geschaeftsstelle" runat="server" 
                                                    CssClass="ddlnormal" DataTextField="NAME" DataValueField="TEAM">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                &nbsp;<asp:TextBox ID="txt_D_GST_KURZ" runat="server" CssClass="TextboxShort" 
                                                    Visible="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_GST_NAME"  CssClass="TextboxNormal" runat="server" 
                                                    Visible="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td colspan="5" class="sektion">
                                               
                                                   6) Kundenbetreuer
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Kürzel/ interne Nummer</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_KZ_INTERN"  CssClass="TextboxShort" 
                                                    runat="server"></asp:TextBox>
                                                <span lang="de">&nbsp;</span><asp:ImageButton ID="ibtSearchKB" runat="server" 
                                                    ImageUrl="/Portal/images/lupe2.gif" ToolTip="Adresssuche" />
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Vorname</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_KB_VNAME"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Nachname</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_KB_NNAME"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Ort</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_KB_ORT01"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td>
                                                <b>Telefonnummer</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_KB_TELEFON"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>E-Mail</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_D_KB_EMAIL"  CssClass="TextboxNormal" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" class="sektion">
                                              7) Steuerzahlung durch Leasinggeber
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Steuerzahlung</b>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_D_DL_KEZI" runat="server"></asp:CheckBox>
                                            </td>
                                            <td width="20">
                                            </td>
                                            <td colspan="2">
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
