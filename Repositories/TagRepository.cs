using FA.JustBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public class TagRepository : ITagRepository
    {

        private readonly BlogDbContext db;
        public TagRepository(BlogDbContext Context)
        {
            this.db = Context;
        }
        public void AddTag(Tag Tag)
        {
            db.Tags.Add(Tag);
            db.SaveChanges();
        }

        public void DeleteTag(Tag Tag)
        {
            db.Tags.Remove(Tag);
            db.SaveChanges();
        }

        public void DeleteTag(int TagId)
        {
            var entityToDelete = db.Tags.Find(TagId);
            if (entityToDelete != null)
            {
                db.Tags.Remove(entityToDelete);
                db.SaveChanges();
            }
        }

        public Tag Find(int TagId)
        {
            return db.Tags.Find(TagId);
        }

        public IList<Tag> GetAllTags()
        {
            return db.Set<Tag>().ToList();
        }

        public IList<Tag> GetPopularTag(int numberToTake)
        {
            List<Tag> tags = new List<Tag>();
            var popularTags = db.PostTag
                .GroupBy(pt => pt.TagId)
                .OrderByDescending(g => g.Count())
                .Take(numberToTake)
                .Select(g => new
                {
                    TagId = g.Key,       
                    TagCount = g.Count()  
                })
                .ToList();
            foreach (var tag in popularTags)
            {
                var popularTag = db.Tags.Where(s => s.TagId ==  tag.TagId).FirstOrDefault();
                tags.Add(popularTag);
            }
            return tags;

        }

        public Tag GetTagByUrlSlug(string urlSlug)
        {
            return db.Tags.Where(tag => tag.UrlSlug == urlSlug).FirstOrDefault();
        }

        public IList<Tag> GetTagByPostId(int postId)
        {
            var listTagId = db.PostTag.Where(s => s.PostId == postId).Include(s=>s.Tag)
                .Select(s => new
                Tag{
                    TagId = s.Tag.TagId,
                    Name = s.Tag.Name,
                    UrlSlug = s.Tag.UrlSlug,
                    Description = s.Tag.Description,
                    Count = s.Tag.Count
                });
            return listTagId.ToList();
        }

        public void UpdateTag(Tag Tag)
        {
            var existingTag = db.Tags.Find(Tag.TagId);

            if (existingTag != null)
            {
                existingTag.Name = Tag.Name;
                existingTag.UrlSlug = Tag.UrlSlug;
                existingTag.Description = Tag.Description;
                existingTag.Count = Tag.Count;

                db.Entry(existingTag).State = EntityState.Modified;
                db.SaveChanges();
            }

        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
