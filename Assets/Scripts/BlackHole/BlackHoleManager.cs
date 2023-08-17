using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleManager : MonoBehaviour
{
    private int maxBlackHoles = 1;
    private int currentBlackHoles;
    private List<GameObject> lofBlackHoles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        currentBlackHoles = 0;
    }

    public int getMaxBlackHoles() { return maxBlackHoles; }
    public void addBlackHole(GameObject blackHole)
    {
        currentBlackHoles++;
        lofBlackHoles.Add(blackHole);
    }
    public void removeBlackHole(GameObject blackHole)
    {
        currentBlackHoles--;
        lofBlackHoles.Remove(blackHole);
    }
    public int getNumBlackHoles() { return currentBlackHoles; }
    public void clearBlackHolesList()
    {
        foreach (var blackHole in lofBlackHoles)
        {
            Destroy(blackHole);
        }
        lofBlackHoles.Clear();
    }

    public GameObject getBlackHole()
    {
        return lofBlackHoles[0];
    }

    public int getBlackHolesCount() {  return lofBlackHoles.Count; }
}
