namespace LibraryManagementAPI.Shared.UseCase;

public interface ISupplierUseCase <Response>
{
    Task<Response> Execute();
}