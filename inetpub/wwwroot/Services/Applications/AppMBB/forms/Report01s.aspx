<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report01s.aspx.cs" Inherits="AppMBB.forms.Report01s" MasterPageFile="../Master/AppMaster.Master" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" onclick="lbBack_Click"></asp:LinkButton>
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
                            <asp:PostBackTrigger ControlID="lnkCreateExcel" />
                       </Triggers>
                        <ContentTemplate>

                    <div id="divAbfrage" runat="server" visible="true">
                     <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        Neue Abfrage starten
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="NewSearch" runat="server" 
                                                ImageUrl="/Services/Images/queryArrow.gif" onclick="NewSearch_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    
                    </div>
        
                    <div id="divError" runat="server" enableviewstate="false" style="padding: 0px 0px 10px 15px;">
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        <asp:Label ID="lblNoData" runat="server" ForeColor="Blue" EnableViewState="False"></asp:Label>
                    </div>

                    <asp:Panel ID="Panel1" runat="server">
                        <div id="TableQuery">
                            <table id="tblEinzel" runat="server" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Abmeldestatus" runat="server">lbl_Abmeldestatus</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left:4px;width:100%">
                                            <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" 
                                                RepeatLayout="Flow">
                                                <asp:ListItem Selected="True">Klärfall</asp:ListItem>
                                                <asp:ListItem>Standard</asp:ListItem>
                                                <asp:ListItem>offen</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left:4px;width:100%">
                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="17" Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left:4px">
                                            <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Vertragsnummer" runat="server">lbl_Vertragsnummer</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left:4px">
                                            <asp:TextBox ID="txtVertragsnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                        <td class="firstLeft active" style="padding-left:4px">
                                            &nbsp;</td>
                                    </tr>
                                    
                                </tbody>
                            </table>
                            
                            <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" 
                            Width="78px" onclick="lbCreate_Click">» Suchen </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="/Services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel" runat="server" 
                                    onclick="lnkCreateExcel_Click">Excel herunterladen</asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server">
                            </uc2:GridNavigation>
                           
                            
                        </div>
                        <div id="data">
                            <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="gvAusgabe"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                            OnSorting="gvAusgabe_Sorting" Width="100%" 
                                            onrowcommand="gvAusgabe_RowCommand">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                            <asp:TemplateField>
                                                  <ItemTemplate>
                                                        <asp:ImageButton ID="ibtEdit" CommandArgument='<%# Bind("CHASSIS_NUM") %>' CommandName="ShowStatus" runat="server" ImageUrl="/services/images/EditTableHS.png" />
                                                    </ItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="col_Vertragsnummer" SortExpression="VERTRAGSNR">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="VERTRAGSNR">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("VERTRAGSNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                 </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_Fahrgestellnummer" SortExpression="CHASSIS_NUM">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("CHASSIS_NUM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_Kennzeichen" SortExpression="LICENSE_NUM">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("LICENSE_NUM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="col_ZBII" SortExpression="ZB2_NR">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ZBII" runat="server" CommandName="Sort" CommandArgument="ZB2_NR">col_ZBII</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ZB2_NR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_ErfDatCarportliste" SortExpression="ERDAT_CARP">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ErfDatCarportliste" runat="server" CommandName="Sort" CommandArgument="ERDAT_CARP">col_ErfDatCarportliste</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblErfDat" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ERDAT_CARP","{0:dd.MM.yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_Belegnr" SortExpression="BELNR">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Belegnr" runat="server" CommandName="Sort" CommandArgument="BELNR">col_Belegnr</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                         <asp:Label ID="Label55" runat="server" Text='<%# Bind("BELNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                
                                                <asp:TemplateField HeaderText="col_ZBIIStatus" SortExpression="ZB2_STATUS">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ZBIIStatus" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_ZBIIStatus</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("ZB2_STATUS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                 </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_ZBIStatus" SortExpression="ZB1_STATUS">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ZBIStatus" runat="server" CommandName="Sort" CommandArgument="ZB1_STATUS">col_ZBIStatus</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("ZB1_STATUS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_KennzVornStatus" SortExpression="KV_STATUS">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_KennzVornStatus" runat="server" CommandName="Sort" CommandArgument="KV_STATUS">col_KennzVornStatus</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label8" runat="server" Text='<%# Bind("KV_STATUS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_KennzHintenStatus" SortExpression="KH_STATUS">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_KennzHintenStatus" runat="server" CommandName="Sort" CommandArgument="KH_STATUS">col_KennzHintenStatus</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label9" runat="server" Text='<%# Bind("KH_STATUS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_FormStatus" SortExpression="FORM_STATUS">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_FormStatus" runat="server" CommandName="Sort" CommandArgument="FORM_STATUS">col_FormStatus</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("FORM_STATUS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_AbmeldeStatus" SortExpression="AB_STATUS">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_AbmeldeStatus" runat="server" CommandName="Sort" CommandArgument="AB_STATUS">col_AbmeldeStatus</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label56" runat="server" Text='<%# Bind("AB_STATUS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_ZulassungsStatus" SortExpression="ZLS_STATUS">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ZulassungsStatus" runat="server" CommandName="Sort" CommandArgument="ZLS_STATUS">col_ZulassungsStatus</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label57" runat="server" Text='<%# Bind("ZLS_STATUS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_VersandStatus" SortExpression="VERS_SATATUS">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_VersandStatus" runat="server" CommandName="Sort" CommandArgument="VERS_SATATUS">col_VersandStatus</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label58" runat="server" Text='<%# Bind("VERS_SATATUS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                       
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div id="divStatus" runat="server" style="color: #595959; border: 1px solid #dfdfdf;"
                        visible="false">
                          <table id="tblStatusanzeige" runat="server" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="background-color: #dfdfdf;height: 22px;vertical-align:middle">
                                    <asp:Label ID="lblStatusanzeige" runat="server" Font-Bold="true" Style="padding-left: 15px;padding-top:5px">Statusanzeige</asp:Label>
                                </td>
                            </tr>
                        </table>

                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr class="formquery">
                                <td style="width: 8%">
                                    &nbsp;
                                </td>
                                <td class="firstLeft active" style="width: 150px">
                                    <asp:Label ID="lblFahrgestellummerStatus" runat="server">Fahrgestellnummer:</asp:Label>
                                </td>
                                <td style="padding-left: 4px">
                                    <asp:Label ID="lblFinShow" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td>
                                </td>
                                <td class="firstLeft active">
                                    <asp:Label ID="lblKennzeichenStatus" runat="server">Kennzeichen:</asp:Label>
                                </td>
                                <td style="padding-left: 4px;">
                                    <asp:Label ID="lblKennzeichenShow" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td>
                                    &nbsp;</td>
                                <td class="firstLeft active" colspan="2">
                        <asp:Label ID="lblStatusError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        <asp:Label ID="lblStatusSuccess" runat="server" ForeColor="Blue" EnableViewState="False" 
                                        Font-Size="14pt"></asp:Label>
                                    </td>
                                
                            </tr>
                            <tr id="trZBI" runat="server" class="formquery">
                                <td style="padding-left: 5px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:ImageButton ID="ibtNewZBI" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                        ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnClick="ibtNewZBI_Click" />
                                    <asp:ImageButton ID="ibtHistZBI" runat="server" ImageUrl="/services/images/History_Bullet.gif"
                                        ToolTip="Historie anzeigen" Style="padding-right: 5px" OnClick="ibtHistZBI_Click" />
                                    <asp:Label ID="lblKennZBI" runat="server" Visible="False"></asp:Label>&nbsp;
                                </td>
                                <td class="firstLeft active" style="vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblZBI" runat="server">Status ZBI:</asp:Label>
                                </td>
                                <td style="padding-left: 4px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblZBIShow" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlZBI" runat="server" Width="200px" Visible="False" Style="vertical-align: top">
                                    </asp:DropDownList>
                                    &nbsp;<asp:TextBox ID="txtZBI" runat="server" Height="60px" TextMode="MultiLine"
                                        Width="300px" Visible="False"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="ibtSaveZBI" runat="server" ImageUrl="/portal/images/savehs.png"
                                        ToolTip="Speichern" Visible="False" Style="padding-right: 5px" 
                                        onclick="ibtSaveZBI_Click" />
                                    &nbsp;<asp:ImageButton ID="ibtCancelZBI" runat="server" ImageUrl="/services/images/del.png"
                                        ToolTip="Abbrechen" Visible="False" Style="padding-right: 5px" 
                                        onclick="CancelStatus" />
                                </td>
                            </tr>
                            <tr id="trZBII" runat="server" class="formquery">
                                <td style="padding-left: 5px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:ImageButton ID="ibtNewZBII" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                        ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnClick="ibtNewZBII_Click" />
                                    <asp:ImageButton ID="ibtHistZBII" runat="server" ImageUrl="/services/images/History_Bullet.gif"
                                        ToolTip="Historie anzeigen" Style="padding-right: 5px" OnClick="ibtHistZBII_Click" />
                                    <asp:Label ID="lblKennZBII" runat="server" Visible="False"></asp:Label>
                                </td>
                                <td class="firstLeft active" style="vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblZBII" runat="server">Status ZBII:</asp:Label>
                                </td>
                                <td style="padding-left: 4px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblZBIIShow" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlZBII" runat="server" Width="200px" Visible="False" Style="vertical-align: top">
                                    </asp:DropDownList>
                                    &nbsp;<asp:TextBox ID="txtZBII" runat="server" Height="60px" TextMode="MultiLine"
                                        Width="300px" Visible="False"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="ibtSaveZBII" runat="server" ImageUrl="/portal/images/savehs.png"
                                        ToolTip="Speichern" Visible="False" Style="padding-right: 5px" OnClick="ibtSaveZBII_Click" />
                                    &nbsp;<asp:ImageButton ID="ibtCancelZBII" runat="server" ImageUrl="/services/images/del.png"
                                        ToolTip="Abbrechen" Visible="False" Style="padding-right: 5px" OnClick="CancelStatus" />
                                </td>
                            </tr>
                            <tr id="trKNZV" runat="server" class="formquery">
                                <td style="padding-left: 5px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:ImageButton ID="ibtNewKNZV" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                        ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnClick="ibtNewKNZV_Click" />
                                    <asp:ImageButton ID="ibtHistKNZV" runat="server" ImageUrl="/services/images/History_Bullet.gif"
                                        ToolTip="Historie anzeigen" Style="padding-right: 5px" OnClick="ibtHistKNZV_Click" />
                                    <asp:Label ID="lblKennKNZV" runat="server" Visible="False"></asp:Label>
                                </td>
                                <td class="firstLeft active" style="vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblKNZV" runat="server">Status KNZV:</asp:Label>
                                </td>
                                <td style="padding-left: 4px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblKNZVShow" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlKNZV" runat="server" Width="200px" Visible="False" Style="vertical-align: top">
                                    </asp:DropDownList>
                                    &nbsp;<asp:TextBox ID="txtKNZV" runat="server" Height="60px" TextMode="MultiLine"
                                        Width="300px" Visible="False"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="ibtSaveKNZV" runat="server" ImageUrl="/portal/images/savehs.png"
                                        ToolTip="Speichern" Visible="False" Style="padding-right: 5px" OnClick="ibtSaveKNZV_Click" />
                                    &nbsp;<asp:ImageButton ID="ibtCancelKNZV" runat="server" ImageUrl="/services/images/del.png"
                                        ToolTip="Abbrechen" Visible="False" Style="padding-right: 5px" OnClick="CancelStatus" />
                                </td>
                            </tr>
                            <tr id="trKNZH" runat="server" class="formquery">
                                <td style="padding-left: 5px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:ImageButton ID="ibtNewKNZH" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                        ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnClick="ibtNewKNZH_Click" />
                                    <asp:ImageButton ID="ibtHistKNZH" runat="server" ImageUrl="/services/images/History_Bullet.gif"
                                        ToolTip="Historie anzeigen" Style="padding-right: 5px" OnClick="ibtHistKNZH_Click" />
                                    <asp:Label ID="lblKennKNZH" runat="server" Visible="False"></asp:Label>
                                </td>
                                <td class="firstLeft active" style="vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblKNZH" runat="server">Status KNZH:</asp:Label>
                                </td>
                                <td style="padding-left: 4px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblKNZHShow" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlKNZH" runat="server" Width="200px" Visible="False" Style="vertical-align: top">
                                    </asp:DropDownList>
                                    &nbsp;<asp:TextBox ID="txtKNZH" runat="server" Height="60px" TextMode="MultiLine"
                                        Width="300px" Visible="False"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="ibtSaveKNZH" runat="server" ImageUrl="/portal/images/savehs.png"
                                        ToolTip="Speichern" Visible="False" Style="padding-right: 5px" OnClick="ibtSaveKNZH_Click" />
                                    &nbsp;<asp:ImageButton ID="ibtCancelKNZH" runat="server" ImageUrl="/services/images/del.png"
                                        ToolTip="Abbrechen" Visible="False" Style="padding-right: 5px" OnClick="CancelStatus" />
                                </td>
                            </tr>
                            <tr id="trForm" runat="server" class="formquery">
                                <td style="padding-left: 5px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:ImageButton ID="ibtNewForm" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                        ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnClick="ibtNewForm_Click" />
                                    <asp:ImageButton ID="ibtHistForm" runat="server" ImageUrl="/services/images/History_Bullet.gif"
                                        ToolTip="Historie anzeigen" Style="padding-right: 5px" OnClick="ibtHistForm_Click" />
                                    <asp:Label ID="lblKennForm" runat="server" Visible="False"></asp:Label>
                                </td>
                                <td class="firstLeft active" style="vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblForm" runat="server">Status Form:</asp:Label>
                                </td>
                                <td style="padding-left: 4px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblFormShow" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlForm" runat="server" Width="200px" Visible="False" Style="vertical-align: top">
                                    </asp:DropDownList>
                                    &nbsp;<asp:TextBox ID="txtForm" runat="server" Height="60px" TextMode="MultiLine"
                                        Width="300px" Visible="False"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="ibtSaveForm" runat="server" ImageUrl="/portal/images/savehs.png"
                                        ToolTip="Speichern" Visible="False" Style="padding-right: 5px" OnClick="ibtSaveForm_Click" />
                                    &nbsp;<asp:ImageButton ID="ibtCancelForm" runat="server" ImageUrl="/services/images/del.png"
                                        ToolTip="Abbrechen" Visible="False" Style="padding-right: 5px" OnClick="CancelStatus" />
                                </td>
                            </tr>
                            <tr id="trAmeldeStatus" runat="server" class="formquery">
                                <td style="padding-left: 5px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:ImageButton ID="ibtNewAmeldeStatus" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                        ToolTip="Neuen Status setzen" Style="padding-right: 5px; padding-left: 5px" OnClick="ibtNewAmeldeStatus_Click" />
                                    <asp:ImageButton ID="ibtHistAmeldeStatus" runat="server" ImageUrl="/services/images/History_Bullet.gif"
                                        ToolTip="Historie anzeigen" Style="padding-right: 5px" OnClick="ibtHistAmeldeStatus_Click" />
                                    <asp:Label ID="lblKennAbmeldeStatus" runat="server" Visible="False"></asp:Label>
                                </td>
                                <td class="firstLeft active" style="vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblAmeldeStatus" runat="server">Ameldestatus:</asp:Label>
                                </td>
                                <td style="padding-left: 4px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblAmeldeStatusShow" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlAmeldeStatus" runat="server" Width="200px" Visible="False"
                                        Style="vertical-align: top">
                                    </asp:DropDownList>
                                    &nbsp;<asp:TextBox ID="txtAmeldeStatus" runat="server" Height="60px" TextMode="MultiLine"
                                        Width="300px" Visible="False"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="ibtSaveAmeldeStatus" runat="server" ImageUrl="/portal/images/savehs.png"
                                        ToolTip="Speichern" Visible="False" Style="padding-right: 5px" OnClick="ibtSaveAmeldeStatus_Click" />
                                    &nbsp;<asp:ImageButton ID="ibtCancelAmeldeStatus" runat="server" ImageUrl="/services/images/del.png"
                                        ToolTip="Abbrechen" Visible="False" Style="padding-right: 5px" OnClick="CancelStatus" />
                                </td>
                            </tr>
                            
                            <tr id="trZLS" runat="server" class="formquery">
                                <td style="padding-left: 5px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:ImageButton ID="ibtNewZlsStatus" runat="server" ImageUrl="/services/images/EditTableHS.png"
                                        ToolTip="Neuen Status setzen" 
                                        Style="padding-right: 5px; padding-left: 5px" onclick="ibtNewZlsStatus_Click" />
                                    <asp:ImageButton ID="ibtHistZlsStatus" runat="server" ImageUrl="/services/images/History_Bullet.gif"
                                        ToolTip="Historie anzeigen" Style="padding-right: 5px" 
                                        onclick="ibtHistZlsStatus_Click" />
                                    <asp:Label ID="lblKennZlsStatus" runat="server" Visible="False"></asp:Label>
                                </td>
                                <td class="firstLeft active" style="vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="Label11" runat="server">Zulassungsstatus:</asp:Label>
                                </td>
                                <td style="padding-left: 4px; vertical-align: top;border-top: 1px solid #dfdfdf">
                                    <asp:Label ID="lblZlsStatusShow" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlZlsStatus" runat="server" Width="200px" Visible="False"
                                        Style="vertical-align: top">
                                    </asp:DropDownList>
                                    &nbsp;<asp:TextBox ID="txtZlsStatus" runat="server" Height="60px" TextMode="MultiLine"
                                        Width="300px" Visible="False"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="ibtSaveZlsStatus" runat="server" ImageUrl="/portal/images/savehs.png"
                                        ToolTip="Speichern" Visible="False" Style="padding-right: 5px" 
                                        onclick="ibtSaveZlsStatus_Click" />
                                    &nbsp;<asp:ImageButton ID="ibtCancelZlsStatus" runat="server" ImageUrl="/services/images/del.png"
                                        ToolTip="Abbrechen" Visible="False" Style="padding-right: 5px" OnClick="CancelStatus" />
                                </td>
                            </tr>



                            <tr class="formquery">
                                <td>
                                    &nbsp;
                                </td>
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                                <td style="padding-left: 4px;">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <div style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                        <div style="float: right; margin-top: 10px; margin-bottom: 31px;">
                            <asp:LinkButton ID="lbtStatusBack" runat="server" CssClass="Tablebutton" Width="78px"
                                OnClick="lbtStatusBack_Click">» Zurück </asp:LinkButton>
                        </div>
                    </div>
                    <div id="divHistory" runat="server" style="color: #595959; border: 1px solid #dfdfdf;"
                        visible="false">
                        <table width="100%">
                            <tr>
                                <td style="height: 30px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 10px;padding-right:10px; height: 250px; vertical-align: top">
                                    <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="gvHistory"
                                        GridLines="None" PageSize="20" Width="100%" EnableModelValidation="True">
                                        <Columns>
                                            <asp:BoundField DataField="LFDNR" HeaderText="Lfd.-Nr.">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="KENNUNG" HeaderText="Kennung">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="STATUS" HeaderText="Status">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ERDAT" DataFormatString="{0:d}" HeaderText="Angelegt am">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ERNAM" HeaderText="Angelegt von">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TEXT" HeaderText="Text">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <HeaderStyle BackColor="#DFDFDF" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <div style="background-color: #dfdfdf; height: 22px; vertical-align: bottom;">
                            &nbsp;
                        </div>
                        <div style="float: right; margin-top: 10px; margin-bottom: 31px;">
                            <asp:LinkButton ID="lbtHistoryBack" runat="server" CssClass="Tablebutton" Width="78px"
                                OnClick="lbtHistoryBack_Click">» Zurück </asp:LinkButton>
                        </div>
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        
                    </div>
                </div>
                
            </div>
        </div>
    </div>
</asp:Content>