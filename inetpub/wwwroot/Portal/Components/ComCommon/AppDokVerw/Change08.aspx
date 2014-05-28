<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change08.aspx.vb" Inherits="CKG.Components.ComCommon.Change08" %>
<%--<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>--%>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
                        <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td class="PageNavigation" colspan="2">
                                    <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"
                                        Visible="False"> (Fahrzeugsuche)</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="120" height="192">
                                    <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                        border="0">
                                        <tr>
                                            <td class="TaskTitle" width="120">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trcmdUpload" runat="server">
                                            <td valign="middle" width="120">
                                                <asp:LinkButton ID="cmdUpload" runat="server" CssClass="StandardButton"> &#149;&nbsp;Mehrfachauswahl</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr id="trcmdSearch" runat="server">
                                            <td valign="middle" width="120">
                                                <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr id="trcmdContinue" runat="server">
                                            <td valign="middle" width="120">
                                                <asp:LinkButton ID="cmdContinue" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" height="192">
                                    <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td class="TaskTitle" valign="top">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblSelection" cellspacing="0" cellpadding="0" width="100%" border="0"
                                        runat="server">
                                        <tr>
                                            <td valign="top" align="left">
                                                <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0">
                                                    <tr id="tr_Leasingvertragsnummer" runat="server">
                                                        <td class="TextLarge" nowrap align="right">
                                                            <asp:Label ID="lbl_Leasingvertragsnummer" runat="server">lbl_Leasingvertragsnummer</asp:Label>&nbsp;&nbsp;
                                                        </td>
                                                        <td class="TextLarge">
                                                            <asp:TextBox ID="txtOrdernummer" runat="server" Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_Kennzeichen" runat="server">
                                                        <td class="StandardTableAlternate" nowrap align="right">
                                                            <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>&nbsp;&nbsp;
                                                        </td>
                                                        <td class="StandardTableAlternate">
                                                            <asp:TextBox ID="txtAmtlKennzeichen" runat="server" Width="250px"></asp:TextBox>&nbsp;(XX-Y1234)
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_KennzeichenZusatz" runat="server">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            * Eingabe von nachgestelltem Platzhalter möglich. Mindestens Kreis und ein Buchstabe
                                                            (z.B. XX-Y*)
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_NummerZB2" runat="server">
                                                        <td class="StandardTableAlternate" nowrap align="right">
                                                            <asp:Label ID="lbl_NummerZB2" runat="server">lbl_NummerZB2</asp:Label>&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td class="StandardTableAlternate">
                                                            <asp:TextBox ID="txtNummerZB2" runat="server" Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_Fahrgestellnummer" runat="server">
                                                        <td class="TextLarge" nowrap align="right">
                                                            <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td class="TextLarge">
                                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" Width="250px" MaxLength="17"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_FahrgestellnummerZusatz" runat="server">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            ** Eingabe von vorangestelltem Platzhalter möglich. Mindestens fünf Zeichen (z.B.
                                                            *12345)
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_Alle" runat="server">
                                                        <td valign="top" class="StandardTableAlternate" nowrap align="right">
                                                            <asp:Label ID="lbl_alle" runat="server">kompletter Bestand</asp:Label>&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td class="StandardTableAlternate">
                                                            <asp:CheckBox ID="chk_alle" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_Platzhaltersuche" runat="server">
                                                        <td valign="top" nowrap align="right">
                                                            <asp:Label ID="lbl_Platzhaltersuche" runat="server">lbl_Platzhaltersuche</asp:Label>&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="cbxPlatzhaltersuche" runat="server" Checked="True" GroupName="grpWeicheSuche"
                                                                Text=" Platzhaltersuche möglich. Nur verwendbare Vorgänge werden angezeigt.">
                                                            </asp:RadioButton><br>
                                                            <asp:RadioButton ID="cbxOhnePlatzhalter" runat="server" GroupName="grpWeicheSuche"
                                                                Text=" Platzhalter werden ignoriert. Informationen zu dem Vorgang werden angezeigt, insofern er im System gefunden wurde.">
                                                            </asp:RadioButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblUpload" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                                        <tr>
                                            <td valign="top" align="left">
                                                <table id="tbl0001" cellspacing="0" cellpadding="5" width="100%" border="0">
                                                    <tr>
                                                        <td class="TextLarge" nowrap align="right">
                                                            Dateiauswahl <a href="javascript:openinfo('Info01.htm');">
                                                                <img src="/Portal/Images/fragezeichen.gif" border="0"></a>:&nbsp;&nbsp;
                                                        </td>
                                                        <td class="TextLarge">
                                                            <input id="upFile" type="file" size="49" name="File1" runat="server">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TextLarge" nowrap align="right">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TextLarge">
                                                            &nbsp;
                                                            <asp:Label ID="lblExcelfile" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="120">
                                    &nbsp;
                                </td>
                                <td valign="top">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="120">
                                    &nbsp;
                                </td>
                                <td>
                                    <!--#include File="../../../PageElements/Footer.html" -->
                                </td>
                            </tr>
                        </table>
					</td>
				</tr>
			</table>
		<asp:literal id="Literal1" runat="server"></asp:literal>
        <script language="JavaScript">										
				<!--
						function openinfo (url) {
								fenster=window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=650,height=250");
								fenster.focus();
						}
				-->
		</script>
		</form>
		</body>
</HTML>
