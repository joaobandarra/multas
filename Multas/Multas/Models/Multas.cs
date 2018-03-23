using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Multas
    {
        [Key]
        public int ID { get; set; }

        public string Infracao { get; set; }
        public string LocalMulta { get; set; }
        public decimal ValorMulta { get; set; }
        public DateTime DataDaMulta { get; set; }

        //****************************************************
        //construção das chaves forasteiras
        //****************************************************

        //FK Agentes
        //ForeignKey NomeAtributoQueÉFK references TABELA(pkDaTabela)
        [ForeignKey ("Agente")]
        public int AgenteFK { get; set; }
        public virtual Agentes Agente { get; set; }



        //FK Viaturas
        //ForeignKey NomeAtributoQueÉFK references TABELA(pkDaTabela)
        [ForeignKey("Viatura")]
        public int ViaturaFK { get; set; }
        public virtual Viaturas Viatura { get; set; }



        //FK Condutores
        //ForeignKey NomeAtributoQueÉFK references TABELA(pkDaTabela)
        [ForeignKey("Condutor")]
        public int CondutorFK { get; set; }
        public virtual Condutores Condutor { get; set; }




        
    }
}