<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wartung.aspx.cs" Inherits="Kantine.Wartung"
    MasterPageFile="Kantine.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div id="Main" runat="server">
        <br />
        <br />
        <center>
            <div class="Tablehead" style="width: 300px;">
                Wartungsmodus!
            </div>
            <div class="Rahmen" style="width: 300px;">
                <asp:Label ID="lblError" runat="server" CssClass="Error" Style="margin: 15px 15px 15px 10px; text-align:left;">
                    Das System befindet sich zur Zeit im Wartungsmodus und ist in Kürze wieder verfügbar.
                </asp:Label>
            </div>
        </center>
    </div>
</asp:Content>
