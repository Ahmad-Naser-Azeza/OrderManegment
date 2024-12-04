using Microsoft.AspNetCore.Mvc.Testing;

namespace Order.Test.Factory
{
    public class OrdersControllerFactory : IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;

        public OrdersControllerFactory()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {

                });
        }

        public HttpClient CreateClient()
        {
            return _factory.CreateClient();
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}
