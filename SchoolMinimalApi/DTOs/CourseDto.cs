using SchoolMinimalApi.Models;

namespace SchoolMinimalApi.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public CourseDto()
        {

        }

        public CourseDto(Course course)
        {
            Id = course.Id;
            Name = course.Name;
        }

        
    }
}
