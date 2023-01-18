using System.Collections.Generic;
using UnityEngine;

namespace CollisionSystem
{
    [System.Serializable]
    public class Collision
    {
        [Header("Settings")]
        [SerializeField, Tooltip("Collider tag to compare")] string colliderTag = "New Collider";
        [SerializeField, Tooltip("Collider Dimension")] Dimension dimension = Dimension.TwoD;
        [SerializeField, Tooltip("Layer of collision")] LayerMask layer;
        [SerializeField, Tooltip("Collider offset")] Vector3 offset = Vector3.zero;
        [SerializeField, Tooltip("Collider Mesh")] ColliderMesh colliderMesh = ColliderMesh.Box;
        [Space(10)]
        [Header("Box Settings")]
        [SerializeField, Tooltip("Box collider size"), Min(0)] Vector2 size = Vector2.one;
        [Space(10)]
        [Header("Circle Settings")]
        [SerializeField, Tooltip("Circle collider radius"), Min(0)] float radius = 1;
        [Space(10)]
        [Header("Raycast Settings")]
        [SerializeField, Tooltip("Raycast direction")] RaycastDirection raycastDir = RaycastDirection.X;
        [SerializeField, Tooltip("Raycast distance"), Min(0)] float raycastDistance = 1;
        [SerializeField, Tooltip("Flip Raycast")] bool flip = false;
        [Space(10)]
        [SerializeField] Draw colliderRenderer = new Draw();
        public bool CompareTag(string _tag)
        {
            return colliderTag == _tag;
        }
        public bool CompareTag(Collision _c)
        {
            return colliderTag == _c.colliderTag;
        }
        public bool InCollision(Vector3 _origin)
        {
            if (dimension == Dimension.TwoD)
            {
                switch (colliderMesh)
                {
                    case ColliderMesh.Box:
                        return Physics2D.OverlapBox(_origin + offset, size, 0f, layer);
                    case ColliderMesh.Sphere:
                        return Physics2D.OverlapCircle(_origin + offset, radius, layer);
                    case ColliderMesh.Raycast:
                        Vector3 dir = Vector3.zero;
                        if (!flip)
                        {
                            switch (raycastDir)
                            {
                                case RaycastDirection.X:
                                    dir = Vector3.right;
                                    break;
                                case RaycastDirection.Y:
                                    dir = Vector3.up;
                                    break;
                                case RaycastDirection.Z:
                                    dir = Vector3.forward;
                                    break;
                            }
                        }
                        else
                        {
                            switch (raycastDir)
                            {
                                case RaycastDirection.X:
                                    dir = Vector3.left;
                                    break;
                                case RaycastDirection.Y:
                                    dir = Vector3.down;
                                    break;
                                case RaycastDirection.Z:
                                    dir = Vector3.back;
                                    break;
                            }
                        }
                        return Physics2D.Raycast(_origin + offset, dir, raycastDistance, layer);
                    default:
                        return false;
                }
            }
            else
            {
                switch (colliderMesh)
                {
                    case ColliderMesh.Box:
                        Collider[] c = Physics.OverlapBox(_origin + offset, size, Quaternion.identity, layer);
                        return c.Length > 0;
                    case ColliderMesh.Sphere:
                        Collider[] s = Physics.OverlapSphere(_origin + offset, radius, layer);
                        return s.Length > 0;
                    case ColliderMesh.Raycast:
                        Vector3 dir = Vector3.zero;
                        if (!flip)
                        {
                            switch (raycastDir)
                            {
                                case RaycastDirection.X:
                                    dir = Vector3.right;
                                    break;
                                case RaycastDirection.Y:
                                    dir = Vector3.up;
                                    break;
                                case RaycastDirection.Z:
                                    dir = Vector3.forward;
                                    break;
                            }
                        }
                        else
                        {
                            switch (raycastDir)
                            {
                                case RaycastDirection.X:
                                    dir = Vector3.left;
                                    break;
                                case RaycastDirection.Y:
                                    dir = Vector3.down;
                                    break;
                                case RaycastDirection.Z:
                                    dir = Vector3.back;
                                    break;
                            }
                        }
                        return Physics.Raycast(_origin + offset, dir, raycastDistance, layer);
                    default:
                        return false;
                }
            }
        }
        public bool InCollision(Vector3 _origin, out Collider2D[] _col)
        {
            switch (colliderMesh)
            {
                case ColliderMesh.Box:
                    _col = Physics2D.OverlapBoxAll(_origin + offset, size, 0f, layer);
                    return Physics2D.OverlapBox(_origin + offset, size, 0f, layer);
                case ColliderMesh.Sphere:
                    _col = Physics2D.OverlapCircleAll(_origin + offset, radius, layer);
                    return Physics2D.OverlapCircle(_origin + offset, radius, layer);
                case ColliderMesh.Raycast:
                    _col = null;
                    Vector3 dir = Vector3.zero;
                    if (!flip)
                    {
                        switch (raycastDir)
                        {
                            case RaycastDirection.X:
                                dir = Vector3.right;
                                break;
                            case RaycastDirection.Y:
                                dir = Vector3.up;
                                break;
                            case RaycastDirection.Z:
                                dir = Vector3.forward;
                                break;
                        }
                    }
                    else
                    {
                        switch (raycastDir)
                        {
                            case RaycastDirection.X:
                                dir = Vector3.left;
                                break;
                            case RaycastDirection.Y:
                                dir = Vector3.down;
                                break;
                            case RaycastDirection.Z:
                                dir = Vector3.back;
                                break;
                        }
                    }
                    return Physics2D.Raycast(_origin + offset, dir, raycastDistance, layer);
                default:
                    _col = null;
                    return false;
            }
        }
        public bool InCollision(Vector3 _origin, out Collider[] _col)
        {
            switch (colliderMesh)
            {
                case ColliderMesh.Box:
                    Collider[] c = Physics.OverlapBox(_origin + offset, size, Quaternion.identity, layer);
                    _col = c;
                    return c.Length > 0;
                case ColliderMesh.Sphere:
                    Collider[] s = Physics.OverlapSphere(_origin + offset, radius, layer);
                    _col = s;
                    return s.Length > 0;
                case ColliderMesh.Raycast:
                    _col = null;
                    Vector3 dir = Vector3.zero;
                    if (!flip)
                    {
                        switch (raycastDir)
                        {
                            case RaycastDirection.X:
                                dir = Vector3.right;
                                break;
                            case RaycastDirection.Y:
                                dir = Vector3.up;
                                break;
                            case RaycastDirection.Z:
                                dir = Vector3.forward;
                                break;
                        }
                    }
                    else
                    {
                        switch (raycastDir)
                        {
                            case RaycastDirection.X:
                                dir = Vector3.left;
                                break;
                            case RaycastDirection.Y:
                                dir = Vector3.down;
                                break;
                            case RaycastDirection.Z:
                                dir = Vector3.back;
                                break;
                        }
                    }
                    return Physics.Raycast(_origin + offset, dir, raycastDistance, layer);
                default:
                    _col = null;
                    return false;
            }
        }
        public bool InCollision(Transform _origin)
        {
            return InCollision(_origin.position);
        }
        public bool InCollision(Transform _origin, out Collider2D[] _col)
        {
            return InCollision(_origin.position, out _col);
        }
        public bool InCollision(Transform _origin, out Collider[] _col)
        {
            return InCollision(_origin.position, out _col);
        }
        public bool InCollision(GameObject _origin)
        {
            return InCollision(_origin.transform.position);
        }
        public bool InCollision(GameObject _origin, out Collider2D[] _col)
        {
            return InCollision(_origin.transform.position, out _col);
        }
        public Collider2D[] ObjectsInCollider2D(Vector3 _origin)
        {
            switch (colliderMesh)
            {
                case ColliderMesh.Box:
                    return Physics2D.OverlapBoxAll(_origin + offset, size, 0f, layer);
                case ColliderMesh.Sphere:
                    return Physics2D.OverlapCircleAll(_origin + offset, radius, layer);
                case ColliderMesh.Raycast:
                    Vector3 dir = Vector3.zero;
                    if (!flip)
                    {
                        switch (raycastDir)
                        {
                            case RaycastDirection.X:
                                dir = Vector3.right;
                                break;
                            case RaycastDirection.Y:
                                dir = Vector3.up;
                                break;
                            case RaycastDirection.Z:
                                dir = Vector3.forward;
                                break;
                        }
                    }
                    else
                    {
                        switch (raycastDir)
                        {
                            case RaycastDirection.X:
                                dir = Vector3.left;
                                break;
                            case RaycastDirection.Y:
                                dir = Vector3.down;
                                break;
                            case RaycastDirection.Z:
                                dir = Vector3.back;
                                break;
                        }
                    }
                    RaycastHit2D[] ray = Physics2D.RaycastAll(_origin + offset, dir, raycastDistance, layer);
                    Collider2D[] c = new Collider2D[ray.Length];
                    for (int i = 0; i < c.Length; i++)
                    {
                        c[i] = ray[i].collider;
                    }
                    return c;
                default:
                    return null;
            }
        }
        public Collider2D[] ObjectsInCollider2D(Transform _origin)
        {
            return ObjectsInCollider2D(_origin.position);
        }
        public Collider2D[] ObjectsInCollider2D(GameObject _origin)
        {
            return ObjectsInCollider2D(_origin.transform.position);
        }
        public Collider[] ObjectsInCollider(Vector3 _origin)
        {
            switch (colliderMesh)
            {
                case ColliderMesh.Box:
                    return Physics.OverlapBox(_origin + offset, size, Quaternion.identity, layer);
                case ColliderMesh.Sphere:
                    return Physics.OverlapSphere(_origin + offset, radius, layer);
                case ColliderMesh.Raycast:
                    Vector3 dir = Vector3.zero;
                    if (!flip)
                    {
                        switch (raycastDir)
                        {
                            case RaycastDirection.X:
                                dir = Vector3.right;
                                break;
                            case RaycastDirection.Y:
                                dir = Vector3.up;
                                break;
                            case RaycastDirection.Z:
                                dir = Vector3.forward;
                                break;
                        }
                    }
                    else
                    {
                        switch (raycastDir)
                        {
                            case RaycastDirection.X:
                                dir = Vector3.left;
                                break;
                            case RaycastDirection.Y:
                                dir = Vector3.down;
                                break;
                            case RaycastDirection.Z:
                                dir = Vector3.back;
                                break;
                        }
                    }
                    RaycastHit[] ray = Physics.RaycastAll(_origin + offset, dir, raycastDistance, layer);//Physics2D.RaycastAll(_origin + offset, dir, raycastDistance, layer);
                    Collider[] c = new Collider[ray.Length];
                    for (int i = 0; i < c.Length; i++)
                    {
                        c[i] = ray[i].collider;
                    }
                    return c;
                default:
                    return null;
            }
        }
        public Collider[] ObjectsInCollider(Transform _origin)
        {
            return ObjectsInCollider(_origin.position);
        }
        public Collider[] ObjectsInCollider(GameObject _origin)
        {
            return ObjectsInCollider(_origin.transform.position);
        }
        public void DrawCollider(Vector3 _origin)
        {
            
            switch (colliderMesh)
            {
                case ColliderMesh.Box:
                    colliderRenderer.WireCube(InCollision(_origin), _origin, offset, size);
                    break;
                case ColliderMesh.Sphere:
                    colliderRenderer.WireSphere(InCollision(_origin), _origin, offset, radius);
                    break;
                case ColliderMesh.Raycast:
                    Vector3 dir = Vector3.zero;
                    if (!flip)
                    {
                        switch (raycastDir)
                        {
                            case RaycastDirection.X:
                                dir = Vector3.right;
                                break;
                            case RaycastDirection.Y:
                                dir = Vector3.up;
                                break;
                            case RaycastDirection.Z:
                                dir = Vector3.forward;
                                break;
                        }
                    }
                    else
                    {
                        switch (raycastDir)
                        {
                            case RaycastDirection.X:
                                dir = Vector3.left;
                                break;
                            case RaycastDirection.Y:
                                dir = Vector3.down;
                                break;
                            case RaycastDirection.Z:
                                dir = Vector3.back;
                                break;
                        }
                    }
                    colliderRenderer.Raycast(InCollision(_origin), _origin, offset, dir, raycastDistance, flip, raycastDir);
                    break;
            }
        }
        public void DrawCollider(Transform _origin)
        {
            DrawCollider(_origin.position);
        }
        public void DrawCollider(GameObject _origin)
        {
            DrawCollider(_origin.transform.position);
        }
    }    
    [System.Serializable]
    public class Draw
    {
        [SerializeField] bool showGizmos = true;
        [SerializeField, Tooltip("Gizmos color outside collision mesh"), InspectorName("Color Outside Collision Mesh")] protected Color colorOutCollision = Color.red;
        [SerializeField, Tooltip("Gizmos color inside collision mesh"), InspectorName("Color Inside Collision Mesh")] protected Color colorInCollision = Color.green;
        public void WireCube(bool _isIn, Vector3 _position, Vector3 _offset, Vector2 _size)
        {
            if (!showGizmos) return;
            Gizmos.color = _isIn ? colorInCollision : colorOutCollision;

            Gizmos.DrawWireCube(_position + _offset, _size);
        }
        public void WireSphere(bool _isIn, Vector3 _position, Vector3 _offset, float _radius)
        {
            if (!showGizmos) return;
            Gizmos.color = _isIn ? colorInCollision : colorOutCollision;
            Gizmos.DrawWireSphere(_position + _offset, _radius);
        }
        public void Raycast(bool _isIn, Vector3 _position, Vector3 _offset, Vector3 _direction, float _distance, bool _flip, RaycastDirection _raycastDir)
        {
            if (!showGizmos) return;    
            Gizmos.color = _isIn ? colorInCollision : colorOutCollision;
            Gizmos.DrawLine(_position + _offset, _position + (_direction * _distance) + _offset);
        }
    }
    public enum ColliderMesh { Box, Sphere, Raycast }
    public enum RaycastDirection { X, Y, Z }
    public enum Dimension { TwoD, ThreeD }
}