<<<<<<< Updated upstream
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
=======
﻿
using SmartLearning.Application.DTOs.InstructorDto;
using SmartLearning.Application.DTOs.Instructors;
>>>>>>> Stashed changes

namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        // GET: api/Instructor
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var instructors = await _instructorService.GetAllAsync();
            return Ok(instructors);
        }

        // GET: api/Instructor/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var instructor = await _instructorService.GetByIdAsync(id);
            if (instructor == null)
                return NotFound();

            return Ok(instructor);
        }

        // POST: api/Instructor
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInstructorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _instructorService.CreateAsync(dto);

            // يرجّع 201 + Location header للـ GetById
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/Instructor/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInstructorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _instructorService.UpdateAsync(id, dto);

            if (!success)
                return NotFound();

            return Ok("Instructor updated successfully");
        }

        // DELETE: api/Instructor/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _instructorService.DeleteAsync(id);

            if (!success)
                return NotFound();

            return Ok("Instructor deleted successfully");
        }
    }
}