<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01Aut.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change01Aut" %>

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
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Autorisieren</asp:LinkButton>
                                    </td>
                                </tr>
                           
                                <tr>
                                    <td valign="middle">
                                         <asp:LinkButton ID="cmdDel" runat="server" CssClass="StandardButton"> &#149;&nbsp;Löschen</asp:LinkButton></td>
                                </tr>
                           
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="lb_zurueck" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton></td>
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
                                        <td class="TextLarge" nowrap="nowrap" height="20">
                                            <asp:Label ID="lbl_Treunehmer" runat="server" >lbl_Treunehmer</asp:Label>
                                        &nbsp;</td>
                                        <td nowrap="nowrap" height="20">
                                              <asp:Label ID="lblTreunehmShow" runat="server" ></asp:Label></td>
                                        <td nowrap="nowrap" height="20">
                                              &nbsp;</td>
                                       <td class="TextLarge" nowrap="nowrap" height="20">
                                           <asp:Label ID="lbl_Aktion" runat="server" >lbl_Aktion</asp:Label>&nbsp;</td>
                                        <td style="width:100%" height="20">
                                            <asp:Label ID="lblAktionShow" runat="server" ></asp:Label></td>   
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap="nowrap" height="20">
                                           <asp:Label ID="lbl_Fin" runat="server" >lbl_Fin</asp:Label>&nbsp;</td>
                                        <td height="20">
                                            <asp:Label ID="lblFinShow" runat="server" ></asp:Label></td>
                                        <td height="20">
                                            &nbsp;</td>
                                       <td class="TextLarge" nowrap="nowrap" height="20">
                                           <asp:Label ID="lbl_Ersteller" runat="server" >lbl_Ersteller</asp:Label>&nbsp;</td>
                                        <td style="width:100%" height="20">
                                            <asp:Label ID="lblErstellerShow" runat="server" ></asp:Label></td>                                               
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap="nowrap" height="20">
                                           <asp:Label ID="lbl_Kennzeichen" runat="server" >lbl_Kennzeichen</asp:Label>&nbsp;</td>
                                        <td height="20">
                                            <asp:Label ID="lblKennzShow" runat="server" ></asp:Label></td>
                                        <td height="20">
                                            &nbsp;</td>
                                       <td class="TextLarge" nowrap="nowrap" height="20">
                                           <asp:Label ID="lbl_Ablehnender" runat="server" >lbl_Ablehnender</asp:Label>&nbsp;</td>
                                        <td style="width:100%" height="20">
                                            <asp:Label ID="lblAblehnShow" runat="server" ></asp:Label></td>                                               
                                    </tr>
                        
                                    <tr>
                                        <td class="TextLarge" nowrap="nowrap" style="vertical-align:top" height="20">
                                           <asp:Label ID="lbl_Versandadresse" runat="server" >lbl_Versandadresse</asp:Label>&nbsp;</td>
                                        <td height="20">
                                            <asp:Label ID="lblAdresseShow" runat="server" ></asp:Label></td>
                                        <td height="20">
                                            &nbsp;</td>
                                        <td class="TextLarge"  style="vertical-align:top" height="20">
                                           <asp:Label ID="lbl_AblehnGrund" runat="server" >lbl_AblehnGrund</asp:Label>&nbsp;</td>
                                        <td height="20" style="vertical-align:top">
                                            <asp:Label ID="lblAblehnGrundShow" runat="server" ></asp:Label></td>                                            
                                    </tr>                                                                        
                                     <tr>
                                         <td class="firstLeft active" nowrap="nowrap" colspan="5">
                                            <asp:Label ID="lblBELNR" runat="server" Visible="false" />&nbsp;</td>

                                    </tr>                                            

                                            <tr>
                                                <td class="LabelExtraLarge" colspan="2">
                                                       <asp:Label ID="lblError"
                                                        runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                                </td>
                                                <td class="LabelExtraLarge">
                                                       &nbsp;</td>
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