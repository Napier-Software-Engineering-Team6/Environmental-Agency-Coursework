using CourseworkApp.Database.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseworkApp.Services
{
  public interface ISensorReadingService
  {
    /// <summary>
    /// Retrieves recent sensor readings including their location data.
    /// </summary>
    /// <param name="lookbackPeriod">The time duration to look back for recent readings.</param>
    /// <returns>A collection of recent sensor reading data transfers.</returns>
    Task<IEnumerable<SensorReadings>> GetRecentReadingsWithLocationAsync(TimeSpan lookbackPeriod);
  }
}