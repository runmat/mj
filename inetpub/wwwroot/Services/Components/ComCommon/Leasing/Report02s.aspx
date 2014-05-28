<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02s.aspx.vb" Inherits="CKG.Components.ComCommon.Report02s"
    MasterPageFile="../../../MasterPage/Services.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <style type="text/css">
        div#DivchkList
        {
           overflow:auto; 
           height:225px; 
           width:75%; 

           border: solid 1px #dfdfdf
              }
      
   </style>
    <div id="site">

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
                    <asp:Panel ID="Panel1" runat="server">
                        <div id="TableQuery" style="margin-bottom: 10px">
                                                        
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                <tr class="formquery">
                                   <td class="firstLeft active" colspan="2" >
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                               <tr class="formquery">
                                    <td class="firstLeft active" >
                                        Datum von
                                    </td>
                                    <td class="active" style="width:100%">
                                        <asp:TextBox ID="txtDatumVon" runat="server"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                                                </ajaxToolkit:CalendarExtender> 
                                                <ajaxToolkit:MaskedEditExtender ID="MEE_DatumVon" runat="server" TargetControlID="txtDatumVon"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </ajaxToolkit:MaskedEditExtender> 
                                    </td>

                                    
                               </tr>
                               <tr class="formquery">
                                    <td class="firstLeft active">
                                        Datum bis
                                    </td>
                                    <td class="active" >
                                        <asp:TextBox ID="txtDatumBis" runat="server"></asp:TextBox>
                                            <asp:CompareValidator ID="cv_txtDatumVon" runat="server" ErrorMessage="'Datum bis' muß größer sein als 'Datum von' "
                                                Type="Date" ControlToValidate="txtDatumVon" ControlToCompare="txtDatumBis" Operator="LessThan"
                                                CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                                                                                
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                                </ajaxToolkit:CalendarExtender> 
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDatumBis"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </ajaxToolkit:MaskedEditExtender> 
                                                
                                    </td>
                                    
                               </tr>
                               
                                    <tr class="formquery">
                                        <td  class="firstLeft active">
                                           <div style = "height:200px;"> Leistungsarten</div></td>
                                        <td  >

                                            <div id="DivchkList">
                                            <asp:CheckBoxList  RepeatLayout="Table" ID="chkListLeistung" runat="server" Width="90%" >
                                            </asp:CheckBoxList>     
                                             </div>                                
                                           </td> 

                                    </tr>
                                    <tr class="formquery">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td class="active" style="padding-top:35px;" >
                                            <span style="padding-left:3px;">
                                                <asp:CheckBox ID="chkOffene" Visible="false" Checked="true" runat="server" Text="End- und Teilrückmeldungen" />
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="height: 19px"  colspan="2">
                                            &nbsp;
                                        </td>
                                        
                                    </tr>
                                    <tr class="formquery">
                                        <td  colspan="2">
                                            <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                Width="1px" />
                                        </td>
                                    </tr>
                                  
                              </table>
                             
                            <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                 
                        <div class="ExcelDiv">
                            <asp:Label ID="lblAnzahl" style=" float:left; padding-left:15px" Font-Bold="true" runat="server" Text="Gesamtanzahl: "></asp:Label>
                            <div style="float:right;margin-bottom:15px"  class="rightPadding">
                                <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                </span>
<%--                                <img style="width:14px;height:14px;" src="../../../Images/Diagramm01_07.jpg" alt="Diagramm" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreatePDF1" ForeColor="White" runat="server" 
                                    >Diagramm</asp:LinkButton>
                                </span>--%>
                            </div>
                        </div>
                        <div id="pagination">

                           <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                          
                            <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvBestand" Width="100%" runat="server" CellPadding="1" CellSpacing="1"
                                            GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0" AllowSorting="True"
                                            AllowPaging="True"  PageSize="20" AutoGenerateColumns="True">
                                            <HeaderStyle BackColor="#9b9b9b" ForeColor="White" Height="30px" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />

                                            <Columns>
                                            
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>


                            
                            
                        </div>
                    </div>
                    
                    <div id="dataFooter">
               
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
