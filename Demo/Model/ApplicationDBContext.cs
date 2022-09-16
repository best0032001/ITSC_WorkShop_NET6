using Microsoft.EntityFrameworkCore;

namespace Demo.Model
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
     : base(options)
        {
        }
    }
}
