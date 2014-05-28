<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_2.aspx.vb" Inherits="AppKruell.Change01_2" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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

    <input id="xCoordHolder" type="hidden" runat="server">
    <input id="yCoordHolder" type="hidden" runat="server">
    <p>
        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
            <tr>
                <td colspan="3">
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
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
                                    <asp:LinkButton ID="lb_Aktuallisieren" runat="server" CssClass="StandardButton"> &#149;&nbsp;Auftrag aktuallisieren</asp:LinkButton><br>
                                    <asp:LinkButton ID="lb_Anzeige" runat="server" CssClass="StandardButton"> &#149;&nbsp;zur Anzeige</asp:LinkButton></p>
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
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
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
                                            <b>Kundennummer</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_KUNNR_LN" TabIndex="1" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#0066cc">
                                        <td colspan="5">
                                            <p align="center">
                                                <b><font color="white" size="5">1) Kunde (Halter)</font></b></p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Kundenname</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_NAME_LN" TabIndex="2" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <b>Nutzer/Fahrer</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_AP_NAME1_LN" TabIndex="10" CssClass="TextboxNormal" runat="server"
                                                ToolTip="Vorname">&#160;</asp:TextBox>
                                            <br>
                                            <asp:TextBox ID="txt_D_AP_NAME2_LN" runat="server" CssClass="TextboxNormal" TabIndex="11"
                                                ToolTip="Nachname">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Straße/Nr.</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_STR_HNR_LN" TabIndex="3" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <b>Mobilfunk</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_HANDY_LN" TabIndex="12" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>PLZ/Ort</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_PLZ_LN" TabIndex="4" CssClass="TextboxShort" Width="100px"
                                                runat="server">
                                            </asp:TextBox>
                                            <asp:TextBox ID="txt_D_STADT_LN" TabIndex="5" CssClass="TextboxNormal" Width="155px"
                                                runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <b>sonstiges</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_SONST_LN" TabIndex="13" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Telefon</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_TEL_LN" TabIndex="6" CssClass="TextboxShort" Width="100px"
                                                runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <b>Versicherungsges.</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_VERS" TabIndex="14" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Fax</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_FAX_LN" TabIndex="7" CssClass="TextboxShort" Width="100px"
                                                runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td rowspan="3">
                                            <b>Fahrzeug/typ</b>
                                        </td>
                                        <td rowspan="3">
                                            <asp:TextBox ID="txt_D_CAR_TYPE" TabIndex="8" CssClass="TextboxMultiline" runat="server"
                                                TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <b>Order-Nr.</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_ORDER_NR" TabIndex="15" CssClass="TextboxShort" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <b>Nr.-Leasinggesl.</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_KUNNR_LG" TabIndex="16" CssClass="TextboxShort" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <b>Leasinggeber</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_NAME_LG" TabIndex="17" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Fahrgestellnummer</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_CHASSIS_NUM" TabIndex="9" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <b>Leasing-Nr</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_CAR_BESTNR" TabIndex="18" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#0066cc">
                                        <td colspan="5">
                                            <p align="center">
                                                <b><font color="white" size="5">2) Fahrzeugaufbereitung</font></b></p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label27" CssClass="bezeichnungsLabel" Width="100%" runat="server"
                                                BackColor="#c1ccd9">durch Firma</asp:Label>
                                        </td>
                                        <td>
                                            <table id="tableblabla" bgcolor="#c1ccd9" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="label32X" CssClass="bezeichnungsLabel" runat="server">Adressauswahl:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDL_S_STANDORT" TabIndex="19" runat="server" AutoPostBack="True"
                                                            CssClass="dropDownListeNormal">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="label1" CssClass="bezeichnungsLabel" runat="server">Name1</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_AUFBERß1" TabIndex="19" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label2" CssClass="bezeichnungsLabel" runat="server">Name2</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_AUFBERß2" TabIndex="20" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label3" CssClass="bezeichnungsLabel" runat="server">Straße/Hausnummer</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_AUFBERß3" TabIndex="21" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" CssClass="bezeichnungsLabel" runat="server">Postleitzahl</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_AUFBERß4" TabIndex="22" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label05" CssClass="bezeichnungsLabel" runat="server">Ort</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_AUFBERß5" TabIndex="23" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label28" CssClass="bezeichnungsLabel" Width="100%" runat="server"
                                                BackColor="#c1ccd9">Aufbereitungsart</asp:Label>
                                        </td>
                                        <td>
                                            <table id="Table1C" bgcolor="#c1ccd9" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="TXT_S_AUFBER_ART" TabIndex="24" CssClass="TextboxNormal" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="IBTN_S_AUFBER_ART1" runat="server" CssClass="ImageButtonNormal"
                                                            ImageUrl="../../../Images/arrowUeberfRight.gif"></asp:ImageButton>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="LBX_S_AUFBEREITUNGSART" runat="server" CssClass="listboxNormal"
                                                            SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="IBTN_S_AUFBER_ART2" runat="server" CssClass="ImageButtonNormal"
                                                            ImageUrl="../../../Images/arrowUeberfRight.gif"></asp:ImageButton>
                                                    </td>
                                                    <td>
                                                        <asp:ListBox ID="LBX_B_AUFBER_ART" runat="server" CssClass="listboxNormal"></asp:ListBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="Imagebutton5" runat="server" CssClass="ImageButtonNormal" ImageUrl="../../../Images/loesch.gif">
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <asp:Label ID="Label29" CssClass="bezeichnungsLabel" Width="100%" runat="server"
                                                BackColor="#C1CCD9" Height="19px">Ausstattung</asp:Label>
                                        </td>
                                        <td>
                                            <table bgcolor="#c1ccd9" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="TXT_S_AUSSTATT" TabIndex="25" CssClass="TextboxNormal" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="IBTN_S_AUSSTATT1" runat="server" CssClass="ImageButtonNormal"
                                                            ImageUrl="../../../Images/arrowUeberfRight.gif"></asp:ImageButton>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="LBX_S_AUSSTATTUNG" runat="server" CssClass="listboxNormal" SelectionMode="Multiple">
                                                        </asp:ListBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="IBTN_S_AUSSTATT2" runat="server" CssClass="ImageButtonNormal"
                                                            ImageUrl="../../../Images/arrowUeberfRight.gif"></asp:ImageButton>
                                                    </td>
                                                    <td>
                                                        <asp:ListBox ID="LBX_B_AUSSTATT" runat="server" CssClass="listboxNormal"></asp:ListBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="IBTN_S_AUSSTATT3" runat="server" CssClass="ImageButtonNormal"
                                                            ImageUrl="../../../Images/loesch.gif"></asp:ImageButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Betankungsorder</b>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_D_CAR_TANK" runat="server"></asp:CheckBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <b>sonstige Optionen</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_SONST_OPT" CssClass="TextboxMultiline" runat="server" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Transportauftrag erforderlich</b>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_S_VORHOL" runat="server"></asp:CheckBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#0066cc">
                                        <td colspan="5">
                                            <p align="center">
                                                <b><font color="white" size="5">3) <i>zusätzliche Einbauten</i></font></b></p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label30" CssClass="bezeichnungsLabel" Width="100%" runat="server"
                                                BackColor="#c1ccd9">durch Firma:(z.B. AES/Wollnik.)</asp:Label>
                                        </td>
                                        <td>
                                            <table id="Table3" bgcolor="#c1ccd9" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="label132X" CssClass="bezeichnungsLabel" runat="server">Adressauswahl:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDL_S_EINBAUFIRMA" TabIndex="19" runat="server" AutoPostBack="True"
                                                            CssClass="dropDownListeNormal">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label6" CssClass="bezeichnungsLabel" runat="server">Name1</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_ZUSEINBAUTß1" TabIndex="26" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label7" CssClass="bezeichnungsLabel" runat="server">Name2</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_ZUSEINBAUTß2" TabIndex="27" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label8" CssClass="bezeichnungsLabel" runat="server">Straße/Hausnummer</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_ZUSEINBAUTß3" TabIndex="28" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label9" CssClass="bezeichnungsLabel" runat="server">Postleitzahl</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_ZUSEINBAUTß4" TabIndex="29" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label10" CssClass="bezeichnungsLabel" runat="server">Ort</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_ZUSEINBAUTß5" TabIndex="30" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3X1" CssClass="bezeichnungsLabel" Width="100%" runat="server"
                                                BackColor="#c1ccd9">Einbau</asp:Label>
                                        </td>
                                        <td>
                                            <table id="Table2" bgcolor="#c1ccd9" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="TXT_S_EINBAU" TabIndex="31" CssClass="TextboxNormal" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="IBTN_S_EINBAU1" runat="server" CssClass="ImageButtonNormal"
                                                            ImageUrl="../../../Images/arrowUeberfRight.gif"></asp:ImageButton>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="LBX_S_EINBAU" runat="server" CssClass="listboxNormal"></asp:ListBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="IBTN_S_EINBAU2" runat="server" CssClass="ImageButtonNormal"
                                                            ImageUrl="../../../Images/arrowUeberfRight.gif"></asp:ImageButton>
                                                    </td>
                                                    <td>
                                                        <asp:DataGrid ID="DTG_S_SONDEINB_POS" runat="server" Width="200px" AutoGenerateColumns="False">
                                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Height="10px" Width="10px" VerticalAlign="Middle">
                                                            </ItemStyle>
                                                            <Columns>
                                                                <asp:BoundColumn DataField="Item" HeaderText="Item"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Beschreibung" HeaderText="Beschreibung"></asp:BoundColumn>
                                                                <asp:ButtonColumn Text="&lt;img src=../../../Customize/Kruell/loesch.gif border=0&gt;"
                                                                    HeaderText="L&#246;schen" CommandName="Delete"></asp:ButtonColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Einbaubeschreibung:</b>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="TXT_S_AUFBER_POS" TabIndex="32" CssClass="TextboxNormal" runat="server">
                                                        </asp:TextBox>
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
                                        <td colspan="5">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label30x" CssClass="bezeichnungsLabel" Width="100%" runat="server"
                                                BackColor="#c1ccd9">Firma Winterräder</asp:Label>
                                        </td>
                                        <td>
                                            <table id="Table3X" bgcolor="#c1ccd9" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="label132" CssClass="bezeichnungsLabel" runat="server">Adressauswahl:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDL_S_FIRMA_WINTER" TabIndex="19" runat="server" AutoPostBack="True"
                                                            CssClass="dropDownListeNormal">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label61" CssClass="bezeichnungsLabel" runat="server">Name1</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_WINTERRADß1" TabIndex="26" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label7a" CssClass="bezeichnungsLabel" runat="server">Name2</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_WINTERRADß2" TabIndex="27" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label8a" CssClass="bezeichnungsLabel" runat="server">Straße/Hausnummer</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_WINTERRADß3" TabIndex="28" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label9a" CssClass="bezeichnungsLabel" runat="server">Postleitzahl</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_WINTERRADß4" TabIndex="29" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label10a" CssClass="bezeichnungsLabel" runat="server">Ort</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_FA_WINTERRADß5" TabIndex="30" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <asp:Label ID="Label31" CssClass="bezeichnungsLabel" Width="100%" runat="server"
                                                BackColor="#C1CCD9">Winterrädertyp</asp:Label>
                                        </td>
                                        <td>
                                            <table id="Table1" bgcolor="#c1ccd9" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="TXT_S_WINTERRAD_POS" TabIndex="24" CssClass="TextboxNormal" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="IBTN_S_WINTERRAD_POS" runat="server" CssClass="ImageButtonNormal"
                                                            ImageUrl="../../../Images/arrowUeberfRight.gif"></asp:ImageButton>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="LBX_S_WINTERRAD_POS" runat="server" CssClass="listboxNormal" SelectionMode="Multiple">
                                                        </asp:ListBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="IBTN_S_WINTERRAD_POS2" runat="server" CssClass="ImageButtonNormal"
                                                            ImageUrl="../../../Images/arrowUeberfRight.gif"></asp:ImageButton>
                                                    </td>
                                                    <td>
                                                        <asp:ListBox ID="LBX_B_WINTERRAD_POS" runat="server" CssClass="listboxNormal"></asp:ListBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="IBTN_S_WINTERRAD_POS_Loesch" runat="server" CssClass="ImageButtonNormal"
                                                            ImageUrl="../../../Images/loesch.gif"></asp:ImageButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#0066cc">
                                        <td colspan="5">
                                            <p align="center">
                                                <b><font color="white" size="5">4) Zulassung und Art</font></b></p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Zulassung und Art</b>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txt_D_ZUL_ART" TabIndex="33" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Zulassungsdienst Name</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDL_S_ZULASSUNGSDIENST" CssClass="dropDownListeNormal" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label26" CssClass="bezeichnungsLabel" Width="100%" runat="server"
                                                BackColor="#c1ccd9">abw. Halter/Zulassung</asp:Label>
                                        </td>
                                        <td>
                                            <table id="Table6" bgcolor="#c1ccd9" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label21" CssClass="bezeichnungsLabel" runat="server">Name1</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_ABW_HALTERß1" TabIndex="34" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label22" CssClass="bezeichnungsLabel" runat="server">Name2</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_ABW_HALTERß2" TabIndex="35" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label23" CssClass="bezeichnungsLabel" runat="server">Straße/Hausnummer</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_ABW_HALTERß3" TabIndex="36" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label24" CssClass="bezeichnungsLabel" runat="server">Postleitzahl</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_ABW_HALTERß4" TabIndex="37" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label25" CssClass="bezeichnungsLabel" runat="server">Ort</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_ABW_HALTERß5" TabIndex="38" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Wunschkennzeichen</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_W_KENNZ" TabIndex="40" CssClass="TextboxShort" runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <b>WKZ reser.:</b>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_D_W_KENNZ_RES" runat="server"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#0066cc">
                                        <td colspan="5">
                                            <p align="center">
                                                <b><font color="white" size="5">5) Auslieferung/Übergabe</font></b></p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Verbringunsdatum</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_GEW_VERBR_DAT" TabIndex="41" CssClass="TextboxShort" runat="server"
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b><b>Lieferung durch</b></b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDL_S_LIEFERUNGDURCH" CssClass="dropDownListeNormal" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label111" CssClass="bezeichnungsLabel" Width="100%" runat="server"
                                                BackColor="#c1ccd9">abw. Lieferanschrift</asp:Label>
                                        </td>
                                        <td>
                                            <table id="Table4" bgcolor="#c1ccd9" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label11" CssClass="bezeichnungsLabel" runat="server">Name1</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_ABW_LIEF_ADRß1" TabIndex="42" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label12" CssClass="bezeichnungsLabel" runat="server">Name2</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_ABW_LIEF_ADRß2" TabIndex="43" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label13" CssClass="bezeichnungsLabel" runat="server">Straße/Hausnummer</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_ABW_LIEF_ADRß3" TabIndex="44" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label14" CssClass="bezeichnungsLabel" runat="server">Postleitzahl</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_ABW_LIEF_ADRß4" TabIndex="45" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label15" CssClass="bezeichnungsLabel" runat="server">Ort</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_ABW_LIEF_ADRß5" TabIndex="46" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <b></b>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Bemerkung zur Verbringung</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_CAR_BEM" TabIndex="47" CssClass="TextboxMultiline" runat="server"
                                                TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr bgcolor="#0066cc">
                                        <td colspan="5">
                                            <p align="center">
                                                <b><font color="white" size="5">6) Rückführung Fahrzeug</font></b></p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Fahrzeug ist Rückfahrzeug?</b>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_D_CAR_RETURN" runat="server"></asp:CheckBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td rowspan="3">
                                            <asp:Label ID="label119" CssClass="bezeichnungsLabel" Width="100%" runat="server"
                                                BackColor="#c1ccd9">Ablieferanschrift Rückl.</asp:Label>
                                        </td>
                                        <td rowspan="3">
                                            <table id="Table5" bgcolor="#c1ccd9" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label16" CssClass="bezeichnungsLabel" runat="server">Name1</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_CAR_RET_ADRß1" TabIndex="51" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label17" CssClass="bezeichnungsLabel" runat="server">Name2</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_CAR_RET_ADRß2" TabIndex="52" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label18" CssClass="bezeichnungsLabel" runat="server">Straße/Hausnummer</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_CAR_RET_ADRß3" TabIndex="53" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label19" CssClass="bezeichnungsLabel" runat="server">Postleitzahl</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_CAR_RET_ADRß4" TabIndex="54" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label20" CssClass="bezeichnungsLabel" runat="server">Ort</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXT_B_CAR_RET_ADRß5" TabIndex="55" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Fahrzeug</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_CAR_RET_BESCH" TabIndex="48" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>amtl. KZ</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_LIC_NUM_RETURN" TabIndex="49" CssClass="TextboxShort" runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Leasinggeber Rückfahrzeug</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_ANGABEN_LG" TabIndex="50" CssClass="TextboxNormal" runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td width="20">
                                        </td>
                                        <td>
                                            <strong>Bemerkung Rückfahrzeug</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_D_BEM_RUECKFZG" TabIndex="60" CssClass="TextboxMultiline" runat="server"
                                                TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#0066cc">
                                        <td colspan="5">
                                            <p align="center">
                                                <b><font color="white" size="5">7) Bemerkungen</font></b></p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Ider ZUL-Unterlagen</b>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txt_S_BEM_SUZ" TabIndex="61" CssClass="TextboxMultilineLarge" runat="server"
                                                TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False">
                </asp:Label>
            </td>
            <td>
            </td>
            </tr>
        </table>
    </p>
    </form>
</body>
</html>
