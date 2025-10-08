using AutoMapper;
using Byway.Application.DTOs;
using Byway.Application.Interfaces;
using Byway.Domain.Entities;
using Byway.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }




        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            IQueryable<Category> query = _unitOfWork.Categories.GetQueryable();

           
            query = query.Include("Courses");

            var categories = await query.ToListAsync();

            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return categoryDtos;
        }


        public async Task<CategoryDto> CreateAsync(CreateOrUpdateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);

            await _unitOfWork.Categories.AddAsync(category);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CategoryDto>(category);
        }







    }
}
