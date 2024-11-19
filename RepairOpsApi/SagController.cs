using Microsoft.AspNetCore.Mvc;
using RepairOpsApi.DataAccess;
using RepairOpsApi.Entities;

namespace RepairOpsApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class SagController : ControllerBase
{
    private readonly ISagRepository _sagRepository;
    
    public SagController(ISagRepository sagRepository)
    {
        _sagRepository = sagRepository;
    }
    
    [HttpPost]
    public async Task<ActionResult<Sag>> CreateSag(Sag sag)
    {
        
        // Hvis Sagsnummer ikke er angivet, opret et nyt GUID
            if (sag.Sagsnummer == Guid.Empty)
            {
                sag.Sagsnummer = Guid.NewGuid();
            }

            // Tjek om sagen allerede findes i databasen 
            if (await _sagRepository.SagExistsAsync(sag.Sagsnummer))
            {
                return Conflict("Sagen eksisterer allerede.");
            }
            
            var createdSag = await _sagRepository.CreateSagAsync(sag);
            return CreatedAtAction("GetSag", new { id = createdSag.Sagsnummer }, createdSag);
        
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Sag>> GetSag(Guid id)
    {
        try
        {
            var sag = await _sagRepository.GetSagAsync(id);
            if (sag == null)
            {
                return NotFound($"Sagen med ID {id} blev ikke fundet.");
            }

            return Ok(sag);
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, $"Internt serverfejl: {ex.Message}");
        }
    }
}