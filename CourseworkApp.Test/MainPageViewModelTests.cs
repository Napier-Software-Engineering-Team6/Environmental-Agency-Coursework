using Xunit;

namespace CourseworkApp.Test;

public class MainPageViewModelTests : IClassFixture<DatabaseFixture>
{
  DatabaseFixture _fixture;
  public MainPageViewModelTests(DatabaseFixture fixture)
  {
    _fixture = fixture;
    _fixture.SeedData();
  }

  [Fact]
  public void Test1()
  {
    Assert.True(true, "This test is always true.");
  }
}
