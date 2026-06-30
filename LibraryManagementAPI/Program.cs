using LibraryManagementAPI.Book.UseCase;
using LibraryManagementAPI.Book.UseCase.Response;
using LibraryManagementAPI.Controller.Member.Request;
using LibraryManagementAPI.Core.Book.Repository;
using LibraryManagementAPI.Core.Book.Repository.InMemory;
using LibraryManagementAPI.Core.Book.UseCase;
using LibraryManagementAPI.Core.Member.Repository;
using LibraryManagementAPI.Core.Member.Repository.InMemory;
using LibraryManagementAPI.Core.Member.UseCase;
using LibraryManagementAPI.Core.Member.UseCase.Dtos;
using LibraryManagementAPI.Shared.UseCase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
builder.Services.AddSingleton<IMemberRepository, InMemoryMemberRepository>();

builder.Services.AddScoped<ISupplierUseCase<List<BookDto>>, ListAllBooksUseCase>();
builder.Services.AddScoped<IFunctionalUseCase<Guid, BookDto>, FindBookByIdUseCase>();
builder.Services.AddScoped<IFunctionalUseCase<CreateMemberRequest, MemberDto>, CreateMemberUseCase>();

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();