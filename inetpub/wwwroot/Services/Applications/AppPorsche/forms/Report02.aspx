<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02.aspx.vb" Inherits="AppPorsche.Report02"  MasterPageFile="../MasterPage/Porsche.Master" %>
<%@ Register TagPrefix="uc2" TagName="menue" Src="MenuePorsche.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
	<meta name="author" content="Christoph Kroschke AG" />

	
    <style type="text/css">
       
        #DoubleLogin
        {
            width: 73%;
        }
       
        #Table4
        {
            width: 549px;
        }
       
        </style>
</asp:Content>
 <asp:Content ID="ContentMenue" ContentPlaceHolderID="ContentPlaceHolderMenue" runat="server">
     <uc2:menue ID="menue"  runat="server"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
		<div id="site">
		<div id="content">
				<div id="navigationSubmenu">
	
				</div>
		
				<div id="innerContent">

	
					<div id="innerContentRight" style="width:100%;">
						<div id="innerContentRightHeading">
							<h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
														
						</div>
						<ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
                        EnableScriptLocalization="true" ID="ScriptManager1" />
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

									<tr class="form">
										<td class="firstLeft active">&nbsp;</td>
										<td class="active">
                                            &nbsp;</td>
                                        <td class="firstLeft active" >
                                            &nbsp;</td>
										<td class="active" align="left" style="vertical-align: top;">
                                            &nbsp;</td>                                        
									</tr>
                                    <tr class="form">
                                        <td class="firstLeft active" >
                                            &nbsp;</td>
                                        <td class="active">
                                        &nbsp;</td>

										<td class="firstLeft active">
                                            &nbsp;</td> <td>          
                                                                                    &nbsp;</td>                            
                                    </tr>
                                    <tr class="form">
                                        <td class="firstLeft active" >
                                            &nbsp;</td>
                                        <td  class="active">
                                            &nbsp;</td>
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                        <td class="active">
                                            &nbsp;</td>
                                          
                                    </tr>
                                    <tr class="form">
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                        <td class="active">
                                            &nbsp;</td>
                                        
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                        
                                    </tr>
                                    <tr class="form">
                                        <td class="firstLeft active" >
                                            &nbsp;</td>
                                        <td class="active">
                                            &nbsp;</td>
                                        
                                                                                <td class="firstLeft active">
                                                                                    &nbsp;</td>
                                        <td class="active">
                                            &nbsp;</td>
 
                                    </tr>
                                    <tr class="form">
                                        <td class="firstLeft active" >
                                            &nbsp;</td>
                                        <td class="active"  align="left" >
                                            &nbsp;</td>
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                        <td>
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
                                        
                                            <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Erstellen" Width="78px"></asp:LinkButton>
                                        
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