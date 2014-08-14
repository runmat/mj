<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserHistory.aspx.vb" Inherits="Admin.UserHistory"
    MasterPageFile="MasterPage/Admin.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Benutzerhistorie"></asp:Label>                            
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />                                                        
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table id="tblSearch" runat="server" cellpadding="0" cellspacing="0" width="100%" border="0">                                 
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="5" width="100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Firma:
                                        </td>
                                        <td class="firstLeft" width="25%">
                                            <%--<asp:label id="lblCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label>--%>
                                            <asp:DropDownList ID="ddlFilterCustomer" runat="server" Visible="false" AutoPostBack="True"
                                                Height="20px" Width="160px">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="firstLeft active">
                                            Benutzername:
                                        </td>
                                        <td class="firstLeft" width="25%">
                                            <asp:TextBox ID="txtFilterUserName" runat="server" Width="160px" Height="20px">*</asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr id="trSelectOrganization" runat="server" class="formquery">
                                        <td class="firstLeft active">
                                            Organisation:
                                        </td>
                                        <td class="firstLeft" width="25%">
                                            <%--<asp:label id="lblOrganization" runat="server" CssClass="InfoBoxFlat" Visible="False" Width="160px"></asp:label>--%>
                                            <asp:DropDownList ID="ddlFilterOrganization" runat="server" Height="20px" Width="160px"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="firstLeft active">
                                            Testzugang:
                                        </td>
                                        <td rowspan="2" class="firstLeft">
                                            <asp:RadioButton ID="rbTestUserAll" runat="server" Checked="True" Text="Alle" GroupName="grpTestUser"
                                                border="0"></asp:RadioButton><br>
                                            <asp:RadioButton ID="rbTestUserProd" runat="server" Text="Nur produktiv" GroupName="grpTestUser"
                                                border="0"></asp:RadioButton><br>
                                            <asp:RadioButton ID="rbTestUserTest" runat="server" Text="Nur Test" GroupName="grpTestUser"
                                                border="0"></asp:RadioButton>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Gruppe:
                                        </td>
                                        <td class="firstLeft">
                                            <asp:DropDownList ID="ddlFilterGroup" runat="server" Height="20px" Width="160px"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="firstLeft active"></td>
                                        <td></td>
								    </tr>				
                                    <tr class="formquery">                                        
                                        <td class="firstLeft" colspan="5" align="right">
                                            <asp:linkbutton id="btnSuche" runat="server" Class="Tablebutton" Width="78px">»Suchen</asp:linkbutton>
                                        </td>                                            
                                    </tr>
                                    <tr>
                                        <td class="firstLeft" colspan="5">&nbsp;</td>
                                    </tr>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="dataQueryFooter" runat="server">
                                    &nbsp;
                            </div>  
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="/PortalZLD/Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel 
                                        herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server" ></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:DataGrid ID="dgSearchResult" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AllowPaging="True" GridLines="None" PageSize="20" CssClass="GridView">
                                                    <SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"
                                                        Wrap="false"></HeaderStyle>
                                                    <PagerStyle Visible="False" />
                                                    <AlternatingItemStyle CssClass="GridTableAlternate" />
                                                    <ItemStyle Wrap="false" CssClass="ItemStyle" />
                                                    <EditItemStyle Wrap="False"></EditItemStyle>
                                                    <Columns>
                                                        <asp:BoundColumn Visible="False" DataField="UserHistoryID" SortExpression="UserHistoryID"
                                                            HeaderText="UserHistoryID"></asp:BoundColumn>
                                                        <asp:ButtonColumn DataTextField="UserName" SortExpression="UserName" HeaderText="Benutzername"
                                                            CommandName="Edit"></asp:ButtonColumn>
                                                        <asp:BoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Kunden-&lt;br&gt;referenz">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="GroupName" SortExpression="GroupName" HeaderText="Gruppe">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="OrganizationName" SortExpression="OrganizationName" HeaderText="Organisation">
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn SortExpression="CustomerAdmin" HeaderText="Firmen-&lt;br&gt;Administrator">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbxSRCustomerAdmin" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "CustomerAdmin") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn SortExpression="TestUser" HeaderText="Testzugang">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbxSRTestUser" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "TestUser") %>'
                                                                    Enabled="False"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="LastPwdChange" SortExpression="LastPwdChange" HeaderText="letzte&lt;br&gt;Kennwort&#228;nderung"
                                                            DataFormatString="{0:dd.MM.yy}">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn SortExpression="PwdNeverExpires" HeaderText="Kennwort&lt;br&gt;l&#228;uft nicht ab">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbxSRPwdNeverExpires" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "PwdNeverExpires") %>'
                                                                    Enabled="False"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="FailedLogins" SortExpression="FailedLogins" HeaderText="Anmelde-&lt;br&gt;Fehlversuche">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn SortExpression="AccountIsLockedOut" HeaderText="Konto&lt;br&gt;gesperrt">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbxSRAccountIsLockedOut" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "AccountIsLockedOut") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn SortExpression="Deleted" HeaderText="Konto&lt;br&gt;gel&#246;scht">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbxAccountDeleted" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "Deleted") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                    <%--<PagerStyle Mode="NumericPages"></PagerStyle>--%>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="Div7" runat="server">
                                &nbsp;
                            </div>
                            <div id="EditUser" runat="server" visible="false" class="active GridView">                                
                                <table width="100%" border="0" style="color:#595959;">
                                    <tr>
                                        <td valign="top" align="left">
                                            <table id="tblLeft" cellspacing="2" cellpadding="0" width="345" bgcolor="white" border="0" class="formquery">
                                                <tr>
                                                    <td height="22" style="font-weight:bold;">
                                                        Benutzername:
                                                    </td>
                                                    <td height="22">
                                                        <asp:TextBox ID="txtUserHistoryID" runat="server" Visible="False" Width="10px" Height="10px"
                                                            BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None" ForeColor="#CEDBDE">-1</asp:TextBox>
                                                        <asp:Label ID="txtUserName" runat="server" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="22" style="font-weight:bold;">
                                                        Kundenreferenz:
                                                    </td>
                                                    <td height="22">
                                                        <asp:Label ID="txtReference" runat="server" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trTestUser" runat="server">
                                                    <td height="22" style="font-weight:bold;">
                                                        Test-Zugang:
                                                    </td>
                                                    <td height="22">
                                                        <asp:CheckBox ID="cbxTestUser" runat="server" Enabled="False"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr id="trCustomer" runat="server">
                                                    <td height="22" style="font-weight:bold;">
                                                        Firma:
                                                    </td>
                                                    <td height="22">
                                                        <asp:Label ID="ddlCustomer" runat="server" Class="InfoBoxFlat" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trCustomerAdmin" runat="server">
                                                    <td height="22" style="font-weight:bold;">
                                                        Firmenadministrator:
                                                    </td>
                                                    <td height="22">
                                                        <asp:CheckBox ID="cbxCustomerAdmin" runat="server" Enabled="False"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr id="trGroup" runat="server">
                                                    <td height="22" style="font-weight:bold;">
                                                        Gruppe:
                                                    </td>
                                                    <td height="22">
                                                        <asp:Label ID="ddlGroups" runat="server" Class="InfoBoxFlat" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trOrganization" runat="server">
                                                    <td height="22" style="font-weight:bold;">
                                                        Organisation:
                                                    </td>
                                                    <td height="22">
                                                        <asp:Label ID="ddlOrganizations" runat="server" Class="InfoBoxFlat" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trOrganizationAdministrator" runat="server">
                                                    <td height="22" style="font-weight:bold;">
                                                        Organisationadministrator:
                                                    </td>
                                                    <td height="22">
                                                        <asp:CheckBox ID="cbxOrganizationAdmin" runat="server" Enabled="False"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>                                        
                                        <td valign="top" align="left">
                                            <table id="tblRight" cellspacing="2" cellpadding="0" width="345" bgcolor="white"
                                                border="0">
                                                <tr>
                                                    <td height="22" style="font-weight:bold;">
                                                        letzte Kennwortänderung:
                                                    </td>
                                                    <td align="left" height="22">
                                                        <asp:Label ID="lblLastPwdChange" runat="server" Class="InfoBoxFlat" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trPwdNeverExpires" runat="server">
                                                    <td height="22" style="font-weight:bold;">
                                                        Kennwort läuft nie ab:
                                                    </td>
                                                    <td align="left" height="22">
                                                        <asp:CheckBox ID="cbxPwdNeverExpires" runat="server" Enabled="False"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="22" style="font-weight:bold;">
                                                        fehlgeschlagene Anmeldungen:
                                                    </td>
                                                    <td align="left" height="22">
                                                        <asp:Label ID="lblFailedLogins" runat="server" Class="InfoBoxFlat" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="22" style="font-weight:bold;">
                                                        Konto gesperrt:
                                                    </td>
                                                    <td align="left" height="22">
                                                        <asp:CheckBox ID="cbxAccountIsLockedOut" runat="server" Enabled="False"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="22" style="font-weight:bold;">
                                                        Kennwort:&nbsp;
                                                    </td>
                                                    <td align="left" height="22">
                                                        &nbsp;
                                                        <asp:Label ID="txtPassword" runat="server" Class="InfoBoxFlat" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="22">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td align="left" height="22">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr id="trMail" runat="server">
                                                    <td height="22" style="font-weight:bold;">
                                                        Angelegt:
                                                    </td>
                                                    <td height="22">
                                                        <asp:Label ID="txtCreated" runat="server" Class="InfoBoxFlat" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="22" style="font-weight:bold;">
                                                        Letzte Änderung:
                                                    </td>
                                                    <td height="22">
                                                        <asp:Label ID="txtLastChange" runat="server" Class="InfoBoxFlat" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="22" style="font-weight:bold;">
                                                        Änderungsdatum:
                                                    </td>
                                                    <td height="22">
                                                        <asp:Label ID="txtLastChanged" runat="server" Class="InfoBoxFlat" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="22" style="font-weight:bold;">
                                                        Änderer:
                                                    </td>
                                                    <td height="22">
                                                        <asp:Label ID="txtLastChangedBy" runat="server" Class="InfoBoxFlat" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="22" style="font-weight:bold;">
                                                        Gelöscht:
                                                    </td>
                                                    <td height="22">
                                                        <asp:CheckBox ID="cbxDeleted" runat="server" Enabled="False"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="22" style="font-weight:bold;">
                                                        Löschdatum:
                                                    </td>
                                                    <td height="22">
                                                        <asp:Label ID="txtDeleteDate" runat="server" Class="InfoBoxFlat" Width="160px"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="33%"></td>
                                    </tr>
                                </table>
                                &nbsp;
                                <div style="text-align:right;Padding:10px;">
                                    <asp:LinkButton ID="lbtnCancel" runat="server" Text="Zurück" CssClass="Tablebutton" Width="78px" Visible="true">Zurück</asp:LinkButton>                                    
                                </div>                                                                
							</div> 
							<div>&nbsp;</div>                           
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>