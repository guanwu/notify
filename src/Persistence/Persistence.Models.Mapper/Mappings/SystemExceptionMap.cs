using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class SystemExceptionMap : EntityTypeConfiguration<SystemException>
    {
        public SystemExceptionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + "exceptions";
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

            this.Property(t => t.Message)
                .HasColumnOrder(40)
                .HasColumnName("message");

            this.Property(t => t.InnerException)
                .HasColumnOrder(50)
                .HasColumnName("inner_exception");
        }
    }
}
