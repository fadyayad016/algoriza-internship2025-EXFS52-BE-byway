using Byway.Domain.Entities;
using Byway.Domain.Interfaces;
using Byway.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Infrastructure.Repositories
{
    public class InstructorRepository: GenericRepository<Instructor>, IInstructorRepository
    {
        private readonly BywayDbContext _context;   
        public InstructorRepository(BywayDbContext context) : base(context)
       {

            _context = context;
       }

        public async Task<int> GetCountAsync()
        {
            return await _context.Instructors.CountAsync();
        }
    }
}
