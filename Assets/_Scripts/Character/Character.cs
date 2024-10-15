using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour, IControllable
{
    [SerializeField] private InventorySystem _InventorySystem; // temp

    [SerializeField] private float _speed = 10f;

    private CharacterController _characterController;
    private Transform _transform;

    private IPickable _currentPickableItem;
    private Transform _currentPickableItemTransform;

    private List<Transform> _pickableQueue = new List<Transform>();

    public List<ItemCraftStruct> avalibleCrafts;//temp
    public ItemCraftStruct craft; // temp
    public bool iscraft = false; // temp

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _transform = transform;
    }

    public void Action()
    {
        Debug.Log("Action");
        if (iscraft)
            CraftManager.Instance.CraftItem(craft);
        else
            avalibleCrafts = CraftManager.Instance.GetAvalibleCrafts(_InventorySystem._inventoryItems);  
    }

    public void Interact()
    {
        if (_currentPickableItem == null)
            return;
        _currentPickableItem.PickUp();
        GetPickableFromQueue();
        Debug.Log("Interact");
    }

    public void Move(Vector2 direction)
    {
        Vector3 scaledMovement = new Vector3(direction.x, 0f, direction.y) * _speed * Time.fixedDeltaTime;

        _transform.LookAt(_transform.position + scaledMovement, Vector3.up);
        _characterController.Move(scaledMovement);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable")) 
        {
            if (_currentPickableItem == null)
            {
                _currentPickableItem = other.GetComponent<IPickable>();
                _currentPickableItemTransform = other.transform;
                _currentPickableItem.SubscribeOnItemPickUp(_InventorySystem.AddItems);
                Debug.Log("Can pick up: " + other.name);
                return;
            }
            _pickableQueue.Add(other.transform);
            Debug.Log("Added to Queue: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == _currentPickableItemTransform)
        {
            _currentPickableItem.UnsubscribeOnItemPickUp(_InventorySystem.AddItems);
            Debug.Log("Get form Queue: " + _currentPickableItemTransform.name);
            GetPickableFromQueue();           
        }
        else if(_pickableQueue.Contains(other.transform))
        {
            _pickableQueue.Remove(other.transform);
        }
    }

    private void GetPickableFromQueue()
    {
        _currentPickableItem = null;
        _currentPickableItemTransform = null;

        if (_pickableQueue.Count == 0)
            return;

        _currentPickableItemTransform = _pickableQueue[0];
        _currentPickableItem = _currentPickableItemTransform.GetComponent<IPickable>();

        _currentPickableItem.SubscribeOnItemPickUp(_InventorySystem.AddItems);

        _pickableQueue.RemoveAt(0);
    }
}
