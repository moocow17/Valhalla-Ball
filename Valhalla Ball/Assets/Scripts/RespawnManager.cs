using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField]
    private float respawnWaitTime;

    [SerializeField]
    private int whiteTeamNumber = 0;
    [SerializeField]
    private int blackTeamNumber = 1;

    List<PlayerToRespawn> playersToRespawn = new List<PlayerToRespawn>();
    List<BallToRespawn> ballsToRespawn = new List<BallToRespawn>();
    List<PlayerSpawnAllocation> whiteSpawnAllocations = new List<PlayerSpawnAllocation>();
    List<PlayerSpawnAllocation> blackSpawnAllocations = new List<PlayerSpawnAllocation>();

    public GameObject[] whiteSpawnLocations;
    public GameObject[] blackSpawnLocations;

    

    // Start is called before the first frame update
    void Awake()
    {
        
        whiteSpawnLocations = GameObject.FindGameObjectsWithTag("WhiteSpawnPoint");
        blackSpawnLocations = GameObject.FindGameObjectsWithTag("BlackSpawnPoint");
        /*for (int i = 0; i < whiteSpawnLocations.Length; i++)
        {
            PlayerSpawnAllocation playerSpawnAllocation = new PlayerSpawnAllocation();
            playerSpawnAllocation.spawnPoint = whiteSpawnLocations[i];
            whiteSpawnAllocations.Add(playerSpawnAllocation);
            whiteSpawnAllocations.
        }
        for (int i = 0; i < blackSpawnLocations.Length; i++)
        {
            PlayerSpawnAllocation playerSpawnAllocation = new PlayerSpawnAllocation();
            playerSpawnAllocation.spawnPoint = blackSpawnLocations[i];
            blackSpawnAllocations.Add(playerSpawnAllocation);
        }*/
        //set player indexes?
        //set ball indexes?
        //set scores?
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
        //loop through players, (loops backwards so i can remove easily without throwing exceptions)
        for (int i = playersToRespawn.Count - 1; i >= 0; i--)
        {
            //check if their respawn time is done, and if so, set them to active and make their transform coordinates the transform coordinates of a spawn point
            //figure out a spawn point to allocate the player to //if want to improve this: (based on which team they are from, whether any players are allocated to a spot already and then which has least people around and then just the last one)
            //remove them from the list
            if(Time.time > playersToRespawn[i].respawnTime)
            {
                if(playersToRespawn[i].player.GetComponent<Mover>().playerTeam == whiteTeamNumber) //if the player was on white team
                {
                    //figure out which white spawn point to allocate to
                    int spawn = Random.Range(0, whiteSpawnLocations.Length);

                    playersToRespawn[i].player.transform.position = whiteSpawnLocations[spawn].transform.position;
                }
                else if (playersToRespawn[i].player.GetComponent<Mover>().playerTeam == blackTeamNumber)  //if the player was on black team
                {
                    int spawn = Random.Range(0, whiteSpawnLocations.Length);

                    playersToRespawn[i].player.transform.position = blackSpawnLocations[spawn].transform.position;
                }
                playersToRespawn[i].player.gameObject.SetActive(true);
                playersToRespawn.Remove(playersToRespawn[i]);
            }
        }
    }

    void RespawnBalls()
    {

    }

    public void PrepPlayerRespawn(GameObject player)
    {
        //add player to the playersToRespawn list
        PlayerToRespawn newlyDeceased = new PlayerToRespawn();
        newlyDeceased.player = player;
        newlyDeceased.respawnTime = Time.time + respawnWaitTime;
        playersToRespawn.Add(newlyDeceased);
        
    }

    public void PrepBallRespawn(GameObject ball, float timeOfScore)
    {

    }
}

public class PlayerToRespawn
{
    public GameObject player { get; set; }
    public float respawnTime { get; set; }
    public Vector2 respawnLocation { get; set; }
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
