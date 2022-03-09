using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType
    {
        BRONZE,
        SILVER,
        GOLD
    }
    int points;
    string mat;
    CollectibleType type;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material.name;
        print(mat);
        if(mat == "Silver (Instance)"){
            this.points = 2;
            this.type = CollectibleType.SILVER;
        }else if(mat == "Bronze (Instance)"){
            this.points = 1;
            this.type = CollectibleType.BRONZE;
        }else{
            this.points = 3;
            this.type = CollectibleType.GOLD;
        }
        print("I am worth " + this.points + " points!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getWorth() {
        return this.points;
    }

    public CollectibleType getType() {
        return this.type;
    }


}
