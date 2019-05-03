using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class JobMap : EntityTypeConfiguration<Job>
    {
        public JobMap()
        {
            // Primary Key
            this.HasKey(t => t.JobId);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + nameof(Job);
            this.ToTable(tableName);

            this.Property(t => t.JobId)
                .HasColumnOrder(10);

            this.Property(t => t.Content)
                .HasColumnOrder(20);

            this.Property(t => t.CreatedAt)
                .HasColumnOrder(30);
        }
    }
}
