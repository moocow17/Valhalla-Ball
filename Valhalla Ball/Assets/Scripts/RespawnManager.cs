using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public int numOfPlayers = 8;

    private GameController gameController;

    [SerializeField]
    private float playerRespawnWaitTime;
    [SerializeField]
    private float ballRespawnWaitTime;

    [SerializeField]
    private int whiteTeamNumber = 0;
    [SerializeField]
    private int blackTeamNumber = 1;

    List<PlayerToRespawn> playersToRespawn = new List<PlayerToRespawn>();
    List<BallToRespawn> ballsToRespawn = new List<BallToRespawn>();
    /*List<PlayerSpawnAllocation> whiteSpawnAllocations = new List<PlayerSpawnAllocation>();
    List<PlayerSpawnAllocation> blackSpawnAllocations = new List<PlayerSpawnAllocation>();*/

    public GameObject[] whiteSpawnLocations;
    public GameObject[] blackSpawnLocations;
    public GameObject[] ballSpawnLocations;
    List<GameObject> whiteStartingSpawnLocations = new List<GameObject>();
    List<GameObject> blackStartingSpawnLocations = new List<GameObject>();
    List<GameObject> shuffledWhiteSpawnLocations = new List<GameObject>();
    List<GameObject> shuffledBlackSpawnLocations = new List<GameObject>();
    List<GameObject> players = new List<GameObject>();
    List<Mover> playerMovers = new List<Mover>();

    public GameObject playerPrefab;

    public Sprite blueWhiteSprite;
    public Sprite blueBlackSprite;
    public Sprite orangeWhiteSprite;
    public Sprite orangeBlackSprite;
    public Sprite purpleWhiteSprite;
    public Sprite purpleBlackSprite;
    public Sprite greenWhiteSprite;
    public Sprite greenBlackSprite;
    public Sprite redWhiteSprite;
    public Sprite redBlackSprite;


    // Start is called before the first frame update
    void Awake()
    {
        gameController = GetComponent<GameController>();

        whiteSpawnLocations = GameObject.FindGameObjectsWithTag("WhiteSpawnPoint");
        blackSpawnLocations = GameObject.FindGameObjectsWithTag("BlackSpawnPoint");
        ballSpawnLocations = GameObject.FindGameObjectsWithTag("BallSpawnPoint");
        for (int i = 0; i < whiteSpawnLocations.Length; i++)
        {
            whiteStartingSpawnLocations.Add(whiteSpawnLocations[i]);
        }
        for (int i = 0; i < blackSpawnLocations.Length; i++)
        {
            blackStartingSpawnLocations.Add(blackSpawnLocations[i]);
        }

        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        SpriteRenderer spriteRenderer = new SpriteRenderer();
        //loop for the number of players            
        for (int i = 0; i < numOfPlayers; i++)
        {

            //create player objects
            players.Add(Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity));
            playerMovers.Add(players[i].GetComponent<Mover>());

            //set their playerIndex number
            playerMovers[i].playerIndex = i;

            //set their teamIndex number
            playerMovers[i].playerTeam = i%2;

            //set their sprite
            switch (i)
            {
                case 0:
                    players[i].GetComponent<SpriteRenderer>().sprite = blueWhiteSprite;
                    break;
                case 1:
                    players[i].GetComponent<SpriteRenderer>().sprite = blueBlackSprite;
                    break;
                case 2:
                    players[i].GetComponent<SpriteRenderer>().sprite = orangeWhiteSprite;
                    break;
                case 3:
                    players[i].GetComponent<SpriteRenderer>().sprite = orangeBlackSprite;
                    break;
                case 4:
                    players[i].GetComponent<SpriteRenderer>().sprite = purpleWhiteSprite;
                    break;
                case 5:
                    players[i].GetComponent<SpriteRenderer>().sprite = purpleBlackSprite;
                    break;
                case 6:
                    players[i].GetComponent<SpriteRenderer>().sprite = greenWhiteSprite;
                    break;
                case 7:
                    players[i].GetComponent<SpriteRenderer>().sprite = greenBlackSprite;
                    break;
                default:
                    players[i].GetComponent<SpriteRenderer>().sprite = redBlackSprite;
                    break;
            }

            //add player to the playersToRespawn list
            PlayerToRespawn newPlayer = new PlayerToRespawn();
            newPlayer.player = players[i];
            newPlayer.respawnTime = 0;
            playersToRespawn.Add(newPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //respawn players
        RespawnPlayers();

        //respawn balls
        RespawnBalls();
    }

    void RespawnPlayers()
    {
        //get the black and white spawn locations and randomise them
        
        shuffledWhiteSpawnLocations.AddRange(whiteStartingSpawnLocations);
        shuffledBlackSpawnLocations.AddRange(blackStartingSpawnLocations);

        shuffledWhiteSpawnLocations.Shuffle();
        shuffledBlackSpawnLocations.Shuffle();
        

        //loop through players, (loops backwards so i can remove easily without throwing exceptions)
        for (int i = playersToRespawn.Count - 1; i >= 0; i--)
        {
            
            //Debug.Log("playersToRespawn[0]: " + playersToRespawn[0].player.GetComponent<Mover>().playerIndex.ToString());
            //check if their respawn time is done, and if so, set them to active and make their transform coordinates the transform coordinates of a spawn point
            //figure out a spawn point to allocate the player to //if want to improve this: (based on which team they are from, whether any players are allocated to a spot already and then which has least people around and then just the last one)
            //remove them from the list
            if (Time.time >= playersToRespawn[i].respawnTime)
            {
                if (playersToRespawn[i].player.GetComponent<Mover>().playerTeam == whiteTeamNumber) //if the player was on white team
                {
                    playersToRespawn[i].player.transform.position = shuffledWhiteSpawnLocations[shuffledWhiteSpawnLocations.Count - 1].transform.position;
                    playersToRespawn[i].player.transform.rotation = Quaternion.Euler(0, 0, 0);
                    if (shuffledWhiteSpawnLocations.Count > 1)
                    {
                        shuffledWhiteSpawnLocations.Remove(shuffledWhiteSpawnLocations[shuffledWhiteSpawnLocations.Count-1]);
                    }
                    /*//figure out which white spawn point to allocate to
                    int spawn = Random.Range(0, whiteSpawnLocations.Length);

                    playersToRespawn[i].player.transform.position = whiteSpawnLocations[spawn].transform.position;*/
                }
                else if (playersToRespawn[i].player.GetComponent<Mover>().playerTeam == blackTeamNumber)  //if the player was on black team
                {
                    playersToRespawn[i].player.transform.position = shuffledBlackSpawnLocations[shuffledBlackSpawnLocations.Count - 1].transform.position;
                    playersToRespawn[i].player.transform.rotation = Quaternion.Euler(0, 0, 180);
                    if (shuffledBlackSpawnLocations.Count > 1)
                    {
                        shuffledBlackSpawnLocations.Remove(shuffledBlackSpawnLocations[shuffledBlackSpawnLocations.Count-1]);
                    }

                    /*//figure out which black spawn point to allocate to
                    int spawn = Random.Range(0, blackSpawnLocations.Length);

                    playersToRespawn[i].player.transform.position = blackSpawnLocations[spawn].transform.position;*/
                }
                playersToRespawn[i].player.gameObject.SetActive(true);
                playersToRespawn.Remove(playersToRespawn[i]);
            }
        }

        shuffledWhiteSpawnLocations.RemoveRange(0, shuffledWhiteSpawnLocations.Count);
        shuffledBlackSpawnLocations.RemoveRange(0, shuffledBlackSpawnLocations.Count);
    }

    void RespawnBalls()
    {
        //loop through balls, (loops backwards so i can remove easily without throwing exceptions)
        for (int i = ballsToRespawn.Count - 1; i >= 0; i--)
        {
            //check if their respawn time is done, and if so, set them to active and make their transform coordinates the transform coordinates of a spawn point
            //figure out a spawn point to allocate the player to //if want to improve this: (based on which team they are from, whether any players are allocated to a spot already and then which has least people around and then just the last one)
            //remove them from the list
            if (Time.time > ballsToRespawn[i].respawnTime)
            {
                //figure out which spawn point to allocate to
                int spawn = Random.Range(0, ballSpawnLocations.Length);

                ballsToRespawn[i].ball.transform.position = ballSpawnLocations[spawn].transform.position;

                Debug.Log("BALL RESPAWN NOW");
                ballsToRespawn[i].ball.gameObject.SetActive(true);
                ballsToRespawn.Remove(ballsToRespawn[i]);
            }
        }
    }

    public void PrepPlayerRespawn(GameObject player)
    {
        //add player to the playersToRespawn list
        if(gameController.gamePlaying)
        {
            PlayerToRespawn newlyDeceased = new PlayerToRespawn();
            newlyDeceased.player = player;
            newlyDeceased.respawnTime = Time.time + playerRespawnWaitTime;
            playersToRespawn.Add(newlyDeceased);
        }
    }

    public void PrepBallRespawn(GameObject ball)
    {
        if (gameController.gamePlaying)
        {
            //add ball to the ballsToRespawn list
            BallToRespawn scoredBall = new BallToRespawn();
            scoredBall.ball = ball;
            scoredBall.respawnTime = Time.time + ballRespawnWaitTime;
            ballsToRespawn.Add(scoredBall);
        }
    }

    public void DestroyAllPlayers()
    {
        //loop for the number of players            
        for (int i = 0; i < numOfPlayers; i++)
        {
            playerMovers[i].KillPlayer(players[i]);
        }
    }
}

public class PlayerToRespawn
{
    public GameObject player { get; set; }
    public float respawnTime { get; set; }
}

public class BallToRespawn
{
    public GameObject ball { get; set; }
    public float respawnTime { get; set; }
}

public class PlayerSpawnAllocation
{
    public GameObject spawnPoint { get; set; }
    public PlayerToRespawn playerToRespawn { get; set; }
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> specifiedList)
    {
        var count = specifiedList.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = specifiedList[i];
            specifiedList[i] = specifiedList[r];
            specifiedList[r] = tmp;
        }
    }
}
