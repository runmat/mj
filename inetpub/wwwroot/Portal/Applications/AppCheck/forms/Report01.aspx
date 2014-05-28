<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01.aspx.vb" Inherits="AppCheck.Report01" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:styles id="ucStyles" runat="server">
    </uc1:styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
        <tbody>
            <tr>
                <td>
                    <uc1:header id="ucHeader" runat="server">
                    </uc1:header>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="PageNavigation" colspan="2">
                                    <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="PageNavigation" colspan="2">
                                </td>
                            </tr>
                            <tr>
                        <td valign="top" style="width:140px">
                            <table id="Table2" cellspacing="0" cellpadding="0" style="width:140px"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" >
                                        &nbsp;
                                    </td>
                                </tr>
                           
                                <tr>
                                    <td valign="middle">
                                         &nbsp;
                                    </td>
                                </tr>
 
                            </table>  
                        </td>
                        <td valign="top">
                                    <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    </table>
                                    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td valign="top" align="left">
                                                    <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                                        <tr>
                                                            <td class="TaskTitle">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>                                                                                                                                                                        
                                                        <tr>
                                                            <td>
                                                                <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" headerCSS="tableHeader"
                                                                    bodyCSS="tableBody" CssClass="tableMain" AllowSorting="True" AutoGenerateColumns="False">
                                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:TemplateColumn SortExpression="Filename" HeaderText="col_Dokument">
                                                                            <ItemStyle Width="40%"></ItemStyle>
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="Linkbutton1" runat="server" CommandArgument="Filename" CommandName="Sort">Dokument</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="lbtPDF" runat="server" CommandName="open" Height="18px" Visible="False"
                                                                                    ImageUrl="../../../images/pdf.gif" ToolTip="PDF"></asp:ImageButton>
                                                                                <asp:ImageButton ID="lbtExcel" runat="server" CommandName="open" Height="18px" Visible="False"
                                                                                    ImageUrl="../../../Images/excel.gif" ToolTip="Excel"></asp:ImageButton>
                                                                                <asp:ImageButton ID="lbtWord" runat="server" CommandName="open" Height="18px" ImageUrl=".../../../Images/Word_Logo.jpg"
                                                                                    Visible="False" ToolTip="Word" />
                                                                                <asp:ImageButton ID="lbtJepg" runat="server" CommandName="open" Height="18px" ImageUrl="../../../Images/jpg-file.png"
                                                                                    ToolTip="JPG" Visible="False" />
                                                                                <asp:ImageButton ID="lbtGif" runat="server" CommandName="open" Height="18px" ImageUrl="../../../Images/Gif_Logo.gif"
                                                                                    ToolTip="GIF" Visible="False" />
                                                                                <asp:ImageButton ID="lbtZip" runat="server" CommandName="open" Height="18px" ImageUrl="../../../Images/Zip.gif"
                                                                                    ToolTip="GIF" Visible="False" />                                                                                    
                                                                                <asp:LinkButton ID="Linkbutton2" runat="server" Width="229px" Height="18px" CommandName="open"
                                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' 
                                                                                    ForeColor="Blue"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn SortExpression="Filedate" HeaderText="col_Zeit">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Zeit" runat="server" CommandArgument="Filedate" CommandName="Sort">Letzte Änderung</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Filedate") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn Visible="False" DataField="Serverpfad" SortExpression="Serverpfad">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn Visible="False" DataField="Pattern" SortExpression="Pattern"></asp:BoundColumn>
                                                                    </Columns>
                                                                    <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                                        PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
                                                                </asp:DataGrid>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    &nbsp;
                                </td>
                                <td valign="top">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                   
                                </td>
                                <td align="left">
                                    <!--#include File="../../../PageElements/Footer.html" -->
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                </td>
                            </tr>

                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
    
</body>
</html>
