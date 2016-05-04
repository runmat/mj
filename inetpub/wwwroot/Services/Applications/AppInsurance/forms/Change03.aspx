<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03.aspx.vb" Inherits="AppInsurance.Change03"
    MasterPageFile="../MasterPage/App.Master" enableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div id="site">
     
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="paginationQuery" style="width: 100%; display: block;">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                Abfrage starten
                                            </td>
                                            <td class="active" valign="top" align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap" colspan="2" >
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="2" width="100%">
                                            <span>
                                                <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                                                    <asp:ListItem Selected="True" Text="Letzte Bestellung < 14 Tage" Value="1"> </asp:ListItem>
                                                    <asp:ListItem Text="Bestellung > 50 Stück" Value="2"> </asp:ListItem>
                                                    <asp:ListItem Text="Nach 2 Tagen nicht bestätigt" Value="3"> </asp:ListItem>
                                                </asp:RadioButtonList>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="2">
                                             &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td style="background-color: #dfdfdf;" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="btnConfirm" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                    Width="130px">» Abfrage starten</asp:LinkButton>
                            </div>
                            <div id="DivPlaceholder" runat="server" style="height: 550px;">
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div id="DivExcel" class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" /><span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel" runat="server" ForeColor="White">Excel herunterladen</asp:LinkButton></span>
                                    </div>
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
                                        <EditRowStyle></EditRowStyle>
                                        <Columns>
                                            <asp:TemplateField>
                                             <HeaderStyle Width="15px"/>
                                                <ItemTemplate>
                                                  
                                                        <asp:ImageButton ID="lbSave" runat="server" ToolTip="Freigeben/Stornieren" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                            CommandName="Update" ImageUrl="../../../Images/Edit_01.gif" Height="16px" Width="16px"
                                                            Visible='<%# DataBinder.Eval(Container.DataItem, "STATUS") = "Gesperrt" %>' />
                                                  
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AGENTUR" HeaderText="Agenturnummer" SortExpression="AGENTUR" />
                                            <asp:BoundField DataField="VERS_JAHR" HeaderText="Verkehrsjahr" SortExpression="VERS_JAHR" />
                                            <asp:TemplateField HeaderText="ANRED" SortExpression="ANRED" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblANRED" runat="server" Text='<%# Bind("ANRED_MEDI") %>'></asp:Label>
                                                    <asp:TextBox ID="txtANRED" runat="server" Text='<%# Bind("ANRED_MEDI") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SPERRE_WEB_ID" SortExpression="SPERRE_WEB_ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSPERRE_WEB_ID" runat="server" Text='<%# Bind("SPERRE_WEB_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="NAME1" HeaderText="Name1" SortExpression="NAME1" />
                                            <asp:BoundField DataField="NAME2" HeaderText="Name2" SortExpression="NAME2" />
                                            <asp:TemplateField HeaderText="STREET" SortExpression="STREET" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStreet" runat="server" Text='<%# Bind("STREET") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="HOUSE_NUM1" SortExpression="HOUSE_NUM1" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHOUSE_NUM1" runat="server" Text='<%# Bind("HOUSE_NUM1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="POST_CODE1" SortExpression="POST_CODE1" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPOST_CODE1" runat="server" Text='<%# Bind("POST_CODE1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CITY1" SortExpression="CITY1" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCITY1" runat="server" Text='<%# Bind("CITY1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ANZAHL" SortExpression="ANZAHL" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblANZAHL" runat="server" Text='<%# Bind("ANZAHL") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ANZ_APVTG" SortExpression="ANZ_APVTG" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblANZ_APVTG" runat="server" Text='<%# Bind("ANZ_APVTG") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SMTP_ADDR" SortExpression="SMTP_ADDR" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSMTP_ADDR" runat="server" Text='<%# Bind("SMTP_ADDR") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TELF1" SortExpression="TELF1" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTELF1" runat="server" Text='<%# Bind("TELF1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WE_ANRED" SortExpression="WE_ANRED" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWE_ANRED" runat="server" Text='<%# Bind("WE_ANRED_MEDI") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WE_NAME1" SortExpression="WE_NAME1" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWE_NAME1" runat="server" Text='<%# Bind("WE_NAME1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WE_NAME2" SortExpression="WE_NAME2" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWE_NAME2" runat="server" Text='<%# Bind("WE_NAME2") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WE_STREET" SortExpression="WE_STREET" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWE_STREET" runat="server" Text='<%# Bind("WE_STREET") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WE_HOUSE_NUM1" SortExpression="WE_HOUSE_NUM1" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWE_HOUSE_NUM1" runat="server" Text='<%# Bind("WE_HOUSE_NUM1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WE_POST_CODE1" SortExpression="WE_POST_CODE1" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWE_POST_CODE1" runat="server" Text='<%# Bind("WE_POST_CODE1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WE_CITY1" SortExpression="WE_CITY1" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWE_CITY1" runat="server" Text='<%# Bind("WE_CITY1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WE_TELF1" SortExpression="WE_TELF1" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWE_TELF1" runat="server" Text='<%# Bind("WE_TELF1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ERDAT" SortExpression="ERDAT" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblERDAT" runat="server" Text='<%# Bind("ERDAT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ERZET" SortExpression="ERZET" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblERZET" runat="server" Text='<%# Bind("ERZET") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DAT_LETZT_BEST" SortExpression="DAT_LETZT_BEST" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDAT_LETZT_BEST" runat="server" Text='<%# Bind("DAT_LETZT_BEST") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UZEIT_LETZT_BEST" SortExpression="UZEIT_LETZT_BEST" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUZEIT_LETZT_BEST" runat="server" Text='<%# Bind("UZEIT_LETZT_BEST") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>    
                                            <asp:TemplateField HeaderText="STATUS_TEXT" SortExpression="STATUS_TEXT" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSTATUS_TEXT" runat="server" Text='<%# Bind("STATUS_TEXT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                              
                                            <asp:BoundField DataField="STATUS" HeaderText="Status" SortExpression="STATUS" />                                            
                                         
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div>
                                    <asp:Panel ID="mb" runat="server" Width="525px" Height="350px" 
                                        style=" position: absolute; left: 25%; top: 25%" BackColor="White" Visible="false">
                                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px;">
                                            <asp:Label ID="lblAdressMessage" runat="server" Text="Detailinformationen" Font-Bold="True"></asp:Label>
                                        </div>
                                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 5px; padding-bottom: 5px">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblPoUpErr" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 479px">
                                                        Beauftragt am:
                                                        <asp:Label ID="lblBeauftragtAm" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        Menge:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAnzahlKennzeichen" style="width:auto" runat="server"  Font-Names="Verdana,sans-serif">
                                                            <asp:ListItem Value="1">1</asp:ListItem>
                                                            <asp:ListItem Value="2">2</asp:ListItem>
                                                            <asp:ListItem Value="3">3</asp:ListItem>
                                                            <asp:ListItem Value="4">4</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 479px">
                                                        letzte Bestellung:
                                                        <asp:Label ID="lblLastOrder" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="190px">
                                                         letzter Auftrag:
                                                    </td>
                                                    <td width="190px">
                                                        <asp:Label ID="lblLastOrder2" runat="server"></asp:Label>
                                                    </td>                                                    
                                                </tr> 
                                                <tr>
                                                    <td style="width: 479px">
                                                        Auftragsnr.:
                                                        <asp:Label ID="lblAuftragsnummer" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="190px">
                                                         aap-Vordrucke:
                                                    </td>
                                                    <td width="190px">
                                                         
                                                         <asp:TextBox ID="txtAnzalAAP" runat="server" MaxLength="3" Width="50px" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 479px">
                                                        Agenturnr.:
                                                        <asp:Label ID="lblAgentur" runat="server"></asp:Label>
                                                        <asp:Label ID="lblIndex" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 479px; padding-top: 5px;">
                                                        <b>Adresse: </b>
                                                    </td>
                                                    <td colspan="2">
                                                        <b>Abweichende Adresse: </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 479px">
                                                        <asp:Label ID="lblAdresse" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblAdresseWe" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 479px">
                                                        E-Mail:
                                                        <asp:TextBox ID="txtEmailAdresse" runat="server"  MaxLength="241"
                                                            Width="270px"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnOK" runat="server" Text="Freigeben" CssClass="TablebuttonLarge"
                                                        Font-Bold="True" Width="90px" OnClick="btnOK_Click"/>
                                                </td>
                                                <td align="right" style="width: 95px">
                                                    <asp:Button ID="btnStorno" runat="server" Text="Stornieren" CssClass="TablebuttonLarge"
                                                        Font-Bold="True" Width="90px" />
                                                </td>
                                                <td align="right" style="width: 100px; padding-left: 5px; padding-right: 5px">
                                                    <asp:Button ID="btnCancel" runat="server" Text="Abbrechen" CssClass="TablebuttonLarge"
                                                        Font-Bold="true" Width="90px"  />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
        </div>
     
    </div>

</asp:Content>
