using AutoMapper;
using ConfluenceRecruitmentTest.Controllers;
using ConfluenceRecruitmentTest.Data;
using ConfluenceRecruitmentTest.Data.Entities;
using ConfluenceRecruitmentTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ConfluenceRecruitmentTest.Tests
{
    public class PortfolioController_test
    {
        [Theory]
        [InlineData("Microsoft", 1, 12.00, 12.00,1,1,1)]
        [InlineData("Amazon", 2, 14.00, 28.00,1,1,1)]
        public async void Post_Test(string name, int quantity, 
            double price, double totalValue,int addCall,
            int savecall,int getcall)
        {

            //Arrange
            StockModel stock = new StockModel()
            {
                Name = name,
                Quantity = quantity,
                Price = price
            };
            PortfolioModel portfolioModel = new PortfolioModel()
            {
                TotalValue = totalValue
            };
            var portfoliocontrollerlogger = Mock.Of<ILogger<PortfolioController>>();
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new StockProfile());
            });
            var mapper = mockMapper.CreateMapper();
            Mock<IStockRepository> stockRepositoryFactoryMock = new Mock<IStockRepository>();
            stockRepositoryFactoryMock.Setup(obj => obj.Add(stock));
            stockRepositoryFactoryMock.Setup(obj => obj.SaveChangesAsync())
                .ReturnsAsync(true);
            stockRepositoryFactoryMock.Setup(obj => obj.GetPortfolioCountAsync())
                .ReturnsAsync(totalValue);
            //Act
            PortfolioController controller = new PortfolioController(stockRepositoryFactoryMock.Object, mapper, portfoliocontrollerlogger);
            var result =await controller.Post(stock);

            //Assert
            var portfolioresult = Assert.IsType<ActionResult<PortfolioModel>>(result);
            var OkActionResult = Assert.IsType<OkObjectResult>(portfolioresult.Result);
            var returnValue = Assert.IsType<PortfolioModel>(OkActionResult.Value);
            Assert.Equal(portfolioModel.TotalValue,returnValue.TotalValue);
            stockRepositoryFactoryMock.Verify(obj => obj.Add(It.IsAny<Stock>()), Times.Exactly(addCall));
            stockRepositoryFactoryMock.Verify(obj => obj.SaveChangesAsync(), Times.Exactly(savecall));
            stockRepositoryFactoryMock.Verify(obj => obj.GetPortfolioCountAsync(), Times.Exactly(getcall));
            stockRepositoryFactoryMock.VerifyNoOtherCalls();

        }
    }
}
