using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using InstitutionAPI.bll.Mapping;
using InstitutionAPI.bll.Services;
using InstitutionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InstitutionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class institutionController : ControllerBase
    {
        private readonly institutionContextService _context;
        private readonly institutionMap institutionmapper;
        private readonly ILogger<institutionController> _logger;
        public institutionController(institutionContext context, ILogger<institutionController> logger)
        {
            _context = new institutionContextService(context);
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<institution>>> GetInstitutitons()
        {
            return await _context.getInstitutions()
               .ToListAsync();
        }

        [HttpGet("{institutionId}")]
        public async Task<ActionResult<institution>> GetInstitution(Guid institutionId)
        {
            var institution = await _context.Find(institutionId);

            if (institution.Value == null)
            {
                _logger.LogInformation(NotFound().ToString() + "|| Time Accessed : " + DateTime.Now + "|| Accessed by : " + Environment.UserName);
                return NotFound();
            }

            return institution;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(Guid id, institution institution)
        {
           

            var oldInst = await _context.Find(id);
            if (oldInst == null)
            {
                return NotFound();
            }

            oldInst.Value.Name = institution.Name;
            oldInst.Value.Code = institution.Code;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!institutionExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpPost]
        public ActionResult<institution> CreateInstitution(institution inst)
        {
            bool creationState = _context.Add(inst);
            if(creationState == true)
            {
                _logger.LogInformation(Created(nameof(GetInstitution), inst).ToString() + "|| Time Accessed : " + DateTime.Now + "|| Accessed by : " + Environment.UserName);
                return Created(
                nameof(GetInstitution),
                inst);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstitution(Guid id)
        {
            var institution = await _context.Find(id);

            if (institution == null)
            {
                _logger.LogInformation(NotFound().ToString() + "|| Time Accessed : " + DateTime.Now + "|| Accessed by : " + Environment.UserName);
                return NotFound();
            }

            _context.Remove(institution);
            await _context.SaveChangesAsync();
            _logger.LogInformation(institution.Value.Code.ToString() + "|| Time Accessed : " + DateTime.Now + "|| Accessed by : " + Environment.UserName);

            return NoContent();
        }
        private bool institutionExists(Guid id)
        {
            return _context.Exists(id);
        }
    }  
}
