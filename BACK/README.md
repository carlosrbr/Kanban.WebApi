## Tecnologias Utilizadas  

- **.NET 8.0 (ASP.NET Core)**  
- **AutoMapper** para mapeamento de objetos  
- **FluentValidation** para validação de modelos  
- **Docker** para containerização  
- **InMemory Database**  

## Estrutura do Projeto  

O projeto segue as boas práticas de **DDD (Domain-Driven Design)**, organizado nas seguintes camadas:  

1. **Presentation**  
   - Contém os **controllers** e **DTOs (Data Transfer Objects)** para a interação com a API.  

2. **Application**  
   - Implementa a lógica de negócios e os serviços.  

3. **Domain**  
   - Define as entidades e interfaces dos repositórios.  

4. **Infrastructure**  
   - Contém as implementações de repositórios e o acesso a dados.  

5. **WebApi**  
   - Camada de apresentação com **ASP.NET Core**.  

## Como Rodar a Aplicação  

### Requisitos  

- **.NET SDK 8.0**  
- **Docker** (se optar por rodar usando containers)  

### Passos para Executar Localmente  

1. Clone o repositório:  

    ```bash  
    git clone https://github.com/carlosrbr/Kanban.WebApi  
    cd Kanban.WebApi/BACK/src/Kanban.WebApi  
    ```  

2. Crie um arquivo `.env` e configure as credenciais necessárias.  

3. **Inicie o back-end**:  

    ```bash  
    dotnet run  
    ```  

    A aplicação estará disponível em: [http://localhost:5000](http://localhost:5000).  

4. **Inicie o front-end**:  

    ```bash  
    cd Kanban.WebApi  
    docker-compose up --build  
    ```  

    A aplicação estará disponível em: [http://localhost:3000](http://localhost:3000).  

## Melhorias e Pendências  
1. Corrigir o `docker-compose` do backend: o pacote `Microsoft.AspNetCore.Authentication.JwtBearer` causa erro em runtime devido à falta da biblioteca `System.IdentityModel.Tokens.Jwt`.  
2. Implementar testes de integração.  
3. Mover as credencias do .env para o appsetting.json

### Observações  

Durante a configuração do front-end com **yarn**, foi necessário incluir no `Dockerfile` para evitar problemas:  

```dockerfile  
ENV NODE_OPTIONS=--openssl-legacy-provider  
