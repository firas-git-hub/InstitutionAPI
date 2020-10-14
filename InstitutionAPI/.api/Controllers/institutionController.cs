using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstitutionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstitutionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class institutionController : ControllerBase
    {
        private readonly institutionContext _context;

        public institutionController(institutionContext context)
        {
            _context = context;
        }

        [HttpGet("{institutionId}")]
        public async Task<ActionResult<institution>> GetInstitution(Guid institutionId)
        {
            var institution = await _context.institutions.FindAsync(institutionId);

            if (institution == null)
            {
                return NotFound();
            }

            return institution;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<institution>>> GetInstitutitons()
        {
            return await _context.institutions
               .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(Guid id, institution institution)
        {
            if (id != institution.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.institutions.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = institution.Name;
            todoItem.Code = institution.Code;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!todoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpPost]
        public ActionResult<institution> CreateInstitution(institution inst)
        {
            var institution = new institution
            {
                Code = inst.Code,
                Name = inst.Name
            };
            _context.institutions.Add(institution);
            _context.SaveChangesAsync();

            return Created(
                nameof(GetInstitution),
                institution);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstitution(Guid id)
        {
            var institution = await _context.institutions.FindAsync(id);

            if (institution == null)
            {
                return NotFound();
            }

            _context.institutions.Remove(institution);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool todoItemExists(Guid id)
        {
            return _context.institutions.Any(e => e.Id == id);
        }
    }  
}
