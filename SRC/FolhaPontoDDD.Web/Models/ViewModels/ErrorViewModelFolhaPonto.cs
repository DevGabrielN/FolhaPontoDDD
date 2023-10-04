namespace FolhaPontoDDD.Web.Models.ViewModels;

public class ErrorViewModelFolhaPonto
{
    public string? RequestId { get; set; }
    public string Message { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
