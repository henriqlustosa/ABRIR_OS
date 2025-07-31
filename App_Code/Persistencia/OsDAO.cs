using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OsDAO
/// </summary>
public class OsDAO
{


    public static List<SolicitanteDados> carregaDadosUsuRespCentroDeCusto(string login)
    {
        var lista = new List<SolicitanteDados>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString()))
        {
            try
            {
                string strQuery;
                strQuery = @"SELECT [NomeCompleto],[loginRede],[rf_usuario],[RSP_A_NOME],[rf_responsavel_custo],[SET_A_DES],[Codigo_centroCusto]
  FROM [hspm_OS].[dbo].[Vw_DadosUsuario_DadosRespCusto] where loginRede=@loginRede ";
                con.Open();
                SqlCommand commd = new SqlCommand(strQuery, con);
                commd.Parameters.AddWithValue("@loginRede", login.Trim());
                SqlDataReader dr = commd.ExecuteReader();
                while (dr.Read())
                {
                    SolicitanteDados s = new SolicitanteDados();
                    s.nomeSolicitante = dr.IsDBNull(0) ? null : dr.GetString(0);
                    s.loginSolicitante = dr.IsDBNull(1) ? null : dr.GetString(1);
                    s.rfSolicitante = dr.IsDBNull(2) ? null : dr.GetString(2);
                    s.nomeResponsavel_Custo = dr.IsDBNull(3) ? null : dr.GetString(3);
                    s.rfResponsavelCusto = dr.IsDBNull(4) ? null : dr.GetString(4);
                    s.descricaoCentroCusto = dr.IsDBNull(5) ? null : dr.GetString(5);
                    s.codCentroCusto = dr.GetInt32(6);
                    lista.Add(s);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                throw;
            }
            return lista;
        }
    }
    public static void GravaCentroDecustoDofuncionario(string login, int codCentroDeCusto)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString()))
        {
            try
            {
                string strQuery = @"INSERT INTO [dbo].[Usuario_CentroDeCusto]
           ([loginRede]
           ,[id_centroDeCusto])
     VALUES
           (@loginRede,@id_centroDeCusto)";
                SqlCommand cmd = new SqlCommand(strQuery, con);
                cmd.Parameters.AddWithValue("@loginRede", login);
                cmd.Parameters.AddWithValue("@id_centroDeCusto", codCentroDeCusto);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                throw;
            }
        }
    }

    public static List<SolicitanteDados> BuscarCentroDeCustoPorLogin(string login)
    {
        var lista = new List<SolicitanteDados>();
        string connectionString = ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"SELECT SET_A_COD, SET_A_DES 
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
            con.Close();
        }
        return lista;
    }

    public static List<SolicitanteDados> BuscarCentroDeCustoPorCentroDeCusto(string centroCusto)
    {
        var lista = new List<SolicitanteDados>();
        string connectionString = ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"SELECT [RSP_A_NOME],[rf_responsavel_custo],[RSP_I_CODI]       
  FROM [hspm_OS].[dbo].[Vw_DadosUsuario_DadosRespCusto]
  where  [Codigo_centroCusto]= @Codigo_centroCusto";

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
            con.Close();
        }
        return lista;
    }

    public static List<SolicitanteDados> BuscarPatrimonio(int codPatrimonio)
    {
        var lista = new List<SolicitanteDados>();
        string connectionString = ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"SELECT [BEM_A_DESC]
  FROM [hspm_OS].[dbo].[NumeroPatrimonio]
  where BEM_A_CHAP=@BEM_A_CHAP";

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
            con.Close();
        }
        return lista;
    }

    //public static List<SolicitanteDados> BuscarSetoresSolicitados()
    //{
    //    var lista = new List<SolicitanteDados>();
    //    string connectionString = ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString();

    //    using (SqlConnection con = new SqlConnection(connectionString))
    //    {
    //        string query = @"SELECT [id],[Setor_Solicitado]      
    //         FROM [hspm_OS].[dbo].[setorSolicitado] 
    //         where ativo=1 order by Setor_Solicitado";

    //        SqlCommand cmd = new SqlCommand(query, con);
    //        //cmd.Parameters.AddWithValue("@loginRede", login);
    //        con.Open();
    //        SqlDataReader reader = cmd.ExecuteReader();

    //        while (reader.Read())
    //        {
    //            var s = new SolicitanteDados();
    //            s.codSetorSolicitado = Convert.ToInt32(reader["id"]);
    //            s.setorSolicitadoDesc = reader["Setor_Solicitado"].ToString();
    //            lista.Add(s);
    //        }
    //        con.Close();
    //    }
    //    return lista;
    //}


    public static int GravaSolicitacaoOS(SolicitanteDados s)
    {
        int nPedido = 0;
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString()))
        {
            try
            {
                string strQuery = @"
                INSERT INTO [dbo].[Solicitacao]
                   ([loginSolicitante]
                   ,[rfSolicitante]
                   ,[ramalSolicitante]
                   ,[centroCusto]
                   ,[responsavelCentroCusto]
                   ,[rfResponsavel]
                   ,[ramalRespCusto]
                   ,[patrimonio]                  
                   ,[andar]
                   ,[local]
                   ,[descricaoServico]
                   ,[obs]
                   ,[dataSolicitacao]
                   ,[status])
             VALUES
                   (@loginSolicitante,@rfSolicitante,@ramalSolicitante,@centroCusto,@responsavelCentroCusto,@rfResponsavel,@ramalRespCusto,@patrimonio,
                    @andar,@local,@descricaoServico,@obs,@dataSolicitacao,@status);
             SELECT CAST(SCOPE_IDENTITY() AS int);";

                SqlCommand cmd = new SqlCommand(strQuery, con);
                cmd.Parameters.AddWithValue("@loginSolicitante", s.loginSolicitante);
                cmd.Parameters.AddWithValue("@rfSolicitante", string.IsNullOrEmpty(s.rfSolicitante) ? (object)DBNull.Value : s.rfSolicitante);
                cmd.Parameters.AddWithValue("@ramalSolicitante", s.ramalSolicitante);
                cmd.Parameters.AddWithValue("@centroCusto", s.codCentroCusto);
                cmd.Parameters.AddWithValue("@responsavelCentroCusto", s.codRespCentroCusto);
                cmd.Parameters.AddWithValue("@rfResponsavel", string.IsNullOrEmpty(s.rfResponsavelCusto) ? (object)DBNull.Value : s.rfResponsavelCusto);
                cmd.Parameters.AddWithValue("@ramalRespCusto", string.IsNullOrEmpty(s.ramalRespSetor) ? (object)DBNull.Value : s.ramalRespSetor);
                cmd.Parameters.AddWithValue("@patrimonio", s.codPatrimonio);
                cmd.Parameters.AddWithValue("@andar", s.andar);
                cmd.Parameters.AddWithValue("@local", s.localDaSolicitacao);
                cmd.Parameters.AddWithValue("@descricaoServico", s.descServicoSolicitado);
                cmd.Parameters.AddWithValue("@obs", string.IsNullOrEmpty(s.obs) ? (object)DBNull.Value : s.obs);
                cmd.Parameters.AddWithValue("@dataSolicitacao", DateTime.Now);
                cmd.Parameters.AddWithValue("@status", 0);


                con.Open();
                nPedido = (int)cmd.ExecuteScalar(); // retorna o ID gerado
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                nPedido = 0;
            }
        }
        return nPedido;
    }


    public static List<SolicitanteDados> BuscarHistoricoOSPorLogin(string login)
    {
        var lista = new List<SolicitanteDados>();
        string connectionString = ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"SELECT [id_solicitacao],[andar],[local],[SET_A_DES]
     ,[descricaoServico],[dataSolicitacao],[status],[patrimonio]
  FROM [hspm_OS].[dbo].[Vw_HistoricoOS]
  where loginSolicitante=@loginSolicitante order by id_solicitacao desc";

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
                //s.setorSolicitadoDesc = reader["Setor_Solicitado"].ToString();
                s.descServicoSolicitado = reader["descricaoServico"].ToString();
                s.dataSolicitacao = reader["dataSolicitacao"] != DBNull.Value ? Convert.ToDateTime(reader["dataSolicitacao"]) : DateTime.MinValue;
                int status = Convert.ToInt32(reader["status"]);
                s.codPatrimonio = Convert.ToInt32(reader["patrimonio"]);
                switch (status)
                {
                    case 0:
                        s.statusSolicitacao = "Aguardando";
                        break;
                    case 1:
                        s.statusSolicitacao = "Recebido";
                        break;
                    case 2:
                        s.statusSolicitacao = "Executando";
                        break;
                    case 3:
                        s.statusSolicitacao = "Em espera ";
                        break;
                    case 4:
                        s.statusSolicitacao = "Finalizado";
                        break;
                    case 5:
                        s.statusSolicitacao = "Recusado";
                        break;
                    default:
                        s.statusSolicitacao = "Desconhecido";
                        break;
                }

                lista.Add(s);
            }
            con.Close();
        }
        return lista;
    }
    public static List<SolicitanteDados> BuscarEquipeCentroCusto(string login)
    {
        var lista = new List<SolicitanteDados>();
        string connectionString = ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {

            //            string query = @"SELECT DISTINCT [NomeCompleto],[loginRede],[SET_A_DES],[Codigo_centroCusto],[RSP_A_NOME]
            //FROM [hspm_OS].[dbo].[Vw_DadosUsuario_DadosRespCusto] 
            //WHERE RSP_I_CODI IN (
            //    SELECT DISTINCT RSP_I_CODI
            //    FROM [hspm_OS].[dbo].[Vw_DadosUsuario_DadosRespCusto]
            //    WHERE loginRede = @loginSolicitante 
            //)
            //order by Codigo_centroCusto";
            string query = @" SELECT DISTINCT [NomeCompleto], [loginRede], [SET_A_DES], [Codigo_centroCusto],[RSP_A_NOME]
FROM[hspm_OS].[dbo].[Vw_DadosUsuario_DadosRespCusto]
WHERE Codigo_centroCusto IN(
    SELECT DISTINCT Codigo_centroCusto
    FROM[hspm_OS].[dbo].[Vw_DadosUsuario_DadosRespCusto]
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
                s.codCentroCusto = Convert.ToInt32(reader["Codigo_centroCusto"].ToString());
                s.nomeResponsavel_Custo = reader["RSP_A_NOME"].ToString();
                lista.Add(s);
            }
            con.Close();
        }
        return lista;
    }

    public static List<SolicitanteDados> VerificaOsAbertaPatrimonio(int patrimonio)
    {
        var lista = new List<SolicitanteDados>();
        string connectionString = ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"SELECT [NomeCompleto]
      ,[id_solicitacao]
      ,[dataSolicitacao]      
      ,[patrimonio]
      ,[local]
      ,[descricaoServico]
      ,[status]
  FROM [hspm_OS].[dbo].[Vw_VericaOsAbertaPatrimonio]
   where (status <> 4 AND status <> 5) and patrimonio=@patrimonio and patrimonio!=46879";

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
                //s.descricaoCentroCusto = reader["SET_A_DES"].ToString();
                //s.setorSolicitadoDesc = reader["Setor_Solicitado"].ToString();
                s.descServicoSolicitado = reader["descricaoServico"].ToString();
                s.dataSolicitacao = reader["dataSolicitacao"] != DBNull.Value ? Convert.ToDateTime(reader["dataSolicitacao"]) : DateTime.MinValue;
                int status = Convert.ToInt32(reader["status"]);
                s.codPatrimonio = Convert.ToInt32(reader["patrimonio"]);
                switch (status)
                {
                    case 0:
                        s.statusSolicitacao = "Aguardando";
                        break;
                    case 1:
                        s.statusSolicitacao = "Recebido";
                        break;
                    case 2:
                        s.statusSolicitacao = "Executando";
                        break;
                    case 3:
                        s.statusSolicitacao = "Em espera ";
                        break;
                    case 4:
                        s.statusSolicitacao = "Finalizado";
                        break;
                    case 5:
                        s.statusSolicitacao = "Recusado";
                        break;
                    default:
                        s.statusSolicitacao = "Desconhecido";
                        break;
                }

                lista.Add(s);
            }
            con.Close();
        }
        return lista;
    }

    //  public static List<ConsultasCentroDeCusto> carregaCentroDeCusto()
    //  {
    //      var lista = new List<ConsultasCentroDeCusto>();
    //      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString()))
    //      {
    //          try
    //          {
    //              string strQuery;
    //              strQuery = @"SELECT SET_A_COD, CONCAT( [SET_A_COD],+' - ',[SET_A_DES]) as custo
    //FROM [hspm_OS].[dbo].[CentroDeCusto] order by SET_A_DES";
    //              con.Open();
    //              SqlCommand commd = new SqlCommand(strQuery, con);
    //              SqlDataReader dr = commd.ExecuteReader();
    //              while (dr.Read())
    //              {
    //                  ConsultasCentroDeCusto c = new ConsultasCentroDeCusto();
    //                  c.idCentroCusto = dr.GetInt32(0);
    //                  c.desc_centroCusto = dr.IsDBNull(1) ? null : dr.GetString(1);
    //                  lista.Add(c);
    //              }
    //              con.Close();
    //          }
    //          catch (Exception ex)
    //          {
    //              string erro = ex.Message;
    //              throw;
    //          }
    //          return lista;
    //      }
    //  }

       
}