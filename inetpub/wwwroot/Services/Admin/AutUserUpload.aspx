<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AutUserUpload.aspx.vb"
    Inherits="Admin.AutUserUpload" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                    <div id="navigationSubmenu">
                        &nbsp;
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>                               
                        <asp:Panel ID="DivSearch" runat="server" >
                            <div id="TableQuery">
                                <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" >
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Firma:
                                            </td>
                                            <td class="active" nowrap="nowrap" style="width:100%"  colspan="2">
                                                <asp:DropDownList ID="ddlFilterCustomer" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="(Bitte wählen Sie eine Firma aus.)"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Art:
                                            </td>
                                            <td class="active" colspan="2" style="width: 100%" >
                                                <span>
                                                    <asp:RadioButtonList ID="rblUpload" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
                                                        Width="175px">
                                                        <asp:ListItem Selected="True">Upload</asp:ListItem>
                                                        <asp:ListItem>Freigeben</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trRemoteLoginKey" runat="server" visible="false">
                                            <td class="firstLeft active" >
                                                RemoteLoginKey generieren:
                                            </td>
                                            <td class="active" >
                                                <asp:CheckBox ID="chkRemoteLoginKey" runat="server" />
                                            </td>
                                            <td class="active" style="width: 100%" >
                                                <span style="color: red">Ist diese Option aktiviert, werden für alle angelegten<br />Benutzer RemoteLogin-Schlüssel generiert</span>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trBenutzerFreigeben" runat="server" >
                                            <td class="firstLeft active" >
                                                Benutzer freigeben:
                                            </td>
                                            <td class="active" >
                                                <asp:CheckBox ID="chkBenutzerFreigeben" runat="server" Checked="true" />
                                            </td>
                                            <td class="active" style="width: 100%" >
                                                <span style="color: red">Ist diese Option aktiviert, werden alle angelegten<br />Benutzer automatisch freigegeben</span>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trKeineMailsSenden" runat="server" >
                                            <td class="firstLeft active" >
                                                keine Mails senden:
                                            </td>
                                            <td class="active" >
                                                <asp:CheckBox ID="chkKeineMailsSenden" runat="server" Checked="false" />
                                            </td>
                                            <td class="active" style="width: 100%" >
                                                <span style="color: red">Ist diese Option aktiviert, wird das Versenden von<br />EMails an die neu angelegten Benutzer unterdrückt</span>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trUsernamePrefix" runat="server" visible="false" >
                                            <td class="firstLeft active" >
                                                Präfix für automatisch ver-<br />gebene Benutzernamen:
                                            </td>
                                            <td class="active" colspan="2">
                                                <asp:TextBox ID="txtUsernamePrefix" Width="150px" runat="server" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery" >
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblDateiauswahl" runat="server" Text="Dateiauswahl:"></asp:Label>
                                            </td>
                                            <td class="active" >
                                                <input id="upFile" type="file" size="35" name="File1" runat="server" />
                                            </td>
                                            <td class="active" style="width: 100%" >
                                                <asp:Panel ID="Panel2" runat="server" Height="22px">
                                                    <asp:ImageButton ID="Image1" runat="server" ImageUrl="../images/down.gif" AlternateText="(Details...)"/>      
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                        <td colspan="3" class="firstLeft active" style="padding-right: 0px">
                                         <asp:Panel ID="Panel1" runat="server"  Height="0" Width="100%">
                                            <table id="InfoTab" cellpadding="0" cellspacing="0" border="0" style="width: 65%; font-size: 9px;">
                                                        <tr>
                                                            <td style="width: 100%" nowrap="nowrap">
                                                                Upload von Vertragsnummern
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%" nowrap="nowrap">
                                                                Erwarteter Dateityp für den Upload: Excel-Datei (*.xls)
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 75%">
                                                                Erwartetes Dateiformat (<strong>mit</strong> Spaltenüberschriften)
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table id="Table1" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf" >
                                                                            Anrede
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Vorname
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Name
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Benutzername
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            <asp:Label runat="server" ID="lblRef1" />
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            <asp:Label runat="server" ID="lblRef2" />
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            <asp:Label runat="server" ID="lblRef3" />
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Filiale
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Testzugang
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Gruppe
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Organisation
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            E-Mailadresse
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Telefonnummer
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Gültigkeitsdatum
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            <asp:Label runat="server" ID="lblWarenkorbNurEigene" Text="Warenkorb nur eigene" Visible="False"/>
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            <asp:Label runat="server" ID="lblDarfNichtAbsenden" Text="Darf Vorgänge nicht absenden" Visible="False"/>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Frau
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Monika
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Schmidt
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            MSchmidt1
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            ja
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Händler
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Standard
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            mmustermann@firma.de
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            +49(40)1234567
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            01.01.2012
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            <asp:Label runat="server" ID="lblWarenkorbNurEigeneZeile1" Text="ja" Visible="False"/>
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            <asp:Label runat="server" ID="lblDarfNichtAbsendenZeile1" Text="ja" Visible="False"/>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Herr
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Karl
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Meier
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            KMeier
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Bank
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            Standard
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            kmeier@firma.de
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            +49(30)7654321
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            <asp:Label runat="server" ID="lblWarenkorbNurEigeneZeile2" Text="" Visible="False"/>
                                                                        </td>
                                                                        <td style="padding-right: 5px;padding-left: 5px; border: solid 1px #dfdfdf">
                                                                            <asp:Label runat="server" ID="lblDarfNichtAbsendenZeile2" Text="" Visible="False"/>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>                                    
                                        </asp:Panel>    
                                    
                                        </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                            </td>
                                            <td style="width: 35%"  colspan="2">
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../images/empty.gif"
                                                    Width="1px" />
                                                <asp:Label ID="lblExcelfile" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                                <asp:TextBox ID="txtEmpty" runat="server" CssClass="InputTextbox" 
                                                    Visible="False" Width="100px">*</asp:TextBox>
                                            </td>
                                            <td align="right" class="rightPadding" nowrap="nowrap"  colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr style="background-color: #dfdfdf; height: 22px">
                                            <td colspan="3">

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
    

                                
                                <div id="QueryFooter" runat="server">
                                    <div id="dataQueryFooter">
                                        <asp:LinkButton ID="lbtnUpload" runat="server" class="Tablebutton"
                                            CssClass="Tablebutton" Font-Names="Verdana,sans-serif" Font-Size="10px"
                                            Height="16px" Text="&amp;nbsp;&amp;#187; Upload" Width="78px"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnLaden" runat="server" CssClass="Tablebutton" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px" Height="16px" Text="&amp;nbsp;&amp;#187; Bearbeiten" 
                                            Width="78px" Visible="False"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnSave" runat="server" CssClass="Tablebutton" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px" Height="16px" Text="&amp;nbsp;&amp;#187; Anlegen" 
                                            Width="78px" Visible="False"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnPruefen" runat="server" CssClass="Tablebutton" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px" Height="16px" Text="&amp;nbsp;&amp;#187; Prüfen" 
                                            Width="78px" Visible="False"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnFreigeben" runat="server" CssClass="Tablebutton" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px" Height="16px" Text="&amp;nbsp;&amp;#187; Suchen" 
                                            Width="78px" Visible="False"></asp:LinkButton>                                                                                        
                                       <asp:LinkButton ID="lbtnBack" runat="server" CssClass="Tablebutton" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px" Height="16px" Text="&amp;nbsp;&amp;#187; Zurück" Width="78px"></asp:LinkButton>                                              
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                           <ajaxToolkit:CollapsiblePanelExtender ID="cpeDemo" runat="Server"
                            TargetControlID="Panel1"
                            ExpandControlID="Panel2"
                            CollapseControlID="Panel2" 
                            Collapsed="True"
                            ImageControlID="Image1"    
                            ExpandedImage="../images/up.gif"
                            CollapsedImage="../images/down.gif"
                             SuppressPostBack="true" />     
                        <div id="Result" runat="Server" visible="false">
                            <div class="ExcelDiv">
                                <div align="right" id="trSearchSpacer" runat="server">
                                    <img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                                            <span class="ExcelSpan">
                                                <asp:LinkButton ID="lnkCreateExcel" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                            </span>
                                </div>
                            </div>
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" width="100%"
                                    align="left" border="0">
                                    <tbody>
                                        <tr id="trSearchResult" runat="server">
                                            <td align="left">
                                                <asp:GridView ID="grvAusgabe" Width="100%" runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowPaging="True" GridLines="None" PageSize="20" EditRowStyle-Wrap="False" PagerStyle-Wrap="True"
                                                    CssClass="GridView">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Anrede" SortExpression="Title">
                                                            <HeaderStyle Width="50px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAnrede" runat="server" Text='<%# Bind("Title") %>' Width="50px"
                                                                    MaxLength="4"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Vorname" SortExpression="Firstname">
                                                            <HeaderStyle Width="80px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtVorname" runat="server" MaxLength="20" Text='<%# Bind("Firstname") %>'
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nachname" SortExpression="LastName">
                                                            <HeaderStyle Width="120px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtNachname" runat="server" MaxLength="50" Text='<%# Bind("LastName") %>'
                                                                    Width="120px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Benutzername" SortExpression="Username">
                                                            <HeaderStyle Width="120px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtUsername" runat="server" MaxLength="50" Text='<%# Bind("Username") %>'
                                                                    Width="120px" Wrap="False"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Referenz" SortExpression="Reference">
                                                            <HeaderStyle Width="70px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtReference" runat="server" MaxLength="12" Text='<%# Bind("Reference") %>'
                                                                    Width="70px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Referenz 2" SortExpression="Reference2">
                                                            <HeaderStyle Width="70px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtReference2" runat="server" MaxLength="12" Text='<%# Bind("Reference2") %>'
                                                                    Width="70px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Referenz 3" SortExpression="Reference3">
                                                            <HeaderStyle Width="70px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtReference3" runat="server" MaxLength="12" Text='<%# Bind("Reference3") %>'
                                                                    Width="70px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Filiale" SortExpression="Store">
                                                            <HeaderStyle Width="80px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtFiliale" runat="server" MaxLength="50" Text='<%# Bind("Store") %>'
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Test" SortExpression="TestUser">
                                                            <HeaderStyle Width="30px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtTestzugang" runat="server" MaxLength="4" Text='<%# Bind("TestUser") %>'
                                                                    Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Gruppe" SortExpression="GroupName">
                                                            <HeaderStyle Width="80px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtGruppe" runat="server" MaxLength="50" Text='<%# Bind("GroupName") %>'
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Organisation" SortExpression="Organization">
                                                            <HeaderStyle Width="80px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtOrganization" runat="server" MaxLength="50" Text='<%# Bind("Organization") %>'
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="E-Mailadresse" SortExpression="EMailAdress">
                                                            <HeaderStyle Width="150px" Wrap="false" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMail" runat="server" MaxLength="50" Text='<%# Bind("EMailAdress") %>'
                                                                    Width="150px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Telefonnummer" SortExpression="Telephone">
                                                            <HeaderStyle Width="80px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtTelefon" runat="server" MaxLength="50" Text='<%# Bind("Telephone") %>'
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Gültig ab" SortExpression="ValidFrom">
                                                            <HeaderStyle Width="80px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtGueltigkeitsdatum" runat="server" MaxLength="10" Text='<%# DataBinder.Eval(Container, "DataItem.ValidFrom","{0:d}") %>'
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Warenkorb nur eigene" SortExpression="WarenkorbNurEigene" Visible="False">
                                                            <HeaderStyle Width="110px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtWarenkorbNurEigene" runat="server" MaxLength="4" Text='<%# Bind("WarenkorbNurEigene") %>'
                                                                    Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Darf Vorgänge nicht absenden" SortExpression="DarfNichtAbsenden" Visible="False">
                                                            <HeaderStyle Width="150px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDarfNichtAbsenden" runat="server" MaxLength="4" Text='<%# Bind("DarfNichtAbsenden") %>'
                                                                    Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Error" SortExpression="Error" HeaderStyle-Width="50px"
                                                            Visible="False">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkError" runat="server" Checked='<%# Bind("Error") %>' Enabled="False" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="50px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID">
                                                            <HeaderStyle Width="0px" />
                                                            <ItemStyle Width="0px" />
                                                        </asp:BoundField>
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
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
