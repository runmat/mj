<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResponsiblePage.aspx.vb"
    Inherits="CKG.PortalZLD.ResponsiblePage" MasterPageFile="../MasterPage/Services.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <body>
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
                        <div id="Result" runat="Server">
                            <div id="TableQuery">
                                <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                    <tr>
                                        <td>
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
                                                                Telefon:&nbsp;<%#DataBinder.Eval(Container, "DataItem.telephone")%>
                                                            </div>
                                                            <div class="responsibleText">
                                                                Telefax:&nbsp;<%#DataBinder.Eval(Container, "DataItem.fax")%>
                                                            </div>
                                                            <div class="responsibleText">
                                                               <div runat="server" id="divMail" visible="false"> Email:&nbsp;<a href="mailto:<%#DataBinder.Eval(Container, "DataItem.mail")%>"> <%#DataBinder.Eval(Container, "DataItem.mail")%></a> </div>
                                                                <div runat="server" id="divMailPartner"> Email:&nbsp; <asp:LinkButton ID="LinkButton1" OnClick="Button1_Click"  Text='<%#DataBinder.Eval(Container, "DataItem.mail")%>' runat="server"></asp:LinkButton>    </div>                                                            

                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="22px">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
<div>
                                <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                                    PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel"
                                    X="380" Y="250">
                                </ajaxToolkit:ModalPopupExtender>
           
                                <asp:Panel ID="mb" runat="server" Width="385px" Height="150px" 
                                    BackColor="White"  style="display:none">
                                    <div style="padding-left:10px;padding-top:15px;margin-bottom:10px;">

                                            Achtung! <br />
                                            Aufträge, Daten und wichtige Mitteilungen versenden sie <br />
                                            bitte immer an das Firmenpostfach:<br />
                                            <asp:Label ID="lblSupportAdress" runat="server"></asp:Label><br />
                                            <br />
                                            Diese E-Mail versenden an:
                                                                                      
                                        </div>
                                            <table>
                                                <tr>
                                                    <td style="padding-left:10px">
                                                        <asp:HyperLink ID="lnkFirmenPost" runat="server"></asp:HyperLink>
                                                    </td>
                                                     <td style="padding-left:25px">
                                                        <asp:HyperLink ID="lnkAnsprech" runat="server"></asp:HyperLink>
                                                    </td>
 
                                                </tr>
                                                <tr>
                                                 <td colspan="2" style="padding-left:135px">
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
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>
        </div>
    </body>
</asp:Content>
