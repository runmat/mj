<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Contact.aspx.vb" Inherits="Admin.Contact"
    MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../JScript/Jquery/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../JScript/Jquery/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server" EnableViewState="False"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkUserManagement" runat="server" ToolTip="Benutzer" NavigateUrl="UserManagement.aspx"
                            Text="Benutzer | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkGroupManagement" runat="server" ToolTip="Gruppen" NavigateUrl="GroupManagement.aspx"
                            Text="Gruppen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkCustomerManagement" ToolTip="Kunden" runat="server" NavigateUrl="CustomerManagement.aspx"
                            Text="Kunden | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkArchivManagement" ToolTip="Archive" runat="server" NavigateUrl="ArchivManagement.aspx"
                            Text="Archive | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkAppManagement" runat="server" ToolTip="Anwendungen" NavigateUrl="AppManagement.aspx"
                            Text="Anwendungen"></asp:HyperLink>
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                            </h1>
                            <span id="arealnkSuche" class="AdminMgmtNav" style="display: none">&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;</span><a id="lnkSuche" class="AdminMgmtNavLink" href="javascript:void(0)" onclick="showSearchFilterArea();" style="display: none">Suche</a>
                            <span id="arealnkSuchergebnis" class="AdminMgmtNav" style="display: none">&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;</span><a id="lnkSuchergebnis" class="AdminMgmtNavLink" href="javascript:void(0)" onclick="showSearchResultArea();" style="display: none">Suchergebnis</a>
                            <%-- ih... enthält jeweils den Sollwert für den nächsten Seitenzustand --%>
                            <input id="ihExpandstatusSearchFilterArea" type="hidden" runat="server" value="1" />
                            <input id="ihExpandstatusSearchResultArea" type="hidden" runat="server" value="0" />
                            <input id="ihExpandStatusInputArea" type="hidden" runat="server" value="0" />
                            <input id="ihExpandStatusInputGroupArea" type="hidden" runat="server" value="0" />
                        </div>
                        <asp:Panel ID="DivSearch" DefaultButton="btnEmpty" runat="server" Style="display: none">
                            <div id="TableQuery">
                                <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Vorname:
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap">
                                                <asp:TextBox ID="txtFilterName1" runat="server" CssClass="InputTextbox" Width="257px">*</asp:TextBox>
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../images/empty.gif"
                                                    Width="1px" TabIndex="-1" />
                                            </td>
                                            <td class="firstLeft active">
                                                Firma:
                                            </td>
                                            <td class="rightPadding" id="tdHierarchyDisplay2">
                                                <asp:DropDownList ID="ddlFilterCustomer" runat="server" AutoPostBack="True" Font-Names="Verdana,sans-serif"
                                                    Width="260px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Nachname:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterName2" runat="server" CssClass="InputTextbox" Width="257px">*</asp:TextBox>
                                            </td>
                                            <td class="firstLeft active">
                                                Gruppe:
                                            </td>
                                            <td class="rightPadding">
                                                <asp:DropDownList ID="ddlFilterGroup" runat="server" Font-Names="Verdana,sans-serif"
                                                    Width="260px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblGroup" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="height: 22px">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr style="background-color: #dfdfdf; height: 22px">
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div id="QueryFooter" runat="server">
                                    <div id="dataQueryFooter">
                                        &nbsp;&nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="Neuer Ansprechpartner&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="21px" Width="155px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="lbtnCancel0" runat="server" Text="Neue Suche&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="21px" Width="155px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px" Visible="False"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="btnSuche" runat="server" Text="Suchen&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="21px" Width="155px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="Result" runat="Server" style="display: none">
                            <%--<div class="ExcelDiv">
                                <div align="right" id="trSearchSpacer" runat="server">
                                    <img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                                    <asp:HyperLink ID="lnkExcel" runat="server" Visible="False" ForeColor="White">Excel-Download: 
                                                                        rechte Maustaste -&gt; Speichern unter...</asp:HyperLink>
                                </div>
                            </div>--%>
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server" PageSizeIndex="0"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <asp:Panel ID="Panel1" runat="server">
                                    <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" align="left"
                                        border="0">
                                        <tr id="trSearchResult" runat="server">
                                            <td align="left">
                                                <asp:GridView ID="dgSearchResult" Width="100%" runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowPaging="True" GridLines="None" PageSize="10" EditRowStyle-Wrap="False" PagerStyle-Wrap="True"
                                                    CssClass="GridView">
                                                    <PagerSettings Visible="False" />
                                                    <PagerStyle Wrap="True" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False" HeaderText="ID">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="ID" CommandArgument="ID" CommandName="Sort" runat="server">ID</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lblGridAnsprechpartner" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ansprechpartner") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:ButtonField DataTextField="Ansprechpartner" SortExpression="Ansprechpartner"
                                                            CommandName="Edit" HeaderText="Ansprechpartner" />
                                                        <asp:ButtonField CommandName="Group" ButtonType="Image" Text="Direkt zur Kunden-/Gruppenzuweisung"
                                                            ImageUrl="/Services/Images/EditTableHS.png" ControlStyle-Height="16px" ControlStyle-Width="16px"
                                                            HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ControlStyle Height="16px" Width="16px" />
                                                            <HeaderStyle Width="25px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="Hierarchie" SortExpression="Hierarchie" HeaderText="Hierarchie">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Abteilung" SortExpression="Abteilung" HeaderText="Abteilung">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Position" SortExpression="Position" HeaderText="Position">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Telefon" SortExpression="Telefon" HeaderText="Telefon">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Mobile" SortExpression="Mobile" HeaderText="Mobile"></asp:BoundField>
                                                        <asp:BoundField DataField="Fax" SortExpression="Fax" HeaderText="Fax"></asp:BoundField>
                                                        <asp:ButtonField CommandName="Del" ButtonType="Image" ImageUrl="/Services/Images/del.png"
                                                            ControlStyle-Height="16px" ControlStyle-Width="16px" HeaderStyle-Width="30px"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ControlStyle Height="16px" Width="16px" />
                                                            <HeaderStyle Width="30px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:ButtonField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <div class="dataFooter">
                                &nbsp;
                            </div>
                        </div>
                        <div id="Input" runat="server" style="display: none">
                            <div id="adminInput">
                                <table id="Tablex1" runat="server" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="firstLeft active" colspan="2">
                                            &nbsp;
                                            <%--<asp:Label ID="lblErrorSave" CssClass="TextError" runat="server" EnableViewState="False"></asp:Label>
                                            <asp:Label ID="lblMessageSave" runat="server" CssClass="TextExtraLarge" 
                                                EnableViewState="False" ForeColor="#00CC00"></asp:Label>--%>
                                        </td>
                                    </tr>
                                    <tr id="trEditUser" runat="server">
                                        <td valign="top">
                                            <table id="tblLeft" style="border-color: #ffffff; padding-right: 50px;" cellspacing="0"
                                                cellpadding="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Anrede:
                                                    </td>
                                                    <td class="active">
                                                        <asp:DropDownList ID="ddlTitle" runat="server" AutoPostBack="True" CssClass="DropDowns">
                                                            <asp:ListItem Value="-" Selected="True">&lt;Bitte ausw&#228;hlen&gt;</asp:ListItem>
                                                            <asp:ListItem Value="Herr">Herr</asp:ListItem>
                                                            <asp:ListItem Value="Frau">Frau</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Vorname:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Nachname:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtLastName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Telefon:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Mobil:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtMobil" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Telefax:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtFax" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        E-Mail (x@y.z):
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtMail" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trMandant" runat="server" visible="false">
                                                    <td class="firstLeft active">
                                                        Mandant:
                                                    </td>
                                                    <td class="active">
                                                        <asp:DropDownList ID="ddlAccountingArea" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Kunden/Gruppen:
                                                    </td>
                                                    <td class="active">
                                                        <asp:ImageButton ID="ibtSetGroup" runat="server" ImageUrl="/Services/images/EditTableHS.png"
                                                            Height="20px" Width="20px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top">
                                            <table id="tblRight" cellspacing="0" style="border-color: #ffffff" cellpadding="0"
                                                width="100%">
                                                <tr id="trEmployee02" runat="server" class="formquery">
                                                    <td class="firstLeft active">
                                                        Hierarchie:
                                                    </td>
                                                    <td align="left" class="active">
                                                        <asp:DropDownList ID="ddlHierarchy" runat="server" AutoPostBack="True" CssClass="DropDowns">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trEmployee03" runat="server" class="formquery">
                                                    <td class="firstLeft active">
                                                        Abteilung:
                                                    </td>
                                                    <td align="left" class="active">
                                                        <asp:TextBox ID="txtDepartment" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trEmployee04" runat="server" class="formquery">
                                                    <td class="firstLeft active">
                                                        Position:
                                                    </td>
                                                    <td align="left" class="active">
                                                        <asp:TextBox ID="txtPosition" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trEmployee05" runat="server" class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblPicture" runat="server" Font-Bold="True">Bild hochladen:</asp:Label>
                                                    </td>
                                                    <td align="left" class="active">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr id="trEmployee07" runat="server" class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="/Services/images/placeholder.png"
                                                            Height="150px" Width="100px" EnableViewState="False" />
                                                    </td>
                                                    <td align="left" class="active" valign="top">
                                                        <asp:Label ID="lblPictureName" runat="server"></asp:Label>
                                                        <br />
                                                        <strong>
                                                            <input id="upFile" runat="server" name="File1" size="11" type="file" /><br />
                                                        </strong>
                                                        <br />
                                                        <asp:LinkButton ID="btnUpload" runat="server" Width="95px" CssClass="Textlarge">&#8226;&nbsp;Hinzufügen</asp:LinkButton>
                                                        &nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnRemove" runat="server" Width="95px">Entfernen</asp:LinkButton><br />
                                                        &nbsp;<br />
                                                        Darstellung auf 150 x 100 Pixeln.
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trCutomerGroups" runat="server">
                                        <td valign="top">
                                            <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" width="100%">
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:ImageButton ID="ibtnFilter" runat="server" Height="16px" ImageUrl="/Services/images/Filter.gif"
                                                            Width="16px" />
                                                        &nbsp;Firma (setzen für&nbsp;
                                                        <asp:Label ID="lblAnsprechpartner" runat="server" Font-Bold="True" ForeColor="#0033CC"></asp:Label>):
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Panel ID="pnlCustomer" runat="server" onscroll="SetDivPosition()" Height="300px"
                                                            Width="400px" ScrollBars="Vertical" BorderColor="#CCCCCC" BorderWidth="1px">
                                                            <asp:GridView ID="gvCustomer" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                Width="380px">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="cbxCustomer" runat="server" Enabled="False" />
                                                                            <asp:Label ID="lblCustID" runat="server" Visible="False" Text='<%# Bind("CustomerID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ControlStyle Width="10px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CustomerName") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btnCustomer" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                CommandName="Edit" Text='<%# Bind("CustomerName") %>' Style="text-align: left;
                                                                                padding-left: 15px; padding-right: 5px; white-space: nowrap; background-color: Transparent;
                                                                                border-style: none; font-family: Verdana; font-size: 10px; font-weight: bold;
                                                                                color: #595959;" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top" style="width: 100%">
                                            <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" width="100%">
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Gruppe:
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" valign="top">
                                                        <asp:Panel ID="pnlGroups" runat="server" Height="300px" Width="400px" ScrollBars="Vertical"
                                                            BorderColor="#CCCCCC" BorderWidth="1px">
                                                            <asp:CheckBoxList ID="cblGroups" runat="server" Height="290px" RepeatLayout="Flow"
                                                                Width="380px" AutoPostBack="True">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                                <div id="DivMatrix" runat="server">
                                    <asp:Table ID="Matrix" runat="server" Visible="False" Style="border-color: #ffffff">
                                    </asp:Table>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;&nbsp;<asp:LinkButton ID="lbtnBackToSearch" runat="server" Text="Zurück&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>
                                    <asp:LinkButton ID="lbtnBack" runat="server" Text="Zurück&amp;nbsp;&amp;#187; " CssClass="Tablebutton"
                                        Height="16px" Width="78px" Visible="False"></asp:LinkButton>
                                    <asp:LinkButton ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        var IsPostBack = '<%=IsPostBack.ToString() %>';
        window.onload = function () {
            var strCook = document.cookie;
            if (strCook.indexOf("!~") != 0) {
                var intS = strCook.indexOf("!~");
                var intE = strCook.indexOf("~!");
                var strPos = strCook.substring(intS + 2, intE);
                if (IsPostBack == 'True') {

                    if (null === document.getElementById("<%=pnlCustomer.ClientID %>")) {

                    }
                    else {
                        document.getElementById("<%=pnlCustomer.ClientID %>").scrollTop = strPos;
                    }

                }
                else {
                    document.cookie = "yPos=!~0~!";
                }
            }
        }
        function SetDivPosition() {
            var intY = document.getElementById("<%=pnlCustomer.ClientID %>").scrollTop;
            document.title = intY;
            document.cookie = "yPos=!~" + intY + "~!";
        }

        function CheckCollapseExpandStatus() {
            if ($("#<%= ihExpandstatusSearchFilterArea.ClientID %>").attr("value") == "1") {
                $("#arealnkSuche").hide();
                $("#lnkSuche").hide();
                $("#arealnkSuchergebnis").hide();
                $("#lnkSuchergebnis").hide();
                $("#<%= Result.ClientID %>").hide();
                $("#<%= Input.ClientID %>").hide();
                $("#<%= DivSearch.ClientID %>").show();
                $("#<%= txtFilterName1.ClientID %>").focus();
            }
            else if ($("#<%= ihExpandstatusSearchResultArea.ClientID %>").attr("value") == "1") {
                $("#arealnkSuchergebnis").hide();
                $("#lnkSuchergebnis").hide();
                $("#arealnkSuche").show();
                $("#lnkSuche").show();
                $("#<%= DivSearch.ClientID %>").hide();
                $("#<%= Input.ClientID %>").hide();
                $("#<%= Result.ClientID %>").show();
            }
            else if ($("#<%= ihExpandstatusInputArea.ClientID %>").attr("value") == "1") {
                $("#arealnkSuche").show();
                $("#lnkSuche").show();
                $("#arealnkSuchergebnis").show();
                $("#lnkSuchergebnis").show();
                $("#<%= DivSearch.ClientID %>").hide();
                $("#<%= Result.ClientID %>").hide();
                $("#<%= trCutomerGroups.ClientID %>").hide();
                $("#<%= trEditUser.ClientID %>").show();
                $("#<%= Input.ClientID %>").show();
            }
            else {
                $("#arealnkSuche").hide();
                $("#lnkSuche").hide();
                $("#arealnkSuchergebnis").hide();
                $("#lnkSuchergebnis").hide();
                $("#<%= DivSearch.ClientID %>").hide();
                $("#<%= Result.ClientID %>").hide();
                $("#<%= trEditUser.ClientID %>").hide();
                $("#<%= trCutomerGroups.ClientID %>").show();
                $("#<%= Input.ClientID %>").show();
            }
        }

        function showSearchFilterArea() {
            $("#<%= ihExpandstatusSearchFilterArea.ClientID %>").attr("value", "1");
            $("#<%= ihExpandstatusSearchResultArea.ClientID %>").attr("value", "0");
            $("#<%= ihExpandstatusInputArea.ClientID %>").attr("value", "0");
            $("#<%= ihExpandstatusInputGroupArea.ClientID %>").attr("value", "0");
            CheckCollapseExpandStatus();
        }

        function showSearchResultArea() {
            $("#<%= ihExpandstatusSearchFilterArea.ClientID %>").attr("value", "0");
            $("#<%= ihExpandstatusSearchResultArea.ClientID %>").attr("value", "1");
            $("#<%= ihExpandstatusInputArea.ClientID %>").attr("value", "0");
            $("#<%= ihExpandstatusInputGroupArea.ClientID %>").attr("value", "0");
            CheckCollapseExpandStatus();
        }

        $(function () {
            CheckCollapseExpandStatus();
        }); 

    </script>
</asp:Content>
