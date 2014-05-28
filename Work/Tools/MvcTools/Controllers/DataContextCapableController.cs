using System.Web.Mvc;
using MvcTools.Web;

namespace MvcTools.Controllers
{
    public abstract class DataContextCapableController : LayoutFlexibleController 
    {
        public abstract string DataContextKey { get; }

        protected string GridGroup { get { return SessionHelper.GetSessionString("GridGroup"); } }

        [ValidateInput(false)]
        protected override System.IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, System.AsyncCallback callback, object state)
        {
            SessionStore.SetCurrentDataContextKey(DataContextKey);

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}
