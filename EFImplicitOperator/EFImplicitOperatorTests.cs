using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EFImplicitOperator
{
    [TestClass]
    public class EFImplicitOperatorTests
    {
        TestContext NewContext => (new TestContextDesignFactory()).CreateDbContext(null);

        [TestInitialize]
        public async Task OnTestInitialize()
        {
            using (var ctx = NewContext)
            {
                await ctx.Database.EnsureDeletedAsync();
                await ctx.Database.EnsureCreatedAsync();
            }
        }

        [TestMethod]
        public async Task Test()
        {
            var child = new Child()
            {
                Id = Guid.Parse("52a459e2-6d9e-4bf2-a744-1be80a9afef9")
            };

            var childRef = new Ref(child.Id);

            Parent withOptional = null;

            using (var ctx = NewContext)
            {
                await ctx.AddAsync(child);
                int parents = 10000;

                foreach (var p in Enumerable.Range(0, parents))
                {
                    var parent = new Parent()
                    {
                        Id = Guid.NewGuid(),
                        RequiredChildId = childRef, // implicit convertion to Guid
                        OptionalChildId = p == parents - 1 ? childRef : null // implicit convertion to Guid?
                    };

                    if (parent.OptionalChildId.HasValue)
                        withOptional = parent;

                    await ctx.AddAsync(parent);
                }

                await ctx.SaveChangesAsync();
            }

            using (var ctx = NewContext)
            {
                var query = ctx.Parents.Where(p => p.OptionalChildId == childRef);
                var localSql = query.ToSql();
                /* Local sql
                SELECT [p].[Id], [p].[OptionalChildId], [p].[RequiredChildId]
                FROM [Parents] AS [p]
                WHERE [p].[OptionalChildId] = '52a459e2-6d9e-4bf2-a744-1be80a9afef9'
                */

                var parent = query.Single();

                /* Query, captured by SQL Profiler
                SELECT [p].[Id], [p].[OptionalChildId], [p].[RequiredChildId]
                FROM [Parents] AS [p]
                */

                Assert.AreEqual(parent.Id, withOptional.Id);

                /* 
                 * Test passed, by whole dataset is fetched
                 */
            }
        }
    }
}
