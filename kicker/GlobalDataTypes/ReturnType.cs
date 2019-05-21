namespace GlobalDataTypes
{
    /// <summary>
    /// Return value of methods which perform actions which cannot raise exceptions.
    /// </summary>
    public enum ReturnType
    {
        /// <summary>
        /// Action has been performed successfully.
        /// </summary>
        Ok = 0,
        
        /// <summary>
        /// Action hasn't been performed successfully.
        /// </summary>
        NotOk = 1
    }
}