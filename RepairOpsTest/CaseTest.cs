using Microsoft.AspNetCore.Mvc;
using Moq;
using RepairOpsApi.Controllers;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;

namespace RepairOpsTest;

[TestFixture]
public class CaseTest
{
    private Mock<ICaseRepository> _mockRepository;
    private CaseController _controller;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<ICaseRepository>();
        _controller = new CaseController(_mockRepository.Object);
    }

    [Test]
    public async Task GetCases_Returns_AllCases()
    {
        //Arrange
        var cases = new List<Case>
        {
            new Case { Id = Guid.NewGuid(), CustomerName = "Jacob" },
            new Case { Id = Guid.NewGuid(), CustomerName = "Jack" },
            new Case { Id = Guid.NewGuid(), CustomerName = "Dennis" }
        };
        _mockRepository.Setup(repo => repo.GetAllCasesAsync()).ReturnsAsync(cases);
        
        //Act
        var result = await _controller.GetCases();
        
        //Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult); //resultat er ok
        var returnedCases = okResult.Value as IEnumerable<Case>;
        Assert.IsNotNull(returnedCases); //retunere cases
        Assert.AreEqual(3, returnedCases.Count()); //antal af case
        Assert.AreEqual("Jacob", returnedCases.First().CustomerName); //første er jacob
        Assert.AreEqual("Dennis", returnedCases.Last().CustomerName);//sidste er dennis
    }
    [Test]
    public async Task GetCase_ReturnsNotFound_WhenNoCase()
    {
        //Arrange
        _mockRepository.Setup(repo => repo.GetCaseByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Case)null);

        //Act
        var result = await _controller.GetCase(Guid.NewGuid());

        //Assert
        Assert.IsInstanceOf<NotFoundResult>(result.Result); //result er ikke fundet da den er null
    }
    [Test]
    public async Task PostCase_AddNewCase()
    {
        //Arrange
        var newCase = new Case { Id = Guid.NewGuid(), CustomerName = "Jacob" };
        _mockRepository.Setup(repo => repo.AddCaseAsync(newCase)).ReturnsAsync(newCase);
        
        //Act
        var result = await _controller.PostCase(newCase);
        
        
        //Assert
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        Assert.IsNotNull(createdAtActionResult); //ikke null
        var returnedCase = createdAtActionResult.Value as Case;
        Assert.AreEqual(newCase.Id, returnedCase.Id); //de to id matcher
    }
    
}