using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Enemy))]
public class MapGenerator : MonoBehaviour {

	public Transform tilePrefab;
	public Transform obstaclePrefab;
	public Map [] maps;
	public int currentMapIndex;
	List<Coord> allTileCoords;
	Queue<Coord> shuffledQueue;

	List<Coord> obstacles;
	bool [,] obstacleMap;

	Map currentMap;

    public Enemy enemy;
    public Player player;
    public Key key;
    public Platform platform;

    void Start () {
		GenerateMap ();
//		currentMap.spawnCoord = new Coord ((int)currentMap.spawnPoint.x, (int)currentMap.spawnPoint.y);
	}

	public void GenerateMap() {

        var destPlayer = GameObject.FindGameObjectWithTag("Player");
        if(destPlayer != null)
        {
            DestroyImmediate(destPlayer);
        }
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        print(enemies.Length);
        for (var i = 0; i < enemies.Length; i++)
        {
            DestroyImmediate(enemies[i]);
        }

            currentMap = maps [currentMapIndex];
		allTileCoords = new List<Coord> ();
		for (int x = 0; x < currentMap.mapSize.x; x++) {
			for (int y = 0; y < currentMap.mapSize.y; y++) {
				allTileCoords.Add(new Coord(x, y));
			}
		}
		shuffledQueue = new Queue<Coord> (Utility.ShuffleArray (allTileCoords.ToArray (), currentMap.seed));

		string name = "map";
		if (transform.FindChild (name)) {
			DestroyImmediate(transform.FindChild(name).gameObject);
		}



		Transform mapHolder = new GameObject (name).transform;
		mapHolder.parent = transform;

		for (int x = 0; x < currentMap.mapSize.x; x++) {
			for (int y = 0; y < currentMap.mapSize.y; y++) {
				Vector3 tilePosition = CoordToPosition (x, y);
				Transform newTile = Instantiate (tilePrefab, tilePosition, Quaternion.Euler (Vector3.right * 90));
				newTile.localScale = Vector3.one * (1 - currentMap.outlinePercent);
				newTile.parent = mapHolder;
			}
		}


		GenerateObstacles ();

        //Spawn Enemies
        int numEnemies = currentMap.Enemies;
        System.Random r = new System.Random();
        while (numEnemies > 0)
        {
            Coord randomCoord = new Coord(r.Next(0, currentMap.mapSize.x), r.Next(0, currentMap.mapSize.y));
            if(Utility.DistanceBetweenCoords(currentMap.spawnCoord.x, currentMap.spawnCoord.y, randomCoord.x, randomCoord.y, currentMap.minDistanceToPlayer))
            {
                print("Distance Ok");
                if (!obstacles.Contains(randomCoord))
                {
                    //Spawn Enemy
                    print("Spawn " + randomCoord.x + " " + randomCoord.y);

                    Enemy spawnedEnemy = Instantiate(enemy, CoordToPosition(randomCoord.x, randomCoord.y, 1), Quaternion.identity) as Enemy;
                    numEnemies--;
                }
            }
        }
        Player spawnedPlayer = Instantiate(player, CoordToPosition(currentMap.spawnCoord.x, currentMap.spawnCoord.y, 1), Quaternion.identity) as Player;

        // Spawn Key
        while (true)
        {
            Coord randomCoord = new Coord(r.Next(0, currentMap.mapSize.x), r.Next(0, currentMap.mapSize.y));
            if (Utility.DistanceBetweenCoords(currentMap.spawnCoord.x, currentMap.spawnCoord.y, randomCoord.x, randomCoord.y, currentMap.minDistanceToPlayer))
            {
                if (!obstacles.Contains(randomCoord))
                {
                    Instantiate(key, CoordToPosition(randomCoord.x, randomCoord.y, 1), Quaternion.identity);
                    break;
                }
            }
        }

        // Spawn Platform
        //while (true)
        //{
        //    Coord randomCoord = new Coord(r.Next(0, currentMap.mapSize.x), r.Next(0, currentMap.mapSize.y));
        //    if (Utility.DistanceBetweenCoords(currentMap.spawnCoord.x, currentMap.spawnCoord.y, randomCoord.x, randomCoord.y, currentMap.minDistanceToPlayer))
        //    {
        //        if (!obstacles.Contains(randomCoord))
        //        {
        //            Instantiate(platform, CoordToPosition(randomCoord.x, randomCoord.y, 1), Quaternion.identity);
        //            break;
        //        }
        //    }
        //}
    }

    void GenerateObstacles() {

		obstacles = new List<Coord> ();
		obstacleMap = new bool[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];
		string name = "obstacles";
		if (transform.FindChild (name)) {
			DestroyImmediate(transform.FindChild(name).gameObject);
		}

		Transform obstacleHolder = new GameObject (name).transform;
		obstacleHolder.parent = transform;

		System.Random rand = new System.Random (4);

		bool [,] containsObstacle = new bool[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];
		int currentObstacleCount = 0;
		for (int i = 0; i < currentMap.maxObstacleCount; i++) {
			Coord coord = GetRandomCoord ();
			Vector3 vec3 = CoordToPosition (coord.x, coord.y);
			containsObstacle [coord.x, coord.y] = true;
			currentObstacleCount++;

			if (coord.x != currentMap.spawnCoord.x && coord.y != currentMap.spawnCoord.y 
				&& MapIsFullyAccessible(containsObstacle, currentObstacleCount)) {

				float height = (float)rand.NextDouble () + currentMap.minObstacleHeight;
				height = Mathf.Min (height, currentMap.maxObstacleHeight);
				Transform obstacle = Instantiate (obstaclePrefab, vec3 + (Vector3.up * height/2), Quaternion.identity) as Transform;
				obstacle.parent = obstacleHolder;

				obstacle.localScale = new Vector3 (((1 - currentMap.outlinePercent) * currentMap.tileSize), height, ((1 - currentMap.outlinePercent) * currentMap.tileSize));
				obstacles.Add (coord);
				this.obstacleMap [coord.x, coord.y] = true;
			} else {
				containsObstacle [coord.x, coord.y] = false;
				currentObstacleCount--;
			}



		}
	}

	Vector3 CoordToPosition(int x, int y, int yOffset = 0) {
		Vector3 tilePosition = new Vector3 (-currentMap.mapSize.x/2 + 0.5f + x, yOffset, -currentMap.mapSize.y/2 + 0.5f + y);
		return tilePosition;
	}
		

	public Coord GetRandomCoord() {

		Coord coord = shuffledQueue.Dequeue ();
		shuffledQueue.Enqueue (coord);

		return coord;
	}

	bool MapIsFullyAccessible( bool[,]obstacleMap, int currentObstacleCount) {
		bool [,] visited = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)]; 
		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (currentMap.spawnCoord);

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

		return visitedTileCount == ((currentMap.mapSize.x * currentMap.mapSize.y) - currentObstacleCount);
	}

	[System.Serializable]
	public struct Coord {

		public int x;
		public int y;

		public Coord(int _x, int _y) {
			x = _x;
			y = _y;
		}
	}

	[System.Serializable]
	public class Map {

		public Coord mapSize = new Coord(10,10);
		public float maxObstacleCount = 60;
		public int seed = 10;
		public float minObstacleHeight;
		public float maxObstacleHeight;
		public int tileSize = 1;
		[Range (0,1)]
		public float outlinePercent = 0.05f;
		public Coord spawnCoord;
        public int minDistanceToPlayer = 10;
        public int Enemies = 4;

		public Map() {

		}

	}
}
