import { useEffect, useState } from "react";
import { listarClientes } from "../services/clienteApi";
import { deletarCliente } from "../services/clienteApi";
import { Link } from "simple-react-routing";
import CabecalhoCliente from "../assets/CabecalhoCliente.png"

export default function ListaClientes(properties) {
    const [lista, setLista] = useState([]);
    const [busca, setBusca] = useState("");
    const [page, setPage] = useState(1);

    useEffect(() => {
        listarClientes(busca, page, 8)
            .then(resposta => {
                if (resposta.status == 200) {
                    resposta.json()
                        .then(clientes => {
                            setLista(clientes);
                        })
                }
            });
    }, [busca, page]);

    return (<>
        <div className="cabecalho"><img className="cabecalhoImg" src={CabecalhoCliente} alt="" /></div>
        
        <div className="pesquisaCliente">
            <input className="pesq"
                placeholder="Pesquise pelo nome..."
                type="search"
                style={{ minWidth: 250 }}
                value={busca}
                onChange={(event) => {
                    setBusca(event.target.value);
                }}
            />
        </div>

        <div className="grid" id="grid-clientes">
            {lista.map(
                cliente => {
                    return <ClienteItem key={cliente.id}
                        cliente={cliente}></ClienteItem>
                }
            )}
        </div>
        <div className="pag">
            <button type="button" onClick={() => setPage(page - 1)}>Anterior</button>
            <span>{page}</span>
            <button type="button" onClick={() => setPage(page + 1)}>Próximo</button>
        </div>
    </>)
}

  
export function ClienteItem(p) {
    const cliente = p.cliente;
    var data = new Date(cliente.dataNascimento);
    var dia = data.getDate().toString().padStart(2, '0');
    var mes = (data.getMonth() + 1).toString().padStart(2, '0');
    var ano = data.getFullYear();
    var anoAtual = new Date().getFullYear();
    var idade = anoAtual - ano

    

    return (<div className="card">
        <ul>
            <li>Nome: {cliente.nomeCompleto}</li>
            <li>CPF: {cliente.cpf}</li>
            <li>Idade: {idade} anos</li>
            <li>Data de Nascimento: {dia}/{mes}/{ano}</li>
            <li>E-mail: {cliente.email}</li>
        </ul>
        <div className="acoes">
            <div><Link to={`/dividas/${cliente.id}`}>Dívidas</Link></div>
            <Link to={"/cliente/editar/" + cliente.id}>Editar</Link>
            <button type="button" data-id-cliente={cliente.id} onClick={() => deletarCliente(cliente.id)}>Excluir</button>
            
        </div>
    </div>)
}