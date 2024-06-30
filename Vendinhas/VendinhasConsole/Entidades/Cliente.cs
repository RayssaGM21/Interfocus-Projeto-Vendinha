using System.ComponentModel.DataAnnotations;


namespace VendinhasConsole.Entidades
{
    public class Cliente
    {
        public Cliente()
        {
            Dividas = new List<Divida>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório")]
        [StringLength(50, MinimumLength = 8)]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11)]
        public string CPF { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        public string Email { get; set;}

        public virtual IList<Divida> Dividas { get; set; } // Adicionando virtual para suportar lazy loading

        public void PrintDados()
        {
            Console.WriteLine(
                "{0} {1} {2} {3}",
                Id, NomeCompleto, CPF, DataNascimento.ToShortDateString()
            );
        }
    }
}


