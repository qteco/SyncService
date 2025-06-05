using SyncService.Core.Interfaces.ApiClients;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;

public class BroadsoftService : IBroadsoftService
{
    private readonly HashSet<string> ProcessedCallIds = new();
    private readonly IRequesterRepository _requesterRepository;
    private readonly IBroadsoftApiClient _apiClient;
    private readonly ISuperopsApiClient _superopsApiClient;

    public BroadsoftService(IBroadsoftApiClient apiClient, IRequesterRepository context, ISuperopsApiClient superopsApiClient)
    {
        _apiClient = apiClient;
        _requesterRepository = context;
        _superopsApiClient = superopsApiClient;
    }

    public async Task MonitorCallCenterAsync()
    {
        var result = await _apiClient.GetLatestCallAsync();

        if (result == null) return;

        var (callId, phoneNumber) = result.Value;

        if (ProcessedCallIds.Contains(callId)) return;

        try
        {
            var requester = await _requesterRepository.GetRequester(phoneNumber);
            Console.WriteLine(requester.UserId);
            Console.WriteLine(requester.Id);
            Console.WriteLine(requester.AccountId);

            if (requester.Id != null)
            {
                Ticket ticket = new Ticket(requester.AccountId, requester.UserId);
                _superopsApiClient.CreateTicket(ticket);
                Console.WriteLine($"Call ticket created for call {phoneNumber}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing call {callId}: {ex.Message}");
        }

        ProcessedCallIds.Add(callId);
    }
}