using System;
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

    public static bool DistanceBetweenCoords(int x1, int y1, int x2, int y2, int distance)
    {
        return (Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)) < (distance * distance);
    }

	public static float Approach (float fromValue, float toValue, float dt) {

		//get how much left
		float diff = toValue - fromValue;

		//we haven't reached it
		if (diff > dt) {
			return fromValue + dt;
		} 

		if (diff < -dt) {
			return fromValue - dt;
		}

		return toValue;

	}
}
