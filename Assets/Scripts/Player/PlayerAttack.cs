using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerData playerData;

    [Header("Shoot")]
    [SerializeField] private float shootRange = 10f;
    [SerializeField] private LayerMask shootMask;
    [SerializeField] private Transform firePoint;

    [Header("Laser Visual")]
    [SerializeField] private float laserDuration = 0.05f;

    private LineRenderer line;
    private bool isReloading;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
        line.useWorldSpace = true;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.material = new Material(Shader.Find("Sprites/Default"));
    }

    private void Update()
    {
        if (isReloading) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        isReloading = true;

        Vector2 origin = firePoint.position;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 direction = ((Vector2)mouseWorldPos - origin).normalized;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, shootRange, shootMask);

        Vector2 endPoint = origin + direction * shootRange;

        if (hit)
        {
            endPoint = hit.point;

            IHealthComponent health = hit.transform.GetComponent<IHealthComponent>();

            if (health != null)
            {
                health.SufferDamange(playerData.damage);
            }
        }

        StartCoroutine(ShowLaser(origin, endPoint));
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(playerData.reloadTime);
        isReloading = false;
    }

    IEnumerator ShowLaser(Vector2 start, Vector2 end)
    {
        line.enabled = true;
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        yield return new WaitForSeconds(laserDuration);

        line.enabled = false;
    }
}