<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Mvc.aspx.vb" Inherits="CKG.Portal.Start.Mvc" %>
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
        
        <script type="text/javascript">

            function isIeAndVersion() {
                var myNav = navigator.userAgent.toLowerCase();
                return (myNav.indexOf('msie') != -1) ? parseInt(myNav.split('msie')[1]) : 9999;
            }

            function getWindowWidth() {
                return document.body.clientWidth - 50;
            }

            function getWindowHeight() {
                var height = 0;
                if (isIeAndVersion() < 10)
                    height = window.document.clientHeight;
                else
                    height = window.document.documentElement.clientHeight;

                return height - 130;
            }

            function onResize() {
                var ifr = document.getElementById('ifrMvcApp');
                ifr.width = getWindowWidth();
                ifr.height = getWindowHeight();
            }

        </script>

	</HEAD>

	<body leftMargin="0" topMargin="0" onload="onResize()" onresize="onResize()">

		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center">
				<TBODY>
					<tr>
						<td><uc1:header id="ucHeader" runat="server" border="0"></uc1:header></td>
					</tr>
				</TBODY>
            </table>
            
            <br/><br/>

            <iframe runat="server" ID="ifrMvcApp" scrolling="auto" width="1900" height="800" frameBorder="0"></iframe>

        </form>
	</body>
</HTML>
