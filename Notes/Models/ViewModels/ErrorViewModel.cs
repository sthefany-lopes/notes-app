namespace Notes.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string? Message { get; set; }

        public ErrorViewModel()
        {
        }

        public ErrorViewModel(string? requestId, string? message)
        {
            RequestId = requestId;
            Message = message;
        }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}