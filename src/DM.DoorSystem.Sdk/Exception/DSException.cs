using DM.DoorSystem.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Exception
{
    public class DSException : ApplicationException
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public Error Error { get; set; }


        public DSException()
        {

        }

        public DSException(string message)
            : base(message)
        {

        }

        public DSException(HttpStatusCode httpStatusCode, Error error, string message = null)
            : base(message)
        {

        }
    }
}
