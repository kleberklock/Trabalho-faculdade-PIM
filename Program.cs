using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaChamadosApi.Data;
using SistemaChamadosApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

// 2. LER A CONNECTION STRING DO appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 3. REGISTRAR O DBCONTEXT (A PONTE COM O BANCO)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// 4. Pipeline
app.UseCors("AllowAll");
app.UseDefaultFiles();
app.UseStaticFiles();

// 5. Mapeamento de Rotas
app.MapGroup("/api/auth").MapAuthApi();
app.MapGroup("/api/usuarios").MapUsuarioApi();
app.MapGroup("/api/produtos").MapProdutoApi();
app.MapGroup("/api/chamados").MapChamadoApi();

app.MapFallbackToFile("index.html");

app.Run();

// --- ENDPOINTS COM BANCO DE DADOS (ATUALIZADOS) ---

// **********************************************
// ********* CLASSE AUTHENDPOINTS (LOGIN) *********
// **********************************************
public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthApi(this IEndpointRouteBuilder app)
    {
        // Rota de LOGIN: /api/auth/login
        app.MapPost("/login", async ([FromBody] LoginRequestDto login, AppDbContext db) =>
        {
            Console.WriteLine($"[API] Recebida tentativa de login para ID: {login.IdEmpresarial}");
            
            // Busca o usuário que corresponda ao ID Empresarial E à Senha
            var usuario = await db.Usuarios
                .FirstOrDefaultAsync(u => u.IdEmpresarial == login.IdEmpresarial && u.Senha == login.Senha);

            if (usuario != null)
            {
                Console.WriteLine($"[API] Login APROVADO para ID: {usuario.IdEmpresarial}");
                // Retorna os dados do usuário para o front-end
                return Results.Ok(new { 
                    message = "Sucesso", 
                    nome = usuario.NomeUsuario,
                    idEmpresarial = usuario.IdEmpresarial,
                    cargo = usuario.Cargo
                });
            }

            Console.WriteLine($"[API] Login REJEITADO para ID: {login.IdEmpresarial}");
            return Results.Json(new { message = "Credenciais inválidas" }, statusCode: 401);
        });
        return app;
    }
}

// ************************************************
// ********* CLASSE USUARIOENDPOINTS (CADASTRO) *********
// ************************************************
public static class UsuarioEndpoints
{
    // Rota de CADASTRO: /api/usuarios/registrar
    public static IEndpointRouteBuilder MapUsuarioApi(this IEndpointRouteBuilder app)
    {
        // Registrar Novo Usuário
        app.MapPost("/registrar", async ([FromBody] UsuarioCadastroDto u, AppDbContext db) =>
        {
            // Opcional: Verifica se o ID Empresarial já existe antes de tentar salvar
            var existe = await db.Usuarios.AnyAsync(x => x.IdEmpresarial == u.IdEmpresarial);
            if (existe)
            {
                return Results.BadRequest(new { message = "ID Empresarial já cadastrado." });
            }

            // Mapeia o DTO para o Modelo de Banco de Dados
            var novoUsuario = new Usuario
            {
                IdEmpresarial = u.IdEmpresarial,
                NomeUsuario = u.Nome, 
                Email = u.Email,
                Senha = u.Senha, // LEMBRETE: EM PRODUÇÃO, USE HASH DE SENHA!
                Cargo = "Funcionario" // Padrão
            };

            await db.Usuarios.AddAsync(novoUsuario);
            
            try
            {
                await db.SaveChangesAsync(); // Salva no banco
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erro ao salvar usuário: {ex.Message}");
                return Results.BadRequest(new { message = "Erro ao salvar dados no banco de dados." });
            }

            Console.WriteLine($"[API] Novo usuário registrado: {u.Nome} (ID: {u.IdEmpresarial})");
            return Results.Created($"/api/usuarios/{u.IdEmpresarial}", new { message = "Cadastrado!" });
        });
        return app;
    }
    
    // ... Aqui estariam outras rotas de usuários
}

// ************************************************
// ********* OUTROS ENDPOINTS (MANTIDOS) **********
// ************************************************

// Classe ProdutoEndpoints (Manter sua implementação existente)
public static class ProdutoEndpoints
{
    public static IEndpointRouteBuilder MapProdutoApi(this IEndpointRouteBuilder app)
    {
        // Mantenha aqui a sua implementação para /api/produtos
        return app;
    }
}

// Classe ChamadoEndpoints (Implementação da rota POST e rota GET de listagem)
public static class ChamadoEndpoints
{
    public static IEndpointRouteBuilder MapChamadoApi(this IEndpointRouteBuilder app)
    {
        // NOVO ENDPOINT: POST /api/chamados
        app.MapPost("/", async ([FromBody] ChamadoRequestDto chamadoDto, AppDbContext db) =>
        {
            Console.WriteLine($"[API] Tentativa de novo chamado para ID Emp: {chamadoDto.IdEmpresarial}");
            
            // 1. Encontrar o ID_USUARIO (Primary Key) baseado no IdEmpresarial
            var usuario = await db.Usuarios
                .FirstOrDefaultAsync(u => u.IdEmpresarial == chamadoDto.IdEmpresarial);

            if (usuario == null)
            {
                Console.WriteLine($"[API] Erro: Usuário com ID Empresarial {chamadoDto.IdEmpresarial} não encontrado.");
                return Results.Unauthorized(); // 401: Usuário não identificado
            }

            var idUsuario = usuario.IdUsuario;
            
            // 2. Criar e Salvar a entidade SERVICO
            // Usamos a descrição e o ID do usuário para o registro de serviço
            var novoServico = new Servico
            {
                DataServico = DateTime.Now,
                // Combina Assunto e Categoria na descrição do Serviço, pois a tabela SERVICO só tem uma coluna de Descrição.
                DescricaoServico = $"[{chamadoDto.Categoria}] {chamadoDto.Assunto}",
                IdUsuario = idUsuario 
            };
            
            await db.Servicos.AddAsync(novoServico);
            
            // *ATENÇÃO*: O SaveChangesAsync() é crucial aqui para gerar o ID_SERVICO
            try 
            {
                await db.SaveChangesAsync(); 
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"[API] ERRO: Falha ao salvar Serviço. {ex.Message}");
                return Results.Problem("Erro ao salvar Serviço no banco de dados.", statusCode: 500);
            }
            
            var idServico = novoServico.IdServico; // ID_SERVICO gerado após o SaveChanges
            
            // 3. Criar e Salvar a entidade SOLICITACAO_SERVICO (o Chamado)
            var novaSolicitacao = new SolicitacaoServico
            {
                IdServico = idServico,
                IdUsuario = idUsuario,
                DataSolicitacao = DateTime.Now,
                Descricao = chamadoDto.Descricao,
                Prioridade = chamadoDto.Prioridade,
                Status = "Pendente" // Status inicial
            };
            
            await db.SolicitacoesServico.AddAsync(novaSolicitacao);
            
            // Salvar a solicitação no banco
            try
            {
                await db.SaveChangesAsync();
                
                // IMPRIME O RETORNO NO TERMINAL, conforme solicitado
                Console.WriteLine($"[API] CHAMADO REGISTRADO com SUCESSO!");
                Console.WriteLine($"> ID Chamado: {novaSolicitacao.IdSolicitacao}");
                Console.WriteLine($"> ID Serviço: {idServico}");
                Console.WriteLine($"> Assunto: {chamadoDto.Assunto}");
                Console.WriteLine($"> Prioridade: {chamadoDto.Prioridade}");
                Console.WriteLine($"[API] Solicitação Salva no Banco de Dados.");

                // Retorna a confirmação para o front-end
                return Results.Created($"/api/chamados/{novaSolicitacao.IdSolicitacao}", new 
                {
                    message = "Chamado criado com sucesso!",
                    idChamado = novaSolicitacao.IdSolicitacao
                });
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"[API] ERRO: Falha ao salvar Solicitação. {ex.Message}");
                return Results.Problem("Erro ao salvar Solicitação no banco de dados.", statusCode: 500);
            }
        });
        
        // --- Exemplo de Rota GET para listar chamados (necessária para o Front-end) ---
        // Adaptei o GET para usar o DTO e incluir o nome do solicitante, 
        // fazendo o Join implícito que o EF Core permite (Include).
        app.MapGet("/", async (AppDbContext db) =>
        {
            var chamados = await db.SolicitacoesServico
                .Include(s => s.Usuario) // Inclui o usuário para pegar o nome
                .OrderByDescending(s => s.DataSolicitacao)
                .Select(s => new ChamadoDto // Mapeia para o DTO
                {
                    IdSolicitacao = s.IdSolicitacao,
                    Assunto = s.Servico != null ? s.Servico.DescricaoServico : s.Descricao, // Assunto vem do Servico
                    Descricao = s.Descricao,
                    SolicitanteNome = s.Usuario != null ? s.Usuario.NomeUsuario : "Desconhecido",
                    DataSolicitacao = s.DataSolicitacao,
                    Prioridade = s.Prioridade,
                    Status = s.Status
                })
                .ToListAsync();

            return Results.Ok(chamados);
        });
        
        // --- Exemplo de Rota GET por ID ---
        app.MapGet("/{id}", async (int id, AppDbContext db) =>
        {
            var chamado = await db.SolicitacoesServico
                .Include(s => s.Servico)
                .Include(s => s.Usuario)
                .Where(s => s.IdSolicitacao == id)
                .Select(s => new ChamadoDto 
                {
                    IdSolicitacao = s.IdSolicitacao,
                    Assunto = s.Servico != null ? s.Servico.DescricaoServico : s.Descricao,
                    Descricao = s.Descricao,
                    SolicitanteNome = s.Usuario != null ? s.Usuario.NomeUsuario : "Desconhecido",
                    DataSolicitacao = s.DataSolicitacao,
                    Prioridade = s.Prioridade,
                    Status = s.Status
                })
                .FirstOrDefaultAsync();

            if (chamado == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(chamado);
        });
        
        return app;
    }
}