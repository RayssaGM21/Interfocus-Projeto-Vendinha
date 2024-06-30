import React, { useEffect, useState } from "react";
import { getById, listarDividas } from "../services/dividaApi";
import CabecalhoDivida from "../assets/CabecalhoDivida.png"

export default function ListaDividas() {
    const [dividas, setDividas] = useState([]);
    const [busca, setBusca] = useState("");
    const [page, setPage] = useState(1);

    useEffect(() => {
        listarDividas(busca)
            .then(resposta => {
                if (resposta.status === 200) {
                    resposta.json()
                        .then(dividas => {
                            // Filtrar as dívidas onde situacao é false
                            const dividasFiltradas = dividas.filter(divida => !divida.situacao);
                            setDividas(dividasFiltradas);
                        });
                }
            })
            .catch(error => console.error('Erro ao buscar dívidas:', error));
    }, [busca, page]);

    const getDivida = async (id) => {
        try {
            const result = await getById(id);
            if (result.status === 200) {
                const dados = await result.json();
                // Setar a divida obtida pelo getById, se necessário
            } else {
                console.error('Erro ao obter dívida:', result.statusText);
            }
        } catch (error) {
            console.error('Erro ao obter dívida:', error);
        }
    }

    return (
        <>
            <div className="cabecalho"><img className="cabecalhoImg" src={CabecalhoDivida} alt="" /></div>
            <div className="pesquisaDivida">
                <input
                    className="pesq"
                    placeholder="Pesquise pela descrição..."
                    type="search"
                    value={busca}
                    onChange={(e) => setBusca(e.target.value)}
                />
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
                    {dividas.map(divida => (
                        <tr onClick={() => getDivida(divida.id)} key={divida.id}>
                            <td>{divida.cliente.nomeCompleto}</td>
                            <td>R$ {divida.valor}</td>
                            <td>{divida.descricao}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <div className="total-card">
                <p>Total das dívidas: R$ {dividas.reduce((total, divida) => total + divida.valor, 0)}</p>
            </div>
            <div className="pag">
                <button type="button" onClick={() => setPage(page - 1)}>Anterior</button>
                <span>{page}</span>
                <button type="button" onClick={() => setPage(page + 1)}>Próximo</button>
            </div>
        </>
    );
}
