using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBarrier : MonoBehaviour
{
    public CombatZone combatZone;
    public bool requiresShrineActivation;
    public ShrineOfClarity requiredShrine;

    public int barrelsRequiredToDestroy = 3; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CanOpen())
            OpenBarrier();
    }

    bool CanOpen()
    {
        if (requiresShrineActivation && !requiredShrine.IsActivated)
            return false;

        if (combatZone != null && !combatZone.IsCleared)
            return false;

        int barrelsLeft = GameObject.FindGameObjectsWithTag("Barrel").Length;
        int destroyed = Mathf.Max(0, barrelsRequiredToDestroy - barrelsLeft);
        if (destroyed != barrelsRequiredToDestroy)
            return false;

        return true;
    }

    void OpenBarrier()
    {
        gameObject.SetActive(false);
    }
}
