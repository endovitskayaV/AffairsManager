using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq.Expressions;

namespace AffairsManager.Models
{
    public class AffairsContext: DbContext
    {
        public DbSet<Affairs> Affairs { get; set; }

        public void Add(Affairs affair)
        {
            this.Affairs.Add(affair);
            this.SaveChanges();
        }

        public void Edit (Affairs affair)
        {
            this.Affairs.AddOrUpdate(affair);
            this.SaveChanges();
        }

        public void Delete(Affairs affair)
        {
            this.Affairs.Remove(affair);
            this.SaveChanges();
        }

        public Affairs GetAffair(Expression<Func<Affairs, bool>> predicate)
        {
            return this.Affairs.First(predicate);
        }
    }
}