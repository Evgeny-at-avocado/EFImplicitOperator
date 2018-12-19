using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFImplicitOperator
{
    public class Child
    {
        [Key]
        public Guid Id { get; set; }
    }

    public class Parent
    {
        [Key]
        public Guid Id { get; set; }

        public Guid RequiredChildId { get; set; }

        [ForeignKey(nameof(RequiredChildId))]
        public Child RequiredChild { get; set; }

        public Guid? OptionalChildId { get; set; }

        [ForeignKey(nameof(OptionalChildId))]
        public Child OptionalChild { get; set; }
    }

    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Child> Childen { get; set; }
        public DbSet<Parent> Parents { get; set; }
    }

    public class TestContextDesignFactory : IDesignTimeDbContextFactory<TestContext>
    {
        public TestContext CreateDbContext(string[] args)
        {
            var designOptionsBuilder = new DbContextOptionsBuilder<TestContext>();

            var connectionString = "Data Source=.\\SQLEXPRESS;Initial catalog=eftest;Integrated Security=False;User ID=scms;Password=scms;";

            designOptionsBuilder.UseSqlServer(connectionString);

            return new TestContext(designOptionsBuilder.Options);
        }
    }
}
