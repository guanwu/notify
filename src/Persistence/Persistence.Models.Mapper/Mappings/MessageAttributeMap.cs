using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class MessageAttributeMap : EntityTypeConfiguration<MessageAttribute>
    {
        public MessageAttributeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + "message_attributes";
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

            this.Property(t => t.Key)
                .HasColumnOrder(40)
                .HasColumnName("key");

            this.Property(t => t.Value)
                .HasColumnOrder(50)
                .HasColumnName("value");

            this.HasRequired(t => t.Message)
                .WithMany(t => t.Attributes)
                .HasForeignKey(t => t.MessageId)
                .WillCascadeOnDelete();
        }
    }
}
