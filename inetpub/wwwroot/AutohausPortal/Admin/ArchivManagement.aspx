<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ArchivManagement.aspx.vb" Inherits="Admin.ArchivManagement"  MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkUserManagement" runat="server" ToolTip="Benutzer" NavigateUrl="UserManagement.aspx"
                            Text="Benutzer | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkContactManagement" runat="server" ToolTip="Ansprechpartner" 
                            NavigateUrl="Contact.aspx" Text="Ansprechpartner | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkGroupManagement" runat="server" ToolTip="Gruppen" NavigateUrl="GroupManagement.aspx"
                            Text="Gruppen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>                            
                        <asp:HyperLink ID="lnkCustomerManagement" ToolTip="Kunden" runat="server" NavigateUrl="CustomerManagement.aspx"
                            Text="Kunden | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkAppManagement" runat="server" ToolTip="Anwendungen" NavigateUrl="AppManagement.aspx"
                            Text="Anwendungen"></asp:HyperLink>                    
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
                                            <td class="firstLeft active" colspan="3">
                                                &nbsp;
                                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trSelectOrganization" runat="server">
                                            <td class="firstLeft active">
                                                Archiv-Name:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterEasyArchivName" runat="server" CssClass="InputTextbox"
                                                    Width="257px">*</asp:TextBox>
                                            <asp:textbox id="Textbox1" runat="server" Visible="False"  BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None" ForeColor="#CEDBDE">-1</asp:textbox>
                                            </td>
                                            <td class="active">&nbsp;</td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" >
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="Images/empty.gif"
                                                    Width="1px" />
                                                <asp:TextBox ID="txtEmpty" runat="server" CssClass="InputTextbox" Width="160px" Visible="False">*</asp:TextBox>
                                            </td>
                                            <td class="active" width="100%">&nbsp;</td>
                                        </tr>
                                        <tr style="background-color: #dfdfdf; height: 22px">
                                            <td colspan="3" width="100%">
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
                                        &nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="Neues Archiv&amp;nbsp;&amp;#187; "
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
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" width="100%"
                                    align="left" border="0">
                                    <tbody>
                                        <tr id="trSearchResult" runat="server">
                                            <td align="left">
                                                <asp:GridView ID="dgSearchResult" Width="100%" runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowPaging="True" GridLines="None" PageSize="20" EditRowStyle-Wrap="False" PagerStyle-Wrap="True"
                                                    CssClass="GridView">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblArchivID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ArchivID") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:ButtonField DataTextField="EasyArchivName" SortExpression="EasyArchivName"
                                                            HeaderText="Archiv" CommandName="Edit">
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="EasyLagerortName" SortExpression="EasyLagerortName"
                                                            HeaderText="Lagerort-Name" />
                                                        <asp:BoundField DataField="EasyQueryIndexName" SortExpression="EasyQueryIndexName" HeaderText="QueryIndex-Name" />
                                                        <asp:BoundField DataField="EasyTitleName" SortExpression="EasyTitleName" HeaderText="Titel" />
                                                        <asp:BoundField DataField="Archivetype" SortExpression="Archivetype" HeaderText="Archivtyp" />
                                                        <asp:ButtonField CommandName="Del" HeaderText="Löschen" ButtonType="Image" ImageUrl="Images/Papierkorb_01.gif"  ControlStyle-Height="16px" ControlStyle-Width="16px">
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
                                                                               Archiv-Name:<asp:TextBox ID="txtArchivID" runat="server" Visible="False" Width="0px"
                                                                                    Height="0px" BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtEasyArchivName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Lagerort-Name:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtEasyLagerortName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                QueryIndex:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:TextBox ID="txtEasyQueryIndex" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                                QueryIndex-Name:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:TextBox ID="txtEasyQueryIndexName" runat="server" CssClass="InputTextbox"></asp:TextBox>
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
                                                                    Titel:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtEasyTitleName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="active">
                                                                    DefaultQuery:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtDefaultQuery" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="active">
                                                                    Archivetype:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtArchivetype" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="active">
                                                                    SortOrder:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtSortOrder" runat="server" CssClass="InputTextbox"></asp:TextBox>
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
 
</asp:Content>