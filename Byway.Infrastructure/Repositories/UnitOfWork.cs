using Byway.Domain.Interfaces;
using Byway.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly BywayDbContext _context;

        public IInstructorRepository Instructors { get; private set; }

        public ICategoryRepository Categories { get; private set; }

        public ICourseRepository Courses { get; private set; }

        public IContentRepository Contents { get; private set; }    



        public UnitOfWork(BywayDbContext context)
        {
            _context = context;
            Instructors = new InstructorRepository(_context);
            Categories = new CategoryRepository(_context);
                
            Courses = new CourseRepository(_context);

            Contents = new ContentRepository(_context);



        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
