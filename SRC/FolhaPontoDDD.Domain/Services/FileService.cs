using FolhaPontoDDD.Domain.Interfaces.Services;
using FolhaPontoDDD.Domain.Models;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace FolhaPontoDDD.Domain.Services;

public class FileService : IFileService
{
    public async Task<List<ArquivoCsv>> ReadAsync(string filesPath, char delimitador, bool containHeader)
    {
        var files = Directory.GetFiles($@"{filesPath}", "*.csv");
        List<ArquivoCsv> arquivosCsv = new();
        var linesCollection = new BlockingCollection<string[]>();
        
        var taskRead = Task.Run(() =>
        {
            try
            {
                Parallel.ForEach(files, filePath =>
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        bool primeiraLinha = true;
                        string? line;
                        string[] infoNameFile = Path.GetFileNameWithoutExtension(filePath)
                                                        .Split('-')
                                                        .Select(part => part.Trim()).ToArray();

                        while ((line = reader.ReadLine()) != null)
                        {
                            if (primeiraLinha && containHeader)
                            {
                                primeiraLinha = false;
                                continue;
                            }
                            string[] linesArray = line.Split(new char[] { delimitador });
                            linesCollection.Add(linesArray.Concat(infoNameFile).ToArray());
                        }
                    }
                });
            }
            finally
            {
                linesCollection.CompleteAdding();
            }
        });
        
        var taskProcess = Task.Run(() =>
        {
            Parallel.ForEach(linesCollection.GetConsumingEnumerable(), line =>
            {
                string[] almoco = line[6].Split("-");
                var linhaArquivo = new ArquivoCsv
                {
                    Codigo = int.Parse(line[0]),
                    Nome = line[1],
                    ValorHora = double.Parse(Regex.Match(line[2], @"\d+(,\d+)?").Value),
                    Data = DateTime.Parse(line[3]),
                    Entrada = TimeSpan.Parse(line[4]),
                    Saida = TimeSpan.Parse(line[5]),
                    InicioAlmoco = TimeSpan.Parse(almoco[0]),
                    FimAlmoco = TimeSpan.Parse(almoco[1]),
                    Departamento = line[7],
                    MesVigente = line[8],
                    AnoVigente = line[9]
                };
                lock (arquivosCsv)
                {
                    arquivosCsv.Add(linhaArquivo);
                }
            });
        });
        await Task.WhenAll(taskRead, taskProcess);

        return arquivosCsv;
    }
}
