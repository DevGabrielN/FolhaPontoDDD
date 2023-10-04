using FolhaPontoDDD.Domain.Models;

namespace FolhaPontoDDD.Domain.Interfaces.Services;

public interface IFileService
{
    Task<List<ArquivoCsv>> ReadAsync(string filesPath, char delimitador, bool containHeader);
}
