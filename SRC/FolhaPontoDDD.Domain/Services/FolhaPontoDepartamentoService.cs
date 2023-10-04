using FolhaPontoDDD.Domain.Interfaces.Services;
using FolhaPontoDDD.Domain.Models;
using System.Globalization;

namespace FolhaPontoDDD.Domain.Services;

public class FolhaPontoDepartamentoService : IFolhaPontoDepartamentoService
{
    public async Task<List<ConsolidadoDepartamento>> ConsolidaDepartamentoAsync(List<ConsolidadoFuncionario> consolidadoFuncionarios)
    {
        List<ConsolidadoDepartamento> consolidadoDepartamentos = new List<ConsolidadoDepartamento>();
        await Task.Run(() =>
        {
            Parallel.ForEach(consolidadoFuncionarios.GroupBy(f => new { f.Departamento, f.Ano, f.Mes }).ToList(), funcionarios =>
            {
                var consolidado = ConsolidadoDepartamentoAsync(funcionarios).Result;
                lock (consolidadoDepartamentos)
                {
                    consolidadoDepartamentos.Add(consolidado);
                }
            });
        });
        return consolidadoDepartamentos;
    }

    private static async Task<ConsolidadoDepartamento> ConsolidadoDepartamentoAsync(IEnumerable<ConsolidadoFuncionario> consolidadoFuncionarios)
    {
        ConsolidadoDepartamento consolidadoDepartamento;
        string departamento = consolidadoFuncionarios.Select(consolidado => consolidado.Departamento).First();
        string mesVigencia = consolidadoFuncionarios.Select(consolidado => consolidado.Mes).First().ToString("MMMM", new CultureInfo("pt-BR"));
        int anoVigencia = consolidadoFuncionarios.Select(consolidado => consolidado.Ano).First().Year;
        double totalPagar = consolidadoFuncionarios.Sum(consolidado => consolidado.TotalReceber);
        double totalDescontos = consolidadoFuncionarios.Sum(consolidado => consolidado.TotalDescontos);
        double totalHorasExtras = consolidadoFuncionarios.Sum(consolidado => consolidado.TotalHorasExtras);

        consolidadoDepartamento = new ConsolidadoDepartamento(
            departamento,
            mesVigencia,
            anoVigencia,
            Math.Round(totalPagar, 1),
            Math.Round(totalDescontos, 1),
            Math.Round(totalHorasExtras, 1),
            consolidadoFuncionarios.ToList());

        return await Task.FromResult(consolidadoDepartamento);

    }

}
