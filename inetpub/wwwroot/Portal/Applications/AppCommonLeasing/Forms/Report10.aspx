<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report10.aspx.vb" Inherits="AppCommonLeasing.Report10" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:styles id="ucStyles" runat="server">
    </uc1:styles>
    <style type="text/css">
        .style1
        {
            width: 8%;
            height: 32px;
        }
        .style2
        {
            width: 100%;
            height: 32px;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager ID="scriptmanager1" EnableScriptGlobalization="true" runat="server">
    </asp:ScriptManager>  
    <table id="Table4" width="100%" align="center">
        <tbody>
            <tr>
                <td>
                    <uc1:header id="ucHeader" runat="server">
                    </uc1:header>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="PageNavigation" colspan="2">
                                    <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="PageNavigation" colspan="2">
                                </td>
                            </tr>
                            <tr>
                        <td valign="top" style="width:140px">
                            <table id="Table2" cellspacing="0" cellpadding="0" style="width:140px"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" >
                                        <asp:LinkButton ID="lb_zurueck" Visible="True" runat="server">lb_zurueck</asp:LinkButton>
                                    </td>
                                </tr>
                           
                                <tr>
                                    <td valign="middle" style="width:140px">
                                        <asp:LinkButton ID="cmdWeiter" Height="18px" Width="120px" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:LinkButton>
                                    </td>
                                </tr>
 
                            </table>  
                        </td>
                        <td valign="top">
                                    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td valign="top" align="left">
                                                    <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                                        <tr>
                                                            <td colspan="2" class="TaskTitle">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                                            </td>
                                                        </tr>
                                                            <tr>
                                                                <td class="TextLarge" nowrap="nowrap" style="width:8%">
                                                                    <asp:Label ID="lbl_ERDatvon" runat="server" >lbl_ERDatvon</asp:Label></td>
                                                                <td style="width:100%">
                                                                    <asp:TextBox ID="txtERDatvon" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="CE_ERDatvon" runat="server" Format="dd.MM.yyyy"
                                                                    PopupPosition="BottomLeft"  Animated="false" Enabled="True" TargetControlID="txtERDatvon">
                                                                </cc1:CalendarExtender> 
                                                                                             
                                                                  </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TextLarge" nowrap="nowrap">
                                                                    <asp:Label ID="lbl_ERDatbis" runat="server">lbl_ERDatbis</asp:Label></td>
                                                                <td class="style2">
                                                                    <asp:TextBox ID="txtERDatbis" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="CE_ERDatbis" runat="server" Format="dd.MM.yyyy"
                                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtERDatbis">
                                                                </cc1:CalendarExtender> 
                                                                                                
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
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    &nbsp;
                                </td>
                                <td valign="top">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;<asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                             
                        </td>
                    </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
    
</body>
</html>
