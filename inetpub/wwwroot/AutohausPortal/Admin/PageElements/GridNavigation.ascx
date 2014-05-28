<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GridNavigation.ascx.vb"
    Inherits="Admin.GridNavigation" %>

        <table cellpadding="0" cellspacing="0" id="tableLinks" runat="server" width="100%"
            bgcolor="#DFDFDF">
            <tbody>

                <tr>
                    <td class="pagination" nowrap="nowrap">
                        <asp:LinkButton ID="lbtnPrevious" runat="server">&#171;&nbsp;</asp:LinkButton>
                        <asp:LinkButton ID="lbtnPrevious10" Visible="false" runat="server">&nbsp;...</asp:LinkButton>
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnPage" OnClick="lbtnPage_PageIndexChanging" CommandArgument='<%#DataBinder.Eval(Container, "DataItem.Index")%>'
                                    runat="server"><%#DataBinder.Eval(Container, "DataItem.Page")%></asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:LinkButton ID="lbtnNext10" Visible="false" runat="server">&nbsp;...</asp:LinkButton>
                        <asp:LinkButton ID="lbtnNext" runat="server">&nbsp;&#187;</asp:LinkButton>
                    </td>
                    <td class="Title" nowrap="nowrap">
                        <asp:Label ID="lbltitle"  Font-Bold="True" runat="server" Visible="False" ></asp:Label>
                    </td> 
                    <td class="paginationText" nowrap="nowrap">
                        <asp:Label ID="lblAnzahl" runat="server"></asp:Label>&nbsp;Datensätze/Seite
                    </td>
                    <td class="paginationForm">
                        <asp:DropDownList ID="ddlPageSize" Font-Names="Verdana,sans-serif" Font-Size="10px"
                            runat="server" AutoPostBack="True" Width="45px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>

