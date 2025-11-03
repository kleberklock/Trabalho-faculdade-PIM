// Nome do arquivo: Program.cs

using SeuProjetoApi.Models; // Importa nossas classes
using Microsoft.AspNetCore.Mvc; // Para [FromBody]

var builder = WebApplication.CreateBuilder(args);

// Habilita o CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()  // Permite qualquer origem (seu arquivo .html)
                  .AllowAnyHeader()  // Permite qualquer cabeçalho
                  .AllowAnyMethod(); // Permite qualquer método (GET, POST, etc.)
        });
});

var app = builder.Build();

// Removi o app.UseHttpsRedirection(); para evitar o warning amarelo
app.UseCors("AllowAll"); // Aplica a política de CORS

// --- GRUPO DE ENDPOINTS DE AUTENTICAÇÃO ---
app.MapGroup("/api/auth").MapAuthApi();

// --- GRUPO DE ENDPOINTS DE USUÁRIO ---
app.MapGroup("/api/usuarios").MapUsuarioApi();

// --- GRUPO DE ENDPOINTS DE PRODUTOS ---
app.MapGroup("/api/produtos").MapProdutoApi();

// --- GRUPO DE ENDPOINTS DE CHAMADOS ---
app.MapGroup("/api/chamados").MapChamadoApi();


app.Run();

// --- DEFINIÇÃO DOS ENDPOINTS DE AUTENTICAÇÃO ---
public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login", ([FromBody] LoginRequestDto login) =>
        {
            if (login.IdEmpresarial == "A123" && login.Senha == "admin123")
            {
                var resposta = new { message = "Login realizado com sucesso!", nome = "João da Silva (Admin)" };
                return Results.Ok(resposta);
            }
            return Results.Json(new { message = "ID Empresarial ou Senha incorretos." }, statusCode: 401);
        });
        
        return app;
    }
}

// --- DEFINIÇÃO DOS ENDPOINTS DE USUÁRIO ---
public static class UsuarioEndpoints
{
    public static IEndpointRouteBuilder MapUsuarioApi(this IEndpointRouteBuilder app) 
    {
        app.MapPost("/registrar", ([FromBody] UsuarioCadastroDto novoUsuario) =>
        {
            var usuario = new Usuario
            {
                NomeUsuario = novoUsuario.Nome,
                IdEmpresarial = novoUsuario.IdEmpresarial,
                Email = novoUsuario.Email,
                Senha = "SENHA_CRIPTOGRAFADA_AQUI", 
                Cargo = "Funcionario", 
                Telefone = null
            };
            return Results.Ok(new { message = "Usuário cadastrado com sucesso!" });
        });

        return app;
    }
}

// --- DEFINIÇÃO DOS ENDPOINTS DE PRODUTOS ---
public static class ProdutoEndpoints
{
    public static IEndpointRouteBuilder MapProdutoApi(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () =>
        {
            var produtosSimulados = new List<Produto>
            {
                new Produto { IdProduto = 1, NomeProduto = "Monitor Dell 24\"", PrecoProduto = 1200.00m, DataFabricacao = new DateTime(2025, 1, 1), Validade = new DateTime(9999, 1, 1), Disponivel = true },
                new Produto { IdProduto = 2, NomeProduto = "Teclado Mecânico Logitech", PrecoProduto = 350.00m, DataFabricacao = new DateTime(2025, 2, 15), Validade = new DateTime(9999, 1, 1), Disponivel = true },
                new Produto { IdProduto = 3, NomeProduto = "SSD Samsung 1TB", PrecoProduto = 600.00m, DataFabricacao = new DateTime(2025, 3, 10), Validade = new DateTime(9999, 1, 1), Disponivel = false }
            };
            return Results.Ok(produtosSimulados);
        });

        return app;
    }
}

// --- DEFINIÇÃO DOS ENDPOINTS DE CHAMADOS ---
public static class ChamadoEndpoints
{
    public static IEndpointRouteBuilder MapChamadoApi(this IEndpointRouteBuilder app)
    {
        // Endpoint: POST /api/chamados (Criar chamado)
        app.MapPost("/", ([FromBody] ChamadoRequestDto novoChamado) =>
        {
            Console.WriteLine($"--- NOVO CHAMADO RECEBIDO ---");
            Console.WriteLine($"Assunto: {novoChamado.Assunto}");
            Console.WriteLine($"Descrição: {novoChamado.Descricao}");
            Console.WriteLine($"-------------------------------");
            return Results.Ok(new { message = "Chamado registrado com sucesso!" });
        });

        // --- NOVO ENDPOINT ---
        // Endpoint: GET /api/chamados (Listar chamados)
        app.MapGet("/", () =>
        {
            // --- LÓGICA DE BANCO DE DADOS (SIMULADA) ---
            // A pessoa do banco de dados irá trocar isso por uma consulta
            // na tabela SOLICITACAO_SERVICO
            
            var chamadosSimulados = new List<ChamadoDto>
            {
                new ChamadoDto 
                {
                    IdSolicitacao = 1,
                    Assunto = "Problema com acesso à VPN",
                    SolicitanteNome = "João Silva",
                    DataSolicitacao = new DateTime(2025, 10, 20),
                    Prioridade = "Alta",
                    Status = "Em andamento"
                },
                new ChamadoDto 
                {
                    IdSolicitacao = 2,
                    Assunto = "Erro ao gerar relatório de vendas",
                    SolicitanteNome = "Maria Souza",
                    DataSolicitacao = new DateTime(2025, 10, 18),
                    Prioridade = "Media",
                    Status = "Pendente"
                },
                new ChamadoDto 
                {
                    IdSolicitacao = 3,
                    Assunto = "Solicitação de instalação de software",
                    SolicitanteNome = "Pedro Costa",
                    DataSolicitacao = new DateTime(2025, 10, 15),
                    Prioridade = "Baixa",
                    Status = "Concluido"
                }
            };
            
            return Results.Ok(chamadosSimulados);
        });
        // --- FIM DO NOVO ENDPOINT ---

        return app;
    }
}