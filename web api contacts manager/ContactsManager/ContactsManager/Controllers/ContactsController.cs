using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ContactsManager.Filters;
using ContactsManager.Models;

namespace ContactsManager.Controllers
{
    [ContactSelfLinkFilter]
    [RoutePrefix("contacts")]
    public class ContactsController : ApiController
    {
        private readonly IContactRepository _repository;

        public ContactsController(IContactRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        public IEnumerable<Contact> Get()
        {
            return _repository.GetAll();
        }

        [Route("{id:int}", Name = "GetContactById")]
        public HttpResponseMessage Get(int id)
        {
            var contact = _repository.Get(id);
            if (contact == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, contact);
        }

        [Route("")]
        public IHttpActionResult Post(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(contact);
                return CreatedAtRoute("GetContactById", new {id = contact.ContactId}, contact);
            }

            return BadRequest(ModelState);
        }

        [Route("{id:int}")]
        public HttpResponseMessage Put(int id, Contact contact)
        {
            contact.ContactId = id;
            _repository.Update(contact);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [Route("{id:int}")]
        public HttpResponseMessage Delete(int id)
        {
            var deleted = _repository.Get(id);
            if (deleted == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            _repository.Delete(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}