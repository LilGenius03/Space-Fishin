using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public GameObject fishPrefab;
    public static int worldSize = 30;

    static int numFish = 10;
    public GameObject[] allFish = new GameObject[numFish];

    public Vector3 goalPos = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < numFish; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize));
            allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0, 10000) < 50)
        {
            goalPos = new Vector3(Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize));
        }
    }
}
