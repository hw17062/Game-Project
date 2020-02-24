using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientStat
{
    private bool present = false;
    private int hp = 0;
    private int ap = 0;
    private GameObject rpcObj;

    public ClientStat()
    {

    }

    public ClientStat(string name, GameObject rpc)
    {
        switch (name)
        {
            case "Warrior":
                present = true;
                hp = 10;
                ap = 10;
                rpcObj = rpc;
                break;
            case "Ranger":
                present = true;
                hp = 10;
                ap = 10;
                rpcObj = rpc;
                break;
            case "Mage":
                present = true;
                hp = 10;
                ap = 10;
                rpcObj = rpc;
                break;
            case "Cleric":
                present = true;
                hp = 10;
                ap = 10;
                rpcObj = rpc;
                break;
            case "DungeonMaster":
                present = true;
                hp = 10;
                ap = 10;
                rpcObj = rpc;
                break;
        }
    }

    public ClientStat(string name)
    {
        switch (name)
        {
            case "Warrior":
                present = true;
                hp = 10;
                ap = 10;
                break;
            case "Ranger":
                present = true;
                hp = 10;
                ap = 10;
                break;
            case "Mage":
                present = true;
                hp = 10;
                ap = 10;
                break;
            case "Cleric":
                present = true;
                hp = 10;
                ap = 10;
                break;
            case "DungeonMaster":
                present = true;
                hp = 10;
                ap = 10;
                break;
        }
    }

    public ClientStat(bool pres, int health, int stam)
    {
        present = pres;
        hp = health;
        ap = stam;
    }

    public void setPres(bool set)
    {
        present = set;
    }

    public void setHP(int set)
    {
        hp = set;
    }

    public void setAP(int set)
    {
        ap = set;
    }

    public void setRpc(GameObject rpc)
    {
        rpcObj = rpc;
    }

    public bool getPres()
    {
        return present;
    }

    public int getHP()
    {
        return hp;
    }

    public int getAP()
    {
        return ap;
    }

    public GameObject getRPC()
    {
        return rpcObj;
    }
}
