using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> KeyCodeList;
    public float maxSize;
    public float growSpeed;
    public bool canGrow;
    private List<Transform> targets = new List<Transform>();

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTimer(true);
            CreateHotKey(collision);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (KeyCodeList.Count <= 0)
        {
            Debug.LogWarning("Not Enought");
            return;
        }
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        KeyCode choosenKey = KeyCodeList[Random.Range(0, KeyCodeList.Count)];
        KeyCodeList.Remove(choosenKey);

        Blackhole_HotKey_Controller newHotKeyController = newHotKey.GetComponent<Blackhole_HotKey_Controller>();

        newHotKeyController.SetUpHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
