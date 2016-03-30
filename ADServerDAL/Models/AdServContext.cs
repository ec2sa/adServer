using System.Data.Entity;

namespace ADServerDAL.Models
{
    public class AdServContext : DbContext
    {
        static AdServContext()
        {
            Database.SetInitializer<AdServContext>(null);
        }

        public AdServContext()
            : base("Name=AdServContext")
        {
        }

	    protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Campaign>().Property(it => it.AdPoints).HasPrecision(12, 6);
			modelBuilder.Entity<Campaign>().Property(it => it.ClickValue).HasPrecision(12, 6);
			modelBuilder.Entity<Campaign>().Property(it => it.ViewValue).HasPrecision(12, 6);
			modelBuilder.Entity<User>().Property(it => it.AdPoints).HasPrecision(12, 6);
	    }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<MultimediaObject> MultimediaObjects { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
