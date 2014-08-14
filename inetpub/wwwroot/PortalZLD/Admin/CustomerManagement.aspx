<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CustomerManagement.aspx.vb"
    Inherits="Admin.CustomerManagement" EnableEventValidation="false" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="atkHtmlEdit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
    
        var HighlightAnimations = {};
        function Highlight(el) {
            if (HighlightAnimations[el.uniqueID] == null) {
                HighlightAnimations[el.uniqueID] = AjaxControlToolkit.Animation.createAnimation({
                    AnimationName: "color",
                    duration: 0.5,
                    property: "style",
                    propertyKey: "backgroundColor",
                    startValue: "#FFFF90",
                    endValue: "#FFFFFF"
                }, el);
            }
            HighlightAnimations[el.uniqueID].stop();
            HighlightAnimations[el.uniqueID].play();
        }

    </script>

    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkUserManagement" runat="server" ToolTip="Benutzer" NavigateUrl="UserManagement.aspx"
                            Text="Benutzer | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkGroupManagement" runat="server" ToolTip="Gruppen" NavigateUrl="GroupManagement.aspx"
                            Text="Gruppen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkAppManagement" runat="server" ToolTip="Anwendungen" NavigateUrl="AppManagement.aspx"
                            Text="Anwendungen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkArchivManagement" ToolTip="Archive" runat="server" NavigateUrl="ArchivManagement.aspx"
                            Text="Archive | "></asp:HyperLink>
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                         <asp:Panel ID="DivSearch1" runat="server" DefaultButton="btnEmpty">
                            <div id="TableQuery">
                                <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                &nbsp;
                                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Firma:
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap" width="100%">
                                                <asp:TextBox ID="txtFilterCustomerName" runat="server" CssClass="InputTextbox">*</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtEmpty" runat="server" CssClass="InputTextbox" Width="160px" Visible="False">*</asp:TextBox>
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="/PortalZLD/images/empty.gif"
                                                    Width="1px" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                            </td>
                                        </tr>
                                        <tr style="background-color: #dfdfdf; height: 22px">
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div id="QueryFooter" runat="server">
                                    <div id="dataQueryFooter">
                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton class="Tablebutton" ID="btnSuche" runat="server"
                                            Text="Suchen&amp;nbsp;&amp;#187; " CssClass="TablebuttonXLarge" Height="16px"
                                            Width="155px" Font-Names="Verdana,sans-serif" Font-Size="10px"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="Neuer Kunde&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="16px" Width="155px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="Result" runat="Server" visible="false">
                            <div class="ExcelDiv">
                                <div align="right" id="trSearchSpacer" runat="server">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server" />
                            </div>
                            <div id="data">
                             <asp:Panel ID="Panel1" runat="server">
                                <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" width="100%"
                                    align="left" border="0">
                                    <tbody>
                                        <tr id="trSearchResult" runat="server">
                                            <td align="left">
                                                <asp:GridView ID="dgSearchResult" Width="1100px" runat="server" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                            AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                            <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EmptyDataRowStyle BackColor="#DFDFDF" />
                                                    <Columns>
                                                    
                                                       <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerID") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:ButtonField CommandName="Del" HeaderStyle-ForeColor="White"  HeaderText="Löschen" ButtonType="Image" ImageUrl="/PortalZLD/Images/Papierkorb_01.gif" ControlStyle-Height="16px" ControlStyle-Width="16px">
                                                        </asp:ButtonField>                                                    
                                                        <asp:ButtonField DataTextField="CustomerName" ItemStyle-Font-Underline="true" ItemStyle-ForeColor="#595959" SortExpression="CustomerName"  HeaderText="Kunde"
                                                            CommandName="Edit" ></asp:ButtonField>
                                                        <asp:BoundField DataField="KUNNR" SortExpression="KUNNR" HeaderText="KUNNR"></asp:BoundField>
                                                        <asp:BoundField DataField="AccountingArea" SortExpression="AccountingArea" HeaderText="Buchungs- kreis" >
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CName" SortExpression="CName" HeaderText="Kontakt-Name">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CAddress" SortExpression="CAddress" HeaderText="Kontakt- Adresse">
                                                        </asp:BoundField>
                                                          <asp:TemplateField HeaderText="Mail-Adresse" HeaderStyle-ForeColor="White">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="HyperLink1" ItemStyle-Font-Underline="true" ItemStyle-ForeColor="#595959" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.CMail") %>' Text='<%# DataBinder.Eval(Container, "DataItem.CMailDisplay") %>' runat="server"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:HyperLinkField  DataNavigateUrlFields="CWeb" ItemStyle-Font-Underline="true" ItemStyle-ForeColor="#595959"  DataTextField="CWebDisplay" SortExpression="CWebDisplay"
                                                            HeaderText="Web-Adresse" ></asp:HyperLinkField>
                                                        <asp:BoundField DataField="CountUsers" SortExpression="CountUsers" HeaderText="Benutzeranzahl" >
                                                        </asp:BoundField>
                                                      </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr id="trConfirm" runat="server">
                                            <td>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                             </asp:Panel>
                            </div>
                            <div class="dataFooter">
                                &nbsp;
                            </div>
                        </div>
                        <div id="Input" runat="server" visible="False">
                            <div id="adminInput" style="padding-top: 10px">
                            <asp:PlaceHolder ID="plhConfirm" runat="server" ></asp:PlaceHolder>
                                <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="1"
                                    Width="100%" CssClass="ajax__tab_xp" BorderStyle="None"> <!--Height="500px"  -->
                                    <ajaxToolkit:TabPanel runat="server" ID="KunnDat" HeaderText="Kunden-Daten">
                                        <ContentTemplate>
                                            <table id="tblKunDaten" style="border-color: #FFFFFF; font-size: 10px;" cellspacing="0"
                                                cellpadding="0" width="100%" border="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Firmenname:<asp:TextBox ID="txtCustomerID" runat="server" Visible="False" Width="0px"
                                                            Height="0px" BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                    </td>
                                                    <td class="active" width="100%">
                                                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        KUNNR
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtKUNNR" runat="server" CssClass="InputTextbox">0</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trMaster" runat="server">
                                                    <td class="firstLeft active">
                                                        Master
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="cbxMaster" runat="server"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trAccountingArea" runat="server">
                                                    <td class="firstLeft active">
                                                        Buchungskreis
                                                    </td>
                                                    <td class="active">
                                                        <asp:DropDownList ID="ddlAccountingArea" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="tr1" runat="server">
                                                    <td class="firstLeft active">
                                                        Portallink:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:DropDownList ID="ddlPortalLink" runat="server" />
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trKundeSperr" runat="server">
                                                    <td class="firstLeft active">
                                                        Kunde sperren:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="chkKundenSperre" runat="server" /></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trTeamViewer" runat="server">
                                                    <td class="firstLeft active">
                                                        TeamViewer verwenden:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="chkTeamviewer" runat="server" Enabled="false" Checked="false"/>
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Kontakt-Name:
                                                    </td>
                                                    <td align="left" class="active">
                                                        <asp:TextBox ID="txtCName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="vertical-align: top">
                                                        Kontakt-Adresse:
                                                    </td>
                                                    <td class="active">
                                                        <!--<asp:TextBox ID="txtCAddress" runat="server" TextMode="MultiLine" CssClass="InputTextbox"></asp:TextBox>-->
                                                        <atkHtmlEdit:Editor ID="EditCAddress" runat="server" Width="680px"></atkHtmlEdit:Editor>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Mailadresse Anzeigetext:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtCMailDisplay" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Mailadresse:
                                                    </td>
                                                    <td class=" active">
                                                        <asp:TextBox ID="txtCMail" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td nowrap="nowrap" class="firstLeft active">
                                                        Web-Adresse Anzeigetext:
                                                    </td>
                                                    <td align="left" class="active">
                                                        <asp:TextBox ID="txtCWebDisplay" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td height="22" class="firstLeft active">
                                                        Web-Adresse:
                                                    </td>
                                                    <td align="left" class=" active">
                                                        <asp:TextBox ID="txtCWeb" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>    
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td nowrap="nowrap" colspan="2" class="firstLeft active" style="border-left: solid 1px #595959;
                                                        border-right: solid 1px #595959; border-top: solid 1px #595959; background-color: #bbbbbb;
                                                        color: White; padding-bottom: 5px;">
                                                        Kundenkontakt
                                                    </td>
                                                </tr>                                            
                                                <tr class="formquery">
                                                    <td nowrap="nowrap" class="firstLeft active"  style="border-left: solid 1px #595959;">
                                                        Kundenpostfach:
                                                    </td>
                                                    <td align="left" class="active" style="border-right:solid 1px #595959;">
                                                        <asp:TextBox ID="txtKundepostfach" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" >
                                                    <td nowrap="nowrap" class="firstLeft active" style="border-left:solid 1px #595959;">
                                                        Kundenhotline:
                                                    </td>
                                                    <td align="left" class="active" style="border-right:solid 1px #595959;">
                                                        <asp:TextBox ID="txtKundenhotline" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr> 
                                                <tr class="formquery" >
                                                    <td nowrap="nowrap" class="firstLeft active" style="border-left:solid 1px #595959;border-bottom:solid 1px #595959;">
                                                        Kundenfax:
                                                    </td>
                                                    <td align="left" class="active" style="border-right:solid 1px #595959;border-bottom:solid 1px #595959;padding-bottom:5px;">
                                                        <asp:TextBox ID="txtKundenfax" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>                                                 
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel runat="server" ID="LoginRules" HeaderText="Login/Passwort-Regen">
                                        <ContentTemplate>
                                            <table id="Table3" style="border-color: #FFFFFF; font-size: 10px;" cellspacing="0"
                                                cellpadding="0" width="100%" border="0">
                                                <tr class="formquery">
                                                    <td colspan="2">
                                                        <div class="formqueryHeader">
                                                            <span>Login-Regeln</span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Neues Kennwort&nbsp;nach n Tagen:
                                                    </td>
                                                    <td class="active" width="100%">
                                                        <asp:TextBox ID="txtNewPwdAfterNDays" runat="server" CssClass="InputTextbox">60</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Konto sperren nach n Fehlversuchen:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtLockedAfterNLogins" runat="server" CssClass="InputTextbox">3</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Mehrfaches Login erlaubt:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="chkAllowMultipleLogin" runat="server" Checked="True"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Remote Login über URL erlaubt:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="chkAllowUrlRemoteLogin" runat="server" Checked="False"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td colspan="2">
                                                        <div class="formqueryHeader">
                                                            <span>Kennwort-Regeln</span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Mindestlänge:
                                                    </td>
                                                    <td class="cellBorderGray active">
                                                        <asp:TextBox ID="txtPwdLength" runat="server" CssClass="InputTextbox">8</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        n numerische Zeichen:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtPwdNNumeric" runat="server" CssClass="InputTextbox">1</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        n Großbuchstaben:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtNCapitalLetter" runat="server" CssClass="InputTextbox">1</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        n Sonderzeichen:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtNSpecialCharacter" runat="server" CssClass="InputTextbox">1</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trPwdHistoryNEntries" runat="server">
                                                    <td class="firstLeft active" runat="server">
                                                        Sperre letze n Kennworte:
                                                    </td>
                                                    <td class="active" runat="server">
                                                        <asp:TextBox ID="txtPwdHistoryNEntries" runat="server" CssClass="InputTextbox">6</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Kennwort nicht per Email:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="cbxPwdDontSendEmail" runat="server" AutoPostBack="True"></asp:CheckBox>
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Username nicht per Email:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="cbxUsernameSendEmail" runat="server"></asp:CheckBox>
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Geheime Frage(Kennwort per Email):
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="cbxForcePasswordQuestion" runat="server"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Namensangaben keine Pflichtfelder:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="cbxNameInputOptional" runat="server"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel runat="server" ID="Benutzer" HeaderText="Benutzer/Organisation">
                                        <HeaderTemplate>
                                            Benutzer/Organisation
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table id="tblStyle1" style="border-color: #FFFFFF; font-size: 10px;" cellspacing="0"
                                                cellpadding="0" width="100%" border="0">
                                                <tr class="formquery">
                                                    <td colspan="2">
                                                        <div class="formqueryHeader">
                                                            <span>Benutzer</span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Max. Anzahl&nbsp;Benutzer:
                                                    </td>
                                                    <td class="active" width="100%">
                                                        <asp:TextBox ID="txtMaxUser" runat="server" Width="50px" CssClass="InputTextbox">600</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="vertical-align: top">
                                                        Kundenadministration:
                                                    </td>
                                                    <td style="vertical-align: top" align="left">
                                                        <asp:UpdatePanel ID="rbUpdate" runat="server">
                                                            <ContentTemplate>
                                                                <asp:RadioButton ID="rbKeine" Text="keine" runat="server" AutoPostBack="True" GroupName="Admin"
                                                                    Width="15px" />
                                                                <br />
                                                                <asp:RadioButton ID="rbvollst" Text="vollständig" runat="server" GroupName="Admin"
                                                                    AutoPostBack="True" Width="15px" />
                                                                <br />
                                                                <asp:RadioButton ID="rbeing" Text="eingeschränkt" runat="server" GroupName="Admin"
                                                                    AutoPostBack="True" Width="15px" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trKundenadministrationInfo" runat="server">
                                                    <td class="firstLeft active" style="vertical-align: top;" runat="server">
                                                        Beschreibung eingeschränkte<br />Administration:
                                                    </td>
                                                    <td class="active" runat="server">
                                                        <asp:TextBox ID="txtKundenadministrationBeschreibung" runat="server" TextMode="MultiLine"
                                                            Rows="3"></asp:TextBox>
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
                                                <tr class="formquery">
                                                    <td colspan="2">
                                                        <div class="formqueryHeader">
                                                            <span>Organisation</span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Organisationsanzeige ein:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="chkShowOrganization" runat="server"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Organisations-Admin auf
                                                        <br />
                                                        Kundengruppe beschränken:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="cbxOrgAdminRestrictToCustomerGroup" runat="server"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Distriktzuordnung:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="chkShowDistrikte" runat="server"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel runat="server" ID="IP_Style" HeaderText="IP-Adressen/Style">
                                        <ContentTemplate>
                                            <asp:UpdatePanel ID="IPUpdate" runat="server">
                                                <ContentTemplate>
                                                    <table id="tblIpAddresses" style="border-color: #FFFFFF; font-size: 10px;" cellspacing="0"
                                                        cellpadding="0" width="100%" border="0">
                                                        <tr class="formquery">
                                                            <td colspan="2">
                                                                <div class="formqueryHeader">
                                                                    <span>Beschränkungen auf IP-Adressen</span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Adress-Kontrolle:
                                                            </td>
                                                            <td class="active" width="100%">
                                                                <span>
                                                                    <asp:CheckBox ID="chkIpRestriction" runat="server"></asp:CheckBox></span>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Standard-User-Name:
                                                            </td>
                                                            <td class="active">
                                                                <asp:TextBox ID="txtIpStandardUser" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Liste gültiger Adressen:
                                                            </td>
                                                            <td class="active">
                                                                <div style="padding-right: 400px;">
                                                                    <asp:Repeater ID="Repeater1" runat="server">
                                                                        <HeaderTemplate>
                                                                            <div style="background-color: #dfdfdf; height: 10px; width: 100%">
                                                                                &nbsp;</div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table class="" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="padding: 2px">
                                                                                        <img alt="" src="/PortalZLD/Images/arrow.gif" border="0" />
                                                                                    </td>
                                                                                    <td style="padding: 2px">
                                                                                        <b>
                                                                                            <%#DataBinder.Eval(Container.DataItem, "IpAddress")%></b></b>
                                                                                    </td>
                                                                                    <td style="padding: 2px">
                                                                                    </td>
                                                                                    <td style="padding: 2px">
                                                                                        <asp:LinkButton ID="btnDel" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "IpAddress")%>'
                                                                                            runat="server">Löschen</asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                IP-Adresse
                                                            </td>
                                                            <td class="active">
                                                                <asp:TextBox ID="txtIpAddress" runat="server" CssClass="InputTextbox"></asp:TextBox><asp:LinkButton
                                                                    ID="btnNewIpAddress" runat="server" Width="58px">Hinzufügen</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td colspan="2  ">
                                                                <div class="formqueryHeader">
                                                                    <span>Style</span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Pfad zum Logo:
                                                            </td>
                                                            <td class=" active">
                                                                <asp:TextBox ID="txtLogoPath" runat="server" CssClass="InputTextbox">../Images/Logo.gif</asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Pfad zum Logo rechts:
                                                            </td>
                                                            <td class="active">
                                                                <asp:TextBox ID="txtLogoPath2" runat="server" CssClass="InputTextbox">../Images/Logo2.gif</asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Pfad zu den Stylesheets:
                                                            </td>
                                                            <td class=" active">
                                                                <asp:TextBox ID="txtCssPath" runat="server" CssClass="InputTextbox">Styles.css</asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Pfad zum Handbuch:
                                                            </td>
                                                            <td class="active">
                                                                <asp:TextBox ID="txtDocuPath" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                          </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel runat="server" ID="Apps" HeaderText="Anwendungen">
                                        <ContentTemplate>
                                            <table id="tblApp" cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF;
                                                font-size: 10px;" border="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="35%">
                                                        nicht zugewiesen
                                                        <p>
                                                            <asp:ListBox ID="lstAppUnAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                Width="350px" Height="150px"></asp:ListBox>
                                                        </p>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton Style="margin-left: 20px" ID="btnAssign" runat="server" ImageUrl="/PortalZLD/Images/Pfeil_vor_01.jpg"
                                                            ToolTip="Zuweisen" Height="37px" Width="37px" />
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="btnUnAssign" runat="server" ImageUrl="/PortalZLD/Images/Pfeil_zurueck_01.jpg"
                                                            ToolTip="Entfernen" Height="37px" Width="37px" /></span>
                                                    </td>
                                                    <td class="active" width="35%">
                                                        zugewiesen
                                                        <p>
                                                            <asp:ListBox ID="lstAppAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                Width="350px" Height="150"></asp:ListBox>
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel runat="server" ID="TPArchive" HeaderText="Archive">
                                        <ContentTemplate>
                                            <table id="tblArchiv" cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF;
                                                font-size: 10px;" border="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="35%">
                                                        nicht zugewiesen
                                                        <p>
                                                            <asp:ListBox ID="lstArchivUnAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                Width="300px" Height="150px"></asp:ListBox>
                                                        </p>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="btnAssignArchiv" Style="margin-left: 40px" runat="server" ImageUrl="/PortalZLD/Images/Pfeil_vor_01.jpg"
                                                            ToolTip="Zuweisen" Height="37px" Width="37px" />
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="btnUnAssignArchiv" runat="server" ImageUrl="/PortalZLD/Images/Pfeil_zurueck_01.jpg"
                                                            ToolTip="Entfernen" Height="37px" Width="37px" /></span>
                                                    </td>
                                                    <td class="active" width="35%">
                                                        zugewiesen
                                                        <p>
                                                            <asp:ListBox ID="lstArchivAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                Width="300px" Height="150"></asp:ListBox>
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                </ajaxToolkit:TabContainer>
                                <div id="dataFooter">
                                    &nbsp;&nbsp;<asp:LinkButton class="Tablebutton" ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;<asp:LinkButton
                                            class="Tablebutton" ID="lbtnConfirm" runat="server" Text="Bestätigen&amp;nbsp;&amp;#187; "
                                            CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;<asp:LinkButton
                                                class="Tablebutton" ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                                CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;<asp:LinkButton
                                                    class="Tablebutton" ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                                    CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
