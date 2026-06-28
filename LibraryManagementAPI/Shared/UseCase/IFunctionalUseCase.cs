namespace LibraryManagementAPI.Shared.UseCase;

public interface IFunctionalUseCase<Request, Response>
{
    Task<Response> Execute(Request request);
}