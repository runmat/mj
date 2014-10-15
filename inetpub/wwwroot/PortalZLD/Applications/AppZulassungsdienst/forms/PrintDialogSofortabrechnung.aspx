<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintDialogSofortabrechnung.aspx.cs" Inherits="AppZulassungsdienst.forms.PrintDialogSofortabrechnung" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="content-type"  content="text/html; charset=iso-8859-1" />
    <link href="/PortalZLD/Styles/js/jquery-ui.css" rel="stylesheet" type="text/css"/>
    <link href="/PortalZLD/Styles/js/jquery.ui.theme.css" rel="stylesheet" type="text/css"/>
</head>
    <script type="text/javascript">
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oWnd = GetRadWindow();
            oWnd.close();

        }
    </script>
    <body style="background-image:none!important; width: 520px ">
        <form id="form1" runat="server">
         <div id="maincontainer" style="width:100%;">
            <div id="content" style="width:100%; margin-left:0px;">
                <asp:GridView ID="GridView2" GridLines="None" Style="border: 1px solid #dfdfdf; width: 100%;
                    font-size: 12px; color: #595959" runat="server" BackColor="White" AutoGenerateColumns="False" OnRowCommand="GridView2_RowCommand"
                    CaptionAlign="Left">
                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                    <RowStyle CssClass="ItemStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="ID">
                            <ItemTemplate>
                                <asp:Label ID="lblIDPrint" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZULBELN") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Datei">
                            <ItemTemplate>
                                <asp:Label ID="lblFileName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FILENAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Aufrufen">
                            <ItemTemplate>
                                <asp:ImageButton ID="cmdPrint" CommandName="Print" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Path") %>'
                                    runat="server" Width="32" Height="32" ImageUrl="~/Images/pdf-logo.png" />
                            </ItemTemplate>
                            <HeaderStyle Width="40px" />
                            <ItemStyle Width="40px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                    <div style="float:right;">
                        <asp:LinkButton ID="cmdCloseDialog" runat="server" Text="Schließen" Height="16px" Width="78px"
                            CssClass="Tablebutton" OnClick="cmdCloseDialog_Click" />
                    </div>
            </div>
        </div>

        </form>
    </body>
</html>