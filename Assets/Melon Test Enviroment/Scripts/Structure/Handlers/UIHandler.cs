using GameDefinations;
using Interfaces;
using UnityEngine;

public enum UI_STATES
{
    Main_Inventory,
    Character_Info,
}

public class UIHandler : MonoBehaviour
{
    public GameObject inventory_prefab;
    
    private GameObject currUI_go;

    private EntityController entityController;
    private FileDatabase fileDatabase;
    private GameDatabase gameDatabase;

    private float timeStamp;
    public float cooldown = 0.075f;

    public string currState = "";
    
    private void Start()
    {
        timeStamp = Time.time + cooldown;

        entityController = GetComponent<EntityController>();
        fileDatabase = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<FileDatabase>();
        gameDatabase = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<GameDatabase>();

        UI_STATES[] states = Utils.GetEnumArray<UI_STATES>();

        for (int i = 0; i < states.Length; i++)
        {
            gameDatabase.RegisterUIState(states[i].ToString().ToLower());
        }
        
    }

    private void Update()
    {
        if(currState == "inv" && currUI_go == null) { currUI_go = Instantiate(inventory_prefab); }
        if(currState == "")
        {
            if(currUI_go != null)
            {
                Destroy(currUI_go);
                currUI_go = null;
            }
        }
    }

    public void SetState(string state)
    {
        if (timeStamp <= Time.time)
        {
            timeStamp = Time.time + cooldown;
            Debug.Log("called");
            if (currState == state) { currState = ""; }
            else { currState = state; }
        }
    }
}
