namespace Service1
{
    public class Service : IService1Contract
    {
        #region IService1Contract Members

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        #endregion
    }
}