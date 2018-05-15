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
    public class AuthorsController : ApiController
    {
        private BookManagementPhuLV2Entities db = new BookManagementPhuLV2Entities();

        // GET: api/Authors
        public IQueryable<Author> GetAuthors()
        {
            return db.Authors;
        }

        // GET: api/Authors/5
        [ResponseType(typeof(Author))]
        public IHttpActionResult GetAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        // PUT: api/Authors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAuthor(int id, Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != author.AuthorID)
            {
                return BadRequest();
            }

            db.Entry(author).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        [ResponseType(typeof(Author))]
        public IHttpActionResult PostAuthor(Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Authors.Add(author);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = author.AuthorID }, author);
        }

        // DELETE: api/Authors/5
        [ResponseType(typeof(Author))]
        public IHttpActionResult DeleteAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }

            db.Authors.Remove(author);
            db.SaveChanges();

            return Ok(author);
        }

        /// <summary>
        /// Funtion Paging Author
        /// GET: /api/Authors?currentPage=1&pageSize=10&searchString=test
        /// </summary>
        /// <param name="currentPage"> current page</param>
        /// <param name="pageSize"> number record in page</param>
        /// <returns>json pageInfo: {category: array record, total: total record of category}</returns>
        [HttpGet]
        //[Route("api/Authors/{currentPage}/{pageSize}/{searchString}")]
        public IHttpActionResult PagingAuthor(int currentPage, int pageSize, string searchString)
        {
            int skip = (currentPage - 1) * pageSize;
            object pageInfo = null;

            if (String.IsNullOrEmpty(searchString))
            {
                pageInfo = new
                {
                    author = db.Authors.OrderBy(x => x.AuthorName).AsQueryable().Skip(skip).Take(pageSize).ToList(),
                    total = db.Authors.Count()
                };
            }
            else
            {
                pageInfo = new
                {
                    author = db.Authors.Where(x => x.AuthorName.Contains(searchString)).OrderBy(x => x.AuthorName).AsQueryable().Skip(skip).Take(pageSize).ToList(),
                    total = db.Authors.Count()
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

        private bool AuthorExists(int id)
        {
            return db.Authors.Count(e => e.AuthorID == id) > 0;
        }
    }
}