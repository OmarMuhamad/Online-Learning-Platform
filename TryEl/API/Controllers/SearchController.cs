﻿using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Course>>> SearchCourses([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var courses = await _context.Courses
                .Where(c => c.CourseName.ToLower().Contains(query.ToLower()) || c.Description.ToLower().Contains(query.ToLower()))
                .ToListAsync();

            return Ok(courses);
        }

        [HttpGet("SearchWithFiltering")]
        public async Task<ActionResult<IEnumerable<Course>>> SearchCoursesWithFiltering(
        [FromQuery] string query,
        [FromQuery] string? level,
        [FromQuery] double? minPrice,
        [FromQuery] double? maxPrice,
        [FromQuery] string language)
        {

            var coursesQuery = _context.Courses.AsQueryable();

            bool isQueryValid = !string.IsNullOrWhiteSpace(query);
            bool isLevelValid = !string.IsNullOrWhiteSpace(level);
            bool isMinPriceValid = minPrice.HasValue;
            bool isMaxPriceValid = maxPrice.HasValue;
            bool isLanguageValid = !string.IsNullOrWhiteSpace(language);


            if (isQueryValid)
            {
                coursesQuery = coursesQuery.Where(c => c.CourseName.ToLower().Contains(query.ToLower()) || c.Description.ToLower().Contains(query.ToLower()));
            }

            if (isLevelValid)
            {
                coursesQuery = coursesQuery.Where(c => c.Level.ToLower() == level.ToLower());
            }

            if (isMinPriceValid)
            {
                coursesQuery = coursesQuery.Where(c => c.Price >= minPrice.Value);
            }
            if (isMaxPriceValid)
            {
                coursesQuery = coursesQuery.Where(c => c.Price <= maxPrice.Value);
            }

            if (isLanguageValid)
            {
                coursesQuery = coursesQuery.Where(c => c.Language.ToLower() == language.ToLower());
            }

            var courses = await coursesQuery.ToListAsync();

            return Ok(courses);
        }
    }
}
