using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.WebApi
{
    /// <summary>
    /// 标记该属性的方法在SwaggerUI中不展示
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SecretVersionedRoute : Attribute
    {
        public SecretVersionedRoute()
        {
        }
    }
}
