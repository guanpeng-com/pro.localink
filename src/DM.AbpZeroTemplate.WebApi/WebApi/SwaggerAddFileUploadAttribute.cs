using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.WebApi
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerAddFileUploadAttribute : Attribute
    {
        public bool Required { get; private set; }

        public SwaggerAddFileUploadAttribute(bool Required = true)
        {
            this.Required = Required;

        }
    }
}
