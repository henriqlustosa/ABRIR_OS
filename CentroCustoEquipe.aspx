<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CentroCustoEquipe.aspx.cs" Inherits="HistoricoOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/reset.css" rel="stylesheet" />
    <link href="css/SolicitarOS.css" rel="stylesheet" />
    <link href="css/estilos.css" rel="stylesheet" />
    <link href="css/gridview.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h4 class="tituloPagina">Centro de Custo Equipe</h4>
    <div>
        <asp:GridView ID="gdvCentroCustoEquipe"
            runat="server"
            AutoGenerateColumns="False"           
            GridLines="Both"
            BorderStyle="Solid"
            BorderWidth="1px"
            CssClass="grid-style">
            <Columns>
                <asp:BoundField DataField="nomeSolicitante" HeaderText="Nome Funcionario"></asp:BoundField>
                <asp:BoundField DataField="loginSolicitante" HeaderText="Login"></asp:BoundField>
                <asp:BoundField DataField="descricaoCentroCusto" HeaderText="Centro de Custo"></asp:BoundField>
                <asp:BoundField DataField="codCentroCusto" HeaderText="Numero"></asp:BoundField>
                <asp:BoundField DataField="nomeResponsavel_Custo" HeaderText="Responsavel"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

