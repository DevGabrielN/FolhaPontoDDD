using FolhaPontoDDD.Domain.Services;

namespace TestProject;

public class FolhaPontoTests
{
    private readonly string DiretorioOrigem = @"C:\FolhaPontoFuncionarios";
    private readonly string DiretorioDestino = @"C:\FolhaPontoFuncionarios";

    private readonly FileService FileService = new ();
    private readonly FolhaPontoFuncionarioService FolhaPontoFuncionarioService = new ();
    private readonly FolhaPontoDepartamentoService FolhaPontoDepartamentoService = new ();
    private readonly ArmazenaRelatorioService ArmazenaRelatorioService = new ();

    [Fact]
    public async void CT_01_01_ImportarArquivo()
    {
        bool result = false;
        try
        {
            var files = await FileService.ReadAsync(DiretorioOrigem, ';', true);
            var consolidadoFuncionarios = await FolhaPontoFuncionarioService.ConsolidaFuncionarioAsync(files);
            var consolidadoDepartamentos = await FolhaPontoDepartamentoService.ConsolidaDepartamentoAsync(consolidadoFuncionarios);
            result = await ArmazenaRelatorioService.SaveJsonAsync(DiretorioDestino, consolidadoDepartamentos);
        }
        finally 
        {
            Assert.True(result);
        }
    }

}