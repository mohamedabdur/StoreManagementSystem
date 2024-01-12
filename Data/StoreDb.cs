using Microsoft.EntityFrameworkCore;
using practice.Models;

namespace practice.Data;
    public class StoreMS:DbContext
{
    public StoreMS(DbContextOptions<StoreMS>options):base(options)
    {
        
    }
    public DbSet<Product> Products{get; set;}

    public DbSet<SellProduct> CustomerRequest{get; set;}


}
