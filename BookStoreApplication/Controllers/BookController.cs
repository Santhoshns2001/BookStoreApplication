using BusinessLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositaryLayer.Entities;

namespace BookStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookBuss bookBuss;
      
        public BookController(IBookBuss bookBuss,IBus bus)
        {
            this.bookBuss = bookBuss;
        }


        [HttpPost]
        [Route("AddBook")]
        public IActionResult AddBook(BookModel model) 
        {
            Book book=bookBuss.AddBook(model);

            if (book != null)
            {
                return Ok(new ResponseModel<Book> { IsSuccuss = true, Message = "book added succussfully", Data = book });
            }
            else
            {
                return BadRequest(new ResponseModel<Book> { IsSuccuss = false, Message = " failed to add book", Data = book });
            }

        }

        [HttpGet]
        [Route("GetAllBooks")]
        public IActionResult GetAllBooks()
        {
            List<Book> books = bookBuss.GetAllBooks();

            if (books != null)
            {
                return Ok(new ResponseModel<List<Book>> { IsSuccuss = true, Message = "list of books fetched succussfully", Data = books });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "something went wrong" });
            }
        }

        [HttpGet("GetById")]
        public IActionResult GetByBookId(int bookId)
        {
            Book book=bookBuss.GetByBookId(bookId);
            if (book != null)
            {
                return Ok(new ResponseModel<Book> { IsSuccuss = true, Message = "book fetched succussfully", Data = book });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "wrong book id is been provided " });
            }
        }

        [HttpPut("UpdateBook")]
        public IActionResult UpdateBook(int bookId,BookModel model)
        {
            Book book=bookBuss.UpdateBook(bookId,model);
            if (book != null)
            {
                return Ok(new ResponseModel<Book> { IsSuccuss = true, Message = "book updated  succussfully", Data = book });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to update", Data = "wrong input is been provided " });
            }

        }

      

        [HttpDelete("DeleteBook")]
        public IActionResult DeleteByBookId(int bookId)
        {
            bool res=bookBuss.DeleteBookById(bookId);
            if (res)
            {
                return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "book is been deleted", Data = "deletion succuss" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to delete", Data = "wrong book id is been provided " });
            }

        }




        // 1) Find the book using any two columns of table.

        [HttpGet("FetchByAuthorOrTitle")]
        public IActionResult FetchByAuthorOrTitle(string? author, string? title)
        {
            List<Book> books = bookBuss.FetchByAuthorOrTitle(author, title);
            if (books != null)
            {
                return Ok(new ResponseModel<List<Book>> { IsSuccuss = true, Message = "book fetched  succussfully", Data = books });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = " no matched input" });
            }

        }

        // 2)Find the data using bookid, if it exst update the data else insert the new book record.

        [HttpGet("FindByBookId")]
        // public IActionResult FindByBookId(int bookId, BookModel model)
        public IActionResult FindByBookId(int bookId, string Title,string author,string description,int originalprice,int disPercentage,int quantity,string image)
        {
            Book book = bookBuss.FindByBookId(bookId,Title,author,description,originalprice,disPercentage,quantity,image);
            if (book != null)
            {
                return Ok(new ResponseModel<Book> { IsSuccuss = true, Message = "book fetched succussfully", Data = book });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "wrong book id is been provided " });
            }
        }



    }
}
