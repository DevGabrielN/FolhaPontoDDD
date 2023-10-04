namespace FolhaPontoDDD.Domain.Models;

public class ArquivoCsv
{
    public string Departamento { get; set; }
    public string MesVigente { get; set; }
    public string AnoVigente { get; set; }
    public int Codigo { get; set; }
    public string Nome { get; set; }
    public double ValorHora { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan Entrada { get; set; }
    public TimeSpan Saida { get; set; }
    public TimeSpan InicioAlmoco { get; set; }
    public TimeSpan FimAlmoco { get; set; }
    public ArquivoCsv(string departamento, string mesVigente, string anoVigente, int codigo, string nome, double valorHora, DateTime data, TimeSpan entrada, TimeSpan saida, TimeSpan inicioAlmoco, TimeSpan fimAlmoco)
    {
        Departamento = departamento;
        MesVigente = mesVigente;
        AnoVigente = anoVigente;
        Codigo = codigo;
        Nome = nome;
        ValorHora = valorHora;
        Data = data;
        Entrada = entrada;
        Saida = saida;
        InicioAlmoco = inicioAlmoco;
        FimAlmoco = fimAlmoco;
    }
    public ArquivoCsv()
    { 
    }
}
