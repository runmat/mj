<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change09_2.aspx.vb" Inherits="AppEC.Change09_2" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>

<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center">
             <tbody>
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Anzeige Report)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="100%"
                                    border="0">
                                    <tr>
                                        <td class="TaskTitle">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="center" style="padding-bottom:2px" width="150">
                                            <asp:LinkButton ID="cmdConfirm" runat="server" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="center" style="padding-bottom:2px" width="150">
                                            <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton" > &#149;&nbsp;Zurück</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="TaskTitle" colspan="3">
                                            <asp:HyperLink ID="lnkKreditlimit" runat="server" CssClass="TaskTitle" Visible="False"
                                                NavigateUrl="Change09.aspx">Zusammenstellung von Abfragekriterien</asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3">
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="TaskTitle">
                                                            <asp:HyperLink ID="lnkExcel" runat="server" CssClass="TaskTitle" Visible="False"
                                                                Target="_blank">Excelformat</asp:HyperLink>&nbsp;
                                                            <asp:Label ID="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:Label>
                                                        </td>
                                    </tr>
                                </table>
                                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>&nbsp;
                            </td>
                            </tr>
                            <tr>
                                <td class="" width="100%">
                                    <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="right" colspan="2">
                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False"
                                        PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain"
                                        bodyHeight="400" AllowSorting="True" AllowPaging="True" Width="100%">
                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                        <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="Unfallnummer" SortExpression="Unfallnummer"
                                                HeaderText="Unfallnummer"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Stornieren">
                                                <HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxStorno" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Status")="" %>'></asp:CheckBox>  
                                                    <asp:Label ID="lblStorno" runat="server" Text="Storniert" Visible='<%# DataBinder.Eval(Container, "DataItem.Status")="S" %>'></asp:Label>
                                                    <asp:Label ID="lblFehler" runat="server" Text="Fehler!" Visible='<%# DataBinder.Eval(Container, "DataItem.Status")="F" %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Stornotext">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtStorno" Width="180px" MaxLength="60" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Status")="" %>'></asp:TextBox>
                                                  <asp:Label ID="lblStornotext" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Stornotext") %>'  Visible='<%# DataBinder.Eval(Container, "DataItem.Status")="S" %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                             
                                            <asp:BoundColumn DataField="Anlagedatum" SortExpression="Anlagedatum"
                                                DataFormatString="{0:dd.MM.yyyy}" HeaderText="Anlagedatum">
                                            </asp:BoundColumn> 
                                            <asp:BoundColumn DataField="Webuser" SortExpression="Webuser" HeaderText="Webuser">
                                            </asp:BoundColumn>                                         
                                            <asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen">
                                            </asp:BoundColumn>                                            
                                            <asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer"
                                                HeaderText="Fahrgestellnummer"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Erstzulassungsdatum" SortExpression="Erstzulassungsdatum"
                                                DataFormatString="{0:dd.MM.yyyy}" HeaderText="Erstzulassungsdatum">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Kennzeicheneingang" SortExpression="Kennzeicheneingang"
                                                DataFormatString="{0:dd.MM.yyyy}" HeaderText="Kennzeicheneingang">
                                            </asp:BoundColumn>                                        
                                            <asp:BoundColumn DataField="Station" SortExpression="Station" HeaderText="Station">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Mahnstufe" SortExpression="Mahnstufe" HeaderText="Mahnstufe">
                                            </asp:BoundColumn>
                                        </Columns>
                                        <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                            HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                </td>

                            </tr>
                    </table>
                    <p align="right">
                        &nbsp;</p>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <p align="right">
                        &nbsp;</p>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <!--#include File="../../../PageElements/Footer.html" -->
                </td>
            </tr>
        </table>
                </td>
            </tr>
            </tbody>
         <script language="JavaScript" type="text/javascript">
          function enableTextbox(Checkbox, Textbox)
            {
                var formCheckBox = document.getElementById(Checkbox);
                var formTextBox = document.getElementById(Textbox);
                
                if (formCheckBox.checked==true) {
                    
                    formTextBox.disabled="";
                    formTextBox.focus();
                }
                else
                {
                    formTextBox.disabled = "disabled";
                    formTextBox.value = "";
                }
            }
        </script> 
        </table>

       
    </form>
</body>
</html>
