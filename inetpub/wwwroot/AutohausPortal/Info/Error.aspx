<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="AutohausPortal.Info._Error" %>
     
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="de" xml:lang="de" xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=iso-8859-1" />
    <meta name="author" content="Christoph Kroschke GmbH" />
	<meta content="Christoph Kroschke GmbH" name="Copyright" /> 
    <link href="../Styles/kroschkeportal.css" type="text/css" rel="stylesheet" />
    <link href="../Styles/ezmark.css" type="text/css" rel="stylesheet" />
    <link href="../Styles/jquery.selectBox.css" type="text/css" rel="stylesheet" />
		<!--[if lte IE 10]>
		<link rel="stylesheet" type="text/css" href="/AutohausPortal/Styles/iestyles.css">
		<![endif]--><!--[if IE 7]>
		<link rel="stylesheet" type="text/css" href="/AutohausPortal/Styles/ie7styles.css">
		<![endif]--><!--[if lte IE 6]>
		<link rel="stylesheet" type="text/css" href="/AutohausPortal/Styles/ie6styles.css">
		<![endif]-->
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/jquery-1.7.1.js"></script> 
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/jquery.selectBox.js"></script>
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/jquery-ui.min.js"></script>
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/ezmark.js?23052013"></script>
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/kroschkeportal.js?26052014"></script>
</head>

<body id="top" class="start">
    <form id="Form1" runat="server" style="margin: 0">
        <div id="maincontainer">
			<!--head-->
			<div id="head" class="login">
				<div class="logo">
					<img src="../images/kroschke-logo.gif" width="382" height="51" alt="Kroschke" />
				</div>
				<!--metanavigation-->
				<div id="metanavigation">

				</div>
				<!--metanavigation-->
			</div>
			<!--head-->

        <div id="contentlogin">
			<div class="inhaltsseite">
				<div class="inhaltsseite_top">&nbsp;</div>
				<div class="innerwrap">
				<h1>Es ist ein Fehler aufgetreten!</h1>	
				<p><asp:Label ID="lblErrorMessage" Style="color: #B54D4D" runat="server" Font-Bold="True"></asp:Label><br />
				
				</p>
				<p>Bitte wenden Sie sich an Ihren Administrator!<br />
				</p>

				<p>  <asp:Label ID="lblCName" runat="server" Font-Bold="True">&nbsp;Christoph Kroschke GmbH</asp:Label><br />
                                            <asp:Label ID="lblCAddress" runat="server">&nbsp;Ladestraße 1, 22926 Ahrensburg<br />&nbsp;Hotline: +49 (0)4102 804-170</asp:Label>
                                            <asp:Panel ID="pnlLinks" style="padding-bottom: 15px" runat="server">
                                                <asp:HyperLink ID="lnkMail" runat="server" NavigateUrl="mailto:service@kroschke.de">&nbsp;service[at]kroschke.de</asp:HyperLink>
                                                <br />
                                                <asp:HyperLink ID="lnkWeb" runat="server" NavigateUrl="http://www.kroschke.de">&nbsp;www.kroschke.de</asp:HyperLink>
                                            </asp:Panel><br />  
</p>

				</div>
				<div class="inhaltsseite_bot">&nbsp;</div>		
			</div>
        </div>
         <div class="trenner">&nbsp;</div>
     </div>

          <div class="trenner20">&nbsp;</div>
            <div id="footer" style="margin-top: 0px;">
                    <div class="footcontent">
                        <div class="footcontentl">
                            <a href="#top">Nach oben scrollen</a>
                        </div>
	                    <div class="footcontentr"> <asp:Label ID="lblCopyright" runat="server" Text="© year Christoph Kroschke GmbH"/></div>
                   </div>
             </div>


    </form>

 </body>        
</html> 





