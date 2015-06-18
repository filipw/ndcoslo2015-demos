using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Filters;
using ContactsManager.Models;

namespace ContactsManager.Filters
{
    public class ContactSelfLinkFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var response = actionExecutedContext.Response;

            Contact contact;
            if (response.TryGetContentValue(out contact))
            {
                AddSelfLink(contact, actionExecutedContext);
                return;
            }

            IEnumerable<Contact> contacts;
            if (response.TryGetContentValue(out contacts))
            {
                var objectContent = (ObjectContent<IEnumerable<Contact>>)actionExecutedContext.Response.Content;
                objectContent.Value = contacts.Select(c => AddSelfLink(c, actionExecutedContext)).ToList();
            }
        }

        static Contact AddSelfLink(Contact contact, HttpActionExecutedContext context)
        {
            var linkProvider = context.Request.GetDependencyScope().GetService(typeof(LinkProvider)) as LinkProvider;
            if (linkProvider == null)
            {
                throw new Exception("Link provider not found");
            }

            contact.Self = linkProvider.GetLink(context.Request, "GetContactById", new { id = contact.ContactId }).ToString(); ;
            return contact;
        }
    }
}