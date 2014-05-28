<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02.aspx.vb" Inherits="KBS.Change02"
    MasterPageFile="~/KBS.Master" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                 <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck"  CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Wareneingangsprüfung"></asp:Label>&nbsp;

                        <asp:label id="lblPageTitle" Text="(Auswahl)" runat="server"></asp:label>
				</h1>
                        </div>
                        <div id="paginationQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="firstLeft active">
                                          bitte wählen Sie einen Vorgang
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <asp:UpdatePanel runat="server" id="upSelection">
                       <ContentTemplate >
                                             
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
                                        <td colspan="5"  class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr  class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblBestellnummer" Text="Bestellnummer:" runat="server"></asp:Label>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtBestellnummer" AutoPostBack="true" runat="server" text="* *"
                                               ></asp:TextBox>
                                        </td>
                                        <td  class="firstLeft active">
                                          <asp:Label ID="lblLieferantName" Text="Lieferant Name:" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        <asp:TextBox ID="txtLieferantName" AutoPostBack="true"   runat="server" text="* *"
                                               ></asp:TextBox>
                                        </td>
                                        <td  width="100%"  >
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr >
                                    <td colspan="5">
                                    &nbsp;
                                    </td>
                                    </tr>
                                     <tr class="formquery">
                                     <td  class="firstLeft active" valign="top">
                                        <asp:Label ID="lblBestellungen"  Text="Bestellungen:" runat="server"></asp:Label>&nbsp;
                                                                                 <asp:Label ID="lblBestellungsAnzahl" Text="12" runat="server"></asp:Label>
                                     </td>
                                        <td colspan="3" >
                                           <asp:ListBox  Rows="10"  Width="100%" id="lbxBestellungen" runat="server" DataTextField="AnzeigeText" DataValueField="Bestellnummer">
                                           </asp:ListBox>
                                        </td>
                                        <td width="100%">&nbsp;</td>
                                    </tr>
                                    
                                </tbody>
                            </table>
                        </div>
                         </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbWeiter" Text="Weiter" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>