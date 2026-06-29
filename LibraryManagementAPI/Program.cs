using LibraryManagementAPI.Book.Repository;
using LibraryManagementAPI.Book.Repository.InMemory;
using LibraryManagementAPI.Book.UseCase;
using LibraryManagementAPI.Book.UseCase.Response;
using LibraryManagementAPI.Shared.UseCase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();

builder.Services.AddScoped<ISupplierUseCase<List<BookDto>>, ListAllBooksUseCase>();
builder.Services.AddScoped<IFunctionalUseCase<Guid, BookDto>, FindBookByIdUseCase>();

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();