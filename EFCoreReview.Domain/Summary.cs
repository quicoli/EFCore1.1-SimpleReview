using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreReview.Domain
{
    public class Summary
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
