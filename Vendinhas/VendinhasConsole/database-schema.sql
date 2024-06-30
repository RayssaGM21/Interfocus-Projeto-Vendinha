create sequence clientes_seq;
create table clientes (
	id integer not null default nextval('clientes_seq'),
	nomeCompleto varchar(50) not null,
	cpf varchar(11) unique not null,
	dataNascimento date not null,
	email varchar(100)
)

create sequence dividas_seq;
create table dividas(
	idDividas integer not null default nextval('dividas_seq'),
	idCliente integer references clientes(id),
	valor decimal not null,
	situacao boolean not null,
	dataCriacao timestamp default now() not null,
	dataPagamento date not null,
	descricao varchar(200) not null,
	primary key (idDividas)
)


