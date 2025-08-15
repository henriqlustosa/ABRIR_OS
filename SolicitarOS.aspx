<%@ Page Title="Solicitação de OS" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SolicitarOS.aspx.cs" Inherits="SolicitarOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/reset.css" rel="stylesheet" />
    <link href="css/SolicitarOS.css?v=2" rel="stylesheet" />
    <link href="css/estilos.css" rel="stylesheet" />
    <link href="css/modal.css" rel="stylesheet" />
    <link href="css/validacaoObrigatoria.css" rel="stylesheet" />
    <style>
        .erro-campo{border:2px solid red!important;background-color:#fff0f0;position:relative}
        .erro-campo::after{content:"*";color:red;position:absolute;top:6px;right:10px;font-weight:bold;font-size:16px}
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Painel captura ENTER e envia para o btnSolicitarOS -->
    <asp:Panel ID="pnlForm" runat="server" DefaultButton="btnSolicitarOS" DefaultFocus="txtPatrimonio">

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
                                <asp:TextBox ID="txtPatrimonio"
             CssClass="input txtPatrimonio"
             runat="server"
             data-obrigatorio="true"
             onkeydown="return handleEnterPatrimonio(event);" />

                                <asp:Button ID="btnPesquisarPatrimonio" CssClass="btnPesquisarPatrimonio"
                                    runat="server" OnClick="btnPesquisarPatrimonio_Click"
                                    CausesValidation="false" />
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
                        <!-- MultiLine: Enter não submete (insere nova linha) -->
                        <asp:TextBox ID="txtDescricao" runat="server" CssClass="txtDescricao"
                            TextMode="MultiLine" data-obrigatorio="true" />
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
                        UseSubmitBehavior="false"
                        OnClientClick="
                            var ok = true;
                            try { ok = (window.validarFormulario ? validarFormulario() : true); } catch(e) { ok = true; }
                            if (!ok) return false;
                            this.disabled = true; this.value = 'Enviando...';
                        "
                        OnClick="btnSolicitarOS_Click" />
                </div>
            </section>
        </main>

    </asp:Panel> <!-- /pnlForm -->

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
        function handleEnterPatrimonio(e) {
            e = e || window.event;
            var key = (typeof e.key !== "undefined") ? e.key : e.keyCode;

            if (key === "Enter" || key === 13) {
                // impede o submit padrão (que iria para o DefaultButton: btnSolicitarOS)
                if (e.preventDefault) e.preventDefault(); else e.returnValue = false;

                // dispara o botão "Pesquisar Patrimônio"
                __doPostBack('<%= btnPesquisarPatrimonio.UniqueID %>', '');

                 return false; // cancela propagação
             }
             return true;
         }
        // Remove erro visual ao digitar
        document.addEventListener('DOMContentLoaded', function () {
            var camposObrigatorios = document.querySelectorAll ?
                document.querySelectorAll('[data-obrigatorio="true"]') :
                (function () { // fallback para navegadores antigos
                    var all = document.getElementsByTagName('*'), list = [];
                    for (var i = 0; i < all.length; i++) {
                        var a = all[i].getAttribute && all[i].getAttribute('data-obrigatorio');
                        if (a === 'true') list.push(all[i]);
                    }
                    return list;
                })();

            for (var i = 0; i < camposObrigatorios.length; i++) {
                (function (campo) {
                    if (campo.addEventListener) {
                        campo.addEventListener('input', function () {
                            if (campo.value.replace(/^\s+|\s+$/g, '') !== '') {
                                campo.className = campo.className.replace(/\berro-campo\b/, '');
                            }
                        }, false);
                    }
                })(camposObrigatorios[i]);
            }
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
                var modal = document.getElementById("modalMensagem");
                if (event.target === modal) fecharModal();
            };
        }

        function fecharModal() {
            document.getElementById("modalMensagem").style.display = "none";
        }

        function validarFormulario() {
            // === validação especial do Centro de Custo (ddlSetor) ===
            var mensagens = [];
            var primeiroErro = null;

            var ddl = document.getElementById('<%= ddlSetor.ClientID %>');
            var PLACEHOLDER_CC = '-- Selecione um Centro de Custo --';

            if (ddl) {
                // texto exibido pode estar “higienizado” (sem código) — usamos o original se existir
                var optSel = ddl.options[ddl.selectedIndex];
                var textoSel = optSel ? (optSel.getAttribute && optSel.getAttribute('data-orig')) || optSel.text : '';
                var ehPlaceholder = (ddl.value === '' || /^\s*--\s*Selecione um Centro de Custo\s*--\s*$/.test(textoSel));

                // limpa estado anterior
                ddl.className = ddl.className.replace(/\berro-campo\b/, '');

                if (ehPlaceholder) {
                    mensagens.push('* Centro de Custo é obrigatório');
                    if (ddl.className.indexOf('erro-campo') === -1) ddl.className += ' erro-campo';
                    if (!primeiroErro) primeiroErro = ddl;
                }
            }

            // === validação dos outros campos obrigatórios via data-obrigatorio ===
            var campos;
            if (document.querySelectorAll) {
                campos = document.querySelectorAll('[data-obrigatorio="true"]');
            } else {
                var all = document.getElementsByTagName('*'); campos = [];
                for (var i = 0; i < all.length; i++) {
                    var a = all[i].getAttribute && all[i].getAttribute('data-obrigatorio');
                    if (a === 'true') campos.push(all[i]);
                }
            }

            for (var j = 0; j < campos.length; j++) {
                var campo = campos[j];
                campo.className = campo.className.replace(/\berro-campo\b/, '');
                if (!campo.value.replace(/^\s+|\s+$/g, '')) {
                    mensagens.push('* ' + obterNomeDoCampo(campo.getAttribute('name') || campo.id) + ' é obrigatório');
                    if (campo.className.indexOf('erro-campo') === -1) campo.className += ' erro-campo';
                    if (!primeiroErro) primeiroErro = campo;
                }
            }

            if (mensagens.length > 0) {
                MostrarMensagem(mensagens.join('<br>'));
                if (primeiroErro && primeiroErro.focus) primeiroErro.focus();
                return false;
            }
            return true;
        }
        (function () {
            var ddl = document.getElementById('<%= ddlSetor.ClientID %>');
            if (!ddl) return;

            function limparErroDDL() {
                ddl.className = ddl.className.replace(/\berro-campo\b/, '');
            }

            if (ddl.addEventListener) {
                ddl.addEventListener('change', limparErroDDL, false);
            } else if (ddl.attachEvent) {
                ddl.attachEvent('onchange', limparErroDDL);
            }
        })();


        function obterNomeDoCampo(name) {
            switch (name) {
                case 'ctl00$ContentPlaceHolder1$txtCentrodeCusto': return 'Centro de Custo';
                case 'ctl00$ContentPlaceHolder1$txtRamalUsuario': return 'Ramal do Usuário';
                case 'ctl00$ContentPlaceHolder1$txtPatrimonio': return 'Patrimônio';
                case 'ctl00$ContentPlaceHolder1$txtEquipamento': return 'Equipamento';
                case 'ctl00$ContentPlaceHolder1$txtAndar': return 'Andar';
                case 'ctl00$ContentPlaceHolder1$txtLocal': return 'Local';
                case 'ctl00$ContentPlaceHolder1$txtDescricao': return 'Descrição do serviço';
                default: return 'Campo obrigatório';
            }
        }
        (function () {
            var ddl = document.getElementById('<%= ddlSetor.ClientID %>');
            if (!ddl) return;

            // Guarda texto original de todas as opções (com código)
            for (var i = 0; i < ddl.options.length; i++) {
                var opt = ddl.options[i];
                if (!opt.getAttribute('data-orig')) {
                    opt.setAttribute('data-orig', opt.text);
                }
            }

            // Remove prefixo "123456 - " só para exibição
            function stripPrefix(txt) {
                if (!txt) return txt;
                return txt.replace(/^\s*\d+\s*-\s*/, '').replace(/^\s+|\s+$/g, '');
            }

            // Restaura o texto original (com código) do item selecionado
            function restoreSelected() {
                var opt = ddl.options[ddl.selectedIndex];
                if (opt) {
                    var orig = opt.getAttribute('data-orig');
                    if (orig) opt.text = orig;
                }
            }

            // Mostra só a descrição no item selecionado
            function sanitizeSelected() {
                var opt = ddl.options[ddl.selectedIndex];
                if (opt) {
                    var orig = opt.getAttribute('data-orig') || opt.text;
                    // não altera o placeholder
                    if (orig === '-- Selecione um Centro de Custo --') return;
                    opt.text = stripPrefix(orig);
                }
            }

            // Antes de abrir a lista (focus/mousedown): restaura para mostrar COM código
            if (ddl.addEventListener) {
                ddl.addEventListener('focus', restoreSelected, false);
                ddl.addEventListener('mousedown', restoreSelected, false);
                ddl.addEventListener('change', sanitizeSelected, false); // após escolher: mostra SEM código
                ddl.addEventListener('blur', sanitizeSelected, false);   // ao fechar: garante SEM código
            } else if (ddl.attachEvent) { // IE antigo
                ddl.attachEvent('onfocus', restoreSelected);
                ddl.attachEvent('onmousedown', restoreSelected);
                ddl.attachEvent('onchange', sanitizeSelected);
                ddl.attachEvent('onblur', sanitizeSelected);
            }

            // Estado inicial: item selecionado aparece SEM código
            sanitizeSelected();
        })();
    </script>

</asp:Content>
