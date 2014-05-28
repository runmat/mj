<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MailPflegeUebersicht.aspx.vb"
    Inherits="Admin.MailPflegeUebersicht" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Mailpflege"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="ddlFilterCustomer" />
                            <asp:PostBackTrigger ControlID="lbnMailtoEmpf" />
                            <asp:PostBackTrigger ControlID="lbnMailtoCC" />
                            <asp:PostBackTrigger ControlID="lbnEmpftoMail" />
                            <asp:PostBackTrigger ControlID="lbnCCtoMail" />
                            <asp:PostBackTrigger ControlID="chkAktiv" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                            <div id="adminInput">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr class="formquery">
                                        <td class="firstLeft active" >
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" width="100px">
                                            Kunde:
                                        </td>
                                        <td class="firstLeft active" width="260px">
                                            <asp:DropDownList ID="ddlFilterCustomer" runat="server" Font-Names="Verdana,sans-serif"
                                                Height="20px" Width="260px" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="firstLeft active" width="100px">
                                            <asp:label ID="lblVorgangsnr" runat="server" Text="Vorgangsnr.:"></asp:label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblVorgangsnummer" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td width="100%">
                                        </td>
                                    </tr>
                                    <tr id="trSelectMail1" runat="server" class="formquery" visible="true">
                                        <td class="firstLeft active" width="100px" style="vertical-align: top;">
                                            Mailvorlage:<br />
                                            (Betreff)
                                        </td>
                                        <td class="firstLeft" width="260px" style="vertical-align: top;">
                                            <asp:ListBox ID="lstBetreff" runat="server" Width="260px" Height="150px" AutoPostBack="true">
                                            </asp:ListBox>
                                        </td>
                                        <td class="firstLeft active" width="100px" style="vertical-align: top;">
                                            Text:
                                        </td>
                                        <td class="firstLeft" style="vertical-align: top; width: 260px;">                                            
                                            <asp:Panel ID="pnlMailtext" runat="server" Width="260px" Height="150px" Wrap="true" ScrollBars="Auto" BorderColor="#bfbfbf" BorderStyle="Solid" BorderWidth="1px">
                                                <asp:Literal ID="ltlMailtext" runat="server" Mode="Transform"></asp:Literal>
                                            </asp:Panel>
                                        </td>
                                        <td width="100%">
                                        </td>
                                    </tr>
                                    <tr id="trSelectMail2" runat="server" class="formquery" visible="true">
                                        <td></td>
                                        <td class="firstLeft active">
                                            <table style="border: none">
                                                <tr valign="top">
                                                    <td >
                                                    <div style="vertical-align:middle;"><asp:CheckBox ID="chkAktiv" runat="server" Text="aktiv" BorderWidth="0" AutoPostBack="true"/></div>
                                                        
                                                    </td>
                                                    <td valign="top">
                                                        <asp:ImageButton ID="ibnTextLoeschen" runat="server" Width="16" AlternateText="Vorlage löschen"
                                                            ImageUrl="../Images/Papierkorb_01.gif"></asp:ImageButton>
                                                        <label>Löschen</label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="firstLeft active" width="100px" style="vertical-align: top;"></td>
                                        <td class="firstLeft active">
                                            <table style="border: none">
                                                <tr valign="top">
                                                    <td valign="top">
                                                        <asp:ImageButton ID="ibnTextBearbeiten" runat="server" Width="16" AlternateText="Vorlage löschen"
                                                            ImageUrl="../Images/Edit_01.gif"></asp:ImageButton>
                                                        <label>Bearbeiten</label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="100%">
                                        </td>
                                    </tr>
                                    <tr id="trSelectMail3" runat="server" class="formquery" visible="true">
                                    
                                        <td class="firstLeft active" style="vertical-align: top;" width="100px" rowspan="2">
                                            Mailadressen:
                                        </td>
                                        <td class="firstLeft" style="vertical-align: top;" width="260px" rowspan="2">
                                            <asp:ListBox ID="lstMailpool" runat="server" AutoPostBack="true" height="260px" 
                                                Width="260px"></asp:ListBox>
                                        </td>
                                        <td class="firstLeft active" style="vertical-align: top;" width="100px">
                                            Empfänger:
                                            <br />
                                            <table  style="border: none">
                                                <tr>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lbnMailtoEmpf" runat="server" Font-Bold="true" 
                                                            Font-Size="12px" ForeColor="#333333" Text="&amp;nbsp;&amp;#187;"></asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lbnEmpftoMail" runat="server" Font-Bold="true" 
                                                            Font-Size="12px" ForeColor="#333333" Text="&amp;nbsp;&amp;#171;"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="firstLeft" style="border: 0; vertical-align: top;" width="260px">
                                            <asp:ListBox ID="lstEmpfaenger" runat="server" AutoPostBack="true" 
                                                Height="120px" Width="260px"></asp:ListBox>
                                        </td>
                                        <td width="100%">
                                        </td>
                                    
                                    </tr>
                                    
                                    <tr class="formquery" id="trSelectMail4" runat="server">
                                        <td class="firstLeft active" width="100px" style="vertical-align: top;">
                                            CC-Empfänger:
                                                        <br />
                                            <table style="border: none">
                                                <tr>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lbnMailtoCC" runat="server" Text="&amp;nbsp;&amp;#187;" Font-Size="12px"
                                                            ForeColor="#333333" Font-Bold="true"></asp:LinkButton>
                                                    </td>
                                                    <td align="center">                                                        
                                                        <asp:LinkButton ID="lbnCCtoMail" runat="server" Text="&amp;nbsp;&amp;#171;" Font-Size="12px"
                                                            ForeColor="#333333" Font-Bold="true"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="firstLeft" width="260px" style="border: 0; vertical-align: top;">
                                        <asp:ListBox ID="lstCC" runat="server" Width="260px" height="120px" AutoPostBack="true"></asp:ListBox>
                                        </td>
                                        <td width="100%">
                                        </td>
                                    </tr>
                                    <tr id="trSelectMail5" runat="server">
                                    <td class="firstLeft active" width="100px" style="vertical-align: top;">
                                        &nbsp;</td>
                                    <td class="firstLeft" colspan="3" align="right">
                                        <asp:LinkButton ID="lbnNewText" runat="server" AlternateText="Neuer Text" 
                                            CssClass="TablebuttonLarge " Font-Names="Verdana,sans-serif" 
                                            ImageUrl="../Images/buttonMiddle.jpg" Style="vertical-align: middle;
                                                text-align: center; font-size: 10px; font-weight: bold; color: #333333; height: 20px;
                                                margin-left: 4px;" Text="» Neuer Text" Width="128px"></asp:LinkButton>
                                        <asp:LinkButton ID="lbnNewMail" runat="server" 
                                            AlternateText="Neue Mailadresse" CssClass="TablebuttonLarge " 
                                            Font-Names="Verdana,sans-serif" ImageUrl="../Images/buttonMiddle.jpg" Style="vertical-align: middle;
                                                text-align: center; font-size: 10px; font-weight: bold; color: #333333; height: 20px;
                                                margin-left: 4px;" Text="» Neue Mailadresse" Width="128px"></asp:LinkButton>
                                    </td>
                                    <td width="100%"></td>
                                    </tr>
                                    <tr id="trNewText1" runat="server" class="formquery" visible="false">
                                        <td class="firstLeft active" width="100px" style="vertical-align: top;">
                                            Betreff:
                                        </td>
                                        <td width="260px" class="firstLeft">
                                            <asp:TextBox ID="txtNewBetreff" runat="server" Width="260px"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active" width="100px" style="vertical-align: top;">
                                            Vorgangsnummer:
                                        </td>
                                        <td class="firstLeft">
                                            <asp:TextBox ID="txtVorgangsnummer" runat="server" Width="240px"></asp:TextBox>
                                            <asp:ImageButton ID="btnVorgangsnrListe" runat="server" Width="20" AlternateText="Liste Vorgangsnummer"
                                                ImageUrl="../Images/Dokumente03_09.jpg"></asp:ImageButton>      
                                        </td>
                                        <td width="100%">
                                        </td>
                                    </tr>
                                    <tr id="trNewText2" runat="server" visible="false" class="formquery">
                                        <td class="firstLeft active" width="100px" style="vertical-align: top;">
                                            Text:
                                        </td>
                                        <td class="firstLeft" height="300px" colspan="2">
                                            <div>
                                                <cc2:Editor ID="Editor1" runat="server"  Width="380px" />
                                            </div>
                                        </td>
                                        <td id="grid" valign="top" align="center" visible="false">
                                            <asp:Label ID="label1" runat="server" Text="verwendete Vorgangsnummern" CssClass="active"></asp:Label>
                                            <asp:ListBox ID="lstVorgangsnummer" runat="server" Width="260px" Height="250px"></asp:ListBox>
                                        </td>
                                        <td width="100%"></td>
                                    </tr>
                                    <tr id="trNewMail" runat="server" visible="false" class="formquery">
                                        <td class="firstLeft active" width="100px" style="vertical-align: top;">
                                            Mailadresse:
                                        </td>
                                        <td class="firstLeft" width="260px">
                                            <asp:TextBox ID="txtNewMail" runat="server" Width="260px"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                        </td>
                                        <td width="100%">
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width: 100%" colspan="5">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>

                            </div>
                                 <div  style="height: 22px;">
                                    &nbsp;
                                </div>                            
                                <div id="dataQueryFooter">
                                    &nbsp;<asp:LinkButton class="Tablebutton" ID="btnSave" runat="server" Text="&amp;nbsp;&amp;#187; Speichern"
                                        CssClass="Tablebutton" Height="16px" Width="78px" Font-Names="Verdana,sans-serif"
                                        Font-Size="10px" Visible="false"></asp:LinkButton>
                                    <asp:LinkButton class="Tablebutton" ID="btnAbbrechen" runat="server" Text="» Abbrechen"
                                        CssClass="Tablebutton" Height="16px" Width="78px" Font-Names="Verdana,sans-serif"
                                        Font-Size="10px" Visible="false"></asp:LinkButton>
                                </div>
                         
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

