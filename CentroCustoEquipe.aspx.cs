using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
    public string Diretoria { get; set; }
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

    // Fix for CS0117: 'UnicodeCategory' does not contain a definition for 'NonspacingMark'
    // The correct enum value is 'UnicodeCategory.NonSpacingMark' (note the capital 'S')



private static string RemoveAcentos(string texto)
{
    if (string.IsNullOrEmpty(texto))
        return texto;

    string norm = texto.Normalize(NormalizationForm.FormD);
    var sb = new StringBuilder();

    for (int i = 0; i < norm.Length; i++)
    {
        UnicodeCategory cat = CharUnicodeInfo.GetUnicodeCategory(norm[i]);
        if (cat != UnicodeCategory.NonSpacingMark) // nome correto da enum
        {
            sb.Append(norm[i]);
        }
    }

    return sb.ToString().Normalize(NormalizationForm.FormC);
}


private void CarregaVisualizacao(string login)
    {
        List<SolicitanteDados> lista = OsDAO.BuscarEquipeCentroCusto(login) ?? new List<SolicitanteDados>();

        // Normalizador compatível com C# 3 / .NET 3.5
        Func<string, string> N = delegate (string s) { return string.IsNullOrEmpty(s) ? "" : s.Trim(); };

        // Agrupa por (Centro, Número, Responsável, Diretoria)
        List<GrupoEquipe> grupos =
            lista.GroupBy(x => new
            {
                Centro = N(x.descricaoCentroCusto),
                Numero = (x.codCentroCusto == null ? "" : x.codCentroCusto.ToString()),
                Responsavel = N(x.nomeResponsavel_Custo),
                Diretoria = N(x.diretoria) // ajuste se sua propriedade tiver outro nome (ex: x.Diretorias)
            })
            .Select(g => new GrupoEquipe
            {
                Centro = g.Key.Centro,
                Numero = g.Key.Numero,
                Responsavel = g.Key.Responsavel,
                Diretoria = g.Key.Diretoria,
                Membros = g.Select(m => new Membro
                {
                    Nome = N(m.nomeSolicitante),
                    Login = N(m.loginSolicitante)
                })
                .OrderBy(m => RemoveAcentos(m.Nome), StringComparer.CurrentCultureIgnoreCase)
                .ToList()
            })
            .OrderBy(g => RemoveAcentos(g.Centro), StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(g => g.Numero)
            .ThenBy(g => RemoveAcentos(g.Responsavel), StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(g => RemoveAcentos(g.Diretoria), StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        if (grupos.Count <= 1)
        {
            // ===== VISUALIZAÇÃO SIMPLES =====
            pnlGrupos.Visible = false;
            pnlListaSimples.Visible = true;

            // Ordena a grade simples por Nome (e empata por Login), ignorando acentos e case
            var listaOrdenada = lista
                .OrderBy(x => RemoveAcentos(N(x.nomeSolicitante)), StringComparer.CurrentCultureIgnoreCase)
                .ThenBy(x => RemoveAcentos(N(x.loginSolicitante)), StringComparer.CurrentCultureIgnoreCase)
                .ToList();

            gdvCentroCustoEquipe.DataSource = listaOrdenada;
            gdvCentroCustoEquipe.DataBind();

            if (grupos.Count == 1)
            {
                GrupoEquipe g = grupos[0];

                // Centro
                if (!string.IsNullOrEmpty(g.Centro))
                {
                    lblCentro.Text = g.Centro;
                    pnlCentro.Visible = true;
                    OcultarColunaSeExistir(gdvCentroCustoEquipe, "Centro de Custo");
                }
                else
                {
                    pnlCentro.Visible = false;
                    MostrarColunaSeExistir(gdvCentroCustoEquipe, "Centro de Custo");
                }

                // Número
                if (!string.IsNullOrEmpty(g.Numero))
                {
                    lblNumero.Text = g.Numero;
                    pnlNumero.Visible = true;
                    OcultarColunaSeExistir(gdvCentroCustoEquipe, "Número");
                }
                else
                {
                    pnlNumero.Visible = false;
                    MostrarColunaSeExistir(gdvCentroCustoEquipe, "Número");
                }

                // Responsável
                if (!string.IsNullOrEmpty(g.Responsavel))
                {
                    lblResponsavel.Text = g.Responsavel;
                    pnlResponsavel.Visible = true;
                    OcultarColunaSeExistir(gdvCentroCustoEquipe, "Responsável");
                }
                else
                {
                    pnlResponsavel.Visible = false;
                    MostrarColunaSeExistir(gdvCentroCustoEquipe, "Responsável");
                }

                // Diretoria
                if (!string.IsNullOrEmpty(g.Diretoria))
                {
                    lblDiretoria.Text = g.Diretoria;
                    pnlDiretoria.Visible = true;
                    OcultarColunaSeExistir(gdvCentroCustoEquipe, "Diretoria");
                }
                else
                {
                    pnlDiretoria.Visible = false;
                    MostrarColunaSeExistir(gdvCentroCustoEquipe, "Diretoria");
                }
            }
        }
        else
        {
            // ===== VISUALIZAÇÃO AGRUPADA =====
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

    // ===== Helpers para (des)ocultar colunas por HeaderText =====
    private static void OcultarColunaSeExistir(GridView grid, string headerText)
    {
        int idx = ObterIndiceColuna(grid, headerText);
        if (idx >= 0) grid.Columns[idx].Visible = false;
    }

    private static void MostrarColunaSeExistir(GridView grid, string headerText)
    {
        int idx = ObterIndiceColuna(grid, headerText);
        if (idx >= 0) grid.Columns[idx].Visible = true;
    }

    private static int ObterIndiceColuna(GridView grid, string headerText)
    {
        if (grid == null || grid.Columns == null) return -1;
        for (int i = 0; i < grid.Columns.Count; i++)
        {
            DataControlField col = grid.Columns[i] as DataControlField;
            if (col != null && string.Equals(col.HeaderText, headerText, StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return -1;
    }
}
