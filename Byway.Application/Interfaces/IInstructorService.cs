using Byway.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Interfaces
{
    public interface IInstructorService
    {
        Task<InstructorDto> CreateInstructorAsync(CreateInstructorDto instructorDto);

        Task<InstructorDto> UpdateInstructorAsync(int id, UpdateInstructorDto instructorDto);
        Task<int> GetInstructorsCountAsync();

        Task<PagedResult<InstructorDto>> GetAllInstructorsAsync(string? searchTerm, int pageNumber, int pageSize);
        Task DeleteInstructorAsync(int id);
    }
}
