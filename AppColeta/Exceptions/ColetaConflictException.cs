using System;

namespace SOColeta.Exceptions
{

	[Serializable]
	public class ColetaConflictException : Exception
	{
		public ColetaConflictException() { }
		public ColetaConflictException(string message) : base(message) { }
		public ColetaConflictException(string message, Exception inner) : base(message, inner) { }
		protected ColetaConflictException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
