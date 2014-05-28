<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report016.aspx.vb" Inherits="CKG.Components.ComCommon.Report016" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI"  TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
    <meta content="JavaScript" name="vs_defaultClientScript"/>
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        #File1
        {
            width: 436px;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server" enctype="multipart/form-data">
    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>
        <table width="100%" align="center">
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%;">
                        <div class="PageNavigation">
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </div>
                        <div id="TableQuery">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                        </div>
                        <div id="Result" runat="Server" style="width: 100%">
                            <div style="display: none">
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                            </div>
                            <div id="data" runat="server">
                                <div>
                                    <telerik:RadGrid ID="rgDokumente" runat="server" PageSize="100" AllowPaging="true" AllowSorting="true" 
                                        AutoGenerateColumns="False" GridLines="None" CssClass="tableMain">
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                        <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="false">
                                            <SortExpressions>
                                                <telerik:GridSortExpression FieldName="DocTypeName" SortOrder="Ascending" />
                                                <telerik:GridSortExpression FieldName="LastEdited" SortOrder="Descending" />
                                            </SortExpressions>
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldName="DocTypeId"></telerik:GridGroupByField>
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="DocTypeId"></telerik:GridGroupByField>
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <Columns>
                                                <telerik:GridBoundColumn SortExpression="DocumentId" DataField="DocumentId" visible="false" />
                                                <telerik:GridTemplateColumn SortExpression="FileName" HeaderText="Dokument" HeaderButtonType="TextButton">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lbtPDF" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../../images/pdf.gif" 
                                                            ToolTip="PDF" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString() = "pdf" %>' />
                                                        <asp:ImageButton ID="lbtExcel" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../../Images/excel.gif" 
                                                            ToolTip="Excel" ImageAlign="Middle" Visible='<%# Eval("FileType").StartsWith("xls") %>' />
                                                        <asp:ImageButton ID="lbtWord" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../../Images/Word_Logo.jpg"
                                                            ToolTip="Word" ImageAlign="Middle" Visible='<%# Eval("FileType").StartsWith("doc") %>' />
                                                        <asp:ImageButton ID="lbtJepg" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../../Images/jpg-file.png"
                                                            ToolTip="JPG" ImageAlign="Middle" Visible='<%# Eval("FileType").StartsWith("jp") %>' />
                                                        <asp:ImageButton ID="lbtGif" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../../Images/Gif_Logo.gif"
                                                            ToolTip="GIF" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString() = "gif" %>' />                                                 
                                                        <span>
                                                            <asp:LinkButton ID="lbtDateiOeffnen" runat="server" CommandName="showDocument" Text='<%# Eval("FileName") %>' >
                                                            </asp:LinkButton>
                                                        </span>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn SortExpression="FileType" DataField="FileType" visible="false" />
                                                <telerik:GridBoundColumn SortExpression="DocTypeId" DataField="docTypeId" Visible="false" />
                                                <telerik:GridBoundColumn SortExpression="DocTypeName" HeaderText="Art" AllowSorting="false" 
                                                    DataField="DocTypeName" HeaderStyle-Width="200px" />
                                                <telerik:GridBoundColumn SortExpression="LastEdited" HeaderText="Letzte Änderung" HeaderButtonType="TextButton" 
                                                    DataField="LastEdited" DataFormatString="{0:dd.MM.yyyy HH:mm:ss}" HeaderStyle-Width="150px">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
