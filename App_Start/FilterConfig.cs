using System.Web;
using System.Web.Mvc;

namespace Team_Project_Deliverable_3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}