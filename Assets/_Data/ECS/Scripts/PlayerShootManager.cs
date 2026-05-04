using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PlayerShootManager : MonoBehaviour
{
    public static PlayerShootManager Instance { get; private set; }
    [SerializeField] private GameObject shootPopupPrefab;

    private void Awake()
    {
        Instance = this;
    }

    /*private void Start()
    {
        PlayerShootingSystem playerShootingSystem = 
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PlayerShootingSystem>();
        playerShootingSystem.OnShoot += PlayerShootingSystem_OnShoot;
    }

    private void PlayerShootingSystem_OnShoot(object sender, System.EventArgs e)
    {
        Entity playerEntity = (Entity)sender;
        LocalTransform localTransform = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(playerEntity);
        Instantiate(shootPopupPrefab, localTransform.Position, Quaternion.identity);
    }*/

    public void PlayerShoot(Vector3 playerPos)
    {
        Instantiate(shootPopupPrefab, playerPos, Quaternion.identity);
    }
}
