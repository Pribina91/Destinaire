using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Destinataire.Core;
using Destinataire.Core.Helpers;
using Destinataire.Core.Interfaces;
using Destinataire.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Destinataire.Data;
using Destinataire.Data.Model;
using Destinataire.Web.Contracts;
using Destinataire.Web.Dtos;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;

namespace Destinataire.Web.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]")]
    [ApiController]
    public class ContactsController : DestinaireBaseController
    {
        private readonly DestinaireContext _context;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;


        public ContactsController(DestinaireContext context, IContactRepository contactRepository, IMapper mapper)
        {
            _context = context;
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<ContactDto>), StatusCodes.Status200OK)]
        public ActionResult<PagedList<ContactDto>> GetContacts([FromQuery] GetContactsParameters parameters)
        {
            var pagedList = _contactRepository.GetAll(parameters.PageIndex, parameters.PageSize, "");

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagedList.Pagination));

            return Ok(_mapper.Map<PagedList<Contact>, PagedList<ContactDto>>(pagedList));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContactDto>> GetContact(Guid id)
        {
            var contact = await _contactRepository.GetByID(id);

            if (contact == null)
            {
                return NotFound(id);
            }

            return Ok(_mapper.Map<ContactDto>(contact));
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutContact(Guid id, ContactDto contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            var dbContact = await _contactRepository.GetByID(id);
            if (dbContact is null)
            {
                return NotFound(id);
            }

            try
            {
                dbContact = _mapper.Map(contact, dbContact);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ContactExists(id))
                {
                    return NotFound(id);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostContact(ContactDto contact)
        {
            try
            {
                var dbContact = _mapper.Map<Contact>(contact);

                await _contactRepository.Insert(dbContact);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetContact", new {id = contact.Id}, contact);
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Contact>> DeleteContact(Guid id)
        {
            if (!await ContactExists(id))
            {
                return NotFound(id);
            }

            await _contactRepository.Delete(id);
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
        }

        private async Task<bool> ContactExists(Guid id)
        {
            return await _contactRepository.GetByID(id) != null;
        }
    }
}