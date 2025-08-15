using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SolicitanteDados
/// </summary>
public class SolicitanteDados
{
    public string loginSolicitante { get; set; }
    public string nomeSolicitante { get; set; }
    public string rfSolicitante { get; set; }
    public string nomeResponsavel_Custo { get; set; }
    public string rfResponsavelCusto { get; set; }
    public string descricaoCentroCusto { get; set; }
    public int codCentroCusto { get; set; }
    public string equipamentoDesc { get; set; }
    public int codSetorSolicitado { get; set; }
    public string setorSolicitadoDesc { get; set; }
    public int codPatrimonio { get; set; }
    public string andar { get; set; }
    public string localDaSolicitacao { get; set; }
    public string descServicoSolicitado { get; set; }
    public string obs { get; set; }
    public string codRespCentroCusto { get; set; }
    public string ramalSolicitante { get; set; }
    public string ramalRespSetor { get; set; }
    public int idSolicitacao { get; set; }
    public DateTime dataSolicitacao { get; set; }
    public string statusSolicitacao { get; set; }

    public int rfUsuario { get; set; }
    public string diretoria { get; set; }
}