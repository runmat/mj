<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Report203_02.aspx.vb"
    Inherits="AppArval.Report203_02" MasterPageFile="../../../MasterPage/Services.Master" %>

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
                                <asp:Label ID="lblHead" runat="server" Text="Detaildaten"></asp:Label></h1>
                                <asp:HyperLink ID="lnkShowCSV" runat="server"></asp:HyperLink>
                                <asp:Label ID="lblDownloadTip" runat="server"></asp:Label>
                        </div>
                        <div id="data">
                            <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="250px">
                                    Halter:
                                    </td>
                                    <td>
                                    <asp:Label ID="lblHalter" runat="server"></asp:Label>
                                     </td>                                    
                                     <td>
                                    Personalausweis:
                                    </td>
                                    <td>
                                    <asp:Label ID="lblPerso" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                    Handelsreg.:
                                    </td>
                                    <td>
                                    <asp:Label ID="lblRegister" runat="server"></asp:Label>
                                    </td>
                                    <td></td>                                    
                                </tr>
                                <tr>
                                    <td>
                                    HOrt:
                                    </td>
                                    <td>
                                    <asp:Label ID="lblHOrt" runat="server"></asp:Label>                                                                    
                                    </td>
                                    <td>
                                    Gewerbeanmeldung:
                                    </td>
                                    <td>
                                    <asp:Label ID="lblGewerbe" runat="server"></asp:Label>
                                    </td>
                                     <td>
                                    Vollständig:
                                    </td>
                                    <td>
                                    <asp:Label ID="lblVollst" runat="server"></asp:Label>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td>
                                    Kundennummer:
                                    </td>
                                    <td>
                                    <asp:Label ID="lblKunnr" runat="server"></asp:Label>                                    
                                    </td>
                                    <td>
                                    Vollmacht:
                                    </td>
                                    <td>
                                    <asp:Label ID="lblVollmacht" runat="server"></asp:Label>
                                    </td>
                                     <td>
                                    Einzugserm.:
                                    </td>
                                    <td>
                                    <asp:Label ID="lblEinzug" runat="server"></asp:Label>  
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    <asp:Label ID="lblKUNNR_SAP" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                    Versicherungsbes.:
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr><td colspan="6"><asp:Label BackColor="#9BD4BE" runat="server" Width="100%" Height="1px"></asp:Label></td></tr>
                                <tr>
                                    <td>
                                    Ausstellungsdatum Vollmacht:
                                    <asp:Label ID="lblDateVollm" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    Beschafftungsdatum Handelsregister/ <br />Gewerbeanmeldung:
                                    </td>
                                    <td>                                    
                                    <asp:Label ID="lbl_DateGew" runat="server"></asp:Label>                                    
                                    </td> 
                                </tr>
                                <tr>
                                    <td>
                                    neue Beschaffung der Vollmacht/<br />Registerauszüge am:
                                    </td>
                                    <td>
                                    <asp:Label ID="lblVollmRegDate" runat="server"></asp:Label>
                                    </td> 
                                </tr>
                                <tr>
                                    <td>
                                    Besonderheiten Kunde:
                                    </td>
                                    <td>
                                    <asp:Label ID="lblBemerk" runat="server"></asp:Label>                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6"><asp:Label ID="Label1" BackColor="#9BD4BE" runat="server" Width="100%" Height="1px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>                            
                                    <td>
                                    EVB Nummer:
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txt_NummerEVB" runat="server" Width="100%"></asp:TextBox>
                                    <asp:Label ID="lblEVB" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td rowspan="3" colspan="3">
                                    <asp:Calendar runat="server" ID="calVon" Visible="false"></asp:Calendar>
                                    <asp:Calendar runat="server" ID="calBis" Visible="false"></asp:Calendar>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    Datum von:
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtDatumvon" runat="server" Width="100%"></asp:TextBox>
                                    <asp:Label ID="lblDateForm1" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                    <asp:LinkButton ID="btnCal1" runat="server" Text="Kalender" CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    Datum Bis:
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtDatumbis" runat="server" Width="100%"></asp:TextBox>
                                    <asp:Label ID="lblDateForm2" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                    <asp:LinkButton ID="btnCal2" runat="server" Text="Kalender" CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>                                    
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                    <asp:linkbutton ID="btnSave" runat="server" Text="Speichern" CssClass="Tablebutton" Height="16px" Width="78px" />
                                    </td>
                                    <td>
                                    <asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="javascript:window.close()" CssClass="Tablebutton" Height="16px" Width="78px">Schließen</asp:hyperlink>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblError" runat="server" Visible="false" > </asp:Label>
                                        <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div id="dataFooter">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>