using Microsoft.AspNetCore.Mvc;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;

namespace RepairOpsApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatRepository _chatRepository;
    private readonly ICaseRepository _caseRepository;
    
    
    public ChatController(IChatRepository chatRepository, ICaseRepository caseRepository)
    {
        _chatRepository = chatRepository;
        _caseRepository = caseRepository;
    }
    
    // Hent chatbeskeder for en sag
    [HttpGet("case/{caseId}")]
    public async Task<IActionResult> GetChatMessages(Guid caseId)
    {
        //henter besked fra repository ud fra caseId
        var chatMessages = await _chatRepository.GetChatsByCaseIdAsync(caseId);
    
        // Returner en tom liste, hvis der ikke er beskeder
        if (chatMessages == null || !chatMessages.Any())
        {
            return Ok(new List<ChatMessage>()); //returnere 200 med en tom liste (vi gider ikke det rød skrift)
        }

        return Ok(chatMessages); //returnere 200 med en liste af beskeder
    }
    
    // Send ny chatbesked
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatMessage chatMessage)
    {
        //validere input så det ikke er en tom besked og at der er et gyldigt caseId
        if (chatMessage == null || string.IsNullOrEmpty(chatMessage.Message) || chatMessage.CaseId == Guid.Empty)
        {
            return BadRequest("Ugyldige data."); //hvis krav ikke er opfyldt
        }
        //controller om sag eksister 
        var caseExists = await _caseRepository.GetCaseByIdAsync(chatMessage.CaseId);
        if (caseExists == null)
        {
            return NotFound("Sagen findes ikke."); //hvis sag ikke findes
        }
        //tilføjer besked til repository
        await _chatRepository.AddChatMessageAsync(chatMessage);
        return Ok(chatMessage); //ok status (200)
    }
    
    
}