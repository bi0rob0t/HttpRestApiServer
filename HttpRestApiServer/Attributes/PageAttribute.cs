using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab6.Attributes
{

    [AttributeUsage(AttributeTargets.Method)]
    public class PageAttribute : Attribute
    {        
        public string _url { get; private set; }
        private string _pattern;
        public PageAttribute(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentException();
            _pattern = pattern;
        }

        public bool ValidationUrl(IEnumerable<string> urlParams)
        {
            string url = "";
            foreach (string param in urlParams)
                url += param;            
            return new Regex(_pattern, RegexOptions.IgnoreCase).IsMatch(url);
        }
    }
}
