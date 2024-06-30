const URL_API = "https://localhost:7236";

export function listarDividas(pesquisa) {
    // PROMISE
    var response = pesquisa ?
        fetch(URL_API + "/api/divida?busca=" + pesquisa) :
        fetch(URL_API + "/api/divida");

    return response;
}

export function getById(id) {
    // PROMISE
    var response = fetch(URL_API + "/api/divida/" + id);
    return response;
}

export function deletarDivida(id) {
    // PROMISE
    var request = {
        method: "DELETE"
    }
    var response = fetch(URL_API + "/api/divida/" + id, request)
    return response;
}

export function postDivida(divida) {
    // PROMISE
    var request = {
        method: divida.id ? "PUT" : "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(divida)
    }
    var response =
        fetch(URL_API + "/api/divida",
            request)
    return response;
}