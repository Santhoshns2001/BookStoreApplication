using ModelLayer;
using RepositaryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IBookBuss
    {
        public Book AddBook(BookModel bookModel);
        public List<Book> GetAllBooks();
        public Book GetByBookId(int bookId);
        public Book UpdateBook(int bookId, BookModel bookModel);
        public bool DeleteBookById(int bookId);
        public List<Book> FetchByAuthorOrTitle(string author, string title);
        public Book FindByBookId( int bookId, string Title, string author, string description, int originalprice, int disPercentage, int quantity, string image);



    }
}
