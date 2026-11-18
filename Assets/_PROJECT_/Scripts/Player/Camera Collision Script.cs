using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraCollisionScript : MonoBehaviour
{
    [SerializeField] GameObject _pivot;
    [SerializeField] LayerMask _collisionMask;
    [SerializeField] float _radius;
    [SerializeField] float _bufferDistance;
    [SerializeField] float _minDistance;

    [SerializeField] float _returnSpeed;
    [SerializeField] float _hideDistance;

    private Camera _camera;

    private Vector3 _offset;
    private float _fullFollowDistance;

    private float _currentFollowDistance;
    private Vector3 _direction;

    private Vector3 _normDirection;

    private RaycastHit hitInfo;

    private int _playerLayerNumber;

    private int _playerBit;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _offset = gameObject.transform.localPosition;
        _fullFollowDistance = _offset.magnitude;
        _currentFollowDistance = _fullFollowDistance;
        _camera = gameObject.GetComponent <Camera> ();
        _playerLayerNumber = LayerMask.NameToLayer("Player");
        _playerBit = 1 << _playerLayerNumber;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        float target = _fullFollowDistance;
        _direction = _pivot.transform.TransformDirection(_offset);
        _normDirection = _direction.normalized;
        bool hitSomething = Physics.SphereCast(_pivot.transform.position, _radius, _normDirection, out hitInfo, _fullFollowDistance, _collisionMask, QueryTriggerInteraction.Ignore);
        if (hitSomething)
        {
            target = (Mathf.Max(hitInfo.distance - _bufferDistance, _minDistance));
        }
           if (target < _currentFollowDistance)
            {
                _currentFollowDistance = target;
            }
           if (target > _currentFollowDistance)
            {
                _currentFollowDistance = Mathf.MoveTowards(_currentFollowDistance, target, _returnSpeed *Time.deltaTime);
            }
        
    gameObject.transform.localPosition = _offset.normalized * _currentFollowDistance;
        if (_currentFollowDistance < _hideDistance)
        {
            _camera.cullingMask = _camera.cullingMask & ~_playerBit;
        }
        else
        {
            _camera.cullingMask = _camera.cullingMask | _playerBit;
        }
            
    }
}
