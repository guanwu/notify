using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class TaskStateMap : EntityTypeConfiguration<TaskState>
    {
        public TaskStateMap()
        {
            // Primary Key
            this.HasKey(t => t.StateId);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + nameof(TaskState);
            this.ToTable(tableName);

            this.Property(t => t.StateId)
                .HasColumnOrder(10);

            this.Property(t => t.StateName)
                .HasColumnOrder(20);

            this.Property(t => t.CreatedAt)
                .HasColumnOrder(30);

            this.Property(t => t.TaskId)
                .HasColumnOrder(40);

            this.HasRequired(t => t.Task)
                .WithMany(t => t.States)
                .HasForeignKey(t => t.TaskId)
                .WillCascadeOnDelete();
        }
    }
}
