using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private Blackhole_Skill_Controller blackHole;


    public void SetUpHotKey(KeyCode _myHotKey, Transform _myEnemy, Blackhole_Skill_Controller _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myHotKey = _myHotKey;
        myText.text = myHotKey.ToString();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
