using Microsoft.AspNetCore.Mvc;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;

namespace RepairOpsApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INotesRepository _notesRepository;
    private readonly ICaseRepository _caseRepository;
    
    public NotesController(INotesRepository notesRepository, ICaseRepository caseRepository)
    {
        _notesRepository = notesRepository;
        _caseRepository = caseRepository;
    }
    
    [HttpGet("case/{caseId}")]
    public async Task<IActionResult> Getnotes(Guid caseId)
    {
        //henter besked fra repository ud fra caseId
        var notes = await _notesRepository.GetnotesByCaseIdAsync(caseId);
    
        // Returner en tom liste, hvis der ikke er beskeder
        if (notes == null || !notes.Any())
        {
            return Ok(new List<Notes>()); //returnere 200 med en tom liste (vi gider ikke det rød skrift)
        }

        return Ok(notes); //returnere 200 med en liste af beskeder
    }
    
    // Send ny chatbesked
    [HttpPost("send")]
    public async Task<IActionResult> Sendnote([FromBody] Notes notes)
    {
        //validere input så det ikke er en tom besked og at der er et gyldigt caseId
        if (notes == null || string.IsNullOrEmpty(notes.Message) || notes.CaseId == Guid.Empty)
        {
            return BadRequest("Ugyldige data."); //hvis krav ikke er opfyldt
        }
        //controller om sag eksister 
        var caseExists = await _caseRepository.GetCaseByIdAsync(notes.CaseId);
        if (caseExists == null)
        {
            return NotFound("Sagen findes ikke."); //hvis sag ikke findes
        }
        //tilføjer besked til repository
        await _notesRepository.AddnotesMessageAsync(notes);
        return Ok(notes); //ok status (200)
    }
    
}