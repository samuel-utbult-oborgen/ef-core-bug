using System;
using System.Linq;
using EfCoreBug;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

// The database instance is expected to run on the default port 1433 and have
// the sa password "P4ssword", but this can be changed without affecting the
// example itself.
var dbContext = new TestDBContext(new DbContextOptionsBuilder<TestDBContext>()
    .UseSqlServer("Server=localhost,1433;User Id=sa;Password=P4ssword;"
        + "TrustServerCertificate=true")
    .Options);

var wrongResult = await Enumerable
    .Range(1, 2)
    .Select(index => dbContext
        .Set<TestEntity>()
        .FromSqlRaw(
            "SELECT @date AS Date",
            new SqlParameter("date", new DateOnly(2024, 1, index))))
    .Aggregate(Queryable.Union)
    .ToListAsync();

Console.WriteLine("Wrong result:");
foreach (var line in wrongResult)
{
    Console.WriteLine(line.Date);
}

var correctResult = await Enumerable
    .Range(1, 2)
    .Select(index => dbContext
        .Set<TestEntity>()
        .FromSqlRaw(
            "SELECT {0} AS Date",
            new DateOnly(2024, 1, index)))
    .Aggregate(Queryable.Union)
    .ToListAsync();

Console.WriteLine("Correct result:");
foreach (var line in correctResult)
{
    Console.WriteLine(line.Date);
}
