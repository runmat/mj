<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02s.aspx.vb" Inherits="CKG.Components.ComCommon.Change02s1"
    MasterPageFile="../../../MasterPage/Services.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .rbList
        {
            width: 100%;
        }

    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu"> 
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" CausesValidation="false" />               
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Erfassung Abmeldeunterlagen"></asp:Label>
                        </h1>
                    </div>
                    
                            <div id="paginationQuery">
                                <table id="Table1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width: 100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="true"></asp:Label>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnEmpty">
                                <div id="TableQuery">
                                
                                <table cellpadding="0" cellspacing="0">
                                    <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <span>
                                                        <asp:RadioButton ID="rdbEinzel" runat="server" Checked="true" GroupName="rdbGroup"
                                                            Text="Einzelauswahl" AutoPostBack="True" TextAlign="Left" />
                                                        <asp:RadioButton ID="rdbUpload" runat="server" GroupName="rdbGroup" Text="Upload"
                                                            AutoPostBack="True" TextAlign="Left" />
                                                    </span>
                                                </td>
                                            </tr>
                                </table>
                                
                                    <table id="tbl_Query" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            
                                        
                                            <tr id="tr_Kunde" runat="server" class="formquery">
                                                <td class="firstLeft active">
                                                    Kunde:
                                                </td>
                                                <td class="active">
                                                    <asp:DropDownList ID="ddlKunde" runat="server" Width="300px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                           
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblCarport" runat="server" Text="CarportID:"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblCarportID" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="white-space:nowrap;">
                                                    <asp:Label ID="lblName2" runat="server" Text="Name:"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblName" runat="server"></asp:Label>
                                                </td>

                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="white-space:nowrap;">
                                                    <asp:Label ID="lblDateDemontage" runat="server" Text="Demontagedatum:"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtDateDemontage" runat="server"></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="txtDateDemontage_CalendarExtender" runat="server"
                                                        TargetControlID="txtDateDemontage">
                                                    </AjaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="CVDemontagedatum" Display="Dynamic" runat="server" ControlToValidate="txtDateDemontage"
                                                        ErrorMessage="Bitte geben Sie ein gültiges Datum ein!" Operator="DataTypeCheck"
                                                        Type="Date"></asp:CompareValidator>
                                                    <asp:RequiredFieldValidator ID="RFVDemontagedatum" Display="Dynamic" runat="server"
                                                        ControlToValidate="txtDateDemontage" ErrorMessage="Bitte geben Sie ein gültiges Datum ein!"></asp:RequiredFieldValidator>
                                                </td>

                                            </tr>                                            
                                           <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="Label2" runat="server" Text="Vorlage ZB I:"></asp:Label>
                                                </td>
                                                <td  class="active" >
                                                <asp:RadioButtonList ID="rblZBI" runat="server" RepeatDirection="Horizontal" 
                                                        RepeatLayout="Flow">
                                                <asp:ListItem Value="0" style="padding-right:5px">Nein</asp:ListItem>
                                                <asp:ListItem Value="1" style="padding-right:5px">Ja</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="2">mit Kopie</asp:ListItem>
                                            </asp:RadioButtonList>
                                           
                                                </td>                                           
                                           </tr>
                                          <tr class="formquery" id="trKennzeichen" runat="server">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="lblKennzeichen1" runat="server" Text="Kennzeichen*:"></asp:Label>
                                                </td>
                                                <td class="active" >                                                
                                                    <asp:TextBox ID="txtKennz" runat="server" MaxLength="15"></asp:TextBox>                                                
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" 
                                                        ImageUrl="../../../images/empty.gif" />
                                                </td>
                                          </tr>
                                          <tr class="formquery">
                                                <td class="firstLeft active" style="white-space:nowrap;">
                                                    Anzahl Kennzeichen*:
                                                </td>
                                                <td class="active" style="width:100%">
                                                    <asp:TextBox ID="txtAnzahlKennz" runat="server" MaxLength="1"></asp:TextBox>
                                                </td>                                          
                                          </tr>

                                            <tr class="formquery" >
                                                <td class="firstLeft active" colspan="2" style="width:100%">
                                                   * Pflichtfelder
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2" style="width:100%">
                                                    &nbsp;</td>
                                            </tr>
                                        </tbody>
                                        
                                    </table>
                                    
                                    <table id="tblUpload" runat="server" cellpadding="0" cellspacing="0" visible="false"
                                            style="padding-bottom: 10px">
                                            <tbody>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Upload:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <input id="upFile" type="file" size="49" name="File1" runat="server" />
                                                        <a href="javascript:openinfo('Info02.htm');">
                                                            <img src="/Services/Images/info.gif" border="0" height="16px" width="16px" 
                                                                title="Struktur Uploaddatei" /></a>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    
                                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px; clear:both;">
                                                &nbsp;
                                        </div>                                                                        
                                </div> </asp:Panel>
                            <div id="dataQueryFooter" style="clear:both;">
                                <asp:LinkButton ID="lb_Weiter" runat="server" CssClass="Tablebutton" Width="78px">» Erfassen </asp:LinkButton>
                                <asp:LinkButton ID="lb_Upload" runat="server" CssClass="Tablebutton" Width="78px" Visible="false">» Upload </asp:LinkButton>
                            </div>       
                           

                            <div id="Result" runat="Server" visible="false">
                                <div id="DivPrint"  runat="server" visible="false"  class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                        <img id="imgPDF" runat="server" src="../../../Images/iconPDF.gif" alt="PDF herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreatePDF1" ForeColor="White" runat="server">PDF herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">

                                                <asp:GridView ID="GridView1"  Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="True" AllowPaging="True" CssClass="GridView" PageSize="20" DataKeyNames="LICENSE_NUM" >
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <HeaderStyle  Width="40px"/>
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgOk" runat="server" ImageUrl="/Services/Images/erfolg.gif" Visible='<%# DataBinder.Eval(Container, "DataItem.FLAG_FOUND")= "X" %>' />
                                                                <asp:Image ID="imgError" runat="server" ImageUrl="/Services/Images/fehler.gif" 
                                                                    Visible='<%# DataBinder.Eval(Container, "DataItem.FLAG_FOUND")<> "X" %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Kennzeichen">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtGridKennzeichen" runat="server" Width="80px" Enabled="false" style="font-size:11px;font-weight:bold" Text='<%# Bind("LICENSE_NUM") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fahrgestellnummer" HeaderStyle-Width="150">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtGridFahrgestellnummer" runat="server" Enabled="False" Width="145px" style="font-size:11px;font-weight:bold"
                                                                    Text='<%# Bind("CHASSIS_NUM") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            
                                                            
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Vertragsnummer" HeaderStyle-Width="120">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtGridVertragsnummer" runat="server" Enabled="False" Width="110px" style="font-size:11px;font-weight:bold"
                                                                    Text='<%# Bind("LIZNR") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                           
                                                        </asp:TemplateField>
                                                        <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="ZZFABRIKNAME" 
                                                            HeaderText="Hersteller" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd.MM.yy}"
                                                            DataField="DAT_DEMONT" HeaderText="Demontage- datum" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="VORLAGE_ZB1_CPL_Text"
                                                            HeaderText="Vorlage ZBI" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="ANZ_KENNZ_CPL" 
                                                            HeaderText="Anzahl Kennzeichen" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderStyle-Width="40" >
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbDelete" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'
                                                                    CommandName="Delete" Height="10px">
																<img alt="entfernen" src="../../../Images/Papierkorb_01.gif" border="0" height="16" width="16"/>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ID" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FLAG_FOUND" Visible="False">
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("FLAG_FOUND") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFlag" runat="server" Text='<%# Bind("FLAG_FOUND") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                   
                                </div>
                                <div id="dataFooter" runat="server">
                                    &nbsp;
                                </div>                                
                            </div>
                            <div id="ButtonsResultTable" runat="server" style="float:right;margin-top:4px;">                               
                                    <asp:LinkButton ID="cmdGetData" runat="server" CssClass="TablebuttonMiddle" Visible="False"
                                        Width="100px" Height="22px" style="text-align:center;">Daten ergänzen</asp:LinkButton>
                                    <asp:LinkButton ID="cmdEdit" runat="server" CssClass="TablebuttonMiddle" Visible="False"
                                        Width="100px" Height="22px" style="text-align:center;">Bearbeiten</asp:LinkButton>
                                    <asp:LinkButton ID="cmdSend" runat="server" CssClass="TablebuttonMiddle" Visible="False"
                                        Width="100px" Height="22px" style="text-align:center;">Weiter</asp:LinkButton>
                                    <asp:LinkButton ID="cmdNewList" runat="server" CssClass="TablebuttonMiddle" Visible="False"
                                        Width="100px" Height="22px" style="text-align:center;">Neue Liste</asp:LinkButton>                               
                            </div>
                            
                            
                            <div>
                                <script type="text/javascript">
                                    function openinfo(url) {
                                        fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=350");
                                        fenster.focus();
                                    }
                                 </script>
                            </div>
                           
                      
                </div>
            </div>
        </div>
    </div>
</asp:Content>
