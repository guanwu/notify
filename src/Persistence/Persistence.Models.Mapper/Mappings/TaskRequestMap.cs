using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class TaskRequestMap : EntityTypeConfiguration<TaskRequest>
    {
        public TaskRequestMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestId);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + nameof(TaskRequest);
            this.ToTable(tableName);

            this.Property(t => t.RequestId)
                .HasColumnOrder(10);

            this.Property(t => t.Content)
                .HasColumnOrder(20);

            this.Property(t => t.CreatedAt)
                .HasColumnOrder(30);

            this.Property(t => t.TaskId)
                .HasColumnOrder(40);

            this.HasRequired(t => t.Task)
                .WithMany(t => t.Requests)
                .HasForeignKey(t => t.TaskId)
                .WillCascadeOnDelete();
        }
    }
}
