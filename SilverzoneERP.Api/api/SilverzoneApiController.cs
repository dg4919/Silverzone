using System;
using System.Web.Http;

namespace Silverzone.Web.api
{
    public class SilverzoneApiController : ApiController
    {

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            GC.SuppressFinalize(this);

            base.Dispose(disposing);
        }


    }
}
