using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfluenceRecruitmentTest.Data
{
    public class StockRepository : IStockRepository
    {
        private readonly StockContext _context;
        private readonly ILogger<StockRepository> _logger;

        public StockRepository(StockContext context, ILogger<StockRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempitng to save the changes in the context");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<double> GetPortfolioCountAsync()
        {
            _logger.LogInformation($"Getting all Stocks from db and fetching Portfolio count");
            //var query = _context.Stocks.ToAsyncEnumerable().Sum(row => row.Price * row.Quantity);
            var query = _context.Stocks.ToList().Sum(row => row.Price * row.Quantity);
            return query;
        }
    }
}
