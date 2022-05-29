using IpMan.Controllers.DataTransferObjects;
using IpMan.Models;
using IpMan.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Controllers;

[ApiController]
[Route("[controller]")]
public class ServersController : ControllerBase
{
    private readonly ServersRepository _repository;

    public ServersController(ServersRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// List all servers
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Server>>> GetServers(CancellationToken cancellationToken)
    {
        return await _repository.GetServers().ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Get detailed information about a server
    /// </summary>
    /// <param name="serverId">ID of a server</param>
    [HttpGet("{serverId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Server>> GetServer(Guid serverId, CancellationToken cancellationToken)
    {
        var server = await _repository.GetServer(serverId, cancellationToken);

        return server is null ? NotFound() : server;
    }

    /// <summary>
    /// Create a new server
    /// </summary>
    /// <response code="400">Model is invalid or server with this IP already exists.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Server>> PostServer([FromBody] ServerDto serverDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || !serverDto.TryGetIp(out var ip)) return BadRequest();

        var server = new Server(ip, serverDto.RackSpaceId!.Value, serverDto.AdministratorId!.Value);
        try
        {
            await _repository.CreateServer(server, cancellationToken);
            return CreatedAtAction(nameof(PostServer), server);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create or update a server
    /// </summary>
    /// <response code="400">Model is invalid or server with this IP already exists.</response>
    /// <response code="200">Server was updated.</response>
    /// <response code="201">Server was not found, so a new one was created.</response>
    [HttpPut("{serverId:Guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> PutServer(Guid serverId, [FromBody] ServerDto serverDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || !serverDto.TryGetIp(out var ip)) return BadRequest();

        var existingServer = await _repository.GetServer(serverId, cancellationToken);
        if (existingServer is null)
        {
            var newServer = new Server(ip, serverDto.RackSpaceId!.Value, serverDto.AdministratorId!.Value);
            await _repository.CreateServer(newServer, cancellationToken);
            return CreatedAtAction(nameof(PutServer), newServer);
        }
        existingServer.AssignIp(ip);
        existingServer.AssignRackSpace(serverDto.RackSpaceId!.Value);
        existingServer.AssignAdministrator(serverDto.AdministratorId!.Value);
        try
        {
            await _repository.UpdateServer(existingServer, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete a server
    /// </summary>
    [HttpDelete("{serverId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteServer(Guid serverId, CancellationToken cancellationToken)
    {
        var server = await _repository.GetServer(serverId, cancellationToken);

        if (server is null) return NotFound();

        await _repository.RemoveServer(server, cancellationToken);
        return Ok();
    }
}
