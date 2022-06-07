using EntityLite.Attributes;
using EntityLite.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    [Table("user")]
    public class User : IBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserDetailsId { get; set; }


        [ForeignKey("UserDetailsId")]
        public virtual UserDetails UserDetails { get; set; }
    }
}
