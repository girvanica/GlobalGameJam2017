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
	public Vector2 spawnPoint = new Vector2(1, 1);
	Coord spawnCoord;
	List<Coord> obstacles;
	void Start () {
		GenerateMap ();
		spawnCoord = new Coord ((int)spawnPoint.x, (int)spawnPoint.y);
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

		obstacles = new List<Coord> ();

		string name = "obstacles";
		if (transform.FindChild (name)) {
			DestroyImmediate(transform.FindChild(name).gameObject);
		}

		Transform obstacleHolder = new GameObject (name).transform;
		obstacleHolder.parent = transform;

		bool [,] containsObstacle = new bool[(int)mapSize.x, (int)mapSize.y];
		int currentObstacleCount = 0;
		for (int i = 0; i < obstacleCount; i++) {
			Coord coord = GetRandomCoord ();
			Vector3 vec3 = CoordToPosition (coord.x, coord.y);
			containsObstacle [coord.x, coord.y] = true;
			currentObstacleCount++;

			if (coord.x != spawnPoint.x && coord.y != spawnPoint.y 
				&& MapIsFullyAccessible(containsObstacle, currentObstacleCount)) {
				Transform obstacle = Instantiate (obstaclePrefab, vec3 + (Vector3.up * 0.5f), Quaternion.identity) as Transform;
				obstacle.parent = obstacleHolder;
				obstacles.Add (coord);

			} else {
				containsObstacle [coord.x, coord.y] = false;
				currentObstacleCount--;
			}



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

	bool MapIsFullyAccessible( bool[,]obstacleMap, int obstacleCount) {
		bool [,] visited = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)]; 
		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (spawnCoord);

		int visitedTileCount = 0;

		while (queue.Count > 0) {
			Coord current = queue.Dequeue ();
//			Debug.Log ("checking " + current.x + "," + current.y);
			for (int x = -1; x <= 1; x++) {
				for (int y = -1; y <= 1; y++) {
					int neighbourX = current.x + x;
					int neighbourY = current.y + y;

					if (x == 0 || y == 0) {
						if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength (0)
						    && neighbourY >= 0 && neighbourY < obstacleMap.GetLength (1)) {

							if (!visited[neighbourX, neighbourY] &&  !obstacleMap[neighbourX, neighbourY]) {
								visited[neighbourX, neighbourY] = true;
//								Debug.Log ("visited " + neighbourX + "," + neighbourY);
								queue.Enqueue(new Coord(neighbourX, neighbourY));
								visitedTileCount++;
							}

						}
					}
				}
			}
		}

		return visitedTileCount == ((mapSize.x * mapSize.y) - obstacleCount);
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
