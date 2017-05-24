using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreReview.Domain
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Chapter> Chapters { get; set; }
        public List<AuthorBook> Authors { get; set; }

        public Summary Summary { get; set; }

        public Book()
        {
            Chapters = new List<Chapter>();
            Authors = new List<AuthorBook>();
        }
    }
}
