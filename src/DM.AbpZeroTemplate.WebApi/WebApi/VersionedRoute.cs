using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.WebApi
{
    [AttributeUsage(AttributeTargets.All)]
    public class VersionedRoute : Attribute
    {
        public VersionedRoute(string name, int version)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; set; }
        public int Version { get; set; }
    }
}
