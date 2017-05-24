using EFCoreReview.Data;
using EFCoreReview.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreReview.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Single object operations:
            //InsertBook();
            //InsertMultipleBooks();
            //RetrieveAndUpdateBook();
            //DeleteTrackedObject();
            //DeleteUntrackedObject();
            //RunSQLSentence();
            //QueryUsingFunctions();

            //Complex object operations
            //AddBook();
            //EagerUsingInclude();
            //ProjectionsWithRelations();
            //EagerByProjections();
            ExplictLoading();

            Console.ReadLine();
        }

        #region single object operations
        private static string ReverseStr(string value)
        {
            var result = value.AsEnumerable();
            return string.Concat(result.Reverse());
        }

        private static void QueryUsingFunctions()
        {
            using (var context = new BookContext())
            {
                var books = context.Books
                    .Select(x => new { newTitle = ReverseStr(x.Title) })
                    .ToList();
                books.ForEach(x => Console.WriteLine(x.newTitle));
            }
        }

        private static void RunSQLSentence()
        {
            using (var context = new BookContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var books = context.Books.FromSql("select * from Books")
                    .Where(x => x.Title.Contains("Agile"))
                    .OrderByDescending(x => x.Title)
                    .ToList();
                books.ForEach(x => Console.WriteLine(x.Title));
            }
        }

        private static void DeleteTrackedObject()
        {
            using (var context = new BookContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var book = context.Books.FirstOrDefault();
                context.Books.Remove(book);
                // or
                //context.Remove(book);
                //context.Entry(book).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private static void DeleteUntrackedObject()
        {
            Book book = null;
            using (var context = new BookContext())
            {
                book = context.Books.FirstOrDefault();
            }

            using (var newContext = new BookContext())
            {
                newContext.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                newContext.Remove(book);
                newContext.SaveChanges();
            }
        }

        private static void RetrieveAndUpdateBook()
        {
            using (var context = new BookContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var book = context.Books.FirstOrDefault();
                book.Title += " -updated book";
                context.SaveChanges();
            }
        }

        private static void InsertMultipleBooks()
        {
            var book = new Book() { Title = "C# 7 and .NET Core: Moder Cross-Platform Development" };
            var otherbook = new Book() { Title = "Agile Coaching" };
            using (var context = new BookContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                //We can do:
                // 1. Call Add twice
                //context.Books.Add(book);
                //context.Books.Add(otherbook);
                // 2. Call AddRange and pass the objects
                //context.Books.AddRange(book, otherbook);
                // 3. Call AddRange and pass a list of Book
                context.Books.AddRange(new List<Book>() { book, otherbook });

                context.SaveChanges();
            }
        }

        private static void InsertBook()
        {
            var book = new Book() { Title = "C# 7 and .NET Core: Moder Cross-Platform Development" };
            using (var context = new BookContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                context.Books.Add(book);
                context.SaveChanges();
            }
        }

    #endregion


        private static void AddBook()
        {
            var book = new Book()
            {
                Title = "Enterprise Applicatio Patterns Using Xamarin.Forms",
                Chapters = new List<Chapter>
                {
                    new Chapter() {Number = 1, Title = "Introduction"},
                    new Chapter() {Number = 2, Title = "MVVM"},
                    new Chapter() {Number = 3, Title = "Dependency Injection"},
                    new Chapter() {Number = 4, Title = "Navigation"}
                }
            };

            using (var context = new BookContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                context.Add(book);
                context.SaveChanges();
            }
        }

        private static void EagerUsingInclude()
        {

            using (var context = new BookContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var bookList = context.Books
                    .Include(x => x.Chapters)
                    .ToList();
            }
        }

        private static void ProjectionsWithRelations()
        {
            using (var context = new BookContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var bookList = context.Books
                    .Select(x => new
                    {
                        x.Id,
                        x.Title,
                        ChapertCount = x.Chapters.Count
                    }).ToList();
            }
        }

        private static void EagerByProjections()
        {
            using (var context = new BookContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var bookList = context.Books
                    .Select(x => new
                    {
                        Book = x,
                        Chapters = x.Chapters
                    }).ToList();
                Console.WriteLine(bookList.Count());
            }
        }

        private static void ProjectionSubQueries()
        {
            using (var context = new BookContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var bookList = context.Books
                    .Select(x => new
                    {
                        Book = x,
                        Chapters = x.Chapters.Where(c => c.Number == 2).ToList()
                    }).ToList();
                Console.WriteLine(bookList.Count());
            }
        }

        private static void ExplictLoading()
        {
            using (var context = new BookContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var book = context.Books.LastOrDefault();
                context.Entry(book).Collection(x => x.Chapters)
                    .Query()
                    .Where(q => q.Number == 2)
                    .Load();
                Console.WriteLine(book.Id);
            }
        }

    }
}
