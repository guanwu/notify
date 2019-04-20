namespace Guanwu.Notify.Widget.DbContext.SqlServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class baseline : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "v4.message_attributes",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        message_id = c.String(nullable: false, maxLength: 128),
                        creator = c.String(),
                        creation_time = c.Long(nullable: false),
                        key = c.String(),
                        value = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("v4.messages", t => t.message_id, cascadeDelete: true)
                .Index(t => t.message_id);
            
            CreateTable(
                "v4.messages",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        creator = c.String(),
                        creation_time = c.Long(nullable: false),
                        content = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "v4.tasks",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        message_id = c.String(nullable: false, maxLength: 128),
                        creator = c.String(),
                        creation_time = c.Long(nullable: false),
                        effective_time = c.Long(nullable: false),
                        priority = c.Long(nullable: false),
                        executor = c.String(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("v4.messages", t => t.message_id, cascadeDelete: true)
                .Index(t => t.message_id);
            
            CreateTable(
                "v4.requests",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        task_id = c.String(nullable: false, maxLength: 128),
                        creator = c.String(),
                        creation_time = c.Long(nullable: false),
                        content = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("v4.tasks", t => t.task_id, cascadeDelete: true)
                .Index(t => t.task_id);
            
            CreateTable(
                "v4.responses",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        request_id = c.String(nullable: false, maxLength: 128),
                        creator = c.String(),
                        creation_time = c.Long(nullable: false),
                        content = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("v4.requests", t => t.request_id, cascadeDelete: true)
                .Index(t => t.request_id);
            
            CreateTable(
                "v4.exceptions",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        creator = c.String(),
                        creation_time = c.Long(nullable: false),
                        message = c.String(),
                        inner_exception = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("v4.message_attributes", "message_id", "v4.messages");
            DropForeignKey("v4.requests", "task_id", "v4.tasks");
            DropForeignKey("v4.responses", "request_id", "v4.requests");
            DropForeignKey("v4.tasks", "message_id", "v4.messages");
            DropIndex("v4.responses", new[] { "request_id" });
            DropIndex("v4.requests", new[] { "task_id" });
            DropIndex("v4.tasks", new[] { "message_id" });
            DropIndex("v4.message_attributes", new[] { "message_id" });
            DropTable("v4.exceptions");
            DropTable("v4.responses");
            DropTable("v4.requests");
            DropTable("v4.tasks");
            DropTable("v4.messages");
            DropTable("v4.message_attributes");
        }
    }
}
