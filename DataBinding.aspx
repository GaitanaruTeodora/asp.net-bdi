<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataBinding.aspx.cs" Inherits="Proiect_GaitanaruTeodora_BDI.Databinding" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .btn_style {
            border-radius: 15px;
            border-color: darkblue;
        }
        .btn-body {
          background-color: #5D7B9D;
          color: white;
          font-weight: bold;
          border-radius: 10px;
          padding: 10px 20px;
          border: none;
          outline: none;
 
        }
        .btn-gv{
          background-color: whitesmoke;
          color: #5D7B9D;
          font-weight: bold;
          border-radius: 10px;
          padding: 10px 20px;
          outline: none;
          border-color: #5D7B9D;
        }
    </style>

</head>
<body>
    <script>
        //setTimeout(function () {
          
        //    document.getElementById("lbMesajValidare").innerHTML="";
          
        //}, 3000);
        //setTimeout(function () {
        //    document.getElementById("lbInsertValidare").innerHTM="";
        //}, 3000);
        //setTimeout(function () {
        //    document.getElementById("lbUpdateValidare").innerHTML = "";
        //}, 3000);
        //setTimeout(function () {
        //    document.getElementById("lbEroareAn").innerHTML = "";
        //}, 3000);
        //setTimeout(function () {
        //    document.getElementById("lbError").innerHTML = "";
        //}, 3000);
        
    </script>

    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbTitluDateGenerale" runat="server" Font-Bold="True" Font-Size="XX-Large" Text="Imprumuturi biblioteca"></asp:Label>
            <hr style="margin-top: 2px; margin-bottom: 10px;" />
            <asp:Button ID="btnCarti" runat="server" OnClick="btnCarti_Click" Text="Toate cartile" CssClass="btn_style" Font-Bold="True" ForeColor="#003366" Height="40px" Width="120px" />
            <asp:Button ID="btnMembri" runat="server" Text="Toti membri" OnClick="btnMembri_Click" CssClass="btn_style" Font-Bold="True" Font-Italic="False" ForeColor="#003366" Height="40px" Width="120px" />

            <asp:Panel ID="PanelCarte" runat="server" Visible="False" Style="margin-bottom: 0px" BorderColor="#000099">
                <br />
                <asp:GridView ID="GvCarte" runat="server" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="GvCarteDS" EnablePersistedSelection="True" EnableSortingAndPagingCallbacks="True" ViewStateMode="Enabled" EnableViewState="False" OnRowDataBound="GvCarte_RowDataBound" CellPadding="6" GridLines="None" ForeColor="#333333">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" Visible="False" />
                        <asp:BoundField DataField="titlu" HeaderText="Titlu" SortExpression="titlu" />
                        <asp:BoundField DataField="editura" HeaderText="Editura" SortExpression="editura" />
                        <asp:BoundField DataField="id_autor" HeaderText="Autor" SortExpression="id_autor" />
                        <asp:BoundField DataField="an_publicare" HeaderText="An publicare" SortExpression="an_publicare" />
                        <asp:BoundField DataField="nr_pagini" HeaderText="Nr. pagini" SortExpression="nr_pagini" />
                        <asp:BoundField DataField="nr_exemplare" HeaderText="Nr. exemplare" SortExpression="nr_exemplare" />
                        <asp:BoundField DataField="nr_disponibile" HeaderText="Nr. disponibile" SortExpression="nr_disponibile" />
                        <asp:BoundField HeaderText="Disponibilitate" SortExpression="nr_disponibile" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <asp:SqlDataSource ID="GvCarteDS" runat="server" ConnectionString="<%$ ConnectionStrings:bibliotecaCS %>" SelectCommand="SELECT * FROM [Carte]"></asp:SqlDataSource>
                <br />
                <asp:Label ID="lbFiltrare" Font-Bold="True" runat="server" Text="Filtreaza cartile dupa autor:"></asp:Label>
                <br />
                <asp:DropDownList ID="DdFiltrareAutor" runat="server" AutoPostBack="True" Height="30px" Width="153px" OnSelectedIndexChanged="DdFiltrareAutor_SelectedIndexChanged">
                </asp:DropDownList>
                
                
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                    <asp:ListItem Text="Toate" Value="Toate" Selected="True" />
                    <asp:ListItem Text="Disponibile" Value="Disponibile" />
                    <asp:ListItem Text="Indisponibile" Value="Indisponibile" />
                </asp:RadioButtonList>
                <br />
                <asp:GridView ID="GvCarteSortat" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="GvCarteSortatDS" AllowPaging="True" CellPadding="4" GridLines="None" OnRowDataBound="GvCarteSortat_RowDataBound" EmptyDataText="Nu exista carti pentru autorul si optiunea selectata." ForeColor="#333333">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="Id" Visible="false" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
                        <asp:BoundField DataField="titlu" HeaderText="Titlu" SortExpression="titlu" />
                        <asp:BoundField DataField="editura" HeaderText="Editura" SortExpression="editura" />
                        <asp:BoundField DataField="id_autor" Visible="false" HeaderText="id_autor" SortExpression="id_autor" />
                        <asp:BoundField DataField="an_publicare" HeaderText="An publicare" SortExpression="an_publicare" />
                        <asp:BoundField DataField="nr_pagini" HeaderText="Nr pagini" SortExpression="nr_pagini" />
                        <asp:BoundField DataField="nr_exemplare" HeaderText="Nr. exemplare" SortExpression="nr_exemplare" />
                        <asp:BoundField DataField="nr_disponibile" HeaderText="Nr. disponibile" SortExpression="nr_disponibile" />
                        <asp:BoundField HeaderText="Disponibilitate" SortExpression="nr_disponibile" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <asp:SqlDataSource ID="GvCarteSortatDS" runat="server" ConnectionString="<%$ ConnectionStrings:bibliotecaCS %>" SelectCommand="SELECT * FROM [Carte] WHERE ([id_autor] = @id_autor)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DdFiltrareAutor" Name="id_autor" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <br />
                <asp:Label ID="Label9" Font-Bold="True" runat="server" Text="Filtreaza cartile dupa anul aparitiei:"></asp:Label>
                <br />
                <asp:Label ID="Label11" runat="server" Text="Filtrati toate cartile publicate inainte de anul: "></asp:Label>
                <asp:TextBox ID="tbAnAparitie" runat="server" BorderStyle="Inset" Width="133px" Height="24px"></asp:TextBox>
                <asp:Button ID="btnFiltreaza" runat="server" CssClass="btn-gv" Text="Filtreaza" OnClick="btnFiltreaza_Click" />
                <br />
                <asp:Label ID="lbEroareAn" runat="server"></asp:Label>
                <br />
                <asp:GridView ID="GvAn" runat="server" CellPadding="4" GridLines="None" OnRowDataBound="GvAn_RowDataBound" ForeColor="#333333">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <asp:Label ID="lblExceptieCarte" runat="server"></asp:Label>
                <br />
            </asp:Panel>
            <br />

            <asp:Panel ID="PanelMembru" runat="server" Visible="False">

                <br />
                <asp:GridView ID="GvMembru" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="GvMembruDS" EnableViewState="False" EmptyDataText="Nu exista membri." CellPadding="4" GridLines="None" ShowFooter="True" OnSelectedIndexChanged="GvMembru_SelectedIndexChanged" ForeColor="#333333">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                        <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" Visible="false" />
                        <asp:BoundField DataField="nume" HeaderText="Nume" SortExpression="nume" />
                        <asp:BoundField DataField="telefon" HeaderText="Telefon" />
                        <asp:BoundField DataField="email" HeaderText="Email" />
                        <asp:TemplateField>
                            <FooterTemplate>
                                <asp:Button ID="BtnToateImprumuturile" CssClass="btn-gv" runat="server" OnClick="BtnToateImprumuturile_Click" Text="Toate imprumuturile" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <asp:SqlDataSource ID="GvMembruDS" runat="server" ConnectionString="<%$ ConnectionStrings:bibliotecaCS %>" SelectCommand="SELECT * FROM [Membru]"></asp:SqlDataSource>
                <br />
              <%--  <div style="display: inline;" >--%>
                    
                <asp:Label ID="lbIstoricImprumuturi" Font-Bold="True"  runat="server" Visible="False"></asp:Label>
                <br />
                <br />
                <asp:GridView ID="GvToateImprumuturile" runat="server" CellPadding="4" GridLines="None" OnRowDataBound="GvToateImprumuturile_RowDataBound" Visible="False" ForeColor="#333333">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <asp:GridView ID="GvImprumut" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Id" DataSourceID="GvImprumutDS" EnableViewState="False" GridLines="None" OnRowDeleting="GvImprumut_RowDeleting" OnRowUpdating="GvImprumut_RowUpdating" ShowFooter="True" ForeColor="#333333" OnSelectedIndexChanged="GvImprumut_SelectedIndexChanged1">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnInserare" CssClass="btn-gv" runat="server" OnClick="btnInserare_Click" Text="Insert" Width="180px" />
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Select" Text="Select"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nr. imprumut" InsertVisible="False" SortExpression="Id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Membru" SortExpression="id_membru">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DdMembruEditare" runat="server" DataSourceID="DdMembruEditareDS" DataTextField="nume" DataValueField="Id" SelectedValue='<%# Bind("id_membru") %>'>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="DdMembruEditareDS" runat="server" ConnectionString="<%$ ConnectionStrings:bibliotecaCS %>" SelectCommand="SELECT * FROM [Membru]"></asp:SqlDataSource>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="DdMembruIns" runat="server" DataSourceID="DdMembruInserare" DataTextField="nume" DataValueField="Id" Enabled="False" Width="157px">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="DdMembruInserare" runat="server" ConnectionString="<%$ ConnectionStrings:bibliotecaCS %>" SelectCommand="SELECT * FROM [Membru] WHERE ([Id] = @Id)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="GvMembru" Name="Id" PropertyName="SelectedValue" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# GetNumeMembru(Eval("id_membru")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data imprumut" SortExpression="data_imprumut">
                            <EditItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("data_imprumut", "{0:d}") %>'></asp:Label>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("data_imprumut") %>' Visible="False"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtDataIInserare" runat="server" TextMode="Date"></asp:TextBox>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("data_imprumut", "{0:d}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data scadenta" SortExpression="data_scadenta">
                            <EditItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("data_scadenta", "{0:d}") %>'></asp:Label>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("data_scadenta") %>' Visible="False"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="Label6" runat="server" Text="Calculata automat + 21 de zile"></asp:Label>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("data_scadenta", "{0:d}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data returnare" SortExpression="data_returnare">
                            <EditItemTemplate>
                                <asp:TextBox ID="TbDataREditare" runat="server" Text='<%# Bind("data_returnare") %>' TextMode="Date"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("data_returnare", "{0:d}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" CssClass="hide" ForeColor="White" Font-Bold="True" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <asp:SqlDataSource ID="GvImprumutDS" runat="server" ConnectionString="<%$ ConnectionStrings:bibliotecaCS %>" DeleteCommand="DELETE FROM [Imprumut] WHERE [Id] = @Id" InsertCommand="INSERT INTO [Imprumut] ([id_membru], [data_imprumut], [data_scadenta], [data_returnare]) VALUES (@id_membru, @data_imprumut, @data_scadenta, @data_returnare)" SelectCommand="SELECT * FROM [Imprumut] WHERE ([id_membru] = @id_membru)" UpdateCommand="UPDATE [Imprumut] SET [id_membru] = @id_membru, [data_imprumut] = @data_imprumut, [data_scadenta] = @data_scadenta, [data_returnare] = @data_returnare WHERE [Id] = @Id">
                    <DeleteParameters>
                        <asp:Parameter Name="Id" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="id_membru" Type="Int32" />
                        <asp:Parameter DbType="Date" Name="data_imprumut" />
                        <asp:Parameter DbType="Date" Name="data_scadenta" />
                        <asp:Parameter DbType="Date" Name="data_returnare" />
                    </InsertParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GvMembru" Name="id_membru" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="id_membru" Type="Int32" />
                        <asp:Parameter DbType="Date" Name="data_imprumut" />
                        <asp:Parameter DbType="Date" Name="data_scadenta" />
                        <asp:Parameter DbType="Date" Name="data_returnare" />
                        <asp:Parameter Name="Id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>

                <asp:Label ID="lbInsertValidare" runat="server"></asp:Label>
                <asp:Label ID="lbUpdateValidare" runat="server"></asp:Label>
                <br />
                <%--<asp:ListView ID="lvCarti" runat="server" DataSourceID="LvCartiDS">
                </asp:ListView>--%>
                <br />


    
                <asp:GridView ID="GvImprumutCarte" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Id" DataSourceID="GvImprumutCarteDS" GridLines="None" ShowFooter="True" ShowFooterWhenEmpty="True" ShowHeaderWhenEmpty="True" OnRowDataBound="GvImprumutCarte_RowDataBound" OnRowDeleting="GvImprumutCarte_RowDeleting" ForeColor="#333333">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <FooterTemplate>
                                <asp:Button ID="btnAdaugaCarte" runat="server" CssClass="btn-gv" OnClick="btnAdaugaCarte_Click" Text="Adauga carte" />
                            </FooterTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="Label7" runat="server" Text="Lista de carti"></asp:Label>
                                <asp:Button ID="btnAdaugaCarteHeader" runat="server" CssClass="btn-gv" OnClick="btnAdaugaCarteHeader_Click" Text="Adauga carte" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Id" InsertVisible="False" SortExpression="Id" Visible="false">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nr. imprumut" SortExpression="id_imprumut" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("id_imprumut") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Carte" SortExpression="id_carte">
                            <FooterTemplate>
                                <asp:DropDownList ID="DdAutorInserare" runat="server" AutoPostBack="True" DataSourceID="DdAutorInserareDS" DataTextField="nume" DataValueField="Id">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="DdAutorInserareDS" runat="server" ConnectionString="<%$ ConnectionStrings:bibliotecaCS %>" SelectCommand="SELECT * FROM [Autor]"></asp:SqlDataSource>
                                <asp:DropDownList ID="DdAdaugaCarte" runat="server" DataSourceID="DdAdaugaCarteDS" DataTextField="titlu" DataValueField="Id">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="DdAdaugaCarteDS" runat="server" ConnectionString="<%$ ConnectionStrings:bibliotecaCS %>" SelectCommand="SELECT * FROM [Carte] WHERE ([id_autor] = @id_autor)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="DdAutorInserare" Name="id_autor" PropertyName="SelectedValue" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </FooterTemplate>
                            <HeaderTemplate>
                                <asp:DropDownList ID="DdAutorInserareHeader" runat="server" AutoPostBack="True" DataSourceID="DdHeaderAutorDS" DataTextField="nume" DataValueField="Id">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="DdHeaderAutorDS" runat="server" ConnectionString="<%$ ConnectionStrings:bibliotecaCS %>" SelectCommand="SELECT * FROM [Autor]"></asp:SqlDataSource>
                                <asp:DropDownList ID="DdAdaugaCarteHeader" runat="server" DataSourceID="DdheaderCarteDS" DataTextField="titlu" DataValueField="Id">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="DdheaderCarteDS" runat="server" ConnectionString="<%$ ConnectionStrings:bibliotecaCS %>" SelectCommand="SELECT * FROM [Carte] WHERE ([id_autor] = @id_autor)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="DdAutorInserareHeader" Name="id_autor" PropertyName="SelectedValue" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# GetTitluCarte(Eval("id_carte")) %>'></asp:Label>
                                <asp:TextBox ID="TextBox1" runat="server" Height="16px" Text='<%# Bind("id_carte") %>' Visible="False"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <EditRowStyle BackColor="#999999" />

                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <asp:SqlDataSource ID="GvImprumutCarteDS" runat="server" ConnectionString="<%$ ConnectionStrings:bibliotecaCS %>"
                    SelectCommand="SELECT * FROM [Imprumut_carte] WHERE ([id_imprumut] = @id_imprumut)" DeleteCommand="DELETE FROM [Imprumut_carte] WHERE [Id] = @Id" InsertCommand="INSERT INTO [Imprumut_carte] ([id_imprumut], [id_carte]) VALUES (@id_imprumut, @id_carte)" UpdateCommand="UPDATE [Imprumut_carte] SET [id_imprumut] = @id_imprumut, [id_carte] = @id_carte WHERE [Id] = @Id">
                    <DeleteParameters>
                        <asp:Parameter Name="Id" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="id_imprumut" Type="Int32" />
                        <asp:Parameter Name="id_carte" Type="Int32" />
                    </InsertParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GvImprumut" Name="id_imprumut" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="id_imprumut" Type="Int32" />
                        <asp:Parameter Name="id_carte" Type="Int32" />
                        <asp:Parameter Name="Id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                <br />
                <asp:Label ID="lbError" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblEroareGenerala" runat="server"></asp:Label>
                <br />
                <br />

                <asp:Label ID="LbStatistici" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Statistici">

                </asp:Label>
                <hr style="margin-top: 2px; margin-bottom: 10px;" />
                <br />
                <asp:Button ID="btnFunctie1" CssClass="btn-body" runat="server" OnClick="btnFunctie1_Click" Text="Situatii exemplare carti" />
                <asp:Button ID="btnProcedura2" runat="server" CssClass="btn-body" OnClick="btnProcedura2_Click" Style="margin-bottom: 0px" Text="Istoric imprumuturi pe autor" />
                <asp:Button ID="btnProcedura3" runat="server" CssClass="btn-body" OnClick="btnProcedura3_Click" Text="Performanta membrilor" />
                <asp:Button ID="btnProcedura5" runat="server" CssClass="btn-body" OnClick="btnProcedura5_Click" Text="Raport Membru" />
                <br />
                <br />
                <asp:Label ID="lbFunctie1" runat="server" Text="Label" Visible="False"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lbProcedura2" runat="server" Text="⚫ Procentul imprumuturilor si numarul de imprumuturi efectuate pe fiecare autor:" Visible="False"></asp:Label>
                <br />
                <br />
                <asp:GridView ID="GvProcedura2" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Visible="False">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <br />
                <asp:Label ID="lbProc3" runat="server" Text="⚫ Selectati un membru pentru a obtine numarul cartilor si al paginilor citite:" Visible="False"></asp:Label>
                <br />
                <br />
                <asp:DropDownList ID="DdProcedura3" runat="server" AutoPostBack="True" DataTextField="nume" DataValueField="Id" Height="40px" OnSelectedIndexChanged="DdProcedura3_SelectedIndexChanged" Visible="False">
                </asp:DropDownList>
                <br />
                <asp:Label ID="lbProcedura3Carti" runat="server" Text="Numarul de carti citite: " Visible="False"></asp:Label>
                <asp:TextBox ID="tbProcedura3Carti" runat="server" Enabled="false" Height="32px" Style="margin-left: 14px; margin-bottom: 0px" Visible="False" Width="163px"></asp:TextBox>
                <br />
                <asp:Label ID="lbProcedura3Pagini" runat="server" Text="Numarul de pagini citite:" Visible="False"></asp:Label>
                <asp:TextBox ID="tbProcedura3Pagini" runat="server" Enabled="false" Height="31px" Style="margin-left: 5px" Visible="False" Width="162px"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="Label10" runat="server" Text="⚫ Vizualizeaza membrul cu numarul maxim de pagini citite:" Visible="False"></asp:Label>
                <asp:Button ID="btnProcedura4" runat="server" CssClass="btn-gv" OnClick="btnProcedura4_Click" Text="Vizualizeaza" Visible="False" />
                <br />
                <br />
                <asp:Label ID="lblProcedura4" runat="server" Text="Label" Visible="False"></asp:Label>
                <br />
                <br />
                &nbsp;<asp:Label ID="lblProcedura5" runat="server"  Text="⚫ Selectati membrul pentru a vedea lista tuturor cartilor imprumutate de membrul respectiv precum si data imprumutului:" Visible="False"></asp:Label>
                &nbsp;
                <br />
                &nbsp;<asp:DropDownList ID="ddProcedura5" runat="server" AutoPostBack="True" DataTextField="nume" DataValueField="Id" Height="54px" OnSelectedIndexChanged="DdSelecteazaMembru_SelectedIndexChanged" Visible="False" Width="215px">
                </asp:DropDownList>
                <br />
                <br />
                <asp:TextBox ID="txtProcedura5" runat="server" Height="206px" TextMode="MultiLine" Visible="False" Width="278px" BorderColor="#003366" BorderStyle="Double" Font-Bold="True"></asp:TextBox>
                <br />
                <asp:Label ID="lblError" runat="server" Text="Label" Visible="False"></asp:Label>
                <br />
                <asp:Label ID="lbExceptieMembru" runat="server"></asp:Label>
                <br />
                <asp:Label ID="Label8" runat="server" Text="Reprezentare grafica" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                <br />
                <hr style="margin-top: 2px; margin-bottom: 10px;" />
                <br />
                <asp:Button ID="btnGrafic1" runat="server" CssClass="btn-body" OnClick="btnGrafic1_Click" Text="Evolutia imprumuturilor din anul 2023" />
                <br />
                <br />
                <asp:Button ID="btnGrafic2" runat="server" CssClass="btn-body" Height="35px" OnClick="btnGrafic2_Click" Text="Istoric imprumuturi pe autor (numeric &amp; procentual)" />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
            </asp:Panel>
            <br />
        </div>
    </form>
</body>
</html>
