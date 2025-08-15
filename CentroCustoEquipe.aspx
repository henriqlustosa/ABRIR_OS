<%@ Page Title="Centro de Custo Equipe" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CentroCustoEquipe.aspx.cs" Inherits="HistoricoOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/reset.css" rel="stylesheet" />
    <link href="css/SolicitarOS.css" rel="stylesheet" />
    <link href="css/estilos.css" rel="stylesheet" />
    <link href="css/gridview.css" rel="stylesheet" />

    <style>
        /* Card/linha */
        .grid-wrap {
            margin-top: 8px;
            background: #fff;
            border: 1px solid #e5e7eb;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 1px 2px rgba(0,0,0,.04);
        }
        .grid-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px 16px;
            background: #f3f4f6;
            border-bottom: 1px solid #e5e7eb;
        }
        .grid-header h4 {
            margin: 0;
            font-size: 18px;
            color: #1f2937;
            font-weight: 600;
        }

        /* Chips */
        .resp-chip {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            padding: 6px 10px;
            border-radius: 999px;
            background: #e6f0ff;
            color: #1e40af;
            font-weight: 600;
            font-size: 14px;
            border: 1px solid #bfdbfe;
        }

        /* GridView */
        .grid-style {
            width: 100%;
            border-collapse: collapse;
        }
        .grid-style th, .grid-style td {
            padding: 10px 12px;
            border: 1px solid #e5e7eb;
        }
        .grid-style th {
            background: #f9fafb;
            color: #374151;
            text-align: left;
            font-weight: 700;
        }
        .grid-style tr:nth-child(even) td { background: #fafafa; }
        .grid-style tr:hover td { background: #f1f5f9; }

        /* Larguras sugeridas */
        .col-nome { width: 34%; }
        .col-login { width: 12%; white-space: nowrap; }
        .col-ccusto { width: 28%; }
        .col-numero { width: 8%; white-space: nowrap; text-align: right; }
        .col-resp { width: 18%; }
        .col-diret { width: 12%; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- ===== Modo 1: um único trio (chips no topo + Grid completo) ===== -->
    <asp:Panel ID="pnlListaSimples" runat="server" Visible="true" CssClass="grid-wrap">
        <div class="grid-header">
            <h4 class="tituloPagina" style="padding:0;margin:0;">Centro de Custo Equipe</h4>
            <div>
                <asp:Panel ID="pnlCentro" runat="server" Visible="false" style="display:inline-block;margin-left:8px;">
                    <span class="resp-chip"><span>Centro de Custo:</span> <asp:Label ID="lblCentro" runat="server" /></span>
                </asp:Panel>
                <asp:Panel ID="pnlNumero" runat="server" Visible="false" style="display:inline-block;margin-left:8px;">
                    <span class="resp-chip"><span>Número:</span> <asp:Label ID="lblNumero" runat="server" /></span>
                </asp:Panel>
                <asp:Panel ID="pnlResponsavel" runat="server" Visible="false" style="display:inline-block;margin-left:8px;">
                    <span class="resp-chip"><span>Responsável:</span> <asp:Label ID="lblResponsavel" runat="server" /></span>
                </asp:Panel>
                <asp:Panel ID="pnlDiretoria" runat="server" Visible="false" style="display:inline-block;margin-left:8px;">
                    <span class="resp-chip"><span>Diretoria:</span> <asp:Label ID="lblDiretoria" runat="server" /></span>
                </asp:Panel>
            </div>
        </div>

        <asp:GridView ID="gdvCentroCustoEquipe"
            runat="server"
            AutoGenerateColumns="False"
            GridLines="None"
            BorderStyle="None"
            CssClass="grid-style"
            AllowSorting="true">
            <Columns>
                <asp:BoundField DataField="nomeSolicitante" HeaderText="Nome Funcionário">
                    <HeaderStyle CssClass="col-nome" /><ItemStyle CssClass="col-nome" />
                </asp:BoundField>
                <asp:BoundField DataField="loginSolicitante" HeaderText="Login">
                    <HeaderStyle CssClass="col-login" /><ItemStyle CssClass="col-login" />
                </asp:BoundField>
                <asp:BoundField DataField="descricaoCentroCusto" HeaderText="Centro de Custo">
                    <HeaderStyle CssClass="col-ccusto" /><ItemStyle CssClass="col-ccusto" />
                </asp:BoundField>
                <asp:BoundField DataField="codCentroCusto" HeaderText="Número">
                    <HeaderStyle CssClass="col-numero" /><ItemStyle CssClass="col-numero" />
                </asp:BoundField>
                <asp:BoundField DataField="nomeResponsavel_Custo" HeaderText="Responsável">
                    <HeaderStyle CssClass="col-resp" /><ItemStyle CssClass="col-resp" />
                </asp:BoundField>
            
                <asp:BoundField DataField="diretoria" HeaderText="Diretoria">
                    <HeaderStyle CssClass="col-diret" /><ItemStyle CssClass="col-diret" />
                </asp:BoundField>
           
            </Columns>
        </asp:GridView>
    </asp:Panel>

    <!-- ===== Modo 2: vários trios (cada trio = chips + Grid de membros Nome/Login) ===== -->
    <asp:Panel ID="pnlGrupos" runat="server" Visible="false">
        <asp:Repeater ID="rptGrupos" runat="server" OnItemDataBound="rptGrupos_ItemDataBound">
            <ItemTemplate>
                <div class="grid-wrap" style="margin-top:12px;">
                    <div class="grid-header">
                        <h4 class="tituloPagina" style="padding:0;margin:0;">Equipe</h4>
                        <div>
                            <span class="resp-chip" style="margin-left:8px;"><span>Centro de Custo:</span> <%# Eval("Centro") %></span>
                            <span class="resp-chip" style="margin-left:8px;"><span>Número:</span> <%# Eval("Numero") %></span>
                            <span class="resp-chip" style="margin-left:8px;"><span>Responsável:</span> <%# Eval("Responsavel") %></span>
                            <span class="resp-chip" style="margin-left:8px;"><span>Diretoria:</span> <%# Eval("Diretoria") %></span>
                        </div>
                    </div>

                    <asp:GridView ID="gvMembros"
                        runat="server"
                        AutoGenerateColumns="False"
                        GridLines="None"
                        BorderStyle="None"
                        CssClass="grid-style">
                        <Columns>
                            <asp:BoundField DataField="Nome" HeaderText="Nome Funcionário" />
                            <asp:BoundField DataField="Login" HeaderText="Login" />
                        </Columns>
                    </asp:GridView>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </asp:Panel>

</asp:Content>

