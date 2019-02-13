using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MeteostationService
{
    public class StatisticsService
    {
        private MyDbContext _context;

        public StatisticsService(
            MyDbContext context
        )
        {
            _context = context;
        }

        
        public void WriteStatistics(string temp, string humidity, DateTime measurementTime, ApplicationUser user)
        {
            _context.Stats.Add(new Stats() { Temperature = temp, Humidity = humidity, MeasurementTime = measurementTime, User = user });
            _context.SaveChanges();
        }
        
        public List<StatisticsResult> GetStatistics(DateTime startTime, DateTime endTime, string userName, double sampleSize = 0.1)
        {
            if ((sampleSize > 1) || (sampleSize <= 0))
                throw new Exception("Invalid sample size. Must be between 0 and 1");

            var res = (from stat in _context.Stats
            where 
                stat.User.UserName == userName && 
                stat.MeasurementTime >= startTime &&
                stat.MeasurementTime <= endTime
            orderby stat.MeasurementTime descending
            select new StatisticsResult() { 
                Humidity = stat.Humidity,
                Temperature = stat.Temperature,
                MeasurementTime = stat.MeasurementTime 
            }).ToList();

            // Count every nth row for smaller output size, or get every result if its count is less then 10
            var nth = res.Count > 10 ? Math.Ceiling(res.Count / (sampleSize * 100)) : 1;
            
            return res
                .Select((s, i) => new { s = s, Index = i})
                .Where(s => s.Index % nth == 0)
                .Select(s => s.s).ToList();
        }

        public List<UserInfo> GetUsers()
        {   
            return (from stat in _context.Stats
                group stat.MeasurementTime by stat.User into mt
                select new UserInfo { 
                    Username = mt.Key.UserName, 
                    MaxDate = mt.Max(), 
                    MinDate = mt.Min() 
                })
                .ToList();
        }
    }
}