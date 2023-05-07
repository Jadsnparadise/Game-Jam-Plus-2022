using System;
using UnityEngine.Events;
using UnityEngine;
using Trigger.Core;

namespace Trigger
{
    [Serializable]
    public abstract class BasicTriggerSystem
    {
        #region Variables
        [SerializeField] string triggerTag = "New Trigger";
        [SerializeField] LayerMask triggerLayerMask;
        [SerializeField] Transform centerObject;
        [SerializeField] TriggerEvents triggerEvents;
        [SerializeField] DrawTrigger drawSettings = new DrawTrigger();


        #region Getters
        public string TriggerTag => triggerTag;
        public LayerMask TriggerLayerMask => triggerLayerMask;
        public Transform CenterObject => centerObject;
        public bool entered { get; private set; }
        public TriggerEvents TriggerEvents => triggerEvents;
        public DrawTrigger DrawSettings => drawSettings;
        #endregion

        #endregion

        #region Methods

        public BasicTriggerSystem()
        {
            triggerTag = "New Trigger";
            drawSettings = new DrawTrigger();
        }

        /// <summary>
        /// Checks the trigger tag against the defined tag.
        /// </summary>
        /// <param name="_tag">Tag to compare.</param>
        /// <returns>Returns true if trigger has the same tag. Returns false otherwise.</returns>
        public virtual bool CompareTag(string _tag)
        {
            return Equals(_tag, triggerTag);
        }

        #region In Trigger Methods
        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <param name="_position">Trigger's center. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger(Vector3 _position, bool callbacks = true);

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <param name="_transform">Trigger's center by transform. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger(Transform _transform, bool callbacks = true);

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <param name="_gameObject">Trigger's center by game object. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger(GameObject _gameObject, bool callbacks = true);

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <param name="_collider2D">Trigger's center by collider2D. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger(Collider2D _collider2D, bool callbacks = true);

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="_component">Trigger's center by <typeparamref name="T"/>. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger<T> (T _component, bool callbacks = true) where T : Component;

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="_position">Trigger's center. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="_ts">Array of components from the trigger.</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger<T>(Vector3 _position, out T[] _ts, bool callbacks = true) where T : class;

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="_transform">Trigger's center by transform. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="_ts">Array of components from the trigger.</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger<T>(Transform _transform, out T[] _ts, bool callbacks = true) where T : class;

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="_gameObject">Trigger's center by game object. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="_ts">Array of components from the trigger.</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger<T>(GameObject _gameObject, out T[] _ts, bool callbacks = true) where T : class;

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="_collider2D">Trigger's center by collider2D. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="_ts">Array of components from the trigger.</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger<T>(Collider2D _collider2D, out T[] _ts, bool callbacks = true) where T : Component;

        /// <summary>
        /// Checks if there's something in trigger and gets a component from it.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="_position">Trigger's center by position. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <param name="_debugError">Debug failed tries to get <typeparamref name="T"/> when it's true. (True by default)</param>
        /// <returns>Returns the component got by trigger.</returns>
        public abstract T InTrigger<T>(Vector3 _position, bool callbacks = true, bool _debugError = true) where T : class;

        /// <summary>
        /// Checks if there's something in trigger and gets a component from it.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="_transform">Trigger's center by transform. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <param name="_debugError">Debug failed tries to get <typeparamref name="T"/> when it's true. (True by default)</param>
        /// <returns>Returns the component got by trigger.</returns>
        public abstract T InTrigger<T>(Transform _transform, bool callbacks = true, bool _debugError = true) where T : class;

        /// <summary>
        /// Checks if there's something in trigger and gets a component from it.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="_gameObject">Trigger's center by game object. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <param name="_debugError">Debug failed tries to get <typeparamref name="T"/> when it's true. (True by default)</param>
        /// <returns>Returns the component got by trigger.</returns>
        public abstract T InTrigger<T>(GameObject _gameObject, bool callbacks = true, bool _debugError = true) where T : class;

        /// <summary>
        /// Checks if there's something in trigger and gets a component from it.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="_collider2D">Trigger's center by collider2D. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <param name="_debugError">Debug failed tries to get <typeparamref name="T"/> when it's true. (True by default)</param>
        /// <returns>Returns the component got by trigger.</returns>
        public abstract T InTrigger<T>(Collider2D _collider2D, bool callbacks = true, bool _debugError = true) where T : class;
        #endregion

        #region GetComponent
        public abstract T[] GetComponent<T>(Vector3 _position) where T : class;
        public abstract T[] GetComponent<T>(Transform _transform) where T : class;
        public abstract T[] GetComponent<T>(GameObject _gameObject) where T : class;
        public abstract T[] GetComponent<T>(Collider2D _collider2D) where T : class;
        public abstract T[] GetComponent<T>(T _component) where T : Component;
        #endregion

        protected void InvokeCallbacks(bool _isIn)
        {
            if (_isIn)
            {
                if (entered)
                {
                    triggerEvents.OnTriggerStayInsideInvoke();
                }
                else
                {
                    entered = true;
                    triggerEvents.OnTriggerEnterInvoke();
                }
            }
            else
            {
                if (entered)
                {
                    entered = false;
                    triggerEvents.OnTriggerExitInvoke();
                }
                else
                {
                    triggerEvents.OnTriggerStayOutsideInvoke();
                }
            }
        }

        #region DrawMethods
        public abstract void DrawTrigger(Vector3 _position);
        public abstract void DrawTrigger(Transform _transform);
        public abstract void DrawTrigger(GameObject _gameObject);
        public abstract void DrawTrigger(Collider2D _collider2D);
        #endregion

        #endregion
    }
}

namespace Trigger.Core
{
    [Serializable]
    public class DrawTrigger
    {
        #region Variables
        [SerializeField, Tooltip("Draw the trigger on gizmos")] bool draw = true;
        [SerializeField, Tooltip("Draw the trigger on gizmos only when the game object is selected")] DrawMode drawMethod = DrawMode.OnDrawGizmos;
        [SerializeField, Tooltip("Draw a solid trigger")] bool drawSolid = false;
        [SerializeField, Tooltip("Color of the trigger when there's something inside")] Color inColor = Color.green;
        [SerializeField, Tooltip("Color of the trigger when there's nothing inside")] Color outColor = Color.red;

        #region Getters
        /// <summary> Draw the trigger on gizmos. </summary>
        public bool Draw => draw;

        /// <summary> Draw the trigger on gizmos. </summary>
        public DrawMode DrawMethod => drawMethod;

        /// <summary> Draw a solid trigger. </summary>
        public bool DrawSolid => drawSolid;

        /// <summary> Color of the trigger when there's something inside. </summary>
        public Color InColor => inColor;

        /// <summary> Color of the trigger when there's nothing inside. </summary>
        public Color OutColor => outColor;
        #endregion

        #endregion

        #region Constructors
        public DrawTrigger()
        {
            draw = true;
            inColor = Color.green;
            outColor = Color.red;
        }
        public DrawTrigger(Color _inColor)
        {
            draw = true;
            inColor = _inColor;
            outColor = Color.red;
        }
        public DrawTrigger(Color _inColor, Color _outColor)
        {
            draw = true;
            inColor = _inColor;
            outColor = _outColor;
        }
        #endregion


        #region Enuns
        [Serializable]
        public enum DrawMode
        {
            OnDrawGizmos, OnDrawGizmosSelect
        }
        #endregion
    }


    [Serializable]
    public class TriggerEvents
    {
        #region Variables
        [SerializeField, Tooltip("Methods that are called when something enter the trigger")] UnityEvent OnTriggerEnter;
        [SerializeField, Tooltip("Methods that are called when something stay inside the trigger")] UnityEvent OnTriggerStayInside;
        [SerializeField, Tooltip("Methods that are called when something stay outside the trigger")] UnityEvent OnTriggerStayOutside;
        [SerializeField, Tooltip("Methods that are called when something exit the trigger")] UnityEvent OnTriggerExit;
        #endregion

        #region Methods
        #region InvokeMethods
        /// <summary> Invoke all methods from OnTriggerEnter unity event. </summary>
        public void OnTriggerEnterInvoke() => OnTriggerEnter?.Invoke();

        /// <summary> Invoke all methods from OnTriggerStayInside unity event. </summary>
        public void OnTriggerStayInsideInvoke() => OnTriggerStayInside?.Invoke();

        /// <summary> Invoke all methods from OnTriggerStayInside unity event. </summary>
        public void OnTriggerStayOutsideInvoke() => OnTriggerStayOutside?.Invoke();

        /// <summary> Invoke all methods from OnTriggerExit unity event. </summary>
        public void OnTriggerExitInvoke() => OnTriggerExit?.Invoke();
        #endregion

        #region AddMethods
        /// <summary> Add a listner to OnTriggerEnter unity event. </summary>
        /// <param name="_newAction"></param>
        public void OnTriggerEnterAddListner(UnityAction _newAction) => OnTriggerEnter.AddListener(_newAction);

        /// <summary> Add a listner to OnTriggerStayInside unity event. </summary>
        /// <param name="_newAction"></param>
        public void OnTriggerStayInsideAddListner(UnityAction _newAction) => OnTriggerStayInside.AddListener(_newAction);

        /// <summary> Add a listner to OnTriggerStayOutside unity event. </summary>
        /// <param name="_newAction"></param>
        public void OnTriggerStayOutsideAddListner(UnityAction _newAction) => OnTriggerStayOutside.AddListener(_newAction);

        /// <summary> Add a listner to OnTriggerExit unity event. </summary>
        /// <param name="_newAction"></param>
        public void OnTriggerExitAddListner(UnityAction _newAction) => OnTriggerExit.AddListener(_newAction);
        #endregion

        #region RemoveMethods
        /// <summary> Remove a listner from OnTriggerEnter unity event. </summary>
        /// <param name="_actionToRemove"></param>
        public void OnTriggerEnterRemoveListner(UnityAction _actionToRemove) => OnTriggerEnter.RemoveListener(_actionToRemove);

        /// <summary> Remove a listner from OnTriggerStayInside unity event. </summary>
        /// <param name="_actionToRemove"></param>
        public void OnTriggerStayInsideRemoveListner(UnityAction _actionToRemove) => OnTriggerStayInside.RemoveListener(_actionToRemove);

        /// <summary> Remove a listner from OnTriggerStayOutside unity event. </summary>
        /// <param name="_actionToRemove"></param>
        public void OnTriggerStayOutsideRemoveListner(UnityAction _actionToRemove) => OnTriggerStayOutside.RemoveListener(_actionToRemove);

        /// <summary> Remove a listner from OnTriggerExit unity event. </summary>
        /// <param name="_actionToRemove"></param>
        public void OnTriggerExitRemoveListner(UnityAction _actionToRemove) => OnTriggerExit.RemoveListener(_actionToRemove);
        #endregion

        #region ClearMethods
        /// <summary> Remove all listeners from OnTriggerEnter </summary>
        public void OnTriggerEnterClear() => OnTriggerEnter.RemoveAllListeners();

        /// <summary> Remove all listeners from OnTriggerStayInside </summary>
        public void OnTriggerStayInsideClear() => OnTriggerStayInside.RemoveAllListeners();

        /// <summary> Remove all listeners from OnTriggerStayInside </summary>
        public void OnTriggerStayOutsideClear() => OnTriggerStayOutside.RemoveAllListeners();

        /// <summary> Remove all listeners from OnTriggerExit </summary>
        public void OnTriggerExitClear() => OnTriggerExit.RemoveAllListeners();
        #endregion
        #endregion
    }

    public static partial class Vector2Extension
    {
        public static Vector3 ToVector3(this Vector2 _vector2)
        {
            return _vector2;
        }
    }

    [Serializable]
    public enum UpdateMethod
    {
        Update = 0, FixedUpdate = 1, LateUpdate = 2
    }
}