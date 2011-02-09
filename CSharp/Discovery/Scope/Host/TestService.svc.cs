namespace Service.Test
{
    public class Service : IServiceContract
    {
        public string GetEnvironment()
        {
            return "Test";
        }
    }
}