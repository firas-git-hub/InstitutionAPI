using InstitutionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace InstitutionAPI.bll.Services
{
    public class institutionContextService
    {
        private readonly institutionContext _context;

        public institutionContextService(institutionContext context)
        {
            _context = context;
        }

        public DbSet<institution> getInstitutions()
        {
            return _context.institutions;
        }

        public async Task<ActionResult<institution>> Find(Guid instId)
        {
            var institution = await _context.institutions.FindAsync(instId);
            return institution;
        }



        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public bool Add(institution inst)
        {
            if(_context.institutions.Find(inst.Id) == null)
            {
                _context.institutions.Add(inst);
                _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public void Remove(ActionResult<institution> inst)
        {
            _context.institutions.Remove(inst.Value);
        }

        public bool Exists(Guid id)
        {
            if (_context.institutions.Find(id) == null)
                return false;
            return true;
        }
    }
}
