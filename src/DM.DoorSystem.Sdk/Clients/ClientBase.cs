using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Clients
{
    public abstract class ClientBase<T>
        where T : ISdk, new()
    {
        protected Requestor Requestor;

        public ClientBase()
        {
            Requestor = new Requestor();
            Requestor.sdk = new T();
        }
    }
}
