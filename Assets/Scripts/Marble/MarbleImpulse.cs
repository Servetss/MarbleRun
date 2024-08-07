﻿using UnityEngine;

public enum BoostType { TestBoost, BoostZonePhysics, BoostZoneSpline}

public class MarbleImpulse : MonoBehaviour
{
    private IBoost _boost;

    [SerializeField] private Ability _accelerateAbility;

    [SerializeField] private GameObject _boostTextOnUI;

    [SerializeField] private BoostСharacteristic _boostСharacteristic;

    private EventMachine _playerEventMachine;

    private Animator _animator;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();

        _animator = GetComponent<Animator>();

        _playerEventMachine = GetComponent<EventMachine>();
        
        _boost = _boostСharacteristic.GetSelectBoostType();

        _boost.Init(this);
    }

    private void Start()
    {
        _playerEventMachine?.SubscribeOnMoveToNextLevel(DisableJumpCamera);
    }

    public Rigidbody Rigidbody { get; private set; }

    private void Update()
    {
        if (_boost.BoostCondition())
        {
            _boost.Impulse(Rigidbody, _boostСharacteristic.Impulse + _accelerateAbility.Boost);
        }
    }

    public void OnPlayerMeshTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BoostZone")
        {
            _animator.SetBool("IsBoostZone", true);

            _boostTextOnUI.SetActive(true);

            _playerEventMachine.BoostZoneStartMethod();

            _boost.EnterTrigger();
        }
    }

    public void OnPlayerMeshTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BoostZone")
        {
            _animator.SetBool("IsBoostZone", false);

            _animator.SetBool("IsJump", true);

            _boostTextOnUI.SetActive(false);

            _playerEventMachine.BoostZoneFinishMethod();

            _boost.ExiteTrigger();
        }
    }

    private void DisableJumpCamera()
    {
        _animator.SetBool("IsJump", false);
    }
}