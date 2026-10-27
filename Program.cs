using Animagens_MuHNA.FrontEnd.Components;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. SERVIÇOS
// ==========================================

// Suporte ao Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 👉 ADICIONADO: Habilita o uso de Controllers de API no projeto
builder.Services.AddControllers();

// (Opcional) Se quiser Swagger para testar as rotas da API:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================================
// 2. PIPELINE DE REQUISIÇÕES (MIDDLEWARES)
// ==========================================

if (app.Environment.IsDevelopment())
{
    // 👉 ADICIONADO: Ativa o Swagger no ambiente de desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// 👉 ADICIONADO: Permite que o navegador acesse arquivos/fotos dinâmicas salvas em wwwroot
app.UseStaticFiles();

app.UseAntiforgery();

app.MapStaticAssets();

// ==========================================
// 3. MAPEAMENTO DE ROTAS
// ==========================================

// 👉 ADICIONADO: Ativa as rotas de todos os seus Controllers (ex: /api/values, /api/imagens)
app.MapControllers();

// Rotas do Blazor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();