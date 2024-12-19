
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect", menuName = "Data/Item effect/Thunder strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }
}
