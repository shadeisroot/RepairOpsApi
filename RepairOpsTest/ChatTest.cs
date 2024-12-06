using Microsoft.AspNetCore.Mvc;
using Moq;
using RepairOpsApi.Controllers;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;

namespace RepairOpsTest;

[TestFixture]
public class ChatTest
{
    private Mock<IChatRepository> _chatRepositoryMock;
    private Mock<ICaseRepository> _caseRepositoryMock;
    private ChatController _controller;
    
    [SetUp]
    public void SetUp()
    {
        _chatRepositoryMock = new Mock<IChatRepository>();
        _caseRepositoryMock = new Mock<ICaseRepository>();
        _controller = new ChatController(_chatRepositoryMock.Object, _caseRepositoryMock.Object);
    }
    
    
    [Test]
    public async Task GetChatMessages_ReturnsChatMessages_WhenMessagesExist()
    {
        //Arrange med case id
        var caseId = Guid.NewGuid();
        //list med beskeder
        var chatMessages = new List<ChatMessage>
        {
            new ChatMessage { Id = Guid.NewGuid(), CaseId = caseId, Message = "Hello", Sender = "Kunde" },
            new ChatMessage { Id = Guid.NewGuid(), CaseId = caseId, Message = "Hi", Sender = "Technician" }
        };
        //mocker
        _chatRepositoryMock.Setup(repo => repo.GetChatsByCaseIdAsync(caseId))
            .ReturnsAsync(chatMessages);

        //Act kald til controller metode
        var result = await _controller.GetChatMessages(caseId);

        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result); //resultetet er type ok (ved ikke hvad det betyder)
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult); //ikke er null

        var returnedMessages = okResult.Value as List<ChatMessage>; //tjekker om beskeder matcher
        Assert.AreEqual(2, returnedMessages.Count); //om der er 2
    }
    
    [Test]
    public async Task GetChatMessages_ReturnsEmptyList_WhenNoMessagesExist()
    {
        //Arrange caseid uden at tilknytte beskeder
        var caseId = Guid.NewGuid();
        
        //mock repository til at give en tom liste
        _chatRepositoryMock.Setup(repo => repo.GetChatsByCaseIdAsync(caseId))
            .ReturnsAsync(new List<ChatMessage>());

        //Act
        var result = await _controller.GetChatMessages(caseId);

        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result); // det der ok object noget
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult); //ikke er null

        var returnedMessages = okResult.Value as List<ChatMessage>;
        Assert.IsEmpty(returnedMessages); //kigger om listen er tom
    }
    
    [Test]
    public async Task SendMessage_ReturnsOk_WhenMessageIsValid()
    {
        //Arrange med chat og case 
        var chatMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            CaseId = Guid.NewGuid(),
            Sender = "Kunde",
            Message = "Hello"
        };
        
        //mock til at sende case
        _caseRepositoryMock.Setup(repo => repo.GetCaseByIdAsync(chatMessage.CaseId))
            .ReturnsAsync(new Case { Id = chatMessage.CaseId });
        
        //mock til at accepter og gemme chat
        _chatRepositoryMock.Setup(repo => repo.AddChatMessageAsync(chatMessage))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _controller.SendMessage(chatMessage);

        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result); //ok noget igen
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult); //ikke null

        var returnedMessage = okResult.Value as ChatMessage;
        Assert.AreEqual(chatMessage.Message, returnedMessage.Message); //den beskeds resultat er det samme som den der bliver sendt
    }
    
    
    [TestCase("")] //tom besked
    public async Task SendMessage_ReturnsBadRequest_WhenMessageIsNullOrEmpty(string messageContent)
    {
        //Arrange besked med null eller ingenting
        var chatMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            CaseId = Guid.NewGuid(),
            Sender = "Kunde",
            Message = messageContent
        };

        //Act
        var result = await _controller.SendMessage(chatMessage);

        //Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result); // resultat er bad request
    }
    
    [Test]
    public async Task SendMessage_ReturnsNotFound_WhenCaseDoesNotExist()
    {
        // Arrange laver en chat, men ingen case som findes
        var chatMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            CaseId = Guid.NewGuid(),
            Sender = "Kunde",
            Message = "Hello"
        };
        //mock returnere en case som ikke findes eller er null
        _caseRepositoryMock.Setup(repo => repo.GetCaseByIdAsync(chatMessage.CaseId))
            .ReturnsAsync((Case)null); // Simuler manglende case

        // Act 
        var result = await _controller.SendMessage(chatMessage);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result); //resultat er ikke fundet
    }
    
}