<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report03.aspx.vb" Inherits="CKG.Components.ComCommon.Report03"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:Button ID="submitFiles" runat="server" Text="Übersicht" Style="display: none;" />


<style type="text/css">
.RadUpload_Default
{
    padding-top: 2px ;
    margin-top: 2px ; 

}
 </style>

    <div id="site">
             <script type="text/javascript">
                 function showUploadStartMessage(sender, args) {

                     var filename = args.get_fileName();
                     var filext = filename.substring(filename.lastIndexOf(".") + 1);
                     if (filext == "pdf") {
                         return true;
                     } else {

                         sender._inputFile.style.backgroundColor = "red"
                         alert('Fehler beim Hochladen der Datei(nur PDF-Format erlaubt!')
                         return false;
                     }
                 }  

        function validationFailed(sender, eventArgs) {
            alert("Es werden nur *.PDF Dateien unterstützt.\n\nDie Datei wird nicht hochgeladen.");
        }


             </script>

        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" CausesValidation="false"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Panel ID="Panel3" runat="server">
                                <div id="paginationQuery">
                                    <table cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td class="active">
                                                    Neue Abfrage starten
                                                </td>
                                                <td align="right">
                                                    <div id="queryImage">
                                                        <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </asp:Panel>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <asp:Panel ID="Panel2" runat="server" Visible="false">
                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                        <tr class="formquery" id="trErrorSearch" runat="server" visible="false">
                                            <td colspan="3" class="firstLeft active">
                                                <asp:Label ID="lblUploadMessage1" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Auftragsnummer:
                                            </td>

                                            <td class="active" style="width: 100%">
                                                <asp:Label ID="lblAuftragsnr" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Fahrt:&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:Label ID="lblFahrt" runat="server"></asp:Label>
                                                <asp:Label ID="lblFahrtnr" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                            </td>
                                            <td class="active" align="right" colspan="2">
                                                &nbsp;</td>
                                        </tr>                                        
                                        <tr class="formquery">
                                        <td colspan="2" style=" padding-top: 0px; padding-left: 7px; padding-right: 3px">
                                            <div class="StandardHeadDetail">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label21" style="margin-left:15px; padding-bottom:5px" ForeColor="White"  runat="server" 
                                                                Font-Size="12px" Font-Bold="True">Dateiupload</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                                <div class="StandardHeadDetailFlag" style="background-image: url(/Services/Images/Versand/overflowbackwhite.png);">
                                                </div>                                            
                                            </td>
                                        </tr>  
                                                                              
                                     <tr>
                                        
                                         <td class="firstLeft active" colspan="2" style="padding-top:15px">
                                              <asp:GridView ID="grvProtokollUpload1" runat="server" AutoGenerateColumns="False" 
                                                GridLines="none" Width="100%" BackColor="#ffffff" ShowHeader="true"
                                                    CellPadding="0" CellSpacing="0" AlternatingRowStyle-BackColor="#DEE1E0">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                   <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                   <RowStyle CssClass="ItemStyle" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="Nr." HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10px" ItemStyle-Width="10px"  />
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFahrt" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.Fahrt")  %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                                                                                        
                                                    <asp:BoundField DataField="ZZPROTOKOLLART" HeaderText="Dokumente"  HeaderStyle-Width="80px" ItemStyle-Width="80px"   HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:TemplateField HeaderText="Datei" >
                                                            <ItemTemplate>
                                                                  <telerik:RadAsyncUpload runat="server" ID="radUpload1" 
                                                                    AllowedFileExtensions="*.pdf" MaxFileInputsCount="1"
                                                                      MultipleFileSelection="Disabled" 
                                                                      OnClientFileUploaded="onFileUploaded" 
                                                                      OnFileUploaded="UploadedComplete" 
                                                                      OnClientFileUploadFailed="onUploadFailed"
                                                                      OnClientValidationFailed="validationFailed" 
                                                                      Visible='<%# DataBinder.Eval(Container, "DataItem.Filename")= "" %>' 
                                                                      DisablePlugins="True"
                                                                      Width="400px" InputSize="40" Culture="de-DE" Font-Size="Small" 
                                                                      Localization-Cancel="Abbrechen" Localization-Remove="Löschen" Localization-Select="Auswählen" UploadedFilesRendering="AboveFileInput" PersistConfiguration="True" RegisterWithScriptManager="True">
                                                                      <Localization Select="Auswählen" Remove="Löschen" Cancel="Abbrechen"  />
                                                                      </telerik:RadAsyncUpload>
                                                                     <asp:Label Width="350px" ID="lblUplFile" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Filename")<> "" %>'
                                                                         Text='<%# System.IO.Path.GetFilename(DataBinder.Eval(Container, "DataItem.Filename"))  %>' />
                    
                                                             <%--       <ajaxToolkit:AsyncFileUpload runat="server" Visible='<% # DataBinder.Eval(Container, "DataItem.Filename")= "" %>'
                                                                     ID="AsyncFileUpload1" Width="480px" UploaderStyle="Traditional"
                                                                     CompleteBackColor="#00A300" OnClientUploadComplete="showUploadStartMessage"   ThrobberID="Throbber" 
                                                                        OnUploadedComplete = "AsyncFileUpload1_UploadedComplete2" 
                                                                        ToolTip='<%# CType(Container, GridViewRow).RowIndex %>' 
                                                                         />                                                                                     
                                                                    <asp:Label ID="Throbber" runat="server" Style="display: none">
                                                                        <img  id="imgWait"  alt="warten.." src="/Services/Images/indicator.gif"/>
                                                                    </asp:Label> 
                                                                    <asp:Label ID="Label43" runat="server" Visible='<% # DataBinder.Eval(Container, "DataItem.Filename")<> "" %>'  Text='<% # DataBinder.Eval(Container, "DataItem.Filename") %>'></asp:Label>
--%>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Löschen"  HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelUploadFile1" visible='<% # DataBinder.Eval(Container, "DataItem.FileName") <> "" %>' CommandArgument='<% # DataBinder.Eval(Container, "DataItem.ID")  %>' CommandName="Loeschen" runat="server" ImageUrl="/Services/Images/del.png"  />
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblKategorie" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.ZZKATEGORIE")  %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                                                                                                         
                                                </Columns>
                                            </asp:GridView>
                                            <div class="dataQueryFooter" style="padding-right: 5px">
                                                <asp:LinkButton ID="cmdCancel" runat="server"  CssClass="TablebuttonLarge" Height="20px" Width="128px">» Übersicht </asp:LinkButton>                                                    
                                            </div>                                              
                                            </td>
                                        </tr>

                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td align="right" class="active" colspan="2">
                                                &nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                   
                                </asp:Panel>
                                <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                   <table id="tabErr" runat="server" cellpadding="0" cellspacing="0" style="border-bottom:none">
                                    <tr class="formquery">
                                                <td nowrap="nowrap" class="firstLeft active" colspan="2" style="border:none important!" >
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                    <asp:CompareValidator ID="CompareValidator5" runat="server" 
                                                        ControlToCompare="txtAuftragdatumBis" ControlToValidate="txtAuftragdatum" 
                                                        CssClass="TextError" 
                                                        ErrorMessage="'Datum von' kann darf nicht größer als 'Datum bis' sein!" 
                                                        Font-Bold="True" ForeColor="" Operator="LessThanEqual" Type="Date"></asp:CompareValidator>
                                                </td>
  
                                            </tr>
                                
                                   </table>

                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery" id="trKUNNR" runat="server">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_KundenNr" runat="server">Kunde</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:DropDownList ID="cmb_KundenNr" runat="server" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblAuftrag" runat="server">Auftragsnr.</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtAuftrag" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblReferenz" runat="server">Referenz</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtReferenz" runat="server" CssClass="TextBoxNormal" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKennzeichen" runat="server">Kennzeichen</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" MaxLength="10" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblAuftragdatum" runat="server">Auftragsdatum von</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtAuftragdatum" runat="server" CssClass="TextBoxNormal"></asp:TextBox><ajaxToolkit:CalendarExtender
                                                        ID="txtDatumVon_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtAuftragdatum">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <span>
                                                        <asp:CompareValidator ID="cv_txtDatumVon" runat="server" ControlToCompare="TextBox1"
                                                            ControlToValidate="txtAuftragdatum" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                            ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label3" runat="server">Auftragsdatum bis</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtAuftragdatumBis" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDatumBis_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtAuftragdatumBis">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cv_txtDatumBis" runat="server" ControlToCompare="TextBox1"
                                                        ControlToValidate="txtAuftragdatumBis" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                        ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label5" runat="server">Überf.- datum von</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtUeberfuehrungdatumVon" runat="server" CssClass="TextBoxNormal"></asp:TextBox><ajaxToolkit:CalendarExtender
                                                        ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtUeberfuehrungdatumVon">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="TextBox1"
                                                        ControlToValidate="txtUeberfuehrungdatumVon" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                        ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label2" runat="server">Überf.- datum bis</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtUeberfuehrungdatumBis" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                        TargetControlID="txtUeberfuehrungdatumBis">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="TextBox1"
                                                        ControlToValidate="txtUeberfuehrungdatumBis" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                        ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Leasinggesellschaft" runat="server">Leasinggesellschaft</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txt_Leasinggesellschaft" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Leasingkunde" runat="server">Leasingkunde</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txt_Leasingkunde" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox><asp:ImageButton
                                                        ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </asp:Panel>
                            </div>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen </asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton></span></div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvUeberf" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false" HeaderText="Lfd.Nr.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLfdnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Counter") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" CommandName="UploadShow" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Counter") %>'
                                                                    ImageUrl="/Services/Images/Formular.gif" Style="padding-right: 5px; padding-top: 3px;" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderStyle-Width="80px" DataField="Dokumente" SortExpression="Dokumente"
                                                            HeaderText="Dokumente"></asp:BoundField>                                                        
                                                        <asp:BoundField HeaderStyle-Width="80px" DataField="Aufnr" SortExpression="Aufnr"
                                                            HeaderText="Auftragsnr."></asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="130px" DataField="ERDAT" DataFormatString="{0:dd.MM.yyyy}"
                                                            SortExpression="ERDAT" HeaderText="Auftragsdatum" HeaderStyle-Wrap="true"></asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="50px" DataField="Fahrtnr" SortExpression="Fahrtnr"
                                                            HeaderText="Fahrt"></asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="100px" DataField="Zzkenn" SortExpression="Zzkenn"
                                                            HeaderText="Kennzeichen"></asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="50px" DataField="Zzbezei" SortExpression="Zzbezei"
                                                            HeaderText="Typ"></asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="140px" DataField="VDATU" SortExpression="VDATU"
                                                            DataFormatString="{0:dd.MM.yyyy}" HeaderText="Überführungsdat."></asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                               
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
