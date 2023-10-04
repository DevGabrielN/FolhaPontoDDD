using FolhaPontoDDD.Domain.Models;

namespace FolhaPontoDDD.Domain.Interfaces.Services;

public interface IArmazenaRelatorioService
{
    Task<bool> SaveJsonAsync(string path, List<ConsolidadoDepartamento> consolidadoDepartamentos);
}
