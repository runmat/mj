<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Selection.aspx.vb" Inherits="CKG.PortalZLD.Selection"
    MasterPageFile="/PortalZLD/MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:HyperLink ID="lnkAdmin" CssClass="Adminbutton" runat="server" Text="" Visible="False"
                    NavigateUrl="../Admin/AdministrationMenu.aspx">Verwaltung</asp:HyperLink>
                &nbsp;
                <div class="divGreet">
                    <asp:Label ID="lblGreet" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div id="innerContent">
                <div>
                    <asp:Label ID="lblError" runat="server" Visible="False" CssClass="TextError" EnableViewState="False"></asp:Label>
                </div>
                <div id="innerContentRight" style="width: 100%">
                    <div class="ExcelDiv">
                        <div align="left" style="padding-left: 15px">
                            <asp:Label ID="lblAktuell" runat="server" Text="Aktuelles"></asp:Label>
                        </div>
                    </div>
                    <div id="data">
                        <table id="TableNews" runat="server" cellpadding="0" cellspacing="0" width="100%"
                            style="border-right-style: solid; border-right-width: 1px; border-right-color: #dfdfdf;
                            border-bottom-color: #dfdfdf; border-left-color: #dfdfdf; border-bottom-style: solid;
                            border-left-style: solid; border-bottom-width: 1px; border-left-width: 1px">
                            <tr>
                                <td height="200px">
                                    <div style="float: left; padding-bottom: 10px">
                                        <div id="NewsPicture" class="responsiblePicture">
                                            <asp:Image ID="imgNews1"  Visible="false"  runat="server" />
                                        </div>
                                        <div id="NewsText" class="responsibleTextName">
                                            <div class="responsibleText">
                                                <asp:Label ID="lblNews1" Visible="false" runat="server" Text="Label"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td height="200px">
                                    <div style="float: left; padding-bottom: 10px">
                                        <div id="Div1" class="responsiblePicture">
                                            <asp:Image ID="imgNews2"  Visible="false" runat="server" />
                                        </div>
                                        <div id="Div2" class="responsibleTextName">
                                            <div class="responsibleText">
                                                <asp:Label ID="lblNews2"  Visible="false" runat="server" Text="Label"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Size="13px"></asp:Label>
                    </div>
                    <div class="ExcelDiv" style="margin-top: 31px;">
                        <div align="left" style="padding-left: 15px">
                            <asp:Label ID="Label2" runat="server" Text="Ansprechpartner"></asp:Label>
                        </div>
                    </div>
                    <div id="TableQuery">
                        <table id="TableRepaeter" runat="server" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td nowrap="nowrap">
                                    <asp:Repeater ID="Repeater1" runat="server">
                                        <ItemTemplate>
                                            <div style="float: left; padding-bottom: 10px">
                                                <div class="responsiblePicture">
                                                    <img alt="<%# DataBinder.Eval(Container, "DataItem.EmployeeName") %>" src="../bilder/responsible/<%# DataBinder.Eval(Container, "DataItem.PictureName") %>.JPG"
                                                        height="150" width="100" />
                                                </div>
                                                <div class="responsibleTextName">
                                                    <%#DataBinder.Eval(Container, "DataItem.FirstName")%>
                                                    <%#DataBinder.Eval(Container, "DataItem.LastName")%>
                                                    <div class="responsibleDepartment">
                                                        <%#DataBinder.Eval(Container, "DataItem.position")%>
                                                    </div>
                                                    <div class="responsibleText">
                                                        Telefon:&nbsp;<%#DataBinder.Eval(Container, "DataItem.telephone")%></div>
                                                    <div class="responsibleText">
                                                        Telefax:&nbsp;<%#DataBinder.Eval(Container, "DataItem.fax")%></div>
                                                    <div class="responsibleText">
                                                        <div runat="server" id="divMail" visible="false">
                                                            Email:&nbsp;<a href="mailto:<%#DataBinder.Eval(Container, "DataItem.mail")%>">
                                                                <%#DataBinder.Eval(Container, "DataItem.mail")%></a>
                                                        </div>
                                                        <div runat="server" id="divMailPartner">
                                                            Email:&nbsp;
                                                            <asp:LinkButton ID="LinkButton1" OnClick="Button1_Click" Text='<%#DataBinder.Eval(Container, "DataItem.mail")%>'
                                                                runat="server"></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </table>
                        &nbsp;
                    </div>
                    <div>
                        <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                            PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel"
                            X="380" Y="250">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="mb" runat="server" Width="385px" Height="150px" BackColor="White"
                            Style="display: none">
                            <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px;">
                                Achtung!
                                <br />
                                Aufträge, Daten und wichtige Mitteilungen versenden sie
                                <br />
                                bitte immer an das Firmenpostfach:<br />
                                <asp:Label ID="lblSupportAdress" runat="server"></asp:Label><br />
                                <br />
                                Diese E-Mail versenden an:
                            </div>
                            <table>
                                <tr>
                                    <td style="padding-left: 10px">
                                        <asp:HyperLink ID="lnkFirmenPost" runat="server"></asp:HyperLink>
                                    </td>
                                    <td style="padding-left: 25px">
                                        <asp:HyperLink ID="lnkAnsprech" runat="server"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 135px">
                                        <asp:Button ID="btnCancel" runat="server" Text="Abbrechen" CssClass="TablebuttonLarge"
                                            Font-Bold="true" Width="80px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div id="dataFooter">
                        &nbsp;</div>
                    &nbsp;
                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal><asp:Literal ID="litAlert"
                    runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
