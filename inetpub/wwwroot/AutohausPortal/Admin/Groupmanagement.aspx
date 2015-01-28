<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Groupmanagement.aspx.vb"
    Inherits="Admin.Groupmanagement" MasterPageFile="MasterPage/Admin.Master" ViewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkUserManagement" runat="server" ToolTip="Benutzer" NavigateUrl="UserManagement.aspx"
                            Text="Benutzer | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkContactManagement" runat="server" ToolTip="Ansprechpartner" 
                            NavigateUrl="Contact.aspx" Text="Ansprechpartner | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkCustomerManagement" ToolTip="Kunden" CssClass="IMGCust" runat="server"
                            NavigateUrl="CustomerManagement.aspx" Text="Kunden | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkArchivManagement" ToolTip="Archive" runat="server" CssClass="IMGArchiv"
                            NavigateUrl="ArchivManagement.aspx" Text="Archive | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkAppManagement" runat="server" ToolTip="Anwendungen" CssClass="IMGApp"
                            NavigateUrl="AppManagement.aspx" Text="Anwendungen"></asp:HyperLink>
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
                                            <td class="firstLeft active">
                                                Firma:
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap" width="100%">
                                                <asp:DropDownList ID="ddlFilterCustomer" runat="server" Font-Names="Verdana,sans-serif"
                                                    Height="20px" Visible="False" Width="260px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCustomer" runat="server" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trSelectOrganization" runat="server">
                                            <td class="firstLeft active">
                                                Gruppe:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterGroupName" runat="server" CssClass="InputTextbox" Width="257px">*</asp:TextBox>
                                                <asp:Label ID="lblGroupName" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtEmpty" runat="server" CssClass="InputTextbox" Width="160px" Visible="False">*</asp:TextBox>
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="images/empty.gif"
                                                    Width="1px" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                            </td>
                                            <td style="width: 35%">
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td align="right" nowrap="nowrap" class="rightPadding">
                                                &nbsp;
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
                                        &nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="Neue Gruppe&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="16px" Width="155px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="Result" runat="Server" visible="false">
                            <div class="ExcelDiv">
                                <div align="right" id="trSearchSpacer1" runat="server">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <table cellspacing="0" style="border-width: 0px; border-color: #ffffff;" 
                                    cellpadding="0" width="100%"
                                    align="left">
                                    <tbody>
                                        <tr id="trSearchResult" runat="server">
                                            <td align="left">
                                                <asp:GridView ID="dgSearchResult" Width="100%" runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowPaging="True" GridLines="None" PageSize="10" EditRowStyle-Wrap="False" PagerStyle-Wrap="True"
                                                    CssClass="GridView">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupID") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:ButtonField DataTextField="GroupName" ItemStyle-Font-Underline="true" ItemStyle-ForeColor="#595959" SortExpression="GroupName" HeaderText="Gruppe"
                                                            CommandName="Edit"></asp:ButtonField>
                                                        <asp:TemplateField SortExpression="IsCustomerGroup" HeaderText="Kundengruppe">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.IsCustomerGroup") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IsCustomerGroup") %>'>
                                                                </asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                       <asp:TemplateField SortExpression="Authorizationright" HeaderText="Geb. Amt anzeigen">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblJA" runat="server" Text="Ja" Visible='<%# DataBinder.Eval(Container, "DataItem.Authorizationright")=0 %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lblNein" runat="server" Text="Nein" Visible='<%# DataBinder.Eval(Container, "DataItem.Authorizationright")=1 %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:ButtonField CommandName="Del" HeaderText="Löschen" ButtonType="Image"  ControlStyle-Height="16px" ControlStyle-Width="16px" ImageUrl="Images/Papierkorb_01.gif">
                                                        </asp:ButtonField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="dataFooter">
                                &nbsp;
                            </div>
                        </div>
                        <div id="Input" runat="server" visible="False">
                            <div id="adminInput">
                                <table id="Tablex1" runat="server" cellspacing="0" cellpadding="0" width="100%"
                                    border="0">
                                    <tr>
                                        <td width="100%">
                                            <table style="border-color: #ffffff" width="100%">
                                                <tr id="trEditUser" runat="server">
                                                    <td align="left" width="50%" valign="top">
                                                        <table id="tblLeft" style="border-color: #ffffff; padding-right: 50px;" cellspacing="0"
                                                            cellpadding="0">
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <table id="tblGroupDaten" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                                        width="100%" border="0">
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Gruppenname:<asp:TextBox ID="txtGroupID" runat="server" Visible="False" Width="0px"
                                                                                    Height="0px" BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtGroupName" style="text-transform: uppercase;" MaxLength="15" Width="250px" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trCustomer" runat="server">
                                                                            <td class="firstLeft active">
                                                                                Firma:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtCustomer" Width="250px" runat="server" Class="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Pfad zum Handbuch:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtDocuPath" Width="250px" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trAccountingArea" runat="server">
                                                                            <td class="firstLeft active">
                                                                                <asp:TextBox ID="txtCustomerID" runat="server" Visible="False" Width="16px" Height="16px"
                                                                                    BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trTVShow" runat="server" class="formquery">
                                                                            <td class="firstLeft active">
                                                                               TeamViewer verwenden:
                                                                            </td>
                                                                            <td class="active">
                                                                                <span>
                                                                                    <asp:CheckBox ID="cbxTeamViewer" runat="server" AutoPostBack="true" Enabled="false"/>
                                                                                </span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trIsServiceGroup" runat="server" class="formquery">
                                                                            <td class="firstLeft active">
                                                                               Service-Gruppe:
                                                                            </td>
                                                                            <td class="active">
                                                                                <span>                                                                                    
                                                                                    <asp:CheckBox ID="cbxIsServiceGroup" runat="server" AutoPostBack="true" Enabled="false"/>
                                                                                </span>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="top" align="center" width="50%">
                                                        <table id="tblRight" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                            bgcolor="white" border="0">
                                                            <tr id="trPwdRules" runat="server">
                                                                <td align="left" colspan="2">
                                                                    <table id="tblPwdRules" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                                        width="100%" border="0">
                                                                        <tr class="formquery">
                                                                            <td class="active">
                                                                                Gebühr Amt benutzen:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:DropDownList ID="ddlAuthorizationright" runat="server" Width="250px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="active">
                                                                                Kundengruppe:
                                                                            </td>
                                                                            <td class="active">
                                                                                <span>
                                                                                    <asp:CheckBox ID="cbxIsCustomerGroup" runat="server"></asp:CheckBox></span>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trStartMethod" runat="server">
                                                                            <td class="active">
                                                                                Startmethode:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtStartMethod" runat="server" CssClass="InputTextbox" Width="250px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="active">
                                                                                n Sonderzeichen:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtNCapitalLetter" runat="server" CssClass="InputTextbox" Width="250px">1</asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>


                                            <table id="tblDown" cellspacing="0" cellpadding="0" style="border-width: 0px; border-color: #FFFFFF;"
                                                width="100%" >
                                                <tr id="trApp" runat="server">
                                                    <td valign="top" align="left" colspan="3">
                                            <div class="formqueryHeader"><span>Anwendungen</span>
                                            </div>                                                    
                                                        <table id="tblApp" cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF"
                                                            border="0">
                                                         
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" width="35%">
                                                                    nicht zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstAppUnAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150px"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 55px"></span>
                                                                    <asp:ImageButton ID="btnAssign" runat="server" ImageUrl="Images/Pfeil_vor_01.jpg"
                                                                        ToolTip="Zuweisen" Height="37px" Width="37px" />
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 30px">
                                                                        <asp:ImageButton ID="btnUnAssign" runat="server" ImageUrl="Images/Pfeil_zurueck_01.jpg"
                                                                            ToolTip="Entfernen" Height="37px" Width="37px" /></span>
                                                                </td>
                                                                <td class="active" width="35%">
                                                                    zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstAppAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trArchiv" runat="server">
                                                    <td class="InfoBoxFlat" valign="top" align="left" colspan="3">
                                            <div class="formqueryHeader"><span>Archive</span>
                                            </div>                                                         
                                                        <table id="tblArchiv" cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF"
                                                            border="0">
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" width="35%">
                                                                    nicht zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstArchivUnAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150px"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 55px"></span>
                                                                    <asp:ImageButton ID="btnAssignArchiv" runat="server" ImageUrl="Images/Pfeil_vor_01.jpg"
                                                                        ToolTip="Zuweisen" Height="37px" Width="37px" />
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 30px">
                                                                        <asp:ImageButton ID="btnUnAssignArchiv" runat="server" ImageUrl="Images/Pfeil_zurueck_01.jpg"
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
                                                    </td>
                                                </tr>
                                                <tr id="trEmployee" runat="server">
                                                    <td class="InfoBoxFlat" valign="top" align="left" colspan="3">
                                            <div class="formqueryHeader"><span> Verantwortliche Mitarbeiter</span>
                                            </div>                                                       
                                                        <table id="tblEmployee" cellspacing="0" cellpadding="0" style="border-color: #FFFFFF"
                                                            border="0">
                                                               <tr class="formquery">
                                                                <td class="firstLeft active" width="35%">
                                                                    nicht zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstEmployeeUnAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150px"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 55px"></span>
                                                                    <asp:ImageButton ID="btnAssignEmployee" runat="server" ImageUrl="Images/Pfeil_vor_01.jpg"
                                                                        ToolTip="Zuweisen" Height="37px" Width="37px" />
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 30px">
                                                                        <asp:ImageButton ID="btnUnAssignEmployee" runat="server" ImageUrl="Images/Pfeil_zurueck_01.jpg"
                                                                            ToolTip="Entfernen" Height="37px" Width="37px" /></span>
                                                                </td>
                                                                <td class="active" width="35%">
                                                                    zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstEmployeeAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trMeldung" runat="server">
                                                    <td class="InfoBoxFlat" valign="top" align="left" colspan="3">
                                            <div class="formqueryHeader"><span> Startmeldung</span>
                                            </div>                                                         
                                                        <table id="tblMeldung" cellspacing="0" cellpadding="0" border="0" style="border-color: #FFFFFF"
                                                            border="0">
                                                                     <tr class="formquery">
                                                                <td class="firstLeft active" valign="top" width="60%">
      
                                                                        <asp:TextBox ID="txtMessage" runat="server" Width="300px" MaxLength="500" Height="150px"
                                                                            Rows="5" TextMode="MultiLine"></asp:TextBox>
                                                                        <br />
                                                                        <asp:TextBox ID="txtMessageOld" runat="server" Visible="False" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                                <td class="firstLeft active" valign="top">
                                                                    <div>
                                                                        Häufigkeit der Startmeldungsanzeige (pro Benutzer):
                                                                        <asp:TextBox ID="txtMaxReadMessageCount" runat="server" Width="40px" MaxLength="2"
                                                                            CssClass="InputTextbox"></asp:TextBox></div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
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
