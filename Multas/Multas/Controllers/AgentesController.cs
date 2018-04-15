using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas.Models;

namespace Multas.Controllers
{
    public class AgentesController : Controller
    {//Cria uma variavel que representa a base de dados
        private MultasDb db = new MultasDb();

        // GET: Agentes
        public ActionResult Index()
        {//db.Agentes.ToList() -> em SQL: SELECT * FROM Agentes;
            //enviar para a view uma lista com todos os agentes, da base de dados
            return View(db.Agentes.ToList());
        }

        // GET: Agentes/Details/5
        public ActionResult Details(int? id)
        {
            //se se escrever 'int?' é possivel não forncer o valor para o ID e não há erro

            //proteção para o caso de não ter sido fornecido um ID válido
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //procura na bd, o agente cujo ID foi fornecido
            Agentes agentes = db.Agentes.Find(id);
            //proteão para o casod e não ter sido encontrado um agente que tenha o id fornecido
            if (agentes == null)
            {
                return HttpNotFound();
            }
            //entrega à view os dados do agente encontrado
            return View(agentes);
        }

        // GET: Agentes/Create
        public ActionResult Create()
        {
            //apresenta a view para se inserir um novo agente
            return View();
        }



        // POST: Agentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //anotador para http post
        [HttpPost]
        //anotador para proteção por roubo de identidade
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente, HttpPostedFileBase fileUploadFotografia)
        {
            //determinar o ID do novo agente
            int novoID = db.Agentes.Max(a =>a.ID)+1;
            //atribuir id ao novo agente
            agente.ID = novoID;

            //var.auxiliar
            string nomeFotografia = "Agente_" + novoID + ".jpg";
            string caminhoParaFotografia = Path.Combine(Server.MapPath("~/imagens/"),nomeFotografia);//indica onde a imagem será guardada

            //verificar se chega efetivamente um ficheiro ao servidor
            if (fileUploadFotografia != null)
            {
                //guardar o nome da imagem na BD
                agente.Fotografia = nomeFotografia;


            }
            else {
                // não há imagem
                ModelState.AddModelError("", "Não foi fornecida uma imagem...");
                return View(agente);// reenvia os dados do 'agente' para a view
            }
            
            //redimensionar a imagem --> ver em casa
            


            //escrever os dados de um novo Agente na BD

            //ModelState.IsValid -> confronta os dados fornecidos da view com as exigências do modelo
            if (ModelState.IsValid)
            {
                //adiciona o novo agente à BD
                db.Agentes.Add(agente);
                //faz commit às alterações
                db.SaveChanges();
                //guardar a imagem no disco rigido
                fileUploadFotografia.SaveAs(caminhoParaFotografia);
                //se tudo correr bem, retorna para a página de index do agente
                return RedirectToAction("Index");
            }

            //se houver um erro, reapresenta os dados do agente
            return View(agente);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            return View(agentes);
        }

        // POST: Agentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Fotografia,Esquadra")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                //neste caso já existe um agente apenas quero editar os seus dados
                db.Entry(agentes).State = EntityState.Modified;
                //efetuar commit
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            return View(agentes);
        }

        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agentes agentes = db.Agentes.Find(id);
            //delete do agente na bd
            db.Agentes.Remove(agentes);
            //commit
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
