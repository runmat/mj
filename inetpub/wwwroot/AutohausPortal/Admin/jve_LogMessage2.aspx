<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="jve_LogMessage2.aspx.vb"
    Inherits="Admin.jve_LogMessage2" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/PortalZLD/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery.ui.datepicker-de.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery.scrollTo-1.4.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
            $("#ctl00_ContentPlaceHolder1_txrDatumVon").unbind();
            $("#ctl00_ContentPlaceHolder1_txtDatumBis").unbind();
            $("#ctl00_ContentPlaceHolder1_txtDatumVon").datepicker();
            $("#ctl00_ContentPlaceHolder1_txtDatumBis").datepicker();
            $("#ctl00_ContentPlaceHolder1_txtDatumBis").keypress();
        }

    </script>
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <div id="Result" runat="Server" visible="false">
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <asp:GridView ID="dgSearchResult" Width="100%" runat="server" AllowSorting="True"
                                        AutoGenerateColumns="False" CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0"
                                        AllowPaging="True" GridLines="None" PageSize="20" EditRowStyle-Wrap="False" PagerStyle-Wrap="True"
                                        CssClass="GridView">
                                        <PagerSettings Visible="False" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" Wrap="False" />
                                        <Columns>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="creationDate" SortExpression="creationDate" HeaderStyle-Width="60px"
                                                HeaderText="Erstellt"></asp:BoundField>
                                            <asp:BoundField DataField="activeDateFrom" SortExpression="activeDateFrom" HeaderStyle-Width="60px"
                                                HeaderText="Datum von" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundField>
                                            <asp:BoundField DataField="activeDateTo" SortExpression="activeDateTo" HeaderStyle-Width="60px"
                                                HeaderText="Datum bis" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundField>
                                            <asp:BoundField DataField="activeTimeFrom" SortExpression="activeTimeFrom" HeaderStyle-Width="45px"
                                                HeaderText="Zeit von" DataFormatString="{0:HH:mm}"></asp:BoundField>
                                            <asp:BoundField DataField="activeTimeTo" SortExpression="activeTimeTo" HeaderStyle-Width="45px"
                                                HeaderText="Zeit bis" DataFormatString="{0:HH:mm}"></asp:BoundField>
                                            <asp:BoundField DataField="titleText" SortExpression="titleText" HeaderStyle-Width="130px"
                                                HeaderText="Betreff"></asp:BoundField>
                                            <asp:TemplateField SortExpression="active" HeaderText="Aktiviert" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TextBox3" runat="server" Font-Bold="True" Width="100%" Visible='<%# DataBinder.Eval(Container, "DataItem.active") %>'
                                                        ReadOnly="True" BorderStyle="Groove" BorderColor="Transparent" BorderWidth="1px"
                                                        ForeColor="Black" BackColor="#2BD514">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.active") %>'>
                                                    </asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="active" HeaderText="Bei jedem&lt;br&gt;Seitenw."
                                                HeaderStyle-Width="60px">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Textbox2" runat="server" Font-Bold="True" Width="100%" Visible='<%# DataBinder.Eval(Container,"DataItem.messageColor") %>'
                                                        ReadOnly="True" BorderStyle="Groove" BorderColor="Transparent" BorderWidth="1px"
                                                        ForeColor="Black" BackColor="#2BD514">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="Textbox4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.messageColor") %>'>
                                                    </asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="active" HeaderText="Login&lt;br&gt;CKE-User" HeaderStyle-Width="60px">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Textbox5" runat="server" Font-Bold="True" Visible='<%# DataBinder.Eval(Container, "DataItem.onlyTEST") %>'
                                                        Width="100%" ReadOnly="True" BorderColor="Transparent" BorderStyle="Groove" BorderWidth="1px"
                                                        BackColor="#2BD514" ForeColor="Black">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="Textbox6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.onlyTEST") %>'>
                                                    </asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="active" HeaderText="Login&lt;br&gt;CKP-User" HeaderStyle-Width="60px">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Textbox7" runat="server" Font-Bold="True" Visible='<%# DataBinder.Eval(Container, "DataItem.onlyPROD") %>'
                                                        Width="100%" ReadOnly="True" BorderColor="Transparent" BorderStyle="Groove" BorderWidth="1px"
                                                        BackColor="#2BD514" ForeColor="Black">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="Textbox8" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.onlyPROD") %>'>
                                                    </asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnSelect" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>'
                                                        ImageUrl="Images/Edit.gif" CausesValidation="false"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            <div id="dataQueryFooter">
                                    <asp:LinkButton ID="lbtn_New" runat="server" Text="Neue Nachricht&amp;nbsp;&amp;#187; "
                                            CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                            </div>
                            </div>
                            <div id="Input" runat="server" visible="False">
                                <div id="adminInput">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td nowrap="nowrap" class="firstLeft active">
                                                </td>
                                                <td nowrap="nowrap" class="firstLeft active" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Kunde:
                                                </td>
                                                <td nowrap="nowrap" valign="top" class="firstLeft active">
                                                    <asp:DropDownList ID="ddlKunde" runat="server" AutoPostBack="True" Font-Names="Verdana,sans-serif"
                                                        Width="260px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Datum&nbsp;Anzeige (tt.mm.jjjj)* von&nbsp;- bis
                                                </td>
                                                <td valign="middle" nowrap="nowrap" class="firstLeft active" style="width: 489px">
                                                    <asp:TextBox ID="txtDatumVon" runat="server" CssClass="InputTextbox" Width="150px"></asp:TextBox>
                                                    <asp:TextBox ID="txtDatumBis" runat="server" CssClass="InputTextbox" Width="150px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Zeit Anzeige (hh:mm)* von&nbsp;- bis
                                                </td>
                                                <td valign="middle" nowrap="nowrap" class="firstLeft active" style="width: 489px">
                                                    <asp:TextBox ID="txtZeitVon" TabIndex="3" runat="server" CssClass="InputTextbox"
                                                        Width="150px"></asp:TextBox>
                                                    <asp:TextBox ID="txtZeitBis" runat="server" CssClass="InputTextbox" TabIndex="3"
                                                        Width="150px"></asp:TextBox>
                                                    <asp:DropDownList ID="ddlTime" runat="server" AutoPostBack="True" Visible="False">
                                                    </asp:DropDownList>
                                                    <asp:CheckBox ID="cbxAlterTime" runat="server" Enabled="False" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" align="left" width="207">
                                                    <p>
                                                        Betreff&nbsp; (bis 50 Zeichen)</p>
                                                </td>
                                                <td class="firstLeft active" align="left">
                                                    <asp:TextBox ID="txtBetreff" TabIndex="5" runat="server" Width="450px" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" align="left" width="207" height="72">
                                                    <p>
                                                        Nachricht (bis 500 Zeichen)</p>
                                                </td>
                                                <td class="firstLeft active" align="left" height="72">
                                                    <p>
                                                        <asp:TextBox ID="txtMessage" TabIndex="6" runat="server" Width="450px" MaxLength="500"
                                                            TextMode="MultiLine" Rows="4"></asp:TextBox></p>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" align="left" width="207">
                                                    <asp:CheckBox ID="cbxLogin" runat="server" Visible="False" Text="Anmeldung zulassen für"
                                                        Enabled="False" Checked="True"></asp:CheckBox>
                                                </td>
                                                <td class="firstLeft active" align="left">
                                                    <span>
                                                        <asp:CheckBox ID="cbxActive" runat="server" Text="Aktiv" Checked="True"></asp:CheckBox>
                                                        <asp:CheckBox ID="cbxAny" runat="server" Text="Onlinezeitraum" Font-Underline="True"
                                                            ForeColor="Transparent"></asp:CheckBox><br>
                                                        <br>
                                                        Anmeldung zulassen für
                                                        <asp:CheckBox ID="cbxActiveOld" runat="server" Visible="False" Text="Aktiv" Checked="True">
                                                        </asp:CheckBox>
                                                        <asp:CheckBox ID="cbxAll" runat="server" Visible="False"></asp:CheckBox>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtid" runat="server" Visible="False" Height="18px" Width="71px"
                                                        DESIGNTIMEDRAGDROP="1031"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <p>
                                                        <span>
                                                            <asp:CheckBox ID="cbxTest" runat="server" Text="TEST-Benutzer" Checked="True"></asp:CheckBox><br>
                                                            <asp:CheckBox ID="cbxProd" runat="server" Text="PROD-Benutzer"></asp:CheckBox><br>
                                                            <asp:CheckBox ID="CheckBox1" runat="server" Text="DAD-Admin" Enabled="False" Checked="True">
                                                            </asp:CheckBox></span></p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <hr width="100%" size="1">
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <u>Formatierungsanweisungen</u>
                                                </td>
                                                <td class="firstLeft active" width="100%" style="font-size: 12px">
                                                    Text in rot:<font face="Courier New" size="2"> {c="#FF0000"}Text...{/c}</font>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td>
                                                </td>
                                                <td class="firstLeft active" style="font-size: 12px">
                                                    Text fett: <font face="Courier New" size="2">{b}Text...{/b}</font>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td>
                                                </td>
                                                <td class="firstLeft active" style="font-size: 12px">
                                                    Text kursiv: <font face="Courier New" size="2">{i}Text...{/i}</font>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td>
                                                </td>
                                                <td class="firstLeft active" style="font-size: 12px">
                                                    Hyperlink: <font face="Courier New" size="2">&nbsp;{h}Text...{/h} (nicht in der Betreffzeile)</font>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td>
                                                </td>
                                                <td class="firstLeft active" style="font-size: 12px">
                                                    Neue Zeile: <font face="Courier New" size="2">&nbsp;{br/}</font>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    * Pflichtfelder
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                    <div id="dataFooter">
                                        <asp:LinkButton ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                            CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnDelete2" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                            CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                            CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                            CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton></div>
                                </div>
                            </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div></div>
</asp:Content>
