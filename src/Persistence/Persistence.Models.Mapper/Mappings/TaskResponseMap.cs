﻿using System.Data.Entity.ModelConfiguration;

namespace Guanwu.Notify.Persistence.Models.Mapper
{
    public class TaskResponseMap : EntityTypeConfiguration<TaskResponse>
    {
        public TaskResponseMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestId);

            // Table & Column Mappings
            string tableName = Const.TABLE_PREFIX + nameof(TaskResponse);
            this.ToTable(tableName);

            this.Property(t => t.RequestId)
                .HasColumnOrder(10);

            this.Property(t => t.Content)
                .HasColumnOrder(20);

            this.Property(t => t.CreatedAt)
                .HasColumnOrder(30);

            this.HasRequired(t => t.Request)
                .WithRequiredDependent(t => t.Response)
                .WillCascadeOnDelete();
        }
    }
}
