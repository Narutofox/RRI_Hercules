using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

    public float NumberOfSecondsLasting;
    public float NumberOfSecondsRemaining;
    public PlayerAttack PlayerAttackScript;
    // Use this for initialization
    void Start () {
        PlayerAttackScript = GameObject.Find(gameObject.name).GetComponent<PlayerAttack>();
    }
	
	// Update is called once per frame
	void Update () {
        NumberOfSecondsLasting += Time.deltaTime;
        if (NumberOfSecondsLasting >= NumberOfSecondsRemaining)
        {
            PlayerAttackScript.StopLightningAttack();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }

}
