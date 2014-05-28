<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Error.aspx.vb" Inherits="CKG.PortalZLD._Error"
    MasterPageFile="../MasterPage/Services.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                       
                        <div id="Result" runat="Server" >
                            <div id="datainfo" style="padding-top: 45px"  align="center">
                                <table class="tableInfo" id="Table3" cellspacing="0" cellpadding="0" style="width:50%"
                                    border="0">
                                    <tr>
                                        <td align="left" height="74" style="border: 1px solid #910100; width:100%">
                                            <asp:Label ID="lblErrorMessage" CssClass="TextError" runat="server" Font-Bold="True"></asp:Label><br>
                                            <br />
                                            &nbsp;Bitte wenden Sie sich an Ihren Administrator!<br />
                                            <br />
                                            <asp:Label ID="lblCName" runat="server" Font-Bold="True">&nbsp;DAD Deutscher Auto Dienst GmbH</asp:Label><br>
                                            <asp:Label ID="lblCAddress" runat="server">&nbsp;Bogenstraße 26, 22926 Ahrensburg<br />&nbsp;Hotline: +49 04102 804-109</asp:Label><br>
                                            <asp:Panel ID="pnlLinks" style="padding-bottom: 15px" runat="server">
                                                <asp:HyperLink ID="lnkMail" runat="server" NavigateUrl="mailto:info@dad.de">&nbsp;info@dad.de</asp:HyperLink>
                                                <br />
                                                <asp:HyperLink ID="lnkWeb" runat="server" NavigateUrl="http://www.dad.de">&nbsp;www.dad.de</asp:HyperLink>
                                            </asp:Panel>

                                    </tr>

                                </table>
                           <div id="dataFooter">
                            &nbsp;</div>
                            </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
