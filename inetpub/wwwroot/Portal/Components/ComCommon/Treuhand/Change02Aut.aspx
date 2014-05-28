<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02Aut.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change02Aut" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:styles id="ucStyles" runat="server">
    </uc1:styles>
</head>

<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
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
                                        <asp:LinkButton ID="cmdSave" Visible="false" runat="server" CssClass="StandardButton"> &#149;&nbsp;Autorisieren</asp:LinkButton>
                                    </td>
                                </tr>
                           
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="cmdDel" Visible="false" runat="server" CssClass="StandardButton"> &#149;&nbsp;Löschen</asp:LinkButton></td>
                                </tr>
                           
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="lb_zurueck" Visible="false" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton></td>
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
                                                <td>
                                                    &nbsp;&nbsp;</td>
                                            </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap="nowrap">
                                            <asp:Label ID="lbl_Treunehmer" runat="server" >lbl_Treunehmer</asp:Label>
                                        &nbsp;</td>
                                        <td nowrap="nowrap">
                                              &nbsp;<asp:Label ID="lblTreunehmShow" runat="server" ></asp:Label></td>
                                        <td nowrap="nowrap">
                                              &nbsp;</td>
                                       <td class="TextLarge" nowrap="nowrap">
                                           &nbsp;<asp:Label ID="lbl_Aktion" runat="server" >lbl_Aktion</asp:Label>&nbsp;</td>
                                        <td style="width:100%" height="20">
                                            &nbsp;
                                            <asp:Label ID="lblAktionShow" runat="server" ></asp:Label></td>   
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap="nowrap">
                                           <asp:Label ID="lbl_Referenz" runat="server" >lbl_Referenz</asp:Label>&nbsp;</td>
                                        <td>
                                            &nbsp;<asp:Label ID="lblRefShow" runat="server" ></asp:Label></td>
                                        <td>
                                            &nbsp;</td>
                                       <td class="TextLarge" nowrap="nowrap">
                                           <asp:Label ID="lbl_Sachbearbeiter" runat="server" >lbl_Sachbearbeiter</asp:Label>&nbsp;</td>
                                        <td style="width:100%" height="20">
                                           &nbsp;<asp:Label ID="lblSachbShow" runat="server" ></asp:Label></td>                                               
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap="nowrap">
                                           <asp:Label ID="lbl_Datum" runat="server" >lbl_Datum</asp:Label>&nbsp;</td>
                                        <td>
                                            <asp:Label ID="lblDatumShow" runat="server" ></asp:Label></td>
                                        <td>
                                            &nbsp;</td>
                                       <td class="TextLarge" nowrap="nowrap">
                                           <asp:Label ID="lbl_SperrDatum" runat="server" >lbl_SperrDatum</asp:Label>&nbsp;</td>
                                        <td style="width:100%" height="20">
                                            &nbsp;<asp:Label ID="lbl_SperrDatShow" runat="server" Height="20px" ></asp:Label></td>                                               
                                    </tr>
                                     <tr>
                                         <td class="firstLeft active" nowrap="nowrap" colspan="5">
                                            <asp:Label ID="lblBELNR" runat="server" Visible="false" />&nbsp;</td>

                                    </tr>                                            

                                            <tr>
                                                <td class="LabelExtraLarge" colspan="5">
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