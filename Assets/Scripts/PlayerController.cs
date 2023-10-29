using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _inputWindow = 0.5f;
    private bool _windowStarted = false;
    [SerializeField]
    private float _holdTime = 0.4f;
    private float _holdStart;

    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private GameObject _playerObject;
    private Rigidbody _rigidbody;

    [SerializeField]
    private int _jumpForce = 10;
    [SerializeField]
    private int _speed = 10;
    [SerializeField]
    private int _rotateDeg = 30;

    [SerializeField]
    private float crouchScale = 0.5f;
    [SerializeField]
    private bool _isCrouching = false;
    public Queue<PlayerInput> inputs { get; private set; }
    private void PerformPlayerAction(int numTaps, bool isHolding, bool isHoldingFirst, bool isSlowTap)
    {
        /// <summary>
        /// Performs an action on the PlayerObject's rigidbody.
        /// </summary>
        /// <param name="numTaps">
        /// How many taps were performed in the inputWindow.
        /// </param>
        /// <param name="isHolding">
        /// If a Hold action was in the inputQueue.
        /// </param>
        /// <param name="isHoldingFirst">
        /// If a Hold action was first in the inputQueue.
        /// </param>
        /// <param name="isSlowTap">
        /// If we did a slow tap.
        /// </param>
        /// There are a set number of actions the player can take.
        /// ------------------------------------------------------
        /// tap -> jump
        /// hold for 0.5 seconds->fast forward
        /// double - tap slow(> 0.1 seconds)->slow forward
        /// double - tap fast(within 0.1 seconds)->stop
        /// tap, hold for the rest of the window->turn right 30 *
        /// hold for > 0.3 seconds, tap->turn left 30 *
        /// triple - tap->crouch
        /// > 4 taps->back

        // Backward
        if (numTaps >= 4 && isHolding == false)
        {
            Debug.Log("Backward");
            _rigidbody.velocity = Vector3.back * _speed;
        }
        // Crouch
        else if (numTaps == 3 && isHolding == false && !_isCrouching)
        {
            Debug.Log("Crouch");
            _playerObject.transform.localScale = new Vector3(1, crouchScale, 1);
            _isCrouching = true;
        }
        // Uncrouch
        else if (numTaps == 3 && isHolding == false && _isCrouching)
        {
            Debug.Log("Uncrouch");
            _playerObject.transform.localScale = new Vector3(1, 1, 1);
            _isCrouching = false;
        }
        // Stop
        else if (numTaps == 2 && isHolding == false && !isSlowTap)
        {
            Debug.Log("Stop");
            _rigidbody.velocity = Vector3.zero;
        }
        // Walk Forward
        else if (numTaps == 2 && isHolding == false)
        {
            Debug.Log("Slow Forward");
            _rigidbody.velocity = Vector3.forward * _speed / 2;
        }
        // Jump
        else if (numTaps == 1 && isHolding == false)
        {
            Debug.Log("Jump");
            _rigidbody.AddForce(Vector3.up * _jumpForce);
        }
        // Run Forward
        else if (numTaps == 0 && isHolding == true)
        {
            Debug.Log("Run Forward");
            _rigidbody.velocity = Vector3.forward * _speed * 2;
        }

    }
    private void ProcessInputWindow()
    {
        /// <summary>
        /// Clears the inputs Queue and calls thte performPlayerAction()
        /// </summary>

        // If the Queue is empty, we don't have to do anything.
        if (inputs.Count == 0)
        {
            return;
        }
        int numTaps = 0;
        bool isHolding = false;
        bool isHoldingFirst = false;
        bool isSlowTap = false;
        int queueSize = inputs.Count;
        for (int i = 0; i < queueSize; i++)
        {
            PlayerInput playerInput = inputs.Dequeue();
            Debug.Log(playerInput);
            if (numTaps == 0 && playerInput == PlayerInput.Hold)
            {
                isHolding = true;
                isHoldingFirst = true;
            }
            if (playerInput == PlayerInput.Hold)
            {
                isHolding = true;
            }
            if (playerInput == PlayerInput.Tap)
            {
                numTaps += 1;
            }
            if (playerInput == PlayerInput.SlowTap)
            {
                numTaps += 1;
                isSlowTap = true;
            }
        }
        /*
        Debug.Log($"Number of Taps: {numTaps}");
        Debug.Log($"IsHolding?: {isHolding}");
        Debug.Log($"IsHoldingFirst?: {isHoldingFirst}");
        */
        inputs.Clear();
        PerformPlayerAction(numTaps, isHolding, isHoldingFirst, isSlowTap);
    }
    private void Start()
    {
        _rigidbody = _playerObject.GetComponent<Rigidbody>();
        inputs = new Queue<PlayerInput>();
        _holdStart = Time.time;
    }
    void Update()
    {
        AddToInputQueue();
    }
    private void AddToInputQueue()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_windowStarted)
            {
                StartCoroutine(ExecuteInputWindowCoroutine());
            }
            Debug.Log("Tapping...");
            if (Time.time >= _holdStart + _holdTime)
            {
                Debug.Log("Slow Tapping...");
                inputs.Enqueue(PlayerInput.SlowTap);
            }
            else
            {
                inputs.Enqueue(PlayerInput.Tap);
            }
            _holdStart = Time.time;
            _windowStarted = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Resetting holdStart");
            _holdStart = Time.time;
        }
        // There can only be one Hold input type in the Queue.
        else if (Input.GetKey(KeyCode.Space) && Time.time >= _holdStart + _holdTime && !inputs.Contains(PlayerInput.Hold))
        {
            Debug.Log("Holding...");
            // A hold will always have a preceding tap.
            if (inputs.Count > 0)
            {
                inputs.Dequeue();
            }
            inputs.Enqueue(PlayerInput.Hold);
            _holdStart = Time.time;
        }
    }
    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    IEnumerator ExecuteInputWindowCoroutine()
    {
        yield return new WaitForSeconds(_inputWindow);
        ProcessInputWindow();
        _windowStarted = false;
        ClearLog();
    }
}

public enum PlayerInput
{
    Tap,
    Hold,
    SlowTap
}