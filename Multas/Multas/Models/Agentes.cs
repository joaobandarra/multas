using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes
    {
        public Agentes() {
            ListaDeMultas = new HashSet<Multas>();
        }

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório!")]
        [RegularExpression ("[A-ZÍÉÂÁ][a-záéíóúàèìòùâêîôûäëïöüãõç]+(( |'|-| dos | da | de | d'| e )[A-ZÍÉÂÁ][a-záéíóúàèìòùâêîôûäëïöüãõç]+){1,3}", ErrorMessage ="O {0} apenas pode conter letras e espaços em branco. cada palavra começa em maiúscula seguida de minúsculas")]
        public string Nome { get; set; }

        
        public  string Fotografia { get; set; }

        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório!")]
        [RegularExpression ("[A-ZÍÉÂÁ]*[a-záéíóúàèìòùâêîôûäëïöüãõç -]*", ErrorMessage ="Não aceita numeros")]
        public string Esquadra { get; set; }


        //complementar a informação sobre o relacionamento dum agente
        //com as multas por ele passadas
        public virtual ICollection<Multas> ListaDeMultas { get; set; }

    }
}