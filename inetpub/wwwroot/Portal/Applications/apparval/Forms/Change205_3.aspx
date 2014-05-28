<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%--<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change205_3.aspx.vb" Inherits="AppARVAL.Change205_3" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header><asp:imagebutton id="ImageButton1" runat="server" Width="3px" ImageUrl="/Portal/Images/empty.gif"></asp:imagebutton></td>
				</tr>
                <tr>
                    <td valign="top" align="left" colspan="3">
                        <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td class="PageNavigation" colspan="3">
                                    <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Adressauswahl)</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="120">
                                    <table id="Table12" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                        border="0">
                                        <tr>
                                            <td class="TaskTitle" width="150">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="center" width="150">
                                                <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="center" width="150">
                                                <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Suchen</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td class="TaskTitle" valign="top">
                                                <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change205.aspx"
                                                    CssClass="TaskTitle">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink ID="lnkFahrzeugAuswahl"
                                                        runat="server" NavigateUrl="Change205_2.aspx" CssClass="TaskTitle">Fahrzeugauswahl</asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td valign="top" align="left" colspan="3">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextLarge" valign="top" align="left" colspan="3">
                                                <table id="Table2" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                                    <tr id="trVersandAdrTemp" runat="server">
                                                        <td class="StandardTableAlternate">
                                                            <asp:Label ID="lblVersandAdresse" runat="server">Zulassungsstelle:</asp:Label>
                                                        </td>
                                                        <td class="StandardTableAlternate" valign="top" width="123">
                                                            <asp:DropDownList ID="ddlZulDienst" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="StandardTableAlternate" valign="top">
                                                            &nbsp;
                                                        </td>
                                                        <td class="StandardTableAlternate" valign="top" width="100%">
                                                            <p>
                                                                &nbsp;</p>
                                                        </td>
                                                    </tr>
                                                    <tr id="trVersandAdrEnd1" runat="server">
                                                        <td class="StandardTableAlternate" valign="center" align="left">
                                                            Name:
                                                        </td>
                                                        <td class="StandardTableAlternate" width="123">
                                                            <asp:TextBox ID="txtName1" runat="server" TabIndex="1" MaxLength="35"></asp:TextBox>
                                                        </td>
                                                        <td class="StandardTableAlternate" align="left">
                                                            Name2:
                                                        </td>
                                                        <td class="StandardTableAlternate" width="100%">
                                                            <asp:TextBox ID="txtName2" TabIndex="1" runat="server" MaxLength="35"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trVersandAdrEnd2" runat="server">
                                                        <td class="StandardTableAlternate" valign="top" align="left">
                                                            Str.:
                                                        </td>
                                                        <td class="StandardTableAlternate" valign="top" width="123">
                                                            <asp:TextBox ID="txtStr" runat="server" TabIndex="2" MaxLength="60"></asp:TextBox>
                                                        </td>
                                                        <td class="StandardTableAlternate" valign="top" nowrap align="left">
                                                            Nr.:
                                                        </td>
                                                        <td class="StandardTableAlternate" width="100%" valign="top">
                                                            <asp:TextBox ID="txtNr" runat="server" MaxLength="10" TabIndex="3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trVersandAdrEnd3" runat="server">
                                                        <td class="StandardTableAlternate" valign="top" align="left">
                                                            Plz.:
                                                        </td>
                                                        <td class="StandardTableAlternate" width="123">
                                                            <asp:TextBox ID="txtPlz" runat="server" TabIndex="4" MaxLength="10"></asp:TextBox>
                                                        </td>
                                                        <td class="StandardTableAlternate" align="left">
                                                            Ort:
                                                        </td>
                                                        <td class="StandardTableAlternate" width="100%">
                                                            <asp:TextBox ID="txtOrt" runat="server" TabIndex="5" MaxLength="40"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trVersandAdrEnd6" runat="server">
                                                        <td class="StandardTableAlternate" valign="top">
                                                        </td>
                                                        <td class="StandardTableAlternate" width="123">
                                                            <asp:TextBox ID="txtLand" TabIndex="4" runat="server" MaxLength="3" Visible="False"></asp:TextBox>
                                                        </td>
                                                        <td class="StandardTableAlternate">
                                                            Land:
                                                        </td>
                                                        <td class="StandardTableAlternate" width="100%">
                                                            <asp:DropDownList ID="ddlLand" runat="server" AutoPostBack="True" TabIndex="10">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="StandardTableAlternate" valign="top" colspan="4">
                                                        </td>
                                                    </tr>
                                                    <tr id="trVersandAdrEnd4" runat="server">
                                                        <td class="TextLarge" valign="baseline">
                                                            <asp:Label ID="txtGrund" runat="server">Versandgrund:</asp:Label>
                                                        </td>
                                                        <td class="" width="123">
                                                            <asp:DropDownList ID="ddlGrund" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="" colspan="2" width="100%" align="left" valign="baseline">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="tdInput1" runat="server" class="TextLarge" valign="baseline" align="left">
                                                            <asp:Label ID="lblMit" runat="server">auf</asp:Label>
                                                        </td>
                                                        <td id="tdInput" runat="server" colspan="3" nowrap>
                                                            <asp:TextBox ID="txtGrundBemerkung" TabIndex="4" runat="server" Width="300px" MaxLength="60"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trVersandAdrEnd5" runat="server">
                                                        <td class="StandardTableAlternate" valign="top" colspan="4">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trZeit" runat="server">
                                                        <td class="StandardTableAlternate" valign="top">
                                                            <asp:Label ID="lblVersandart" runat="server">Versandart:</asp:Label>
                                                        </td>
                                                        <td class="StandardTableAlternate" colspan="6">
                                                            <asp:RadioButton ID="chkVersandStandard" runat="server" Text="sendungsverfolgt" Checked="True"
                                                                GroupName="Versandart"></asp:RadioButton>
                                                            <asp:RadioButton ID="chk0900" runat="server" Text="vor 9:00 Uhr*" GroupName="Versandart">
                                                            </asp:RadioButton>
                                                            <asp:RadioButton ID="chk1000" runat="server" Text="vor 10:00 Uhr*" GroupName="Versandart">
                                                            </asp:RadioButton>
                                                            <asp:RadioButton ID="chk1200" runat="server" Text="vor 12:00 Uhr*" GroupName="Versandart">
                                                            </asp:RadioButton>
                                                            <asp:RadioButton ID="chkZweigstellen" runat="server" Visible="False" Text="Zweigstellen:"
                                                                Checked="True" GroupName="grpVersand"></asp:RadioButton>
                                                            <asp:RadioButton ID="chkZulassungsstellen" runat="server" Visible="False" Text="Zulassungsstellen:"
                                                                GroupName="grpVersand"></asp:RadioButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" colspan="3">
                                                *Diese Versandarten sind mit zusätzlichen Kosten verbunden.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" colspan="3">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <!--#include File="../../../PageElements/Footer.html" -->
                                                <br>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Literal ID="litScript" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <script language="JavaScript">										
							<!--																					
								function ShowHide()
								{
									o = document.getElementById("tdInput");
									p = document.getElementById("tdInput1");
									
									o.style.display = "none";
									p.style.display = "none";
									document.Form1.txtGrundBemerkung.value = "";
																											
									if (document.Form1.ddlGrund.value == "001"){
										o.style.display = "";
										p.style.display = "";
										window.document.Form1.txtGrundBemerkung.focus();
									}
									if (document.Form1.ddlGrund.value == "005"){
										o.style.display = "";
										p.style.display = "";
										window.document.Form1.txtGrundBemerkung.focus();
									}																							
								}								
							-->
                            </script>
                        </table>
                    </td>
                </tr>
            </table>
        </form>
	</body>
</HTML>
