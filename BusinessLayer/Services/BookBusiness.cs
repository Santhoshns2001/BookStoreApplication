using BusinessLayer.Interfaces;
using ModelLayer;
using RepositaryLayer.Entities;
using RepositaryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class BookBusiness : IBookBuss
    {

        private readonly IBookRepo bookRepo;

        public BookBusiness(IBookRepo bookRepo)
        {
            this.bookRepo = bookRepo;
        }
        public Book AddBook(BookModel bookModel)
        {
           return bookRepo.AddBook(bookModel);
        }

       public  List<Book> GetAllBooks()
        {
           return bookRepo.GetAllBooks();
        }

       public  Book GetByBookId(int bookId)
        {
           return bookRepo.GetByBookId(bookId);
        }

        public Book UpdateBook(int bookId, BookModel bookModel)
        {
            return bookRepo.UpdateBook(bookId, bookModel);
        }

        public bool DeleteBookById(int bookId)
        {
           return bookRepo.DeleteBookById(bookId);
        }

        public List<Book> FetchByAuthorOrTitle(string author, string title)
        {
           return bookRepo.FetchByAuthorOrTitle(author, title);
        }
    }
}
