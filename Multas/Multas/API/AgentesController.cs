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
using Multas.Models;

namespace Multas.API
{
    public class AgentesController : ApiController
    {
        private MultasDb db = new MultasDb();

        // GET: api/Agentes
        public IHttpActionResult GetAgentes()//busca listagem
        {
            
            var resultado=db.Agentes.Select(agente => new { agente.ID, agente.Nome, agente.Fotografia, agente.Esquadra}).ToList();
            return Ok(resultado);
        }

        // GET: api/Agentes/5
        [ResponseType(typeof(Agentes))]
        public IHttpActionResult GetAgentes(int id)//busca apenas 1 lista
        {
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return NotFound();
            }

            return Ok(agentes);
        }

        // PUT: api/Agentes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAgentes(int id, Agentes agentes)//faz uodate a um agente
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != agentes.ID)
            {
                return BadRequest();
            }

            db.Entry(agentes).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentesExists(id))
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

        // POST: api/Agentes
        [ResponseType(typeof(Agentes))]
        public IHttpActionResult PostAgentes(Agentes agentes)//cria um agente
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Agentes.Add(agentes);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AgentesExists(agentes.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = agentes.ID }, agentes);
        }

        // DELETE: api/Agentes/5
        [ResponseType(typeof(Agentes))]
        public IHttpActionResult DeleteAgentes(int id)//elimina um agente
        {
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return NotFound();
            }

            db.Agentes.Remove(agentes);
            db.SaveChanges();

            return Ok(agentes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AgentesExists(int id)
        {
            return db.Agentes.Count(e => e.ID == id) > 0;
        }
    }
}