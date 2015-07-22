<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RechnungsanhangTemplateMgmt.aspx.vb" Inherits="Admin.RechnungsanhangTemplateMgmt"  MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function ConfirmDeleteTemplate() {
            return confirm("Wollen Sie das gewählte Template wirklich löschen?");
        }
    </script>
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <div id="navigationSubmenu">
                        <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                            Text="Zurück"></asp:LinkButton>                   
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"/>
                            </h1>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblError" CssClass="TextError"/>
                        </div>
                        <div id="Result" runat="Server">
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" width="100%"
                                    align="left" border="0">
                                    <tbody>
                                        <tr>
                                            <td align="left">
                                                <asp:GridView ID="dgSearchResult" Width="100%" runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowPaging="True" GridLines="None" PageSize="20" EditRowStyle-Wrap="False" PagerStyle-Wrap="True"
                                                    CssClass="GridView" DataKeyNames="ID">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"/>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"/>
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" Visible="False" />
                                                        <asp:ButtonField DataTextField="Bezeichnung" SortExpression="Bezeichnung" HeaderText="Bezeichnung" CommandName="Edit"/>
                                                        <asp:BoundField DataField="DatenAbZeile" SortExpression="DatenAbZeile" HeaderText="Daten ab Zeile"/>
                                                        <asp:BoundField DataField="SpalteKennzeichen" SortExpression="SpalteKennzeichen" HeaderText="Spalte Kennzeichen"/>
                                                        <asp:BoundField DataField="SpalteGebuehren" SortExpression="SpalteGebuehren" HeaderText="Spalte Gebühren"/>
                                                        <asp:TemplateField HeaderText="Löschen">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ImageUrl="/PortalZLD/Images/Papierkorb_01.gif" Width="16px" Height="16px" 
                                                                    CommandName="Del" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' OnClientClick="return ConfirmDeleteTemplate();"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbtnNew" runat="server" Text="Neues Template&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="16px" Width="155px" Visible="True"/>
                            </div>
                        </div>
                        <div id="Input" runat="server" visible="False">
                            <div id="adminInput">
                                <table id="Tablex1" class="" runat="server"  cellspacing="0"
                                    cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <table style="border-color: #ffffff">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <table id="tblLeft" style="border-color: #ffffff; padding-right: 50px;" cellspacing="0"
                                                            cellpadding="0">
                                                            <tr>
                                                                <td>
                                                                    <table style="border-color: #FFFFFF" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                        <tr class="formquery" style="display: none">
                                                                            <td class="firstLeft active" style="width: 200px">
                                                                               
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:Label ID="lblID" runat="server"/>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" style="width: 200px">
                                                                               Bezeichnung:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtBezeichnung" runat="server" CssClass="InputTextbox" Width="620px" MaxLength="50"/>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" style="width: 200px">
                                                                                Daten ab Zeile (1..99):
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtDatenAbZeile" runat="server" CssClass="InputTextbox" Width="40px" MaxLength="2"/>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" style="width: 200px">
                                                                                Spalte Kennzeichen (A..ZZ):
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:TextBox ID="txtSpaltenKennzeichen" runat="server" CssClass="InputTextbox TextUpperCase" Width="40px" MaxLength="2"/>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" style="width: 200px">
                                                                                Spalte Gebühren (A..ZZ):
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:TextBox ID="txtSpaltenGebuehren" runat="server" CssClass="InputTextbox TextUpperCase" Width="40px" MaxLength="2"/>
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
                                <div  style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>                                  
                                <div id="dataFooter">
                                    <asp:LinkButton ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                                    CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"/>
                                    &nbsp;
                                    <asp:LinkButton ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                                    CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"/>  
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
         </div>
    </div>
</asp:Content>
