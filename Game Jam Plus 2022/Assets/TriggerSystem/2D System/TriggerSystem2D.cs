using System;
using UnityEngine;
using Trigger.Core;

namespace Trigger.System2D
{
    public abstract class System2D : BasicTriggerSystem
    {
        [SerializeField] Vector2 triggerOffset = Vector2.one;
        public Vector2 TriggerOffset => triggerOffset;

        #region Methods
        public System2D() : base()
        {
            SetTriggerOffset(Vector2.zero);
        }

        public void SetTriggerOffset(Vector2 _newOffset)
        {
            triggerOffset = _newOffset;
        }

        public void FlipOffset()
        {
            SetTriggerOffset(-triggerOffset);
        }

        public void FlipXOffset()
        {
            SetTriggerOffset(new Vector2(-triggerOffset.x, triggerOffset.y));
        }

        public void FlipYOffset()
        {
            SetTriggerOffset(new Vector2(triggerOffset.x, -triggerOffset.y));
        }
        #endregion
    }

    [Serializable]
    public class BoxTrigger2D : System2D
    {
        #region Variables
        [SerializeField] Vector2 triggerSize = Vector2.one;

        #region Getters
        public Vector2 TriggerSize => triggerSize;
        #endregion

        #endregion

        #region Methods

        #region Constructor

        public BoxTrigger2D() : base()
        {
            triggerSize = Vector2.one;
        }

        #endregion

        #region Draw

        public void SetTriggerSize(Vector2 _newSize)
        {
            triggerSize = _newSize;
        }

        public override void DrawTrigger(Vector3 _position)
        {
            if (!DrawSettings.Draw) return;
            if (CenterObject != null)
            {
                _position = CenterObject.position;
            }

            Gizmos.color = InTrigger(_position, false) ? DrawSettings.InColor : DrawSettings.OutColor;

            if (DrawSettings.DrawSolid)
            {
                Gizmos.DrawCube(_position + TriggerOffset.ToVector3(), TriggerSize);
            }
            else
            {
                Gizmos.DrawWireCube(_position + TriggerOffset.ToVector3(), TriggerSize);
            }
            
        }

        public override void DrawTrigger(Transform _transform)
        {
            DrawTrigger(_transform.position);
        }

        public override void DrawTrigger(GameObject _gameObject)
        {
            DrawTrigger(_gameObject.transform.position);
        }

        public override void DrawTrigger(Collider2D _collider2D)
        {
            DrawTrigger(_collider2D.transform.position);
        }
        #endregion

        #region InTrigger
        public override bool InTrigger(Vector3 _position, bool callbacks = true)
        {
            if (CenterObject != null)
            {
                _position = CenterObject.position;
            }
            bool isIn = Physics2D.OverlapBox(_position + TriggerOffset.ToVector3(), TriggerSize, 0f, TriggerLayerMask);
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }
            
            return isIn;
        }

        public override bool InTrigger(Transform _transform, bool callbacks = true)
        {
            return InTrigger(_transform.position, callbacks);
        }

        public override bool InTrigger(GameObject _gameObject, bool callbacks = true)
        {
            return InTrigger(_gameObject.transform.position, callbacks);
        }

        public override bool InTrigger(Collider2D _collider2D, bool callbacks = true)
        {
            return InTrigger(_collider2D.transform.position, callbacks);
        }

        public override bool InTrigger<T>(T _component, bool callbacks = true)
        {
            return InTrigger(_component.transform.position, callbacks);
        }

        public override bool InTrigger<T>(Vector3 _position, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_position);
            return InTrigger(_position, callbacks);
        }

        public override bool InTrigger<T>(Transform _transform, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_transform.position);
            return InTrigger(_transform.position, callbacks);
        }

        public override bool InTrigger<T>(GameObject _gameObject, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_gameObject.transform.position);
            return InTrigger(_gameObject.transform.position, callbacks);
        }

        public override bool InTrigger<T>(Collider2D _collider2D, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_collider2D.transform.position);
            return InTrigger(_collider2D.transform.position, callbacks);
        }

        public override T InTrigger<T>(Vector3 _position, bool callbacks = true, bool _debugErrors = true)
        {
            if (CenterObject != null)
            {
                _position = CenterObject.position;
            }
            if (!InTrigger(_position, callbacks)) return null;

            GameObject g = Physics2D.OverlapBox(_position + TriggerOffset.ToVector3(), TriggerSize, TriggerLayerMask).gameObject;
            if (g.TryGetComponent(out T _component))
            {
                return _component;
            }
            if (_debugErrors)
            {
                Debug.LogWarning($"<color=yellow>Warning:</color> The Game Object: <<color=#DC143C>{g.name}</color>> doesn't have: <<color=#DC143C>{typeof(T).Name}</color>> as component");
            }
            return null;
        }

        public override T InTrigger<T>(Transform _transform, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_transform.position, callbacks, _debugErrors);
        }

        public override T InTrigger<T>(GameObject _gameObject, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_gameObject.transform.position, callbacks, _debugErrors);
        }

        public override T InTrigger<T>(Collider2D _collider2D, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_collider2D.transform.position, callbacks, _debugErrors);
        }
        #endregion

        #region GetComponent
        public override T[] GetComponent<T>(Vector3 _position)
        {
            if (CenterObject != null)
            {
                _position = CenterObject.position;
            }
            Collider2D[] colliders = Physics2D.OverlapBoxAll(_position + TriggerOffset.ToVector3(), TriggerSize, 0f, TriggerLayerMask);
            int lenght = colliders.Length;
            T[] _ts = new T[lenght];
            for (int i = 0; i < lenght; i++)
            {
                if (colliders[i].TryGetComponent(out T _component))
                {
                    _ts[i] = _component;
                }
                else
                {
                    Debug.LogWarning($"<color=yellow>Warning:</color> The Game Object: <<color=#DC143C>{colliders[i].gameObject}</color>> doesn't have: <<color=#DC143C>{typeof(T).Name}</color>> as component");
                }
            }
            return _ts;
        }

        public override T[] GetComponent<T>(Transform _transform) { return GetComponent<T>(_transform.position); }
        public override T[] GetComponent<T>(GameObject _gameObject) { return GetComponent<T>(_gameObject.transform.position); }
        public override T[] GetComponent<T>(Collider2D _collider2D) { return GetComponent<T>(_collider2D.transform.position); }
        public override T[] GetComponent<T>(T _component) { return GetComponent<T>(_component.transform.position); }
        #endregion
        #endregion
    }


    [Serializable]
    public class CircleTrigger2D : System2D
    {
        #region Variables
        [SerializeField] float triggerRadius = 0.5f;

        #region Getters
        public float TriggerRadius => triggerRadius;
        #endregion

        #endregion

        #region Methods

        public void SetTriggerRadius(float _newRadius)
        {
            triggerRadius = _newRadius;
        }

        #region InTrigger
        public override bool InTrigger(Vector3 _position, bool callbacks = true)
        {
            if (CenterObject != null)
            {
                _position = CenterObject.position;
            }
            bool isIn = Physics2D.OverlapCircle(_position + TriggerOffset.ToVector3(), TriggerRadius, TriggerLayerMask);
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }
            return isIn;
        }

        public override bool InTrigger(Transform _transform, bool callbacks = true)
        {
            return InTrigger(_transform.position, callbacks);
        }

        public override bool InTrigger(GameObject _gameObject, bool callbacks = true)
        {
            return InTrigger(_gameObject.transform.position, callbacks);
        }

        public override bool InTrigger(Collider2D _collider2D, bool callbacks = true)
        {
            return InTrigger(_collider2D.transform.position, callbacks);
        }

        public override bool InTrigger<T>(T _component, bool callbacks = true)
        {
            return InTrigger(_component.transform.position, callbacks);
        }

        public override bool InTrigger<T>(Vector3 _position, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_position);
            return InTrigger(_position, callbacks);
        }

        public override bool InTrigger<T>(Transform _transform, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_transform.position);
            return InTrigger(_transform.position, callbacks);
        }

        public override bool InTrigger<T>(GameObject _gameObject, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_gameObject.transform.position);
            return InTrigger(_gameObject.transform.position, callbacks);
        }

        public override bool InTrigger<T>(Collider2D _collider2D, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_collider2D.transform.position);
            return InTrigger(_collider2D.transform.position, callbacks);
        }

        public override T InTrigger<T>(Vector3 _position, bool callbacks = true, bool _debugErrors = true)
        {
            if (CenterObject != null)
            {
                _position = CenterObject.position;
            }
            if (!InTrigger(_position, callbacks)) return null;

            GameObject g = Physics2D.OverlapCircle(_position + TriggerOffset.ToVector3(), TriggerRadius, TriggerLayerMask).gameObject;
            if (g.TryGetComponent(out T _component))
            {
                return _component;
            }
            if (_debugErrors)
            {
                Debug.LogWarning($"<color=yellow>Warning:</color> The Game Object: <<color=#DC143C>{g.name}</color>> doesn't have: <<color=#DC143C>{typeof(T).Name}</color>> as component");
            }
            return null;
        }

        public override T InTrigger<T>(Transform _transform, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_transform.position, callbacks, _debugErrors);
        }

        public override T InTrigger<T>(GameObject _gameObject, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_gameObject.transform.position, callbacks, _debugErrors);
        }

        public override T InTrigger<T>(Collider2D _collider2D, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_collider2D.transform.position, callbacks, _debugErrors);
        }
        #endregion

        #region GetComponent

        public override T[] GetComponent<T>(Vector3 _position)
        {
            if (CenterObject != null)
            {
                _position = CenterObject.position;
            }
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_position + TriggerOffset.ToVector3(), TriggerRadius, TriggerLayerMask);
            int lenght = colliders.Length;
            T[] _ts = new T[lenght];
            for (int i = 0; i < lenght; i++)
            {
                if (colliders[i].TryGetComponent(out T _component))
                {
                    _ts[i] = _component;
                }
                else
                {
                    Debug.LogWarning($"<color=yellow>Warning:</color> The Game Object: <<color=#DC143C>{colliders[i].gameObject}</color>> doesn't have: <<color=#DC143C>{typeof(T).Name}</color>> as component");
                }
            }
            return _ts;
        }

        public override T[] GetComponent<T>(Transform _transform)
        {
            return GetComponent<T>(_transform.position);
        }

        public override T[] GetComponent<T>(GameObject _gameObject)
        {
            return GetComponent<T>(_gameObject.transform.position);
        }

        public override T[] GetComponent<T>(Collider2D _collider2D)
        {
            return GetComponent<T>(_collider2D.transform.position);
        }

        public override T[] GetComponent<T>(T _component)
        {
            return GetComponent<T>(_component.transform.position);
        }
        #endregion

        #region Draw
        public override void DrawTrigger(Vector3 _position)
        {
            if (!DrawSettings.Draw) return;
            if (CenterObject != null)
            {
                _position = CenterObject.position;
            }
            Gizmos.color = InTrigger(_position, false) ? DrawSettings.InColor : DrawSettings.OutColor;

            if (DrawSettings.DrawSolid)
            {
                Gizmos.DrawSphere(_position + TriggerOffset.ToVector3(), TriggerRadius);
            }
            else
            {
                Gizmos.DrawWireSphere(_position + TriggerOffset.ToVector3(), TriggerRadius);
            }
            
        }

        public override void DrawTrigger(Transform _transform)
        {
            DrawTrigger(_transform.position);
        }

        public override void DrawTrigger(GameObject _gameObject)
        {
            DrawTrigger(_gameObject.transform.position);
        }

        public override void DrawTrigger(Collider2D _collider2D)
        {
            DrawTrigger(_collider2D.transform.position);
        }
        #endregion

        #endregion
    }
}