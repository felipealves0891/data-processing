--º exclui o banco caso exista
use [master];
drop database if exists [dbDataProcessing];
go

--º cria o banco de dados
create database [dbDataProcessing];
go

use [dbDataProcessing];
go

--º cria uma tabela de teste 
create table [dbDataProcessing].[dbo].[TblTeste] (
    [Id]          int identity,
    [Created]     datetime not null default getdate(),
    [Value]       varchar(max) not null,
    primary key([Id])
);
go

--º adiciona dados de teste 
insert into [dbDataProcessing].[dbo].[TblTeste] ([Value]) values ('Fulano');
insert into [dbDataProcessing].[dbo].[TblTeste] ([Value]) values ('Siclano');
insert into [dbDataProcessing].[dbo].[TblTeste] ([Value]) values ('Joaquim');