using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreReview.Domain
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Number { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
    }
}
