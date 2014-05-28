<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02_1.aspx.vb" Inherits="CKG.Components.ComCommon.Report02_1" 
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;</div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                           
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width:100%">
                                        <tbody>
                                            <tr class="formquery">
                                                <td nowrap="nowrap" class="firstLeft active" colspan="4">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                    </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td nowrap="nowrap" class="firstLeft active" colspan="4" style="background-color: #DFDFDF;">
                                                <div style="height: 20px; vertical-align: middle;">Übersicht Auftragsdaten</div>                                                
                                                   </td>
                                            </tr>
                                            <tr class="formquery" >
                                                <td class="firstLeft active">
                                                    Auftrags-Nr.:</td>
                                                <td class="active">
                                                    <strong>
                                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                                    </strong></td>
                                                <td class="active">
                                                    Ihre Referenz:</td>
                                                <td class="active" style="width:100%">
                                                    <asp:Label ID="Label3" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                            
                                            Kennzeichen:</td>
                                                <td class="active">
                                                    <asp:Label ID="Label4" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td class="active" nowrap="nowrap">
                                                    Datum Terminvereinbarung /&nbsp;mit:</td>
                                                <td class="active">
                                                    <asp:Label ID="Label11" runat="server" Font-Bold="True"></asp:Label>
                                                    <asp:Label ID="Label18" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Fahrzeugmodell:</td>
                                                <td class="active">
                                                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Width="188px"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    Vereinbarter Zeitraum:</td>
                                                <td class="active">
                                                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_TextLN" Runat="server">Leasingkunde</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="Label7" runat="server" Font-Bold="True" Width="188px"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    Leasinggesellschaft</td>
                                                <td class="active">
                                                    <asp:Label ID="lbl_DataLG" Runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>


                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKfall" runat="server" Visible="False">Klärfall</asp:Label>
                                                </td>
                                                <td class="active" colspan="3">
                                                    <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>      
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="4">
                                                    Übersicht Auftragsdaten </td>
                                            </tr>           
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Abholort:</td>
                                                <td class="active">
                                                    <asp:Label ID="Label8" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    Abgabeort:</td>
                                                <td class="active">
                                                    <asp:Label ID="Label9" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>                                                                                                                      
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Abholung am:</td>
                                                <td class="active">
                                                    <asp:Label ID="Label19" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    Abholung am:</td>
                                                <td class="active">
                                                    <asp:Label ID="Label10" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="4" style="background-color: #DFDFDF;">
                                                 <div style="height: 20px; vertical-align: middle;">Zusatzinformationen</div> </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    
                                                    Kundenberater:</td>
                                                <td class="firstLeft active">
                                                    
                                                    <asp:Label ID="Label14" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    
                                                    Telefon-Nr.:</td>
                                                <td class="firstLeft active">
                                                    
                                                    <asp:Label ID="Label15" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                         
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    
                                                    Gefahrene Km:</td>
                                                <td class="firstLeft active">
                                                    
                                                    <asp:Label ID="Label13" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    
                                                    eMail:</td>
                                                <td class="firstLeft active">
                                                    
                                                    <asp:Label ID="Label16" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                         
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    
                                                    &nbsp;</td>
                                                <td class="firstLeft active">
                                                    
                                                    &nbsp;</td>
                                                <td class="active">
                                                    
                                                    Fahrt-Nr.:</td>
                                                <td class="firstLeft active">
                                                    
                                                    <asp:Label ID="Label2" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                         
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="background-color: #DFDFDF;" colspan="4">
                                                   <div style="height: 20px; vertical-align: middle;"> Archivierte Dokumente</div></td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Übergabeprotokolle</td>
                                                <td class="active">
                                                    <asp:ImageButton ID="btnShowProtokoll" runat="server" 
                                                        ImageUrl="../../../Images/Dokumente03_09.jpg" Height="24px" Width="24px" />
                                                </td>
                                                <td class="active">
                                                      <strong>Vorschau:</strong>
                                                </td>
                                                <td class="active">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Digitalfotos</td>
                                                <td class="active">
                                                    <asp:ImageButton ID="btnShowPics" runat="server" 
                                                        ImageUrl="../../../Images/Fotos.jpg" Height="24px" Width="24px" />
                                                </td>
                                                <td class="active"  valign="top" colspan="2" rowspan="4">
                                                    <div align="left"
                                                        style="width: 130px; vertical-align:top">
                                                        <table id="tblPreview" border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td id="tCell" runat="server">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                   </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;</td>
                                                <td class="active">
                                                    <asp:ImageButton ID="btnShowAbmeldung" runat="server" 
                                                        ImageUrl="../../../Images/iconpdf.gif" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <input id="txtHidden" type="hidden" name="txtHidden" runat="server" />
                                                    <input id="txtType" type="hidden" name="txtType" runat="server" />
                                                    </td>
                                                <td class="active">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblScript" runat="server"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Literal ID="litScript" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>

                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
    			<script language="JavaScript">
				<!-- //
    			    function SetKey(File) {
    			        var oHidden = document.getElementById("ctl00_ContentPlaceHolder1_txtHidden")
    			        oHidden.value = File;
				}
				//-->
				</script>
</asp:Content>
