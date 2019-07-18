using SecuroteckWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace SecuroteckWebApplication.Controllers
{
    public class ProtectedController : ApiController
    {
        [APIAuthorise]
        [ActionName("Hello")]
        public HttpResponseMessage Get()
        {
            string key = Request.Headers.GetValues("ApiKey").First();
            var dbaccess = new UserDatabaseAccess();
            dbaccess.KeyReturnUsername(key);
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Hello " + dbaccess.KeyReturnUsername(key));
            }
        }
        [APIAuthorise]
        [Route("api/protected/sha1")]
        [HttpGet] public HttpResponseMessage Sha1([FromUri]string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
            string Message = message;
            byte[] asciiByteMessage = Encoding.ASCII.GetBytes(Message);
            byte[] sha1ByteMessage;
            SHA1 sha1Provider = new SHA1CryptoServiceProvider();
            sha1ByteMessage = sha1Provider.ComputeHash(asciiByteMessage);
            ByteArrayToString(sha1ByteMessage);
            return Request.CreateResponse(HttpStatusCode.OK, ByteArrayToString(sha1ByteMessage));
        }

        [APIAuthorise]
        [Route("api/protected/sha256")]
        [HttpGet]
        public HttpResponseMessage Sha256([FromUri]string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
            string Message = message;
            byte[] asciiByteMessage = Encoding.ASCII.GetBytes(Message);
            byte[] sha256ByteMessage;
            SHA256 sha256Provider = new SHA256CryptoServiceProvider();
            sha256ByteMessage = sha256Provider.ComputeHash(asciiByteMessage);
            ByteArrayToString(sha256ByteMessage);
            return Request.CreateResponse(HttpStatusCode.OK, ByteArrayToString(sha256ByteMessage));
        }

        static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
                
            }
            return hex.ToString().ToUpper();
        }

    }
}
