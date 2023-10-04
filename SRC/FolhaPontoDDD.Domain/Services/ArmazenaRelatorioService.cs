using FolhaPontoDDD.Domain.Interfaces.Services;
using FolhaPontoDDD.Domain.Models;
using FolhaPontoDDD.Domain.Models.Exceptions;
using Newtonsoft.Json;

namespace FolhaPontoDDD.Domain.Services;

public class ArmazenaRelatorioService : IArmazenaRelatorioService
{
    public async Task<bool> SaveJsonAsync(string path, List<ConsolidadoDepartamento> consolidadoDepartamentos)
    {
        if (!Directory.Exists(path))
        {
            throw new DomainException($"Falha ao localizar o diretório: {path}");
        }

        try
        {            
            var nomeArquivo = $"ConsolidadoFolhaPonto_{DateTime.Now.ToString("ddMMyyyy_HHmm")}.json";
            var json = JsonConvert.SerializeObject(consolidadoDepartamentos, Formatting.Indented);

            using (StreamWriter writer = File.CreateText(Path.Combine(path, nomeArquivo)))
            {
                await writer.WriteAsync(json);
            }
            
            return true;

        }
        catch (Exception ex)
        {            
            throw new DomainException($"Ocorreu um erro inesperado ao tentar salvar o arquivo Json: {ex.Message}");            
        }

    }

}
