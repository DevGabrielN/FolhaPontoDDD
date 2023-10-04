using FolhaPontoDDD.Domain.Interfaces.Services;
using FolhaPontoDDD.Domain.Models;
using FolhaPontoDDD.Domain.Models.Exceptions;
using System.Globalization;
using System.Security.Cryptography;

namespace FolhaPontoDDD.Domain.Services;

public class FolhaPontoFuncionarioService : IFolhaPontoFuncionarioService
{
    private List<ArquivoCsv>? _csvFile { get; set; }

    public async Task<List<ConsolidadoFuncionario>> ConsolidaFuncionarioAsync(List<ArquivoCsv> csvFile)
    {
        _csvFile = csvFile;
        List<ConsolidadoFuncionario> consolidadoFuncionarios = new List<ConsolidadoFuncionario>();

        await Task.Run(() =>
        {
            Parallel.ForEach(_csvFile.GroupBy(funcionario => new { funcionario.Codigo, funcionario.AnoVigente, funcionario.MesVigente }).ToList(), listaFolhaPonto =>
            {
                var diasTrabalhadosEFaltas = GetDiasTrabalhadosEFaltas(listaFolhaPonto).Result;
                lock (consolidadoFuncionarios)
                {
                    consolidadoFuncionarios.Add(diasTrabalhadosEFaltas);
                }
            });
        });
        return consolidadoFuncionarios;
    }


    private static async Task<DateTime> GetMonthAsync(string mes)
    {
        return await Task.Run(() =>
        {
            if (DateTime.TryParseExact(mes.ToLower(), "MMMM", CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            throw new DomainException($"Erro ao converter o mês: '{mes}'. Formato inválido");
        });
    }

    private static async Task<DateTime> GetYearAsync(string ano)
    {
        return await Task.Run(() =>
        {
            if (DateTime.TryParseExact(ano.ToLower(), "yyyy", CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            throw new DomainException($"Erro ao converter o ano: '{ano}'. Formato inválido");
        });
    }

    private static async Task<bool> IsDiaUtilAsync(DateTime date)
    {
        var task = Task.Run(() =>
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            return true;
        });

        return await task;
    }

    private static async Task<int> CountDiasUteisAsync(DateTime year, DateTime month)
    {
        var fimMes = new DateTime(year.Year, month.Month + 1, 1).AddDays(-1);
        int diasUteis = 0;
        for (int i = 1; i <= fimMes.Day; i++)
        {
            DateTime data = new(year.Year, month.Month, i);
            if (await IsDiaUtilAsync(data))
            {
                diasUteis++;
            }
        }
        return diasUteis;
    }

    private static async Task<ConsolidadoFuncionario> GetDiasTrabalhadosEFaltas(IEnumerable<ArquivoCsv> folhaPonto)
    {
        #region variaveisLocais
        var mes = await GetMonthAsync(folhaPonto.Select(f => f.MesVigente).First());
        var ano = await GetYearAsync(folhaPonto.Select(f => f.AnoVigente).First());
        int diasUteisMes = await CountDiasUteisAsync(ano, mes);
        var folhaPontoList = folhaPonto.ToList();
        var constJornadaDeTrabalho = new TimeSpan(8, 0, 0); //Define o tempo de jornada de trabalho            
        var fimMes = new DateTime(ano.Year, mes.Month + 1, 1).AddDays(-1);
        int diasTrabalhados = 0;
        int diasExtras = 0;
        int diasFaltas = 0;
        double horasExtras = 0;
        double horasDebito = 0;
        double totalReceber = 0;
        double totalDescontos = 0;
        double totalHorasExtras = 0;
        ConsolidadoFuncionario consolidadoFuncionario;
        string nome = folhaPontoList.Select(f => f.Nome).First();
        int codigo = folhaPontoList.Select(f => f.Codigo).First();
        string departamento = folhaPontoList.Select(f => f.Departamento).First();
        double valorParaDescontar = (folhaPontoList
            .OrderByDescending(f => f.Data)
            .Select(f => f.ValorHora)
            .FirstOrDefault() * constJornadaDeTrabalho.TotalHours * 21) / diasUteisMes; //verifica o último valor de salário para cálcular o salário diário de acordo com os dias úteis do mês atual

        #endregion

        for (int i = 1; i <= fimMes.Day; i++)
        {
            var dataAtualLoop = new DateTime(ano.Year, mes.Month, i);
            var marcacaoDePontoDoDia = folhaPontoList.Where(f => f.Data.Equals(dataAtualLoop)).FirstOrDefault();
            //Verifica se é dia trabalhado ou falta
            if (marcacaoDePontoDoDia != null)
            {
                diasTrabalhados++;
                var periodoTrabalho = marcacaoDePontoDoDia.Saida.Subtract(marcacaoDePontoDoDia.Entrada);
                var periodoAlmoco = marcacaoDePontoDoDia.FimAlmoco.Subtract(marcacaoDePontoDoDia.InicioAlmoco);
                var horasTrabalhadas = periodoTrabalho.Subtract(periodoAlmoco);
                var valorSalarioPorHora = ((marcacaoDePontoDoDia.ValorHora * constJornadaDeTrabalho.TotalHours * 21) / diasUteisMes) / constJornadaDeTrabalho.TotalHours; //Cálcula o valor de salário por hora do dia atual, com base nos dias úteis do mês atual.
                var valorEsperado = valorSalarioPorHora * constJornadaDeTrabalho.TotalHours;
                if (await IsDiaUtilAsync(marcacaoDePontoDoDia.Data))
                {
                    if (horasTrabalhadas > constJornadaDeTrabalho)
                    {
                        horasExtras += horasTrabalhadas.Subtract(constJornadaDeTrabalho).TotalHours;
                    }
                    else if (horasTrabalhadas < constJornadaDeTrabalho)
                    {
                        horasDebito += horasTrabalhadas.Subtract(constJornadaDeTrabalho).TotalHours;
                    }
                    var receber = horasTrabalhadas.TotalHours * valorSalarioPorHora;
                    if (receber < valorEsperado)
                    {
                        totalDescontos += receber - valorEsperado;
                    }
                    else if (receber > valorEsperado)
                    {
                        totalHorasExtras += receber - valorEsperado;
                    }
                    totalReceber += receber;

                }
                else
                {
                    diasExtras++;
                    horasExtras += horasTrabalhadas.TotalHours;
                    var receber = horasTrabalhadas.TotalHours * valorSalarioPorHora * 2; //ganha dobrado em um dia não útil
                    totalReceber += receber;
                    totalHorasExtras += receber;
                }
            }
            else if (await IsDiaUtilAsync(dataAtualLoop))
            {
                diasFaltas++;                
                totalDescontos += -valorParaDescontar;
            }
        }
        consolidadoFuncionario = new ConsolidadoFuncionario(
            nome,
            codigo,
            ano,
            mes,
            departamento,
            Math.Round(totalReceber, 1),
            totalDescontos,
            totalHorasExtras,
            Math.Round(horasExtras, 2),
            Math.Round(horasDebito,2),
            diasFaltas,
            diasExtras,
            diasTrabalhados
        );

        return consolidadoFuncionario;
    }
}
