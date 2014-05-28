<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResponsiblePage.aspx.vb"
    Inherits="CKG.Portal.Info.ResponsiblePage" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright" />
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" >
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        div.transbox
        {
            width: 100%;
            height: 100%;
            background-color: #ffffff;
            filter: alpha(opacity=60);
            opacity: 0.6;
            z-index: -1;
            position: relative;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <div id="divInfo" style="display: none" runat="server">
        Achtung!
        <br />
        Aufträge, Daten und wichtige Mitteilungen versenden sie
        <br />
        bitte immer an das Firmenpostfach:<br />
        <asp:Label ID="lblSupportAdress" runat="server"></asp:Label><br />
        <br />
        Diese E-Mail versenden an:
        <table>
            <tr>
                <td style="padding-left: 10px">
                    <asp:HyperLink ID="lnkFirmenPost" runat="server"></asp:HyperLink>
                </td>
                <td style="padding-left: 50px">
                    <asp:HyperLink ID="lnkAnsprech" runat="server"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-left: 111px; padding-top: 10px">
                    <asp:Button ID="btnCancel" runat="server" Text="Abbrechen" CssClass="TablebuttonLarge"
                        Font-Bold="true" Width="80px" />
                </td>
            </tr>
        </table>
    </div>
    <div id="FormDiv" runat="server">
        <table width="100%" align="center" cellpadding="0" cellspacing="0">
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
                                <td class="PageNavigation" width="150">
                                    &nbsp;
                                </td>
                                <td class="PageNavigation">
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="150">
                                    <table id="Table2" bordercolor="#ffffff" cellspacing="1" cellpadding="0" width="150"
                                        border="1">
                                        <tr>
                                            <td class="TextHeader" width="150">
                                                Ansprechpartner
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" height="25">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" height="74">
                                                <asp:Repeater ID="Repeater1" runat="server">
                                                    <ItemTemplate>
                                                        <div style="float: left; padding-bottom: 10px">
                                                            <table border="1px" style="border-color: Gray; border-style: solid; background-color: White;"
                                                                rules="none" width="400px;">
                                                                <tr>
                                                                    <td style="width: 100;">
                                                                        <div class="responsiblePicture">
                                                                            <img alt="<%# DataBinder.Eval(Container, "DataItem.EmployeeName") %>" src="../../../Services/bilder/responsible/<%# DataBinder.Eval(Container, "DataItem.PictureName") %>"
                                                                                height="150" width="100" /><!--.JPG-->
                                                                        </div>
                                                                    </td>
                                                                    <td style="padding: 4px; vertical-align: top; text-align: left;">
                                                                        <div class="responsibleTextName">
                                                                            <%#DataBinder.Eval(Container, "DataItem.Name1")%>
                                                                            <%#DataBinder.Eval(Container, "DataItem.Name2")%>
                                                                        </div>
                                                                        <div class="responsibleDepartment">
                                                                            <%#DataBinder.Eval(Container, "DataItem.position")%>
                                                                        </div>
                                                                        <div style="height: 100%;">
                                                                            &nbsp;
                                                                        </div>
                                                                        <div class="responsibleText">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        Telefon:
                                                                                    </td>
                                                                                    <td>
                                                                                        <%#DataBinder.Eval(Container, "DataItem.Telefon")%>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Mobil:
                                                                                    </td>
                                                                                    <td>
                                                                                        <%#DataBinder.Eval(Container, "DataItem.Mobile")%>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Telefax:
                                                                                    </td>
                                                                                    <td>
                                                                                        <%#DataBinder.Eval(Container, "DataItem.fax")%>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Email:
                                                                                    </td>
                                                                                    <td>
                                                                                        <div runat="server" id="divMail" visible="true">
                                                                                            <a href="mailto:<%#DataBinder.Eval(Container, "DataItem.mail")%>">
                                                                                                <%#DataBinder.Eval(Container, "DataItem.mail")%></a>
                                                                                        </div>
                                                                                        <%--<div runat="server" id="divMailPartner">
                                                                                            <asp:LinkButton ID="LinkButton1" OnClick="Button1_Click" Text='<%#DataBinder.Eval(Container, "DataItem.mail")%>'
                                                                                                runat="server"></asp:LinkButton>
                                                                                        </div>--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            &nbsp;                                                            
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" height="25">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <!--#include File="../PageElements/Footer.html" -->
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
