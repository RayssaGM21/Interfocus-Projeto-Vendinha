using System.ComponentModel.DataAnnotations;
using NHibernate;
using VendinhasConsole.Entidades; 

namespace VendinhasConsole.Services
{
    public class DividaService
    {
        private readonly ISessionFactory sessionFactory;
              
        public DividaService(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public static bool Validacao(Divida divida, int clienteId, ClienteService clienteService, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();
            var valido = Validator.TryValidateObject(divida,
                new ValidationContext(divida),
                erros,
                true);

            var todasDividasCliente = clienteService.ListarDividas(clienteId);
            decimal totalDividasCliente = todasDividasCliente.Sum(d => d.Valor);

            if ((totalDividasCliente + divida.Valor) > 200)
            {
                erros.Add(new ValidationResult("O valor total das dívidas do cliente não pode ultrapassar R$ 200", new[] { "Valor" }));
                valido = false;
            }

            return valido;
        }

        public bool Criar(Divida divida, int clienteId, ClienteService clienteService, out List<ValidationResult> erros)
        {
            if (Validacao(divida, clienteId, clienteService, out erros))
            {
                using var sessao = sessionFactory.OpenSession();
                using var transaction = sessao.BeginTransaction();
                sessao.Merge(divida);
                transaction.Commit();
                return true;
            }
            return false;
        }

        public bool Editar(Divida divida, int clienteId, ClienteService clienteService, out List<ValidationResult> erros)
        {
            if (Validacao(divida, clienteId, clienteService, out erros))
            {
                using var sessao = sessionFactory.OpenSession();
                using var transaction = sessao.BeginTransaction();
                sessao.Merge(divida);
                transaction.Commit();
                return true;
            }
            return false;
        }
        /*
        public bool Excluir (int idDivida, out List<ValidationResult > erros)
        {
            erros = new List<ValidationResult>();
            using var sessao = sessionFactory.OpenSession();
            using var transaction = sessao.BeginTransaction();
            var divida = sessao.Query<Divida>();
            if (divida == null)
            {
                erros.Add(new ValidationResult("Registro não encontrado", new[] { "idDivida" }));
                return false;
            }
            sessao.Delete(divida);
            transaction.Commit();
            return true;
         
        }*/
        /*
        public List<Divida> Listar()
        {
            using var sessao = sessionFactory.OpenSession();
            var dividas = sessao.Query<Divida>()
                    .OrderByDescending(c => c.Valor)
                    .ToList();
                return dividas;
        }*/

        public virtual Divida Retorna(int idDivida)
        {
            using var sessao = sessionFactory.OpenSession();
            var divida = sessao.Get<Divida>(idDivida);
            return divida;
        }
        /*
        public virtual List<Divida> Listar(string busca)
        {
            using var sessao = sessionFactory.OpenSession();
            var divida = sessao.Query<Divida>()
                .Where(c => c.Descricao.Contains(busca))
                .OrderBy(c => c.IdDivida)
                .Take(10)
                .ToList();
            return divida;

        }*/

        public bool Excluir(int idDivida, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();
            using var sessao = sessionFactory.OpenSession();
            using var transaction = sessao.BeginTransaction();
            var divida = sessao.Query<Divida>()
                               .FirstOrDefault(d => d.IdDivida == idDivida);
            if (divida == null)
             {
                erros.Add(new ValidationResult("Registro não encontrado", new[] { "idDivida" }));
                return false;
             }

             sessao.Delete(divida);
             transaction.Commit();
             return true;
        }

        public List<Divida> Listar()
        {
            using var sessao = sessionFactory.OpenSession();
            return sessao.Query<Divida>()
                         .OrderByDescending(d => d.Valor)
                         .ToList();
        }

        public List<Divida> Listar(string busca)
        {
            using var sessao = sessionFactory.OpenSession();
            return sessao.Query<Divida>()
                             .Where(d => d.Descricao.Contains(busca))
                             .OrderBy(d => d.IdDivida)
                             .ToList();
            
        }
    }
}
