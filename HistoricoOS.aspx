<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HistoricoOS.aspx.cs" Inherits="HistoricoOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/reset.css" rel="stylesheet" />
    <link href="css/SolicitarOS.css" rel="stylesheet" />
    <link href="css/estilos.css" rel="stylesheet" />
    <link href="css/gridview.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h4 class="tituloPagina">Histórico Ordem de Serviço</h4>
       <div>

   <asp:GridView ID="gdvHistorico"
    runat="server"
    AutoGenerateColumns="False"
    DataKeyNames="idSolicitacao"
    OnRowCommand="gdvHistorico_RowCommand"
    GridLines="Both"
    BorderStyle="Solid"
    BorderWidth="1px"
    CssClass="grid-style">

        <Columns>
            <asp:BoundField DataField="idSolicitacao" HeaderText="Solicitação"></asp:BoundField>
            <asp:BoundField DataField="codPatrimonio" HeaderText="Patrimônio"></asp:BoundField>
            <asp:BoundField DataField="andar" HeaderText="Andar"></asp:BoundField>
            <asp:BoundField DataField="localDaSolicitacao" HeaderText="Local"></asp:BoundField>
            <asp:BoundField DataField="descricaoCentroCusto" HeaderText="Centro de Custo"></asp:BoundField>
            <%--<asp:BoundField DataField="setorSolicitadoDesc" HeaderText="Setor Solicitado"></asp:BoundField>--%>
            <asp:BoundField DataField="descServicoSolicitado" HeaderText="Descrição do Serviço"></asp:BoundField>
            <asp:BoundField DataField="dataSolicitacao" HeaderText="Data da Solicitação"></asp:BoundField>
            <asp:BoundField DataField="statusSolicitacao" HeaderText="Status"></asp:BoundField>
           <%-- <asp:TemplateField HeaderStyle-CssClass="sorting_disabled" HeaderText="Ação">
                <ItemTemplate>
                    <div class="form-inline">
                        <asp:LinkButton ID="lbDadosOS" CommandName="historicoSolicitacao" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'
                            Class="btn btn-outline-primary" runat="server">Selecionar                                                                
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
                <HeaderStyle CssClass="sorting_disabled"></HeaderStyle>
            </asp:TemplateField>--%>
        </Columns>
    </asp:GridView>


    </div>
</asp:Content>

