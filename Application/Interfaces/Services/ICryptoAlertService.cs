namespace ApiWithOtherApi.Application.Interfaces.Services
{
    public interface ICryptoAlertService
    {
        public Task CheckPriceAsync(string coinName, decimal targetPrice , string jobId);
    }
}
