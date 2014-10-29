<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrganizationManagement.aspx.vb"
    Inherits="Admin.OrganizationManagement" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../JScript/Jquery/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../JScript/Jquery/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>

    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkUserManagement" runat="server" ToolTip="Benutzer" NavigateUrl="UserManagement.aspx"
                            Text="Benutzer | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkContact" runat="server" ToolTip="Ansprechpartner" NavigateUrl="Contact.aspx"
                            Text="Ansprechpartner | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkGroupManagement" runat="server" ToolTip="Gruppen" NavigateUrl="GroupManagement.aspx"
                            Text="Gruppen | "></asp:HyperLink>
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
                            <input id="ihExpandstatusSearchFilterArea" type="hidden" runat="server" value="1"/>
                            <input id="ihExpandstatusSearchResultArea" type="hidden" runat="server" value="0"/>
                            <input id="ihExpandStatusInputArea" type="hidden" runat="server" value="0"/>
                        </div>
                        <asp:Panel ID="DivSearch1" runat="server" DefaultButton="btnEmpty" style="display: none">
                            <div id="TableQuery">
                                <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Firma:
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap" width="100%">
                                                <asp:DropDownList ID="ddlFilterCustomer" runat="server"  Font-Names="Verdana,sans-serif"
                                                    Height="20px" Visible="False" Width="260px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCustomer" runat="server" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trSelectOrganization" runat="server">
                                            <td class="firstLeft active">
                                                Organisation:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterOrganizationName" runat="server" CssClass="InputTextbox"
                                                    Width="257px">*</asp:TextBox>
                                                <asp:Label ID="lblOrganizationName" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtEmpty" runat="server" CssClass="InputTextbox" Width="160px" Visible="False">*</asp:TextBox>
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../images/empty.gif"
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
                                        &nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="Neue Organisation&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="16px" Width="155px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="Result" runat="Server" style="display: none">
                            <%--<div class="ExcelDiv">
                                <div align="right" id="trSearchSpacer" runat="server">
                                    &nbsp;
                                </div>
                            </div>--%>
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server" PageSizeIndex="0"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" width="100%"
                                    align="left" border="0">
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
                                                                <asp:Label ID="lblOrgaID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrganizationID") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:ButtonField DataTextField="OrganizationName" SortExpression="OrganizationName"
                                                            HeaderText="Organisation" CommandName="Edit">
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="TableLinkHead"></HeaderStyle>
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="OrganizationReference" SortExpression="OrganizationReference"
                                                            HeaderText="Referenz" />
                                                        <asp:TemplateField SortExpression="AllOrganizations" HeaderText="Zeige ALLE Organisationen">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.AllOrganizations") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AllOrganizations") %>'>
                                                                </asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Customername" SortExpression="Customername" HeaderText="Firma" />
                                                        <asp:BoundField DataField="OName" SortExpression="OName" HeaderText="Kontakt-Name" />
                                                        <asp:ButtonField CommandName="Del" HeaderText="Löschen" ButtonType="Image" ImageUrl="../../Images/Papierkorb_01.gif" ControlStyle-Height="16px" ControlStyle-Width="16px">
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
                        <div id="Input" runat="server" style="display: none">
                            <div id="adminInput">
                                <table id="Tablex1" class="" runat="server"  cellspacing="0"
                                    cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <table style="border-color: #ffffff">
                                                <tr id="trEditUser" runat="server">
                                                    <td align="left" width="33%" valign="top">
                                                        <table id="tblLeft" style="border-color: #ffffff; padding-right: 50px;" cellspacing="0"
                                                            cellpadding="0">
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <table id="tblGroupDaten" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                                        width="100%" border="0">
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Firma:<asp:TextBox ID="txtCustomerID" runat="server" Visible="False" Width="0px"
                                                                                    Height="0px" BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtCustomer" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Organisationsname:<asp:TextBox ID="txtOrganizationID" runat="server" Visible="False"
                                                                                    Width="0px" Height="0px" BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtOrganizationName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Organisationsreferenz:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:TextBox ID="txtOrganizationReference" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                                Zeige ALLE Organisationen:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <span>
                                                                                    <asp:CheckBox ID="cbxAllOrganizations" runat="server"></asp:CheckBox></span>
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
                                                                            <td class="firstLeft active">
                                                                                Kontakt-Adresse:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:TextBox ID="txtCAddress" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table id="tblRight" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                            bgcolor="white" border="0">
                                                            <tr class="formquery">
                                                                <td class="active">
                                                                    Mailadresse Anzeigetext:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtCMailDisplay" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="active">
                                                                    Mailadresse:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtCMail" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="active">
                                                                    Web-Adresse Anzeigetext:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtCWebDisplay" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="active">
                                                                    Web-Adresse:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtCWeb" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td nowrap="nowrap" class="active">
                                                                    Pfad zum Logo:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtLogoPath" runat="server" CssClass="InputTextbox">../Images/Logo.gif</asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td nowrap="nowrap" class="active">
                                                                    Pfad zu den Stylesheets:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtCssPath" runat="server" CssClass="InputTextbox">Styles.css</asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                

                        
                                </table>
                                 <div  style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>                                  
                                <div id="dataFooter">
                                                                          &nbsp;&nbsp;<asp:LinkButton class="Tablebutton" ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                                CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;<asp:LinkButton
                                                    class="Tablebutton" ID="lbtnConfirm" runat="server" Text="Bestätigen&amp;nbsp;&amp;#187; "
                                                    CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;<asp:LinkButton
                                                        class="Tablebutton" ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                                        CssClass="Tablebutton" Height="16px" Width="78px" ></asp:LinkButton>&nbsp;<asp:LinkButton
                                                            class="Tablebutton" ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                                            CssClass="Tablebutton" Height="16px" Width="78px" ></asp:LinkButton>  
                                
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
         </div>
    </div>   
    <script type="text/javascript">

        function CheckCollapseExpandStatus() {
            if ($("#<%= ihExpandstatusSearchFilterArea.ClientID %>").attr("value") == "1") {
                $("#arealnkSuche").hide();
                $("#lnkSuche").hide();
                $("#arealnkSuchergebnis").hide();
                $("#lnkSuchergebnis").hide();
                $("#<%= Result.ClientID %>").hide();
                $("#<%= Input.ClientID %>").hide();
                $("#<%= DivSearch1.ClientID %>").show();
            }
            else if ($("#<%= ihExpandstatusSearchResultArea.ClientID %>").attr("value") == "1") {
                $("#arealnkSuchergebnis").hide();
                $("#lnkSuchergebnis").hide();
                $("#arealnkSuche").show();
                $("#lnkSuche").show();
                $("#<%= DivSearch1.ClientID %>").hide();
                $("#<%= Input.ClientID %>").hide();
                $("#<%= Result.ClientID %>").show();
            }
            else {
                $("#arealnkSuche").show();
                $("#lnkSuche").show();
                $("#arealnkSuchergebnis").show();
                $("#lnkSuchergebnis").show();
                $("#<%= DivSearch1.ClientID %>").hide();
                $("#<%= Result.ClientID %>").hide();
                $("#<%= Input.ClientID %>").show();
            }
        }

        function showSearchFilterArea() {
            $("#<%= ihExpandstatusSearchFilterArea.ClientID %>").attr("value", "1");
            $("#<%= ihExpandstatusSearchResultArea.ClientID %>").attr("value", "0");
            $("#<%= ihExpandstatusInputArea.ClientID %>").attr("value", "0");
            CheckCollapseExpandStatus();
        }

        function showSearchResultArea() {
            $("#<%= ihExpandstatusSearchFilterArea.ClientID %>").attr("value", "0");
            $("#<%= ihExpandstatusSearchResultArea.ClientID %>").attr("value", "1");
            $("#<%= ihExpandstatusInputArea.ClientID %>").attr("value", "0");
            CheckCollapseExpandStatus();
        }

        $(function () {
            CheckCollapseExpandStatus();
        }); 
        
    </script>
</asp:Content>
