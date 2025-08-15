using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public class Solicitacao_Pedido
{
    // Propriedades da tabela
    public int Id_Solicitacao { get; set; }
    public int UsuarioSolicitanteId { get; set; }
    public int CentroCustoId { get; set; }
    public int UsuarioRespCCId { get; set; }
    public string RamalSolicitante { get; set; }
    public string RamalRespCusto { get; set; }
    public string Patrimonio { get; set; }
    public string Andar { get; set; }
    public string Local { get; set; }
    public string DescricaoServico { get; set; }
    public string Obs { get; set; }
    public DateTime? DataSolicitacao { get; set; }
    public int Status { get; set; }
    // Change the property type of DataFinalizacao from DateTime to DateTime?
    public DateTime? DataFinalizacao { get; set; }
    // Recupera a string de conexão do arquivo Web.config
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString();

    // Construtor que já busca IDs e preenche campos
    public Solicitacao_Pedido(
        string loginSolicitante,
        string loginResponsavel,
        string codCentroCusto,
        string patrimonio,
        string andar,
        string local,
        string descricaoServico,
        string obs,
        string ramalSolicitante,
        string ramalRespCusto)
    {
        Id_Solicitacao = 0; // Novo registro

        // Buscar IDs no banco
        CentroCustoId = ObterIdCentroCustoPorCodigo(codCentroCusto);
        UsuarioRespCCId = ObterIdUsuarioPorLogin(loginResponsavel);
        UsuarioSolicitanteId = ObterIdUsuarioPorLogin(loginSolicitante);

        // Demais campos
        Patrimonio = patrimonio;
        Andar = andar;
        Local = local;
        DescricaoServico = descricaoServico;
        Obs = obs;
        RamalSolicitante = ramalSolicitante;
        RamalRespCusto = ramalRespCusto;
        DataSolicitacao = DateTime.Now;
        // In the constructor, this assignment is now valid:
        DataFinalizacao = null;
        Status = 1;
    }

    private int ObterIdUsuarioPorLogin(string login)
    {
        if (string.IsNullOrEmpty(login) || login.Trim().Length == 0)
            return 0;

      

        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new SqlCommand(
                "SELECT UsuarioId FROM dbo.Usuarios WHERE LoginRede = @login", conn))
            {
                cmd.Parameters.Add("@login", SqlDbType.VarChar).Value = login.Trim().ToUpper();

                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
    }

    private int ObterIdCentroCustoPorCodigo(string codigoCentro)
    {
        if (string.IsNullOrEmpty(codigoCentro) || codigoCentro.Trim().Length == 0)
            return 0;

     

        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new SqlCommand(
                "SELECT IdCentroDeCusto FROM dbo.CentroDeCusto WHERE codigoCentroDeCusto = @codigo", conn))
            {
                cmd.Parameters.Add("@codigo", SqlDbType.VarChar).Value = codigoCentro.Trim();

                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
    }
}
