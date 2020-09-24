using Microsoft.EntityFrameworkCore;
using TestSolution.Model;

namespace TestSolution.Context
{
    public sealed class WordContext : DbContext
    {
        public DbSet<Word> Words { get; set; }

        public WordContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;UserId=root;Password=root;database=wordsdb;");
        }
    }
}