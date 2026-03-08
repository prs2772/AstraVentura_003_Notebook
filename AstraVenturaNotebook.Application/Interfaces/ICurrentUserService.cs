namespace AstraVenturaNotebook.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string GetUserId(); // Extraerá el ID del token generado por AstraVenturaAuth
    }
}
