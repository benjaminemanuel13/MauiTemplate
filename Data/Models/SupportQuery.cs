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
    public class SupportQuery : IBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserDetailsId { get; set; }

        [ForeignKey("UserDetailsId")]
        [Ignore]
        public UserDetails UserDetails { get; set; }

        public string Description { get; set; } = "Description";

        public string Title { get; set; } = "Title";

        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
