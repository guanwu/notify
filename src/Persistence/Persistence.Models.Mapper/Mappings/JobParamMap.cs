using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class JobParamMap : EntityTypeConfiguration<JobParam>
    {
        public JobParamMap()
        {
            // Primary Key
            this.HasKey(t => t.ParamId);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + nameof(JobParam);
            this.ToTable(tableName);

            this.Property(t => t.ParamId)
                .HasColumnOrder(10);

            this.Property(t => t.Name)
                .HasColumnOrder(20);

            this.Property(t => t.Value)
                .HasColumnOrder(30);

            this.Property(t => t.CreatedBy)
                .HasColumnOrder(40);

            this.Property(t => t.CreatedAt)
                .HasColumnOrder(50);

            this.Property(t => t.JobId)
                .HasColumnOrder(60);

            this.HasRequired(t => t.Job)
                .WithMany(t => t.Params)
                .HasForeignKey(t => t.JobId)
                .WillCascadeOnDelete();
        }
    }
}
