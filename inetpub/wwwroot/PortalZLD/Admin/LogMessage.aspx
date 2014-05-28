<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogMessage.aspx.vb" Inherits="Admin.LogMessage" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Reportname"></asp:Label>
                        </h1>
                    </div>

                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="TblSearch" runat="server" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td nowrap="nowrap" class="firstLeft active">
                                                <asp:textbox id="txtid" runat="server" Visible="False" Height="18px" Width="71px" DESIGNTIMEDRAGDROP="1031"></asp:textbox>
                                            </td>
                                            <td nowrap="nowrap" class="firstLeft active" width="100%">
                                            <div style="float:left">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </div>
                                            <asp:UpdatePanel ID="PLServerZeit" runat="server">
                                            <ContentTemplate>
                                                <div style="float:right">
	    											<asp:button id="Button1" runat="server" Height="20px" Width="114px" Text="Serverzeit" Font-Size="XX-Small"></asp:button>
    												<asp:textbox id="txtServerzeit" runat="server" Width="160px" BorderWidth="1px" BorderStyle="Solid" ReadOnly="True" Wrap="False">Serverzeit</asp:textbox>&nbsp;
                                                </div></ContentTemplate></asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Kunde:
                                            </td>
                                            <td nowrap="nowrap" valign="top" class="firstLeft active">
                                                <asp:DropDownList ID="ddlKunde" runat="server" Font-Names="Verdana,sans-serif">
                                                </asp:DropDownList>
                                                
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                               im Zeitraum von:
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox CssClass="InputTextbox" runat="server" Width="83px" ToolTip="Datum von"
                                                    ID="txtDateVon" MaxLength="10"  />
                                                <ajaxToolkit:CalendarExtender ID="defaultCalendarExtenderVon" runat="server" TargetControlID="txtDateVon" />
                                                &nbsp;bis:&nbsp;
                                                <asp:TextBox CssClass="InputTextbox" runat="server" Width="83px" ToolTip="Datum bis"
                                                    ID="txtDateBis" MaxLength="10"  />
                                                <ajaxToolkit:CalendarExtender ID="defaultCalendarExtenderBis" runat="server" TargetControlID="txtDateBis" />                                                
                                                <asp:checkbox id="cbxAlter" runat="server" Visible="False" Enabled="False"></asp:checkbox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                               Zeit Anzeige (hh:mm)*<br/>
												von&nbsp;- bis
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                    <asp:textbox id="txtZeitVon" tabIndex="3" runat="server" Width="150px" CssClass="InputTextbox"></asp:textbox>&nbsp;
                                                    <asp:textbox id="txtZeitBis" tabIndex="4" runat="server" Width="150px" ></asp:textbox>
													<asp:dropdownlist id="ddlTime" runat="server" Visible="False" AutoPostBack="True"></asp:dropdownlist>
													<asp:checkbox id="cbxAlterTime" runat="server" Visible="False" Enabled="False" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Technischer Name:
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox ID="txtTechName" runat="server" CssClass="InputTextbox">*</asp:TextBox>
                                                <asp:Label ID="lblTechName" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Betreff&nbsp; (bis 50 Zeichen)</td>
                                            <td valign="top" nowrap="nowrap" class="firstLeft active">
                                                <asp:textbox id="txtBetreff" tabIndex="5" runat="server" CssClass="InputTextbox" Width="450px" MaxLength="50"></asp:textbox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td  class="firstLeft active">
                                                Nachricht (bis 500 Zeichen)
                                            </td>
                                            <td  class="firstLeft active">
                                                <asp:textbox id="txtMessage" tabIndex="6" runat="server" Width="450px" CssClass="InputTextbox"  MaxLength="500" TextMode="MultiLine" Rows="4"></asp:textbox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <span><asp:checkbox id="cbxLogin" runat="server" Visible="False" Text="Anmeldung zulassen für" Enabled="False" Checked="True"></asp:checkbox></span></td>
                                            <td class="firstLeft active">
                                            	<span><asp:checkbox id="cbxActive" runat="server" Text="Aktiv" Checked="True"></asp:checkbox>
												<asp:checkbox id="cbxAny" runat="server" Text="Onlinezeitraum" ></asp:checkbox><br /><br />
												
												Anmeldung zulassen für
												<asp:checkbox id="cbxActiveOld" runat="server" Visible="False" Text="Aktiv" Checked="True"></asp:checkbox>
												<asp:checkbox id="cbxAll" runat="server" Visible="False"></asp:checkbox></span>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;</td>
                                            <td class="firstLeft active">
												<span>
												    <asp:checkbox id="cbxTest" runat="server" Text="TEST-Benutzer" Checked="True"></asp:checkbox><br />
													<asp:checkbox id="cbxProd" runat="server" Text="PROD-Benutzer"></asp:checkbox><br />
													<asp:checkbox id="CheckBox1" runat="server" Text="DAD-Admin" Enabled="False" Checked="True"></asp:checkbox>
                                                </span>
                                             </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <u>Formatierungsanweisungen</u></td>
                                            <td class="firstLeft active">
                                                <p>
                                                    <font size="1"><font size="2">Text in rot</font>: {c=&quot;#FF0000&quot;}Text...{/c}</font></p>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;</td>
                                            <td class="firstLeft active">
                                                    <p><font size="1"><font size="2">Text fett</font>: {b}Text...{/b}</font></p></td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;</td>
                                            <td class="firstLeft active">
                                                    <p><font size="1"><font size="2">Text kursiv</font>: {i}Text...{/i}</font></p></td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;</td>
                                            <td class="firstLeft active">
                                                    <p><font size="1"><font size="2">Hyperlink</font>:
                                                        {h}Text...{/h} </font></p></td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;</td>
                                            <td class="firstLeft active">
                                                    <p><font size="1"><font size="2">Neue Zeile</font>:
                                                        {br/}</font></p></td>
                                        </tr>                                                                                                                                                                
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;</td>
                                            <td class="firstLeft active">
                                                &nbsp;</td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>

                    <div id="dataQueryFooter">
                        <asp:Panel ID="ButtonPanel" runat="server">
                            <asp:LinkButton ID="btnSave" runat="server" CssClass="Tablebutton" Width="78px">» Speichern</asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="Tablebutton" Width="78px">» Löschen</asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="btnDelete2" runat="server" CssClass="Tablebutton" Width="78px">» Löschen</asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="btnCancel" runat="server" CssClass="Tablebutton" Width="78px">» Abbrechen</asp:LinkButton>
                        </asp:Panel>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">

                            &nbsp;</div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gridMain" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="id" HeaderText="id" Visible="False">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="creationDate" SortExpression="creationDate" HeaderText="Erstellt">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="activeDateFrom" SortExpression="activeDateFrom" HeaderText="Datum von"
                                                            DataFormatString="{0:dd.MM.yyyy}"></asp:BoundField>
                                                        <asp:BoundField DataField="activeDateTo" SortExpression="activeDateTo" HeaderText="Datum bis"
                                                            DataFormatString="{0:dd.MM.yyyy}"></asp:BoundField>
                                                        <asp:BoundField DataField="activeTimeFrom" SortExpression="activeTimeFrom" HeaderText="Zeit von"
                                                            DataFormatString="{0:HH:mm}"></asp:BoundField>
                                                        <asp:BoundField DataField="activeTimeTo" SortExpression="activeTimeTo" HeaderText="Zeit bis"
                                                            DataFormatString="{0:HH:mm}"></asp:BoundField>
                                                        <asp:TemplateField SortExpression="titleText" HeaderText="Betreff">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBetreff" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.titleText") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="active" HeaderText="Aktiviert">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Image ID="active" runat="server" Font-Bold="True" Width="16px" Visible='<%# DataBinder.Eval(Container, "DataItem.active") %>'
                                                                    ImageUrl="../images/Confirm_mini.gif" Height="16px"></asp:Image>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="Textbox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.active") %>'>
                                                                </asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="active" HeaderText="Bei jedem Seitenw.">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Image ID="activeSite" runat="server" Font-Bold="True" Width="16px" Visible='<%# DataBinder.Eval(Container, "DataItem.messageColor") %>'
                                                                    ImageUrl="../images/Confirm_mini.gif" Height="16px"></asp:Image>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="Textbox4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.messageColor") %>'>
                                                                </asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="active" HeaderText="Login&lt;br&gt;CKE-User">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Image ID="ckeUser" runat="server" Font-Bold="True" Width="16px" Visible='<%# DataBinder.Eval(Container, "DataItem.onlyTEST") %>'
                                                                    ImageUrl="../images/Confirm_mini.gif" Height="16px"></asp:Image>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="Textbox6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.onlyTEST") %>'>
                                                                </asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="active" HeaderText="Login&lt;br&gt;CKP-User">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Image ID="ckpUser" runat="server" Font-Bold="True" Width="16px" Visible='<%# DataBinder.Eval(Container, "DataItem.onlyPROD") %>'
                                                                    ImageUrl="../images/Confirm_mini.gif" Height="16px"></asp:Image>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="Textbox8" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.onlyPROD") %>'>
                                                                </asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Auswahl">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnSelect" runat="server" CommandName="Select" ImageUrl="../Images/Edit_01.gif"
                                                                    CausesValidation="false" Width="16px" Height="16px"></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
						<asp:literal id="litConfirm" runat="server"></asp:literal>
						<asp:literal id="serverzeit" runat="server"></asp:literal>
                    </div>
                </div>
                <div id="dataFooter">
                    <asp:LinkButton ID="btnNew" runat="server" CssClass="TablebuttonLarge" 
                        Width="120px" Height="16px">» Neue Nachricht</asp:LinkButton>
                    </div>
            </div>
        </div>
    </div>
</asp:Content>