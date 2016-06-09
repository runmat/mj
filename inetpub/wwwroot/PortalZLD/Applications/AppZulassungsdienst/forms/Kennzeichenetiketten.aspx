<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kennzeichenetiketten.aspx.cs"
    Inherits="AppZulassungsdienst.forms.Kennzeichenetiketten" MasterPageFile="../MasterPage/App.Master" %>
    
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <script language="javascript" type="text/javascript">
        function checkZulassungsdatum() {
            var tb = document.getElementById('<%= txtZulDate.ClientID %>');
            document.getElementById('<%= ihDatumIstWerktag.ClientID %>').value = nurWerktage(tb.value)[0];
            return true;
        }
    </script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" onclick="lb_zurueck_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div>
                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        Neue Abfrage starten
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="NewSearch" runat="server" 
                                                ImageUrl="../../../Images/queryArrow.gif" onclick="NewSearch_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                        <div id="TableQuery" style="margin-bottom: 10px">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblDatum" runat="server">Datum der Zulassung:</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtZulDate" runat="server" CssClass="TextBoxNormal" Width="75px"
                                                MaxLength="6"></asp:TextBox>
                                            <asp:Label ID="txtZulDateFormate" Style="padding-left: 2px; font-weight: normal"
                                                Height="15px" runat="server">(ttmmjj)</asp:Label>
                                            <asp:LinkButton runat="server" Style="padding-left: 10px; font-weight: normal" Height="15px"
                                                ID="lbtnGestern" Text="Gestern |" Width="60px" />
                                            <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnHeute"
                                                Width="50px" Text="Heute |" />
                                            <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnMorgen"
                                                Width="60px" Text="Morgen" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="height: 36px">
                                            <asp:Label ID="lblbID" runat="server">ID:</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="height: 36px">
                                            <asp:TextBox ID="txtID" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="8" Width="75px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="height: 36px">
                                            <asp:Label ID="lblKennzeichen" runat="server">Kennzeichen:</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="height: 36px">
                                            <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal TextUpperCase" 
                                                MaxLength="20" Width="125px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="height: 36px">
                                            <asp:Label ID="lblDruckerpositionZeile" runat="server">Druck ab Zeile:</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="height: 36px">
                                            <asp:TextBox ID="txtDruckerpositionZeile" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="1" Width="25px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="height: 36px">
                                            <asp:Label ID="lblDruckerpositionSpalte" runat="server">Druck ab Spalte:</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="height: 36px">
                                            <asp:TextBox ID="txtDruckerpositionSpalte" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="1" Width="25px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="padding-top: 0px">
                                            <asp:Label ID="lblDarstellung" runat="server">Darstellung:</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-top: 0px">
                                            <asp:RadioButtonList ID="rbAnsicht" Width="250px" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="0" Selected="True" Text="Delta-Liste"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Gesamt-Liste"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="2">
                                            <asp:ImageButton
                                                ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                Width="1px" onclick="cmdCreate_Click" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="cmdCreate_Click" OnClientClick="checkZulassungsdatum();">» Weiter </asp:LinkButton>
                    </div>
                    <asp:Panel ID="Panel2" DefaultButton="btnEmpty2" runat="server" Visible="False">
                        <div id="Result" runat="Server">
                            <div id="data">
                                <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                    <tr>
                                        <td>
                                            <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="10" AllowPaging="true" AllowSorting="true" 
                                                AutoGenerateColumns="False" GridLines="None" OnNeedDataSource="rgGrid1_NeedDataSource" Skin="Default">
                                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                                <ItemStyle CssClass="ItemStyle" />
                                                <AlternatingItemStyle CssClass="ItemStyle" />
                                                <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="true" DataKeyNames="SapId">
                                                    <SortExpressions>
                                                        <telerik:GridSortExpression FieldName="SapId" SortOrder="Ascending" />
                                                    </SortExpressions>
                                                    <HeaderStyle ForeColor="White" />
                                                    <FooterStyle BackColor="#FFB27F" HorizontalAlign="Right" Wrap="false" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="Auswahl" HeaderStyle-Width="60px">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbxAuswahl" runat="server" Checked='<%# (bool)Eval("IsSelected") %>' AutoPostBack="True" OnCheckedChanged="cbxAuswahl_CheckedChanged"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn SortExpression="SapId" DataField="SapId" HeaderText="ID" HeaderStyle-Width="70px"/>
                                                        <telerik:GridBoundColumn SortExpression="KundenNr" DataField="KundenNr" HeaderText="Kunnr." HeaderStyle-Width="80px"/>
                                                        <telerik:GridBoundColumn SortExpression="KundenName" DataField="KundenName" HeaderText="Kunde"/>
                                                        <telerik:GridBoundColumn SortExpression="Zulassungsdatum" DataField="Zulassungsdatum" HeaderText="Zul.datum" DataFormatString="{0:dd.MM.yyyy}" HeaderStyle-Width="80px"/>
                                                        <telerik:GridBoundColumn SortExpression="Kennzeichen" DataField="Kennzeichen" HeaderText="Kennzeichen" HeaderStyle-Width="95px"/>
                                                        <telerik:GridBoundColumn SortExpression="Referenz1" DataField="Referenz1" HeaderText="Referenz 1"/>
                                                        <telerik:GridBoundColumn SortExpression="Referenz2" DataField="Referenz2" HeaderText="Referenz 2"/>
                                                        <telerik:GridBoundColumn SortExpression="Fahrzeugtyp" DataField="Fahrzeugtyp" HeaderText="Fahrzeugtyp" HeaderStyle-Width="85px"/>
                                                        <telerik:GridBoundColumn SortExpression="Farbe" DataField="Farbe" HeaderText="Farbe" HeaderStyle-Width="75px"/>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td>
                                            <asp:ImageButton
                                                ID="btnEmpty2" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                Width="1px" onclick="cmdPrint_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="dataFooter">
                        <asp:LinkButton ID="cmdPrint" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdPrint_Click" Visible="False">» Drucken </asp:LinkButton>
                    </div>
                    <input type="hidden" runat="server" id="ihDatumIstWerktag" value="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
