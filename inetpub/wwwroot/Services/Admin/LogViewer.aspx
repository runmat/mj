<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogViewer.aspx.vb" Inherits="Admin.LogViewer" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="tel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <tel:RadAjaxManagerProxy runat="server"></tel:RadAjaxManagerProxy>
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
                           <%-- <asp:PostBackTrigger ControlID="lnkCreateExcel1" />--%>
                        </Triggers>
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red"></asp:Label>
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr class="formquery">
                                        <td class="firstLeft active" >
                                            &nbsp;
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
                                            <asp:TextBox ID="txtFilterUserName" runat="server" Width="156px">*</asp:TextBox>
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
                                        <td class="active" style="padding: 0px;" colspan="2">                                          
                                            <tel:RadDatePicker ID="rdpVonDatum" runat="server" Width="186px" 
                                            DateInput-BorderColor="#bfbfbf" DateInput-BorderWidth="1px" DateInput-BorderStyle="Solid" >
                                                <DateInput style="margin-right:18px;"></DateInput>                                                                                       
                                            </tel:RadDatePicker>
                                        </td>
                                        <td width="100%" >
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblBisDatum" runat="server" Text="Bis:"></asp:Label>
                                        </td>
                                        <td class="active" style="padding: 0px;" colspan="2">                                           
                                            <tel:RadDatePicker ID="rdpBisDatum" runat="server" Width="186px" 
                                            DateInput-BorderColor="#bfbfbf" DateInput-BorderWidth="1px" DateInput-BorderStyle="Solid" DateInput-Padding="0px">
                                            <DateInput style="margin-right:18px;"></DateInput> 
                                            </tel:RadDatePicker>
                                        </td>
                                        <td width="100%" >
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
                                        <td class="firstLeft active" width="70px">
                                           Meldungstyp:
                                        </td>
                                        <td >
                                            <asp:DropDownList ID="ddlMessagetype" runat="server" Width="160px">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="100%" colspan="2">
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
                                <%--<div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        &nbsp;<img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel 
                                        herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>--%>                               
                                <div id="data" style="border: 1px solid #dfdfdf;">
                                    <tel:RadGrid ID="rgSearchResult" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                        PageSize="10" BorderStyle="None">
                                        <ItemStyle Wrap="false" CssClass="ItemStyle" />
                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="GridTableHead"
                                            ForeColor="#2b4c91" />
                                        <FooterStyle CssClass="RADGridFooter" />
                                        <PagerStyle CssClass="RADGridFooter" />
                                        <MasterTableView>
                                            <Columns>
                                                <tel:GridBoundColumn Visible="False" DataField="UserID" SortExpression="UserID" HeaderText="UserID" />
                                                <tel:GridTemplateColumn SortExpression="UserName" HeaderText="Benutzername" DataField="UserName">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>'
                                                            CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "UserID") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </tel:GridTemplateColumn>
                                                <tel:GridBoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Kundenreferenz" />
                                                <tel:GridBoundColumn DataField="GroupName" SortExpression="GroupName" HeaderText="Gruppe" />
                                                <tel:GridTemplateColumn SortExpression="CustomerAdmin" HeaderText="Firmenadministrator">
                                                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbxSRCustomerAdmin" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "CustomerAdmin") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </tel:GridTemplateColumn>
                                                <tel:GridTemplateColumn SortExpression="TestUser" HeaderText="Testzugang">
                                                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbxSRTestUser" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "TestUser") %>'
                                                            Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </tel:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </tel:RadGrid>
                                    <table id="TblLog" runat="server" cellspacing="0" cellpadding="0" width="100%" border="0"
                                        style="margin-bottom: 0px;">
                                        <tr>
                                            <td>
                                                &nbsp;
                                                <asp:Label ID="Label2" runat="server"> Datenanzeige</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <style type="text/css">
                                                    DIV#content DIV#innerContentRight DIV#data TABLE .RADGridFooter td table
                                                    {
                                                        margin-bottom: 0px;
                                                    }
                                                </style>
                                                <tel:RadGrid ID="rgResult" runat="server" Width="100%" CellPadding="0" AllowSorting="True"
                                                    Visible="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                                    BorderStyle="None">
                                                    <ItemStyle Wrap="false" />
                                                    <FooterStyle CssClass="RADGridFooter" />
                                                    <PagerStyle CssClass="RADGridFooter" />
                                                    <MasterTableView DataKeyNames="ID">
                                                        <Columns>
                                                            <tel:GridBoundColumn DataField="ID" SortExpression="ID" HeaderText="ID" Visible="false" />
                                                            <tel:GridBoundColumn DataField="UserName" SortExpression="UserName" HeaderText="Benutzername" />
                                                            <tel:GridBoundColumn DataField="Inserted" SortExpression="Inserted" HeaderText="Angelegt" />
                                                            <tel:GridBoundColumn DataField="Task" SortExpression="Task" HeaderText="Anwendung" />
                                                            <tel:GridBoundColumn DataField="Identification" SortExpression="Identification" HeaderText="Identifikation" />
                                                            <tel:GridTemplateColumn SortExpression="Description" HeaderText="Beschreibung">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'
                                                                        NAME="Label1">
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="Textbox1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'
                                                                        NAME="Textbox1">
                                                                    </asp:TextBox>
                                                                </EditItemTemplate>
                                                            </tel:GridTemplateColumn>
                                                        </Columns>
                                                        <DetailTables>
                                                            <tel:GridTableView runat="server" AutoGenerateColumns="true" Width="100%">
                                                            </tel:GridTableView>
                                                        </DetailTables>
                                                    </MasterTableView>
                                                </tel:RadGrid>
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
