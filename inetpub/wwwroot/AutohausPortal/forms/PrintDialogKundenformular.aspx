<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="PrintDialogKundenformular.aspx.cs" Inherits="AutohausPortal.forms.PrintDialogKundenformular" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=iso-8859-1" />
    <meta name="author" content="Christoph Kroschke GmbH" />
    <meta content="Christoph Kroschke GmbH" name="Copyright" />
    <link href="../Styles/kroschkeportal.css" type="text/css" rel="stylesheet" />
    <link href="../Styles/ezmark.css" type="text/css" rel="stylesheet" />
    <link href="../Styles/jquery.selectBox.css" type="text/css" rel="stylesheet" />
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/jquery-1.7.1.js"></script>
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/jquery.selectBox.js"></script>
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/jquery-ui.min.js"></script>
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/ezmark.js?23052013"></script>
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/kroschkeportal.js?26052014"></script>
     <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/jquery.blockUI.js"></script>
    <link rel="stylesheet" type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" />
</head>
    <script type="text/javascript">
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oWnd = GetRadWindow();
            oWnd.close();

        }
    </script>
    <body style="background-image:none!important; width: 245px;">
        <form id="form1" runat="server">
            <div id="maincontainer" style="width:100%;">
                <div class="content" style="width:100%;">
                    <div class="formbuttons">
                        <table align="center">
                            <tr>
                                <td align="center">
                                    <asp:ImageButton ID="cmdPrint" runat="server" ImageUrl="/AutohausPortal/Images/logoPDF.png" onclick="cmdPrint_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="cmdCloseDialog" runat="server" CssClass="dynbutton" Text="Schließen" onclick="cmdCloseDialog_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </form>
    </body>
</html>
