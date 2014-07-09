<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Selection.aspx.vb" Inherits="CKG.Portal.Start.Selection" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="True" name="vs_showGrid">
		<meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
        <script type="text/javascript" src="/Portal/Scripts/jquery-1.7.1.min.js"></script>
        <script type="text/javascript">

            function LogPageVisit(appId, href) {
                // Logging für IE8 vorerst überspringen
                if (IsIEVersionOrLower(8)) {
                    return true;
                }

                var url = '/Portal/Log.aspx?APP-ID=' + appId;
                $.get(url).always(function () {
                    window.location.href = href;
                });

                return false;
            }

            function IsIEVersionOrLower(ieVersion) {
                if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
                    var ieversion = new Number(RegExp.$1);
                    if (ieversion <= ieVersion) {
                        return true;
                    }
                }

                return false;
            }

        </script>
        <style type="text/css">
            
            .MainmenuItem {
                height: 18px;
            }
            
        </style>
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center">
				<TBODY>
					<tr>
						<td><uc1:header id="ucHeader" runat="server" border="0"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<TD class="PageNavigation" align="left" width="100" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label></TD>
									<TD class="PageNavigation" vAlign="center" width="100%">&nbsp;
										<asp:hyperlink id="lnkAdmin" runat="server" Visible="False" CssClass="PageNavigation" NavigateUrl="../Admin/AdminMenu.aspx">Administration</asp:hyperlink></TD>
								</TR>
								<tr>
									<td colSpan="3">&nbsp;&nbsp;
										<BR>
										<table id="tbAnwendungen" cellSpacing="0" cellPadding="0" border="0" runat="server">
										</table>
									</td>
								</tr>
								<TR id="tr4" runat="server">
									<TD colSpan="2"></TD>
									<TD><asp:label id="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:label><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
								</TR>
								<TR id="tr5" runat="server">
									<TD colSpan="2"></TD>
									<TD><!--#include File="../PageElements/Footer.html" --></TD>
								</TR>
							</TABLE>
						</td>
					</tr>
				</TBODY></table>
			<asp:literal id="Literal1" runat="server"></asp:literal><asp:literal id="litAlert" runat="server"></asp:literal></form>
	</body>
</HTML>
