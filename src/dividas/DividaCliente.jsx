import { useEffect, useState } from "react";
import { getById, listarDividas } from "../services/dividaApi";



export default function ListaDividas() {
    
    var [dividas, setDividas] = useState([]);
    var [divida, setDivida] = useState();
    const [busca, setBusca] = useState("");
    const [page, setPage] = useState(1);

    useEffect(() => {
        listarDividas(busca)
            .then(resposta => {
                if (resposta.status == 200) {
                    resposta.json()
                        .then(dividas => {
                            setDividas(dividas);
                        })
                }
            });
    }, [busca, page]);

    const getDivida = async (id) => {
        var result = await getById(id);
        if (result.status == 200) {
            var dados = await result.json();
            setDivida(dados);
        }
    }
    
    return (<>
        <h1>Dividas</h1>
        <div className="row">
            <input type="search" value={busca}
                onChange={(e) => setBusca(e.target.value)} />
        </div>

        <table id="table-dividas">
            <thead>
                <tr>
                    <th>Nome</th>
                    <th>Valor</th>
                    <th>Descrição</th>
                </tr>
            </thead>
            <tbody>
                {dividas.map(divida =>
                    <tr onClick={() => getDivida(divida.id)} key={divida.id}>
                        <td>{divida.cliente.nomeCompleto}</td>
                        <td>R$ {divida.valor}</td>
                        <td>{divida.descricao}</td>
                    </tr>
                )}
            </tbody>
        </table>
        <div className="total-card"><p>Total das dívidas: R$ {dividas.reduce((total, divida) => total + divida.valor, 0)}</p></div>
        <div className="row">
            <button type="button" onClick={() => setPage(page - 1)}>Anterior</button>
            <span>{page}</span>
            <button type="button" onClick={() => setPage(page + 1)}>Próximo</button>
        </div>
    </>)
}