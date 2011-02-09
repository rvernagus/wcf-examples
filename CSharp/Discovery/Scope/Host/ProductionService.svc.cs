namespace Service.Production
{
    public class Service : IServiceContract
    {
        public string GetEnvironment()
        {
            return "Production";
        }
    }
}