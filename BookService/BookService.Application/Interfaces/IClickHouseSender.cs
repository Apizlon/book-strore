namespace BookService.Application.Interfaces;

public interface IClickHouseSender
{
    Task SendEventAsync(string eventType, string userName, string status, string errorMessage = "");
}