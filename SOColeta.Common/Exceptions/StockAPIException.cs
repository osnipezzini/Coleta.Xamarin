namespace SOColeta.Common.Exceptions;

[Serializable]
public class StockAPIException : Exception
{
    public StockAPIException() { }
    public StockAPIException(string message) : base(message) { }
    public StockAPIException(string message, Exception inner) : base(message, inner) { }
    protected StockAPIException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
