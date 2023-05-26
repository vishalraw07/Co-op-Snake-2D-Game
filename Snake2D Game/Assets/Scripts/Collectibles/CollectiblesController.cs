using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class CollectiblesController : MonoBehaviour
{
    [SerializeField] GameObject foodPrefab;
    [SerializeField] GameObject powerUpPrefab;
    [SerializeField] Transform LeftBorder;
    [SerializeField] Transform RightBorder;
    [SerializeField] Transform TopBorder;
    [SerializeField] Transform BottomBorder;
    [SerializeField] Transform FoodSection;
    [SerializeField] Transform PowerUpSection;
    float minX, minY, maxX, maxY;
    float newX, newY;
    float newX2, newY2;
    bool foodFlag, powerFlag;
    int timeStep;
    void Awake()
    {
        minX = LeftBorder.position.x + 0.5f;
        maxX = RightBorder.position.x - 0.5f;
        minY = BottomBorder.position.y + 0.5f;
        maxY = TopBorder.position.y - 0.5f;
    }
    public void SpawnFood(List<Transform> playerbodies)
    {
        foodFlag = false;
        do
        {
            foodFlag = false;
            newX = Random.Range(minX, maxX);
            newY = Random.Range(minY, maxY);

            newX = Mathf.Round(newX * 2) / 2;
            newY = Mathf.Round(newY * 2) / 2;

            foreach (var item in playerbodies)
            {
                if (item.position.x == newX || item.position.y == newY)
                {
                    foodFlag = true;
                    break;
                }
            }
        } while (foodFlag);
        Vector3 newPos = new Vector3(newX, newY, 0f);
        GameObject newFood = Instantiate(foodPrefab, newPos, Quaternion.identity);
        newFood.transform.parent = FoodSection;
        StartCoroutine(DestroySpawnedFood(newFood));
    }
    IEnumerator DestroySpawnedFood(GameObject food)
    {
        yield return new WaitForSeconds(Random.Range(5, 11));
        if (food != null)
            Destroy(food);
    }
    public void SpawnPowerUp(List<Transform> playerbodies)
    {
        playerbodies.AddRange(GetFoodTransforms());
        powerFlag = false;
        do
        {
            powerFlag = false;
            newX2 = Random.Range(minX, maxX);
            newY2 = Random.Range(minY, maxY);

            newX2 = Mathf.Round(newX2 * 2) / 2;
            newY2 = Mathf.Round(newY2 * 2) / 2;

            foreach (var item in playerbodies)
            {
                if (item.position.x == newX2 || item.position.y == newY2)
                {
                    powerFlag = true;
                    break;
                }
            }
        } while (powerFlag);
        Vector3 newPos = new Vector3(newX2, newY2, 0f);
        GameObject newPower = Instantiate(powerUpPrefab, newPos, Quaternion.identity);
        newPower.transform.parent = PowerUpSection;
    }
    List<Transform> GetFoodTransforms()
    {
        List<Transform> foodTransforms = new List<Transform>();
        foodTransforms.AddRange(transform.GetComponentsInChildren<Transform>());
        return foodTransforms;
    }
}
