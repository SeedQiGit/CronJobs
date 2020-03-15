using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CronJobsMysql.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cron_job",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CreateUser = table.Column<long>(type: "bigint(20)", nullable: false, defaultValueSql: "'0'"),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<long>(type: "bigint(20)", nullable: false, defaultValueSql: "'0'"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "''"),
                    Description = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "''"),
                    CronExpress = table.Column<string>(type: "varchar(400)", nullable: false, defaultValueSql: "''"),
                    JobState = table.Column<long>(type: "bigint(20)", nullable: false, defaultValueSql: "'0'"),
                    RequestUrl = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "''")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cron_job", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cron_job");
        }
    }
}
