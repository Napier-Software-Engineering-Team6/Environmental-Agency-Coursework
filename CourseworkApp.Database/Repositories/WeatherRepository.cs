using CourseworkApp.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseworkApp.Database.Data;


namespace CourseworkApp.Database.Repositories
{
    public class WeatherRepository
    {
        private readonly WeatherDbContext _context;

        public WeatherRepository(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task<List<WeatherSensor>> GetAllWeatherDataAsync()
        {
            return await _context.Weather.ToListAsync();
        }

        public async Task<List<WeatherSensor>> GetWeatherDataByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Weather
                .Where(w => w.Time >= startDate && w.Time <= endDate)
                .OrderBy(w => w.Time)
                .ToListAsync();
        }

        public async Task<WeatherSensor> GetWeatherDataByIdAsync(int id)
        {
            return await _context.Weather.FindAsync(id);
        }

        public async Task AddWeatherDataAsync(WeatherSensor weatherData)
        {
            await _context.Weather.AddAsync(weatherData);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWeatherDataAsync(WeatherSensor weatherData)
        {
            _context.Weather.Update(weatherData);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWeatherDataAsync(int id)
        {
            var weatherData = await _context.Weather.FindAsync(id);
            if (weatherData != null)
            {
                _context.Weather.Remove(weatherData);
                await _context.SaveChangesAsync();
            }
        }
    }
}
