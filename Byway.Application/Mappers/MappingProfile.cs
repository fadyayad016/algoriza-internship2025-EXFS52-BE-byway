using AutoMapper;
using Byway.Application.DTOs;
using Byway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // ✅ --- THIS IS THE CORRECTED MAP --- ✅
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
                // Add the missing mappings below
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Certification, opt => opt.MapFrom(src => src.Certification))
                .ForMember(dest => dest.TotalHours, opt => opt.MapFrom(src => src.TotalHours))
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.Contents)); // Also map the contents

            // --- The rest of your mappings remain the same ---

            CreateMap<Content, ContentDto>();

            CreateMap<AddCourseDto, Course>()
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore());

            CreateMap<AddContentDto, Content>();

            CreateMap<UpdateCourseDto, Course>()
       .ForMember(dest => dest.Contents, opt => opt.Ignore());

            CreateMap<CreateInstructorDto, Instructor>();
            CreateMap<Instructor, InstructorDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => (int)src.JobTitle));

            CreateMap<UpdateInstructorDto, Instructor>();

            CreateMap<Category, CategoryDto>()
                       .ForMember(
                           dest => dest.CourseCount,
                           opt => opt.MapFrom(src => src.Courses.Count)
                       );
            CreateMap<CreateOrUpdateCategoryDto, Category>();


        }
    }
}
