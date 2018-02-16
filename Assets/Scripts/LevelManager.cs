using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public GameObject ToInstantiate;
    public float MinXValue = 0;
    public float MaxXValue = 0;
    public float MinYValue = 0;
    public float MaxYValue = 0;
    public float Timer = 5;
    private float timeLeft = 0;

    private void Start()
    {
        Fireball EnemyFireball = ToInstantiate.GetComponent<Fireball>();
        if (EnemyFireball != null)
        {
            EnemyFireball.DestroyOnGroundHit = true;
            EnemyFireball.GenerateItemsOnHit = true;
        }
       
        timeLeft = Timer;
    }

    void Update () {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Instantiate(ToInstantiate, new Vector3(Random.Range(MinXValue, MaxXValue),
                Random.Range(MinYValue, MaxYValue)), Quaternion.identity);
            timeLeft = Timer;
        }
    }
}
