namespace Service
{
    public class RestService : IRestContract
    {
        public string GetValue(string value)
        {
            return string.Format("Get: {0}", value);
        }

        public string PostValue(string value)
        {
            return string.Format("Post: {0}", value);
        }
    }
}