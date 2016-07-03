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
using Certified_Label.Models;

namespace Certified_Label.Controllers
{
    public class HttpRequestsController : ApiController
    {
        private Certified_LabelEntities db = new Certified_LabelEntities();

        // GET: api/HttpRequests
        public IQueryable<HttpRequest> GetHttpRequests()
        {
            return db.HttpRequests;
        }

        // GET: api/HttpRequests/5
        [ResponseType(typeof(HttpRequest))]
        public IHttpActionResult GetHttpRequest(int id)
        {
            HttpRequest httpRequest = db.HttpRequests.Find(id);
            if (httpRequest == null)
            {
                return NotFound();
            }

            return Ok(httpRequest);
        }

        // PUT: api/HttpRequests/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHttpRequest(int id, HttpRequest httpRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != httpRequest.RequestID)
            {
                return BadRequest();
            }

            db.Entry(httpRequest).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HttpRequestExists(id))
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

        // POST: api/HttpRequests
        [ResponseType(typeof(HttpRequest))]
        public IHttpActionResult PostHttpRequest(HttpRequest httpRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HttpRequests.Add(httpRequest);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = httpRequest.RequestID }, httpRequest);
        }

        // DELETE: api/HttpRequests/5
        [ResponseType(typeof(HttpRequest))]
        public IHttpActionResult DeleteHttpRequest(int id)
        {
            HttpRequest httpRequest = db.HttpRequests.Find(id);
            if (httpRequest == null)
            {
                return NotFound();
            }

            db.HttpRequests.Remove(httpRequest);
            db.SaveChanges();

            return Ok(httpRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HttpRequestExists(int id)
        {
            return db.HttpRequests.Count(e => e.RequestID == id) > 0;
        }
    }
}