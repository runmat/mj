<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Report_002_01.aspx.cs"
    Inherits="Leasing.forms.Report_002_01" EnableEventValidation="false"  MasterPageFile="../Master/App.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>                               
                            </h1>
                        </div>    
                                      
                        <div id="TableQuery">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;border: none">
                                <tr class="formquery">
                                
                                    <td class="firstLeft active" style="width: 100%; height: 19px;">
                                        <asp:Label ID="lblNoData" runat="server" CssClass="TextError" Visible="false"></asp:Label><br />
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                         </div>
                         <div id="Result">
                            <div class="ExcelDiv">
                                <div align="right" class="rightPadding">
                                    <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                    <span class="ExcelSpan">
                                        <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server" 
                                        onclick="lnkCreateExcel1_Click">Excel 
                                        herunterladen</asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                             <div id="pagination" style="border-width: 0px;">
                                 <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                             </div>
                             <div id="data">

                                            <asp:GridView AutoGenerateColumns="false" BackColor="White" runat="server" ID="GridView1"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                             Style="width: auto;" onrowcommand="GridView1_RowCommand">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead" ></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                            <asp:TemplateField>
                                                  <HeaderStyle width="85"/>  
                                                   <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                         <ItemTemplate>
                                                        <asp:HyperLink ID="Hyperlink3" runat="server" Target="_blank" 
                                                            ImageUrl="/Services/Images/Lupe_16x16.gif" NavigateUrl='<%# "Report_002_02.aspx?equipment=" + DataBinder.Eval(Container.DataItem, "Equipment") + "&amp;kf=" + DataBinder.Eval(Container.DataItem, "Klaerfall") %>'>Details</asp:HyperLink>
                                                                         &nbsp;
                                                        <asp:ImageButton ID="ibtnFormular" CommandName="Formular" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Equipment") %>'  
                                                                ImageUrl="/Services/Images/Formular.gif" runat="server" 
                                                                 Visible='<%# DataBinder.Eval(Container.DataItem, "Klaerfall")!= "" %>' />
                                                        </ItemTemplate>
                                            </asp:TemplateField>  
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEquipment" Text='<%# DataBinder.Eval(Container, "DataItem.Equipment") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                            
                                                <asp:TemplateField SortExpression="Angelegt" HeaderText="col_Angelegt">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Angelegt" runat="server" CommandName="Sort" CommandArgument="Angelegt">col_Angelegt</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblAngelegt" Text='<%# DataBinder.Eval(Container, "DataItem.Angelegt", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="LVNr" HeaderText="col_LVNr">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_LVNr" runat="server" CommandName="Sort" CommandArgument="Modell">col_LVNr</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblLVNr" Text='<%# DataBinder.Eval(Container, "DataItem.LVNr") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Beginn" HeaderText="col_Beginn">
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Beginn" runat="server" CommandName="Sort" CommandArgument="Beginn">col_Beginn</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBeginn" Text='<%# DataBinder.Eval(Container, "DataItem.Beginn", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="GeplEnde" HeaderText="col_GeplEnde">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_GeplEnde" runat="server" CommandName="Sort" CommandArgument="GeplEnde">col_GeplEnde</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblGeplEnde" Text='<%# DataBinder.Eval(Container, "DataItem.GeplEnde", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Versicherung" HeaderText="col_Fahrer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versicherung" runat="server" CommandName="Sort" CommandArgument="Versicherung">col_Versicherung</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVersicherung" Text='<%# DataBinder.Eval(Container, "DataItem.Versicherung") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Status" HeaderText="col_Status">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="Status">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblStatus" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>       
                                                <asp:TemplateField SortExpression="Versand" HeaderText="col_Versand">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versand" runat="server" CommandName="Sort" CommandArgument="Versand">col_Versand</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVersand" Text='<%# DataBinder.Eval(Container, "DataItem.Versand", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>                                                                            
                                                <asp:TemplateField SortExpression="Rueckgabe_LN" HeaderText="col_Rueckgabe_LN">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Rueckgabe_LN" runat="server" CommandName="Sort" CommandArgument="Rueckgabe_LN">col_Rueckgabe_LN</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRueckgabe_LN" Text='<%# DataBinder.Eval(Container, "DataItem.Rueckgabe_LN", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Versand_VG" HeaderText="col_Versand_VG">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versand_VG" runat="server" CommandName="Sort" CommandArgument="Versand_VG">col_Versand_VG</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVersand_VG" Text='<%# DataBinder.Eval(Container, "DataItem.Versand_VG", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>   
                                                <asp:TemplateField SortExpression="Rueckgabe_VG" HeaderText="col_Rueckgabe_VG">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Rueckgabe_VG" runat="server" CommandName="Sort" CommandArgument="col_Rueckgabe_VG">col_Rueckgabe_VG</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRueckgabe_VG" Text='<%# DataBinder.Eval(Container, "DataItem.Rueckgabe_VG", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField> 
                                                <asp:TemplateField SortExpression="Klaerfall" HeaderText="col_Klaerfall">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Klaerfall" runat="server" CommandName="Sort" CommandArgument="Klaerfall">col_Klaerfall</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblKlaerfall" Text='<%# DataBinder.Eval(Container, "DataItem.Klaerfall") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>     
                                                <asp:TemplateField SortExpression="Info" HeaderText="col_Info">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Info" runat="server" CommandName="Sort" CommandArgument="Info">col_Info</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblInfo" Text='<%# DataBinder.Eval(Container, "DataItem.Info") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>                                                                                                                                                                                                                                                                                                                
                                            </Columns>
                                            </asp:GridView>
                              </div>
                         </div>
                        <div id="dataFooter" runat="server">
                            &nbsp;
                        </div> 
                       <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                        <ajaxtoolkit:modalpopupextender ID="ModalPopupExtender" runat="server" TargetControlID="btnFake"
                            PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="false" 
                            CancelControlID="cmdCancel">
                        </ajaxtoolkit:modalpopupextender>
                        <asp:Panel ID="mb" runat="server" Width="470px" Height="300px"  BackColor="White"
                            Style="display: none; border: solid 2px #ff9138; color: #595959;font-weight:bold">
                            <div style="padding-left: 5px; padding-top: 20px; margin-bottom: 10px;">
                                <asp:Label ID="lblMessagePopUp" runat="server" Font-Bold="True" CssClass="TextError"></asp:Label>
                            </div>
                            <table width="100%">
                                <tr  >
                                    <td style="padding-bottom: 5px; padding-left: 15px">
                                        LV-Nr:
                                    </td>
                                    <td width="100%">
                                        <strong>
                                            <asp:Label ID="lblLVNr" runat="server"></asp:Label></strong>
                                            <asp:HiddenField ID="hfEquinr" runat="server" />
                                    </td>
                                </tr>
                                <tr >
                                    <td  style="padding-bottom: 5px; padding-left: 15px" nowrap="nowrap">
                                        LV beendet zum
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDatum" MaxLength="10"  runat="server" ></asp:TextBox>
                                        <ajaxToolkit:TextBoxWatermarkExtender ID="extWatermarkEmail" runat="server" TargetControlID="txtDatum" 
                                        WatermarkText="dd.mm.yyyyy" WatermarkCssClass="Watermarked"></ajaxToolkit:TextBoxWatermarkExtender>
                                        </td>                          

</tr>
                                <tr>
                                    <td  style="padding-bottom: 5px; padding-left: 15px" nowrap="nowrap">
                                        SB ist in Ordnung
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxSB" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 5px; padding-left: 15px"  nowrap="nowrap">
                                        Höhe der Entschädigung im<br />
                                        Schadensfall ist in Ordnung
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxEnt" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 5px; padding-left: 15px; height: 27px;"  
                                        nowrap="nowrap">
                                        Versichererwechsel
                                    </td>
                                    <td style="height: 27px">
                                        <asp:CheckBox ID="cbxVers" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 5px; padding-left: 15px"  nowrap="nowrap">
                                        Fahrzeugwechsel
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxFahrz" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 5px; padding-left: 15px" >
                                        Sonstiges
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="txtBemerkung" runat="server" Width="256px" MaxLength="256"></asp:TextBox>
                                        <asp:Image ID="Image2" runat="server" ImageUrl="/Portal/Images/info.GIF" ToolTip="Maximal 256 Zeichen">
                                        </asp:Image>
                                    </td>
                                </tr><tr>
                                    <td nowrap="nowrap">
                                        &nbsp;
                                    </td>
                                    <td nowrap="nowrap">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2" style="width: 100%; padding-right: 20px;">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                            TabIndex="30" onclick="cmdSave_Click">» Absenden</asp:LinkButton><asp:LinkButton ID="cmdCancel" runat="server" CssClass="Tablebutton" Width="78px"
                                            Height="16px" TabIndex="30">» Abbrechen</asp:LinkButton></td></tr></table></asp:Panel>
                         
                         
                         </div></div></div></div></div></asp:content>