using System.ComponentModel.DataAnnotations;
using NHibernate;
using NHibernate.Linq;
using VendinhasConsole.Entidades;

namespace VendinhasConsole.Services
{
    public class ClienteService
    {
        private readonly ISessionFactory session;

        public ClienteService(ISessionFactory session)
        {
            this.session = session;
        }

        public List<Cliente> ListarTodos()
        {
            using var sessao = session.OpenSession();
            var clientes = sessao.Query<Cliente>()
                         .OrderBy(c => c.Id)
                         .ToList();
            return clientes;
        }

        public bool Criar(Cliente cliente, out List<ValidationResult> erros)
        {
            if (Validacao(cliente, out erros))
            {
                using var sessao = session.OpenSession();
                using var transaction = sessao.BeginTransaction();
                sessao.Save(cliente);
                transaction.Commit();
                return true;
            }
            return false;
        }

        public bool Editar(Cliente cliente, out List<ValidationResult> erros)
        {
            if (Validacao(cliente, out erros))
            {
                using var sessao = session.OpenSession();
                using var transaction = sessao.BeginTransaction();
                sessao.Merge(cliente);
                transaction.Commit();
                return true;
            }
            return false;
        }

        public Cliente Excluir(int id, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();
            using var sessao = session.OpenSession();
            using var transaction = sessao.BeginTransaction();
            var Cliente = sessao.Query<Cliente>()
                .Where(c => c.Id == id)
                .FirstOrDefault();
            if (Cliente == null)
            {
                erros.Add(new ValidationResult("Cliente não encontrado", new[] { "id" }));
                return null;
            }

            sessao.Delete(Cliente);
            transaction.Commit();
            return Cliente;

        }

       public virtual List<Cliente> Listar(int page, int pageSize)
        {
            using var sessao = session.OpenSession();
            var clientes = page == 0 ? sessao.Query<Cliente>()
                .OrderByDescending(c => c.Id)
                .ToList() :
                sessao.Query<Cliente>()
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize) 
                .ToList();
            return clientes;
        }

        public virtual Cliente Retorna(int id)
        {
            using var sessao = session.OpenSession();
            var cliente = sessao.Get<Cliente>(id);
            return cliente;
        }

        public virtual List<Cliente> Listar(string busca, int page, int pageSize)
        {
            using var sessao = session.OpenSession();
            var Clientes = sessao.Query<Cliente>()
                .Where(c => c.NomeCompleto.Contains(busca) ||
                            c.CPF.Contains(busca))
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return Clientes;
        }

        public List<Divida> ListarDividas(int clienteId)
        {
            using var sessao = session.OpenSession();
            var cliente = sessao.Query<Cliente>()
                                .FetchMany(c => c.Dividas) // FetchMany to eagerly load Dividas collection
                                .Where(c => c.Id == clienteId)
                                .FirstOrDefault();

            if (cliente != null)
              return cliente.Dividas.OrderByDescending(d => d.Valor).ToList();
            else
              return new List<Divida>();
            
        }


        /* ESTÁTICO */

        private static int Contador = 1000;

        private static List<Cliente> Clientes = new List<Cliente>()
        {/*
            new Cliente { NomeCompleto="Ana Maria Gonçalves", CPF = "1234567991", DataNascimento =  new DateTime(2000, 1, 9), Email = "anamariagolçalves@email.com", Id = 1 },
            new Cliente {NomeCompleto="João Paulo Mendes", CPF = "9876543210", DataNascimento =  new DateTime(1997, 3, 7), Email = "joaopaulomendes@email.com", Id = 2}
            */

        };

        public static bool Validacao(Cliente cliente, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();
            var valido = Validator.TryValidateObject(cliente,
                new ValidationContext(cliente),
                erros,
                true
                );
            var diaMinimo = DateTime.Today.AddYears(-18);
            if (cliente.DataNascimento > diaMinimo)
            {
                erros.Add(new ValidationResult(
                    "O cliente deve ser maior de idade",
                    new[] { "DataNascimento" }));
                valido = false;
            }
            if (((cliente.CPF).Length != 11) || (cliente.CPF).Distinct().Count() == 1)
            {
                valido = false;
            }
            else
            {
                int[] multiplicadores1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int soma = 0;

                for (int i = 0; i < 9; i++)
                {
                    soma += int.Parse(cliente.CPF[i].ToString()) * multiplicadores1[i];
                }
                int resto = soma % 11;
                int digito1 = resto < 2 ? 0 : 11 - resto;

                int[] multiplicadores2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                soma = 0;
                for (int i = 0; i < 10; i++)
                {
                    soma += int.Parse(cliente.CPF[i].ToString()) * multiplicadores2[i];
                }

                resto = soma % 11;
                int digito2 = resto < 2 ? 0 : 11 - resto;

                valido = cliente.CPF.EndsWith(digito1.ToString() + digito2.ToString());

            }

            return valido;

        }

        public static bool CriarCliente(Cliente cliente, out List<ValidationResult> erros)
        {
            cliente.Id = cliente.Id++;
            var valido = Validacao(cliente, out erros);
            if (valido)
            {
                Clientes.Add(cliente);
            }
            else
            {
                foreach (var erro in erros)
                {
                    Console.WriteLine("{0} : {1}",
                        erro.MemberNames.First(),
                        erro.ErrorMessage);
                }
            }
            return valido;
        }

        public static bool EditarCliente(Cliente novoCliente, out List<ValidationResult> erros)
        {
            var clienteExistente = Clientes
              .FirstOrDefault(x => x.Id == novoCliente.Id);
            erros = new List<ValidationResult>();

            if (clienteExistente == null)
            {
                return false;
            }
            var valido = Validacao(novoCliente, out erros);
            if (valido)
            {
                clienteExistente.NomeCompleto = novoCliente.NomeCompleto;
                clienteExistente.CPF = novoCliente.CPF;
                clienteExistente.Email = novoCliente.Email;
                clienteExistente.DataNascimento = novoCliente.DataNascimento;
            }
            return valido;
        }
        

        public static List<Cliente> ListarPage(string buscaCliente,
            int skip = 0, int pageSize = 0)
        {
            var consulta = Clientes
                .Where(a =>
                a.Id.ToString() == buscaCliente ||
                a.NomeCompleto.Contains(buscaCliente,
                                 StringComparison.OrdinalIgnoreCase) ||
                a.Email.Contains(buscaCliente))
                .OrderBy(x => x.DataNascimento)
                .AsEnumerable();
            if (skip > 0)
            {
                consulta = consulta.Skip(skip);
            }
            if (pageSize > 0)
            {
                consulta = consulta.Take(pageSize);
            }
            return consulta.ToList();
        }
        public static Cliente Remover (int codigo)
        {
            var cliente = Clientes
                          .Where(x => x.Id == codigo)
                          .FirstOrDefault();
            Clientes.Remove(cliente);
            return cliente;
        }
    }
}

        //public void AdicionarDivida(int clienteId, Divida divida)
        //{
          //  using (var sessao = session.OpenSession())
          //  using (var transaction = sessao.BeginTransaction())
            //{
              //  var cliente = sessao.Get<Cliente>(clienteId);
                //if (cliente == null)
                //{
                  //  throw new ArgumentException("Cliente não encontrado", nameof(clienteId));
                //}

                //divida.ClienteId = clienteId; // Associando o Id do cliente à divida
                //sessao.Save(divida); // Salvando a divida
                //transaction.Commit();
            //}
        //}
