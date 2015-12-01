<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verbandbuch.aspx.cs"
    Inherits="AppZulassungsdienst.forms.Verbandbuch" MasterPageFile="../MasterPage/App.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>                   
                    <div id="paginationQuery">
                    </div>
                    <div id="TableQuery" style="margin-bottom: 10px">
                        <div id="Queryfooter" runat="server">   
                        </div>
                    </div>
                    <div id="dataQueryFooter" class="dataQueryFooter">
                    </div>                         
                    <div id="data" style="text-align: center" >
                        
                                <asp:LinkButton CssClass="TablebuttonXLarge" Width="155px"
                                            Height="22px" runat="server" ID="btnErfassung" OnClick="btnErfassung_Click" Text="Verbandbuch-Erfassung" Style="margin-top: 20px;"/> <br/>
                     
                           
                                <asp:LinkButton CssClass="TablebuttonXLarge" Width="155px"
                                            Height="22px" runat="server" ID="btnReport" OnClick="btnReport_Click" Text="Verbandbuch-Report" Style="margin-top: 10px; margin-bottom: 20px" />
  
                    </div>
                                           
                    <div id="dataFooter" class="dataQueryFooter">   
                    </div>       
                </div>
            </div>
        </div>
    </div>
</asp:Content>
