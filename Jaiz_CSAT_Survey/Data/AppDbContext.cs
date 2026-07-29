using Jaiz_CSAT_Survey.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Jaiz_CSAT_Survey.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<SurveyResponse> SurveyResponses { get; set; }

        public DbSet<SurveyAlert> SurveyAlerts { get; set; }

        // Fluent Configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<SurveyResponse>(entity =>
            {
                entity.ToTable("SurveyResponses");

                entity.HasKey(x => x.SurveyId);
                
            });
            modelBuilder.Entity<SurveyAlert>(entity =>
            {
               
                entity.HasKey(x => x.Id);

                entity.HasOne<SurveyResponse>()
                      .WithMany()
                      .HasForeignKey(x => x.SurveyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
