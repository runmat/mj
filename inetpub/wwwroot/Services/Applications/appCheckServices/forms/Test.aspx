<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="appCheckServices.forms.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" bodyCSS="tableBody" CssClass="tableMain" 
            headerCSS="tableHeader" Width="100%">
            <AlternatingItemStyle CssClass="GridTableAlternate" />
            <HeaderStyle CssClass="GridTableHead" ForeColor="White" Wrap="False" />
            <Columns>
                <asp:TemplateColumn HeaderText="col_Dokument" SortExpression="Filename">
                    <ItemStyle Width="40%" />
                    <HeaderTemplate>
                        <asp:LinkButton ID="Linkbutton1" runat="server" CommandArgument="Filename" 
                            CommandName="Sort">Dokument</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="lbtPDF" runat="server" CommandName="open" Height="18px" 
                            ImageUrl="../../../images/pdf.gif" ToolTip="PDF" Visible="False" />
                        <asp:ImageButton ID="lbtExcel" runat="server" CommandName="open" Height="18px" 
                            ImageUrl="../../../Images/excel.gif" ToolTip="Excel" Visible="False" />
                        <asp:ImageButton ID="lbtWord" runat="server" CommandName="open" Height="18px" 
                            ImageUrl=".../../../Images/Word_Logo.jpg" ToolTip="Word" Visible="False" />
                        <asp:ImageButton ID="lbtJepg" runat="server" CommandName="open" Height="18px" 
                            ImageUrl="../../../Images/jpg-file.png" ToolTip="JPG" Visible="False" />
                        <asp:ImageButton ID="lbtGif" runat="server" CommandName="open" Height="18px" 
                            ImageUrl="../../../Images/Gif_Logo.gif" ToolTip="GIF" Visible="False" />
                        <asp:ImageButton ID="lbtZip" runat="server" CommandName="open" Height="18px" 
                            ImageUrl="../../../Images/Zip.gif" ToolTip="GIF" Visible="False" />
                        <asp:LinkButton ID="Linkbutton2" runat="server" CommandName="open" 
                            ForeColor="Blue" Height="18px" 
                            Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' Width="229px"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="col_Zeit" SortExpression="Filedate">
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Zeit" runat="server" CommandArgument="Filedate" 
                            CommandName="Sort">Letzte Änderung</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" 
                            Text='<%# DataBinder.Eval(Container, "DataItem.Filedate") %>'>
                                                                                </asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="Serverpfad" SortExpression="Serverpfad" 
                    Visible="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="Pattern" SortExpression="Pattern" Visible="False">
                </asp:BoundColumn>
            </Columns>
            <PagerStyle Font-Bold="True" Font-Size="12pt" HorizontalAlign="Left" 
                NextPageText="nächste&amp;gt;" PrevPageText="&amp;lt;vorherige" Wrap="False" />
        </asp:DataGrid>
    
    </div>
    </form>
</body>
</html>
