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
    public class CasesController : ApiController
    {
        private Certified_LabelEntities db = new Certified_LabelEntities();

        // GET: api/Cases
        public IQueryable<Case> GetCases()
        {
            return db.Cases;
        }

        // GET: api/Cases/5
        [ResponseType(typeof(Case))]
        public IHttpActionResult GetCase(string id)
        {
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return NotFound();
            }

            return Ok(@case);
        }

        // PUT: api/Cases/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCase(string id, Case @case)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @case.CaseNumber)
            {
                return BadRequest();
            }

            db.Entry(@case).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaseExists(id))
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

        // POST: api/Cases
        [ResponseType(typeof(Case))]
        public IHttpActionResult PostCase(Case @case)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cases.Add(@case);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CaseExists(@case.CaseNumber))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = @case.CaseNumber }, @case);
        }

        // DELETE: api/Cases/5
        [ResponseType(typeof(Case))]
        public IHttpActionResult DeleteCase(string id)
        {
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return NotFound();
            }

            db.Cases.Remove(@case);
            db.SaveChanges();

            return Ok(@case);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CaseExists(string id)
        {
            return db.Cases.Count(e => e.CaseNumber == id) > 0;
        }
    }
}