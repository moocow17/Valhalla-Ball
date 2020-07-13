using MilkShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int goalTeam;

    [HideInInspector]
    public ScoreManager scoreManager;
    private RespawnManager respawnManager;

    public GameObject goalScorePrefab;
    GameObject goalScoreParticles;
    public ShakePreset explosionShakePreset;
    public ShakePreset bigExplosionShakePreset;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        scoreManager = gameControllerObject.GetComponent<ScoreManager>();
        respawnManager = gameControllerObject.GetComponent<RespawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }    

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckIfScore(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckIfScore(collision);
    }

    public void CheckIfScore(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Mover playerMover = (Mover)collision.gameObject.GetComponent(typeof(Mover));

            
            if (playerMover.hasBall && playerMover.playerTeam != goalTeam)
            {
                Score(playerMover);
                GameObject playerGameObject = playerMover.gameObject;
                playerMover.KillPlayer(playerGameObject);
            }
        }
    }

    public void Score(Mover scorer)
    {
        Collider2D ballCollider = Helper.FindComponentInChildWithTag<Collider2D>(scorer.gameObject, "Ball");
        GameObject ballObject = ballCollider.gameObject;
        Transform ballObjectTransform = ballObject.transform;

        //SOUND EFFECT
        AudioManager.instance.Play("ImplosionExplosion", 1f, 1f, false);

        //PARTICLE EFFECT
        goalScoreParticles = Instantiate(goalScorePrefab, transform.position, transform.rotation) as GameObject;

        //SHAKE EFFECT
        StartCoroutine(GoalShake());

        scorer.hasBall = false;

        ballCollider.attachedRigidbody.isKinematic = false;
        ballCollider.enabled = true;
        ballObjectTransform.parent = null;
        ballObject.SetActive(false);
        respawnManager.PrepBallRespawn(ballObject);

        scoreManager.UpdateScore(this);
    }

    IEnumerator GoalShake()
    {
        Shaker.ShakeAll(explosionShakePreset);

        yield return new WaitForSeconds(2.4f);

        Shaker.ShakeAll(bigExplosionShakePreset);

    }
}
