using IpMan.Controllers.DataTransferObjects;
using IpMan.Models;
using IpMan.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Controllers;

[ApiController]
[Route("[controller]")]
public class BuildingsController : ControllerBase
{
    private readonly BuildingsRepository _repository;

    public BuildingsController(BuildingsRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// List all buildings
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Building>>> GetBuildings(CancellationToken cancellationToken)
    {
        return await _repository.GetBuildings().ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Get detailed information about a building
    /// </summary>
    /// <param name="buildingId">ID of a building</param>
    [HttpGet("{buildingId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Building>> GetBuilding(Guid buildingId, CancellationToken cancellationToken)
    {
        var building = await _repository.GetBuilding(buildingId, cancellationToken);

        return building is null ? NotFound() : building;
    }

    /// <summary>
    /// Create a new building
    /// </summary>
    /// <response code="400">Model is invalid or building with this name already exists.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Building>> PostBuilding([FromBody] BuildingDto buildingDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest();

        var building = new Building(buildingDto.Name!, buildingDto.AddressLine1!, buildingDto.AddressLine2);
        try
        {
            await _repository.CreateBuilding(building, cancellationToken);
            return CreatedAtAction(nameof(PostBuilding), building);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create or update a building
    /// </summary>
    /// <response code="400">Model is invalid or building with this name already exists.</response>
    /// <response code="200">Building was updated.</response>
    /// <response code="201">Building was not found, so a new one was created.</response>
    [HttpPut("{buildingId:Guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> PutBuilding(Guid buildingId, [FromBody] BuildingDto buildingDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest();

        var existingBuilding = await _repository.GetBuilding(buildingId, cancellationToken);
        if (existingBuilding is null)
        {
            var newBuilding = new Building(buildingDto.Name!, buildingDto.AddressLine1!, buildingDto.AddressLine2);
            await _repository.CreateBuilding(newBuilding, cancellationToken);
            return CreatedAtAction(nameof(PutBuilding), newBuilding);
        }
        existingBuilding.Rename(buildingDto.Name!);
        existingBuilding.ChangeAddress(buildingDto.AddressLine1!, buildingDto.AddressLine2);
        try
        {
            await _repository.UpdateBuilding(existingBuilding, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete a building
    /// </summary>
    [HttpDelete("{buildingId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteBuilding(Guid buildingId, CancellationToken cancellationToken)
    {
        var building = await _repository.GetBuilding(buildingId, cancellationToken);

        if (building is null) return NotFound();

        await _repository.RemoveBuilding(building, cancellationToken);
        return Ok();
    }
}
