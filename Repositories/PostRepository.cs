using FA.JustBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogDbContext db;
        public PostRepository(BlogDbContext context)
        {
            this.db = context;
        }

        public void AddPost(Post post)
        {
            ValidatePost(post);
            db.Posts.Add(post);
            db.SaveChanges();
        }

        public void AddPost(string Id_, string Title, string description, string PostContent, string UrlSlug, bool Published, string PostedOn, bool Modified, string CategoryId)
        {
            var post = new Post
            {
                Id = ValidateNumberFormat(Id_, "ID"),
                Title = Title,
                Description = description,
                Content = PostContent,
                UrlSlug = UrlSlug,
                Published = Published,
                PostedOn = ValidateDateTimeFormat(PostedOn),
                Modified = Modified,
                CategoryId = ValidateNumberFormat(CategoryId, "CategoryId"),
            };
            AddPost(post);
        }

        public int CountPostsForCategory(string category)
        {
            var cateEntity = db.Categories.Where(s => s.Name == category).FirstOrDefault();
            var postList = db.Posts.Where(s => s.CategoryId == cateEntity.Id);
            return postList.Count();
        }

        public int CountPostsForTag(string tag)
        {
            var tagEntity = db.Tags.Where(s => s.Name == tag).FirstOrDefault();
            var postList = db.Posts.Where(s => s.CategoryId == tagEntity.TagId);
            return postList.Count();
        }

        public void DeletePost(Post post)
        {
            db.Posts.Remove(post);
            db.SaveChanges();
        }

        public void DeletePostById(int postId)
        {
            var entityToDelete = db.Posts.Find(postId);
            if (entityToDelete != null)
            {
                db.Posts.Remove(entityToDelete);
                db.SaveChanges();
            }
        }

        public Post FindPost(int year, int month, int id)
        {
            return db.Posts.FirstOrDefault(post => post.PostedOn.Year == year && post.PostedOn.Month == month && post.Id == id);
        }

        public Post FindPost(int postId)
        {
            return db.Posts.Where(s => s.Id == postId).Include(s => s.Category).Include(s => s.PostTag).ThenInclude(s => s.Tag).FirstOrDefault(); ;
        }

        public IList<Post> GetAllPosts()
        {
            return db.Set<Post>().Include(s => s.Category).Include(s => s.PostTag).ThenInclude(s => s.Tag).ToList();
        }

        public IList<Post> GetLatestPost(int size)
        {
            return db.Posts.OrderByDescending(p => p.PostedOn).Include(s => s.Category).Include(s => s.PostTag).ThenInclude(s => s.Tag).Take(size).ToList();
        }

        public IList<Post> GetPostsByCategory(string category)
        {
            Category category_ = db.Categories.Where(s => s.Name == category).FirstOrDefault();
            return db.Posts.Where(s => s.CategoryId == category_.Id).Include(s => s.Category).Include(s => s.PostTag).ThenInclude(s => s.Tag).ToList();
        }

        public IList<Post> GetPostsByMonth(int monthYear)
        {
            return db.Posts.Where(post => post.PostedOn.Month == monthYear).ToList();
        }

        public IList<Post> GetPostsByTag(string tag)
        {
            List<Post> PostList = new List<Post>();
            var tagEntity = db.Tags.Where(s => s.Name == tag).FirstOrDefault();
            var ListbyTag = db.PostTag.Where(s => s.TagId == tagEntity.TagId).ToList();
            foreach ( var post in ListbyTag) 
            {
                var PostEntity = db.Posts.Where(s => s.Id == post.PostId).Include(s => s.Category).Include(s => s.PostTag).ThenInclude(s => s.Tag).FirstOrDefault();
                PostList.Add(PostEntity);
            }
            return PostList;
        }

        public IList<Post> GetPublisedPosts()
        {
            return db.Posts.Where(s => s.Published == true).Include(s => s.Category).Include(s => s.PostTag).ThenInclude(s => s.Tag).ToList();
        }

        public IList<Post> GetUnpublisedPosts()
        {
            return db.Posts.Where(s => s.Published == false).Include(s => s.Category).Include(s => s.PostTag).ThenInclude(s => s.Tag).ToList();
        }

        public void UpdatePost(Post post)
        {

            var existingPost = db.Posts.Find(post.Id);
            var postTagList = db.PostTag.Where(s=> s.PostId == post.Id).ToList();

            if (existingPost != null)
            {
                existingPost.Title = post.Title;
                existingPost.Description = post.Description;
                existingPost.Content = post.Content;
                existingPost.UrlSlug = post.UrlSlug;
                existingPost.Published = post.Published;
                existingPost.PostedOn = post.PostedOn;
                existingPost.Modified = post.Modified;
                existingPost.UrlSlug = post.UrlSlug;
                existingPost.CategoryId = post.CategoryId;
                existingPost.PostTag = post.PostTag;

                foreach (var postTag in postTagList)
                {
                    db.PostTag.Remove(postTag);
                }
                db.Entry(existingPost).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public IList<Post> GetMostViewedPost(int size)
        {
            return db.Posts.OrderByDescending(p => p.ViewCount).Include(s => s.Category).Include(s => s.PostTag).ThenInclude(s => s.Tag).Take(size).ToList();
        }

        public IList<Post> GetHighestPosts(int size)
        {
            return db.Posts.OrderByDescending(p => p.RateCount).Take(size).ToList();
        }
        public void Dispose()
        {
            db.Dispose();
        }

        private static DateTime ValidateDateTimeFormat(string PostedOn)
        {
            if (DateTime.TryParseExact(PostedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;

            Console.WriteLine("PostedOn field is not correct datetime format!");
            throw new FormatException("PostedOn field is not correct datetime format!");
        }

        private static int ValidateNumberFormat(string number, string fieldName)
        {
            try
            {               
                return Convert.ToInt32(number); ;
            }
            catch (Exception)
            {
                Console.WriteLine($"{fieldName} field is not correct integer number format");
                throw new FormatException($"{fieldName} field is not correct integer number format");
            }
        }

        private static void ValidatePost(Post post)
        {
            var validationContext = new ValidationContext(post, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(post, validationContext, validationResults, validateAllProperties: true))
            {
                // Validation failed
                var errorMessage = string.Join(Environment.NewLine, validationResults.Select(result => result.ErrorMessage));
                Console.WriteLine(errorMessage);
                throw new ValidationException(errorMessage);
            }
        }
    }
}
