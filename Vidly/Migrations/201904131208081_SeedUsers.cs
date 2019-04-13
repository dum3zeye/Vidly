namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'44f77604-715f-471b-b314-220c672c1af5', N'guest@vidly.com', 0, N'ANVR1johLuyxCcgH+4yAy9CpyChrTF/hzUMwWI/Awjo5a0Edr/RSsOkkjBZXjpaJjw==', N'dd1c89fb-7959-4edc-9748-f05cf2de87c8', NULL, 0, 0, NULL, 1, 0, N'guest@vidly.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'8bb089ce-78c0-4d9b-8ca9-48dcb65b6fdb', N'admin@vidly.com', 0, N'AGTmUHraz2mYRCqMdpI3XQ5yIR75YGYka0RhdMCcWEPkfz4DesP21pmL7bm4fbp0MA==', N'06b8e3a7-e1fa-4dd1-9e04-b8cfe197339e', NULL, 0, 0, NULL, 1, 0, N'admin@vidly.com')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'08d79c4d-58c2-4421-a2de-beb6da7b3b15', N'CanManageMovies')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'8bb089ce-78c0-4d9b-8ca9-48dcb65b6fdb', N'08d79c4d-58c2-4421-a2de-beb6da7b3b15')
");
        }

        public override void Down()
        {
        }
    }
}