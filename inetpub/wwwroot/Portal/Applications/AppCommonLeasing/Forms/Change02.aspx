<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02.aspx.vb" Inherits="AppCommonLeasing.Change02" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        
    </style>
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
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TaskTitle" valign="top" colspan="2">
                                &nbsp;
                            </td>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table id="Table2" cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                    <tr id="trCreate" runat="server">
                        <td valign="center">
                            <asp:LinkButton ID="lb_Weiter" OnClientClick="Show_BusyBox1();" Text="Weiter" runat="server"
                                CssClass="StandardButton"></asp:LinkButton>
                        </td>
                        
                    </tr>
                    <tr>
                        <td valign="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <div align="center" style="margin-left: -60px">
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label><br />
                    <br />
                    <asp:Label ID="lblErrorDetails" runat="server"></asp:Label>
                    <asp:HyperLink ID="lnkFahrzeughistorie" runat="server" Text="Fahrzeughistorie" ToolTip="Anzeige Fahrzeughistorie"
                        Target="_blank" />
                </div>
            </td>
        </tr>
        
        <tr id="tr_Upload" runat="server">
            <td valign="top">
                &nbsp;
            </td>
            <td>
                <table id="Table11" cellspacing="0" cellpadding="5" width="100%" bgcolor="white"
                    border="0">
                    <tr>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rb_Auswahl" runat="server" Font-Bold="True" RepeatDirection="Horizontal"
                                TextAlign="Left" AutoPostBack="True">
                                <asp:ListItem Selected="True" Value="1">Einzelauswahl</asp:ListItem>
                                <asp:ListItem Value="2">Upload Kennzeichen</asp:ListItem>
                                <asp:ListItem Value="3">Upload Vertragsnummern</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left">
                            <table id="tblUpload" runat="server" cellspacing="0" cellpadding="5" width="100%"
                                border="0">
                                <tr>
                                    <td nowrap align="left">
                                        &nbsp;
                                    </td>
                                    <td class="TextLarge" width="100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap align="left">
                                        <div id="info1" runat="server">
                                            Dateiauswahl <a class="tip" href="#">
                                                <img border="0" src="/Portal/Images/fragezeichen.gif" />
                                                <span>
                                                <table id="Table1" cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100%" nowrap="nowrap">
                                                            Upload von Kennzeichen
                                                        </td>
                                                    </tr>                                                    
                                                    <tr>
                                                        <td   style="width: 100%" nowrap="nowrap">
                                                            Erwarteter Dateityp für den Upload: Excel-Datei (*.xls)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%">
                                                            Erwartetes Dateiformat (<strong>Ohne</strong> Spaltenüberschriften)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div>
                                                            W-123HG
                                                            </div>
                                                            <div>
                                                            DL-812BM
                                                            </div>
                                                            <div>
                                                            WT-894AR
                                                            </div>
                                                            <div class="Last">
                                                                &nbsp;
                                                            </div>                                                                                                                        
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td >
                                                            
                                                        </td>
                                                    </tr>

                                                </table>        
                                                
                                                </span>
                                                
                                                </a> :&nbsp;&nbsp;
                                        </div>
                                        <div id="info2" runat="server">
                                            Dateiauswahl <a class="tip" href="#">
                                                <img border="0" src="/Portal/Images/fragezeichen.gif" />
                                                <span>
                                                <table id="InfoTab" cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100%" nowrap="nowrap">
                                                            Upload von Vertragsnummern
                                                        </td>
                                                    </tr>                                                    
                                                    <tr>
                                                        <td   style="width: 100%" nowrap="nowrap">
                                                            Erwarteter Dateityp für den Upload: Excel-Datei (*.xls)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%">
                                                            Erwartetes Dateiformat (<strong>Ohne</strong> Spaltenüberschriften)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div>
                                                            40182471
                                                            </div>
                                                            <div>
                                                            40182472
                                                            </div>
                                                            <div>
                                                            40182473
                                                            </div>
                                                            <div class="Last">
                                                                &nbsp;
                                                            </div>                                                                                                                        
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td >
                                                            
                                                        </td>
                                                    </tr>

                                                </table>        
                                                
                                                </span>
                                                
                                                </a> :&nbsp;&nbsp;
                                        </div>
                                    </td>
                                    <td class="TextLarge" width="100%">
                                        <input id="upFile" type="file" size="49" name="File1" runat="server">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap align="left">
                                        &nbsp;
                                    </td>
                                    <td class="TextLarge">
                                        &nbsp;
                                        <asp:Label ID="lblExcelfile" runat="server"></asp:Label>
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
            
            
            
            <td valign="top">
                <table id="tblEinzelauswahl" runat="server" cellspacing="0" cellpadding="5" width="100%"
                    bgcolor="white" border="0">
                    
                    <tr id="tr_Leasingvertragsnummer" class="TextLarge">
                        <td align="left">
                            <asp:Label ID="lbl_Leasingvertragsnummer" runat="server"></asp:Label>
                        </td>
                        <td align="left" width="100%">
                            <asp:TextBox ID="txtLeasingvertragsnummer" runat="server" CssClass="TextBoxNormal"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tr_Kennzeichen" class="StandardTableAlternate">
                        <td align="left">
                            <asp:Label ID="lbl_Kennzeichen" runat="server"></asp:Label>
                        </td>
                        <td align="left" width="100%">
                            <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tr_KennzeichenZusatz" runat="server">
                        <td colspan="2">
                            <b>Eingabe mehrerer Kennzeichen oder Vertragsnummern getrennt durch Komma möglich.</b>
                        </td>
                    </tr>
                    <tr id="tr_Suchname" class="TextLarge">
                        <td align="left">
                            <asp:Label ID="lbl_Suchname" runat="server"></asp:Label>
                        </td>
                        <td align="left" width="100%">
                            <asp:TextBox ID="txtSuchname" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tr_Fahrgestellnummer" class="StandardTableAlternate">
                        <td align="left">
                            <asp:Label ID="lbl_Fahrgestellnummer" runat="server"></asp:Label>
                        </td>
                        <td align="left" width="100%">
                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl_Info" CssClass="TextLarge" Text="Alle Eingaben mit mehrfacher Platzhalter-Suche (*) möglich (z.B. 'F*23Z*1*')"
                                runat="server">Alle Eingaben mit mehrfacher Platzhalter-Suche (*) möglich (z.B. 'F*23Z*1*'). Ausnahme: Mehrfachauswahl.</asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr>
            <td valign="top">
                &nbsp;
            </td>
            <td align="left" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td valign="top">
                &nbsp;
            </td>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
   
    
</body>
</html>
