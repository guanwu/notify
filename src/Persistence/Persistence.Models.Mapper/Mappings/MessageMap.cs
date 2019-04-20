using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + "messages";
            this.ToTable(tableName);

            this.Property(t => t.Id)
                .HasColumnOrder(10)
                .HasColumnName("id");

            this.Property(t => t.Creator)
                .HasColumnOrder(20)
                .HasColumnName("creator");

            this.Property(t => t.CreationTime)
                .HasColumnOrder(30)
                .HasColumnName("creation_time");

            this.Property(t => t.Content)
                .HasColumnOrder(40)
                .HasColumnName("content");
        }
    }
}
