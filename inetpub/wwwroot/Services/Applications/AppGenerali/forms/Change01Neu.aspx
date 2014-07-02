<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01Neu.aspx.vb"  Inherits="AppGenerali.Change01Neu"  MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
		<div id="site">
		<div id="content">
				<div id="navigationSubmenu">
	&nbsp;
				</div>
		
				<div id="innerContent">

	
					<div id="innerContentRight" style="width:100%;">
						<div id="innerContentRightHeading">
							<h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
														
						</div>

						<div id="pagination">
							<table cellpadding="0" cellspacing="0">
								<tbody>
									<tr>
										<td class="active">Bitte die Versandadresse eintragen!</td>
									</tr>
								</tbody>
							</table>
						</div>
						<div id="data">
						 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
							<table cellpadding="" cellspacing="0" 
                                style="border-right-width: 1px; border-left-width: 1px; border-left-style: solid; border-right-style: solid; border-right-color: #DFDFDF; border-left-color: #DFDFDF" >
							<tfoot ><tr><td colspan="4">&nbsp;</td></tr></tfoot>
								<tbody>

									<tr><td colspan="4">
                                        <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td></tr>

									<tr>
										<td>&nbsp;</td>
										<td >&nbsp;</td>
										<td >&nbsp;</td>
										<td >&nbsp;</td>
									</tr>
									<tr class="" >
										<td class="firstLeft active" >
                                            <asp:Label ID="lbl_Anrede" runat="server">Anrede:</asp:Label>
                                                                </td>
										<td >
                                            <asp:RadioButtonList ID="rblAnrede" runat="server" 
                                                BorderColor="White" CellPadding="0" CellSpacing="0" Font-Bold="False" 
                                                RepeatDirection="Horizontal" >
                                                <asp:ListItem  Text="Firma" Value="Firma"></asp:ListItem>
                                                <asp:ListItem Text="Herr" Value="Herr"></asp:ListItem>
                                                <asp:ListItem Text="Frau" Value="Frau"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
										<td class="firstLeft active" >
                                            <asp:Label ID="lbl_AnzahlKennzeichen" runat="server">Anzahl Kennzeichen:</asp:Label>
                                        </td>
										<td class="active" >
                                            <asp:DropDownList ID="ddlAnzahlKennzeichen" runat="server" Width="250px" 
                                                Font-Names="Verdana,sans-serif">
                                                <asp:ListItem>1</asp:ListItem>
                                                <asp:ListItem>2</asp:ListItem>
                                                <asp:ListItem>3</asp:ListItem>
                                                <asp:ListItem>4</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
									</tr>
									<tr class="form">
										<td class="firstLeft active" >
                                            <asp:Label ID="lbl_Vorname" runat="server">Firma (o.Name1):</asp:Label>
                                                                </td>
										<td class="active" nowrap="nowrap">
                                            <asp:TextBox ID="txtVorname" runat="server" MaxLength="40" Width="277px" 
                                                CssClass="InputTextbox"></asp:TextBox>
                                            <asp:Image ID="Image1"  ImageUrl="../../../Images/Info01_06.jpg" runat="server" 
                                                ToolTip="(z.B. Agentur etc.&nbsp;oder Erika Mustermann)" Height="20px" Width="20px" 
                                                ImageAlign="Bottom" AlternateText="Info" />                                                
                                        </td>
										<td class="firstLeft active">
                                            <asp:Label ID="lbl_Vermittlernummer" runat="server">VD / Bezirk:</asp:Label>
                                        </td>
										<td class="active">
                                            <asp:TextBox ID="txtVermittlernummer1" runat="server" MaxLength="3" 
                                                Width="60px" CssClass="InputTextbox"></asp:TextBox>&nbsp;
                                            <asp:TextBox ID="txtVermittlernummer2" runat="server" MaxLength="5" 
                                                Width="180px" CssClass="InputTextbox"></asp:TextBox>
                                        </td>
									</tr>
									<tr class="form">
										<td class="firstLeft active"><asp:label id="lbl_Name" runat="server">Name 2:</asp:label>
                                                                </td>
										<td class="active">
                                            <asp:textbox id="txtName" runat="server" 
                                                                    MaxLength="40" Width="277px" 
                                                CssClass="InputTextbox"></asp:textbox>
                                            <asp:Image ID="Image2"  ImageUrl="../../../Images/Info01_06.jpg" runat="server" 
                                                ToolTip="(z.B. Erika Mustermann)" Height="20px" Width="20px" 
                                                ImageAlign="Bottom" AlternateText="Info" />
                                        </td>
										<td  class="firstLeft active" rowspan="2">
                                            <asp:Label ID="lbl_Versandart" runat="server">Versandart:</asp:Label>
                                            &nbsp;
                                            </td>
										<td class="rightPadding"  rowspan="3" align="left" >
                                            <asp:radiobutton id="rbNormal" runat="server" GroupName="grpVersandart" 
                                                Checked="True"  Font-Bold="True" Text="Normal"></asp:radiobutton>
                                                 &nbsp;(Versandkosten trägt &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Versicherungsgesellschaft)
                                      <br  />
                                       <br  />
                                            <asp:radiobutton  id="rbExpress" runat="server" GroupName="grpVersandart" 
                                                Text="Express"  Font-Bold="True" BorderColor="White" BorderStyle="None"></asp:radiobutton>
                                                 &nbsp;(Versandkosten in Höhe von &nbsp;&nbsp;&nbsp;&nbsp; 38,68 Euro trägt der Vermittler)
                                                </td>
									</tr>
                                    <tr class="form">
                                        <td class="firstLeft active" >
                                            <asp:Label ID="lbl_Strasse" runat="server">Strasse / Nr.:</asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtStrasse" runat="server" MaxLength="60" Width="206px" 
                                                CssClass="InputTextbox"></asp:TextBox>
                                        &nbsp;
                                            <asp:TextBox ID="txtHausnummer" runat="server" MaxLength="10" Width="60px" 
                                                CssClass="InputTextbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="form">
                                        <td class="firstLeft active" >
                                            <asp:Label ID="lbl_Postleitzahl" runat="server">Postleitzahl / Ort:</asp:Label>
                                        </td>
                                        <td  class="active">
                                            <asp:TextBox ID="txtPostleitzahl" runat="server" MaxLength="5" Width="60px" 
                                                CssClass="InputTextbox"></asp:TextBox>
                                            &nbsp;
                                            <asp:TextBox ID="txtOrt" runat="server" MaxLength="40" Width="206px" 
                                                CssClass="InputTextbox" ></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                            </td>
                                    </tr>
                                    <tr class="form">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Tel" runat="server">Telefon:</asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txt_Tel" runat="server" Width="277px" 
                                                CssClass="InputTextbox"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active" >
                                            &nbsp;
                                        </td>
                                        <td class="rightPadding" rowspan="9">
                                            <strong><u>Hinweis:<br/>
                                            </u></strong>Die Deutsche Post AG garantiert für&nbsp;<b><i>Standardsendungen</i></b>
                                            keine Zustellzeiten<br/>
                                            und gibt die Zustellwahrscheinlichkeit wie folgt an:&nbsp;
                                            <br/><br/>
                                            &nbsp;&nbsp;&nbsp;- 95% aller Sendungen werden dem <br /> &nbsp;&nbsp;&nbsp;&nbsp; Empfänger innerhalb von 24 Stunden
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;zugestellt,<br />
                                            &nbsp;&nbsp;&nbsp;- 3% aller Sendungen benötigen &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;zwischen 24 und 48 Stunden bis
                                            zur<br/> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Zustellung.<br />
                                            <br />
                                            <span lang="de"><b><i>Expresssendungen</i></b> erfolgen bis 12:00 Uhr des nächsten Tages.
                                                Vorraussetzung für eine zeitgerechte Lieferung ist der Bestelleingang bis 16:00
                                                Uhr.</span>
                                            &nbsp;
                                            &nbsp;
                                            &nbsp;
                                            &nbsp;
                                            &nbsp;
                                                                                
                                            &nbsp;&nbsp;</td>                                        
                                    </tr>
                                    <tr class="form">
                                        <td class="firstLeft active" >
                                            <asp:Label ID="lbl_EmailAdresse" runat="server">E-Mail:</asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtEmailAdresse" runat="server" MaxLength="241" Width="277px" 
                                                CssClass="InputTextbox"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                    </tr>
                                    <tr class="form">
                                        <td class="firstLeft active" >
                                            <asp:Label ID="lbl_KeineEmailVorhanden" runat="server">Keine E-Mail vorhanden:</asp:Label>
                                        </td>
                                        <td class="active"  align="left" >
                                            <asp:CheckBox ID="chkKeineEmailVorhanden"  runat="server" BorderStyle="None" 
                                                Width="50px" BorderColor="White"></asp:CheckBox>
                                        </td>
                                        <td class="active">
                                            &nbsp;
                                        </td>
                                    </tr>

                                    <tr class="form">
										<td class="firstLeft active" >
                                            abw. Versandadresse:</td>
										<td class="active" align="left" style="vertical-align: top;">
                                            <asp:CheckBox ID="chkAdresse" runat="server" AutoPostBack="True" 
                                                BorderColor="White"  Width="50px" BorderStyle="None" />
                                        </td>
                                        <td class="active">
                                            &nbsp;
                                        </td>										
									</tr>
																																				
									<tr id="trWEAnrede">
										<td class="firstLeft active" style="padding-top: 10px">
                                            <asp:Label ID="lbl_WEAnrede" visible="False" runat="server">Anrede:</asp:Label>
                                        </td>
										<td style="padding-top: 10px">
                                            <asp:RadioButtonList ID="rblWE_Anrede"  visible="false" runat="server" 
                                                BorderColor="White" CellPadding="0" CellSpacing="0" Font-Bold="False" 
                                                RepeatDirection="Horizontal" >
                                                <asp:ListItem  Text="Firma" Value="Firma"></asp:ListItem>
                                                <asp:ListItem Text="Herr" Value="Herr"></asp:ListItem>
                                                <asp:ListItem Text="Frau" Value="Frau"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
										                                        <td class="active" style="padding-top: 10px">
                                            &nbsp;
                                        </td>
									</tr>																											
									<tr id="trWEName"   runat="server" class="form">
										<td class="firstLeft active">
                                            <asp:Label ID="lbl_WEVorname" visible="False" runat="server">Firma (o.Name1):</asp:Label>
                                        </td>
										<td>
                                            <asp:TextBox ID="txtWE_Vorname" visible="false" runat="server" MaxLength="40" Width="277px" 
                                                CssClass="InputTextbox"></asp:TextBox>
                                            </td>
										<td>&nbsp;</td>
									</tr>
									
                                    <tr id="trWEName2" runat="server" class="form">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_WEName" visible="False" runat="server">Name 2:</asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:textbox id="txtWE_Name" visible="false" runat="server" 
                                                                    MaxLength="40" Width="277px" 
                                                CssClass="InputTextbox"></asp:textbox>
                                            </td>
                                        <td class="active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr id="trWEStrasse"  runat="server" class="form">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_WEStrasse"  visible="False" runat="server">Strasse / Nr.:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWE_Strasse" runat="server"  visible="false" MaxLength="60" Width="206px" 
                                                CssClass="InputTextbox"></asp:TextBox>
                                            &nbsp;<asp:TextBox ID="txtWE_Hausnummer" runat="server"  visible="false" MaxLength="10" Width="60px" 
                                                CssClass="InputTextbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr id="trWEOrt" runat="server" class="form">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_WEPostleitzahl"   visible="False"  runat="server">Postleitzahl / Ort:</asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtWE_Postleitzahl" runat="server"  visible="false"  MaxLength="5" Width="60px" 
                                                CssClass="InputTextbox"></asp:TextBox>
                                            &nbsp; <asp:TextBox ID="txtWE_Ort" runat="server"  visible="false"  MaxLength="40" Width="206px" 
                                                CssClass="InputTextbox" ></asp:TextBox></td>
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                    </tr>
                                    <tr id="trWETel" runat="server" class="form">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_WETel"  visible="False" runat="server">Telefon:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWE_Tel"  visible="false" runat="server" Width="277px" 
                                                CssClass="InputTextbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td class="rightPadding" align="right">
                                        
                                            &nbsp;</td>
                                    </tr>

                                    
                                    <tr class="form">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td class="rightPadding" align="right">
                                        
                                            <asp:LinkButton ID="cmdContinue" runat="server" CssClass="Tablebutton" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Weiter" Width="78px"></asp:LinkButton>
                                             <asp:LinkButton ID="cmdConfirm" runat="server" CssClass="Tablebutton" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Absenden" Width="78px" Visible="False"></asp:LinkButton>
                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                        <td align="right">
                                        
                                            &nbsp;&nbsp;</td>
                                    </tr>
								</tbody>
							</table>
                                     </ContentTemplate>
                                     </asp:UpdatePanel>							
						</div>
						<div id="dataFooter">
                                        &nbsp;</div>
					</div>

				</div>
			</div>
	</div>
</div> 
</asp:Content>