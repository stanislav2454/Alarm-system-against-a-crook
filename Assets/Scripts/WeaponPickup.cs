using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponBase _weaponPrefab;
    [SerializeField] private string _pickupMessage = "Press E to pick up";
    [SerializeField] private float _pickupRadius = 2f;

    private Vector3 _weaponPosition = new Vector3(0.55f, 1.19f, 0.775f);
    private Vector3 _weaponRotation = Vector3.zero;
    private bool _playerInRange;

    private void Update()
    {
        if (_playerInRange && Input.GetKeyDown(KeyCode.E))//TODO:
        {
            TryPickupWeapon();
        }
    }

    private void TryPickupWeapon()
    {
        var player = FindObjectOfType<Userinput>();//TODO:
        if (player != null)
        {
            var inventory = player.GetComponent<WeaponInventory>();
            if (inventory != null)
            {
                WeaponBase newWeapon = Instantiate(_weaponPrefab, player.transform);
                newWeapon.transform.localPosition = _weaponPosition;
                newWeapon.transform.localEulerAngles = _weaponRotation;

                inventory.AddWeapon(newWeapon);
                Destroy(gameObject);

                Debug.Log($"Picked up: {_weaponPrefab.Name}");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Userinput>(out _))
        {
            _playerInRange = true;
            // Здесь можно показать UI подсказку
            Debug.Log(_pickupMessage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Userinput>(out _))
        {
            _playerInRange = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _pickupRadius);
    }
}