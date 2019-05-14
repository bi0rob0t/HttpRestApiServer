using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : Attribute
    {
        public string ControllerName { get; private set; }

        public ControllerAttribute(string name)
        {
            ControllerName = name;
        }


    }
}
