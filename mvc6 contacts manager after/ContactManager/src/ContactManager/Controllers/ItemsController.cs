using System;
using Microsoft.AspNet.Mvc;

namespace ContactManager.Controllers
{
    public class ItemsController
    {
        [HttpGet("items")]
        public void Get()
        {
            throw new Exception("something dramatic happened!");
        }
    }
}