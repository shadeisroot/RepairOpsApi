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
    
    [Test]
    public async Task DeleteCase_RemovesCase_WhenCaseExists()
    {
        // Arrange
        var caseId = Guid.NewGuid();
        var existingCase = new Case { Id = caseId, CustomerName = "Jacob" };

        _mockRepository.Setup(repo => repo.GetCaseByIdAsync(caseId)).ReturnsAsync(existingCase); // Mock GetCaseByIdAsync
        _mockRepository.Setup(repo => repo.DeleteCaseAsync(caseId)).ReturnsAsync(true); // Mock DeleteCaseAsync

        // Act
        var result = await _controller.DeleteCase(caseId);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result); // Sletningen skal returnere NoContent
        _mockRepository.Verify(repo => repo.GetCaseByIdAsync(caseId), Times.Once); // Bekræft, at GetCaseByIdAsync blev kaldt
        _mockRepository.Verify(repo => repo.DeleteCaseAsync(caseId), Times.Once);
    }
    
    [Test]
    public async Task DeleteCase_ReturnsNotFound_WhenCaseDoesNotExist()
    {
        // Arrange
        var caseId = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.GetCaseByIdAsync(caseId)).ReturnsAsync((Case)null); // Mock returnerer null

        // Act
        var result = await _controller.DeleteCase(caseId);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result); // Sørg for, at resultatet er NotFound
        _mockRepository.Verify(repo => repo.GetCaseByIdAsync(caseId), Times.Once); // Bekræft, at GetCaseByIdAsync blev kaldt
        _mockRepository.Verify(repo => repo.DeleteCaseAsync(It.IsAny<Guid>()), Times.Never);
    }
    
    [Test]
public async Task GetStatusHistory_ReturnsHistory_WhenHistoryExists()
{
    // Arrange
    var caseId = Guid.NewGuid();
    var statusHistory = new List<StatusHistory>
    {
        new StatusHistory { Id = Guid.NewGuid(), CaseId = caseId, OldStatus = "Modtaget", NewStatus = "Under reparation", ChangedAt = DateTime.UtcNow.AddDays(-1) },
        new StatusHistory { Id = Guid.NewGuid(), CaseId = caseId, OldStatus = "Under reparation", NewStatus = "Afventer dele", ChangedAt = DateTime.UtcNow }
    };

    _mockRepository.Setup(repo => repo.GetStatusHistoryByCaseIdAsync(caseId)).ReturnsAsync(statusHistory);

    // Act
    var result = await _controller.GetStatusHistory(caseId);

    // Assert
    var okResult = result as OkObjectResult;
    Assert.IsNotNull(okResult); // Resultatet skal være OK
    var returnedHistory = okResult.Value as IEnumerable<StatusHistory>;
    Assert.IsNotNull(returnedHistory); // Resultatet må ikke være null
    Assert.AreEqual(2, returnedHistory.Count()); // Sørg for, at begge poster blev returneret
    Assert.AreEqual("Modtaget", returnedHistory.First().OldStatus); // Den første statusændring
    Assert.AreEqual("Afventer dele", returnedHistory.Last().NewStatus); // Den sidste statusændring
}

[Test]
public async Task GetStatusHistory_ReturnsNotFound_WhenNoHistoryExists()
{
    // Arrange
    var caseId = Guid.NewGuid();
    _mockRepository.Setup(repo => repo.GetStatusHistoryByCaseIdAsync(caseId)).ReturnsAsync(new List<StatusHistory>());

    // Act
    var result = await _controller.GetStatusHistory(caseId);

    // Assert
    Assert.IsInstanceOf<NotFoundObjectResult>(result); // Sørg for, at resultatet er NotFound
    _mockRepository.Verify(repo => repo.GetStatusHistoryByCaseIdAsync(caseId), Times.Once); // Bekræft, at metoden blev kaldt
}

}