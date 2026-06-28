namespace LibraryManagementAPI.Shared.UseCase;

public interface IConsumerUseCase <Request>
{
    void Execute(Request request);
}