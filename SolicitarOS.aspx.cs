using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SolitarOS : System.Web.UI.Page
{
    public static class VariaveisGlobais
    {
        public static string login { get; set; }
        public static string nomeUsuario { get; set; }
        public static int codRespCusto { get; set; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
        Response.Cache.SetNoStore();
        Response.Cache.SetAllowResponseInBrowserHistory(false);


        // 1. Verifica se o usuário está logado (existe sessão)
        if (Session["login"] == null)
        {
            Response.Redirect("~/login.aspx"); // Redireciona se não estiver logado
            return;
        }
        // 2. Verifica se o perfil é diferente de "1" (Administrador)
        List<int> perfis = Session["perfis"] as List<int>;
        if (perfis == null || ((!perfis.Contains(1)) && (!perfis.Contains(2)) && (!perfis.Contains(3))))
        {
            Response.Redirect("~/aberto/SemPermissao.aspx");
        }
        VariaveisGlobais.nomeUsuario = Session["nomeUsuario"] as string;
        VariaveisGlobais.login = Session["login"] as string;
        if (!this.IsPostBack)
        {
            carregaDadosUsuario(VariaveisGlobais.login);
            CarregarDropDownSetores(VariaveisGlobais.login);
            //CarregarDropDownSetoresSolicitados();
        }
        if (ddlSetor.SelectedItem.Text == "-- Selecione um Centro de Custo --")
        {
            ddlSetor.Attributes.Add("style", "color : red;");
        }
        else
        {
            ddlSetor.Attributes.Add("style", "color : black;");
        }
     
    }

    private void carregaDadosUsuario(string login)
    {
        List<SolicitanteDados> lista = OsDAO.carregaDadosUsuRespCentroDeCusto(login);

        foreach (var i in lista)
        {
            txtNomeUsuario.Text = i.nomeSolicitante;
            txtRfUsuario.Text = i.rfSolicitante;
        }
    }

    private void CarregarDropDownSetores(string login)
    {
        List<SolicitanteDados> lista = OsDAO.BuscarCentroDeCustoPorLogin(login);

        ddlSetor.DataSource = lista;
        ddlSetor.DataTextField = "descricaoCentroCusto";   // Vai aparecer no dropdown
        ddlSetor.DataValueField = "codCentroCusto";     // Vai ser o valor interno
        ddlSetor.DataBind();
        if (lista.Count > 1)
        {
            ddlSetor.Items.Insert(0, new ListItem("-- Selecione um Centro de Custo --", ""));
            //ddlSetor.Items[0].Attributes.CssStyle.Add("color", "red");
        }
        else
        {
            txtCentrodeCusto.Text = ddlSetor.SelectedValue;
            List<SolicitanteDados> lista2 = OsDAO.BuscarCentroDeCustoPorCentroDeCusto(txtCentrodeCusto.Text);
            foreach (var i in lista2)
            {
                txtNomeResponsavel.Text = i.nomeResponsavel_Custo;
                txtRfResponsavel.Text = i.rfResponsavelCusto;
                VariaveisGlobais.codRespCusto = i.codRespCentroCusto;
            }
        }
    }
    protected void ddlSetor_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtNomeResponsavel.Text = "";
        txtRfResponsavel.Text = "";
        txtCentrodeCusto.Text = ddlSetor.SelectedValue;
        List<SolicitanteDados> lista = OsDAO.BuscarCentroDeCustoPorCentroDeCusto(txtCentrodeCusto.Text);
        foreach (var i in lista)
        {
            txtNomeResponsavel.Text = i.nomeResponsavel_Custo;
            txtRfResponsavel.Text = i.rfResponsavelCusto;
            VariaveisGlobais.codRespCusto = i.codRespCentroCusto;
        }
    }

    protected void btnPesquisarPatrimonio_Click(object sender, EventArgs e)
    {
        txtEquipamento.Text = "";
        if (txtPatrimonio.Text.Length < 1)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "mensagem", "alert('Informe um numero de patrimonio valido!');", true);
            return;
        }
        int chapa = Convert.ToInt32(txtPatrimonio.Text);
        List<SolicitanteDados> lista = OsDAO.BuscarPatrimonio(chapa);
        foreach (var i in lista)
        {
            // txt.Text = i.nomeResponsavel_Custo;
            txtEquipamento.Text = i.equipamentoDesc;
        }
        if (lista.Count == 0)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "mensagem",
      "alert('Patrimônio Nº " + chapa +
      " não encontrado.\\nVerifique o número.\\nSe não existir cadastro, descreva o equipamento no campo Equipamento:.');",
      true);

        }
    }

    //private void CarregarDropDownSetoresSolicitados()
    //{
    //    List<SolicitanteDados> lista = OsDAO.BuscarSetoresSolicitados();

    //    ddlSetorSolicitado.DataSource = lista;
    //    ddlSetorSolicitado.DataTextField = "setorSolicitadoDesc";   // Vai aparecer no dropdown
    //    ddlSetorSolicitado.DataValueField = "codSetorSolicitado";     // Vai ser o valor interno
    //    ddlSetorSolicitado.DataBind();
    //    if (lista.Count > 1)
    //    {
    //        ddlSetorSolicitado.Items.Insert(0, new ListItem("-- Selecione um setor --", ""));
    //        //ddlSetor.Items[0].Attributes.CssStyle.Add("color", "red");
    //    }

    //}

    //protected void ddlSetorSolicitado_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlSetorSolicitado.SelectedItem.Text == "-- Selecione um setor --")
    //    {
    //        ddlSetorSolicitado.Attributes.Add("style", "color : red;");
    //    }
    //    else
    //    {
    //        ddlSetorSolicitado.Attributes.Add("style", "color : black;");
    //    }
    //}

    protected void btnSolicitarOS_Click(object sender, EventArgs e)
    {
        if (/*ddlSetorSolicitado.SelectedItem.Text == "-- Selecione um setor --" ||*/ ddlSetor.SelectedItem.Text == "-- Selecione um Centro de Custo --"
         || txtRamalUsuario.Text.Length < 4 || txtEquipamento.Text.Length < 2 || txtAndar.Text.Length < 1 || txtLocal.Text.Length < 1 || txtDescricao.Text.Length < 2)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "mensagem", "alert('Aviso! \\nObrigatório Centro de Custo" +
                "\\nObrigatório Setor que realizará o serviço \\nObrigatório informar equipamento\\nObrigatório Descrição do serviço", true);
            return;
        }


        SolicitanteDados s = new SolicitanteDados();
        s.codCentroCusto = Convert.ToInt32(txtCentrodeCusto.Text);
        s.codRespCentroCusto = VariaveisGlobais.codRespCusto;
        if (s.codRespCentroCusto < 1)
        {
            string answer = "Algo deu Errado, Tente novamente";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
                        "alert('" + answer + "'); window.location.href='SolicitarOS.aspx';", true);
            return;
        }
        s.loginSolicitante = VariaveisGlobais.login;
        s.rfSolicitante = txtRfUsuario.Text;
        s.codPatrimonio = Convert.ToInt32(txtPatrimonio.Text);
        //s.codSetorSolicitado = Convert.ToInt32(ddlSetorSolicitado.SelectedValue);
        s.andar = txtAndar.Text;
        s.localDaSolicitacao = txtLocal.Text;
        s.descServicoSolicitado = txtDescricao.Text;
        s.obs = txtObs.Text;
        s.ramalSolicitante = txtRamalUsuario.Text;
        s.ramalRespSetor = txtRamalResponsavel.Text;
        List<SolicitanteDados> l = OsDAO.VerificaOsAbertaPatrimonio(s.codPatrimonio);// Verificar se tem OS aberta para esse patrimonio
        foreach (var i in l)
        {
            if (l.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "mensagem", "alert('Já existem uma OS aberta para esse Patrimônio\\n" +
                    "Nº Ordem de Serviço: " + i.idSolicitacao+ "\\n" +
                    "Solicitante: "+i.nomeSolicitante+"\\n" +
                    "Data Solicitação: "+i.dataSolicitacao+"\\n" +
                    "Status: "+i.statusSolicitacao+"');", true);
                return;
            }

        }
       
        int nPedido = OsDAO.GravaSolicitacaoOS(s);
        if (nPedido > 0)
        {
            //  ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "mensagem", "alert('Solicitação Gravada com suecesso! \\n Numero da sua Solicitação é "+nPedido+"');", true);
            string answer = "Solicitação Gravada com suecesso! \\n Numero da sua Solicitação é " + nPedido + "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
                        "alert('" + answer + "'); window.location.href='SolicitarOS.aspx';", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "mensagem", "alert('Erro! \\n solicitação não foi gravada!');", true);
        }

    }


}