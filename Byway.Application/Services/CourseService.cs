using AutoMapper;
using Byway.Application.DTOs;
using Byway.Application.Interfaces;
using Byway.Domain.Entities;
using Byway.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Byway.Application.Services
{
    public class CourseService:ICourseService
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<AddCourseDto> _addCourseValidator;
        private readonly IValidator<UpdateCourseDto> _updateCourseValidator;
        private readonly IHostEnvironment _hostEnvironment;




        public CourseService(IUnitOfWork unitOfWork,
            IMapper mapper, IValidator<AddCourseDto> addCourseValidator,
            IValidator<UpdateCourseDto> updateCourseValidator,
            IHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _addCourseValidator = addCourseValidator;
            _updateCourseValidator = updateCourseValidator;
            _hostEnvironment = hostEnvironment;
        }









        public async Task<PaginatedResult<CourseDto>> GetAllCoursesAsync(CourseQueryParams queryParams)
        {
            IQueryable<Course> query = _unitOfWork.Courses.GetQueryable()
                .Include(c => c.Instructor)
                .Include(c => c.Category);

            if (queryParams.CategoryId.HasValue && queryParams.CategoryId.Value > 0)
            {
                query = query.Where(c => c.CategoryId == queryParams.CategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var searchTerm = queryParams.SearchTerm;


                var ratingMatch = Regex.Match(searchTerm, @"\b([1-5])\b");
                var priceMatch = Regex.Match(searchTerm, @"(\$?)(\d+\.?\d+)");

                if (ratingMatch.Success && !searchTerm.Contains("$"))
                {
                    if (int.TryParse(ratingMatch.Groups[1].Value, out int rating))
                    {
                        query = query.Where(c => c.Rating == rating);
                    }
                    searchTerm = searchTerm.Replace(ratingMatch.Value, "").Trim();
                }
                else if (priceMatch.Success)
                {
                    if (decimal.TryParse(priceMatch.Groups[2].Value, out decimal price))
                    {
                        query = query.Where(c => c.Price == price);
                    }
                    searchTerm = searchTerm.Replace(priceMatch.Value, "").Trim();
                }

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var term = searchTerm.Trim().ToLower();
                    query = query.Where(c => c.Name != null && c.Name.ToLower().Contains(term));
                }
            }

            var totalCount = await query.CountAsync();
            var paginatedCourses = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            var courseDtos = _mapper.Map<List<CourseDto>>(paginatedCourses);
            return new PaginatedResult<CourseDto>
            {
                Items = courseDtos,
                TotalCount = totalCount
            };
        }


        public async Task<CourseDto> GetCourseByIdAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id, new[] { "Instructor", "Category", "Contents" });

            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {id} was not found.");
            }

            return _mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> CreateCourseAsync(AddCourseDto courseDto)
        {
            await _addCourseValidator.ValidateAndThrowAsync(courseDto);

            string imageUrlPath = null;
            if (courseDto.ImageFile != null)
            {
                imageUrlPath = await SaveImageAsync(courseDto.ImageFile);
            }

            var course = _mapper.Map<Course>(courseDto);
            course.ImagePath = imageUrlPath;

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.CompleteAsync();

            var createdCourse = await _unitOfWork.Courses.GetByIdAsync(course.Id, new[] { "Instructor", "Category" });
            return _mapper.Map<CourseDto>(createdCourse);
        }



        public async Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto courseDto)
        {
            await _updateCourseValidator.ValidateAndThrowAsync(courseDto);

            // Load the course and its related contents
            var existingCourse = await _unitOfWork.Courses.GetByIdAsync(id, new[] { "Instructor", "Category", "Contents" });
            if (existingCourse == null)
            {
                throw new KeyNotFoundException($"Course with ID {id} was not found.");
            }

            // Handle image update
            string newImageUrlPath = existingCourse.ImagePath;
            if (courseDto.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(existingCourse.ImagePath))
                {
                    DeleteImage(existingCourse.ImagePath);
                }
                newImageUrlPath = await SaveImageAsync(courseDto.ImageFile);
            }

            // Map simple properties
            _mapper.Map(courseDto, existingCourse);
            existingCourse.ImagePath = newImageUrlPath;

            // Manually update the Contents collection
            var contentsToRemove = existingCourse.Contents.ToList();
            if (contentsToRemove.Any())
            {
                _unitOfWork.Contents.DeleteRange(contentsToRemove);
            }
            var newContents = new List<Content>();
            if (courseDto.Contents != null && courseDto.Contents.Any())
            {
                foreach (var contentDto in courseDto.Contents)
                {
                    newContents.Add(_mapper.Map<Content>(contentDto));
                }
            }
            existingCourse.Contents = newContents;

            // ✅ --- THIS IS THE MISSING LINE --- ✅
            // Explicitly tell the change tracker that the course entity has been modified.
            _unitOfWork.Courses.Update(existingCourse);

            // Now, when you save, EF knows to generate the UPDATE SQL command.
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CourseDto>(existingCourse);
        }














        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var savePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "courses");

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            var filePath = Path.Combine(savePath, uniqueFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/courses/{uniqueFileName}";
        }



        private void DeleteImage(string imageUrlPath)
        {
            if (string.IsNullOrEmpty(imageUrlPath)) return;

            var physicalPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", imageUrlPath.TrimStart('/'));

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
        }


        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(course.ImagePath))
            {
                DeleteImage(course.ImagePath);
            }

            _unitOfWork.Courses.Delete(course);
            await _unitOfWork.CompleteAsync();

            return true;
        }





      


    }
}
