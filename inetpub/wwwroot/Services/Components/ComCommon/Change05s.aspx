<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change05s.aspx.vb" Inherits="CKG.Components.ComCommon.Change05s" MasterPageFile="../../MasterPage/Services.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">
        .Watermark
        {
            color: Gray;
        }
    </style>


    <div id="site">
        <div id="content">
            <div id="navigationSubmenu" style="height:15px">
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
                            <div id="TableQuery" style="margin-bottom: 10px;background-color:#ECEBEC">
                                  <asp:Panel ID="PanelAdressAenderung" runat="server" style="padding-top:5px;" >
                                    <table id="Table5" cellspacing="0" cellpadding="5" width="100%" border="0">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError" 
                                                    EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server"
                                                    EnableViewState="False" ForeColor="Blue" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td class="firstLeft active" style="width:20%">
                                                <asp:Label ID="Kunnr" runat="server" Text="Auftraggeber:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:Label ID="lblKunnrShow" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblKennung" runat="server" Text="Kennung:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlKennung" runat="server" Width="500px" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblAdresse" runat="server" Text="Adresse:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:DropDownList ID="ddlAdresse" runat="server" Width="500px" 
                                                    AutoPostBack="True" Enabled="False">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                               <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active" style="width:20%">
                                                <asp:Label ID="lblReferenz" runat="server" Text="Referenz:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtReferenz" runat="server" Width="300px" MaxLength="120"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblName1" runat="server" Text="Name1:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName1" runat="server" Width="300px" MaxLength="30"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblName2" runat="server" Text="Name2:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName2" runat="server" Width="300px" MaxLength="30"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="Name2Water" runat="server" TargetControlID="txtName2" WatermarkCssClass="Watermark" WatermarkText="Firma o. Ansprechpartner"></cc1:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblStrasse" runat="server" Text="Straße:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtStrasse" runat="server" Width="300px" MaxLength="30"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblPLZ" runat="server" Text="Postleitzahl:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtPLZ" runat="server" Width="300px" MaxLength="10"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblOrt" runat="server" Text="Ort:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtOrt" runat="server" Width="300px" MaxLength="25"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblLand" runat="server" Text="Land:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:DropDownList ID="ddlLand" runat="server" Width="300px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblTelefon" runat="server" Text="Telefon:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtTelefon" runat="server" Width="300px" MaxLength="30"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblMail" runat="server" Text="E-Mail-Adresse:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtMail" runat="server" Width="300px"></asp:TextBox>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblFax" runat="server" Text="Fax-Nummer:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtFax" runat="server" Width="300px" MaxLength="30"></asp:TextBox>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblLoeschkennzeichen" runat="server" Text="Löschkennzeichen:"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:CheckBox ID="chkLoeschkennzeichen" runat="server" Enabled="False" />
                                            </td>
                                        </tr>
                                </table>
                                </asp:Panel>
                               <div id="dataQueryFooter" style="float:left;padding-left:10px;margin-top:20px;height:40px">
                                                    <asp:ImageButton ID="ibtNeuanlage" runat="server" ImageUrl="/Services/images/Adresspflege/Neuanlage.png" />
                                                    &nbsp;<asp:ImageButton ID="ibtSuchen" runat="server" ImageUrl="/Services/images/Adresspflege/Suche.png" />
                                                    &nbsp;<asp:ImageButton ID="ibtCancel" runat="server" ImageUrl="/Services/images/Adresspflege/Cancel.png" />
                                                    &nbsp;<asp:ImageButton ID="ibtSave" runat="server" ImageUrl="/Services/images/Adresspflege/Save.png" />
                                                    &nbsp;
                                                </div>
                            </div>
                     
                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
        </div>
    </div>
</asp:Content>