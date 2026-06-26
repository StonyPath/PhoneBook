using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Application.Commands.AddContact;
using PhoneBook.Application.Commands.DeleteContact;
using PhoneBook.Application.Commands.UpdateContact;
using PhoneBook.Application.DTOs;
using PhoneBook.Application.Queries.GetContactsByTag;

namespace NoteBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContactsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddContact([FromBody] AddContactCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(AddContact), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateContact(Guid id, [FromBody] UpdateContactCommand command)
    {
        if (id != command.Id)
            return BadRequest("The ID in the URL does not match the ID in the body.");

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteContact(Guid id)
    {

        await _mediator.Send(new DeleteContactCommand(id));
        return NoContent();
    }

    [HttpGet("byTag")]
    [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTag([FromQuery] string tag)
    {
        var results = await _mediator.Send(new GetContactsByTagQuery(tag));
        return Ok(results);
    }
}
