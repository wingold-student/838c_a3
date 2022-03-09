using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int WINNING_SCORE = 10;
    public float sphereRadius = 2.0f;
    public float maxSphereRadius = 50.0f;
    int layerMask = 0; // Collectibles
    int totalScore = 0;
    int totalCollected = 0;
    int totalUnique = 0;
    bool bronzeCollected = false, silverCollected = false, goldCollected = false;
    Transform mainCameraTrans = null;
    Transform winTxt = null;
    TextMeshPro totalScoreTxt = null;
    TextMeshPro totalCollectedTxt = null;
    TextMeshPro totalUniqueTxt = null;
    // Start is called before the first frame update
    void Start()
    {
        this.mainCameraTrans = this.gameObject.transform.Find("Main Camera");

        this.totalScoreTxt = mainCameraTrans.Find("totalScore").gameObject.GetComponent<TextMeshPro>();
        this.totalCollectedTxt = mainCameraTrans.Find("totalCollected").gameObject.GetComponent<TextMeshPro>();
        this.totalUniqueTxt = mainCameraTrans.Find("totalUnique").gameObject.GetComponent<TextMeshPro>();
        this.winTxt = mainCameraTrans.Find("winText");

        this.layerMask = LayerMask.GetMask("Collectibles");
    }

    void checkForWin() {
        if (this.totalScore >= WINNING_SCORE) {
            this.winTxt.gameObject.SetActive(true);
        }
    }

    void updateText() {
        string scoreStr = "Total Score: " + this.totalScore.ToString();
        string collectedStr = "Objects Collected: " + this.totalCollected.ToString();
        string uniqueStr = "Unique Collected: " + this.totalUnique.ToString();

        this.totalScoreTxt.text = scoreStr;
        this.totalCollectedTxt.text = collectedStr;
        this.totalUniqueTxt.text = uniqueStr;
    }

    void pickupCollectible(Collectible collectible) {
        int collectiblePoints = collectible.getWorth();
        Collectible.CollectibleType collectibleType = collectible.getType();

        this.totalScore += collectiblePoints;
        this.totalCollected++;

        print(
                "Collected " +
                collectible.getType().ToString() +
                " with worth: " +
                collectiblePoints.ToString()
            );

        if (!bronzeCollected && collectibleType == Collectible.CollectibleType.BRONZE) {
            bronzeCollected = true;
            this.totalUnique++;
        } else if (!silverCollected && collectibleType == Collectible.CollectibleType.SILVER) {
            silverCollected = true;
            this.totalUnique++;
        } else if (!goldCollected && collectibleType == Collectible.CollectibleType.GOLD) {
            goldCollected = true;
            this.totalUnique++;
        }
    }

    bool handleCollision(GameObject collidedObject) {
        Collectible collectibleObj = collidedObject.GetComponent<Collectible>();

        if (collectibleObj == null)
            return false;

        pickupCollectible(collectibleObj);

        Destroy(collidedObject);
        return true;
    }

    void doLineRaycast() {
        Debug.DrawLine(this.mainCameraTrans.position,
                        this.mainCameraTrans.position + (100.0f * this.mainCameraTrans.forward),
                        Color.red,
                        100.0f);
        RaycastHit result;

        if (Physics.Raycast(this.mainCameraTrans.position,
                            this.mainCameraTrans.transform.forward,
                            out result,
                            Mathf.Infinity)) {

            GameObject collidedObject = result.collider.gameObject;
            print("My ray hit: " + collidedObject.name);

            handleCollision(collidedObject);
        }
    }

    double getDistance(Transform other) {
        float myXSquard = mainCameraTrans.position.x * mainCameraTrans.position.x;
        float myYSquard = mainCameraTrans.position.y * mainCameraTrans.position.y;
        float myZSquard = mainCameraTrans.position.z * mainCameraTrans.position.z;

        float otherXSquard = other.position.x * other.position.x;
        float otherYSquared = other.position.y * other.position.y;
        float otherZSquard = other.position.z * other.position.z;

        float xDiff = myXSquard - otherXSquard;
        float yDiff = myYSquard - otherYSquared;
        float zDiff = myZSquard - otherZSquard;

        return Math.Sqrt(xDiff + yDiff + zDiff);
    }

    void doSpherecast() {
        Collider[] collided = new UnityEngine.Collider[0];
        bool hitCollectible = false;
        float curSphereRadius = this.sphereRadius;
        double minDistance = -1;

        while (hitCollectible == false && curSphereRadius < this.maxSphereRadius) {
            collided = Physics.OverlapSphere(this.mainCameraTrans.position,
                                                        curSphereRadius,
                                                        this.layerMask,
                                                        QueryTriggerInteraction.Collide);
            GameObject closest = null;
            GameObject tmpObj = null;

            foreach(Collider collidedObj in collided) {
                print(collidedObj.gameObject.name);
                hitCollectible = true;
                tmpObj = collidedObj.gameObject;

                double dist = getDistance(tmpObj.transform);
                if (minDistance == -1 || (minDistance != -1 && minDistance > dist)) {
                    minDistance = dist;
                    closest = tmpObj;
                }
            }

            if (hitCollectible && closest != null) {
                print("Closest was: " + closest.name);
                handleCollision(closest);
            }
            
            curSphereRadius += 1.0f;
        }
    }

    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) {
            print("Line raycast fired!");
            doLineRaycast();
        } else if (Input.GetKeyDown(KeyCode.U)) {
            print("Sphere cast fired!");
            doSpherecast();
        }

        updateText();
    }
    
    void OnTriggerEnter(Collider other) {
        GameObject collidedObject = other.gameObject;
        print("I hit " + collidedObject.name);
        handleCollision(collidedObject);
    }
}
