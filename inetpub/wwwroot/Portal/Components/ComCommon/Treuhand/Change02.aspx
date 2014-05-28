<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change02" %>

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
                                                <td class="TextLarge" valign="top" >
                                                    <asp:Label ID="lblCustomer" runat="server"  Text="Treunehmer:"></asp:Label>
                                                    </td>
                                                    <td style="width:90%" style="vertical-align:text-top;">
                                                      <span style="vertical-align:top"> <asp:RadioButtonList ID="rdbCustomer" 
                                                            runat="server" AutoPostBack="True">
                                                        </asp:RadioButtonList> </span>  
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                               </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table id = "tblUpload" runat="server" visible="false" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                        <tr>
                                                            <td class="TextLarge" style="width:10%">
                                                                <asp:Label ID="lblAktion" runat="server"  Text="Aktion:"></asp:Label></td>
                                                            <td class="TextLarge" >
                                                            <asp:RadioButton ID="rb_Sperren" GroupName="Vorgang" runat="server" 
                                                                Text="sperren" Checked="True" /></td>
                                                            <td class="TextLarge" style="width:88%">
                                                            <asp:RadioButton ID="rb_Entsperren" runat="server" GroupName="Vorgang" 
                                                                Text="entsperren" /></td>                                                                
                                                        </tr>                                                        
                                                        <tr>
                                                            <td class="TextLarge" nowrap>
                                                                Dateiauswahl <a href="javascript:openinfo('Info01.htm');">
                                                                    <img src="/Portal/Images/fragezeichen.gif" border="0"></a>:&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" style="width:95%" colspan="2">
                                                                <input id="upFile" type="file" size="49" name="File1" runat="server">&nbsp;
                                                            </td>
                                                        </tr>

                                                    </table>
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

    <script language="JavaScript" type="text/javascript">										
				<!--
						function openinfo (url) {
								fenster=window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=650,height=250");
								fenster.focus();
						}
				-->
    </script>
    </form>
</body>
</html>
