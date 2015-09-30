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
                    <div id="data">
                        <iframe runat="server" ID="ifrVerbandbuch" width="100%" height="600px" style="border:0;margin:0;padding: 0" ></iframe>
                    </div>                       
                    <div id="dataFooter" class="dataQueryFooter">   
                    </div>       
                </div>
            </div>
        </div>
    </div>
</asp:Content>
