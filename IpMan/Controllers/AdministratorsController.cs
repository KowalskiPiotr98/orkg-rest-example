using IpMan.Controllers.DataTransferObjects;
using IpMan.Models;
using IpMan.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Controllers;

[ApiController]
[Route("[controller]")]
public class AdministratorsController : ControllerBase
{
    private readonly AdministratorsRepository _repository;

    public AdministratorsController(AdministratorsRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Administrator>>> GetAdministrators(CancellationToken cancellationToken)
    {
        return await _repository.GetAdministrators().ToListAsync(cancellationToken);
    }

    [HttpGet("{administratorId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Administrator>> GetAdministrator(Guid administratorId, CancellationToken cancellationToken)
    {
        var administrator = await _repository.GetAdministrator(administratorId, cancellationToken);

        return administrator is null ? NotFound() : administrator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Administrator>> PostAdministrator([FromBody] AdministratorDto administratorDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest();

        var administrator = new Administrator(administratorDto.FirstName!, administratorDto.LastName!, administratorDto.EmailAddress!);
        try
        {
            await _repository.CreateAdministrator(administrator, cancellationToken);
            return CreatedAtAction(nameof(PostAdministrator), administrator);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{administratorId:Guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> PutAdministrator(Guid administratorId, [FromBody] AdministratorDto administratorDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest();

        var existingAdministrator = await _repository.GetAdministrator(administratorId, cancellationToken);
        if (existingAdministrator is null)
        {
            var newAdministrator = new Administrator(administratorDto.FirstName!, administratorDto.LastName!, administratorDto.EmailAddress!);
            await _repository.CreateAdministrator(newAdministrator, cancellationToken);
            return CreatedAtAction(nameof(PutAdministrator), newAdministrator);
        }
        existingAdministrator.SetPersonalData(administratorDto.FirstName!, administratorDto.LastName!, administratorDto.EmailAddress!);
        try
        {
            await _repository.UpdateAdministrator(existingAdministrator, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{administratorId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAdministrator(Guid administratorId, CancellationToken cancellationToken)
    {
        var administrator = await _repository.GetAdministrator(administratorId, cancellationToken);

        if (administrator is null) return NotFound();

        await _repository.RemoveAdministrator(administrator, cancellationToken);
        return Ok();
    }
}
