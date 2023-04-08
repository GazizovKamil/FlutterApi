using Microsoft.EntityFrameworkCore;

namespace FlutterApi.Data
{
    public class FlutterApiDB : DbContext
    {
        public FlutterApiDB(DbContextOptions options) : base(options){ }

        public DbSet<User> Users { get; set; }

    }
}
