<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dokumentenanforderung_3.aspx.cs" Inherits="AutohausPortal.forms.Dokumentenanforderung_3" MasterPageFile="/AutohausPortal/MasterPage/Form.Master" %>

<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/Form.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="formulare">
        <!-- WICHTIG:
			Für den IE6 ist es leider notwendig, sowohl die Breite der Formfelder als auch die der umgebenden Layer zu definieren.
			Dabei muß der umgebende Layer immer 30 Pixel breiter sein, als das umschließende Formfeld.
			Wenn ein Input-Feld mit einem eingelagerten Button benötigt wird (z.B. ein Datepicker), muß dieser Wert nochmals um 
			25 erhöht werden.
			-->
              <!-- FORMULARLAYER1 -->
                <div style="margin-left: 65px">
                    <h3>
                        <asp:Label ID="lblError" runat="server" Style="color: #B54D4D" Text=""></asp:Label>
                        <asp:Label ID="lblMessage" runat="server" Style="color: #269700" Text=""></asp:Label>      
                    </h3>
                </div>
              <div class="formlayer">
                <div class="formlayer_top"><h2>
                        Vollmachten/Einzugsermächtigung</h2>
                </div>

                <div class="formlayer_plus" style="display:block" id="form1">
                    <!--formularbereich1-->
                    <div class="formularbereich">
                        <div class="formlayer_plus_top">
                            &nbsp;</div>
                        <div class="formlayer_plus_content">
                            <!--formulardaten-->
                            <div class="formulardaten">
                           <h3> Rechte Maustaste auf Download. Ziel speichern unter...</h3>
                                <asp:Repeater ID="Repeater1" runat="server">
                                <ItemTemplate>
                                        <div class="formname" style="width: 200px;">
                                                <%#DataBinder.Eval(Container.DataItem, "Bundesland")%>
                                        </div>
                                        <div class="formname" style="width: 100px; margin-left: 25px">
                                        <asp:HyperLink ID="HyperLink1" NavigateUrl='<%#DataBinder.Eval(Container.DataItem, "Pfad")%>' runat="server">Download</asp:HyperLink> 
                                        </div>

                                        <div class="trenner">
                                            &nbsp;
                                        </div> 
                                 </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </div>
</asp:Content>