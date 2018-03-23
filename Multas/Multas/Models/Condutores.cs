using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Condutores
    {
        public Condutores()
        {
            ListaDeMultas = new HashSet<Multas>();
        }


        [Key]
        public int ID { get; set; }//chave primária

        //dados proprios do condutor
        public string Nome { get; set; }
        public string BI { get; set; }
        public string Telemovel{ get; set; }
        public DateTime DataNascimento { get; set; }

        //dados da carta de condução do condutor
        public string NumCartaConducao { get; set; }
        public string LocalEmissao { get; set; }
        public DateTime DataValidadeCarta { get; set; }



        //complementar a informação sobre o relacionamento dum condutor com as multas por ele feitas
        public virtual ICollection<Multas> ListaDeMultas { get; set; }

    }
}