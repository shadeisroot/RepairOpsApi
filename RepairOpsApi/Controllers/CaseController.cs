using Microsoft.AspNetCore.Mvc;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;

namespace RepairOpsApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CaseController : ControllerBase
{
    private readonly ICaseRepository _repository;

    public CaseController(ICaseRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet] //henter alle sager (/api/case)
    public async Task<ActionResult<IEnumerable<Case>>> GetCases()
    {  
        //returnerer en liste af case (HTTP 200 (OK))
        return Ok(await _repository.GetAllCasesAsync());
    }
    
    [HttpGet("{id}")] //henter en sag ud fra id (/api/case/{ID})
    public async Task<ActionResult<Case>> GetCase(Guid id)
    {
        var caseItem = await _repository.GetCaseByIdAsync(id); //finder case med det angivene ID
        if (caseItem == null) return NotFound(); //returnere HTTP 404, hvis case ikke findes
        return caseItem; //retunere en case
    }
    
    [HttpPost] //opretter en ny sag (/api/case)
    public async Task<ActionResult<Case>> PostCase(Case caseItem)
    {
        try
        {
            //validerer input
            if (caseItem == null)
            {
                return BadRequest(new { Message = "CaseItem kan ikke være null." });
            }
            //opdatere case og gemmer den i aatabasen
            var createdCase = await _repository.AddCaseAsync(caseItem);
            //reutnere med HTTP 201 (Created) med de nye detaljer
            return CreatedAtAction("GetCase", new { id = createdCase.Id }, createdCase);
        }
        catch (Exception ex)
        {
            //returnere HTTP 500 med fejl
            return StatusCode(500, new { Message = "En intern serverfejl opstod", Error = ex.Message });
        }
    }
    
    [HttpPut("{id}")] // Opdater en sag (/api/case/{id})
    public async Task<IActionResult> PutCase(Guid id, [FromBody] Case caseItem)
    {
       if (caseItem == null || string.IsNullOrEmpty(caseItem.Status))
          {
              return BadRequest(new { Message = "Status er påkrævet." });
          }
      
          // Hent den eksisterende sag
          var existingCase = await _repository.GetCaseByIdAsync(id);
      
          if (existingCase == null)
          {
              return NotFound(new { Message = $"Sagen med ID {id} blev ikke fundet." });
          }
      
          // Kun opdater status
          existingCase.Status = caseItem.Status;
      
          // Opdater sagen i databasen
          var success = await _repository.UpdateCaseAsync(existingCase);
      
          if (!success)
          {
              return StatusCode(500, new { Message = "En fejl opstod under opdateringen af sagen." });
          }
      
          return Ok(new
          {
              Message = "Sagen blev opdateret succesfuldt.",
              UpdatedCase = existingCase
          });
    }
    
    [HttpDelete("{id}")] //slette (med id) (/api/case/{id})
    public async Task<IActionResult> DeleteCase(Guid id)
    {
        // Hent sagen først
        var existingCase = await _repository.GetCaseByIdAsync(id);
        if (existingCase == null)
        {
            return NotFound(); // Returner NotFound hvis sagen ikke eksisterer
        }

        // Slet sagen
        var result = await _repository.DeleteCaseAsync(id);
        if (!result)
        {
            return BadRequest(); // Hvis sletning fejler
        }

        return NoContent(); // Succesfuld sletning
    }
    
}