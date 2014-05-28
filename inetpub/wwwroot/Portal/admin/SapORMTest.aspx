<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SapORMTest.aspx.vb" Inherits="CKG.Admin.SapORMTest" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body>
    <form id="form1" runat="server">
    
    <div>
 <asp:ScriptManager ID="Scriptmanager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ScriptManager>
        <table cellspacing="0" cellpadding="0" width="100%" align="center">
            <tbody>
                <tr>
                    <td>
                        <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td class="PageNavigation" colspan="2" height="25">
                                        <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="120">
                                        <table id="Table2" cellspacing="0" cellpadding="0" width="120"
                                            border="0">
                                            <tr>
                                                <td class="TaskTitle">
                                                    <asp:LinkButton ID="btnCallSAP" runat="server" Text="Call SAP" CssClass="StandardButton"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" width="150">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" width="150">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" width="150">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" width="150">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                                            <tbody>
                                                <tr>
                                                    <td class="TaskTitle" colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr id="trSearch" runat="server">
                                                    <td align="left" colspan="2">
                                                        <table>
                                                            <tr>
                                                                <td>TestBapi:</td>
                                                                <td style="padding: 5px 5px 5px 5px;">
                                                                    <asp:TextBox ID="txtTestBapi" runat="server" AutoPostBack="true" Width="300px" Text="Z_M_BAPIRDZ"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>ErpConnector vorhanden?</td>
                                                                <td style="padding: 5px 5px 5px 5px;">
                                                                    <asp:Image id="imgERPvorhanden" runat="server" ImageUrl="../Images/Problem.jpg"/>
                                                                    <asp:Label ID="lblERPvorhanden" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                                <td>SAP ORM vorhanden?</td>
                                                                <td style="padding: 5px 5px 5px 5px;">
                                                                    <asp:Image id="imgSAPORMvorhanden" runat="server" ImageUrl="../Images/Problem.jpg"/>
                                                                    <asp:Label ID="lblSAPORMvorhanden" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                                <td>SAP erreichbar?</td>
                                                                <td style="padding: 5px 5px 5px 5px;">
                                                                    <asp:Image id="Image2" runat="server" ImageUrl="../Images/Problem.jpg"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>                                           
                                                <tr>
                                                    <td align="left" height="25" colspan="2">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label><asp:Label
                            ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <!--#include File="../PageElements/Footer.html" -->
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>

