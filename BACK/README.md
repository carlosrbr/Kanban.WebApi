## Tecnologias Utilizadas  

- **.NET 8.0 (ASP.NET Core)**  
- **AutoMapper** para mapeamento de objetos  
- **FluentValidation** para valida��o de modelos  
- **Docker** para containeriza��o  
- **InMemory Database**  

## Estrutura do Projeto  

O projeto segue as boas pr�ticas de **DDD (Domain-Driven Design)**, organizado nas seguintes camadas:  

1. **Presentation**  
   - Cont�m os **controllers** e **DTOs (Data Transfer Objects)** para a intera��o com a API.  

2. **Application**  
   - Implementa a l�gica de neg�cios e os servi�os.  

3. **Domain**  
   - Define as entidades e interfaces dos reposit�rios.  

4. **Infrastructure**  
   - Cont�m as implementa��es de reposit�rios e o acesso a dados.  

5. **WebApi**  
   - Camada de apresenta��o com **ASP.NET Core**.  

## Como Rodar a Aplica��o  

### Requisitos  

- **.NET SDK 8.0**  
- **Docker** (se optar por rodar usando containers)  

### Passos para Executar Localmente  

1. Clone o reposit�rio:  

    ```bash  
    git clone https://github.com/carlosrbr/Kanban.WebApi  
    cd Kanban.WebApi/BACK/src/Kanban.WebApi  
    ```  

2. Crie um arquivo `.env` e configure as credenciais necess�rias.  

3. **Inicie o back-end**:  

    ```bash  
    dotnet run  
    ```  

    A aplica��o estar� dispon�vel em: [http://localhost:5000](http://localhost:5000).  

4. **Inicie o front-end**:  

    ```bash  
    cd Kanban.WebApi  
    docker-compose up --build  
    ```  

    A aplica��o estar� dispon�vel em: [http://localhost:3000](http://localhost:3000).  

## Melhorias e Pend�ncias  
1. Corrigir o `docker-compose` do backend: o pacote `Microsoft.AspNetCore.Authentication.JwtBearer` causa erro em runtime devido � falta da biblioteca `System.IdentityModel.Tokens.Jwt`.  
2. Implementar testes de integra��o.  
3. Mover as credencias do .env para o appsetting.json

### Observa��es  

Durante a configura��o do front-end com **yarn**, foi necess�rio incluir no `Dockerfile` para evitar problemas:  

```dockerfile  
ENV NODE_OPTIONS=--openssl-legacy-provider  
