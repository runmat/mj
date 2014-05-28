<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report30s.aspx.vb" Inherits="CKG.Components.ComCommon.Report30s1"
    MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" CausesValidation="false"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
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
                                        <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="ibtNewSearch" runat="server" ImageUrl="../../Images/queryArrow.gif" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:Panel ID="divTrenn" runat="server" Visible="false">
                            <div id="PlaceHolderDiv">
                            </div>
                        </asp:Panel>
                    </div>
                    <div id="TableQuery">
                        <asp:Panel ID="divSelection" runat="server" DefaultButton="lbCreate">
                            <table cellpadding="0" cellspacing="0">
                                <tfoot>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="2">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width: 120px">
                                            Datum von:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAbDatum" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="txtAbDatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtAbDatum">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="meetxtAbdatum" runat="server" TargetControlID="txtAbDatum"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <asp:CompareValidator ID="cv_DatumVon" runat="server" ErrorMessage="Falsches Datumsformat"
                                                Type="Date" ControlToValidate="txtAbDatum" ControlToCompare="TextBox1" Operator="DataTypeCheck"
                                                CssClass="TextError" ForeColor=""></asp:CompareValidator>                                            
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Datum bis:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBisDatum" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="txtBisDatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtBisDatum">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="meetxtBisDatum" runat="server" TargetControlID="txtBisDatum"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <asp:CompareValidator ID="cv_DatumBis" runat="server" ErrorMessage="Falsches Datumsformat"
                                                Type="Date" ControlToValidate="txtBisDatum" ControlToCompare="TextBox1" Operator="DataTypeCheck"
                                                CssClass="TextError" ForeColor=""></asp:CompareValidator>                                           
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Kennzeichen:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtKennzeichen" runat="server"></asp:TextBox>                                            
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Fahrgestellnummer:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChassisNum" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Name1:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName1" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Name2:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName2" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Straße:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStrasse" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            PLZ:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPLZ" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Ort:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOrt" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>                                    
                                    <tr class="formquery">
                                        <td colspan="2">
                                            <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                            <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                Width="1px" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
                        </asp:Panel>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                            <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btndefault" runat="server"
                                Text="Button" />
                        </div>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div>
                             <asp:Label ID="lblErrorResult" CssClass="TextError" runat="server" EnableViewState="false"></asp:Label>
                        </div>
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton></span></div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">                            
                            <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True">
                                <PagerSettings Visible="False" />
                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                <RowStyle CssClass="ItemStyle" />
                                <Columns>
                                    <asp:TemplateField  HeaderText="Statusabfrage"><%--HeaderStyle-Width="100px"--%>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnEdit" runat="server" SortExpression="ibtnEdit" CommandName="Info"
                                                ImageUrl="/services/images/shipping.gif" ToolTip="Versandstatus anzeigen." CommandArgument='<%# DataBinder.Eval(Container, "DataItem.POOLNR") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZLSDAT" HeaderText="col_Datum">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Datum" runat="server" CommandName="Sort" CommandArgument="ZZLSDAT">col_Datum</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDatum" Text='<%# DataBinder.Eval(Container, "DataItem.ZZLSDAT","{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZDIEN1" HeaderText="col_Versandart">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Versandart" runat="server" CommandName="Sort" CommandArgument="ZZDIEN1">col_Versandart</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblVersandart" Text='<%# DataBinder.Eval(Container, "DataItem.ZZDIEN1") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="FRACHTFUEHRER" HeaderText="col_Frachtfuehrer">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Frachtfuehrer" runat="server" CommandName="Sort" CommandArgument="FRACHTFUEHRER">col_Frachtfuehrer</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFrachtfuehrer" Text='<%# DataBinder.Eval(Container, "DataItem.FRACHTFUEHRER") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Adresse" HeaderText="col_Adresse">
                                    <ItemStyle Height="42px" Wrap="true" />
                                       <HeaderTemplate>
                                            <asp:LinkButton ID="col_Adresse" runat="server" CommandName="Sort" CommandArgument="Adresse">col_Adresse</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lbAdresse" Text='<%# DataBinder.Eval(Container, "DataItem.Adresse") %>' >
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="dataFooter">
                            &nbsp;
                        </div>
                        <div>
                            <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                            <ajaxToolkit:modalpopupextender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                                PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel"
                                X="450" Y="50">
                            </ajaxToolkit:modalpopupextender>
                            <asp:Panel ID="mb" runat="server" Width="600px" BackColor="White" style="display:none">
                                <div style="padding:10px 10px 10px 10px; margin-bottom: 10px; text-align: center; background-color:#DCDCDC; height:25px;">
                                    <asp:Label ID="lblVersandstatus" runat="server" Text="Versandstatus"
                                        Font-Bold="True" Font-Size="12px" ></asp:Label>
                                </div>                       
                                <div style="margin-left: 10px;">
                                    <table cellpadding="5" cellspacing="0" style="width: 98%;" border="1">
                                        <tr valign="middle">
                                            <td style="font-weight: bold; padding-left: 20px;" valign="middle">
                                                <b>Status </b>&nbsp;
                                            </td>
                                            <td valign="middle">
                                                <b>Datum </b>&nbsp;
                                            </td>
                                            <td valign="middle">
                                                <b>Uhrzeit </b>&nbsp;
                                            </td>
                                            <td valign="middle">
                                                <b>Meldung </b>&nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trStatus0" runat="server">
                                            <td>
                                                <asp:Image ID="Image1" runat="server" ImageUrl="/services/images/vlog/1.png" Width="80px"
                                                    Height="80px" />&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDat0" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTime0" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMeld0" runat="server" Text="Ihr Auftrag wurde bearbeitet."></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trStatus1" runat="server">
                                            <td>
                                                <asp:Image ID="Image6" runat="server" Height="80px" ImageUrl="/services/images/vlog/6.png"
                                                    Width="80px" Visible="False" />
                                                <asp:Image ID="Image2" runat="server" Height="80px" ImageUrl="/services/images/vlog/2.png"
                                                    Width="80px" />&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDat1" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTime1" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMeld1" runat="server"></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trStatus2" runat="server">
                                            <td>
                                                <asp:Image ID="Image7" runat="server" Height="80px" ImageUrl="/services/images/vlog/7.png"
                                                    Visible="False" Width="80px" />
                                                <asp:Image ID="Image3" runat="server" Height="80px" ImageUrl="/services/images/vlog/3.png"
                                                    Width="80px" />&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDat2" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTime2" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMeld2" runat="server"></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trStatus3" runat="server">
                                            <td>
                                                <asp:Image ID="Image8" runat="server" Height="80px" ImageUrl="/services/images/vlog/8.png"
                                                    Visible="False" Width="80px" />
                                                <asp:Image ID="Image4" runat="server" Height="80px" ImageUrl="/services/images/vlog/4.png"
                                                    Width="80px" />&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDat3" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTime3" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMeld3" runat="server"></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trStatus4" runat="server">
                                            <td>
                                                <asp:Image ID="Image9" runat="server" Height="80px" ImageUrl="/services/images/vlog/9.png"
                                                    Visible="False" Width="80px" />
                                                <asp:Image ID="Image5" runat="server" Height="80px" ImageUrl="/services/images/vlog/5.png"
                                                    Width="80px" />&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDat4" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTime4" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMeld4" runat="server"></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trFehler" runat="server" visible="false">
                                            <td>
                                                <asp:Image ID="ImageFehler" runat="server" Height="80px" ImageUrl="/services/images/vlog/Fehler.png"
                                                    Width="80px" />&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDat5" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTime5" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMeld5" runat="server"></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="text-align: center">
                                                <asp:Button ID="btnCancel" runat="server" Text="OK" CssClass="TablebuttonLarge" Font-Bold="true"
                                                    Width="90px" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="height: 10px">
                                    &nbsp;
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
