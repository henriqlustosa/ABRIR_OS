<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SolicitarOS.aspx.cs" Inherits="SolitarOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/reset.css" rel="stylesheet" />
    <link href="css/SolicitarOS.css?v=2" rel="stylesheet" />
    <link href="css/estilos.css" rel="stylesheet" />
 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h4 class="tituloPagina">Solicita Ordem de Serviço</h4>
    <main class="container">
        <section class="centroDeCusto">
            <div class="boxCentroDeCusto">
                <asp:Label ID="Label1" runat="server" Text="Centro de Custo:"></asp:Label>
                <div class="alinhaTxtDdl">
                    <asp:TextBox CssClass="txtCentrodeCusto input" ID="txtCentrodeCusto" runat="server" ReadOnly="true"></asp:TextBox>
                   <asp:DropDownList 
    ID="ddlSetor" 
    runat="server" 
    AutoPostBack="true" 
    OnSelectedIndexChanged="ddlSetor_SelectedIndexChanged"
    CssClass="ddlSetor input" 
    Height="34px">
</asp:DropDownList>

                </div>
            </div>
        </section>
        <hr />
        <section class="boxSolicitacao">
            <div class="divResponsavel">
                <div class="alinhaLblTxt divNome">
                    <asp:Label ID="Label2" runat="server" Text="Responsável:"></asp:Label>
                    <asp:TextBox ID="txtNomeResponsavel" CssClass="txtNomeResponsavel input" runat="server" Enabled="False"></asp:TextBox>
                </div>
                <div class="divRfRamal">
                    <div class="alinhaLblTxt divRf">
                        <asp:Label ID="lblRfResponsavel" runat="server" Text="R.F.:"></asp:Label>
                        <asp:TextBox ID="txtRfResponsavel" CssClass="txtRfResponsavelt input" runat="server" Enabled="False"></asp:TextBox>
                    </div>
                    <div class="alinhaLblTxt divRamal">
                        <asp:Label ID="lblRamalResponsavel" runat="server" Text="Ramal:"></asp:Label>
                        <asp:TextBox ID="txtRamalResponsavel" CssClass="txtRamalResponsavel input" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="divUsuario">
                <div class="alinhaLblTxt divNome">
                    <asp:Label ID="Label3" runat="server" Text="Usuário:"></asp:Label>
                    <asp:TextBox ID="txtNomeUsuario" CssClass="txtNomeUsuario input" runat="server" Enabled="False"></asp:TextBox>
                </div>
                <div class="divRfRamal">
                    <div class="alinhaLblTxt divRf">
                        <asp:Label ID="Label4" runat="server" Text="R.F.:"></asp:Label>
                        <asp:TextBox ID="txtRfUsuario" CssClass="txtRfUsuario input" runat="server" Enabled="False"></asp:TextBox>
                    </div>
                    <div class="alinhaLblTxt divRamal">
                        <asp:Label ID="Label5" runat="server" Text="Ramal:"></asp:Label>
                        <asp:TextBox ID="txtRamalUsuario" CssClass="txtRamalUsuario input" runat="server"></asp:TextBox>
                        <asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server"
                        controltovalidate="txtRamalUsuario"
                        errormessage="Obrigatório"
                        display="Dynamic"
                        forecolor="Red"
                        validationgroup="Cadastro" />
                    </div>
                </div>
            </div>
            <hr />
            <div class="solicitacao">
                <div class="alinhaLblTxt equipamento">
                    <div class="alinhaLblTxt divPatrimonio">
                        <asp:Label ID="Label8" runat="server" Text="Patrimônio:"></asp:Label>
                        <div class="alinhaTxtBtn">
                            <asp:TextBox ID="txtPatrimonio" CssClass="input txtPatrimonio" runat="server"></asp:TextBox>                        
                            <asp:Button ID="btnPesquisarPatrimonio" CssClass="btnPesquisarPatrimonio" runat="server" OnClick="btnPesquisarPatrimonio_Click" />
                            <asp:requiredfieldvalidator id="Requiredfieldvalidator6" runat="server"
                        controltovalidate="txtPatrimonio"
                        errormessage="Obrigatório"
                        display="Dynamic"
                        forecolor="Red"
                        validationgroup="Cadastro" />
                        </div>
                    </div>
                    <div class="alinhaLblTxt divEquipamento">
                        <asp:Label ID="Label6" runat="server" Text="Equipamento:"></asp:Label>
                          <asp:TextBox ID="txtEquipamento" CssClass="ddlEquipamento input" runat="server" placeholder="**Seviços sem Patrimônio digitem 46879 no campo Patrimonio**"></asp:TextBox>
                                 <asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server"
                        controltovalidate="txtEquipamento"
                        errormessage="Obrigatório"
                        display="Dynamic"
                        forecolor="Red"
                        validationgroup="Cadastro" />
                        <%--<asp:DropDownList ID="ddlEquipamento" CssClass="ddlEquipamento input" runat="server"></asp:DropDownList>--%>
                    </div>
                </div>

                <div class="informarcoes">
                 <%--   <div class="alinhaLblTxt divSetorSolicitado">
                        <asp:Label ID="Label9" runat="server" Text="Setor Solicitado:"></asp:Label>
                        <asp:DropDownList ID="ddlSetorSolicitado" CssClass="input ddlSetorSolicitado" runat="server" AutoPostBack="true" 
    OnSelectedIndexChanged="ddlSetorSolicitado_SelectedIndexChanged" ></asp:DropDownList>
                    </div>--%>
                    <div class="alinhaLblTxt divAndar">
                        <asp:Label ID="Label7" runat="server" Text="Andar:"></asp:Label>
                        <asp:TextBox ID="txtAndar" CssClass="input" runat="server"></asp:TextBox>
                             <asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server"
                        controltovalidate="txtAndar"
                        errormessage="Obrigatório"
                        display="Dynamic"
                        forecolor="Red"
                        validationgroup="Cadastro" />
                    </div>

                    <%--<div class="alinhaLblTxt divSerie">
                        <asp:Label ID="Label9" runat="server" Text="Série:"></asp:Label>
                        <asp:TextBox ID="txtSerie" CssClass="input" runat="server"></asp:TextBox>
                    </div>--%>
                    <div class="alinhaLblTxt divLocal">
                        <asp:Label ID="Label10" runat="server" Text="Local"></asp:Label>
                        <asp:TextBox ID="txtLocal" CssClass="txtLocal input" runat="server"></asp:TextBox>
                             <asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server"
                        controltovalidate="txtLocal"
                        errormessage="Obrigatório"
                        display="Dynamic"
                        forecolor="Red"
                        validationgroup="Cadastro" />
                    </div>
                </div>

                <div class="alinhaLblTxt divDescricao">
                    <asp:Label ID="lblDescricao" runat="server" Text="Descrição do serviço:"></asp:Label>
                    <%--<asp:TextBox ID="txtDescricao" runat="server" class="txtDescricao" TextMode="MultiLine"></asp:TextBox>--%>
                    <asp:TextBox ID="txtDescricao" runat="server" class="txtDescricao" TextMode="MultiLine"></asp:TextBox>
                         <asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server"
                        controltovalidate="txtDescricao"
                        errormessage="Obrigatório"
                        display="Dynamic"
                        forecolor="Red"
                        validationgroup="Cadastro" />
                </div>
                <div class="alinhaLblTxt divDescricao">
                    <asp:Label ID="LabelObs" runat="server" Text="Observações:"></asp:Label>
                    <%--<asp:TextBox ID="txtObs" runat="server" class="txtDescricao"  TextMode="MultiLine"></asp:TextBox>--%>
                    <asp:TextBox ID="txtObs" runat="server" class="txtDescricao"  TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <hr />
            <div class="divBotao">
                <asp:Button ID="btnSolicitarOS" runat="server" Text="Solicitar" CssClass="btnSolicitar" ValidationGroup="Cadastro" OnClick="btnSolicitarOS_Click" />
            </div>
        </section>
    </main>

      <script type="text/javascript">
    window.onload = function() {
        var usuario = '<%= Session["login"] != null ? Session["login"].ToString() : "" %>';

        if (usuario === "") {
            alert("Sessão expirada. Você será redirecionado para o login.");
            window.location.href = "../login.aspx";
        }
    }
</script>


</asp:Content>

