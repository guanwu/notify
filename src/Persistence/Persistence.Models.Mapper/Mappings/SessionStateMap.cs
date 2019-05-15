using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class SessionStateMap : EntityTypeConfiguration<SessionState>
    {
        public SessionStateMap()
        {
            // Primary Key
            this.HasKey(t => t.StateId);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + nameof(SessionState);
            this.ToTable(tableName);

            this.Property(t => t.StateId)
                .HasColumnOrder(10);

            this.Property(t => t.StateName)
                .HasColumnOrder(20);

            this.Property(t => t.CreatedAt)
                .HasColumnOrder(30);

            this.Property(t => t.SessionId)
                .HasColumnOrder(40);

            this.HasRequired(t => t.Session)
                .WithMany(t => t.States)
                .HasForeignKey(t => t.SessionId)
                .WillCascadeOnDelete();
        }
    }
}
