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
    public class CategoriesController : ApiController
    {
        private BookManagementPhuLV2Entities db = new BookManagementPhuLV2Entities();

        // GET: api/Categories
        public IHttpActionResult GetCategoriesName()
        {
            object cateInfo = new
            {
                CateInfo = db.Categories.Select(x =>
                new {x.CateID, x.CateName }).ToList()
            };
        return Ok(cateInfo);
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CateID)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CateID }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CateID == id) > 0;
        }

        /// <summary>
        /// Funtion Paging Category
        /// GET: /api/Categories?currentPage=1&pageSize=10&searchString=test
        /// </summary>
        /// <param name="currentPage"> current page</param>
        /// <param name="pageSize"> number record in page</param>
        /// <returns>json pageInfo: {category: array record, total: total record of category}</returns>
        [HttpGet]
        //[Route("api/Categories/{currentPage}/{pageSize}/{searchString}")]
        public IHttpActionResult PagingCategory(int currentPage, int pageSize, string searchString)
        {
            int skip = (currentPage - 1) * pageSize;
            object pageInfo = null;
            
            if (String.IsNullOrEmpty(searchString))
            {
                pageInfo = new
                {
                    category = db.Categories.OrderBy(x => x.CateName).AsQueryable().Select(x => new {
                        CateID = x.CateID,
                        CateName = x.CateName,
                        Description = x.Description
                    }).Skip(skip).Take(pageSize).ToList(),
                    total = db.Categories.Count()
                };
            }
            else
            {
                pageInfo = new
                {
                    category = db.Categories.Where(x => x.CateName.Contains(searchString)).OrderBy(x => x.CateName).AsQueryable().Select(x => new {
                        CateID = x.CateID,
                        CateName = x.CateName,
                        Description = x.Description
                    }).Skip(skip).Take(pageSize).ToList(),
                    total = db.Categories.Count()
                };
            }
            return Ok(pageInfo);
        }
    }
}