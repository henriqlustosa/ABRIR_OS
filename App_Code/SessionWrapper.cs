using System.Collections.Generic;
using System.Web;

public static class SessionWrapper
{
    public static string Login
    {
        get { return HttpContext.Current.Session["login"] as string; }
        set { HttpContext.Current.Session["login"] = value; }
    }

    public static string NomeUsuario
    {
        get { return HttpContext.Current.Session["nomeUsuario"] as string; }
        set { HttpContext.Current.Session["nomeUsuario"] = value; }
    }

    public static int CodRespCusto
    {
        get
        {
            object valor = HttpContext.Current.Session["codRespCusto"];
            return valor != null ? (int)valor : 0;
        }
        set { HttpContext.Current.Session["codRespCusto"] = value; }
    }

    public static List<int> Perfis
    {
        get { return HttpContext.Current.Session["perfis"] as List<int>; }
    }
}
