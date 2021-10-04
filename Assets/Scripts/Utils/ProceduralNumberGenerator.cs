namespace Utils
{
	public class ProceduralNumberGenerator {
		private static int currentPosition = 0;
		private const string key = "12342412123334242143223314412441212322344321212233344";

		public static int GetNextNumber() {
			var currentNum = key.Substring(currentPosition++ % key.Length, 1);
			return int.Parse (currentNum);
		}
	}
}
