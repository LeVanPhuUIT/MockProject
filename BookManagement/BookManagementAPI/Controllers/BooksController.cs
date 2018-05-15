using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BookManagementAPI.Models;

namespace BookManagementAPI.Controllers
{
    public class BooksController : ApiController
    {
        private BookManagementPhuLV2Entities db = new BookManagementPhuLV2Entities();

        // GET: api/Books
        public IQueryable<Book> GetBooks()
        {
            try
            {
                return db.Books;
            }
            catch (Exception ex)
            {
                Console.WriteLine("-------------" + ex.Message);
                return null;
            }
        }

        // GET: api/Books/5
        [ResponseType(typeof(Book))]
        public IHttpActionResult GetBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.BookID)
            {
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Books
        [ResponseType(typeof(Book))]
        public IHttpActionResult PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = book.BookID }, book);
        }

        // DELETE: api/Books/5
        [ResponseType(typeof(Book))]
        public IHttpActionResult DeleteBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return Ok(book);
        }

        /// <summary>
        /// Funtion Paging Book
        /// GET: /api/Authors?currentPage=1&pageSize=10&searchString=test
        /// </summary>
        /// <param name="currentPage"> current page</param>
        /// <param name="pageSize"> number record in page</param>
        /// <returns>json pageInfo: {category: array record, total: total record of category}</returns>
        [HttpGet]
        //[Route("api/Books/{currentPage}/{pageSize}/{searchString}")]
        public IHttpActionResult PagingAuthor(int currentPage, int pageSize, string searchString)
        {
            int skip = (currentPage - 1) * pageSize;
            object pageInfo = null;

            if (String.IsNullOrEmpty(searchString))
            {
                pageInfo = new
                {
                    book = db.Books.OrderBy(x => x.Title).AsQueryable().Skip(skip).Take(pageSize).ToList(),
                    total = db.Books.Count()
                };
            }
            else
            {
                pageInfo = new
                {
                    author = db.Books.Where(x => x.Title.Contains(searchString)).OrderBy(x => x.Title).AsQueryable().Skip(skip).Take(pageSize).ToList(),
                    total = db.Books.Count()
                };
            }
            return Ok(pageInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.BookID == id) > 0;
        }
    }
}