using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public Transform tilePrefab;
	public Transform obstaclePrefab;

	public Vector2 mapSize;
	public int tileSize;
	List<Coord> allTileCoords;
	Queue<Coord> shuffledQueue;
	[Range (0,1)]
	public float outlinePercent;
	public int obstacleCount;
	public int seed = 10;

	void Start () {
		GenerateMap ();

	}

	public void GenerateMap() {

		allTileCoords = new List<Coord> ();
		for (int x = 0; x < mapSize.x; x++) {
			for (int y = 0; y < mapSize.y; y++) {
				allTileCoords.Add(new Coord(x, y));
			}
		}
		shuffledQueue = new Queue<Coord> (Utility.ShuffleArray (allTileCoords.ToArray (), seed));


		string name = "map";
		if (transform.FindChild (name)) {
			DestroyImmediate(transform.FindChild(name).gameObject);
		}



		Transform mapHolder = new GameObject (name).transform;
		mapHolder.parent = transform;

		for (int x = 0; x < mapSize.x; x++) {
			for (int y = 0; y < mapSize.y; y++) {
				Vector3 tilePosition = CoordToPosition (x, y);
				Transform newTile = Instantiate (tilePrefab, tilePosition, Quaternion.Euler (Vector3.right * 90));
				newTile.localScale = Vector3.one * (1 - outlinePercent);
				newTile.parent = mapHolder;
			}
		}


		GenerateObstacles ();
	}

	void GenerateObstacles() {

		string name = "obstacles";
		if (transform.FindChild (name)) {
			DestroyImmediate(transform.FindChild(name).gameObject);
		}

		Transform obstacleHolder = new GameObject (name).transform;
		obstacleHolder.parent = transform;

		for (int i = 0; i < obstacleCount; i++) {
			Coord coord = GetRandomCoord ();
			Vector3 vec3 = CoordToPosition (coord.x, coord.y);

			Transform obstacle = Instantiate (obstaclePrefab, vec3 + (Vector3.up * 0.5f), Quaternion.identity) as Transform;
			obstacle.parent = obstacleHolder;
		}
	}

	Vector3 CoordToPosition(int x, int y) {
		Vector3 tilePosition = new Vector3 (-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
		return tilePosition;
	}
		

	public Coord GetRandomCoord() {

		Coord coord = shuffledQueue.Dequeue ();
		shuffledQueue.Enqueue (coord);

		return coord;
	}

	public struct Coord {

		public int x;
		public int y;

		public Coord(int _x, int _y) {
			x = _x;
			y = _y;
		}
	}
}
