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
    [Table("userDetails")]
    public class UserDetails : IBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = "Benjamin";

        [Many]
        public List<SupportQuery> SupportQueries { get; set; }
    }
}
