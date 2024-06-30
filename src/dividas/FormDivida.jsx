import React, { useEffect, useState } from 'react';
import { postDivida } from '../services/dividaApi';

const URL_API = "https://localhost:7236";

export default function FormDivida({ divida, onClose }) {
  const [listaClientes, setListaClientes] = useState([]);
  const [errorMessage, setErrorMessage] = useState('');
  const [situacaoDivida, setSituacaoDivida] = useState(false);
  const [todayDate, setTodayDate] = useState(getFormattedToday());

  useEffect(() => {
    fetch(URL_API + '/api/cliente')
      .then((response) => response.json())
      .then((data) => setListaClientes(data))
      .catch((error) => console.error('Erro ao buscar clientes:', error));
  }, []);

  function getFormattedToday() {
    const today = new Date();
    const year = today.getFullYear();
    let month = today.getMonth() + 1;
    let day = today.getDate();
    if (month < 10) {
      month = '0' + month;
    }
    if (day < 10) {
      day = '0' + day;
    }
    return `${year}-${month}-${day}`;
  }

  const toggleSituacaoDivida = () => {
    setSituacaoDivida(!situacaoDivida);
  };

  const salvarDivida = async (evento) => {
    evento.preventDefault();
    var dados = new FormData(evento.target);
    var dividaDados = {
      valor: parseFloat(dados.get('valor')), 
      descricao: dados.get('descricao'),
      situacao: situacaoDivida,
      dataCriacao: todayDate,
      dataPagamento: dados.get('dataPagamento'),
      id: divida.idDivida
    };

    try {
      var result = await postDivida(dividaDados);
      if (result.status === 200) {
        onClose();
      } else {
        var error = await result.json();
        setErrorMessage('Houve erro ao salvar dívida: \n' + JSON.stringify(error, null, '\t'));
      }
    } catch (error) {
      console.error('Erro ao salvar dívida:', error);
      setErrorMessage('Houve um erro ao salvar a dívida. Por favor, tente novamente.');
    }
  };

  return (
    <div>
      <form onSubmit={salvarDivida}>
        <div className="formulario">
                <div className="input">
                    <label>Valor da Dívida:</label>
                    <input placeholder="Valor da dívida" min="1" max="200" type="number" name="valor" step=".01"/>
                    <span className="error"></span>
                </div>
                <div className="input">
                <label>Data de Criação:</label>
                <input
                    type="date"
                    name="dataCriacao"
                    value={todayDate}
                    onChange={(e) => setTodayDate(e.target.value)}
                />
                </div>
                <div className="input">
                    <label>Data de Pagamento:</label>
                    <input type="date" name="dataPagamento" />
                    <span className="error"></span>
                </div>
    
                <div className="input">
                    <label>Descrição da Dívida:</label>
                    <textarea placeholder="Descrição da dívida" name="descricao"></textarea>
                    <span className="error"></span>
                </div>
                <div className="input">
                    <label>Está pago?</label>
                    <div className="switch">
                        <label className="switch-label">
                            <input type="checkbox" onChange={toggleSituacaoDivida} checked={situacaoDivida} />
                        <span className="slider round"></span>
                        </label>
                    </div>
                </div>
                <div className="input">
                        <label htmlFor="selectCliente">Selecione um cliente:</label>
                        <select id="selectCliente" name="cliente">
                            {listaClientes.map((cliente) => (
                            <option key={cliente.id} value={cliente.id}>
                                {cliente.nomeCompleto}
                            </option>
                            ))}
                        </select>
                </div>
                
            
          <p className="error">{errorMessage}</p>
        </div>

        

        <div className="acoes">
          <button type="submit">Adicionar Dívida</button>
        </div>
      </form>
    </div>
  );
}
