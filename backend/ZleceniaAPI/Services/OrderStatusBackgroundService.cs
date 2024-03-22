using Microsoft.Extensions.DependencyInjection;

namespace ZleceniaAPI.Services
{
    public class OrderStatusBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<OrderStatusBackgroundService> _logger;

        public OrderStatusBackgroundService(IServiceScopeFactory scopeFactory, ILogger<OrderStatusBackgroundService> logger)
        {
            _serviceScopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                    await orderService.CheckAndUpdateOrderStatus();
                }

                _logger.LogInformation("Executed background task");


                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
