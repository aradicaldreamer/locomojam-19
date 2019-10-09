using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tilePrefabs;
    [SerializeField]
    private GameObject[] eventPrefabs;

    public static GameManager Instance;

    //map tile variables
    [SerializeField]
    private TileHolder[] tileGridRaw;
    private TileHolder[][] tileGrid;
    private int xLength = 9, yLength = 5;

    //play variables
    private Point startIndex = new Point(0, 0);
    private Point endIndex = new Point(0, 0);
    [SerializeField] private GameObject endPoint;
    private bool startPlay = false;

    //track drawing variables
    [SerializeField]
    private GameObject trackLine;
    private float minDist = 5;
    [SerializeField]
    private float lineSpeed = 0.5f;
    [SerializeField]
    private Transform trackParent;
    private List<GameObject> path;
    private List<Point> pathCoords;
    private float tileSize = 75f;
    private float tileHalfwayPoint = 37.5f;
    private float drawTime = 0.15f;
    private float drawTimer = 0;

    //player variables
    [SerializeField]
    private GameObject player;
    private Point currentTile = new Point(0, 0);//current tile the end of the path is on
    private Point currentTilePlayer;//current tile the player is on
    [SerializeField]
    private float playerMaxSpeed = 250;
    private float playerSpeed = 0;

    //events variables
    [SerializeField]
    private int maxEventsOnScreen = 6;
    [SerializeField]
    private int minEventsOnScreen = 3;
    [SerializeField]
    private float newEventTime = 10;
    private float newEventTimer;
    [SerializeField]
    private float eventChance = 0.5f;
    private List<Event> events;
    [SerializeField]
    private GameObject eventParent;

    //timer variables
    [SerializeField]
    private TextMeshProUGUI timer;
    private float timerTime;
    [SerializeField]
    private float startingTime = 60;
    private float currentTime;
    private int minutes, seconds;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        } else
        {
            Instance = this;
        }

        path = new List<GameObject>();
        pathCoords = new List<Point>();

        player = GameObject.FindGameObjectWithTag("Player");

        tileGrid = new TileHolder[xLength][];
        for (int i = 0; i < xLength; i++)
        {
            tileGrid[i] = new TileHolder[yLength];
        }

        newEventTimer = Time.time + newEventTime;
        events = new List<Event>();

        int tileGridRawCounter = 0;

        playerSpeed = playerMaxSpeed;

        timerTime = 0;
        currentTime = startingTime;
        SetTimer();
        
        for (int i = 0; i < tileGridRaw.Length; i += yLength)
        {
            for (int j = 0; j < yLength; j++)
            {
                tileGrid[i / yLength][j] = tileGridRaw[tileGridRawCounter];
                tileGridRawCounter++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlay && tileGrid != null)
        {
            startPlay = true;
            StartPlay();
        } else
        {
            Play();
        }

        if (events.Count < maxEventsOnScreen && newEventTimer < Time.time)
        {
            if (Random.value < eventChance)
            {
               SpawnEvent();
                newEventTimer = Time.time + newEventTime;
            }
        }

        SetTimer();
    }

    private void StartPlay()
    {
        startIndex = new Point(Random.Range(0, xLength - 1), Random.Range(0, yLength - 1));

        while (tileGrid[startIndex.x][startIndex.y].getTileType() == TileEnum.Empty)
        {
            startIndex = new Point(Random.Range(0, xLength - 1), Random.Range(0, yLength - 1));
        }

        Vector3 newPosition = CalculateNewPosition(tileGrid[startIndex.x][startIndex.y].gameObject);

        player.transform.localPosition = newPosition;
        currentTile = new Point(startIndex.x, startIndex.y);
        currentTilePlayer = new Point(startIndex.x, startIndex.y);
        ResetStartAndEnd();
    }

    private void Play()
    {
        if (Input.GetButton("Submit") && drawTimer < Time.time)
        {
            drawTimer = Time.time + drawTime;
            GameObject newLine;
            if (Input.GetAxis("Horizontal") > 0 && tileGrid[currentTile.x][currentTile.y].HasDirection(3))
            {
                newLine = Instantiate(trackLine, trackParent);
                newLine.transform.localPosition = CalculateNewPosition(tileGrid[currentTile.x][currentTile.y].gameObject, tileHalfwayPoint);
                currentTile.x++;
                pathCoords.Add(new Point(currentTile.x, currentTile.y));
                path.Add(newLine);
            } else if (Input.GetAxis("Horizontal") < 0 && tileGrid[currentTile.x][currentTile.y].HasDirection(1))
            {
                newLine = Instantiate(trackLine, trackParent);
                newLine.transform.localPosition = CalculateNewPosition(tileGrid[currentTile.x][currentTile.y].gameObject, -tileHalfwayPoint);
                currentTile.x--;
                pathCoords.Add(new Point(currentTile.x, currentTile.y));
                path.Add(newLine);
            } else if (Input.GetAxis("Vertical") > 0 && tileGrid[currentTile.x][currentTile.y].HasDirection(0))
            {
                newLine = Instantiate(trackLine, trackParent);
                newLine.transform.localPosition = CalculateNewPosition(tileGrid[currentTile.x][currentTile.y].gameObject, 0, tileHalfwayPoint);
                newLine.transform.eulerAngles = new Vector3(0, 0, 90);
                currentTile.y--;
                pathCoords.Add(new Point(currentTile.x, currentTile.y));
                path.Add(newLine);
            } else if (Input.GetAxis("Vertical") < 0 && tileGrid[currentTile.x][currentTile.y].HasDirection(2))
            {
                newLine = Instantiate(trackLine, trackParent);
                newLine.transform.localPosition = CalculateNewPosition(tileGrid[currentTile.x][currentTile.y].gameObject, 0, -tileHalfwayPoint);
                newLine.transform.eulerAngles = new Vector3(0, 0, 90);
                currentTile.y++;
                pathCoords.Add(new Point(currentTile.x, currentTile.y));
                path.Add(newLine);
            }
        }

        if (path.Count > 0)
        {
            followPath();
        }
    }

    public GameObject SetTile(TileEnum tileType)
    {
        return tilePrefabs[(int)tileType];
    }

    private Vector3 CalculateNewPosition(GameObject parentPosition, float xMod = 0, float yMod = 0)
    {
        return new Vector3(parentPosition.transform.localPosition.x + xMod, parentPosition.transform.localPosition.y + yMod, 
                                parentPosition.transform.localPosition.z);
    }

    private void CheckIfEnd()
    {
        if (currentTilePlayer.x == endIndex.x && currentTilePlayer.y == endIndex.y)
        {
            ResetStartAndEnd();
        }
    }

    private void ResetStartAndEnd()
    {
        startIndex = new Point(currentTile.x, currentTile.y);
        Vector3 newPosition = CalculateNewPosition(tileGrid[currentTile.x][currentTile.y].gameObject);

        endIndex = new Point(Random.Range(0, xLength), Random.Range(0, yLength));

        while (tileGrid[endIndex.x][endIndex.y].getTileType() == TileEnum.Empty ||
                Mathf.Abs(endIndex.x - startIndex.x) + Mathf.Abs(endIndex.y - startIndex.y) < minDist)
        {
            endIndex = new Point(Random.Range(0, xLength), Random.Range(0, yLength));
        }
        newPosition = CalculateNewPosition(tileGrid[endIndex.x][endIndex.y].gameObject);

        endPoint.transform.localPosition = newPosition;
    }

    private void followPath()
    {
        if (pathCoords[0].x > currentTilePlayer.x)
        {
            if (Mathf.Abs(tileGrid[pathCoords[0].x][pathCoords[0].y].transform.localPosition.x - player.transform.localPosition.x) <= playerSpeed / tileSize)
            {
                player.transform.localPosition = tileGrid[pathCoords[0].x][pathCoords[0].y].transform.localPosition;
                Destroy(path[0]);
                path.RemoveAt(0);
                pathCoords.RemoveAt(0);
                currentTilePlayer.x++;
            } else
            {
                player.transform.localPosition = CalculateNewPosition(player, playerSpeed / tileSize);
            }
        } else if (pathCoords[0].x < currentTilePlayer.x)
        {
            if (Mathf.Abs(tileGrid[pathCoords[0].x][pathCoords[0].y].transform.localPosition.x - player.transform.localPosition.x) <= playerSpeed / tileSize)
            {
                player.transform.localPosition = tileGrid[pathCoords[0].x][pathCoords[0].y].transform.localPosition;
                Destroy(path[0]);
                path.RemoveAt(0);
                pathCoords.RemoveAt(0);
                currentTilePlayer.x--;
            } else
            {
                player.transform.localPosition = CalculateNewPosition(player, -(playerSpeed / tileSize));
            }
        } else if (pathCoords[0].y > currentTilePlayer.y)
        {
            if (Mathf.Abs(tileGrid[pathCoords[0].x][pathCoords[0].y].transform.localPosition.y - player.transform.localPosition.y) <= playerSpeed / tileSize)
            {
                player.transform.localPosition = tileGrid[pathCoords[0].x][pathCoords[0].y].transform.localPosition;
                Destroy(path[0]);
                path.RemoveAt(0);
                pathCoords.RemoveAt(0);
                currentTilePlayer.y++;
            } else
            {
                player.transform.localPosition = CalculateNewPosition(player, 0, -(playerSpeed / tileSize));
            }
        } else if (pathCoords[0].y < currentTilePlayer.y)
        {
            if (Mathf.Abs(tileGrid[pathCoords[0].x][pathCoords[0].y].transform.localPosition.y - player.transform.localPosition.y) <= playerSpeed / tileSize)
            {
                player.transform.localPosition = tileGrid[pathCoords[0].x][pathCoords[0].y].transform.localPosition;
                Destroy(path[0]);
                path.RemoveAt(0);
                pathCoords.RemoveAt(0);
                currentTilePlayer.y--;
            } else
            {
                player.transform.localPosition = CalculateNewPosition(player, 0, playerSpeed / tileSize);
            }
        }

        TriggerEvents();

        CheckIfEnd();
    }

    public float LowerSpeed(float changePercentage)
    {
        float speedToLower = playerMaxSpeed * changePercentage;
        playerSpeed -= speedToLower;
        return speedToLower;
    }

    public void FixSpeed(float speedAddition)
    {
        playerSpeed += speedAddition;
    }

    public void RemoveEvent(Event trafficEvent)
    {
        events.Remove(trafficEvent);
        Destroy(trafficEvent.gameObject);
    }

    public void SpawnEvent()
    {
        Point spawnPoint = new Point(Random.Range(0, xLength), Random.Range(0, yLength));

        while (tileGrid[spawnPoint.x][spawnPoint.y].getTileType() == TileEnum.Empty ||
                (Mathf.Abs(spawnPoint.x - currentTilePlayer.x) + Mathf.Abs(spawnPoint.y - currentTilePlayer.y)) < minDist)
        {
            spawnPoint = new Point(Random.Range(0, xLength - 1), Random.Range(0, yLength - 1));
        }
        Vector3 newPosition = CalculateNewPosition(tileGrid[spawnPoint.x][spawnPoint.y].gameObject);

        GameObject spawnObject = Instantiate(eventPrefabs[0], eventParent.transform);

        spawnObject.transform.localPosition = newPosition;

        spawnObject.GetComponentInChildren<Event>().SetLocation(spawnPoint);

        events.Add(spawnObject.GetComponentInChildren<Event>());
    }

    private void TriggerEvents()
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].GetLocation().Compare(currentTilePlayer))
            {
                events[i].TriggerEvent();
            }
        }
    }

    private void SetTimer()
    {
        if (timerTime < Time.time) {
            timerTime = timerTime + 1;
            minutes = (int)(currentTime / 60);
            seconds = (int)(currentTime % 60);
            timer.text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10? "0" : "") + seconds;
            currentTime--;
        }

        if (currentTime <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {

    }
}
