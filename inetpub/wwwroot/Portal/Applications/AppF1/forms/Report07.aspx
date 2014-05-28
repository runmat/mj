<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report07.aspx.vb" Inherits="AppF1.Report07" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style1
        {
            text-align: left;
        }
        #Table3
        {
            margin-left: 0px;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server" enctype="multipart/form-data">
    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>
    <table width="100%" align="center">
        <tr>
            <td colspan="2">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="3">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" runat="server"> (Download Zip-Datei)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" valign="top" width="120">
                            &nbsp;
                        </td>
                        <td class="TaskTitle" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TextHeader" width="150">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton">Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="style1">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        
                                </tr>
                        <tr>
                            <td class="LabelExtraLarge">
                                Zum Download bitte <em>linke</em> Maustaste benutzen.
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grvFiles" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:GridView ID="grvFiles" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            CssClass="tableMain" UseAccessibleHeader="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Dateiname" SortExpression="FileName">
                                                    <ItemTemplate>
                                                        <img alt="" src="../../../images/excel.gif" style="width: 20px; height: 20px" />&nbsp;
                                                        <asp:LinkButton ID="lbtFilename" runat="server" CommandName="open" ForeColor="Blue"
                                                            Height="11px" Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ChangeDate" HeaderText="Änderungsdatum" SortExpression="ChangeDate">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridTableHead" />
                                        </asp:GridView>
                                        <asp:Literal ID="Literal1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Link") %>'></asp:Literal>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>                                
                             </table>
                        </td>
</tr>
                </table>
            </td>
        </tr>
        <tr>
            <td width="50%" >
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                
            </td>
            <td width="50%" >
                 <!--#include File="../../../PageElements/Footer.html" --></td>
        </tr>
    </table>
    </td></tr> </table>
    </form>
</body>
</html>
