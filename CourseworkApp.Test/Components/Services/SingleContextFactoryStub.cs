using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Data; // Contains TestDbContext

namespace CourseworkApp.Test.Components.Services
{
  public class SingleContextFactoryStub : IDbContextFactory<TestDbContext>
  {
    private readonly TestDbContext _context;

    public SingleContextFactoryStub(TestDbContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public TestDbContext CreateDbContext() => _context;

    public Task<TestDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_context);
  }
}