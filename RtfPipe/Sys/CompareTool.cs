// project    : System Framelet
namespace RtfPipe.Sys
{

	public static class CompareTool
	{

		public static bool AreEqual( object left, object right )
		{
			return left == right || ( left != null && left.Equals( right ) );
		}

	}

}

