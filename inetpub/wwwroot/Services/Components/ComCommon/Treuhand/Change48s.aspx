<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change48s.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change48s" MasterPageFile="../../../MasterPage/Services.Master" %>


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
                               <table id="tab1" visible="false" runat="server" cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr  class="formquery" >
                                        <td class="firstLeft active"  style="width:100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                                                ></asp:Label>
                                                
                                            <asp:Label ID="lblNoData" runat="server" ></asp:Label>                                                
                                        </td>
                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active"  style="width:100%">
                                             &nbsp;</td>
                                     </tr>
                                </table>
                            </div>
   
                            <div id="Result" runat="Server" visible="false">
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" style="width:100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" 
                                            AllowSorting="True" >
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField SortExpression="AppID" Visible="false" HeaderText="AppID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppID") %>'></asp:Label> 
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AppName" Visible="false" HeaderText="AppName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AppURL" Visible="false" HeaderText="AppURL">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppURL" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppURL") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AuthorizationID" Visible="false" HeaderText="AuthorizationID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAuthorizationID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AuthorizationID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Auswahl">
                                                    <HeaderStyle Width="65px"/>
                                                    <ItemTemplate>
                                                        <span style="float:left">
                                                            <asp:ImageButton ID="ibtnAutorisieren" runat="server"  CommandName="Autorisieren" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Autorisieren" 
                                                                ImageUrl="../../../Images/Confirm_mini.gif" Height="16px" Width="16px"
                                                                />
                                                        </span>
                                                        <span style="float:right">
                                                            <asp:ImageButton ID="ibtnDel" runat="server" ToolTip="Löschen" CommandName="Loeschen" CommandArgument='<%# Container.DataItemIndex %>' Visible='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'
                                                                ImageUrl="../../../Images/Papierkorb_01.gif" Height="16px" Width="16px"
                                                                />
                                                        </span>                                                    
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AppFriendlyName" HeaderText="col_Anwendung">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Anwendung" runat="server" CommandName="Sort" CommandArgument="AppFriendlyName">col_Anwendung</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblAnwendung" Text='<%# DataBinder.Eval(Container, "DataItem.AppFriendlyName") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="InitializedBy" HeaderText="col_Initiator">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Initiator" runat="server" CommandName="Sort" CommandArgument="InitializedBy">col_Initiator</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblInitiator" Text='<%# DataBinder.Eval(Container, "DataItem.InitializedBy") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField SortExpression="InitializedWhen" HeaderText="col_Angelegt">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Angelegt" runat="server" CommandName="Sort" CommandArgument="InitializedWhen">col_Angelegt</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblAngelegt" Text='<%# DataBinder.Eval(Container, "DataItem.InitializedWhen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="CustomerReference" HeaderText="col_Referenz">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Referenz" runat="server" CommandName="Sort" CommandArgument="CustomerReference">col_Referenz</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblReferenz" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerReference") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField SortExpression="ProcessReference" HeaderText="col_Merkmal1">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Merkmal1" runat="server" CommandName="Sort" CommandArgument="ProcessReference">col_Merkmal1</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMerkmal1" Text='<%# DataBinder.Eval(Container, "DataItem.ProcessReference") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ProcessReference2" HeaderText="col_Merkmal2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Merkmal2" runat="server" CommandName="Sort" CommandArgument="ProcessReference2">col_Merkmal2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMerkmal2" Text='<%# DataBinder.Eval(Container, "DataItem.ProcessReference2") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="BatchAuthorization" HeaderText="col_SammelAuth">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_SammelAuth" runat="server" CommandName="Sort" CommandArgument="BatchAuthorization">col_SammelAuth</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
														<asp:CheckBox id="chkBatchAuth" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'>
														</asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                                                            
                                            </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                        <uc2:GridNavigation ID="GridNavigation2" Visible="false" runat="server"></uc2:GridNavigation>                                            
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView2"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" 
                                            AllowSorting="True">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField SortExpression="AppID" Visible="false" HeaderText="AppID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppID") %>'></asp:Label> 
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AppName" Visible="false" HeaderText="AppName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AppURL" Visible="false" HeaderText="AppURL">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppURL" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppURL") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AuthorizationID" Visible="false" HeaderText="AuthorizationID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAuthorizationID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AuthorizationID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AppFriendlyName" HeaderText="Anwendung">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lbtnAnwendung" runat="server" CommandName="Sort" CommandArgument="AppFriendlyName">Anwendung</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblAnwendung" Text='<%# DataBinder.Eval(Container, "DataItem.AppFriendlyName") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="InitializedBy" HeaderText="col_Initiator2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Initiator2" runat="server" CommandName="Sort" CommandArgument="InitializedBy">col_Initiator2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblInitiator" Text='<%# DataBinder.Eval(Container, "DataItem.InitializedBy") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField SortExpression="InitializedWhen" HeaderText="col_Angelegt2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Angelegt2" runat="server" CommandName="Sort" CommandArgument="InitializedWhen">col_Angelegt2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblAngelegt" Text='<%# DataBinder.Eval(Container, "DataItem.InitializedWhen", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="CustomerReference" HeaderText="col_Referenz2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Referenz2" runat="server" CommandName="Sort" CommandArgument="CustomerReference">col_Referenz2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblReferenz" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerReference") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField SortExpression="ProcessReference" HeaderText="col_ProcessReference">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Merkmal1" runat="server" CommandName="Sort" CommandArgument="ProcessReference">col_ProcessReference</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblProcessReference" Text='<%# DataBinder.Eval(Container, "DataItem.ProcessReference") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
											<asp:BoundField DataField="Ergebnis" SortExpression="Ergebnis" HeaderText="Ergebnis"></asp:BoundField>
                                                                                        
                                            </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>                                    
                                </div>
                             <div id="dataQueryFooter" >
                                <asp:LinkButton ID="cmdSave" runat="server" CssClass="TablebuttonLarge" Height="16px" Width="130px">» Autorisieren</asp:LinkButton>
                           </div>                                
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
