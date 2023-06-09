# Lead Management API

## 📖 Introdução

API construída com asp.net para gerenciamento de Leads.

## ⚙️ Instalação de dependências

Para rodar o projeto localmente, é necessário ter instalado na máquina o .NET 6.

Para installar os pacotes necessários, execute o comando:

```powershell
dotnet restore
```

## 🚶 Migrations

Quase lá! O último passo antes de iniciar o servidor é executar as migrações. Para isso, é preciso ajustar o valor do campo `LeadDbConnection` no arquivo [appsettings.json](./LeadManagement/appsettings.json) com a string de conexão com o banco de dados (SQL Server).

Após, use o comando:

```powershell
dotnet-ef database update --project .\LeadManagement\
```

Caso não tenha a feramenta `dotnet-ef`, execute o comando `dotnet tool install --global dotnet-ef` para instalar globalmente e depois execute novamente o comando para aplicar as migrações.

## ⏯️ Iniciar o servidor

Pronto! Agora é só executar os comandos para fazer o build e rodar a aplicação: 

```powershell
dotnet build
dotnet run --project .\LeadManagement\
```

A API estará disponível em https://localhost:7151/api/ e o swagger em https://localhost:7151/swagger/.

## 🔀 Rotas

* (POST) **/lead**
    * Criar Lead
* (POST) **/lead/{id}/accept**
    * Aceitar Lead
* (POST) **/lead/{id}/decline**
    * Recusar Lead
* (GET) **/lead**
    * Listar todas as Leads
* (GET) **/lead/accepted**
    * Listar todas as Leads aceitas
* (GET) **/lead/{id}**
    * Buscar Leads
* (PUT) **/lead/{id}**
    * Editar Lead
* (DELETE) **/lead/{id}**
    * Excluír Lead

Para detalhes sobre corpo e parâmetro das requisições, consulte o swagger.

## ⚗️ Testes

Para rodar os testes automatizados, execute o comando:

```powershell
dotnet test
```
