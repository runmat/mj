<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ContactPage.aspx.vb" Inherits="CKG.Services.ContactPage"
    MasterPageFile="../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                <div id="innerContentRight" >

                    <div id="Result" runat="Server" style="margin-top: 25px;">
 
                         <div class="DivKontakt" runat="server">
                            <div class="ImpressumHead" style="width: 100%">
                                <h1>
                                    <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                            </div>                        

                                        <div ID="pnl1ndAddress" class ="pnl1ndAddress" runat="server" Visible="true" >                                        
                                            <table cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCName" runat="server" CssClass="lblKontaktFirma">DAD Deutscher Auto Dienst GmbH</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCAddress" runat="server">Ladestraße 1</asp:Label><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOrt" runat="server">22926 Ahrensburg</asp:Label><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td>
                                                        <asp:Label ID="lblCHotline" runat="server"></asp:Label><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCTel0" runat="server">Telefon: +49 4102 804-0</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfax0" runat="server">Telefax: +49 4102 804-111</asp:Label>
                                                    </td>
                                                </tr>
                                               <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>   --%>                                             
                                                <tr>
                                                    <td >
                                                        <asp:Panel ID="pnlLinks" runat="server">
                                                           <asp:HyperLink ID="lnkMail" runat="server" NavigateUrl="mailto:info@dad.de">info@dad.de</asp:HyperLink>
                                                            <br />
                                                            <asp:HyperLink ID="lnkWeb" runat="server" NavigateUrl="http://www.dad.de">www.dad.de</asp:HyperLink>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>                               
                                 
                                        <div ID="pnl2ndAddress" class ="pnl2ndAddress"  runat="server" Visible="False">                                 
                                            <table cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCName2" runat="server" CssClass="lblKontaktFirma">Christoph Kroschke GmbH</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCAddress2" runat="server">Ladestraße 1 </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCOrt" runat="server">22926 Ahrensburg</asp:Label><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCTel" runat="server">Telefon: +49 4102 804-0</asp:Label><br />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfax" runat="server">Telefax: +49 4102 804-111</asp:Label><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlLinks2" runat="server">
                                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="mailto:info@kroschke.de">info@kroschke.de</asp:HyperLink>
                                                            <br />
                                                            <asp:HyperLink ID="lnkWeb2" runat="server" NavigateUrl="http://www.kroschke.de">www.kroschke.de</asp:HyperLink>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                </table>
                                        </div>

                               <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </div>
   
                    </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </div>
                </div>

                <div id="innerContentLeft2" style="display:none">
                    <div class="ImpressumHead" style="width: 100%">
                        <h1>
                            <asp:Label ID="Label1" runat="server" Text="Rückrufservice"></asp:Label></h1>
                    </div>
                        <div id="innerContentLeft2Callback">
                            <table cellspacing="0" cellpadding="0" width="100%" >
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel1" runat="server" Visible="true" Width="300px" 
                                           >
                                            <table cellspacing="0" cellpadding="0" width="100%">
                                                <tr>
                                                    <td class="innerCallback" colspan="3">
                                                        <asp:Label ID="Label2" runat="server" CssClass="lblKontaktFirma">Fragen? Wir rufen Sie kostenlos zurück!</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="innerCallback" colspan="3">
                                                        <asp:Label ID="Label3" runat="server">Einfach Ihre Telefonnummer eingeben <br/> und "abschicken" anklicken!</asp:Label><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="innerCallback" colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="innerCallback" width="75px" >
                                                        Ihr Name:&nbsp;<br />
                                                    </td>
                                                    <td align="left" class="innerCallback">
                                                        <asp:DropDownList ID="DropDownList1" runat="server" Width="60px" 
                                                            CssClass="DropDownNormal">
                                                            <asp:ListItem>Frau</asp:ListItem>
                                                            <asp:ListItem>Herr</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </td>
                                                    <td class="innerCallback">
                                                        <asp:TextBox ID="TextBox1" runat="server" Width="140px" 
                                                            CssClass="TextBoxNormal"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="innerCallback"  width="75px">
                                                        Telefon-Nr.:</td>
                                                    <td align="left" class="innerCallback" colspan="2">
                                                        <asp:TextBox ID="TextBox2" runat="server" Width="207px" 
                                                            CssClass="TextBoxNormal"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="innerCallback" width="75px">
                                                        Betreff:</td>
                                                    <td align="left" class="innerCallback" colspan="2">
                                                        <asp:TextBox ID="TextBox3" runat="server" Width="207px" 
                                                            CssClass="TextBoxNormal"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="innerCallback" width="75px">
                                                        gewünschter Zeitraum:</td>
                                                    <td align="left" class="innerCallback" colspan="2">
                                                        <asp:TextBox ID="TextBox4" runat="server" Width="207px" 
                                                            CssClass="TextBoxNormal"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="innerCallback" >
                                                        &nbsp;</td>
                                                    <td align="right" class="innerCallback" colspan="2" style="padding-right: 15px">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="Tablebutton" 
                                                            Height="16px" Width="78px" Enabled="False">&nbsp;» Absenden </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                  
                        <span style="text-align: center; padding-left:15px; font-size: medium; font-weight: bold; color: #800000;display:none">Leider noch nicht aktiv!</span></div>
                </div>


        </div>

    </div>
    
</asp:Content>
