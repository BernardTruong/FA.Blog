using FA.JustBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace FA.JustBlog.Core.Repositories
{
    internal class CommentRepository : ICommentRepository
    {
        private BlogDbContext db;
        public CommentRepository(BlogDbContext context)
        {
            this.db = context;
        }
        public void AddComment(Comment comment)
        {
            db.Add(comment);
        }

        public void AddComment(int postId, string commentName, string commentEmail, string commentHeader, string commentText)
        {
            Comment newComment = new Comment
            {
                PostId = postId,
                Name = commentName,
                Email = commentEmail,
                CommentHeader = commentHeader,
                CommentText = commentText,
                CommentTime = DateTime.Now,
            };
            db.Comments.Add(newComment);
        }

        public void DeleteComment(Comment comment)
        {
            db.Comments.Remove(comment);
        }

        public void DeleteComment(int commendId)
        {
            Comment commentToDelete = Find(commendId);
            if (commentToDelete != null)
            {
                db.Comments.Remove(commentToDelete);
            }
        }

        public Comment Find(int commentId)
        {
            return db.Comments.FirstOrDefault(c => c.Id == commentId);
        }

        public IList<Comment> GetAllComments()
        {
            return db.Comments.ToList();
        }

        public IList<Comment> GetCommentsForPost(int postId)
        {
            return db.Comments.Where(c => c.PostId == postId).ToList();
        }

        public IList<Comment> GetCommentsForPost(Post post)
        {
            return GetCommentsForPost(post.Id);
        }

        public void UpdateComment(Comment comment)
        {
            Comment existingComment = db.Comments.Find(comment.Id);
            if (existingComment != null)
            {
                existingComment.Name = comment.Name;
                existingComment.Email = comment.Email;
                existingComment.CommentHeader = comment.CommentHeader;
                existingComment.CommentText = comment.CommentText;
                existingComment.CommentTime = comment.CommentTime;
            }
        }
    }
}
