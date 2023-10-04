using FolhaPontoDDD.Domain.Interfaces.Services;
using FolhaPontoDDD.Domain.Models.Exceptions;
using FolhaPontoDDD.Web.Models;
using FolhaPontoDDD.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FolhaPontoDDD.Web.Controllers;

public class FolhaPontoController : Controller
{
    private readonly IFileService _fileService;   
    private readonly IArmazenaRelatorioService _armazenaRelatorioService;
    private readonly IFolhaPontoFuncionarioService _folhaPontoFuncionarioService;
    private readonly IFolhaPontoDepartamentoService _folhaPontoDepartamentoService;
    public FolhaPontoController(
        IFileService fileService,
        IArmazenaRelatorioService armazenaRelatorioService, 
        IFolhaPontoFuncionarioService folhaPontoFuncionarioService, 
        IFolhaPontoDepartamentoService folhaPontoDepartamentoService)
    {
        _fileService = fileService;
        _armazenaRelatorioService = armazenaRelatorioService;
        _folhaPontoFuncionarioService = folhaPontoFuncionarioService;
        _folhaPontoDepartamentoService = folhaPontoDepartamentoService;
    }
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Index(FolhaPonto model)
    {
        var stopwatch = new Stopwatch();        
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }
        try
        {
            stopwatch.Start();
            var csvfile = await _fileService.ReadAsync(model.DiretorioOrigem, model.Delimitador, model.ContainHeaders);
            if (!csvfile.Any())
            {
                throw new DomainException("Nenhum valor encontrado no(s) arquivo(s) .CSV");
            }
            var conFuncionario = await _folhaPontoFuncionarioService.ConsolidaFuncionarioAsync(csvfile);
            var conDepartamento = await _folhaPontoDepartamentoService.ConsolidaDepartamentoAsync(conFuncionario);
            await _armazenaRelatorioService.SaveJsonAsync(model.DiretorioDestino, conDepartamento);

            stopwatch.Stop();
            model.Mensagem = "O relatório foi gerado com sucesso.";
            model.Milissegundos = stopwatch.ElapsedMilliseconds;
            
            return View("Index", model);
        }
        catch (Exception ex)
        {            
            return RedirectToAction(nameof(Error), new { message = $"{ex.Message}" });
        }
    }

    public IActionResult Error(string message)
    {
        var viewModel = new ErrorViewModelFolhaPonto
        {
            Message = message,
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };

        return View(viewModel);
    }
}
