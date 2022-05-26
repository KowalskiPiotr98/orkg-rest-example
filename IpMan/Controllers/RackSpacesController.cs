using IpMan.Controllers.DataTransferObjects;
using IpMan.Models;
using IpMan.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Controllers;

[ApiController]
[Route("[controller]")]
public class RackSpacesController : ControllerBase
{
    private readonly RackSpaceRepository _repository;

    public RackSpacesController(RackSpaceRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RackSpace>>> GetRackSpaces(CancellationToken cancellationToken)
    {
        return await _repository.GetRackSpaces().ToListAsync(cancellationToken);
    }

    [HttpGet("{rackSpaceId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RackSpace>> GetRackSpace(Guid rackSpaceId, CancellationToken cancellationToken)
    {
        var rackSpace = await _repository.GetRackSpace(rackSpaceId, cancellationToken);

        return rackSpace is null ? NotFound() : rackSpace;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<RackSpace>> PostRackSpace([FromBody] RackSpaceDto rackSpaceDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest();

        var rackSpace = new RackSpace(rackSpaceDto.BuildingId!.Value, rackSpaceDto.Room!, rackSpaceDto.ServerRack!.Value, rackSpaceDto.RackRow!.Value);
        try
        {
            await _repository.CreateRackSpace(rackSpace, cancellationToken);
            return CreatedAtAction(nameof(PostRackSpace), rackSpace);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{rackSpaceId:Guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> PutRackSpace(Guid rackSpaceId, [FromBody] RackSpaceDto rackSpaceDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest();

        var existingRackSpace = await _repository.GetRackSpace(rackSpaceId, cancellationToken);
        if (existingRackSpace is null)
        {
            var newRackSpace = new RackSpace(rackSpaceDto.BuildingId!.Value, rackSpaceDto.Room!, rackSpaceDto.ServerRack!.Value, rackSpaceDto.RackRow!.Value);
            await _repository.CreateRackSpace(newRackSpace, cancellationToken);
            return CreatedAtAction(nameof(PutRackSpace), newRackSpace);
        }
        existingRackSpace.SetBuilding(rackSpaceDto.BuildingId!.Value);
        existingRackSpace.SetLocationInBuilding(rackSpaceDto.Room!, rackSpaceDto.ServerRack!.Value, rackSpaceDto.RackRow!.Value);
        try
        {
            await _repository.UpdateRackSpace(existingRackSpace, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{rackSpaceId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteRackSpace(Guid rackSpaceId, CancellationToken cancellationToken)
    {
        var rackSpace = await _repository.GetRackSpace(rackSpaceId, cancellationToken);

        if (rackSpace is null) return NotFound();

        await _repository.RemoveRackSpace(rackSpace, cancellationToken);
        return Ok();
    }
}
