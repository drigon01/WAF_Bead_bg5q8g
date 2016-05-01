using System.Web;
using System.Web.Mvc;

namespace WAF_Bead_bg5q8g
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
