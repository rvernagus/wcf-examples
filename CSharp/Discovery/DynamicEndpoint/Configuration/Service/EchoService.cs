namespace Discovery.Service
{
    public class EchoService : IEchoService
    {
        public string Echo(string text)
        {
            return text;
        }
    }
}