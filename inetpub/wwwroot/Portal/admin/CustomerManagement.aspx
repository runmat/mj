<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CustomerManagement.aspx.vb"
    Inherits="CKG.Admin.CustomerManagement" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright" />
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
    <meta content="JavaScript" name="vs_defaultClientScript"/>
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <script type="text/javascript" language="javascript" id="ScrollPosition">
<!--

        function sstchur_SmartScroller_GetCoords() {

            var scrollX, scrollY;

            if (document.all) {
                if (!document.documentElement.scrollLeft)
                    scrollX = document.body.scrollLeft;
                else
                    scrollX = document.documentElement.scrollLeft;

                if (!document.documentElement.scrollTop)
                    scrollY = document.body.scrollTop;
                else
                    scrollY = document.documentElement.scrollTop;
            }
            else {
                scrollX = window.pageXOffset;
                scrollY = window.pageYOffset;
            }



            document.forms["Form1"].xCoordHolder.value = scrollX;
            document.forms["Form1"].yCoordHolder.value = scrollY;

        }

        function sstchur_SmartScroller_Scroll() {


            var x = document.forms["Form1"].xCoordHolder.value;
            var y = document.forms["Form1"].yCoordHolder.value;
            window.scrollTo(x, y);

        }

        window.onload = sstchur_SmartScroller_Scroll;
        window.onscroll = sstchur_SmartScroller_GetCoords;
        window.onkeypress = sstchur_SmartScroller_GetCoords;
        window.onclick = sstchur_SmartScroller_GetCoords;
  
// -->
    </script>

    <input type="hidden" id="xCoordHolder" runat="server" />
    <input type="hidden" id="yCoordHolder" runat="server" />
    <table cellspacing="0" cellpadding="0" width="100%" align="center">
        <tbody>
            <tr>
                <td height="18">
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="PageNavigation" colspan="2">
                                    &nbsp;Administration (Kundenverwaltung)
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="120" height="25">
                                    <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                        border="0">
                                        <tr>
                                            <td class="TaskTitle"" width="150">
                                            &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="center" width="150">
                                                <asp:LinkButton ID="lbtnNew" runat="server" CssClass="StandardButton">&#149;&nbsp;Neuen Kunden anlegen</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="center" width="150">
                                                <asp:LinkButton ID="lbtnSave" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Speichern</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="center" width="150">
                                                <asp:LinkButton ID="lbtnConfirm" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Bestätigen</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="center" width="150">
                                                <asp:LinkButton ID="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Verwerfen</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="center" width="150">
                                                <asp:LinkButton ID="lbtnDelete" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Löschen</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                                        <tbody>
                                            <tr>
                                                <td class="TaskTitle">
                                                    <asp:HyperLink ID="lnkUserManagement" runat="server" CssClass="TaskTitle" NavigateUrl="UserManagement.aspx">Benutzerverwaltung</asp:HyperLink><asp:HyperLink
                                                        ID="lnkGroupManagement" runat="server" CssClass="TaskTitle" NavigateUrl="GroupManagement.aspx">Gruppenverwaltung</asp:HyperLink><asp:HyperLink
                                                            ID="lnkOrganizationManagement" runat="server" CssClass="TaskTitle" NavigateUrl="OrganizationManagement.aspx">Organisationsverwaltung</asp:HyperLink>
                                                    <asp:HyperLink ID="lnkAppManagement" runat="server" CssClass="TaskTitle" NavigateUrl="AppManagement.aspx"
                                                        Visible="False">Anwendungsverwaltung</asp:HyperLink><asp:HyperLink ID="lnkArchivManagement"
                                                            runat="server" CssClass="TaskTitle" NavigateUrl="ArchivManagement.aspx">Archivverwaltung</asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trSearch" runat="server">
                                                <td align="left">
                                                    <table bgcolor="white" border="0">
                                                        <tr>
                                                            <td valign="bottom" width="100">
                                                                Firma:
                                                            </td>
                                                            <td valign="bottom">
                                                                <asp:TextBox ID="txtFilterCustomerName" runat="server" Width="160px" Height="20px">*</asp:TextBox>
                                                            </td>
                                                            <td valign="bottom">
                                                                <asp:Button ID="btnSuche" runat="server" CssClass="StandardButton" Text="Suchen">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trSearchSpacer" runat="server">
                                                <td align="left" height="25">
                                                </td>
                                            </tr>
                                            <tr id="trSearchResult" runat="server">
                                                <td align="left">
                                                    <asp:DataGrid ID="dgSearchResult" runat="server" Width="100%" BackColor="White" AllowPaging="True"
                                                        AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="1px">
                                                        <SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
                                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top">
                                                        </HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundColumn Visible="False" DataField="CustomerID" SortExpression="CustomerID"
                                                                HeaderText="CustomerID"></asp:BoundColumn>
                                                            <asp:ButtonColumn DataTextField="CustomerName" SortExpression="CustomerName" HeaderText="Kunde"
                                                                CommandName="Edit"></asp:ButtonColumn>
                                                            <asp:BoundColumn DataField="KUNNR" SortExpression="KUNNR" HeaderText="KUNNR"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="AccountingArea" SortExpression="AccountingArea" HeaderText="Buchungs-<br>kreis">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CName" SortExpression="CName" HeaderText="Kontakt-Name">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CAddress" SortExpression="CAddress" HeaderText="Kontakt-Adresse">
                                                            </asp:BoundColumn>
                                                            <asp:HyperLinkColumn DataNavigateUrlField="CMail" DataTextField="CMailDisplay" SortExpression="CMailDisplay"
                                                                HeaderText="Mail-Adresse"></asp:HyperLinkColumn>
                                                            <asp:HyperLinkColumn DataNavigateUrlField="CWeb" DataTextField="CWebDisplay" SortExpression="CWebDisplay"
                                                                HeaderText="Web-Adresse"></asp:HyperLinkColumn>
                                                            <asp:BoundColumn DataField="CountUsers" SortExpression="CountUsers" HeaderText="Benutzer-&lt;br&gt;anzahl">
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="l&#246;schen">
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtnSRDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                                        ImageUrl="../Images/icon_nein_s.gif"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                            <tr id="trConfirm" runat="server">
                                                <td class="InfoBoxFlat">
                                                    <asp:PlaceHolder ID="plhConfirm" runat="server"></asp:PlaceHolder>
                                                </td>
                                            </tr>
                                            <tr id="trEditUser" runat="server">
                                                <td align="left">
                                                    <table width="100%" border="0">
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <table id="tblLeft" cellspacing="2" cellpadding="0" width="345" bgcolor="white" border="0">
                                                                    <tr>
                                                                        <td width="250" height="22">
                                                                            Firmenname:<asp:TextBox ID="txtCustomerID" runat="server" Visible="False" Width="0px"
                                                                                Height="0px" BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                        </td>
                                                                        <td align="right" width="160" height="22">
                                                                            <asp:TextBox ID="txtCustomerName" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="250" height="22">
                                                                            KUNNR
                                                                        </td>
                                                                        <td align="right" width="160" height="22">
                                                                            <asp:TextBox ID="txtKUNNR" runat="server" Width="160px" Height="20px">0</asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trMaster" runat="server">
                                                                        <td width="250" height="22">
                                                                            Master
                                                                        </td>
                                                                        <td align="left" width="160" height="22">
                                                                            <asp:CheckBox ID="cbxMaster" runat="server" Width="160px"></asp:CheckBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trAccountingArea" runat="server">
                                                                        <td width="250px" height="22px">
                                                                            Buchungskreis
                                                                        </td>
                                                                        <td align="left" width="160px" height="22px">
                                                                            <asp:DropDownList ID="ddlAccountingArea" runat="server" Width="160px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trPortalLink" runat="server">
                                                                        <td width="250px" height="22px">
                                                                            Portallink:
                                                                        </td>
                                                                        <td align="left" width="160px" height="22px">
                                                                            <asp:DropDownList ID="ddlPortalLink" runat="server" width="160px" />
                                                                        </td>
                                                                    </tr>
                                                                     <tr id="tr1" runat="server">
                                                                        <td width="250" height="22">
                                                                            Kunde sperren:
                                                                        </td>
                                                                        <td align="left" width="160" height="22">
                                                                            <asp:CheckBox ID="chkKundenSperre" runat="server" Width="160px"></asp:CheckBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trTeamViewer" runat="server">
                                                                        <td width="250" height="22">
                                                                            TeamViewer verwenden:
                                                                        </td>
                                                                        <td align="left" width="160" height="22">
                                                                            <span>
                                                                                <asp:CheckBox ID="chkTeamviewer" runat="server" />
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trLoginRules" runat="server">
                                                                        <td class="InfoBoxFlat" align="left" colspan="2">
                                                                            <table id="tblLoginRules" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td align="middle" colspan="3" height="22" rowspan="1">
                                                                                        Login-Regeln
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Neues Passwort&nbsp;nach n Tagen:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtNewPwdAfterNDays" runat="server" Width="160px" Height="20px">60</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Konto sperren nach n Fehlversuchen:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtLockedAfterNLogins" runat="server" Width="160px" Height="20px">3</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Mehrfaches Login erlaubt:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:CheckBox ID="chkAllowMultipleLogin" runat="server" Checked="True"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trPwdRules" runat="server">
                                                                        <td class="InfoBoxFlat" align="left" colspan="2">
                                                                            <table id="tblPwdRules" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td align="middle" colspan="2" height="22">
                                                                                        Passwort-Regeln
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Mindestlänge:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtPwdLength" runat="server" Width="160px" Height="20px">8</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        n numerische Zeichen:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtPwdNNumeric" runat="server" Width="160px" Height="20px">1</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        n Großbuchstaben:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtNCapitalLetter" runat="server" Width="160px" Height="20px">1</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="21">
                                                                                        n Sonderzeichen:
                                                                                    </td>
                                                                                    <td align="right" height="21">
                                                                                        <asp:TextBox ID="txtNSpecialCharacter" runat="server" Width="160px" Height="20px">1</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="trPwdHistoryNEntries" runat="server">
                                                                                    <td height="22">
                                                                                        Sperre letze n Passwörter:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtPwdHistoryNEntries" runat="server" Width="160px" Height="20px">6</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Passwort nicht per Email:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:CheckBox ID="cbxPwdDontSendEmail" runat="server" AutoPostBack="true"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Username nicht per Email:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:CheckBox ID="cbxUsernameSendEmail" runat="server" Enabled="true"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Geheime Frage<br />
                                                                                        (Passwort per Email):
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:CheckBox ID="cbxForcePasswordQuestion" runat="server"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Namensangaben keine Pflichtfelder:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:CheckBox ID="cbxNameInputOptional" runat="server"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trCustomerUser" runat="server">
                                                                        <td class="InfoBoxFlat" align="left" colspan="2">
                                                                            <table id="tblStyle" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td align="middle" colspan="2" height="22">
                                                                                        Benutzer
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Max. Anzahl&nbsp;Benutzer:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtMaxUser" runat="server" Width="160px" Height="20px">600</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                <td height="22">
                                                                                       Kundenadministration
                                                                                       </td>
                                                                                <td >
                                                                               <asp:RadioButtonList AutoPostBack="true" runat="server" ID="rblKundenAdministration">
                                                                               <asp:ListItem Text="keine" Value="0"></asp:ListItem>
                                                                               <asp:ListItem Text="vollständig" Value="2"></asp:ListItem>
                                                                               <asp:ListItem Text="eingeschränkt" Value="1"></asp:ListItem>
                                                                               </asp:RadioButtonList>
                                                                               </td>
                                                                               
                                                                                
                                                                               
                                                                                </tr>
                                                                                <tr id="trKundenadministrationInfo" runat="server">
                                                                                    <td height="22">
                                                                                       Beschreibung eingeschränkte Administration
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtKundenadministrationBeschreibung" runat="server"  TextMode="MultiLine" Rows="3" Width="160px" ></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="vertical-align: top;" runat="server">
                                                        Autom. Benutzersperrung nach:
                                                    </td>
                                                    <td class="active" style="vertical-align: top;">
                                                        <asp:TextBox ID="txtUserLockTime" runat="server" MaxLength="4" Width="40px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="ftbeUserLockTime" runat="server" TargetControlID="txtUserLockTime"
                                                            FilterType="Numbers" Enabled="True">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                        &nbsp;Tagen
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="vertical-align: top;" runat="server">
                                                        Autom. Benutzer löschen
                                                        <br />
                                                        nach Sperrung:
                                                    </td>
                                                    <td class="active" style="vertical-align: top;">
                                                        <asp:TextBox ID="txtUserDeleteTime" runat="server" MaxLength="4" Width="40px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="ftbeUserDeleteTime" runat="server" TargetControlID="txtUserDeleteTime"
                                                            FilterType="Numbers" Enabled="True">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                        &nbsp;Tagen
                                                    </td>
                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trShowOrganization" runat="server">
                                                                        <td class="InfoBoxFlat" align="left" colspan="2">
                                                                            <table id="tblStyle2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td align="middle" colspan="2" height="22">
                                                                                        Organisation
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Organisationsanzeige ein:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:CheckBox ID="chkShowOrganization" runat="server"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Organisatios-Admin auf Kundengruppe beschränken
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:CheckBox ID="cbxOrgAdminRestrictToCustomerGroup" runat="server"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Distriktzuordnung
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:CheckBox ID="chkShowDistrikte" runat="server"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td valign="top" align="left" width="100%">
                                                                <table id="tblRight" cellspacing="2" cellpadding="0" width="345" bgcolor="white"
                                                                    border="0">
                                                                    <tr>
                                                                        <td height="22">
                                                                            Kontakt-Name:
                                                                        </td>
                                                                        <td align="right" height="22">
                                                                            <asp:TextBox ID="txtCName" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" height="22">
                                                                            Kontakt-Adresse:<br />
                                                                            <asp:Label runat="server">( {} entspricht <> )</asp:Label>
                                                                        </td>
                                                                        <td align="right" height="22">
                                                                            <asp:TextBox ID="txtCAddress" runat="server" Width="160px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="22">
                                                                            Mailadresse Anzeigetext:
                                                                        </td>
                                                                        <td align="right" height="22">
                                                                            <asp:TextBox ID="txtCMailDisplay" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="22">
                                                                            Mailadresse:
                                                                        </td>
                                                                        <td align="right" height="22">
                                                                            <asp:TextBox ID="txtCMail" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                    <td colspan="2">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                    <td colspan="2" style="text-align:center;">Kundenkontakt</td>
                                                                    </tr>
                                                                    <tr style="background-color:#dddddd;">
                                                                        <td height="22">
                                                                            Kundenpostfach:</td>
                                                                        <td align="right" height="22">
                                                                            <asp:TextBox ID="txtKundenpostfach" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>                                                                    
                                                                    <tr style="background-color:#dddddd;">
                                                                        <td height="22">
                                                                            Kundenhotline:</td>
                                                                        <td align="right" height="22">
                                                                            <asp:TextBox ID="txtKundenhotline" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color:#dddddd;">
                                                                        <td height="22">
                                                                            Kundenfax:</td>
                                                                        <td align="right" height="22">
                                                                            <asp:TextBox ID="txtKundenfax" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                    <td colspan="2">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="22">
                                                                            Web-Adresse Anzeigetext:
                                                                        </td>
                                                                        <td align="right" height="22">
                                                                            <asp:TextBox ID="txtCWebDisplay" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="22">
                                                                            Web-Adresse:
                                                                        </td>
                                                                        <td align="right" height="22">
                                                                            <asp:TextBox ID="txtCWeb" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trStyle" runat="server">
                                                                        <td class="InfoBoxFlat" align="left" colspan="2">
                                                                            <table id="tblStyle" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td align="middle" colspan="2" height="22">
                                                                                        Style
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Pfad zum Logo:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtLogoPath" runat="server" Width="160px" Height="20px">../Images/Logo.gif</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Pfad zum Logo rechts:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtLogoPath2" runat="server" Width="160px" Height="20px">../Images/Logo2.gif</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Pfad zu den Stylesheets:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtCssPath" runat="server" Width="160px" Height="20px">Styles.css</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Pfad zum Handbuch:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtDocuPath" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trIP" runat="server">
                                                                        <td class="InfoBoxFlat" align="left" colspan="2">
                                                                            <table id="tblIpAddresses" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td align="middle" colspan="2" height="22">
                                                                                        Beschränkungen auf IP-Adressen
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Adress-Kontrolle:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:CheckBox ID="chkIpRestriction" runat="server"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="22">
                                                                                        Standard-User-Name:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:TextBox ID="txtIpStandardUser" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td valign="top" height="22">
                                                                                        Liste gültiger Adressen:
                                                                                    </td>
                                                                                    <td align="right" height="22">
                                                                                        <asp:DataGrid ID="grdIpAddresses" runat="server" Width="160px" AutoGenerateColumns="False"
                                                                                            ShowHeader="False">
                                                                                            <Columns>
                                                                                                <asp:BoundColumn DataField="IpAddress" SortExpression="IpAddress" HeaderText="IP-Adresse">
                                                                                                    <HeaderStyle Width="100px"></HeaderStyle>
                                                                                                </asp:BoundColumn>
                                                                                                <asp:ButtonColumn Text="L&#246;schen" CommandName="Delete">
                                                                                                    <HeaderStyle Width="60px"></HeaderStyle>
                                                                                                </asp:ButtonColumn>
                                                                                            </Columns>
                                                                                        </asp:DataGrid>
                                                                                        <table cellspacing="0" cellpadding="0" width="160" border="0">
                                                                                            <tr>
                                                                                                <td width="100">
                                                                                                    <asp:TextBox ID="txtIpAddress" runat="server" Width="98px"></asp:TextBox>
                                                                                                </td>
                                                                                                <td width="60">
                                                                                                    <asp:LinkButton ID="btnNewIpAddress" runat="server" Width="58px">Hinzufügen</asp:LinkButton>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblDown" cellspacing="2" cellpadding="0" width="100%" border="0">
                                                        <tr id="trApp" runat="server">
                                                            <td class="InfoBoxFlat" valign="top" align="left" colspan="3">
                                                                <table id="tblApp" cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                                                    <tr>
                                                                        <td align="left" colspan="3" height="22">
                                                                            <u>Anwendungen</u>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" nowrap width="112" height="22">
                                                                            <p>
                                                                                nicht zugewiesen</p>
                                                                        </td>
                                                                        <td valign="top" align="left" width="37" height="22">
                                                                            <p>
                                                                                <asp:Button ID="btnAssign" runat="server" CssClass="StandardButton" Width="30px"
                                                                                    Text="+ >"></asp:Button></p>
                                                                            <p>
                                                                                &nbsp;</p>
                                                                        </td>
                                                                        <td valign="top" align="left" width="100%" height="22">
                                                                            <p>
                                                                                <asp:ListBox ID="lstAppUnAssigned" runat="server" Width="100%" Height="100px" SelectionMode="Multiple">
                                                                                </asp:ListBox>
                                                                            </p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" nowrap width="112" height="22">
                                                                            zugewiesen
                                                                        </td>
                                                                        <td valign="top" align="left" width="37" height="22">
                                                                            <p>
                                                                                <asp:Button ID="btnUnAssign" runat="server" CssClass="StandardButton" Width="30px"
                                                                                    Text="< -"></asp:Button></p>
                                                                        </td>
                                                                        <td valign="top" align="left" width="100%" height="22">
                                                                            <asp:ListBox ID="lstAppAssigned" runat="server" Width="100%" Height="100px" SelectionMode="Multiple">
                                                                            </asp:ListBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3" height="22">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trArchiv" runat="server">
                                                            <td class="InfoBoxFlat" valign="top" align="left" colspan="3">
                                                                <table id="tblArchiv" cellspacing="0" cellpadding="0" width="100%" bgcolor="white"
                                                                    border="0">
                                                                    <tr>
                                                                        <td align="left" colspan="3" height="22">
                                                                            <u>Archive</u>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" nowrap width="112" height="22">
                                                                            <p>
                                                                                nicht zugewiesen</p>
                                                                        </td>
                                                                        <td valign="top" align="left" width="37" height="22">
                                                                            <p>
                                                                                <asp:Button ID="btnAssignArchiv" runat="server" CssClass="StandardButton" Width="30px"
                                                                                    Text="+ >"></asp:Button></p>
                                                                            <p>
                                                                                &nbsp;</p>
                                                                        </td>
                                                                        <td valign="top" align="left" width="100%" height="22">
                                                                            <p>
                                                                                <asp:ListBox ID="lstArchivUnAssigned" runat="server" Width="100%" Height="100px"
                                                                                    SelectionMode="Multiple"></asp:ListBox>
                                                                            </p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" nowrap width="112" height="22">
                                                                            zugewiesen
                                                                        </td>
                                                                        <td valign="top" align="left" width="37" height="22">
                                                                            <p>
                                                                                <asp:Button ID="btnUnAssignArchiv" runat="server" CssClass="StandardButton" Width="30px"
                                                                                    Text="< -"></asp:Button></p>
                                                                        </td>
                                                                        <td valign="top" align="left" width="100%" height="22">
                                                                            <asp:ListBox ID="lstArchivAssigned" runat="server" Width="100%" Height="100px" SelectionMode="Multiple">
                                                                            </asp:ListBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3" height="22">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" height="25">
                                                </td>
                                            </tr>
                                            <tr id="trKundenInfo" runat="server">
                                              
                                                <td class="InfoBoxFlat">
                                                    <table runat="server" width="100%" id="tblKundenInfo">
                                                        <tr>
                                                            <td align="left">
                                                                <u>Eintragen</u>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table id="tblVersandanschrift" runat="server" cellspacing="0" cellpadding="5" width="50%"
                                                                    border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lbl_Name" Text="Name" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td align="left" width="50%">
                                                                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lbl_Telefon" Text="Telefon" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td align="left" width="50%">
                                                                            <asp:TextBox ID="txtTelefon" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lbl_Vorname" Text="Vorname" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td align="left" width="50%">
                                                                            <asp:TextBox ID="txtVorname" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lbl_Telefax" Text="Telefax" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td align="left" width="50%">
                                                                            <asp:TextBox ID="txtTelefax" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lbl_Email" Text="Email" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td align="left" colspan="2">
                                                                            <asp:TextBox ID="txtEmail" runat="server" Height="22px" Width="200px"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3" nowrap="nowrap" width="100%">
                                                                            <asp:RadioButtonList ID="rblPersonType" RepeatDirection="Horizontal" runat="server">
                                                                                <asp:ListItem Value="Businessowner">Businesowner</asp:ListItem>
                                                                                <asp:ListItem Value="Adminperson">Admin Person</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:LinkButton ID="lbEintragen"  runat="server" Text="eintragen"> </asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                              <u>Businessowner</u>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="gvBusinessOwner" Width="100%" runat="server" BackColor="White"
                                                                    AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black"
                                                                    BorderStyle="Solid">
                                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" Visible="false" />
                                                                        <asp:TemplateField HeaderText="Löschen" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton Visible="true" ID="lbEntfernen" CommandName="entfernen" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>'
                                                                                    runat="server"><img src="../../Images/loesch.gif" border="0"> </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                                                        <asp:BoundField DataField="Vorname" HeaderText="Vorname" SortExpression="Vorname" />
                                                                        <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" />
                                                                        <asp:BoundField DataField="Telefon" HeaderText="Telefon" SortExpression="Telefon" />
                                                                        <asp:BoundField DataField="Telefax" HeaderText="Telefax" SortExpression="Telefax" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                        <td>
                                                        &nbsp;
                                                        </td>
                                                        </tr>
                                                          <tr>
                                                            <td align="left">
                                                              <u>Administrationsberechtigte Person</u>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="gvAdminPerson" Width="100%" runat="server" BackColor="White"
                                                                    AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black"
                                                                    BorderStyle="Solid">
                                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" Visible="false" />
                                                                        <asp:TemplateField HeaderText="Löschen" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton Visible="true" ID="lbEntfernen" CommandName="entfernen" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>'
                                                                                    runat="server"><img src="../../Images/loesch.gif" border="0"> </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                                                        <asp:BoundField DataField="Vorname" HeaderText="Vorname" SortExpression="Vorname" />
                                                                        <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" />
                                                                        <asp:BoundField DataField="Telefon" HeaderText="Telefon" SortExpression="Telefon" />
                                                                        <asp:BoundField DataField="Telefax" HeaderText="Telefax" SortExpression="Telefax" />
                                                                    </Columns>
                                                                </asp:GridView>
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
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server" CssClass="TextLarge" EnableViewState="False"></asp:Label><asp:Label
                                        ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <!--#include File="../PageElements/Footer.html" -->
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
