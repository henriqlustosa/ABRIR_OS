using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SolicitarOS : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigurarCache();

        if (!ValidarSessaoUsuario()) return;

        if (!IsPostBack)
        {
            SessionWrapper.Login = Session["login"] as string;
            SessionWrapper.NomeUsuario = Session["nomeUsuario"] as string;

            try
            {
                CarregarDadosUsuario(SessionWrapper.Login);
                CarregarDropDownSetores(SessionWrapper.Login);
            }
            catch (Exception ex)
            {
                MostrarMensagem("Erro ao carregar dados: " + ex.Message);
            }
        }

        AjustarCorDropDownSetor();
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
        if (string.IsNullOrEmpty(SessionWrapper.Login))
        {
            Response.Redirect("~/login.aspx");
            return false;
        }

        List<int> perfis = SessionWrapper.Perfis;
        if (perfis == null || !(perfis.Contains(1) || perfis.Contains(2) || perfis.Contains(3)))
        {
            Response.Redirect("~/aberto/SemPermissao.aspx");
            return false;
        }

        return true;
    }

    private void CarregarDadosUsuario(string login)
    {
        List<SolicitanteDados> lista = OsDAO.CarregaDadosUsuarioEResponsavel(login);
        if (lista.Count > 0)
        {
            SolicitanteDados dados = lista[0];
            txtNomeUsuario.Text = dados.nomeSolicitante;
            txtRfUsuario.Text = dados.rfSolicitante;
            txtRfResponsavel.Text = dados.rfResponsavelCusto;
           txtNomeResponsavel.Text = dados.nomeResponsavel_Custo;
        }
    }


    private void CarregarDropDownSetores(string login)
    {
        List<SolicitanteDados> lista = OsDAO.BuscarCentroDeCustoPorLogin(login);

        ddlSetor.DataSource = lista;
        ddlSetor.DataTextField = "descricaoCentroCusto";
        ddlSetor.DataValueField = "codCentroCusto";
        ddlSetor.DataBind();

        if (lista.Count > 1)
        {
            ddlSetor.Items.Insert(0, new ListItem("-- Selecione um Centro de Custo --", ""));
        }
        else if (lista.Count == 1)
        {
            ddlSetor.SelectedIndex = 0;
            PreencherResponsavelCentroCusto(ddlSetor.SelectedValue.ToString());
            txtCentrodeCusto.Text = ddlSetor.SelectedValue.ToString();
        }
    }

    private void AjustarCorDropDownSetor()
    {
        string cor = "black";
        if (ddlSetor.SelectedItem != null && ddlSetor.SelectedItem.Text == "-- Selecione um Centro de Custo --")
        {
            cor = "red";
        }
        ddlSetor.Attributes["style"] = "color: " + cor + ";";
    }

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
            MostrarMensagem("Erro ao carregar responsável do setor: " + ex.Message);
        }
    }

    private void PreencherResponsavelCentroCusto(string codCentroCusto)
    {
        List<SolicitanteDados> lista = OsDAO.BuscarResponsavelPorCentroDeCusto(codCentroCusto);
        if (lista.Count > 0)
        {
            SolicitanteDados item = lista[0];
            txtNomeResponsavel.Text = item.nomeResponsavel_Custo;
            txtRfResponsavel.Text = item.rfResponsavelCusto;
        
            SessionWrapper.LoginResponsavel = item.codRespCentroCusto.ToString();
        }
    }

    protected void btnPesquisarPatrimonio_Click(object sender, EventArgs e)
    {
        txtEquipamento.Text = "";

        int chapa;
        if (!int.TryParse(txtPatrimonio.Text.Trim(), out chapa))
        {
            MostrarMensagem("<strong>Aviso:</strong> Informe um número de patrimônio válido!");
            return;
        }

        try
        {
            List<SolicitanteDados> lista = OsDAO.BuscarPatrimonio(chapa);
            if (lista.Count > 0)
            {
                txtEquipamento.Text = lista[0].equipamentoDesc;
            }
            else
            {
                MostrarMensagem("<strong>Patrimônio Nº " + chapa + "</strong> não encontrado.<br>Verifique o número.<br>Se não existir cadastro, descreva o equipamento no campo <strong>Equipamento</strong>.");
            }
        }
        catch (Exception ex)
        {
            MostrarMensagem("Erro ao buscar patrimônio: " + ex.Message);
        }
    }

    protected void btnSolicitarOS_Click(object sender, EventArgs e)
    {

        string loginUsuario = SessionWrapper.Login;
     string loginUsuarioResposavel = SessionWrapper.LoginResponsavel;
     string codRespCentroCusto = txtCentrodeCusto.Text;
     int    codPatrimonio =  Convert.ToInt32(txtPatrimonio.Text);
        if (!ValidarCamposObrigatorios()) return;

        try
        {
            var solicitacao = new Solicitacao_Pedido(
  loginUsuario,
    loginUsuarioResposavel,
    codRespCentroCusto,
    txtPatrimonio.Text,
    txtAndar.Text,
    txtLocal.Text,
    txtDescricao.Text,
    txtObs.Text,
    txtRamalUsuario.Text,
    txtRamalResponsavel.Text
);


            if (string.IsNullOrEmpty(codRespCentroCusto)
     || codRespCentroCusto.Trim().Length == 0) { 
                RedirecionarComAlerta("Algo deu errado. Tente novamente.");
                return;
            }

            List<SolicitanteDados> osAbertas = OsDAO.VerificaOsAbertaPatrimonio(codPatrimonio);
            if (osAbertas.Count > 0)
            {
                SolicitanteDados os = osAbertas[0];
                string mensagem = "<strong>Já existe uma OS aberta para esse Patrimônio</strong><br>" +
                                  "Nº Ordem de Serviço: <strong>" + os.idSolicitacao + "</strong><br>" +
                                  "Solicitante: " + os.nomeSolicitante + "<br>" +
                                  "Data Solicitação: " + os.dataSolicitacao + "<br>" +
                                  "Status: " + os.statusSolicitacao;
                MostrarMensagem(mensagem);
                return;
            }

            int nPedido = OsDAO.GravaSolicitacaoOS(solicitacao);
            if (nPedido > 0)
            {
                RedirecionarComAlerta("Solicitação gravada com sucesso!<br><strong>Número da Solicitação: " + nPedido + "</strong>");
            }
            else
            {
                MostrarMensagem("Erro! Solicitação não foi gravada.");
            }
        }
        catch (Exception ex)
        {
            MostrarMensagem("Erro ao solicitar OS: " + ex.Message);
        }
    }

    private bool ValidarCamposObrigatorios()
    {
        string textoSelecionado = ddlSetor.SelectedItem != null ? ddlSetor.SelectedItem.Text : "";

        if (textoSelecionado == "-- Selecione um Centro de Custo --" ||
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

   

    private void MostrarMensagem(string msgHtml)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msgModal", "MostrarMensagem('" + msgHtml.Replace("'", "\\'") + "');", true);
    }

    private void RedirecionarComAlerta(string msgHtml)
    {
        string script = "MostrarMensagem('" + msgHtml.Replace("'", "\\'") + "'); setTimeout(function(){ window.location.href='SolicitarOS.aspx'; }, 4000);";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", script, true);
    }
}
