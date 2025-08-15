using System;
using System.Collections.Generic;
using System.Data.SqlClient;          // Para capturar erros específicos de SQL
using System.Globalization;           // Formatação de datas para pt-BR
using System.Text;                    // StringBuilder nas mensagens
using System.Text.RegularExpressions;
using System.Web;                     // HttpUtility.HtmlEncode
using System.Web.UI;                  // Page
using System.Web.UI.WebControls;      // TextBox, DropDownList
using System.Text.RegularExpressions;
/// <summary>
/// Página de solicitação de OS (Ordem de Serviço).
/// Compatível com C# 3 / .NET 3.5.
/// </summary>
public partial class SolicitarOS : Page
{
    #region Constantes, campos e utilitários

    private const string PLACEHOLDER_CC = "-- Selecione um Centro de Custo --";
    private const int REDIRECT_DELAY_MS = 4000;
    private static readonly int[] PERFIS_PERMITIDOS = new int[] { 1, 2, 3 };

    private static bool IsNullOrWhiteSpaceCompat(string value)
    {
        return value == null || value.Trim().Length == 0;
    }
    private static string EscapeJsLiteral(string html)
    {
        return (html ?? string.Empty).Replace("'", "\\'");
    }
    private static string FormataDataPtBr(DateTime data)
    {
        CultureInfo ptBR = new CultureInfo("pt-BR");
        return data.ToString("dd/MM/yyyy HH:mm", ptBR);
    }
    private static string TextoLimpo(TextBox txt)
    {
        return (txt == null || txt.Text == null) ? string.Empty : txt.Text.Trim();
    }
    private static bool UsuarioTemPerfilPermitido(List<int> perfis)
    {
        if (perfis == null || perfis.Count == 0) return false;
        for (int i = 0; i < perfis.Count; i++)
        {
            int p = perfis[i];
            for (int j = 0; j < PERFIS_PERMITIDOS.Length; j++)
                if (p == PERFIS_PERMITIDOS[j]) return true;
        }
        return false;
    }

    #endregion

    #region Ciclo de vida da página

    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigurarCache();

        // Ajuste: se o wrapper não estiver preenchido, tente pegar direto da Session
        if (IsNullOrWhiteSpaceCompat(SessionWrapper.Login))
        {
            string loginSessao = Session["login"] as string;
            if (!IsNullOrWhiteSpaceCompat(loginSessao))
                SessionWrapper.Login = loginSessao;
        }

        if (!ValidarSessaoUsuario()) return;

        if (!IsPostBack)
        {
            // Prepara nome no wrapper, se existir
            if (IsNullOrWhiteSpaceCompat(SessionWrapper.NomeUsuario))
                SessionWrapper.NomeUsuario = Session["nomeUsuario"] as string;

            try
            {
                CarregarDadosUsuario(SessionWrapper.Login);
                CarregarDropDownSetores(SessionWrapper.Login);

                // Fallback: garante Enter -> Solicitar mesmo se o Panel mudar
                if (this.Form != null)
                {
                    this.Form.DefaultButton = btnSolicitarOS.UniqueID;
                    this.Form.DefaultFocus = txtPatrimonio.ClientID;
                }
            }
            catch (Exception ex)
            {
                MostrarMensagem("Erro ao carregar dados: " + HttpUtility.HtmlEncode(ex.Message));
            }
        }

        AtualizarCorDropDownSetor();
    }

    private void ConfigurarCache()
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
        Response.Cache.SetNoStore();
        Response.Cache.SetAllowResponseInBrowserHistory(false);
    }

    private bool ValidarSessaoUsuario()
    {
        if (IsNullOrWhiteSpaceCompat(SessionWrapper.Login))
        {
            // Tentativa final de ler da Session crua
            string raw = Session["login"] as string;
            if (!IsNullOrWhiteSpaceCompat(raw))
            {
                SessionWrapper.Login = raw;
            }
            else
            {
                Response.Redirect("~/login.aspx");
                return false;
            }
        }

        List<int> perfis = SessionWrapper.Perfis;
        if (!UsuarioTemPerfilPermitido(perfis))
        {
            Response.Redirect("~/aberto/SemPermissao.aspx");
            return false;
        }
        return true;
    }

    #endregion

    #region Carregamento de dados

    private void CarregarDadosUsuario(string login)
    {
        List<SolicitanteDados> lista = OsDAO.CarregaDadosUsuarioEResponsavel(login);
        if (lista == null || lista.Count == 0) return;

        SolicitanteDados dados = lista[0];
        txtNomeUsuario.Text = dados.nomeSolicitante;
        txtRfUsuario.Text = dados.rfSolicitante;
        txtRfResponsavel.Text = dados.rfResponsavelCusto;
       
    }

    private void CarregarDropDownSetores(string login)
    {
        List<SolicitanteDados> lista = OsDAO.BuscarCentroDeCustoPorLogin(login); 
        if (lista == null) lista = new List<SolicitanteDados>();

        ddlSetor.DataSource = lista;
        ddlSetor.DataTextField = "descricaoCentroCusto";
        ddlSetor.DataValueField = "codCentroCusto";
        ddlSetor.DataBind();

        if (lista.Count > 1)
        {
            ddlSetor.Items.Insert(0, new ListItem(PLACEHOLDER_CC, ""));
        }
        else if (lista.Count == 1)
        {
            ddlSetor.SelectedIndex = 0;
            txtCentrodeCusto.Text = ddlSetor.SelectedValue;
            PreencherResponsavelCentroCusto(ddlSetor.SelectedValue);
          
        }
    }

    private void AtualizarCorDropDownSetor()
    {
        string cor = "black";
        if (ddlSetor.SelectedItem != null && ddlSetor.SelectedItem.Text == PLACEHOLDER_CC)
            cor = "red";
        ddlSetor.Attributes["style"] = "color: " + cor + ";";
    }

    #endregion

    #region Eventos de UI

    protected void ddlSetor_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtNomeResponsavel.Text = "";
        txtRfResponsavel.Text = "";

        txtCentrodeCusto.Text = ddlSetor.SelectedValue;
        

        try
        {
            PreencherResponsavelCentroCusto(txtCentrodeCusto.Text);
        }
        catch (Exception ex)
        {
            MostrarMensagem("Erro ao carregar responsável do setor: " + HttpUtility.HtmlEncode(ex.Message));
        }
    }

    private void PreencherResponsavelCentroCusto(string codCentroCusto)
    {
        List<SolicitanteDados> lista = OsDAO.BuscarResponsavelPorCentroDeCusto(codCentroCusto);
        if (lista == null || lista.Count == 0) return;

        SolicitanteDados item = lista[0];
        txtNomeResponsavel.Text = item.nomeResponsavel_Custo;
        txtRfResponsavel.Text = item.rfResponsavelCusto;

        // Atenção: aqui é usado codRespCentroCusto como "login" do responsável; ajuste se necessário.
        SessionWrapper.LoginResponsavel = item.codRespCentroCusto.ToString();
    }

    #endregion

    #region Clique: Pesquisar Patrimônio

    protected void btnPesquisarPatrimonio_Click(object sender, EventArgs e)
    {
        txtEquipamento.Text = string.Empty;

        string patrimonioTexto = (txtPatrimonio.Text == null ? string.Empty : txtPatrimonio.Text.Trim());
        if (patrimonioTexto.Length == 0)
        {
            MostrarMensagem("<strong>Aviso:</strong> Informe o número do patrimônio.");
            txtPatrimonio.Focus();
            return;
        }

        int chapa;
        if (!int.TryParse(patrimonioTexto, out chapa) || chapa <= 0)
        {
            MostrarMensagem("<strong>Aviso:</strong> Informe um número de patrimônio válido!");
            txtPatrimonio.Focus();
            return;
        }

        try
        {
            List<SolicitanteDados> lista = OsDAO.BuscarPatrimonio(chapa);

            if (lista == null || lista.Count == 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<strong>Patrimônio Nº ")
                  .Append(HttpUtility.HtmlEncode(chapa.ToString()))
                  .Append("</strong> não encontrado.<br>")
                  .Append("Verifique o número.<br>")
                  .Append("Se não existir cadastro, descreva o equipamento no campo <strong>Equipamento</strong>.");

                MostrarMensagem(sb.ToString());
                txtEquipamento.Focus();
                return;
            }

            string desc = lista[0].equipamentoDesc;
            txtEquipamento.Text = (desc == null ? string.Empty : desc.Trim());
            txtAndar.Focus();
        }
        catch (SqlException)
        {
            MostrarMensagem("Erro de banco de dados ao buscar patrimônio.");
        }
        catch (Exception)
        {
            MostrarMensagem("Erro ao buscar patrimônio.");
        }
    }

    #endregion

    #region Clique: Solicitar OS

    protected void btnSolicitarOS_Click(object sender, EventArgs e)
    {
        // 1) Sessão
        string loginUsuario, loginResponsavel;
        if (!ValidarSessao(out loginUsuario, out loginResponsavel))
            return;

        // 2) Campos obrigatórios
        if (!ValidarCamposObrigatorios())
            return;

        // 2.1) Centro de Custo
        string centroCusto;
        if (!ValidarCentroDeCustoSelecionado(out centroCusto))
            return;

        // 2.2) Patrimônio
        int codPatrimonio;
        if (!TentarObterPatrimonio(out codPatrimonio))
            return;

        try
        {
            // 3) Modelo para persistência
            Solicitacao_Pedido solicitacao = new Solicitacao_Pedido(
                loginUsuario,
                loginResponsavel,
                centroCusto,
                TextoLimpo(txtPatrimonio),
                TextoLimpo(txtAndar),
                TextoLimpo(txtLocal),
                TextoLimpo(txtDescricao),
                TextoLimpo(txtObs),
                TextoLimpo(txtRamalUsuario),
                TextoLimpo(txtRamalResponsavel)
            );

            // 4) Impedir OS duplicada
            List<SolicitanteDados> osAbertas = OsDAO.VerificaOsAbertaPatrimonio(codPatrimonio);
            if (osAbertas != null && osAbertas.Count > 0)
            {
                MostrarMensagem(MontarMensagemOsAberta(osAbertas[0]));
                return;
            }

            // 5) Persistir
            int numeroPedido = OsDAO.GravaSolicitacaoOS(solicitacao);
            if (numeroPedido > 0)
            {
                RedirecionarComAlerta(
                    "Solicitação gravada com sucesso!<br><strong>Número da Solicitação: " +
                    HttpUtility.HtmlEncode(numeroPedido.ToString()) + "</strong>"
                );
            }
            else
            {
                MostrarMensagem("Erro! Solicitação não foi gravada.");
            }
        }
        catch (SqlException)
        {
            MostrarMensagem("Erro de banco de dados ao solicitar OS.");
        }
        catch (Exception)
        {
            MostrarMensagem("Erro ao solicitar OS.");
        }
    }

    #endregion

    #region Validações e mensagens

    private bool ValidarSessao(out string loginUsuario, out string loginResponsavel)
    {
        loginUsuario = SessionWrapper.Login;
        loginResponsavel = SessionWrapper.LoginResponsavel;

        if (IsNullOrWhiteSpaceCompat(loginUsuario))
        {
            RedirecionarComAlerta("Sessão expirada. Faça login novamente.");
            return false;
        }
        if (IsNullOrWhiteSpaceCompat(loginResponsavel))
        {
            RedirecionarComAlerta("Não foi possível identificar o responsável do Centro de Custo.");
            return false;
        }
        return true;
    }

    private bool ValidarCamposObrigatorios()
    {
        string textoSelecionado = (ddlSetor.SelectedItem != null) ? ddlSetor.SelectedItem.Text : "";

        if (textoSelecionado == PLACEHOLDER_CC ||
            txtRamalUsuario.Text.Length < 4 ||
            txtEquipamento.Text.Length < 2 ||
            txtAndar.Text.Length < 1 ||
            txtLocal.Text.Length < 1 ||
            txtDescricao.Text.Length < 2)
        {
            MostrarMensagem("Preencha todos os campos obrigatórios:<br>- Centro de Custo<br>- Equipamento<br>- Descrição<br>- Ramal, Andar e Local");
            return false;
        }
        return true;
    }

    private bool ValidarCentroDeCustoSelecionado(out string centroCusto)
    {
        centroCusto = (txtCentrodeCusto.Text == null ? string.Empty : txtCentrodeCusto.Text.Trim());
        if (IsNullOrWhiteSpaceCompat(centroCusto))
        {
            RedirecionarComAlerta("Centro de Custo inválido. Tente novamente.");
            return false;
        }
        return true;
    }

    private bool TentarObterPatrimonio(out int codPatrimonio)
    {
        codPatrimonio = 0;
        string texto = (txtPatrimonio.Text == null ? string.Empty : txtPatrimonio.Text.Trim());
        if (!int.TryParse(texto, out codPatrimonio))
        {
            MostrarMensagem("Informe um patrimônio numérico válido.");
            return false;
        }
        return true;
    }

    private static string MontarMensagemOsAberta(SolicitanteDados os)
    {
        if (os == null) return "Já existe uma OS aberta para esse Patrimônio.";

        string dataSolicStr = FormataDataPtBr(os.dataSolicitacao);

        StringBuilder sb = new StringBuilder();
        sb.Append("<strong>Já existe uma OS aberta para esse Patrimônio</strong><br>");
        sb.Append("Nº Ordem de Serviço: <strong>")
          .Append(HttpUtility.HtmlEncode(os.idSolicitacao.ToString()))
          .Append("</strong><br>");
        sb.Append("Solicitante: ")
          .Append(HttpUtility.HtmlEncode(os.nomeSolicitante ?? string.Empty))
          .Append("<br>");
        sb.Append("Data Solicitação: ")
          .Append(HttpUtility.HtmlEncode(dataSolicStr))
          .Append("<br>");
        sb.Append("Status: ")
          .Append(HttpUtility.HtmlEncode(os.statusSolicitacao ?? string.Empty));

        return sb.ToString();
    }

    private void MostrarMensagem(string msgHtml)
    {
        string script = "MostrarMensagem('" + EscapeJsLiteral(msgHtml) + "');";
        ScriptManager.RegisterStartupScript(this, GetType(), "msgModal", script, true);
    }

    private void RedirecionarComAlerta(string msgHtml)
    {
        string script = "MostrarMensagem('" + EscapeJsLiteral(msgHtml) + "');" +
                        "setTimeout(function(){ window.location.href='SolicitarOS.aspx'; }, " + REDIRECT_DELAY_MS + ");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", script, true);
    }

    #endregion
}
