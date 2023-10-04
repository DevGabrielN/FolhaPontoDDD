using Newtonsoft.Json;

namespace FolhaPontoDDD.Domain.Models;

public class ConsolidadoDepartamento
{
    [JsonProperty(Order = 1)]
    public string Departamento { get; set; }
    [JsonProperty(Order = 2)]
    public string MesVigencia { get; set; }
    [JsonProperty(Order = 3)]
    public int AnoVigencia { get; set; }
    [JsonProperty(Order = 4)]
    public double TotalPagar { get; set; }
    [JsonProperty(Order = 5)]
    public double TotalDescontos { get; set; }
    
    [JsonProperty(Order = 6)]    
    public double TotalExtras { get; set; }
    [JsonProperty(Order = 7)]
    public List<ConsolidadoFuncionario> Funcionarios { get; set; }

    public ConsolidadoDepartamento(string departamento, string mesVigencia, int anoVigencia, double totalPagar, double totalDescontos, double totalHorasExtras, List<ConsolidadoFuncionario> funcionarios)
    {
        Departamento = departamento;
        MesVigencia = mesVigencia;
        AnoVigencia = anoVigencia;
        TotalPagar = totalPagar;
        TotalDescontos = totalDescontos;
        TotalExtras = totalHorasExtras;
        Funcionarios = funcionarios;
    }
}



