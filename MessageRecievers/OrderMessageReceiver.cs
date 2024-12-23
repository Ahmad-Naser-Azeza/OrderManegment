namespace MessageRecievers
{
    public class OrderMessageReceiver : BackgroundService
    {
        private readonly ILogger<OrderMessageReceiver> _logger;
        private readonly OrderReceiverService _orderReceiverService;

        public OrderMessageReceiver(ILogger<OrderMessageReceiver> logger, OrderReceiverService orderReceiverService)
        {
            _logger = logger;
            _orderReceiverService = orderReceiverService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Start receiving RabbitMQ messages
            _orderReceiverService.StartReceiving();
            
        }
    }
}
