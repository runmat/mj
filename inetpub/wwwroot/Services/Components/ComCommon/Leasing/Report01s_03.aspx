<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01s_03.aspx.vb"
    Inherits="CKG.Components.ComCommon.Report01s_03" MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div>
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false"></asp:Label>
                <asp:Label ID="lblInfo" runat="server" ForeColor="Blue" style="padding-left:15px" class="firstLeft active" Font-Bold="true" EnableViewState="false"></asp:Label>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Detailmaske"></asp:Label>
                        </h1>
                        
                        
                        <span style="float: right; padding-right: 15px"><span style="margin-right: 5px">
                                    <img src="../../../Images/iconPDF.gif" alt="PDF herunterladen" height="13px" />
                                </span><span>
                                    <asp:LinkButton ID="lbCreatePDF" runat="server" Text="PDF herunterladen" ForeColor="White"></asp:LinkButton>
                                </span></span>
                                
                                <span style="float: right; padding-right: 15px"><span style="margin-right: 5px">
                                    <img src="/Services/Images/pdf_email_btn.gif" alt="PDF mailen" height="13px" />
                                </span><span>
                                         <asp:LinkButton ID="lbCreateMail" runat="server" Text="PDF mailen" ForeColor="White"></asp:LinkButton>
                                  
                                </span></span>
                                
                    </div>
                   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div>
                                <table cellpadding="0" cellspacing="0" width="100%" style="padding-left:15px; color: #4c4c4c; background-color: #dfdfdf">
                                    <tr style="height: 22px">
                                        <td class="active">
                                            Allgemeine Daten
                                        </td>
                                        <td>
                                           
                                        <td align="right">
                                            <div style="padding-right: 5px">
                                                <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="/Services/Images/queryArrow.gif" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="padding-top: 5px">
                                <asp:Panel ID="pnlAllgDaten" runat="server" Width="100%">
                                    <table cellpadding="0" cellspacing="0" width="100%" style="padding-left:15px">
                                        <tr style="padding-top: 5px">
                                            <td style="width:200px">
                                                <asp:Label ID="lbl_LeasVertragsnummer" runat="server" >lbl_LeasVertragsnummer</asp:Label>
                                            </td>
                                            <td style="width:200px;">
                                                <asp:Label ID="lblLvNummer" runat="server"></asp:Label>
                                            </td>
                                            <td align="left">
                                                Kennzeichen:
                                            </td>
                                            <td style="text-align:left">
                                                <asp:Label ID="lblKennzeichen" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 10px">
                                            <td style="width:200px">
                                                Fahrgestellnummer:
                                            </td>
                                            <td style="width:200px">
                                                <asp:Label ID="lblFahrgestellnummer" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                Kundenstatus:</td>
                                            <td>



                                                <asp:Label ID="lblKundenstatus" runat="server"></asp:Label>



                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <div style="padding-top: 10px;">
                                <table cellpadding="0" cellspacing="0" width="100%" style="padding-left:15px;color: #4c4c4c; background-color: #dfdfdf">
                                <tr style="height: 22px">
                                        <td class="active">
                                            Halterdaten
                                        </td>
                                        <td>
                                           
                                        <td align="right">
                                            <div style="padding-right: 5px">
                                                <asp:ImageButton ID="ibtHalter" runat="server" ImageUrl="/Services/Images/queryArrow.gif" />
                                            </div>
                                        </td>
                                    </tr>
                                
                                
                                </table>
                            </div>
                            <div style="padding-top: 7px">
                                <asp:Panel ID="pnlHalter" runat="server" Width="100%">
                                    <table cellpadding="0" cellspacing="0" width="100%" style="padding-left:15px">
                                        <tr>
                                            <td style="width:80px">
                                                Mitarbeiternr:
                                            </td>
                                            <td  style="width:150px">
                                                <asp:Label ID="lblMitarbeiternr" runat="server" Font-Bold="False"></asp:Label>
                                            </td>
                                            <td style="width:80px">
                                                PLZ/Ort:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblHalterPLZ" runat="server" Font-Bold="False"></asp:Label>/<asp:Label
                                                    ID="lblHalterOrt" runat="server" Font-Bold="False"></asp:Label>
                                            </td>
                                            
                                        </tr>
                                        <tr style="padding-top: 5px">
                                            <td style="width:80px">
                                                Haltername:
                                            </td>
                                            <td  style="width:150px">
                                                <asp:Label ID="lblHaltername" runat="server" Font-Bold="False"></asp:Label>
                                            </td>
                                            
                                             <td style="width:80px">
                                                Telefon:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblHalterTelefon" runat="server" Font-Bold="False"></asp:Label>
                                            </td>
                                            
                                        </tr>
                                        <tr style="padding-top: 5px">
                                            <td style="width:80px">
                                                Strasse:
                                            </td>
                                            <td  style="width:150px">
                                                <asp:Label ID="lblHalterStrasse" runat="server" Font-Bold="False"></asp:Label>
                                            </td>
                                            <td style="width:80px">
                                                E-Mail:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblHalterEMail" runat="server" Font-Bold="False"></asp:Label>
                                            </td>
                                            
                                        </tr>
                                            
                                       
                                    </table>
                                </asp:Panel>
                            </div>
                            <div style="padding-top: 10px">
                                <table cellpadding="0" cellspacing="0" width="100%" style="padding-left:15px; color: #4c4c4c; background-color: #dfdfdf">
                                    <tr style="height: 22px">
                                        <td class="active">
                                            Kilometerstände Fahrzeugübergabe
                                        </td>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <div style="padding-right: 5px">
                                                <asp:ImageButton ID="ibtKM" runat="server" ImageUrl="/Services/Images/queryArrow.gif" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="padding-top: 10px">
                                <asp:Panel ID="pnlKM" runat="server" Width="100%">
                                    <table cellpadding="0" cellspacing="0" width="100%" style="padding-left:15px">
                                        <tr>
                                            <td style="width:200px">
                                                &nbsp;
                                            </td>
                                            <td style="width:200px">
                                                Abfahrt
                                            </td>
                                            <td style="width:200px">
                                                Ankunft
                                            </td>
                                            <td>
                                                Differenz
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 10px">
                                            <td style="width:200px">
                                                Hinfahrt
                                            </td>
                                            <td style="width:200px">
                                                <asp:Label ID="lblHinAbfahrt" runat="server"></asp:Label>
                                            </td>
                                            <td style="width:200px">
                                                <asp:Label ID="lblHinAnkunft" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDiffHin" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 10px">
                                            <td style="width:200px">
                                                Rückfahrt
                                            </td>
                                            <td style="width:200px">
                                                <asp:Label ID="lblRueckAbfahrt" runat="server"></asp:Label>
                                            </td>
                                            <td style="width:200px">
                                                <asp:Label ID="lblRueckAnkunft" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDiffRueck" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <div style="padding-top: 10px">
                                <table cellpadding="0" cellspacing="0" width="100%" style="padding-left:15px; color: #4c4c4c; background-color: #dfdfdf">
                                    <tr style="height: 22px">
                                        <td class="active">
                                            Rückmeldungen
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDienstleistungen" runat="server"></asp:Label></td>
                                        </td>
                                        <td align="right">
                                            <div style="padding-right: 5px">
                                                <asp:ImageButton ID="ibtRueckmeldung" runat="server" 
                                                    ImageUrl="/Services/Images/queryArrow.gif" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="padding-top: 5px">
                                <asp:Panel ID="pnlRueckmeldung" runat="server" Width="100%">
                                    <table cellpadding="0" cellspacing="0" width="100%" style="padding-left:15px" >
                                        <tr>
                                            <td>
                                                
                                                <table style="width:100%;padding-top:10px;padding-bottom:10px">
                                                    <tr>
                                                        <td width="150px">
                                                            <b>Dienstleister</b></td>
                                                        <td width="150px">
                                                            <b>Leistungsart</b></td>
                                                        <td width="120px">
                                                            <b>Art</b></td>
                                                        <td width="60px">
                                                            <b>Ursache</b>
                                                        </td>
                                                        <td width="80px">
                                                            <b>Datum</b></td>
                                                        <td width="100px">
                                                            <b>Prognosedatum</b></td>                                                            
                                                        <td width="160px" style="padding-right:20px">
                                                            <b>Rückmeldetext</b></td>
                                                    </tr>
                                                </table>
                                                
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    <hr style="padding-bottom:5px" />
                                    <div id="divAusgabe" runat="server" style="padding-left: 15px;height:330px;">
                                        <asp:Literal ID="litRueck" runat="server"></asp:Literal>
                                    </div>
                                    
                                    </asp:Panel>
                            </div>
                            
                            <ajaxToolkit:CollapsiblePanelExtender ID="cpeAllData" runat="Server" TargetControlID="pnlAllgDaten"
                                ExpandControlID="NewSearch" CollapseControlID="NewSearch" Collapsed="true" ImageControlID="NewSearch"
                                ExpandedImage="/Services/Images/collapse.jpg" CollapsedImage="/Services/Images/expand.jpg"
                                SuppressPostBack="true" />
                            <ajaxToolkit:CollapsiblePanelExtender ID="cpeHalter" runat="Server" TargetControlID="pnlHalter"
                                ExpandControlID="ibtHalter" CollapseControlID="ibtHalter" Collapsed="false" ImageControlID="ibtHalter"
                                ExpandedImage="/Services/Images/collapse.jpg" CollapsedImage="/Services/Images/expand.jpg"
                                SuppressPostBack="true" />
                            <ajaxToolkit:CollapsiblePanelExtender ID="cpeKM" runat="Server" TargetControlID="pnlKM"
                                ExpandControlID="ibtKM" CollapseControlID="ibtKM" Collapsed="true" ImageControlID="ibtKM"
                                ExpandedImage="/Services/Images/collapse.jpg" CollapsedImage="/Services/Images/expand.jpg"
                                SuppressPostBack="true" />
                                <ajaxToolkit:CollapsiblePanelExtender ID="cplRueckmeldung" runat="Server" TargetControlID="pnlRueckmeldung"
                                ExpandControlID="ibtRueckmeldung" CollapseControlID="ibtRueckmeldung" Collapsed="false" ImageControlID="ibtRueckmeldung"
                                ExpandedImage="/Services/Images/collapse.jpg" CollapsedImage="/Services/Images/expand.jpg"
                                SuppressPostBack="true" />
                        
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Sollen die Detaildaten per E-Mail versendet werden?"
                                                TargetControlID="lbCreateMail" />
                                               
            </div>
            
        </div>
    </div>
</asp:Content>
