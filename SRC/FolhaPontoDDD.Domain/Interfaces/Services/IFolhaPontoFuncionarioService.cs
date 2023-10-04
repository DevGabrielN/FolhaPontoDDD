using FolhaPontoDDD.Domain.Models;
namespace FolhaPontoDDD.Domain.Interfaces.Services;
public interface IFolhaPontoFuncionarioService
{
    Task<List<ConsolidadoFuncionario>> ConsolidaFuncionarioAsync(List<ArquivoCsv> csvFile);
}
