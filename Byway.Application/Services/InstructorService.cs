using AutoMapper;
using Byway.Application.DTOs;
using Byway.Application.Interfaces;
using Byway.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateInstructorDto> _validator;
        public InstructorService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateInstructorDto> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;

        }


        public async Task<InstructorDto> CreateInstructorAsync(CreateInstructorDto instructorDto)
        {
            await _validator.ValidateAndThrowAsync(instructorDto);

            string imageUrlPath = null;
            if (instructorDto.Image != null && instructorDto.Image.Length > 0)
            {
                imageUrlPath = await SaveImageAsync(instructorDto.Image);
            }

            var instructor = _mapper.Map<Domain.Entities.Instructor>(instructorDto);

            instructor.ImagePathUrl = imageUrlPath;

            await _unitOfWork.Instructors.AddAsync(instructor);
            await _unitOfWork.CompleteAsync();

            var resultDto = _mapper.Map<InstructorDto>(instructor);
            return resultDto;
        }


        private async Task<string> SaveImageAsync(IFormFile image)
        {

            var savePath = Path.Combine("wwwroot", "images", "instructors");

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            // Create a unique file name to avoid conflicts
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
            var filePath = Path.Combine(savePath, uniqueFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // Return the URL path that the frontend will use to access the image
            return $"/images/instructors/{uniqueFileName}";
        }


        public async Task<InstructorDto> UpdateInstructorAsync(int id, UpdateInstructorDto instructorDto)
        {

            var existingInstructor = await _unitOfWork.Instructors.GetByIdAsync(id);
            if (existingInstructor == null)
            {
                return null;
            }

            string newImageUrlPath = existingInstructor.ImagePathUrl;
            if (instructorDto.Image != null && instructorDto.Image.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingInstructor.ImagePathUrl))
                {
                    DeleteImage(existingInstructor.ImagePathUrl);
                }

                newImageUrlPath = await SaveImageAsync(instructorDto.Image);
            }


            _mapper.Map(instructorDto, existingInstructor);


            existingInstructor.ImagePathUrl = newImageUrlPath;


            await _unitOfWork.CompleteAsync();

            var resultDto = _mapper.Map<InstructorDto>(existingInstructor);
            return resultDto;
        }

        private void DeleteImage(string imageUrlPath)
        {
            if (string.IsNullOrEmpty(imageUrlPath))
            {
                return;
            }


            var physicalPath = Path.Combine("wwwroot", imageUrlPath.TrimStart('/'));

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
        }

        public async Task<int> GetInstructorsCountAsync()
        {
            return await _unitOfWork.Instructors.CountAsync();
        }


        public async Task<PagedResult<InstructorDto>> GetAllInstructorsAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            var query = _unitOfWork.Instructors.GetQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();


                var matchingJobTitles = Enum.GetNames<JobTitle>()
                    .Where(name => name.ToLower().Contains(term))
                    .Select(name => Enum.Parse<JobTitle>(name))
                    .ToList();


                query = query.Where(i =>
                    (i.Name != null && i.Name.ToLower().Contains(term)) ||
                    (matchingJobTitles.Any() && matchingJobTitles.Contains(i.JobTitle))
                );
            }


            var totalCount = await query.CountAsync();

            var paginatedQuery = query
    .OrderByDescending(i => i.Name) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var paginatedInstructors = await paginatedQuery.ToListAsync();

            var instructorDtos = _mapper.Map<List<InstructorDto>>(paginatedInstructors);

            return new PagedResult<InstructorDto>
            {
                Items = instructorDtos,
                TotalCount = totalCount
            };
        }


        public async Task DeleteInstructorAsync(int id)
        {
            var existingInstructor = await _unitOfWork.Instructors.GetByIdAsync(id);
            if (existingInstructor == null)
            {
               
                throw new KeyNotFoundException($"Instructor with ID {id} was not found.");
            }

           
            var hasCourses = await _unitOfWork.Courses.AnyAsync(c => c.InstructorId == id);
            if (hasCourses)
            {
                
                throw new InvalidOperationException("This instructor cannot be deleted because they are assigned to one or more courses.");
            }

            if (!string.IsNullOrEmpty(existingInstructor.ImagePathUrl))
            {
                DeleteImage(existingInstructor.ImagePathUrl);
            }

            _unitOfWork.Instructors.Delete(existingInstructor);
            await _unitOfWork.CompleteAsync();
        }
    }
}
