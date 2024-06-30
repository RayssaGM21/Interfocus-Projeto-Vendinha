const URL_API = "https://localhost:7236";

export function listarClientes(pesquisa, page, pageSize) {
    var data = page == 0 ? "" : `page=${page}&pageSize=${pageSize}`;

    var response = pesquisa ?
        fetch(`${URL_API}/api/cliente?pesquisa=${pesquisa}&${data}`) :
        fetch(`${URL_API}/api/cliente?${data}`);

    return response;
}

export function getById(id) {
    var response = fetch(`${URL_API}/api/cliente/${id}`);
    return response;
}

export function deletarCliente(id) {
    var request = {
        method: "DELETE"
    };

    var response = fetch(`${URL_API}/api/cliente/${id}`, request);
    return response;
}

export function postCliente(cliente) {
    var url = cliente.id ? `${URL_API}/api/cliente/${cliente.id}` : `${URL_API}/api/cliente`;

    var request = {
        method: cliente.id ? "PUT" : "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(cliente)
    };

    var response = fetch(url, request);
    return response;
}
