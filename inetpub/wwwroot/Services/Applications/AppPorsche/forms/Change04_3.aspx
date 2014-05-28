<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_3.aspx.vb" Inherits="AppPorsche.Change04_3"   MasterPageFile="../MasterPage/Porsche.Master" %>
<%@ Register TagPrefix="uc2" TagName="menue" Src="MenuePorsche.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<meta name="author" content="Christoph Kroschke AG" />
</asp:Content>
 <asp:Content ID="ContentMenue" ContentPlaceHolderID="ContentPlaceHolderMenue" runat="server">
     <uc2:menue ID="menue"  runat="server"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
		<div id="site">
		<div id="content">
				<div id="navigationSubmenu">
	                <asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx">| Fahrzeugauswahl</asp:hyperlink>
	                <a class="active" >| Adressauswahl</a>
				</div>
		
				<div id="innerContent">

	
					<div id="innerContentRight" style="width:100%;">
						<div id="innerContentRightHeading">
							<h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
														
						</div>
						<ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
                        EnableScriptLocalization="true" ID="ScriptManager1" />

                        <uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten>
						<div id="data">
						 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                        <table id="tab1" cellpadding="0" cellspacing="0" >
                           
                             <tfoot  style="height:20px; background-color: #dfdfdf"><tr><td colspan="6">&nbsp;</td></tr></tfoot>
                            
                            <tr>
 
                                <td nowrap="nowrap" width="100%" colspan="6">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                </td>
                            </tr>                                
                            <tr class="form">
                                <td class="firstLeft active" >
                                    <u>Zustellungsart:</u>
                                </td>
                                <td nowrap="nowrap">
                                    <asp:RadioButton ID="chkVersandStandard" runat="server" GroupName="Versandart" Checked="True"
                                        Text="Standard" Width="100px"></asp:RadioButton><br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">(siehe Hinweis)</font>
                                </td>
                                <td  nowrap="nowrap" >
                                    <asp:RadioButton ID="chk0900" runat="server" GroupName="Versandart" 
                                        Text="Express (vor 9:00 Uhr)" Width="100px">
                                    </asp:RadioButton><br />
                                    &nbsp;&nbsp;&nbsp;&nbsp; (28,00 Euro Netto)
                                </td>
                                <td  id="idPreis1" nowrap="nowrap" runat="server">
                                    <asp:RadioButton ID="chk1000" runat="server" GroupName="Versandart" 
                                        Text="vor 10:00 Uhr" Width="100px">
                                    </asp:RadioButton><br>
                                    &nbsp;&nbsp;&nbsp;&nbsp; (23,00 Euro Netto)
                                </td>
                                <td id="idPreis2" nowrap="nowrap"  runat="server">
                                    <asp:RadioButton ID="chk1200" runat="server" GroupName="Versandart" 
                                        Text="vor 12:00 Uhr" Width="100px">
                                    </asp:RadioButton><br />
                                    &nbsp;&nbsp;&nbsp;&nbsp; (17,80 Euro Netto)
                                </td>
                                <td nowrap="nowrap" width="100%">
                                    &nbsp;</td>
                            </tr>
                            <tr class="form" >
                                <td  nowrap="nowrap" class="firstLeft active" colspan="4" >
                                    <u>Achtung:</u>&nbsp;Die Nettopreise verstehen sich pro Sendung. &nbsp;</td>
                                <td nowrap="nowrap" class="firstLeft active" colspan="2"  >
                                    &nbsp;</td>
                                                                                                                                        
                            </tr>
                            <tr class="form">
                                <td class="firstLeft active"  nowrap="nowrap">
                                    Versandadresse:</td>
                                <td class="active" nowrap="nowrap" colspan="3">
                                    <asp:dropdownlist id="cmbZweigstellen" runat="server" Width="380px"></asp:dropdownlist>
                                </td>
                                <td class="active" colspan="2">

                                    <asp:RadioButton ID="chkZweigstellen" runat="server" Checked="True" 
                                        GroupName="grpVersand" Text="&lt;u&gt;Versandadresse:&lt;/u&gt;" 
                                        Visible="False" Width="150px" />

                                </td>
                            </tr>
                            <tr class="form">
                                <td class="firstLeft active" colspan="6"   style="font-weight: normal">
                                <strong><u>Hinweis:</u> </strong><br />
                                    Die Deutsche Post AG garantiert für&nbsp;Standardsendungen keine 
                                    Zustellzeiten<br />
                                    und gibt die Zustellwahrscheinlichkeit wie folgt an:&nbsp;
                                    <br />
                                    <br />
                                    &nbsp;&nbsp;&nbsp;-95% aller Sendungen werden dem Empfänger innerhalb von 24 Stunden 
                                    zugestellt,<br />
                                    &nbsp;&nbsp;&nbsp;-3% aller Sendungen benötigen zwischen 24 und 48 Stunden bis zur Zustellung.<br />
                                    <br />
                                    Bitte beachten Sie hierzu auch die Beförderungsbedingungen der Deutschen Post 
                                    AG.</td>
                            </tr>
                             <tr class="form">
                                 <td class="firstLeft active" colspan="6" style="font-weight: normal">
                                     &nbsp;</td>
                             </tr>
                        </table>
                                     </ContentTemplate>
                                     </asp:UpdatePanel>							
						</div>
						<div id="dataFooter">
                                        &nbsp;<asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" 
                                        Height="20px" Width="78px">» Suchen</asp:LinkButton>
                                    <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" 
                                        Height="20px" Width="78px">» Weiter</asp:LinkButton>
                                </div>
					</div>

				</div>
			</div>
	</div>
</div> 
</asp:Content>