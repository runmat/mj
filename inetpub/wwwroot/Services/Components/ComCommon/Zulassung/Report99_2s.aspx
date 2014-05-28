<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report99_2s.aspx.vb" Inherits="CKG.Components.ComCommon.Report99_2s" MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>

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
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="background-color: #dfdfdf;">
                                                <td class="firstLeft active" style="width: 25%">
                                                    <asp:Label ID="lblBundesland" runat="server">Bundesland</asp:Label>
                                                </td>
                                                <td class="active" >
                                                    <asp:Label ID="Label1" runat="server">Formulare für Lastschrifteinzug</asp:Label>
                                                  </td>
                                            </tr> 

                                        </tbody>
                                    </table>
                                <asp:Repeater ID="Repeater1" runat="server">
                                    <ItemTemplate>
                                        <table class="" border="0" cellpadding="0" cellspacing="0" width="100%">
                                             <tr class="formquery">
                                                <td class="firstLeft active" style="width: 25%">
                                                    <%#DataBinder.Eval(Container.DataItem, "Bundesland")%>
                                                </td>
                                               <td class="active">
                                                   <asp:HyperLink ID="HyperLink1" NavigateUrl='<%#DataBinder.Eval(Container.DataItem, "Pfad")%>' runat="server">Download</asp:HyperLink> 
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    </asp:Repeater>
                                    <div id="Queryfooter" runat="server" style="padding-bottom: 15px;background-color: #dfdfdf;">
                                        &nbsp;
                                    </div>
                                </div>

                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
