using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLite.Attributes
{
    public class ForeignKeyAttribute : IgnoreAttribute
    {
        public string Property { get; set; }

        public ForeignKeyAttribute(string property)
        { 
            Property = property;
        }
    }
}
