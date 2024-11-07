using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class BlogPostsController : Controller
{
    private readonly IBlogPostRepository blogPostRepository;
    
    public BlogPostsController(IBlogPostRepository BlogPostRepository)
    {
        this.blogPostRepository = BlogPostRepository;
    }
    
    
    [HttpPost]
    public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
    {
        var blogPost = new BlogPost
        {
            Author = request.Author,
            Content = request.Content,
            FeaturedImageUrl = request.FeaturedImageUrl,
            IsVisible = request.IsVisible,
            PublishedDate = request.PublishedDate,
            ShortDescription = request.ShortDescription,
            Title = request.Title,
            UrlHandle = request.UrlHandle,
        };
        
        blogPost = await blogPostRepository.CreateAsync(blogPost);

        var response = new BlogPostDto
        {
            Id = blogPost.Id,
            Author = blogPost.Author,
            Content = blogPost.Content,
            FeaturedImageUrl = blogPost.FeaturedImageUrl,
            IsVisible = blogPost.IsVisible,
            PublishedDate = blogPost.PublishedDate,
            ShortDescription = blogPost.ShortDescription,
            Title = blogPost.Title,
            UrlHandle = blogPost.UrlHandle
        };
        
        return Ok(response);

    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts()
    {
        var blogPosts = await blogPostRepository.GetAllAsync();

        var response = new List<BlogPostDto>();
        foreach (var blogPost in blogPosts)
        {
            response.Add(new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle
            });
        }
        return Ok(response);
    }
    
    
}