using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IInstructorRepository Instructors { get; }
        ICategoryRepository Categories { get; }
        ICourseRepository Courses { get; }

        // TO:
        IContentRepository Contents { get; } // ✅ Plural       
        Task<int> CompleteAsync();

    }
}
