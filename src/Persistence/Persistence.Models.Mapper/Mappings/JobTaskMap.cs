using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class JobTaskMap : EntityTypeConfiguration<JobTask>
    {
        public JobTaskMap()
        {
            // Primary Key
            this.HasKey(t => t.TaskId);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + nameof(JobTask);
            this.ToTable(tableName);

            this.Property(t => t.TaskId)
                .HasColumnOrder(10);

            this.Property(t => t.TaskName)
                .HasColumnOrder(20);

            this.Property(t => t.CreatedAt)
                .HasColumnOrder(30);

            this.Property(t => t.JobId)
                .HasColumnOrder(40);

            this.HasRequired(t => t.Job)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.JobId)
                .WillCascadeOnDelete();
        }
    }
}
