using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HistoricoOS : System.Web.UI.Page
{
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
        string nome = Session["nomeUsuario"] as string;
        string login = Session["login"] as string;
        if (!this.IsPostBack)
        {
            carregaEquipeCentroCusto(login);
        }
    }


    private void carregaEquipeCentroCusto(string login)
    {
        var lista = new List<SolicitanteDados>();
        lista = OsDAO.BuscarEquipeCentroCusto(login);
        gdvCentroCustoEquipe.DataSource = lista;
        gdvCentroCustoEquipe.DataBind();
    }
    //protected void gdvHistorico_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    int idSolicitacao = Convert.ToInt32(gdvCentroCustoEquipe.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString());

    //    //  Response.Redirect("~/CadastrarAltaPaciente/CadastraAlta.aspx?nrSeq=" + nrSeq);
    //   // Response.Redirect("~/administrativo/RegistrarLigacao.aspx?nrConulta=" + NrConsulta);
    //}

}