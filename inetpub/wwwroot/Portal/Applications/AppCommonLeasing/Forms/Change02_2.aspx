<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_2.aspx.vb" Inherits="AppCommonLeasing.Change02_2" %>

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
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120px">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" colspan="2">
                                        &nbsp;<asp:LinkButton ID="lb_zurueck" Visible="True" runat="server">lb_zurueck</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="lb_weiter" CssClass="StandardButton" runat="server">Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="100%">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;</td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td>
                                                    <p>
                                                        <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp;<asp:Label
                                                            ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;</p>
                                                    <p>
                                                        <asp:Label ID="lblInfo" Font-Bold="true" ForeColor="#DD0000" runat="server"></asp:Label></p>
                                                </td>
                                                <td align="right">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="120px">
                        </td>
                        <td valign="top">
                            <table id="Tablex1" runat="server" cellspacing="0" cellpadding="5" width="100%" bgcolor="white"
                                border="0">
                                <tr id="tr_Partneradresse" class="TextLarge">
                                    <td align="left" nowrap="nowrap">
                                        <asp:RadioButton ID="rb_Partneradresse" runat="server" AutoPostBack="true"
                                            GroupName="AdressArt" />
                                    </td>
                                    <td align="left" width="100%">
                                        <asp:DropDownList ID="ddlPartneradressen" runat="server" 
                                            CssClass="DropDownXLarge" visible="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                               
                                <tr id="tr_Geschaeftsstelle" class="StandardTableAlternate">
                                    <td align="left" nowrap="nowrap">
                                        <asp:RadioButton ID="rb_Geschaeftsstelle" runat="server" AutoPostBack="true" 
                                            GroupName="AdressArt" />
                                    </td>
                                    <td align="left" width="100%">
                                       <asp:DropDownList ID="ddlGeschaeftsstelle" runat="server" 
                                            CssClass="DropDownXLarge" visible="false">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblSuche" runat="server" CssClass="TextLarge" 
                                            Visible="False" ></asp:Label>
                                        <asp:LinkButton ID="lb_NeuSuche" CssClass="StandardButton" runat="server" 
                                            Visible="False">Neu Suche</asp:LinkButton>
                                        <table id="tblSuche" runat="server" visible="False" cellspacing="0" cellpadding="5" width="100%"
                                            bgcolor="white" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_NameSuche" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="10%">
                                                    <asp:TextBox ID="txtNameSuche" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                                <td width="100%">
                                                    <asp:Label ID="lbl_Info0"  CssClass="TextLarge" 
                                                        Text="Alle Eingaben mit Platzhalter-Suche (*) möglich (z.B. Muster*')" 
                                                        runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_OrtSuche" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="10%">
                                                    <asp:TextBox ID="txtOrtSuche" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                                <td width="100%">
                                                   &nbsp;&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_PostleitzahlSuche" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="10%">
                                                    <asp:TextBox ID="txtPostleitzahlSuche" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                </td>
                                                <td width="100%">
                                        <asp:LinkButton ID="lb_Suche" CssClass="StandardButton" runat="server">Suchen</asp:LinkButton>
                                                </td>
                                            </tr>
                                            </table>
                                    </td>
                                </tr>
                               
                                <tr id="tr_Versandanschrift" class="TextLarge">
                                    <td align="left" nowrap="nowrap">
                                        <asp:RadioButton ID="rb_Versandanschrift" runat="server" AutoPostBack="true" GroupName="AdressArt" />
                                    </td>
                                    <td align="left" width="100%">
                                    </td>
                                </tr>
                                <tr id="trVersandanschriftValue">
                                    <td align="left">
                                    </td>
                                    <td align="left" width="100%">
                                        <table id="tblVersandanschrift" visible="false" runat="server" cellspacing="0" cellpadding="5" width="100%"
                                            bgcolor="white" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_FirmaName" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtName1" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_AnsprechpartnerZusatz" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtName2" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_Strasse" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtStrasse" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Hausnummer" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtHausnummer" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_Postleitzahl" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtPostleitzahl" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Ort" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtOrt" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_Land" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:DropDownList ID="ddlLand" runat="server" CssClass="DropDownNormal">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="left" width="50%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="tr_Bemerkung" class="StandardTableAlternate">
                                    <td align="left">
                                        <asp:Label ID="lbl_Bemerkung" runat="server" CssClass="TextLarge"></asp:Label>
                                    </td>
                                    <td align="left" width="100%">
                                        <asp:TextBox ID="txtBemerkung" runat="server" CssClass="TextBoxXLarge"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trVersandgrund" class="StandardTableAlternate">
                                    <td align="left">
                                        <asp:Label ID="lbl_Versandgrund" runat="server" CssClass="TextLarge"></asp:Label>
                                    </td>
                                    <td align="left" width="100%">
                                        <table id="tblVersandgrund" cellspacing="0" cellpadding="0" width="100%" bgcolor="white"
                                            border="0">
                                            <tr class="StandardTableAlternate">
                                                <td align="left" width="50%">
                                                    <asp:DropDownList ID="ddlVersandgrund" runat="server" AutoPostBack="True" 
                                                        CssClass="DropDownXLarge">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblVersandGrundZusatzBemerkung" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtVersandGrundZusatzEingabe" runat="server" CssClass="TextBoxNormal"
                                                        Visible="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trVersandart" class="StandardTableAlternate">
                                    <td align="left">
                                        <asp:Label ID="lbl_Versandart" runat="server" CssClass="TextLarge"></asp:Label>
                                    </td>
                                    <td align="left" width="100%">
                                    </td>
                                </tr>
                                <tr id="trVersandartValue">
                                    <td align="left">
                                    </td>
                                    <td align="left" width="100%">
                                        <table id="tblVersandart" runat="server" cellspacing="0" cellpadding="5" width="100%"
                                            bgcolor="white" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_Normal" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:RadioButton ID="rb_1391" runat="server" AutoPostBack="false" 
                                                        GroupName="Versandart" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Express" runat="server" CssClass="TextLarge"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:RadioButton ID="rb_1385" runat="server" AutoPostBack="false" GroupName="Versandart" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    </td>
                                                <td align="left" width="50%">
                                                    <asp:RadioButton ID="rb_5530" runat="server" AutoPostBack="false" 
                                                        GroupName="Versandart" Checked="True" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:RadioButton ID="rb_1389" runat="server" AutoPostBack="false" GroupName="Versandart" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:RadioButton ID="rb_Selbstabholung" runat="server" AutoPostBack="false" 
                                                        GroupName="Versandart" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:RadioButton ID="rb_1390" runat="server" AutoPostBack="false" GroupName="Versandart" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                               
                                <tr id="trZusatzanschreiben" class="StandardTableAlternate">
                                    <td align="left">
                                        <asp:Label ID="lbl_Zusatzanschreiben" runat="server" CssClass="TextLarge"></asp:Label>
                                    </td>
                                    <td align="left" width="100%">
                                    </td>
                                </tr>
                                <tr id="tr_ZusatzanschreibenValue">
                                    <td align="left">
                                    </td>
                                    <td align="left" width="100%">
                                        <table id="tblZusatzanschreiben" runat="server" runat="server" cellspacing="0" cellpadding="5"
                                            width="100%" bgcolor="white" border="0">
                                            <tr id="trParameterTemp1" visible="false">
                                                <td align="left" colspan="2">
                                                    <asp:CheckBox ID="chbEingentumsvorbehaltEintragen" Checked="true" Text="Eigentumsvorbehalt durch KLS eintragen"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="trParameterTemp2" visible="false">
                                                <td align="left" colspan="2">
                                                    <asp:CheckBox ID="chbBenutzerueberlassung" Text="Benutzungsüberlassung mitschicken"
                                                        runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr id="trParameterEndg1" visible="false">
                                                <td align="left" colspan="2">
                                                    <asp:CheckBox ID="chbEingentumsvorbehaltLoeschen" Text="Eingentumsvorbehalt durch KLS löschen"
                                                        runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr id="trParameterEndg2" visible="false">
                                                <td align="left" colspan="2">
                                                    <asp:CheckBox ID="chbDevinkulierungsschreiben" Text="Devinkulierungsschreiben mitschicken"
                                                        runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                   
                </table>
                </td>
                </tr>
     <tr>
            <td align="right">
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
                </table>
    </form>
</body>
</html>
