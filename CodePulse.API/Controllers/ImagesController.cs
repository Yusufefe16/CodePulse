﻿using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : Controller
{
    
    private readonly IImageRepository imageRepository;
    public ImagesController(IImageRepository imageRepository)
    {
        this.imageRepository = imageRepository;
    }
    //GET: {apibaseurl}/api/images
    [HttpGet]
    public async Task<IActionResult> GetAllImages()
    {
        var Images = await imageRepository.GetAll();
        
        // Domain model to DTO

        var response = new List<BlogImageDto>();
        foreach (var Image in Images)
        {
            response.Add(new BlogImageDto
            {
                Id = Image.Id,
                DateCreated = Image.DateCreated,
                FileName = Image.FileName,
                Title = Image.Title,
                FileExtension = Image.FileExtension,
                Url = Image.Url
            });
        }
        
        return Ok(response);
    }
    
    
    //POST: {apibaseurl}/api/images
    [HttpPost]
    public async Task<IActionResult> UploadImages(
        IFormFile file,
        [FromForm] string fileName, 
        [FromForm] string title
        )
    {
        ValidateFileUpload(file);
        if (ModelState.IsValid)
        {
            //File upload
            var blogImage = new BlogImage
            {
                FileExtension = Path.GetExtension(file.FileName).ToLower(),
                FileName = fileName,
                Title = title,
                DateCreated = DateTime.Now
            };
            
            blogImage = await imageRepository.Upload(file, blogImage);

            //Domain Modal to DTO

            var response = new BlogImageDto
            {
                Id = blogImage.Id,
                DateCreated = blogImage.DateCreated,
                FileName = blogImage.FileName,
                Title = blogImage.Title,
                FileExtension = blogImage.FileExtension,
                Url = blogImage.Url
            };
            
            return Ok(response);
        }
        
        return BadRequest(ModelState);
    }

    private void ValidateFileUpload(IFormFile file)
    {
        var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png"};

        if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
        {
            ModelState.AddModelError("file", "Unsupported file format");
        }

        if (file.Length > 10485760)
        {
            ModelState.AddModelError("file", "File size cannot be more than 10MB");
        }
    }
}