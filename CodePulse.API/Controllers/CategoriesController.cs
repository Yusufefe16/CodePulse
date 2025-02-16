﻿using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryRepository categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        /*[Authorize(Roles = "Writer")]*/
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {

            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            
            await categoryRepository.CreateAsync(category);

            var response = new CategoryDto
            {
                Id = category.Id,
                UrlHandle = category.UrlHandle,
                Name = category.Name
            };
            
            return Ok(response);
        }

        [HttpGet] //https://localhost:44313/api/Categories?query=html&sortBy=name&sortDirection=desc
        public async Task<IActionResult> GetAllCategories(
            [FromQuery] string? query, 
            [FromQuery] string? sortBy, 
            [FromQuery] string? sortDirection,
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null
            )
        {
            var categories = await categoryRepository.GetAllAsync(query, sortBy, sortDirection, pageNumber, pageSize);
            // Map Domain modal to DTO 
            
            var response = new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }

            return Ok(response);

        }

        [HttpGet]//https://localhost:44313/api/Categories/{id}
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);

            if (existingCategory is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                UrlHandle = existingCategory.UrlHandle,
                Name = existingCategory.Name
            };
            return Ok(response);
        }
        
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDTO request)
        {
            // Convert DTO to Domain Model    

            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            
            category = await categoryRepository.UpdateAsync(category);

            if (category == null)
            {
                return NotFound();
            }

            var response = new Category
            {
                Id = category.Id,
                UrlHandle = category.UrlHandle,
                Name = category.Name
            };
            
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            
            // Convert Domain model to DTO
            var response = new Category
            {
                Id = category.Id,
                UrlHandle = category.UrlHandle,
                Name = category.Name
            };
            
            return Ok(response);
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCategoriesTotal()
        {
            var count = await categoryRepository.GetCount();
            return Ok(count);
        }
            
    }
}
