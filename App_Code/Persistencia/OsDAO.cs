using System;
using System.Collections.Generic;
using System.Configuration;
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
                SELECT NomeCompleto, loginRede, rf_usuario, RSP_A_NOME, rf_responsavel_custo, SET_A_DES, Codigo_centroCusto
                FROM Vw_DadosUsuario_DadosRespCusto
                WHERE loginRede = @loginRede";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@loginRede", login.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var s = new SolicitanteDados();
                s.nomeSolicitante = dr["NomeCompleto"] as string;
                s.loginSolicitante = dr["loginRede"] as string;
                s.rfSolicitante = dr["rf_usuario"] as string;
                s.nomeResponsavel_Custo = dr["RSP_A_NOME"] as string;
                s.rfResponsavelCusto = dr["rf_responsavel_custo"] as string;
                s.descricaoCentroCusto = dr["SET_A_DES"] as string;
                s.codCentroCusto = Convert.ToInt32(dr["Codigo_centroCusto"]);
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
                SELECT SET_A_COD, SET_A_DES
                FROM Vw_Usuario_CentroDeCusto
                WHERE loginRede = @loginRede";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@loginRede", login);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var s = new SolicitanteDados();
                s.codCentroCusto = Convert.ToInt32(reader["SET_A_COD"]);
                s.descricaoCentroCusto = reader["SET_A_COD"] + " - " + reader["SET_A_DES"].ToString();
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
                SELECT RSP_A_NOME, rf_responsavel_custo, RSP_I_CODI
                FROM Vw_DadosUsuario_DadosRespCusto
                WHERE Codigo_centroCusto = @Codigo_centroCusto";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Codigo_centroCusto", centroCusto);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var s = new SolicitanteDados();
                s.nomeResponsavel_Custo = reader["RSP_A_NOME"].ToString();
                s.rfResponsavelCusto = reader["rf_responsavel_custo"].ToString();
                s.codRespCentroCusto = Convert.ToInt32(reader["RSP_I_CODI"]);
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
    public static int GravaSolicitacaoOS(SolicitanteDados s)
    {
        int nPedido = 0;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"
                INSERT INTO Solicitacao (
                    loginSolicitante, rfSolicitante, ramalSolicitante, centroCusto, 
                    responsavelCentroCusto, rfResponsavel, ramalRespCusto, patrimonio, 
                    andar, local, descricaoServico, obs, dataSolicitacao, status
                )
                VALUES (
                    @loginSolicitante, @rfSolicitante, @ramalSolicitante, @centroCusto, 
                    @responsavelCentroCusto, @rfResponsavel, @ramalRespCusto, @patrimonio, 
                    @andar, @local, @descricaoServico, @obs, @dataSolicitacao, @status
                );
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            SqlCommand cmd = new SqlCommand(query, con);

            // Parâmetros da solicitação
            cmd.Parameters.AddWithValue("@loginSolicitante", s.loginSolicitante);
            cmd.Parameters.AddWithValue("@rfSolicitante", (object)s.rfSolicitante ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ramalSolicitante", s.ramalSolicitante);
            cmd.Parameters.AddWithValue("@centroCusto", s.codCentroCusto);
            cmd.Parameters.AddWithValue("@responsavelCentroCusto", s.codRespCentroCusto);
            cmd.Parameters.AddWithValue("@rfResponsavel", (object)s.rfResponsavelCusto ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ramalRespCusto", (object)s.ramalRespSetor ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@patrimonio", s.codPatrimonio);
            cmd.Parameters.AddWithValue("@andar", s.andar);
            cmd.Parameters.AddWithValue("@local", s.localDaSolicitacao);
            cmd.Parameters.AddWithValue("@descricaoServico", s.descServicoSolicitado);
            cmd.Parameters.AddWithValue("@obs", (object)s.obs ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@dataSolicitacao", DateTime.Now);
            cmd.Parameters.AddWithValue("@status", 0); // status inicial: Aguardando

            con.Open();
            nPedido = (int)cmd.ExecuteScalar();
        }

        return nPedido;
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
                  AND patrimonio != 46879";

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
    private static string ObterDescricaoStatus(int status)
    {
        switch (status)
        {
            case 0: return "Aguardando";
            case 1: return "Recebido";
            case 2: return "Executando";
            case 3: return "Em espera";
            case 4: return "Finalizado";
            case 5: return "Recusado";
            default: return "Desconhecido";
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
                id_solicitacao,
                andar,
                local,
                SET_A_DES,
                descricaoServico,
                dataSolicitacao,
                status,
                patrimonio
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
                s.descricaoCentroCusto = reader["SET_A_DES"].ToString();
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
            SELECT DISTINCT 
                NomeCompleto, 
                loginRede, 
                SET_A_DES, 
                Codigo_centroCusto,
                RSP_A_NOME
            FROM Vw_DadosUsuario_DadosRespCusto
            WHERE Codigo_centroCusto IN (
                SELECT DISTINCT Codigo_centroCusto
                FROM Vw_DadosUsuario_DadosRespCusto
                WHERE loginRede = @loginSolicitante
            )
            ORDER BY Codigo_centroCusto";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@loginSolicitante", login);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var s = new SolicitanteDados();
                s.nomeSolicitante = reader["NomeCompleto"].ToString();
                s.loginSolicitante = reader["loginRede"].ToString();
                s.descricaoCentroCusto = reader["SET_A_DES"].ToString();
                s.codCentroCusto = Convert.ToInt32(reader["Codigo_centroCusto"]);
                s.nomeResponsavel_Custo = reader["RSP_A_NOME"].ToString();

                lista.Add(s);
            }
        }

        return lista;
    }


    // Você pode continuar a refatorar os outros métodos seguindo esse mesmo padrão:
    // - Uso de `using` para garantir fechamento de conexões
    // - Nomes claros
    // - Comentários objetivos
    // - Conversão segura de tipos
}
