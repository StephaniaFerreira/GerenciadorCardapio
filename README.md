# 📌 Gerenciador de Cardápio — API .NET + MongoDB

API REST desenvolvida em **.NET 6** com **MongoDB**, cujo principal objetivo é demonstrar **modelagem de dados orientada a consulta** utilizando banco NoSQL. 

Para que a consulta por intervalo de tempo e por receita ao cardápio tenha uma boa performance adicionei dois indexes idx_cardapio_periodo e idx_cardapio_nome_receita.

O sistema permite:

* Cadastro de receitas
* Gerenciamento de cardápios por usuário
* Organização por períodos (semana, mês ou trimestre)
* Consultas por intervalo de datas
* Associação flexível entre receitas e cardápios

Este projeto foi construído com foco em aprendizado de:

✔ Modelagem NoSQL
✔ Arquitetura de APIs
✔ Organização de serviços
✔ Separação de responsabilidades

---

## 🛠 Stack utilizada

* **.NET** → versão `6.0.428`
* **MongoDB.Driver** → versão `2.7.0`
* MongoDB (banco NoSQL)

Arquitetura organizada em:

```
Controllers → endpoints da API
Models → modelos de dados
Services → regras de negócio e acesso ao banco
```

---

## 📂 Estrutura do projeto

```
GerenciadorCardapioSolution/
├── GerenciadorCardapio/                  # Projeto principal
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Data/
│   ├── Configurations/
│   └── GerenciadorCardapio.csproj
│
├── GerenciadorCardapioUnitTests/         # Testes unitários
│   └── GerenciadorCardapioUnitTests.csproj
│
├── GerenciadorCardapioIntegrationTests/  # Testes de integração
│   └── GerenciadorCardapioIntegrationTests.csproj
│
└── GerenciadorCardapioSolution.sln       # Solution que agrega todos
```

---

## ▶ Como executar localmente

### ✅ Pré-requisitos

Instale:

* .NET SDK 6
* MongoDB rodando localmente

---

### 1️⃣ Clone o projeto

```
git clone <url-do-repositorio>
cd GerenciadorCardapio
```

---

### 

---

### 2️⃣ Inicie o MongoDB

Linux:

```
sudo systemctl start mongod
```

ou:

```
mongod
```

---

### 3️⃣ Execute a API e Testes Unitários e de Integração

```
dotnet restore
dotnet run
dotnet test
```

A API estará disponível em:

```
https://localhost:xxxx/swagger
```
