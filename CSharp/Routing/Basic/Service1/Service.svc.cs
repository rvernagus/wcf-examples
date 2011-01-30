namespace Service1
{
    public class Service : IContract
    {
        #region IContract Members

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        #endregion
    }
}