using Xunit;

namespace CourseworkApp.Test;

public class SensorConfigModelTests : IClassFixture<DatabaseSensorFixture>
{
  DatabaseSensorFixture _fixture;
  public SensorConfigModelTests(DatabaseSensorFixture fixture)
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
