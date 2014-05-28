<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report02.aspx.cs" Inherits="appCheckServices.forms.Report02" MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="false"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
 
                            <div id="paginationQuery" style="width: 100%; display: block;">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                    <div id="TableQuery">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false"></asp:Label>
                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp
                                </td>
                            </tr>
                            <tr id="trDatumVon" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lblERDatvon" runat="server"  Text="Erstellungdatum von:"></asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtDatumvon" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CE_Datumvon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtDatumvon">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr id="trDatumBis" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lblERDatbis" runat="server"  Text="Erstellungdatum bis:"></asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtDatumBis" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Wählen Sie 'Datum bis' größer als 'Datum von'!"
                                        ControlToCompare="txtDatumvon" ControlToValidate="txtDatumBis" Operator="GreaterThan"
                                        Type="Date"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="2" align="right" style="width: 100%">
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                Height="16px" CausesValidation="False" Font-Underline="False" OnClick="cmdSearch_Click">» Suchen</asp:LinkButton>
                            &nbsp;
                        </div>
                    </div>
                            <div id="Result" runat="Server" visible="false">
                                <div id="pagination">
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" headerCSS="tableHeader"
                                                    bodyCSS="tableBody" CssClass="GridView" AllowSorting="True" AutoGenerateColumns="False"
                                                    OnItemCommand="DataGrid1_ItemCommand" GridLines="None">
                                                    <AlternatingItemStyle BackColor="#DEE1E0"></AlternatingItemStyle>
                                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <Columns>
                                                                        <asp:TemplateColumn Visible="false">                                                                            
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblBELGNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Belegnummer") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>                                                                            
                                                                        <asp:TemplateColumn SortExpression="Filename" HeaderText="col_Dokument">
                                                                            <ItemStyle Width="28%"></ItemStyle>
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="Linkbutton1" runat="server" CommandArgument="Filename" CommandName="Sort">Dokument</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="lbtPDF" runat="server" CommandName="open" Height="18px" Visible="False"
                                                                                    ImageUrl="../../../images/iconPDF.gif" ToolTip="PDF"></asp:ImageButton>
                                                                                <asp:ImageButton ID="lbtExcel" runat="server" CommandName="open" Height="18px" Visible="False"
                                                                                    ImageUrl="../../../Images/iconXLS.gif" ToolTip="Excel"></asp:ImageButton>
                                                                                <asp:ImageButton ID="lbtWord" runat="server" CommandName="open" Height="18px" ImageUrl="../../../Images/Word_Logo.jpg"
                                                                                    Visible="False" ToolTip="Word" />
                                                                                <asp:ImageButton ID="lbtJepg" runat="server" CommandName="open" Height="18px" ImageUrl="../../../Images/jpg-file.png"
                                                                                    ToolTip="JPG" Visible="False" />
                                                                                <asp:ImageButton ID="lbtGif" runat="server" CommandName="open" Height="18px" ImageUrl="../../../Images/Gif_Logo.gif"
                                                                                    ToolTip="GIF" Visible="False" />
                                                                                <asp:ImageButton ID="lbtZip" runat="server" CommandName="open" Height="18px" ImageUrl="../../../Images/Zip.gif"
                                                                                    ToolTip="ZIP" Visible="False" />
                                                                                <asp:LinkButton ID="Linkbutton2" runat="server" Width="229px" Height="18px" CommandName="open"
                                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' ></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn SortExpression="AnzPos" HeaderText="col_AnzPos">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_AnzPos" runat="server" CommandArgument="AnzPos" CommandName="Sort">AnzPos</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAnzPos" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AnzPos") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn SortExpression="geprueft" HeaderText="col_geprueft">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_geprueft" runat="server" CommandArgument="geprueft" CommandName="Sort">geprueft</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgeprueft" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.geprueft") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>  
                                                                        <asp:TemplateColumn SortExpression="nicht_geprueft" HeaderText="col_nicht_geprueft">
                                                                            <HeaderTemplate>
                                                                            
                                                                                <asp:LinkButton ID="col_nicht_geprueft" runat="server" CommandArgument="nicht_geprueft" CommandName="Sort">nicht_geprueft</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                             <ItemStyle  HorizontalAlign="Center" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblnicht_geprueft" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.nicht_geprueft") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn> 

                                                                        <asp:TemplateColumn SortExpression="Rueckmeldung" HeaderText="col_Rueckmeldung">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Rueckmeldung" runat="server" CommandArgument="Rueckmeldung" CommandName="Sort">Rueckmeldung</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRueckmeldung" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Rueckmeldung", "{0:d}") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>                                                                         
                                                                        <asp:TemplateColumn SortExpression="Beauftragungsdatum" HeaderText="col_Beauftragt">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Beauftragt" runat="server" CommandArgument="Beauftragt" CommandName="Sort">Beauftragt</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Beauftragungsdatum", "{0:d}") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>                                                                        
                                                                        <asp:TemplateColumn SortExpression="Erstellungsdatum" HeaderText="col_Erstellt">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Erstellt" runat="server" CommandArgument="Erstellungsdatum" CommandName="Sort">Erstellt</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblErdat" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Erstellungsdatum", "{0:d}") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>

                                                                        
                                                                        <asp:TemplateColumn SortExpression="Downloaddatum" HeaderText="col_Downloaddatum">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Downloaddatum" runat="server" CommandArgument="Downloaddatum" CommandName="Sort">Downloaddatum</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDownloaddatum" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.Downloaddatum", "{0:d}") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>      
                                                                        <asp:TemplateColumn SortExpression="Downloaduser" HeaderText="col_Downloaduser">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Downloaduser" runat="server" CommandArgument="Downloaduser" CommandName="Sort">Downloaduser</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDownloaduser" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Downloaduser") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>                                                                                                                                                                                                                                                                                                                                                                                 
                                                                        <asp:BoundColumn Visible="False" DataField="Serverpfad" SortExpression="Serverpfad">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn Visible="False" DataField="Pattern" SortExpression="Pattern"></asp:BoundColumn>
                                                                    </Columns>
                                                    <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                        PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div>
                       
                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
        </div>
    </div>
</asp:Content>
