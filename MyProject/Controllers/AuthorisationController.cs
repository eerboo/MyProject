using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using MyProject.Authentication;
using MyProject.Models;
using Newtonsoft.Json;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace MyProject.Controllers
{
    public class AuthorisationController : ApiController
    {
        [Route("login")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult Login(string login, string pass )
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, "");
            var headers = new HttpResponseMessage();

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(login, pass))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(login, false);
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new Models.CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName                         
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                            (
                            1, login, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                            );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);                      
                        var cookie = new CookieHeaderValue("Cookie1", enTicket);
                        
                        //headers.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        headers.Headers.AddCookies(new[] { cookie });
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        response.Headers.AddCookies(new[] { cookie });
                    }                    
                }
            }
            ModelState.AddModelError("", "Something Wrong : Username or Password invalid");
            return ResponseMessage(headers);
        }
        [Route("logout")]
        public IHttpActionResult LogOut()
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK,"");
            var cookie = new CookieHeaderValue("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response.Headers.AddCookies(new[] { cookie });

            FormsAuthentication.SignOut();
            return Ok();
        }   
    }
    
}
