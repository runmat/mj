<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Report203.aspx.vb"
    Inherits="AppArval.Report203" MasterPageFile="../../../MasterPage/Services.Master" %>
    
<%@ Register Assembly="BusyBoxDotNet" Namespace="BusyBoxDotNet" TagPrefix="busyboxdotnet" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                
                                    <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>&nbsp;
                                    <asp:Label ID="lblPageTitle" runat="server">(Zusammenstellung von Abfragekriterien)</asp:Label>
                                
                            </h1>
                        </div>
                        <div id="paginationQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="firstLeft active">
                                            bitte geben Sie ihre Suchkriterien ein
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="TableQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tfoot>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr class="formquery">
                                        <td colspan="3" class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                   
                                  
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                           Haltername:
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="TextBoxNormal" ID="txtAbmeldedatumVon" runat="server"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                           Art:
                                        </td>
                                        <td>
                                           <asp:RadioButtonList id="rbArt" runat="server" RepeatDirection="Horizontal">
																<asp:ListItem Value="0" Selected="True">alle</asp:ListItem>
																<asp:ListItem Value="1">vollst&#228;ndig</asp:ListItem>
																<asp:ListItem Value="2">unvollst&#228;ndig</asp:ListItem>
															</asp:RadioButtonList>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdCreate" Text="Weiter" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton" ></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
