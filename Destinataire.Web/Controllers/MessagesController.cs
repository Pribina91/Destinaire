using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Destinataire.Core.Helpers;
using Destinataire.Core.Interfaces;
using Destinataire.Core.Repositories.Filtering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Destinataire.Data;
using Destinataire.Data.Model;
using Destinataire.Web.Contracts;
using Destinataire.Web.Dtos;
using Newtonsoft.Json;

namespace Destinataire.Web.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : DestinaireBaseController
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMessageService _messageService;
        private readonly DestinaireContext _context;
        private readonly IMapper _mapper;

        public MessagesController(DestinaireContext destinaireContext, IMessageRepository messageRepository,
            IMapper mapper, IContactRepository contactRepository, IMessageService messageService)
        {
            _context = destinaireContext;
            _messageRepository = messageRepository;
            _mapper = mapper;
            _contactRepository = contactRepository;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("received")]
        [ProducesResponseType(typeof(PagedList<MessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedList<MessageDto>>> GetReceivedMessages(
            [FromQuery] GetPersonalMessagesParameters parameters)
        {
            if (!await IsContactValid(parameters.ContactId)) return BadRequest();

            var pagedList =
                _messageRepository.GetReceivedMessages(parameters.ContactId, parameters.PageIndex, parameters.PageSize);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagedList.Pagination));

            return Ok(_mapper.Map<PagedList<Message>, PagedList<MessageDto>>(pagedList));
        }

        [HttpGet]
        [Route("sent")]
        [ProducesResponseType(typeof(PagedList<MessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedList<MessageDto>>> GetSentMessages(
            [FromQuery] GetPersonalMessagesParameters parameters)
        {
            if (!await IsContactValid(parameters.ContactId)) return BadRequest();

            var pagedList =
                _messageRepository.GetSentMessages(parameters.ContactId, parameters.PageIndex, parameters.PageSize);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagedList.Pagination));

            return Ok(_mapper.Map<PagedList<Message>, PagedList<MessageDto>>(pagedList));
        }


        [HttpGet]
        [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessages([FromQuery] GetMessagesParameters parameters)
        {
            var pagedList = _messageRepository.GetMessages(
                new GetMessagesFilter() {From = parameters.From, To = parameters.To, Search = parameters.Search},
                parameters.PageIndex, parameters.PageSize,
                parameters.Order);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagedList.Pagination));

            return Ok(_mapper.Map<PagedList<Message>, PagedList<MessageDto>>(pagedList));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageDto>> GetMessage(Guid id)
        {
            var message = await _messageRepository.GetByID(id);

            if (message == null)
            {
                return NotFound(id);
            }

            return _mapper.Map<MessageDto>(message);
        }


        [HttpPatch]
        [Route("markread")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> MarkReadMessages([FromBody] PatchMessagesParameters parameters)
        {
            if (parameters.MessageIds != null && parameters.MessageIds.Any())
            {
                await _messageService.MarkMessagesAsRead(parameters.MessageIds);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch]
        [Route("markunread")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> MarkUnreadMessages([FromBody] PatchMessagesParameters parameters)
        {
            if (parameters.MessageIds != null && parameters.MessageIds.Any())
            {
                await _messageService.MarkMessagesAsNotRead(parameters.MessageIds);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(typeof(MessageDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MessageDto>> SendMessage([FromBody] SendMessageInput parameters)
        {
            var validationTasks = new List<Task<bool>>
            {
                IsContactValid(parameters.SenderId),
                IsContactValid(parameters.RecipientId),
            };

            await Task.WhenAll(validationTasks);

            if (validationTasks.Any(t => !t.Result))
            {
                return BadRequest();
            }

            try
            {
                var message =
                    await _messageService.SendMessage(parameters.SenderId, parameters.RecipientId, parameters.Text);
                await _context.SaveChangesAsync();
                var messageDto = _mapper.Map<Message, MessageDto>(message);
                return CreatedAtAction("GetMessage", new {id = messageDto.Id}, messageDto);
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            if (!await MessageExists(id))
            {
                return NotFound(id);
            }

            await _messageRepository.Delete(id);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> MessageExists(Guid id)
        {
            return await _messageRepository.GetByID(id) != null;
        }

        private async Task<bool> IsContactValid(Guid contactId)
        {
            return await _contactRepository.GetByID(contactId) != null;
        }
    }
}