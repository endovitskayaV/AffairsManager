using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AffairsManager.Models
{
    public class AffairsContext: DbContext
    {
        public DbSet<Affairs> Affairs { get; set; }
    }
}