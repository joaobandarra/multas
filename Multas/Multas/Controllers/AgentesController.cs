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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Agentes
        public ActionResult Index()
        {
            //db.Agentes.ToList() -> em SQL: SELECT * FROM Agentes Order by nome;
            //enviar para a view uma lista com todos os agentes, da base de dados, ordenada alfabéticamente


            var listaAgentes = db.Agentes.ToList().OrderBy(a=>a.Nome);

            return View(listaAgentes);
        }

        // GET: Agentes/Details/5
        public ActionResult Details(int? id)
        {
            //se se escrever 'int?' é possivel não forncer o valor para o ID e não há erro

            //proteção para o caso de não ter sido fornecido um ID válido
            if (id == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                // foi ou não foi introduzido um id valido?
                // ou foi introduzido um valor completmnente errado?
                return RedirectToAction("Index");

            }

            //procura na bd, o agente cujo ID foi fornecido
            Agentes agente = db.Agentes.Find(id);
            //proteão para o casod e não ter sido encontrado um agente que tenha o id fornecido
            if (agente == null)
            {
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            //entrega à view os dados do agente encontrado
            return View(agente);
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
            int novoID = 0;
            //*******************************************
            //proteger a geração de um novo ID
            //*******************************************
            //Determinar o nº de agentes na tabela
            if (db.Agentes.Count() == 0)
            {
                novoID = 1;
            }
            else
            {
                novoID = db.Agentes.Max(a => a.ID) + 1;
            }
            //atribuir id ao novo agente
            agente.ID = novoID;
            //*******************************************
            //outra hipotese possivel seria utilizar o 
            //try{}
            //catch (Exception){}

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

                try
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
                catch (Exception)
                {
                    //enviaruma mensagem de erro para o utilizador
                    ModelState.AddModelError("","Ocorreu um erro não determinado na criação de um novo agente...");
                    
                }
               
            }
            //se se chegar aqui, é porque aconteceu algum problema
            //se houver um erro, reapresenta os dados do agente
            return View(agente);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
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
        public ActionResult Edit([Bind(Include = "ID,Nome,Fotografia,Esquadra")] Agentes agente)
        {
            //falta tratar as iamgens



            if (ModelState.IsValid)
            {
                //neste caso já existe um agente apenas quero editar os seus dados
                db.Entry(agente).State = EntityState.Modified;
                //efetuar commit
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agente);
        }

        // GET: Agentes/Delete/5
        //procura os dados de um agente e mostra-os no ecrã
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            return View(agentes);
        }

        // POST: Agentes/Delete/5
        //concretiza, torna definitiva (qd possivel)
        //a remoção de um agente

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
            Agentes agente = db.Agentes.Find(id);

            try
            {
                //delete do agente na bd
                db.Agentes.Remove(agente);
                //commit
                db.SaveChanges();
                //redirecoinar para a pagina inicial
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //gerar uma mensagem de erro, a ser apresentada ao utilizador
                ModelState.AddModelError("",string.Format("Não foi possível remover o Agente'{0}' porque existem {1} multas associadas a ele.",agente.Nome, agente.ListaDeMultas.Count));
                
            }
            // reenvia os dados para a view
            return View(agente);
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
