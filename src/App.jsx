import React from 'react'
import './App.css'
import Layout from './layout/Layout.jsx';
import { BrowserRouter, registerPathTypeParameter } from 'simple-react-routing';
import Home from './Home.jsx';
import ListaClientes from './clientes/ListaClientes.jsx';
import FormClientes from './clientes/FormClientes.jsx';
import ListaDividas from './dividas/ListaDividas.jsx';
import FormDivida from './dividas/FormDivida.jsx';
import DividaCliente from './dividas/DividaCliente.jsx'

registerPathTypeParameter("numero", /[0-9]+/);

function App() {
 
  return (
    <BrowserRouter
      notFoundPage={<h1>404 - NOT FOUND</h1>}
      routes={[
        {
          path: "",
          component: <Home></Home>
        },
        {
          path: "cliente",
          component: <ListaClientes></ListaClientes>
        },
        {
          path: "cliente/criar",
          component: <FormClientes></FormClientes>
        },
        {
          path: "cliente/editar/:id(numero)",
          component: <FormClientes></FormClientes>
        },
        {
          path: "dividas",
          component: <ListaDividas></ListaDividas>
        },
        {
          path: "dividas/criar",
          component: <FormDivida></FormDivida>
        },
        {
          path:"dividas/:id(numero)",
          component: <DividaCliente></DividaCliente>
        }
      ]}>
    
      <Layout></Layout>
    </BrowserRouter>
  )

}

export default App