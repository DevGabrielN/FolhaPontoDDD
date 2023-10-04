using FolhaPontoDDD.Domain.Models;

namespace FolhaPontoDDD.Domain.Interfaces.Services;

public interface IFolhaPontoDepartamentoService
{
    Task<List<ConsolidadoDepartamento>> ConsolidaDepartamentoAsync(List<ConsolidadoFuncionario> consolidadoFuncionarios);
}
