import { Link } from "simple-react-routing";
import Logo from "../assets/pecunia_logo.png"
function Navbar() {
    return (<nav>
        <ul id="divTexto">
            <img src={Logo} className="logo" />
            <li className="textoAba"><Link to="/">Home</Link></li>
            <li className="textoAba"><Link to="/cliente">Clientes</Link></li>
            <li className="textoAba"><Link to="/cliente/criar">Novo cliente</Link></li>
            <li className="textoAba"><Link to="/dividas">Dívidas Pendentes</Link></li>
            <li className="textoAba"><Link to="/dividas/criar">Adicionar Dívida</Link></li>
        </ul>
    </nav>)
};

export default Navbar;