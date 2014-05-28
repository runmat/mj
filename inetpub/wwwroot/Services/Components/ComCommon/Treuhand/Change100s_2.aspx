<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change100s_2.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change100s_2" MasterPageFile="../../../MasterPage/Services.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <div id="TableQuery">
                               <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr  class="formquery" >
                                        <td class="firstLeft active" colspan="2"  style="width:100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                                                ></asp:Label>
                                                
                                            <asp:Label ID="lblNoData" runat="server" ></asp:Label>                                                
                                        </td>
                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active" colspan="2"  style="width:100%">
                                             &nbsp;</td>
                                     </tr>
                                </table>
                                <div id="Queryfooter" runat="server" >
                                    &nbsp;
                                </div>
                            </div>
   
                            <div id="Result" runat="Server" visible="false">
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server" ></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0" style="width:100%" >
                                        <tr>
                                            <td>
                                        <asp:GridView Width="100%" AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                            CssClass="GridView" GridLines="None" PageSize="10" AllowPaging="True" 
                                            AllowSorting="True" EnableModelValidation="True">
                                            <PagerSettings Visible="False" />
                                            <EditRowStyle Wrap="False" />
                                            <EmptyDataRowStyle Wrap="False" />
                                            <FooterStyle Wrap="True" />
                                            <HeaderStyle CssClass="GridTableHead" Wrap="False"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" Wrap="False" />
                                            <PagerStyle Wrap="False" />
                                            <RowStyle CssClass="ItemStyle" Wrap="False" />
                                            <Columns>
                                             <asp:TemplateField SortExpression="ID" HeaderText="col_ID" Visible="false" >
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ID" runat="server" CommandName="Sort" CommandArgument="ID">col_ID</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblID" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="SUBRC" HeaderText="col_Anfordern">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Anfordern" runat="server" CommandName="Sort" CommandArgument="SUBRC">col_Anfordern</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAnfordern" runat="server" 
                                                            AutoPostBack="True"  Enabled='<%# DataBinder.Eval(Container, "DataItem.MESSAGE")= "" %>'  Checked='<%# DataBinder.Eval(Container, "DataItem.SELECT") = "99" %>' oncheckedchanged="chkAnfordern_CheckedChanged"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="EQUI_KEY" HeaderText="col_Schluessel">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Schluessel" runat="server" CommandName="Sort" CommandArgument="EQUI_KEY">col_Schluessel</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEQUI_KEY" Text='<%# DataBinder.Eval(Container, "DataItem.EQUI_KEY") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField SortExpression="ZZREFERENZ2" HeaderText="col_Referenznummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Referenznummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ2">col_Referenznummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                     <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblZZREFERENZ2" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ2") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ERNAM" HeaderText="col_Sachbearbeiter">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Sachbearbeiter" runat="server" CommandName="Sort" CommandArgument="ERNAM">col_Sachbearbeiter</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblERNAM" Text='<%# DataBinder.Eval(Container, "DataItem.ERNAM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ERDAT" HeaderText="col_Datum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Datum" runat="server" CommandName="Sort" CommandArgument="ERDAT">col_Datum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblERDAT" Text='<%# DataBinder.Eval(Container, "DataItem.ERDAT", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="SPERRDAT" HeaderText="col_Sperrdatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Sperrdatum" runat="server" CommandName="Sort" CommandArgument="SPERRDAT">col_Sperrdatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSPERRDAT" Text='<%# DataBinder.Eval(Container, "DataItem.SPERRDAT", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField SortExpression="TREUH_VGA" HeaderText="col_Aktion">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Aktion" runat="server" CommandName="Sort" CommandArgument="TREUH_VGA">col_Aktion</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblTREUH_VGA" Text="Sperren" Visible='<%# DataBinder.Eval(Container, "DataItem.TREUH_VGA")= "S" %>'>
                                                        </asp:Label>
                                                        <asp:Label runat="server" ID="lblTREUH_VGA2" Text="Ensperren" Visible='<%# DataBinder.Eval(Container, "DataItem.TREUH_VGA")= "F" %>'>
                                                        </asp:Label>                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="MESSAGE" HeaderText="col_Status">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="MESSAGE">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMESSAGE" Text='<%# DataBinder.Eval(Container, "DataItem.MESSAGE") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ERROR" HeaderText="col_Fehlertext" 
                                                    Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fehlertext" runat="server" CommandName="Sort" CommandArgument="ERROR">col_Fehlertext</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFehlertext" Text='<%# DataBinder.Eval(Container, "DataItem.ERROR") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                                <SelectedRowStyle Wrap="False" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                
                                    <table id="tabCrtl" width="100%" border="0" style="float: left"  >
                                    <tr>
                                        <td align="left" width="230">
                                        <asp:Panel ID="pnInfo" runat="server">
                                            <div class="new_layout" >
                                            <div id="infopanel" class="infopanel">
                                            <label><asp.label id="InfoHead" runat="server">Information!</asp.label></label>
                                            <div>
                                            <asp:Label ID="InfoText" runat="server" >
                                            Vor dem Absenden muss zunächst eine Prüfung erfolgen.
                                            </asp:Label> 
                                            </div></div></div>
                                            </asp:Panel>
                                            
                                        </td>
                                        <td align="right">
                                             <div id="dataQueryFooter" >
                                             <asp:LinkButton ID="cmdCheck" runat="server" CssClass="Tablebutton" Width="78px">» Prüfen</asp:LinkButton>
                                             <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" 
                                             Visible="False" Width="78px">» Absenden</asp:LinkButton>
                                            </div> 
                                        </td>
                                    </tr>
                                    </table>
                                
                                   <br/>
           
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
