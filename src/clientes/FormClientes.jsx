import { useNavigation, useRouter } from "simple-react-routing"
import { postCliente, getById } from "../services/clienteApi";
import { useEffect, useReducer, useState } from "react";


export default function FormClientes() {
    const { pathParams } = useRouter();

    const id = pathParams["id"];

    const { navigateTo } = useNavigation();

    const [errorMessage, setErrorMessage] = useState("");

    const [email, setEmail] = useReducer((old, value) => {
        return value.toLowerCase().replaceAll(" ", "");
    }, "");
    
    const [cpf, setCpf] = useReducer((old, value) => {
        var regex = /[0-9]+/g;
        var digitos = value.replace(/[^0-9]+/g, "").substring(0, 11);

        if (digitos.length <= 3) return digitos;
        else if (digitos.length <= 6) {
            return digitos.replace(/(\d{3})(\d+)/, "$1.$2");
        }
        else if (digitos.length <= 9) {
            return digitos.replace(/(\d{3})(\d{3})(\d+)/, "$1.$2.$3");
        }
        else {
            return digitos.replace(/(\d{3})(\d{3})(\d{3})(\d+)/, "$1.$2.$3-$4");
        }

    }, "");

    const salvarCliente = async (evento) => {
        evento.preventDefault();
        var dados = new FormData(evento.target);
        var cliente = {
            id: id,
            nomeCompleto: dados.get("nome"),
            email: dados.get("email"),
            dataNascimento: dados.get("dataNascimento"),
            cpf: cpf.replace(/[^\d]/g, "")
        };
        var result = await postCliente(cliente);
        if (result.status == 200) {
            navigateTo(null, "/clientes")
        }
        else {
            var erro = await result.json();
            setErrorMessage("Erro ao criar cliente: \n" + JSON.stringify(erro, null, '\t'))
        }
    }

    const [cliente, setCliente] = useState();

    useEffect(() => {
        if (id) {
            getById(id)
                .then(e => e.json())
                .then(result => setCliente(result));
        }
        else {
            setCliente({ inscricoes: [] })
        }
    }, []);

    return cliente && (<>
        <h1>{id ? "Editar" : "Cadastrar"} cliente</h1>
        <form onSubmit={salvarCliente}>
            <div className="formulario">
                <div className="row">
                    <div className="input">
                        <label>Nome:</label>
                        <input defaultValue={cliente.nomeCompleto} placeholder="Nome Completo" type="text" name="nome" />
                        <span className="error"></span>
                    </div>
                </div>
                <div className="row">
                    <div className="input">
                        <label>Data nascimento:</label>
                        <input defaultValue={cliente.dataNascimento?.substring(0, 10)} type="date" name="dataNascimento" />
                        <span className="error"></span>
                    </div>
                </div>
                <div className="row">
                    <div className="input">
                        <label>Email:</label>
                        <input value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            placeholder="E-mail do cliente" type="email" name="email" />
                        <span className="error"></span>
                    </div>
                </div>
                <div className="row">
                    <div className="input">
                        <label>CPF:</label>
                        <input value={cpf}
                            onChange={(e) => setCpf(e.target.value)}
                            placeholder="CPF do cliente" type="text" name="cpf" />
                        <span className="error"></span>
                    </div>
                </div>
            </div>
            <div className="acoes">
            <button type="submit" >Salvar</button></div>
            <p className="error">{errorMessage}</p>
        </form>
    </>
    )
}