using System;
using System.Web.Http;

namespace ContactsManager.Controllers
{
    public class ItemsController : ApiController
    {
        [Route("items")]
        public void Get()
        {
            throw new Exception("something dramatic happened!");
        }
    }
}