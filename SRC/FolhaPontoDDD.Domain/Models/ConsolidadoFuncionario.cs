using Newtonsoft.Json;

namespace FolhaPontoDDD.Domain.Models;

public class ConsolidadoFuncionario
{
    [JsonProperty(Order = 1)]
    public string Nome { get; set; }
    [JsonProperty(Order = 2)]
    public int Codigo { get; set; }
    [JsonIgnore]
    public DateTime Ano { get; set; }
    [JsonIgnore]
    public DateTime Mes { get; set; }
    [JsonIgnore]
    public string Departamento { get; set; }
    [JsonProperty(Order = 3)]
    public double TotalReceber { get; set; }
    [JsonIgnore]
    public double TotalDescontos { get; set; }
    [JsonIgnore]
    public double TotalHorasExtras { get; set; }
    [JsonProperty(Order = 4)]
    public double HorasExtras { get; set; }
    [JsonProperty(Order = 5)]
    public double HorasDebito { get; set; }
    [JsonProperty(Order = 6)]
    public int DiasFalta { get; set; }
    [JsonProperty(Order = 7)]
    public int DiasExtras { get; set; }
    [JsonProperty(Order = 8)]
    public int DiasTrabalhados { get; set; }
   
    public ConsolidadoFuncionario(string nome, int codigo, DateTime ano, DateTime mes, string departamento, double totalReceber, double totalDescontos, double totalHorasExtras, double horasExtras, double horasDebito, int diasFalta, int diasExtras, int diasTrabalhados)
    {
        Nome = nome;
        Codigo = codigo;
        Ano = ano;
        Mes = mes;
        Departamento = departamento;
        TotalReceber = totalReceber;
        TotalDescontos = totalDescontos;
        TotalHorasExtras = totalHorasExtras;
        HorasExtras = horasExtras;
        HorasDebito = horasDebito;
        DiasFalta = diasFalta;
        DiasExtras = diasExtras;
        DiasTrabalhados = diasTrabalhados;
    }
}
