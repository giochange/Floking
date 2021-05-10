using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject fishPrefab;//diretiva do objeto peixe
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;
    [Header("Configurações do Cardume")] //menu de gerenciamento do peixe

    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;//variavel de rotação


    void Start()
    {
        allFish = new GameObject[numFish];//criação dos peixe perante as setagem
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
              Random.Range(-swinLimits.y, swinLimits.y),
              Random.Range(-swinLimits.z, swinLimits.z));

            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity); //instancia o prefab
            allFish[i].GetComponent<Flock>().myManager = this;// pegada do metodo dentro do flock
        }
        goalPos = this.transform.position;
    }
    void Update()
    {
        if(Random.Range(0,100)<10)
        goalPos= this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
              Random.Range(-swinLimits.y, swinLimits.y),
              Random.Range(-swinLimits.z, swinLimits.z));
    }


}
