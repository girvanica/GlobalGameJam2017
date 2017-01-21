/// <summary>
/// Static Calls Useful Everywhere.
/// </summary>
public static class Utility {
    public static string TestString()
    {
        return "Test";
    }

	public static T[] ShuffleArray<T>(T[] array, int seed) {

		System.Random rand = new System.Random (seed);

		for (int i = 0; i < array.Length - 1; i++) {

			int randomIndex = rand.Next (i, array.Length);
			T temp = array [randomIndex];
			array [randomIndex] = array [i];
			array [i] = temp;
		}

		return array;
	}
}
