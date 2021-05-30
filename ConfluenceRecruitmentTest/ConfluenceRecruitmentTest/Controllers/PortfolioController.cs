using AutoMapper;
using ConfluenceRecruitmentTest.Data;
using ConfluenceRecruitmentTest.Data.Entities;
using ConfluenceRecruitmentTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfluenceRecruitmentTest.Controllers
{
    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly IStockRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(IStockRepository repository, IMapper mapper, ILogger<PortfolioController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger; ;
        }

        [HttpPost]
        public async Task<ActionResult<PortfolioModel>> Post(StockModel model)
        {
            try
            {
                PortfolioModel portfolioModel = new PortfolioModel();
                // Create a new Stock
                var stock = _mapper.Map<Stock>(model);
                _repository.Add(stock);
                if (await _repository.SaveChangesAsync())
                {
                    var totalValue = await _repository.GetPortfolioCountAsync();
                    portfolioModel.TotalValue = totalValue;

                    return Ok(portfolioModel);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in processing Stocks : {ex} ");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }
    }
}
