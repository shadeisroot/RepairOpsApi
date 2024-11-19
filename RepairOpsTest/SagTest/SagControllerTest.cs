using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RepairOpsApi;
using RepairOpsApi.Controller;
using RepairOpsApi.DataAccess;
using RepairOpsApi.Entities;


namespace RepairOpsTest.SagTest
{
    [TestFixture]
    public class SagControllerTest
    {
        private Mock<ISagRepository> _mockSagRepository;
        private SagController _controller;

        [SetUp]
        public void Setup()
        {
            _mockSagRepository = new Mock<ISagRepository>();
            _controller = new SagController(_mockSagRepository.Object);
        }
        
        [Test]
        public async Task SagCreateTest()
        {
            // Arrange
            var newSag = new Sag { Sagsnummer = Guid.Empty };

            _mockSagRepository.Setup(r => r.SagExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockSagRepository.Setup(r => r.CreateSagAsync(It.IsAny<Sag>())).ReturnsAsync(newSag);

            // Act
            var result = await _controller.CreateSag(newSag);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            var returnedSag = createdResult.Value as Sag;
            Assert.IsNotNull(returnedSag);
            Assert.AreNotEqual(Guid.Empty, returnedSag.Sagsnummer); 
        }
        
        [Test]
        public async Task SagCreateConflictTest()
        {
            // Arrange
            var existingSag = new Sag { Sagsnummer = Guid.NewGuid() };
            _mockSagRepository.Setup(r => r.SagExistsAsync(existingSag.Sagsnummer)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateSag(existingSag);

            // Assert
            var conflictResult = result.Result as ConflictObjectResult;
            Assert.IsNotNull(conflictResult);
            Assert.AreEqual(409, conflictResult.StatusCode);
        }
        
        [Test]
        public async Task SagGetTest()
        {
            // Arrange
            var existingSag = new Sag { Sagsnummer = Guid.NewGuid() };
            _mockSagRepository.Setup(r => r.GetSagAsync(existingSag.Sagsnummer)).ReturnsAsync(existingSag);

            // Act
            var result = await _controller.GetSag(existingSag.Sagsnummer);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedSag = okResult.Value as Sag;
            Assert.AreEqual(existingSag.Sagsnummer, returnedSag.Sagsnummer);
        }
        
        [Test]
        public async Task SagGetNotFoundTest()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            _mockSagRepository.Setup(r => r.GetSagAsync(nonExistentId)).ReturnsAsync((Sag)null);

            // Act
            var result = await _controller.GetSag(nonExistentId);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}