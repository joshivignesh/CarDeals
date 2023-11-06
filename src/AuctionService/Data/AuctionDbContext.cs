using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions options) : base(options)
        {
        }

        public AuctionDbContext()
        {
        }

        public DbSet<Auction> Auctions { get; set; }

    }
}
