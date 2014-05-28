<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02s_0.aspx.vb" Inherits="CKG.Components.ComCommon.Beauftragung2.Change02s_0"
    MasterPageFile="../../../MasterPage/Services.Master" %>
    
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/Services/PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div>
                        <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    </div>
                    <div id="Search" runat="Server" style="width: 100%">
                        <asp:Panel ID="pnlDummy" runat="server" DefaultButton="btnDummy">
                            <div style="display: none">
                                <asp:Button ID="btnDummy" runat="server" />
                            </div>
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                            <asp:Label ID="lblKunde" runat="server" Text="Vorgang-ID"></asp:Label>
                                        </td>
                                        <td class="active" align="left" nowrap="nowrap" style="width: 103%">
                                            <asp:TextBox ID="txtID" runat="server" Width="100px" CssClass="InputSolid" TabIndex="1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="2" style="width: 100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbSearch" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                    TabIndex="2">» Suchen</asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </div>
                    <div id="Result" runat="Server" style="width: 100%" Visible="False">
                        <div id="pagination">
                          <uc2:GridNavigation ID="GridNavigation1" runat="server"/>
                        </div>
                        <div id="data">
                            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                CellPadding="0" GridLines="None" CssClass="GridView" PageSize="20" AllowPaging="True" AllowSorting="True">
                                <PagerSettings Visible="False" />
                                <HeaderStyle CssClass="GridTableHead" ForeColor="White" Wrap="False" />
                                <AlternatingRowStyle BackColor="#DEE1E0" />
                                <RowStyle CssClass="ItemStyle" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderStyle Width="35px"/>
                                        <ItemStyle Wrap="False"/>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" CommandName="Bearbeiten" CommandArgument='<%# Eval("ZULBELN") %>' Width="16px" Height="16px" 
                                                ImageUrl="../../../Images/edit_01.gif" ToolTip="Bearbeiten"/>
                                            <asp:ImageButton runat="server" CommandName="Zuruecksetzen" CommandArgument='<%# Eval("ZULBELN") %>' Width="16px" Height="16px" 
                                                ImageUrl="../../../Images/reactivate.png" ToolTip="Zurücksetzen" style="margin-left: 3px"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ZULBELN" SortExpression="ZULBELN" HeaderText="ID">
                                        <HeaderStyle Width="75px"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NAME1" SortExpression="NAME1" HeaderText="Kunde">
                                        <HeaderStyle Width="110px"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MAKTX" SortExpression="MAKTX" HeaderText="Dienstleistung">
                                        <HeaderStyle Width="100px"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ZZZLDAT" SortExpression="ZZZLDAT" HeaderText="Zul.datum" DataFormatString="{0:dd.MM.yyyy}">
                                        <HeaderStyle Width="60px"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="KREISKZ" SortExpression="KREISKZ" HeaderText="StVA">
                                        <HeaderStyle Width="30px"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ZZREFNR1" SortExpression="ZZREFNR1" HeaderText="Referenz 1">
                                        <HeaderStyle Width="100px"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ZZREFNR2" SortExpression="ZZREFNR2" HeaderText="Referenz 2">
                                        <HeaderStyle Width="125px"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ZZKENN" SortExpression="ZZKENN" HeaderText="Kennzeichen">
                                        <HeaderStyle Width="85px"/>
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <div>
                                &nbsp;
                            </div>
                        </div>
                        <div id="dataFooter">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
