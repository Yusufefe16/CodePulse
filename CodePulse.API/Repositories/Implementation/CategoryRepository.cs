﻿using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext dbContext;
    
    public CategoryRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<Category> CreateAsync(Category category)
    {
        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();
        
        return category;
    }

    public async Task<IEnumerable<Category>> GetAllAsync(
        string? query = null, 
        string? sortBy = null, 
        string? sortDirection = null,
        int? pageNumber = 1,
        int? pageSize = 100
        )
    {
        //Query
        var categories = dbContext.Categories.AsQueryable();

        //Filter

        if (string.IsNullOrWhiteSpace(query) == false)
        {
            categories = categories.Where(c => c.Name.Contains(query));
        }
        
        //Sorting

        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
            {
                var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) 
                    ? true : false;
                categories = isAsc ? categories.OrderBy(c => c.Name) : categories.OrderByDescending(c => c.Name);
            }
            if (string.Equals(sortBy, "URL", StringComparison.OrdinalIgnoreCase))
            {
                var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) 
                    ? true : false;
                categories = isAsc ? categories.OrderBy(c => c.UrlHandle) : categories.OrderByDescending(c => c.UrlHandle);
            }
        }
        
        //Pagination
        
        var skipResults = (pageNumber -1) * pageSize;

        categories = categories.Skip(skipResults ?? 0).Take(pageSize ?? 100);

        return await categories.ToListAsync();
    }
    public async Task<Category> GetById(Guid id)
    {
        return await dbContext.Categories.FirstOrDefaultAsync(x=> x.Id == id);
    }
    public async Task<Category> UpdateAsync(Category category)
    {
        var existingCategory =  await dbContext.Categories.FirstOrDefaultAsync(x=> x.Id == category.Id);

        if (existingCategory != null)
        {
            dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        return null;
    }
    public async Task<Category> DeleteAsync(Guid id)
    {
        var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x=> x.Id == id);

        if (existingCategory is null)
        {
            return null;
        }
        
        dbContext.Categories.Remove(existingCategory);
        await dbContext.SaveChangesAsync();

        return existingCategory;
    }

    public async Task<int> GetCount()
    {
        return await dbContext.Categories.CountAsync();
    }
}