<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report_002_02.aspx.cs"
    Inherits="Leasing.forms.Report_002_02" EnableEventValidation="false" MasterPageFile="../../../MasterPage/Services.Master" %>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
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
                                <asp:Label ID="lblPageTitle" Text="(Details)" runat="server"></asp:Label>
                            </h1>
                        </div>                        
                        <div id="TableQuery">
                            <div id="Error" style="border-left:1px solid #DFDFDF; border-right:1px solid #DFDFDF;">
                                &nbsp;
                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                &nbsp;
                            </div>
                          
                            


                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label8" runat="server">LV-Nr. / Status</asp:Label>:
                                    </td>
                                    <td class="active">
                                        <asp:Label ID="lblLVNr" runat="server" />
                                        &nbsp;/&nbsp;
                                        <asp:Label ID="lblStatus" runat="server" />
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label17" runat="server">Angelegt am:</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:Label ID="lblAntrag" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label25" runat="server">Leasingdauer:</asp:Label>
                                    </td>
                                    <td class="active" style="width: 100%">
                                        <asp:Label ID="lblLBeginn" runat="server"></asp:Label>&nbsp;-
                                        <asp:Label ID="lblLEnde" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="6" style="height: 10px">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="6" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                        <span style="font-weight: bold">Leasingnehmer</span>
                                    </td>
                                </tr> 

                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label10" runat="server">Name:</asp:Label>
                                    </td>
                                    <td  class="active" style="font-weight:normal" >
                                        <asp:Label ID="lblNameLN" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px" >
                                        <asp:Label ID="Label24" runat="server"> Kunden-Nr.:</asp:Label>
                                    </td>
                                    <td colspan="3" >
                                        <asp:Label ID="lblKonzernID" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label37" runat="server">Name 2:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblName2LN" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                        <asp:Label ID="Label18" runat="server">Versand:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblVersandLN" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label38" runat="server">Name 3:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblName3LN" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                        <asp:Label ID="Label42" runat="server">Rückgabe:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblRueckLN" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label22" runat="server">Straße:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblStrLN" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                        <asp:Label ID="Label19" runat="server"> Eingang unvollständig:</asp:Label>
                                    </td>
                                    <td class="active" colspan="3" >
                                        <asp:DropDownList ID="lblUnvLN" runat="server" style="width:auto" />
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label21" runat="server"> Postleitzahl:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPLZLN" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    </td>
                                    <td class="active" colspan="3">
                                    </td>
                                </tr>
                                <tr id="Tr1" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label23" runat="server">Ort:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOrtLN" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    </td>
                                    <td class="active" colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px"  colspan="6">
                                    
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="6" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                        <span style="font-weight: bold">Halter</span>
                                    </td>
                                </tr> 
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                    <asp:Label ID="Label28" runat="server">Name:</asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblName1" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    
                                        <asp:Label ID="Label32" runat="server">Postleitlzahl:</asp:Label>
                                    
                                    </td>
                                    <td colspan="3" >
                                        
                                        <asp:Label ID="lblOrt_ZO" runat="server"></asp:Label>
                                        
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label29" runat="server">Name 2:</asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblName2" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    
                                        <asp:Label ID="Label33" runat="server">Ort:</asp:Label>
                                    
                                    </td>
                                    <td colspan="3" >
                                        
                                        <asp:Label ID="lblPstlz_ZO" runat="server"></asp:Label>
                                        
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label30" runat="server">Name 3:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblName3" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    
                                        <asp:Label ID="Label34" runat="server">Kunden-Nr:</asp:Label>
                                    
                                    </td>
                                    <td colspan="3" >
                                        
                                        <asp:Label ID="lblKonzs_ZO" runat="server"></asp:Label>
                                        
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label31" runat="server">Straße:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblStras_ZO" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    
                                    </td>
                                    <td colspan="3" >
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px"  colspan="6">
                                    
                                        &nbsp;</td>
                                </tr>  
                                <tr>
                                    <td colspan="6" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                        <span style="font-weight: bold">Versicherungsgeber</span>
                                    </td>
                                </tr> 
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label13" runat="server">Versich.Schein-Nr.:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVschein" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    
                                        <asp:Label ID="Label15" runat="server">Versand:</asp:Label>
                                    
                                    </td>
                                    <td colspan="3">
                                        
                                        <asp:Label ID="lblVersandVG" runat="server"></asp:Label>
                                        
                                    </td>
                                </tr>   
                                
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label14" runat="server">Versicherungsdauer:</asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblVBeginn" runat="server"></asp:Label>&nbsp;-
                                        <asp:Label ID="lblVEnde" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    
                                        <asp:Label ID="Label43" runat="server">Rückgabe:</asp:Label>
                                    
                                    </td>
                                    <td colspan="3">
                                        
                                        <asp:Label ID="lblRueckVG" runat="server"></asp:Label>
                                        
                                    </td>
                                </tr>  
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                      
                                        <asp:Label ID="Label9" runat="server">Name:</asp:Label>
                                      
                                    </td>
                                    <td >
                                       
                                        <asp:Label ID="lblNameVG" runat="server"></asp:Label>
                                       
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    
                                        <asp:Label ID="Label16" runat="server">Eingang unvollständig:</asp:Label>
                                    
                                    </td>
                                    <td colspan="3">
                                        
                                        <asp:DropDownList ID="lblUnvVG" runat="server" style="width:auto" />
                                        
                                    </td>
                                </tr>  
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                       
                                        <asp:Label ID="Label11" runat="server"> Straße:</asp:Label>
                                       
                                    </td>
                                    <td >
                                       
                                        <asp:Label ID="lblStrVG" runat="server"></asp:Label>
                                       
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    
                                    </td>
                                    <td class="active" colspan="3">
                                        
                                    </td>
                                </tr>                                                                                                                                                                                                                                                                                      
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                       
                                        <asp:Label ID="Label12" runat="server">Postleitzahl:</asp:Label>
                                       
                                    </td>
                                    <td >
                                       
                                        <asp:Label ID="lblPLZVG" runat="server"></asp:Label>
                                       
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    
                                        &nbsp;</td>
                                    <td class="active" colspan="3">
                                        
                                        &nbsp;</td>
                                </tr>                                                                                                                                                                                                                                                                                      
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                       
                                        <asp:Label ID="Label39" runat="server">Ort:</asp:Label>
                                       
                                    </td>
                                    <td >
                                       
                                        <asp:Label ID="lblOrtVG" runat="server"></asp:Label>
                                       
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                    
                                        &nbsp;</td>
                                    <td class="active" colspan="3">
                                        
                                        &nbsp;</td>
                                </tr>                                                                                                                                                                                                                                                                                      
 
                                <tr class="formquery">
                                    <td class="firstLeft active" style="height: 10px" colspan="6">
                                        &nbsp;</td>
                                </tr>

                                 
                                 <tr>
                                    <td colspan="6" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                        <span style="font-weight: bold">Mahndaten</span>
                                    </td>
                                </tr> 

                               
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label26" runat="server">Zuletzt gemahnt / Stufe:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMadatLN" runat="server"></asp:Label>
                                        &nbsp;/&nbsp;
                                        <asp:Label ID="lblMahnsLN" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label27" runat="server">Zuletzt gemahnt / Stufe:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblMadatVG" runat="server"></asp:Label>
                                        &nbsp;/&nbsp;
                                        <asp:Label ID="lblMahnsVG" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px"  colspan="6">
                                    
                                        &nbsp;</td>
                                </tr>
                                 <tr>
                                    <td colspan="6" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                        </td>
                                </tr> 
                                <tr class="formquery">
                                    <td class="firstLeft active" >
                                        <asp:Label ID="Label36" runat="server">Kundenbetreuer(in):</asp:Label>
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:Label ID="lblKonzs_ZK" runat="server"></asp:Label>&nbsp;
                                        <asp:Label ID="lblName1_ZK" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label20" runat="server">Versicherungsumfang:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblVersUmf" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="height: 5px"  colspan="6">
                                    
                                        &nbsp;</td>
                                </tr>
                                <tr >
                                            <td  colspan="6" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                        <span style="font-weight: bold">Fahrzeugdaten</span></td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label4" runat="server"> Erstzulassung:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEz" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                        <asp:Label ID="Label41" runat="server">Fahrzeugart:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblFzArt" runat="server"></asp:Label>
                                    </td>                                    
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label40" runat="server">Hersteller / Typ:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblHerst" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 154px">
                                        <asp:Label ID="Label2" runat="server">Fahrgestellnr.:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblFGNr" runat="server"></asp:Label>
                                    </td>                                    
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label3" runat="server">Kennzeichen:</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblKennz" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px"  colspan="6">
                                    
                                        &nbsp;</td>
                                </tr>
                                <tr id="trBemerkungen" runat="server" >
                                            <td  colspan="6" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                         <span style="font-weight: bold">Bemerkungen</span></td>
                                </tr>
                                <tr id="trBemerkungenShow" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblInfo" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:Label ID="lblB1" runat="server"></asp:Label>
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:Label ID="lblB2" runat="server"></asp:Label>
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:Label ID="lblB3" runat="server"></asp:Label>
                                    </td>
                                    <td nowrap="nowrap" width="100%" colspan="2">
                                        <asp:Label ID="lblB4" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trBemerkungenddl" runat="server"  class="formquery">
                                    <td class="firstLeft" colspan="6">
                                        <asp:DropDownList ID="ddl1" runat="server"  style="width:auto"/>
                                    </td>
                                </tr>
                                <tr class="formquery" id="trBemerkungenErfassen" runat="server">
                                    <td class="firstLeft active" colspan="6">
                                        <asp:Label ID="lblBemerkungen" runat="server">Bemerkungen erfassen:</asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery" id="trtxtBem1" runat="server">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        1:
                                    </td>
                                    <td nowrap="nowrap" class="active">
                                        <asp:TextBox ID="txtBem1" runat="server" MaxLength="25"></asp:TextBox>
                                    </td>
                                    <td class="firstLeft active" nowrap="nowrap" style="width: 154px">
                                        2:
                                    </td>
                                    <td nowrap="nowrap" class="active" colspan="3">
                                        <asp:TextBox ID="txtBem2" runat="server" MaxLength="25"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery" id="trtxtBem2" runat="server">
                                    <td nowrap="nowrap" class="firstLeft active">
                                        3:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtBem3" runat="server" MaxLength="25"></asp:TextBox>
                                    </td>
                                    <td nowrap="nowrap" class="firstLeft active" style="width: 154px">
                                        4:
                                    </td>
                                    <td class="active" colspan="3">
                                        <asp:TextBox ID="txtBem4" runat="server" MaxLength="25"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td nowrap="nowrap" class="firstLeft active" colspan="6">
                                        &nbsp;</td>
                                </tr>                   
                            </table>

                        </div>
                        <div id="Div7" runat="server">
                            &nbsp;
                        </div>
                    </div>
                        <div id="dataFooter" style="float:right; padding-left:15px;">
                            <asp:LinkButton ID="btnSave" runat="server" CssClass="Tablebutton" Width="78px" Height="16px">Speichern</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:content>
