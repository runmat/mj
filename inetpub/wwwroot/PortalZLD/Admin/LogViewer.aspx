<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogViewer.aspx.vb" Inherits="Admin.LogViewer"
    MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Veränderungslog"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="4" style="width: 100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="active firstLeft" width="70px">
                                            Firma:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustomer" runat="server"></asp:Label><br />
                                            <asp:DropDownList ID="ddlFilterCustomer" runat="server" Width="160px" Height="20px"
                                                AutoPostBack="True" Visible="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" width="100%">
                                        </td>
                                    </tr>
                                    <tr id="trGruppe" runat="server" class="formquery">
                                        <td class="firstLeft active" width="70">
                                            Gruppe:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblGroup" runat="server" Visible="False"></asp:Label><br />
                                            <asp:DropDownList ID="ddlFilterGroup" runat="server" Width="160px" Height="20px">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="100%" colspan="2">
                                        </td>
                                    </tr>
                                    <tr id="trOrganisation" runat="server" class="formquery">
                                        <td class="firstLeft active" width="70px">
                                            Organisation:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOrganisation" runat="server" Visible="False"></asp:Label><br />
                                            <asp:DropDownList ID="ddlFilterOrganization" runat="server" Width="160px" Height="20px">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" width="100%">
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" width="70px">
                                            Benutzername:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUserID" runat="server" Width="0px" Height="0px" Visible="False"
                                                ForeColor="#CEDBDE" BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None">-1</asp:TextBox>
                                            <asp:Label ID="lblUserName" runat="server" Width="160px" Visible="False"></asp:Label>
                                            <asp:TextBox ID="txtFilterUserName" runat="server" Width="160px">*</asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnSuche" runat="server" CssClass="TablebuttonLarge" Width="128px"
                                                Text="» Benutzer suchen" Style="vertical-align: middle; text-align: center; font-size: 10px;
                                                font-weight: bold; color: #333333; height: 16px; margin-left: 4px;"></asp:LinkButton>
                                        </td>
                                        <td width="100%">
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblVonDatum" runat="server" Text="Von:"></asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtVonDatum" runat="server" Width="160px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtVonDatum_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtVonDatum">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td width="100%" colspan="2">
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblBisDatum" runat="server" Text="Bis:"></asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtBisDatum" runat="server" Width="160px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtBisDatum_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtBisDatum">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td width="100%" colspan="2">
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" width="70px">
                                            Aufgabe:
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlAction" runat="server" Width="320px">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="100%">
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="5" style="width: 100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» 
                                 Erstellen</asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div id="NavExcel" runat="Server" >
                                    <div class="ExcelDiv">
                                        <div align="right" class="rightPadding">
                                            &nbsp;<img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                                            <span class="ExcelSpan">
                                                <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel 
                                            herunterladen</asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                    
                                    <div id="pagination">
                                        <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                    </div>
                                </div>
                                <div id="data">
                                    <table id="TblUser" runat="server" cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:DataGrid ID="dgSearchResult" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="20" CssClass="GridView">
                                                    <ItemStyle Wrap="false" CssClass="ItemStyle" />
                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="GridTableHead">
                                                    </HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundColumn Visible="False" DataField="UserID" SortExpression="UserID" HeaderText="UserID">
                                                        </asp:BoundColumn>
                                                        <asp:ButtonColumn DataTextField="UserName" SortExpression="UserName" HeaderText="Benutzername"
                                                            CommandName="Edit"></asp:ButtonColumn>
                                                        <asp:BoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Kundenreferenz">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="GroupName" SortExpression="GroupName" HeaderText="Gruppe">
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn SortExpression="CustomerAdmin" HeaderText="Firmenadministrator">
                                                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbxSRCustomerAdmin" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "CustomerAdmin") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn SortExpression="TestUser" HeaderText="Testzugang">
                                                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbxSRTestUser" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "TestUser") %>'
                                                                    Enabled="False"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                    <PagerStyle Mode="NumericPages"></PagerStyle>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>

                                    </table>
                                    

                                    <table id="TblLog" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                                        <tr>
                                            <td>
                                                &nbsp;
                                                <asp:Label ID="Label2" runat="server"> Datenanzeige</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" CellPadding="0" AllowSorting="True"
                                                    AllowPaging="True" AutoGenerateColumns="False" BackColor="White">
                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                    <ItemStyle Wrap="false" CssClass="ItemStyle" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <Columns>
                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageButton1" runat="server"></asp:ImageButton>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" Visible="False"></asp:CheckBox>
                                                                <asp:Label ID="lblID" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="UserName" SortExpression="UserName" HeaderText="Benutzername">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Inserted" SortExpression="Inserted" HeaderText="Angelegt">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Task" SortExpression="Task" HeaderText="Anwendung"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Identification" SortExpression="Identification" HeaderText="Identifikation">
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn SortExpression="Description" HeaderText="Beschreibung">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>'
                                                                    NAME="Label1">
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="Textbox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>'
                                                                    NAME="Textbox1">
                                                                </asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                    <PagerStyle NextPageText="&amp;gt;&amp;gt;&amp;gt;" PrevPageText="&amp;lt;&amp;lt;&amp;lt;"
                                                        Position="Top"></PagerStyle>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </div>
                            </div>
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
