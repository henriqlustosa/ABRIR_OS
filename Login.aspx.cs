using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Web;
using System.Web.Security;

public partial class Login : System.Web.UI.Page
{
    // Executado sempre que a página é carregada (inclusive postbacks)
    protected void Page_Load(object sender, EventArgs e)
    {
        // Impede que a página seja armazenada em cache (por segurança)
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
        Response.Cache.SetNoStore();
        Response.Cache.SetAllowResponseInBrowserHistory(false);
    }

    // Evento disparado ao clicar no botão de login
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        // Opção recomendada (independente de cultura)
        string login = (txtUsuario.Text ?? string.Empty).Trim().ToUpperInvariant();

        string senha = txtSenha.Text.Trim();   // Senha digitada

        try
        {
            // =============================
            // 1. Autenticação no Active Directory
            // =============================
            using (DirectoryEntry entry = new DirectoryEntry("LDAP://10.10.68.43", login, senha))
            {
                // Tenta autenticar. Se falhar, lança exceção.
                object nativeObject = entry.NativeObject;

                // =============================
                // 2. Buscar nome completo no AD
                // =============================
                string nomeCompleto = "";
                using (DirectorySearcher searcher = new DirectorySearcher(entry))
                {
                    // Filtro para encontrar o usuário pelo login
                    searcher.Filter = string.Format("(sAMAccountName={0})", login);

                    searcher.PropertiesToLoad.Add("displayName"); // Campo que queremos buscar

                    SearchResult result = searcher.FindOne();
                    if (result != null && result.Properties.Contains("displayname"))
                    {
                        nomeCompleto = result.Properties["displayname"][0].ToString();
                    }
                }

                // =============================
                // 3. Verificar se o usuário existe na base do sistema
                // =============================
                int usuarioId;
                List<int> perfisDoUsuario = new List<int>(); // Lista de perfis a serem preenchidos

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString()))
                {
                    string sql = "SELECT UsuarioId FROM Usuarios WHERE LoginRede = @LoginRede AND Ativo = 1";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@LoginRede", login);

                    con.Open();
                    object usuarioIdObj = cmd.ExecuteScalar(); // Retorna o ID do usuário
                    con.Close();

                    // Se não encontrou o usuário ou ele está inativo
                    if (usuarioIdObj == null)
                    {
                        lblMensagem.Text = "Usuário sem permissão de acesso.";
                        return;
                    }

                    usuarioId = Convert.ToInt32(usuarioIdObj); // Converte para int
                }

                // =============================
                // 4. Buscar perfis do usuário
                // =============================
                using (SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["hspm_OSConnectionString"].ToString()))
                {
                    string query = @"
                            SELECT up.idPerfil
                                FROM UsuarioPerfil up                          
                                WHERE up.UsuarioId = @UsuarioId";

                    SqlCommand cmd2 = new SqlCommand(query, con2);
                    cmd2.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    con2.Open();
                    SqlDataReader reader = cmd2.ExecuteReader();

                    while (reader.Read())
                    {
                        // Adiciona cada perfil à lista
                        perfisDoUsuario.Add(Convert.ToInt32(reader["idPerfil"]));
                    }

                    reader.Close();
                }

                // Verifica se o usuário não possui perfis
                if (perfisDoUsuario.Count == 0)
                {
                    lblMensagem.Text = "Usuário sem perfil de acesso.";
                    return;
                }

                // =============================
                // 5. Salvar informações na sessão
                // =============================
                Session["login"] = login;
                Session["perfis"] = perfisDoUsuario;
                Session["nomeUsuario"] = nomeCompleto;

                // =============================
                // 6. Redirecionar para página principal
                // =============================
                Response.Redirect("~/SolicitarOS.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
        catch (Exception ex)
        {
            // Em produção, o ideal seria logar o erro (ex.Message)
            lblMensagem.Text = "Login inválido. Verifique seu usuário e senha.";
        }
    }
}
