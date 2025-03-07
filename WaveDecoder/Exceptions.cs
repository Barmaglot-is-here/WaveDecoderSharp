namespace WaveDecoderSharp;

[Serializable]
public class WaveLoadingException : Exception
{
	public WaveLoadingException() { }
	public WaveLoadingException(string message) : base(message) { }
	public WaveLoadingException(string message, Exception inner) : base(message, inner) { }
	protected WaveLoadingException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}