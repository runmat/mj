<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change06_2.aspx.vb" Inherits="AppF2.Change06_2"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbHaendlersuche" runat="server">Händlerauswahl</asp:LinkButton>
                &nbsp;</div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                    </div>
                    <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
                    <div id="TableQuery">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table id="tab1" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="ShowScript" runat="server" class="formquery">
                                            <td class="active" width="100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="Result" runat="Server">
                        <div id="pagination">
                            <%--<uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>--%>
                        </div>
                        <div id="data">
                            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                CellPadding="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0" AllowSorting="True"
                                AllowPaging="True" CssClass="GridView" PageSize="20">
                                <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                <PagerSettings Visible="False" />
                                <RowStyle CssClass="ItemStyle" />
                                <EmptyDataRowStyle BackColor="#DFDFDF" />
                                <Columns>
                                    <asp:BoundField Visible="False" DataField="Kreditkontrollbereich" HeaderText="Kreditkontrollbereich">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Kontingentart" HeaderText="Kontingentart">
                                    <HeaderStyle ForeColor="White" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Altes Kontingent">
                                        <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblKontingent_Alt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
                                            </asp:Label>
                                            <asp:Label ID="lblRichtwert_Alt" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Richtwert_Alt") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Ausschoepfung" HeaderText="Inanspruchnahme">
                                        <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Freies Kontingent">
                                        <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFrei" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gesperrt - Alt">
                                        <HeaderStyle HorizontalAlign="Center" ForeColor="White"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Gesperrt_Alt" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'
                                                Enabled="False"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Neues Kontingent">
                                        <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="Image2" runat="server" Width="12px" ImageUrl="/Services/Images/empty.gif"
                                                Height="12px"></asp:Image>
                                            <asp:TextBox ID="txtKontingent_Neu" runat="server" CssClass="InputRight" Width="50px"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Neu") %>'>
                                            </asp:TextBox>
                                            <asp:TextBox ID="txtRichtwert_Neu" runat="server" CssClass="InputRight" Width="50px"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.Richtwert_Neu") %>'>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gesperrt - Neu ">
                                        <HeaderStyle HorizontalAlign="Center" ForeColor="White"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="imgGesperrt_Neu" runat="server" Width="12px" ImageUrl="/Services/Images/empty.gif"
                                                Height="12px"></asp:Image>
                                            <asp:CheckBox ID="chkGesperrt_Neu" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Neu") %>'>
                                            </asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False" HeaderText="ZeigeKontingentart">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkZeigeKontingentart" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
                                            </asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div id="dataFooter">
                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px">» Sichern</asp:LinkButton>
                        <asp:LinkButton ID="cmdConfirm" runat="server" CssClass="Tablebutton" Width="78px">» Bestätigen</asp:LinkButton>
                        <asp:LinkButton ID="cmdReset" runat="server" CssClass="Tablebutton" Width="78px">» Verwerfen</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
