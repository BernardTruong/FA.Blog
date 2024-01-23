using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Models
{
    public class Post
    {
        
        public int Id { get; set; }
        
        [MaxLength(100)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$")]
        public string Title { get; set; }

        [MaxLength(100)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$")]
        public string? Description { get; set; }

        [MaxLength(1000)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$")]
        public string Content { get; set; }

        [MaxLength(100)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$")]
        public string UrlSlug { get; set; }
        public bool Published {  get; set; }

        public DateTime PostedOn { get; set; }
        public bool Modified { get; set; }

        public int ViewCount { get; set; }

        public int RateCount { get; set; }

        public int TotalRate { get; set; }

        public decimal Rate
        {
            get
            {
                if (RateCount != 0)
                {
                    return TotalRate / RateCount;
                }
                else
                {
                    return 0;
                }
            }
        }



        public int? CategoryId {  get; set; }
        public Category? Category { get; set; }
        public IList<PostTag>? PostTag { get; set; }
    }
}
