using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

// https://github.com/aspnet/EntityFrameworkCore/issues/6482#issuecomment-399061388
// With some midifications - drop instance type check

namespace EFImplicitOperator
{
    public static class IQueryableHelper
    {
        private static readonly FieldInfo _queryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_queryCompiler");

        private static readonly TypeInfo _queryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo _queryModelGeneratorField = _queryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_queryModelGenerator");

        private static readonly FieldInfo _databaseField = _queryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo _dependenciesProperty = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static string ToSql<TEntity>(this IQueryable<TEntity> queryable)
            where TEntity : class
        {
            var queryCompiler = (IQueryCompiler)_queryCompilerField.GetValue(queryable.Provider);
            var queryModelGenerator = (IQueryModelGenerator)_queryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = queryModelGenerator.ParseQuery(queryable.Expression);
            var database = _databaseField.GetValue(queryCompiler);
            var queryCompilationContextFactory = ((DatabaseDependencies)_dependenciesProperty.GetValue(database)).QueryCompilationContextFactory;
            var queryCompilationContext = queryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            return modelVisitor.Queries.Join(Environment.NewLine + Environment.NewLine);
        }
    }
}