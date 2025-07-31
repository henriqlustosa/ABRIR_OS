<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html>
<head>
    <title>Ordem de Serviço</title>    
    
    <link href="css/login.css?v=2" rel="stylesheet" />
</head>
<body class="body">
    <div class="login__titulo">Ordem de Serviço</div>
    <form id="form1" runat="server" style="width: 65%">
        <div class="login__container">
            <div class="login__imagem">
                <img src="imagem/manutencao.svg" />
            </div>
            <div class="login__box ">
                <div class="login__box-informacao">
                    <div class="lista-locais">
                       <%-- <h2 class="login__box-informacao-titulo">Salas para reserva</h2>
                        <ul class="list">
                            <li class="list__item">Anfiteatro</li>
                            <li class="list__item">9º andar - Sala de Grupos</li>
                            <li class="list__item">3º andar - Sala de Reuniões</li>
                        </ul>--%>
                        <p class="login__box-informacao-texto">* Usar o mesmo login e senha de rede.</p>
                    </div>
                </div>
                <div class="login__box-autentica">
                    <h2>Login*</h2>
                    <asp:Label ID="lblUsuario" runat="server" Text="Usuário:"></asp:Label>
                    <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblSenha" runat="server" Text="Senha:"></asp:Label>
                    <asp:TextBox ID="txtSenha" runat="server" TextMode="Password"></asp:TextBox><br />
                    <br />
                    <div class="login__box-botao">
                        <asp:Button ID="btnLogin" runat="server" Text="Entrar" OnClick="btnLogin_Click" /><br />
                    </div>

                    <asp:Label ID="lblMensagem" runat="server" ForeColor="Red"></asp:Label>
                    <div class="login__logo">
                        <%--<img class="logo__hspm" src="../img/hspmLogoColor.jpg" />
                        <img class="logo__PMSP" src="../img/logoPrefSP.png" />--%>
                        <img class="logo" src="imagem/logoHspmPrefeituraColor.jpg" />
                    </div>
                </div>
            </div>
        </div>
    </form>
    <footer>
        <p>Desenvolvido por DITEC (Divisão de Tecnologia da Informação) - hspminformatica@hspm.sp.gov.br</p>
    </footer>
   <%-- </form>--%>
</body>
</html>

