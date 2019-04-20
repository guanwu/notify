using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class ScheduledTaskMap : EntityTypeConfiguration<ScheduledTask>
    {
        public ScheduledTaskMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + "tasks";
            this.ToTable(tableName);

            this.Property(t => t.Id)
                .HasColumnOrder(10)
                .HasColumnName("id");

            this.Property(t => t.MessageId)
                .HasColumnOrder(15)
                .HasColumnName("message_id");

            this.Property(t => t.Creator)
                .HasColumnOrder(20)
                .HasColumnName("creator");

            this.Property(t => t.CreationTime)
                .HasColumnOrder(30)
                .HasColumnName("creation_time");

            this.Property(t => t.EffectiveTime)
                .HasColumnOrder(40)
                .HasColumnName("effective_time");

            this.Property(t => t.Priority)
                .HasColumnOrder(50)
                .HasColumnName("priority");

            this.Property(t => t.Executor)
                .HasColumnOrder(60)
                .HasColumnName("executor");

            this.Property(t => t.Status)
                .HasColumnOrder(70)
                .HasColumnName("status");

            this.HasRequired(t => t.Message)
                .WithMany(t => t.ScheduledTasks)
                .HasForeignKey(t => t.MessageId)
                .WillCascadeOnDelete();
        }
    }
}
