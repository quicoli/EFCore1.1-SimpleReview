using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreReview.Domain
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AuthorBook> Books { get; set; }

        public Author()
        {
            Books = new List<AuthorBook>();
        }
    }
}
