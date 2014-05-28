<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04.aspx.vb" Inherits="AppPorsche.Change04"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <a class="active">Fahrzeugsuche</a>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
              
                        <div id="data">

                                    <table id="tab1" cellpadding="0" cellspacing="0">
                                        <tfoot>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </tfoot>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td nowrap="nowrap" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="form">
                                                <td nowrap="nowrap" class="firstLeft active">
                                                    Nummer ZB2:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtVertragsNr" runat="server" CssClass="InputTextbox" Width="120px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="form">
                                                <td nowrap="nowrap" class="firstLeft active">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td nowrap="nowrap" class="firstLeft active">
                                                    <asp:TextBox ID="txtFahrgestellNr" runat="server" Width="120px" CssClass="InputTextbox"></asp:TextBox>
                                                    &nbsp;<span style="font-weight: normal">*(mit Platzhaltersuche)</span>
                                                </td>
                                            </tr>
                                            <tr class="form">
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="right" class="rightPadding">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
 
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>          
                                <div id="dataFooter">
                                    &nbsp;<asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Height="20px"
                                        Width="78px">» Weiter</asp:LinkButton>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>                        
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
