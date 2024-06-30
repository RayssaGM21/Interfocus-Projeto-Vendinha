using System.ComponentModel.DataAnnotations;

namespace VendinhasConsole.Entidades
{
    public class Divida
    {
        public virtual int IdDivida { get; set; }

        [Required(ErrorMessage = "O valor da dívida é obrigatório")]
        public virtual decimal Valor { get; set; }

        public virtual bool Situacao { get; set; }

        [Required(ErrorMessage = "A data de criação da dívida é obrigatória")]
        public virtual DateTime DataCriacao { get; set; }

        public virtual DateTime? DataPagamento { get; set; }

        [StringLength(200, ErrorMessage = "A descrição não pode ter mais de 200 caracteres")]
        
        public virtual required string Descricao { get; set; }

        public virtual int IdCliente { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual void PrintDados()
        {
            Console.WriteLine(
                "{0} {1} {2} {3} {4} {5}",
                IdDivida, Valor, Situacao, DataCriacao.ToShortDateString(),
                DataPagamento.HasValue ? DataPagamento.Value.ToShortDateString() : "Não pago",
                Descricao
            );
        }
    }
}
