<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Report02" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:styles id="ucStyles" runat="server">
    </uc1:styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="scriptmanager1" EnableScriptGlobalization="true" runat="server">
    </asp:ScriptManager>  
    <table id="Table4" width="100%" align="center">
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
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width:140px">
                            <table id="Table2" cellspacing="0" cellpadding="0" style="width:140px"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" >
                                        &nbsp;
                                    </td>
                                </tr>
                           
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="cmdWeiter" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
 
                            </table>  
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td  valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td  colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" style="vertical-align:top; padding-top:5px;width:10%">
                                                    <asp:Label ID="lblCustomer" runat="server"  Text="Treunehmer:"></asp:Label>
                                                    </td>
                                                    <td style="width:90%" >
                                                      <span style="vertical-align:top"> 
                                                        <asp:RadioButtonList ID="rdbCustomer" 
                                                            runat="server">
                                                        </asp:RadioButtonList> </span>  
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" nowrap="nowrap">
                                                    <asp:Label ID="lblERDatvon" runat="server"  Text="Datum von:"></asp:Label></td>
                                                <td style="width:90%">
                                                    <asp:TextBox ID="txtERDatvon" runat="server"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CE_ERDatvon" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtERDatvon">
                                                </cc1:CalendarExtender> 
                                                <cc1:MaskedEditExtender ID="MEE_ERDatvon" runat="server" TargetControlID="txtERDatvon"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </cc1:MaskedEditExtender>                                                                                                   
                                                  </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" nowrap="nowrap" style="width:8%">
                                                    <asp:Label ID="lblERDatbis" runat="server"  Text="Datum bis:"></asp:Label></td>
                                                <td style="width:90%">
                                                    <asp:TextBox ID="txtERDatbis" runat="server"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CE_ERDatbis" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtERDatbis">
                                                </cc1:CalendarExtender> 
                                                <cc1:MaskedEditExtender ID="MEE_ERDatbis" runat="server" TargetControlID="txtERDatbis"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </cc1:MaskedEditExtender>                                                                                                   
                                                  </td>
                                            </tr>                                            
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                               </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelExtraLarge" colspan="2">
                                                       <asp:Label ID="lblError"
                                                        runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                     </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
