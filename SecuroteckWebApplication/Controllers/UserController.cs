using Newtonsoft.Json.Linq;
using SecuroteckWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SecuroteckWebApplication.Controllers
{
    public class UserController : ApiController
    {

        [ActionName("New")]
        public HttpResponseMessage Get([FromUri]string username)
        {
            var dbaccess = new UserDatabaseAccess();

            if (dbaccess.CheckUserName(username))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "True - User Does Exist! Did you mean to do a POST to create a new user?");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "False - User Does Not Exist! Did you mean to do a POST to create a new user?");
            }

        }

        [ActionName("New")]
        public HttpResponseMessage Post([FromBody]string username)
        {

            if (string.IsNullOrEmpty(username))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Oops. Make sure your body contains a string with your username and your Content - Type is Content - Type:application / json");
            }
            var dbaccess = new UserDatabaseAccess();
            if (dbaccess.CheckUserName(username))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Oops. This username is already in use. Please try again with a new username.");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, dbaccess.NewUser(username));
            }
        }

        [APIAuthorise]
        [ActionName("RemoveUser")]
        public HttpResponseMessage Delete([FromUri]string username)
        {

            string key = Request.Headers.GetValues("ApiKey").First();
            var dbaccess = new UserDatabaseAccess();
            if (dbaccess.DeleteUser(username, key))
            {
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, false);
            }
        }

        [APIAuthorise]
        [AdminRole]
        [ActionName("ChangeRole")]
        public HttpResponseMessage Post([FromBody]JObject username)
        {

            string user = username.GetValue("username").ToString();
            string role = username.GetValue("role").ToString();
            if (role == "User" || role == "Admin")
            {
                var dbaccess = new UserDatabaseAccess();
                if (!dbaccess.CheckUserName(user))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NOT DONE: Username does not exist");
                }
                else if (dbaccess.ChangeRole(user, role))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "DONE");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NOT DONE: An error occured");
                }
                
            }
            else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NOT DONE: Role does not exist");
                }

        }
        
    }
}
