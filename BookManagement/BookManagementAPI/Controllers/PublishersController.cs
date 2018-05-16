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
using System.Threading.Tasks;
using System.Web;

namespace BookManagementAPI.Controllers
{
    public class PublishersController : ApiController
    {
        private BookManagementPhuLV2Entities db = new BookManagementPhuLV2Entities();

        // GET: api/Publishers
        [ResponseType(typeof(Publisher))]
        public IHttpActionResult GetPublisherName()
        {
            object publisherInfo = new
            {
                PublisherInfo = db.Publishers.Select(x =>
                new { x.PubID, x.Name }).ToList()
            };
            return Ok(publisherInfo);
        }

        // GET: api/Publishers/5
        [ResponseType(typeof(Publisher))]
        public IHttpActionResult GetPublisher(int id)
        {
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(publisher);
        }

        // PUT: api/Publishers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPublisher(int id, Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != publisher.PubID)
            {
                return BadRequest();
            }

            db.Entry(publisher).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
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

        // POST: api/Publishers
        [ResponseType(typeof(Publisher))]
        public IHttpActionResult PostPublisher(Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Publishers.Add(publisher);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = publisher.PubID }, publisher);
        }

        // DELETE: api/Publishers/5
        [ResponseType(typeof(Publisher))]
        public IHttpActionResult DeletePublisher(int id)
        {
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return NotFound();
            }

            db.Publishers.Remove(publisher);
            db.SaveChanges();

            return Ok(publisher);
        }

        /// <summary>
        /// Funtion Paging Publisher
        /// GET: /api/Publishers?currentPage=1&pageSize=10&searchString=test
        /// </summary>
        /// <param name="currentPage"> current page</param>
        /// <param name="pageSize"> number record in page</param>
        /// <returns>json pageInfo: {Publisher: array record, total: total record of Publisher}</returns>
        [HttpGet]
        //[Route("api/Publishers/{currentPage}/{pageSize}/{searchString}")]
        public IHttpActionResult PagingPublisher(int currentPage, int pageSize, string searchString)
        {
            int skip = (currentPage - 1) * pageSize;
            object pageInfo = null;

            if (String.IsNullOrEmpty(searchString))
            {
                pageInfo = new
                {
                    publisher = db.Publishers.OrderBy(x => x.Name).AsQueryable().Skip(skip).Take(pageSize).ToList(),
                    total = db.Publishers.Count()
                };
            }
            else
            {
                pageInfo = new
                {
                    publisher = db.Publishers.Where(x => x.Name.Contains(searchString)).OrderBy(x => x.Name).AsQueryable().Skip(skip).Take(pageSize).ToList(),
                    total = db.Publishers.Count()
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

        private bool PublisherExists(int id)
        {
            return db.Publishers.Count(e => e.PubID == id) > 0;
        }
    }
}