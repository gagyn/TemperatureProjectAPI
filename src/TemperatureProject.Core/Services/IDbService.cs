using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TemperatureProject.Core.Services
{
    public interface IDbService
    {
        void Insert(IEnumerable<TemperatureRecord> records);
        IQueryable<TemperatureRecord> GetAll();
        TemperatureRecord GetLast();
        IQueryable<TemperatureRecord> GetWhere(Expression<Func<TemperatureRecord, bool>> where);
        IQueryable<TemperatureRecord> GetBetween(DateTime fromDate, DateTime toDate);
        long CountToNow(DateTime fromDate);
        long CountBetween(DateTime fromDate, DateTime toDate);
        long CountAll();
    }
}