<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02.aspx.vb" Inherits="CKG.Components.ComCommon.Change02"
    MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
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
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" cellpadding="0" cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td nowrap="nowrap" width="100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:LinkButton ID="btnConfirm" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                                    Width="130px">» Abfrage starten</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <asp:Panel ID="PanelAdressAenderung" runat="server">
                                    <table id="Table5" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                        <tr id="tr_Kunnr_I" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Kunnr_I" runat="server">lblKunnr_I</asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:Label ID="lblKunnr_IShow" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr_Name1" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Name1" runat="server">lblName1</asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName1" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_Name2" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Name2" runat="server">lblName2</asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName2" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_Land1" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Land1" runat="server">lblLand1</asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtLand1" runat="server" Width="300px" MaxLength="3"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_Pstlz" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Pstlz" runat="server">lblPstlz</asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtPstlz" runat="server" Width="300px" MaxLength="5"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_Ort01" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Ort01" runat="server">lblOrt01</asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtOrt01" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_Stras" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Stras" runat="server">lblStras</asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtStras" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_Telf1" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Telf1" runat="server">lblTelf1</asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtTelf1" runat="server" Width="300px" MaxLength="16"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_Telfx" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Telfx" runat="server">lblTelfx</asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtTelfx" runat="server" Width="300px" MaxLength="31"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_Smtp_Addr" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Smtp_Addr" runat="server">lblSmtp_Addr</asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtSmtp_Addr" runat="server" Width="300px" MaxLength="241"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_Katr9" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Katr9" runat="server">lblKatr9</asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlKatr9" runat="server" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="2" align="right" class="rightPadding" style="width: 100%">
                                                <div id="dataQueryFooter">
                                                    <asp:LinkButton ID="lb_Aendern2" runat="server" CssClass="TablebuttonLarge" Width="130px"
                                                        Height="16px">lb_Aendern2</asp:LinkButton>
                                                    &nbsp;
                                                    <asp:LinkButton ID="lb_Back2" runat="server" CssClass="TablebuttonLarge" Width="130px"
                                                        Height="16px">lb_Back2</asp:LinkButton>
                                                    &nbsp;
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br>
                            </div>
                            <div id="DivPlaceholder" runat="server" style="height: 550px;">
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right">
                                        <img src="../../Images/iconXLS.gif" alt="Excel herunterladen" 
                                            style="height: 14px" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lbCreateExcel" runat="server" Text="Excel herunterladen" ForeColor="White"></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data" style="overflow-x: scroll;">
                                    <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" Width="1040px"
                                            ID="GridView1" CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True"
                                            AllowSorting="True">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle  CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle  CssClass="ItemStyle" />
                                            <EditRowStyle ></EditRowStyle>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <p align="center">
                                                        <asp:ImageButton ID="lb_Aendern" runat="server" ToolTip="Ändern" CommandArgument='<%# Container.DataItemIndex %>'
                                                            CommandName="Aendern" ImageUrl="../../../Images/Edit_01.gif" Height="16px" Width="16px" />
                                                    </p>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField Visible="False" DataField="Kunnr" SortExpression="Kunnr" HeaderText="Kunnr-Hidden">
                                            </asp:BoundField>
                                            <asp:TemplateField SortExpression="Kunnr" HeaderText="col_Kunnr">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Kunnr" runat="server" CommandName="Sort" CommandArgument="Kunnr" ForeColor="White">col_Kunnr_I</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKunnr_IShow2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kunnr") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Name1" HeaderText="col_Name1">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Name1" runat="server" CommandName="Sort" CommandArgument="Name1" ForeColor="White">col_Name1</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Literal ID="Literal22" runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.Kunnr") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.Name1") &amp; "</a>" %>'>
                                                    </asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Name2" HeaderText="col_Name2">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Name2" runat="server" CommandName="Sort" CommandArgument="Name2" ForeColor="White">col_Name2</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName2Show" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name2") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Land1" HeaderText="col_Land1">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Land1" runat="server" CommandName="Sort" CommandArgument="Land1" ForeColor="White">col_Land1</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLand1Show" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Land1") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Pstlz" HeaderText="col_Pstlz">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Pstlz" runat="server" CommandName="Sort" CommandArgument="Pstlz" ForeColor="White">col_Pstlz</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPstlzShow" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Pstlz") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Ort01" HeaderText="col_Ort01">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Ort01" runat="server" CommandName="Sort" CommandArgument="Ort01" ForeColor="White">col_Ort01</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrt01Show" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ort01") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Stras" HeaderText="col_Stras">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Stras" runat="server" CommandName="Sort" CommandArgument="Stras" ForeColor="White">col_Stras</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStrasShow" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Stras") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Telf1" HeaderText="col_Telf1">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Telf1" runat="server" CommandName="Sort" CommandArgument="Telf1" ForeColor="White">col_Telf1</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTelf1Show" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Telf1") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Telfx" HeaderText="col_Telfx">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Telfx" runat="server" CommandName="Sort" CommandArgument="Telfx" ForeColor="White">col_Telfx</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTelfxShow" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Telfx") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Smtp_Addr" HeaderText="col_Smtp_Addr">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Smtp_Addr" runat="server" CommandName="Sort" CommandArgument="Smtp_Addr" ForeColor="White">col_Smtp_Addr</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSmtp_AddrShow" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Smtp_Addr") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Katr9" HeaderText="col_Katr9">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Katr9" runat="server" CommandName="Sort" CommandArgument="Katr9" ForeColor="White">col_Katr9</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKatr9Show" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Katr9") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Vtext" HeaderText="col_Vtext">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Vtext" runat="server" CommandName="Sort" CommandArgument="Vtext" ForeColor="White">col_Vtext</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVtextShow" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vtext") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lbCreateExcel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
        </div>
    </div>
</asp:Content>
