﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePulse.API.Migrations
{
    /// <inheritdoc />
    public partial class AddingBlogImageDomainModelll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "BlogImages",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "BlogImages",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "BlogImages",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BlogImages",
                newName: "id");
        }
    }
}
