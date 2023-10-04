using System.ComponentModel.DataAnnotations;

namespace FolhaPontoDDD.Web.Models;

public class FolhaPonto
{
    [DiretorioExistente(ErrorMessage = "Diretório inválido.")]
    [Display(Name = "Diretório de origem")]
    [Required(ErrorMessage = "O {0} é obrigatório")]
    public string DiretorioOrigem { get; set; }

    [DiretorioExistente(ErrorMessage = "Diretório inválido.")]
    [Display(Name = "Diretório de destino")]
    [Required(ErrorMessage = "O {0} é obrigatório")]
    public string DiretorioDestino { get; set; }
    [Required(ErrorMessage = "O {0} é obrigatório")]
    public bool ContainHeaders { get; set; }
    [Required(ErrorMessage = "O {0} é obrigatório")]
    public char Delimitador { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public long Milissegundos { get; set; }
}

public class DiretorioExistenteAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext? validationContext)
    {
        string diretorio = (string)value;

        if (Directory.Exists(diretorio))
        {
            return ValidationResult.Success; 
        }

        return new ValidationResult(ErrorMessage ?? "Diretório inválido");
    }
}
