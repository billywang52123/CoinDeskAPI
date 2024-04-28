using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoinDeskAPI.Controllers;
using CoinDeskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TestCoinDesk
{
    public class CoinDeskControllerTests
    {
        private CoinDeskController _controller;
        private CurrencyController _currencyController;

        [SetUp]
        public void Setup()
        {
            // 在這裡初始化你的Controller，你可能需要模擬一些相依性
            var context = new CurrencyContext();
            var _currencyDictionary = new CurrencyDictionary();
            _controller = new CoinDeskController(context, _currencyDictionary);
            _currencyController = new CurrencyController(context);
        }

        [Test]
        public async Task GetTransformedDataAndSet_ReturnsOkResult()
        {
            // Arrange

            // Act
            var result = await _controller.GetTransformedDataAndSet();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is OkObjectResult);
        }



        [Test]
        public async Task PostCurrencyItem_ReturnsCreatedAtAction()
        {
            // Arrange
            var currencyItem = new PostCurrencyItem
            {
                Currency = "BSD",
                Symbol = "$",
                Rate = "1",
                Description = "United States Dollar",
                Rate_float = 1.0,
                Chinese = "美元",
                Japanese = "ドル",
                English = "Dollar"
            };

            // Act
            var result = await _currencyController.PostCurrencyItem(currencyItem);

            // Assert
            Assert.IsInstanceOf<ActionResult<PostCurrencyItem>>(result);
        }

        [Test]
        public async Task GetAllCurrency_ReturnsCorrectType()
        {
            // Act
            var result = await _currencyController.GetAllCurrency();

            // Assert
            Assert.IsInstanceOf<ActionResult<IEnumerable<CurrencyItem>>>(result);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public async Task GetCurrency_ById_WithValidId_ReturnsCorrectType()
        {
            // Arrange
            int validId = 1;

            // Act
            var result = await _currencyController.GetCurrency_ById(validId);

            // Assert
            Assert.IsInstanceOf<ActionResult<CurrencyItem>>(result);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public async Task PutCurrencyItem_ReturnsNoContentResult()
        {
            // Arrange
            int existingId = 17;
            var currencyItem = new CurrencyItem
            {
                Id = existingId,
                Currency = "USD",
                Symbol = "$",
                Rate = "100",
                Description = "Test currency",
                Rate_float = 100.0,
                Chinese = "美元",
                Japanese = "ドル",
                English = "Dollar"
            };

            // Act
            var result = await _currencyController.PutCurrencyItem(existingId, currencyItem);

            // Assert
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result is NoContentResult);
        }

        [Test]
        public async Task DeleteCurrency_ReturnsNoContentResult()
        {
            // Arrange
            int existingId = 17;

            // Act
            var result = await _currencyController.DeleteCurrency(existingId);

            // Assert
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result is NoContentResult);
        }
    }
}