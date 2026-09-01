using Microsoft.EntityFrameworkCore;
using MiniCommercial.Data;

namespace MiniCommercial.Tests;

public abstract class TestBase
{
    protected ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Base unique pour chaque test
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}