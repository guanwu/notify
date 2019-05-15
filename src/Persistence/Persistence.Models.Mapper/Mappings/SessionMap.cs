using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class SessionMap : EntityTypeConfiguration<Session>
    {
        public SessionMap()
        {
            // Primary Key
            this.HasKey(t => t.SessionId);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + nameof(Session);
            this.ToTable(tableName);

            this.Property(t => t.SessionId)
                .HasColumnOrder(10);

            this.Property(t => t.AppId)
                .HasColumnOrder(20);

            this.Property(t => t.CreatedAt)
                .HasColumnOrder(30);
        }
    }
}
