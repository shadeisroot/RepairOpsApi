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
        // 1. Hent den eksisterende sag
        var existingCase = await _repository.GetCaseByIdAsync(id);

        // 2. Hvis sagen ikke findes
        if (existingCase == null)
        {
            return NotFound(new { Message = $"Sagen med ID {id} blev ikke fundet." });
        }
        // 3. Opdater kun de felter, der er angivet
        existingCase.CustomerName = caseItem.CustomerName ?? existingCase.CustomerName;
        existingCase.EquipmentType = caseItem.EquipmentType ?? existingCase.EquipmentType;
        existingCase.ProblemDescription = caseItem.ProblemDescription ?? existingCase.ProblemDescription;
        existingCase.ExpectedDeliveryDate = caseItem.ExpectedDeliveryDate != DateTime.MinValue ? caseItem.ExpectedDeliveryDate : existingCase.ExpectedDeliveryDate;
        existingCase.Priority = caseItem.Priority ?? existingCase.Priority;
        existingCase.AssignedTechnician = caseItem.AssignedTechnician ?? existingCase.AssignedTechnician;
        existingCase.Status = caseItem.Status ?? existingCase.Status;

        // 4. Opdater sagen i databasen
        var success = await _repository.UpdateCaseAsync(existingCase);

        if (!success)
        {
            return StatusCode(500, new { Message = "En fejl opstod under opdateringen af sagen." });
        }
        // 5. Returnér den opdaterede sag
        return Ok(new
        {
            Message = "Sagen blev opdateret succesfuldt.",
            UpdatedCase = existingCase
        });
    }
    
    [HttpDelete("{id}")] //slette (med id) (/api/case/{id})
    public async Task<IActionResult> DeleteCase(Guid id)
    {
        //forsøger og slette case
        var success = await _repository.DeleteCaseAsync(id);
        //retunere 404 hvis case ikke findes
        if (!success) return NotFound();
        //returnere 204 hvis sletning var succesfuld
        return NoContent();
    }
    
}