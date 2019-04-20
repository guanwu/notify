using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class ServiceRequestMap : EntityTypeConfiguration<ServiceRequest>
    {
        public ServiceRequestMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + "requests";
            this.ToTable(tableName);

            this.Property(t => t.Id)
                .HasColumnOrder(10)
                .HasColumnName("id");

            this.Property(t => t.TaskId)
                .HasColumnOrder(15)
                .HasColumnName("task_id");

            this.Property(t => t.Creator)
                .HasColumnOrder(20)
                .HasColumnName("creator");

            this.Property(t => t.CreationTime)
                .HasColumnOrder(30)
                .HasColumnName("creation_time");

            this.Property(t => t.Content)
                .HasColumnOrder(40)
                .HasColumnName("content");
            
            this.HasRequired(t => t.Task)
                .WithMany(t => t.ServiceRequests)
                .HasForeignKey(t => t.TaskId)
                .WillCascadeOnDelete();
        }
    }
}
