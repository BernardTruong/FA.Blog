using FA.JustBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private BlogDbContext db;
        public CategoryRepository(BlogDbContext context)
        {
            this.db = context;
        }
        public void AddCategory(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();

        }

        public void DeleteCategory(Category category)
        {
            db.Categories.Remove(category);
            db.SaveChanges();
        }

        public void DeleteCategory(int categoryId)
        {
            var entityToDelete = db.Categories.Find(categoryId);
            if (entityToDelete != null)
            {
                db.Categories.Remove(entityToDelete);
                db.SaveChanges();
            }
        }

        public Category Find(int categoryId)
        {
            return db.Categories.Find(categoryId);
        }

        public IList<Category> GetAllCategories()
        {
            return db.Set<Category>().ToList();
        }

        public void UpdateCategory(Category category)
        {
            var existingCategory = db.Categories.Find(category.Id);

            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.UrlSlug = category.UrlSlug;
                existingCategory.Description = category.Description;

                db.Entry(existingCategory).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
