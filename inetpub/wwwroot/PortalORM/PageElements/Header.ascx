<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Header.ascx.vb" Inherits="CKG.Portal.PageElements.Header"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" %>
<asp:Literal ID="litSetBackground" runat="server"></asp:Literal>
<table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
    <tr>
        <td valign="bottom" align="left" width="189" colspan="2">
            <asp:Image ID="imgCustomerLogo" runat="server" Visible="False"></asp:Image>
        </td>
        <td class="" valign="bottom" nowrap="nowrap" align="left" width="100%">
            <asp:Label ID="lblUserName" runat="server" BorderWidth="1px" BorderStyle="Solid"
                BorderColor="Gainsboro" Font-Size="9px"></asp:Label>
        </td>
        <td class="HeaderLinks" valign="bottom" nowrap="nowrap" id="tdMessage" visible="false"
            runat="server">
            <a href="javascript:openinfoMessage();">
                <img runat="server" id="imgMessage" alt="Wichtige Mitteilung!" src="../Images/yellow.jpg"
                    border="0" width="16" height="16" /></a> <span id="SessionTimeCount"></span>
        </td>        
       <td class="HeaderLinks" valign="bottom" id="tdQuickSupport" runat="server"
            visible="false">
            <!-- TeamViewer Logo (generated at http://www.teamviewer.com) -->
            <div id="divTeamviewer" runat="server" style="position: relative; width: 50px; height: 50px;">
                <a href="/PortalORM/Downloads/DADQS_de.exe" style="text-decoration: none;">
                     <img src="/PortalORM/Images/TeamviewerLogo.png" alt="Download TeamViewer"
                    title="Download TeamViewer" border="0" style="position:relative; z-index:4; margin:0px 0px 0px 0px;"/></a>
            </div>
        </td>
        <td class="HeaderLinks" valign="bottom" nowrap="nowrap" id="tdHauptmenue" runat="server">
            <asp:HyperLink ID="lnkHauptmenue" runat="server" NavigateUrl="/PortalORM/Start/Selection.aspx">Hauptmenü</asp:HyperLink>
        </td>
        <td class="HeaderLinks" id="tdChangePasword" valign="bottom" nowrap="nowrap" runat="server">
            <asp:HyperLink ID="lnkChangePassword" runat="server" Visible="False" NavigateUrl="/PortalORM/Start/ChangePassword.aspx">Passwort ändern</asp:HyperLink>
        </td>
        <td class="HeaderLinks" id="tdLogout" valign="bottom" nowrap="nowrap" runat="server">
            <asp:HyperLink ID="lnkLogout" runat="server" Visible="False" NavigateUrl="/PortalORM/Start/Logout.aspx">Abmelden</asp:HyperLink>
        </td>
        <td class="HeaderLinks" id="tdHandbuch" valign="bottom" nowrap="nowrap" runat="server">
            <asp:HyperLink ID="lnkHandbuch" runat="server" Target="_blank">Handbuch</asp:HyperLink>
        </td>
        <td class="HeaderLinks" valign="bottom" nowrap="nowrap">
            <asp:HyperLink ID="lnkContact" NavigateUrl="/PortalORM/Info/ContactPage.aspx" runat="server">Kontakt</asp:HyperLink>
        </td>
        <td class="HeaderLinks" valign="bottom" nowrap="nowrap" id="tdResponsible" runat="server">
            <asp:HyperLink ID="lnkResponsible" NavigateUrl="/PortalORM/Info/ResponsiblePage.aspx"
                runat="server">Ansprechpartner</asp:HyperLink>
        </td>
        <td class="HeaderLinks" valign="bottom" nowrap="nowrap">
            <asp:HyperLink ID="lnkImpressum" NavigateUrl="/PortalORM/Info/Impressum.aspx" runat="server">Impressum</asp:HyperLink>
        </td>
        <td valign="baseline" align="right" colspan="1">
            <img id="imgDADLogo" hspace="0" border="0" runat="server" />
        </td>
    </tr>
</table>
<script language="JavaScript" type="text/javascript">
										<!--    //
    // window.document.Form1.elements[window.document.Form1.length-3].focus();
    //-->
    function openinfoMessage() {
        var width = 550;
        var height = 200;
        var left = parseInt((screen.availWidth / 2) - (width / 2));
        var top = parseInt((screen.availHeight / 2) - (height / 2));
        var windowFeatures = "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=" + width + ",height=" + height + ",status,resizable,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
        fenster = window.open("../Info/VIMessage.aspx", "Mitteiling", windowFeatures);
        fenster.focus();
    }
    var mins
    var secs

    function __doPostBack(eventTarget, eventArgument) {
        if (!document.forms[0].onsubmit || (document.forms[0].onsubmit() != false)) {
            document.forms[0].__EVENTTARGET.value = eventTarget;
            document.forms[0].__EVENTARGUMENT.value = eventArgument;
            document.forms[0].submit();
        }
    }


    function cd(Min, sec) {
        mins = 1 * m(Min); // change minutes here
        secs = 0 + s(":" + sec); // change seconds here (always add an additional second to your total)
        redo();
    }

    function m(obj) {
        for (var i = 0; i < obj.length; i++) {
            if (obj.substring(i, i + 1) == ":")
                break;
        }
        return (obj.substring(0, i));
    }

    function s(obj) {
        for (var i = 0; i < obj.length; i++) {
            if (obj.substring(i, i + 1) == ":")
                break;
        }
        return (obj.substring(i + 1, obj.length));
    }

    function dis(mins, secs) {
        var disp;
        if (mins <= 9) {
            disp = " 0";
        } else {
            disp = " ";
        }
        disp += mins + ":";
        if (secs <= 9) {
            disp += "0" + secs;
        } else {
            disp += secs;
        }
        return (disp);
    }

    function redo() {
        secs--;
        if (secs == -1) {
            secs = 59;
            mins--;
        }
        document.getElementById('SessionTimeCount').innerHTML = " (Abmeldung in " + dis(mins, secs) + " Minuten)" // setup additional displays here.
        if ((mins == 0) && (secs == 0)) {
            __doPostBack('__Page', 'Logout'); // change timeout message as required
            // window.location = "yourpage.htm" // redirects to specified page once timer ends and ok button is pressed
        } else {
            cd = setTimeout("redo()", 1000);
        }
    }
    
</script>
