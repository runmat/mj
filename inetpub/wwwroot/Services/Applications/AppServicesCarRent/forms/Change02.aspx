<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02.aspx.vb" Inherits="AppServicesCarRent.Change02"  MasterPageFile="../MasterPage/App.Master" %>
<%@ Register TagPrefix="uc" TagName="SearchForm" Src="../../../PageElements/SearchForm/SearchForm.ascx" %>
<%@ Register TagPrefix="c" Namespace="CKG.Services" Assembly = "CKG.Services" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active" Text="zurück" />
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text=""></asp:Label> 
                        </h1>
                    </div>
                    <asp:Panel ID="Panel2" DefaultButton="btndefault" runat="server">
                        <div id="TableQuery">
                            <c:ErrorLabel ID="lblError" runat="server" />
                        
                            <uc:SearchForm ID="FrmSearch" runat="server" />

                            <div id="dataQueryFooter" style="margin: 10px;">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                                <asp:Button style="display:none" OnClientClick="Show_BusyBox1();" UseSubmitBehavior="false" ID="btndefault" runat="server" Text="Button" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
