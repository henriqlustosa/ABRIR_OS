<%@ Page Title="Solicitação de OS" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SolicitarOS.aspx.cs" Inherits="SolicitarOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/reset.css" rel="stylesheet" />
    <link href="css/SolicitarOS.css?v=2" rel="stylesheet" />
    <link href="css/estilos.css" rel="stylesheet" />
    <link href="css/modal.css" rel="stylesheet" />
    <link href="css/validacaoObrigatoria.css" rel="stylesheet" />

    <style>
        .erro-campo {
            border: 2px solid red !important;
            background-color: #fff0f0;
            position: relative;
        }

            .erro-campo::after {
                content: "*";
                color: red;
                position: absolute;
                top: 6px;
                right: 10px;
                font-weight: bold;
                font-size: 16px;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h4 class="tituloPagina">Solicitação de Ordem de Serviço</h4>

    <main class="container">
        <!-- CENTRO DE CUSTO -->
        <section class="centroDeCusto">
            <div class="boxCentroDeCusto">
                <asp:Label ID="Label1" runat="server" Text="Centro de Custo:" />
                <div class="alinhaTxtDdl">
                    <asp:TextBox ID="txtCentrodeCusto" CssClass="txtCentrodeCusto input" runat="server" ReadOnly="true" />
                    <asp:DropDownList ID="ddlSetor" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlSetor_SelectedIndexChanged"
                        CssClass="ddlSetor input" Height="34px" />
                </div>
            </div>
        </section>

        <hr />

        <!-- RESPONSÁVEL E USUÁRIO -->
        <section class="boxSolicitacao">
            <div class="divResponsavel">
                <div class="alinhaLblTxt divNome">
                    <asp:Label ID="Label2" runat="server" Text="Responsável:" />
                    <asp:TextBox ID="txtNomeResponsavel" CssClass="txtNomeResponsavel input" runat="server" Enabled="false" />
                </div>
                <div class="divRfRamal">
                    <div class="alinhaLblTxt divRf">
                        <asp:Label ID="lblRfResponsavel" runat="server" Text="R.F.:" />
                        <asp:TextBox ID="txtRfResponsavel" CssClass="txtRfResponsavelt input" runat="server" Enabled="false" />
                    </div>
                    <div class="alinhaLblTxt divRamal">
                        <asp:Label ID="lblRamalResponsavel" runat="server" Text="Ramal:" />
                        <asp:TextBox ID="txtRamalResponsavel" CssClass="txtRamalResponsavel input" runat="server" />
                    </div>
                </div>
            </div>

            <div class="divUsuario">
                <div class="alinhaLblTxt divNome">
                    <asp:Label ID="Label3" runat="server" Text="Usuário:" />
                    <asp:TextBox ID="txtNomeUsuario" CssClass="txtNomeUsuario input" runat="server" Enabled="false" />
                </div>
                <div class="divRfRamal">
                    <div class="alinhaLblTxt divRf">
                        <asp:Label ID="Label4" runat="server" Text="R.F.:" />
                        <asp:TextBox ID="txtRfUsuario" CssClass="txtRfUsuario input" runat="server" Enabled="false" />
                    </div>
                    <div class="alinhaLblTxt divRamal">
                        <asp:Label ID="Label5" runat="server" Text="Ramal:" />
                        <asp:TextBox ID="txtRamalUsuario" CssClass="txtRamalUsuario input" runat="server" data-obrigatorio="true" />
                    </div>
                </div>
            </div>

            <hr />

            <!-- DADOS DA SOLICITAÇÃO -->
            <div class="solicitacao">
                <div class="alinhaLblTxt equipamento">
                    <div class="alinhaLblTxt divPatrimonio">
                        <asp:Label ID="Label8" runat="server" Text="Patrimônio:" />
                        <div class="alinhaTxtBtn">
                            <asp:TextBox ID="txtPatrimonio" CssClass="input txtPatrimonio" runat="server" data-obrigatorio="true" />
                            <asp:Button ID="btnPesquisarPatrimonio" CssClass="btnPesquisarPatrimonio" runat="server" OnClick="btnPesquisarPatrimonio_Click" />
                        </div>
                    </div>
                    <div class="alinhaLblTxt divEquipamento">
                        <asp:Label ID="Label6" runat="server" Text="Equipamento:" />
                        <asp:TextBox ID="txtEquipamento" CssClass="ddlEquipamento input" runat="server"
                            placeholder="**Serviços sem Patrimônio: digite 46879**" data-obrigatorio="true" />
                    </div>
                </div>

                <div class="informarcoes">
                    <div class="alinhaLblTxt divAndar">
                        <asp:Label ID="Label7" runat="server" Text="Andar:" />
                        <asp:TextBox ID="txtAndar" CssClass="input" runat="server" data-obrigatorio="true" />
                    </div>
                    <div class="alinhaLblTxt divLocal">
                        <asp:Label ID="Label10" runat="server" Text="Local:" />
                        <asp:TextBox ID="txtLocal" CssClass="txtLocal input" runat="server" data-obrigatorio="true" />
                    </div>
                </div>

                <div class="alinhaLblTxt divDescricao">
                    <asp:Label ID="lblDescricao" runat="server" Text="Descrição do serviço:" />
                    <asp:TextBox ID="txtDescricao" runat="server" CssClass="txtDescricao" TextMode="MultiLine" data-obrigatorio="true" />
                </div>

                <div class="alinhaLblTxt divDescricao">
                    <asp:Label ID="LabelObs" runat="server" Text="Observações:" />
                    <asp:TextBox ID="txtObs" runat="server" CssClass="txtDescricao" TextMode="MultiLine" />
                </div>
            </div>

            <hr />

            <div class="divBotao">
                <asp:Button ID="btnSolicitarOS" runat="server" Text="Solicitar"
                    CssClass="btnSolicitar"
                    OnClientClick="return validarFormulario();"
                    OnClick="btnSolicitarOS_Click" />
            </div>
        </section>
    </main>

    <!-- Modal Customizado HSPM -->
    <div id="modalMensagem" class="modal-hspm">
        <div class="modal-hspm-conteudo">
            <div class="modal-hspm-topo">
                <strong>Atenção</strong>
                <span class="modal-hspm-fechar" onclick="fecharModal()">×</span>
            </div>
            <div class="modal-hspm-corpo" id="mensagemModalBody"></div>
        </div>
    </div>

    <!-- JS -->
    <script type="text/javascript">

        // Remove erro visual ao digitar
        document.addEventListener('DOMContentLoaded', function () {
            const camposObrigatorios = document.querySelectorAll('[data-obrigatorio="true"]');

            camposObrigatorios.forEach(campo => {
                campo.addEventListener('input', function () {
                    if (campo.value.trim() !== '') {
                        campo.classList.remove('erro-campo');
                    }
                });
            });
        });


        window.onload = function () {
            var usuario = '<%= Session["login"] != null ? Session["login"].ToString() : "" %>';
            if (usuario === "") {
                alert("Sessão expirada. Você será redirecionado para o login.");
                window.location.href = "../login.aspx";
            }
        };

        function MostrarMensagem(msgHtml) {
            document.getElementById("mensagemModalBody").innerHTML = msgHtml;
            document.getElementById("modalMensagem").style.display = "block";

            window.onclick = function (event) {
                const modal = document.getElementById("modalMensagem");
                if (event.target === modal) {
                    fecharModal();
                }
            };
        }

        function fecharModal() {
            document.getElementById("modalMensagem").style.display = "none";
        }

        function validarFormulario() {
            const campos = document.querySelectorAll('[data-obrigatorio="true"]');
            let mensagens = [];
            let primeiroErro = null;

            campos.forEach(campo => {
                campo.classList.remove("erro-campo");

                if (!campo.value.trim()) {
                    mensagens.push(`* ${obterNomeDoCampo(campo.getAttribute('name') || campo.id)} é obrigatório`);
                    campo.classList.add("erro-campo");
                    if (!primeiroErro) primeiroErro = campo;
                }
            });

            if (mensagens.length > 0) {
                MostrarMensagem(mensagens.join("<br>"));
                if (primeiroErro) primeiroErro.focus();
                return false;
            }

            return true;
        }

        function obterNomeDoCampo(name) {
            switch (name) {
                case 'ctl00$ContentPlaceHolder1$txtRamalUsuario': return 'Ramal do Usuário';
                case 'ctl00$ContentPlaceHolder1$txtPatrimonio': return 'Patrimônio';
                case 'ctl00$ContentPlaceHolder1$txtEquipamento': return 'Equipamento';
                case 'ctl00$ContentPlaceHolder1$txtAndar': return 'Andar';
                case 'ctl00$ContentPlaceHolder1$txtLocal': return 'Local';
                case 'ctl00$ContentPlaceHolder1$txtDescricao': return 'Descrição do serviço';
                default: return 'Campo obrigatório';
            }
        }
    </script>

</asp:Content>
