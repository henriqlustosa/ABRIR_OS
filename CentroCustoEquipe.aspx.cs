using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

public class Membro
{
    public string Nome { get; set; }
    public string Login { get; set; }
}

public class GrupoEquipe
{
    public string Centro { get; set; }
    public string Numero { get; set; }
    public string Responsavel { get; set; }
    public List<Membro> Membros { get; set; }
}

public partial class HistoricoOS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // ... suas validações de sessão/perfil ...
        if (!IsPostBack)
        {
            string login = Session["login"] as string;
            CarregaVisualizacao(login);
        }
    }

    private void CarregaVisualizacao(string login)
    {
        List<SolicitanteDados> lista = OsDAO.BuscarEquipeCentroCusto(login) ?? new List<SolicitanteDados>();

        // Normaliza strings para agrupar corretamente
        Func<string, string> N = s => string.IsNullOrEmpty(s) ? "" : s.Trim();

        // Agrupa por trio (Centro, Número, Responsável)
        List<GrupoEquipe> grupos =
            lista.GroupBy(x => new
            {
                Centro = N(x.descricaoCentroCusto),
                Numero = (x.codCentroCusto == null ? "" : x.codCentroCusto.ToString()),
                Responsavel = N(x.nomeResponsavel_Custo)
            })
            .Select(g => new GrupoEquipe
            {
                Centro = g.Key.Centro,
                Numero = g.Key.Numero,
                Responsavel = g.Key.Responsavel,
                Membros = g
                    .Select(m => new Membro
                    {
                        Nome = N(m.nomeSolicitante),
                        Login = N(m.loginSolicitante)
                    })
                    .OrderBy(m => m.Nome)
                    .ToList()
            })
            .OrderBy(g => g.Centro)
            .ThenBy(g => g.Numero)
            .ThenBy(g => g.Responsavel)
            .ToList();

        if (grupos.Count <= 1)
        {
            // ===== VISUALIZAÇÃO SIMPLES (um trio) =====
            pnlGrupos.Visible = false;
            pnlListaSimples.Visible = true;

            gdvCentroCustoEquipe.DataSource = lista;
            gdvCentroCustoEquipe.DataBind();

            // chips no cabeçalho + esconder colunas repetidas
            if (grupos.Count == 1)
            {
                GrupoEquipe g = grupos[0];

                // Centro
                if (!string.IsNullOrEmpty(g.Centro))
                {
                    lblCentro.Text = g.Centro;
                    pnlCentro.Visible = true;
                    gdvCentroCustoEquipe.Columns[2].Visible = false;
                }
                else { pnlCentro.Visible = false; gdvCentroCustoEquipe.Columns[2].Visible = true; }

                // Número
                if (!string.IsNullOrEmpty(g.Numero))
                {
                    lblNumero.Text = g.Numero;
                    pnlNumero.Visible = true;
                    gdvCentroCustoEquipe.Columns[3].Visible = false;
                }
                else { pnlNumero.Visible = false; gdvCentroCustoEquipe.Columns[3].Visible = true; }

                // Responsável
                if (!string.IsNullOrEmpty(g.Responsavel))
                {
                    lblResponsavel.Text = g.Responsavel;
                    pnlResponsavel.Visible = true;
                    gdvCentroCustoEquipe.Columns[4].Visible = false;
                }
                else { pnlResponsavel.Visible = false; gdvCentroCustoEquipe.Columns[4].Visible = true; }
            }
        }
        else
        {
            // ===== VISUALIZAÇÃO AGRUPADA (vários trios) =====
            pnlListaSimples.Visible = false;
            pnlGrupos.Visible = true;

            rptGrupos.DataSource = grupos;
            rptGrupos.DataBind();
        }
    }

    protected void rptGrupos_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        GrupoEquipe grupo = (GrupoEquipe)e.Item.DataItem;
        GridView gv = (GridView)e.Item.FindControl("gvMembros");
        gv.DataSource = grupo.Membros;
        gv.DataBind();
    }
}
