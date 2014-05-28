<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="GridNavigation.ascx.cs" Inherits="Kantine.GridNavigation" %>
<link href="../Styles/GridNavigation.css" media="screen, projection" type="text/css"
        rel="stylesheet" />
     
<table cellpadding="0" cellspacing="0" id="tableLinks" runat="server" width="100%" bgcolor="#DFDFDF">
    <tbody>
        <tr align="center">
            <td class="pagination" nowrap="nowrap" style="padding: 0px 0px 0px 3px;">                
                <asp:LinkButton ID="lbtnPrevious" runat="server" onclick="lbtnPrevious_Click" CssClass="paginationText">&#171;&nbsp;</asp:LinkButton>
                <asp:LinkButton ID="lbtnPrevious10" Visible="false" runat="server" 
                    onclick="lbtnPrevious10_Click" CssClass="paginationText">&nbsp;...</asp:LinkButton>
                <asp:Repeater ID="Repeater1" runat="server" >
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnPage" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Index")%>'
                            runat="server" CssClass="paginationText"><%# DataBinder.Eval(Container, "DataItem.Page")%></asp:LinkButton>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:LinkButton ID="lbtnNext10" Visible="false" runat="server" 
                    onclick="lbtnNext10_Click" CssClass="paginationText">&nbsp;...</asp:LinkButton>
                <asp:LinkButton ID="lbtnNext" runat="server" onclick="lbtnNext_Click" CssClass="paginationText">&nbsp;&#187;</asp:LinkButton>
            </td>
            <td class="Title" nowrap="nowrap" width="100%">
                <asp:Label ID="lbltitle" Font-Bold="True" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblAnzahl" runat="server"></asp:Label>&nbsp;
            </td>
           
            <td class="paginationForm">
                <asp:DropDownList ID="ddlPageSize" Font-Names="Verdana,sans-serif" Font-Size="10px"
                    runat="server" AutoPostBack="True" Width="45px" 
                    onselectedindexchanged="ddlPageSize_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="paginationText" nowrap="nowrap" align="center" style="padding: 0px 3px 0px 3px;">
               Datensätze/Seite
            </td>
        </tr>
    </tbody>
</table>
