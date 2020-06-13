using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public bool isPortal;

    public bool isPellet;
    public bool isSupperPellet;

    public bool didConsumePlayerOne;
    public bool didConsumePlayerTwo;

    public bool isGhostHouseEntrance;
    public bool isGhostHouse;

    public bool isBonusItem;
    public int pointValue;

    public GameObject portalReciver;

}
