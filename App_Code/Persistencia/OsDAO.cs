using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Classe responsável por todas as operações de acesso a dados
/// relacionadas a solicitações de Ordem de Serviço (OS).
/// </summary>
public class OsDAO
{
    // Recupera a string de conexão do arquivo Web.config
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString();

    /// <summary>
    /// Retorna os dados do solicitante e do responsável pelo centro de custo com base no login de rede.
    /// </summary>
    public static List<SolicitanteDados> CarregaDadosUsuarioEResponsavel(string login)
    {
        var lista = new List<SolicitanteDados>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"
                SELECT [nomeUsuario],[loginRedeUsuario],[rfResponsavel],[nomeResponsavel],[descricao],[codigoCentroDeCusto],[rfUsuario]
                FROM Vw_DadosUsuario_DadosRespCusto
                WHERE loginRedeUsuario = @loginRede";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@loginRede", login.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var s = new SolicitanteDados();
                s.nomeSolicitante = dr["nomeUsuario"] as string;
                s.loginSolicitante = dr["loginRedeUsuario"] as string;
                s.rfSolicitante = dr["rfUsuario"] as string;
                s.nomeResponsavel_Custo = dr["nomeResponsavel"] as string;
                s.rfResponsavelCusto = dr["rfResponsavel"] as string;
                s.descricaoCentroCusto = dr["descricao"] as string;
                s.codCentroCusto = Convert.ToInt32(dr["codigoCentroDeCusto"]);

                s.rfUsuario = Convert.ToInt32(dr["rfUsuario"]);
                lista.Add(s);
            }
        }

        return lista;
    }

    /// <summary>
    /// Associa um centro de custo a um usuário.
    /// </summary>
    public static void GravaCentroDeCustoDoFuncionario(string login, int codCentroDeCusto)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"
                INSERT INTO Usuario_CentroDeCusto (loginRede, id_centroDeCusto)
                VALUES (@loginRede, @id_centroDeCusto)";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@loginRede", login);
            cmd.Parameters.AddWithValue("@id_centroDeCusto", codCentroDeCusto);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// Retorna os centros de custo vinculados ao login do usuário.
    /// </summary>
    public static List<SolicitanteDados> BuscarCentroDeCustoPorLogin(string login)
    {
        var lista = new List<SolicitanteDados>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"
                SELECT  [codigoCentroDeCusto],[descricaoCentroDeCusto]
                FROM Vw_Usuario_CentroDeCusto
                WHERE LoginDeRedeUsuario = @loginRede";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@loginRede", login);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var s = new SolicitanteDados();
                s.codCentroCusto = Convert.ToInt32(reader["codigoCentroDeCusto"]);
                s.descricaoCentroCusto = reader["codigoCentroDeCusto"] + " - " + reader["descricaoCentroDeCusto"].ToString();
               
                lista.Add(s);
            }
        }

        return lista;
    }

    /// <summary>
    /// Retorna os dados do responsável por um centro de custo específico.
    /// </summary>
    public static List<SolicitanteDados> BuscarResponsavelPorCentroDeCusto(string centroCusto)
    {
        var lista = new List<SolicitanteDados>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"
                SELECT [nomeResponsavel], [rfResponsavel], loginRedeResponsavel
                FROM Vw_DadosUsuario_DadosRespCusto
                WHERE [codigoCentroDeCusto] = @Codigo_centroCusto";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Codigo_centroCusto", centroCusto);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var s = new SolicitanteDados();
                s.nomeResponsavel_Custo = reader["nomeResponsavel"].ToString();
                s.rfResponsavelCusto = reader["rfResponsavel"].ToString();
                s.codRespCentroCusto = reader["loginRedeResponsavel"].ToString();
                lista.Add(s);
            }
        }

        return lista;
    }

    /// <summary>
    /// Retorna a descrição de um patrimônio com base no número.
    /// </summary>
    public static List<SolicitanteDados> BuscarPatrimonio(int codPatrimonio)
    {
        var lista = new List<SolicitanteDados>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"
                SELECT BEM_A_DESC
                FROM NumeroPatrimonio
                WHERE BEM_A_CHAP = @BEM_A_CHAP";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@BEM_A_CHAP", codPatrimonio);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var s = new SolicitanteDados();
                s.equipamentoDesc = reader["BEM_A_DESC"].ToString();
                lista.Add(s);
            }
        }

        return lista;
    }

    /// <summary>
    /// Grava uma nova solicitação de OS no banco e retorna o ID gerado.
    /// </summary>



    public static int GravaSolicitacaoOS(Solicitacao_Pedido s)
    {
        if (s == null) throw new ArgumentNullException("s");

        int novoId;

        // Use o mesmo nome de chave do field estático da classe
        string cs = ConfigurationManager
            .ConnectionStrings["hspm_OSConnectionString"]
            .ConnectionString;

        const string sql = @"
INSERT INTO dbo.Solicitacao
(
    UsuarioSolicitanteId,
    CentroCustoId,
    UsuarioRespCCId,
    ramalSolicitante,
    ramalRespCusto,
    patrimonio,
    andar,
    [local],
    descricaoServico,
    obs,
    dataSolicitacao,
    [status]

)
VALUES
(
    @UsuarioSolicitanteId,
    @CentroCustoId,
    @UsuarioRespCCId,
    @ramalSolicitante,
    @ramalRespCusto,
    @patrimonio,
    @andar,
    @local,
    @descricaoServico,
    @obs,
    @dataSolicitacao,
    @status
   
);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using (var con = new SqlConnection(cs))
        using (var cmd = new SqlCommand(sql, con))
        {
            // IDs obrigatórios
            cmd.Parameters.Add("@UsuarioSolicitanteId", SqlDbType.Int).Value = s.UsuarioSolicitanteId;
            cmd.Parameters.Add("@CentroCustoId", SqlDbType.Int).Value = s.CentroCustoId;
            cmd.Parameters.Add("@UsuarioRespCCId", SqlDbType.Int).Value = s.UsuarioRespCCId;

            // Strings curtas (null -> DBNull)
            cmd.Parameters.Add("@ramalSolicitante", SqlDbType.VarChar, 10)
               .Value = (object)(s.RamalSolicitante ?? (string)null) ?? DBNull.Value;

            cmd.Parameters.Add("@ramalRespCusto", SqlDbType.VarChar, 10)
               .Value = (object)(s.RamalRespCusto ?? (string)null) ?? DBNull.Value;

            // Patrimônio (tenta converter)
            object patrimonio = DBNull.Value;
            if (!IsNullOrWhiteSpace(s.Patrimonio))
            {
                int p;
                if (int.TryParse(s.Patrimonio, out p))
                    patrimonio = p;
            }
            cmd.Parameters.Add("@patrimonio", SqlDbType.Int).Value = patrimonio;

            // Demais textos
            cmd.Parameters.Add("@andar", SqlDbType.VarChar, 20).Value = (object)(s.Andar ?? (string)null) ?? DBNull.Value;
            cmd.Parameters.Add("@local", SqlDbType.VarChar, 100).Value = (object)(s.Local ?? (string)null) ?? DBNull.Value;
            cmd.Parameters.Add("@descricaoServico", SqlDbType.VarChar, 1000).Value = (object)(s.DescricaoServico ?? (string)null) ?? DBNull.Value;
            cmd.Parameters.Add("@obs", SqlDbType.VarChar, 1000).Value = (object)(s.Obs ?? (string)null) ?? DBNull.Value;

            // Datas
            cmd.Parameters.Add("@dataSolicitacao", SqlDbType.DateTime2)
               .Value = (object)(s.DataSolicitacao != default(DateTime) ? s.DataSolicitacao : DateTime.Now);



            cmd.Parameters.Add("@status", SqlDbType.Int).Value = s.Status;

        
            con.Open();
            novoId = (int)cmd.ExecuteScalar();
        }

        return novoId;
    }


    // ==== Helpers compatíveis com C# 3.0 (.NET 3.5) ====

    private static object TryParseIntOrDbNull(string valor)
    {
        if (valor == null) return DBNull.Value;

        int n;
        if (int.TryParse(valor, out n))
            return n;

        return DBNull.Value;
    }

    private static int ParseStatusOrDefault(string status, int defaultValue)
    {
        if (status == null) return defaultValue;

        int n;
        if (int.TryParse(status, out n))
            return n;

        return defaultValue;
    }



    /// <summary>
    /// Verifica se há alguma OS aberta para o patrimônio informado.
    /// </summary>
    public static List<SolicitanteDados> VerificaOsAbertaPatrimonio(int patrimonio)
    {
        var lista = new List<SolicitanteDados>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"
                SELECT NomeCompleto, id_solicitacao, dataSolicitacao, patrimonio, local, descricaoServico, status
                FROM Vw_VericaOsAbertaPatrimonio
                WHERE (status <> 4 AND status <> 5)
                  AND patrimonio = @patrimonio
                  AND patrimonio != 46879 AND patrimonio != 1";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@patrimonio", patrimonio);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var s = new SolicitanteDados();
                s.nomeSolicitante = reader["NomeCompleto"].ToString();
                s.idSolicitacao = Convert.ToInt32(reader["id_solicitacao"]);
                s.localDaSolicitacao = reader["local"].ToString();
                s.descServicoSolicitado = reader["descricaoServico"].ToString();
                s.dataSolicitacao = reader["dataSolicitacao"] != DBNull.Value ? Convert.ToDateTime(reader["dataSolicitacao"]) : DateTime.MinValue;
                s.codPatrimonio = Convert.ToInt32(reader["patrimonio"]);
                s.statusSolicitacao = ObterDescricaoStatus(Convert.ToInt32(reader["status"]));
                lista.Add(s);
            }
        }

        return lista;
    }

    /// <summary>
    /// Converte o código de status numérico para descrição textual.
    /// </summary>
    private static string ObterDescricaoStatus(int statusId)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT Nome 
        FROM dbo.StatusDaSolicitacao 
        WHERE StatusId = @StatusId", con))
        {
            cmd.Parameters.AddWithValue("@StatusId", statusId);

            con.Open();
            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
                return result.ToString();

            return "Desconhecido"; // valor padrão caso não encontre
        }
    }

    /// <summary>
    /// Retorna o histórico de solicitações de OS realizadas por um determinado login de solicitante.
    /// </summary>
    /// <param name="login">Login do usuário solicitante</param>
    /// <returns>Lista de objetos SolicitanteDados com informações das OS</returns>
    public static List<SolicitanteDados> BuscarHistoricoOSPorLogin(string login)
    {
        var lista = new List<SolicitanteDados>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"
            SELECT 
                [id_solicitacao]
      ,[andar]
      ,[local]
      ,[descricaoServico]
      ,[dataSolicitacao]
      ,[status]
      ,[patrimonio]
      ,[NomeCompleto]
      ,[loginSolicitante]
      ,[descricaoCentroDeCusto]
            FROM Vw_HistoricoOS
            WHERE loginSolicitante = @loginSolicitante
            ORDER BY id_solicitacao DESC";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@loginSolicitante", login);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var s = new SolicitanteDados();

                s.idSolicitacao = Convert.ToInt32(reader["id_solicitacao"]);
                s.andar = reader["andar"].ToString();
                s.localDaSolicitacao = reader["local"].ToString();
                s.descricaoCentroCusto = reader["descricaoCentroDeCusto"].ToString();
                s.descServicoSolicitado = reader["descricaoServico"].ToString();
                s.dataSolicitacao = reader["dataSolicitacao"] != DBNull.Value
                    ? Convert.ToDateTime(reader["dataSolicitacao"])
                    : DateTime.MinValue;
                s.codPatrimonio = Convert.ToInt32(reader["patrimonio"]);

                // Converte o status numérico para uma descrição textual
                int status = Convert.ToInt32(reader["status"]);
                s.statusSolicitacao = ObterDescricaoStatus(status);

                lista.Add(s);
            }
        }

        return lista;
    }
    /// <summary>
    /// Retorna a equipe vinculada ao mesmo centro de custo do usuário informado.
    /// Cada colaborador retornado pertence ao(s) mesmo(s) centro(s) de custo do login base.
    /// </summary>
    /// <param name="login">Login do usuário base</param>
    /// <returns>Lista de colaboradores vinculados ao(s) mesmo(s) centro(s) de custo</returns>
    public static List<SolicitanteDados> BuscarEquipeCentroCusto(string login)
    {
        var lista = new List<SolicitanteDados>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            // Consulta para retornar todos os usuários que compartilham o mesmo centro de custo
            string query = @"
            SELECT  
    nomeUsuario, 
    loginRedeUsuario, 
    descricao, 
    codigoCentroDeCusto,
    nomeResponsavel,
diretoria
FROM Vw_DadosUsuario_DadosRespCusto
WHERE codigoCentroDeCusto IN (
    SELECT  codigoCentroDeCusto
    FROM Vw_DadosUsuario_DadosRespCusto
    WHERE loginRedeUsuario =@loginSolicitante
)
ORDER BY codigoCentroDeCusto;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@loginSolicitante", login);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var s = new SolicitanteDados();
                s.nomeSolicitante = reader["nomeUsuario"].ToString();
                s.loginSolicitante = reader["loginRedeUsuario"].ToString();
                s.descricaoCentroCusto = reader["descricao"].ToString();
                s.codCentroCusto = Convert.ToInt32(reader["codigoCentroDeCusto"]);
                s.nomeResponsavel_Custo = reader["nomeResponsavel"].ToString();
                s.diretoria = reader["diretoria"].ToString();
                lista.Add(s);
            }
        }

        return lista;
    }
    // Replace all usages of string.IsNullOrWhiteSpace with a compatible implementation for .NET 3.5
    private static bool IsNullOrWhiteSpace(string value)
    {
        return value == null || value.Trim().Length == 0;
    }


   
}
