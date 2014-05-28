<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="Kantine.Default" 
        MasterPageFile="Kantine.Master" EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
  <div id="Main" style="text-align: center;">
        <br />
        <div style="float: left;">
            <asp:Label ID="lblError" runat="server" class="Error"></asp:Label>
        </div>
        <br />
        <br />
        <div class="Heading">
            Default
        </div>
        <div class="Rahmen">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Defaultseite
                    </td>
                </tr>
            </table>
        </div>        
    </div>
</asp:Content>
