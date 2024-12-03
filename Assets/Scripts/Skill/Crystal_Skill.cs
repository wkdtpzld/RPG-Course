using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpped;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    public override void UseSkill()
    {
        base.UseSkill();

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);

            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

            currentCrystalScript.SetUpCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpped, FindClosestEnemy(currentCrystal.transform));
        }
        else
        {
            if (canMoveToEnemy) return;
            player.transform.position = currentCrystal.transform.position;

            currentCrystal.transform.position = player.transform.position;
            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
