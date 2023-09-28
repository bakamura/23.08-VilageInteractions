using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Icognito : CharBase {

    [Header("Behaviour")]

    private InteractionType _interactionTypeCurrent;
    private enum InteractionType {
        Hurt,
        Heal,
        Neutral
    }
    private int _charInteractedToday;

    [Header("Movement")]

    [SerializeField] private float _movementSpeed;
    private Transform _movementFollowTargetHurt;
    private Transform _movementFollowTargetHeal;

    [Header("Cache")]

    private Rigidbody2D _rb;
    private Timer _timeManager;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        _timeManager = FindObjectOfType<Timer>();
    }

    private void Update() {
        switch (_timeManager.periodo) {
            case 0:
                _interactionTypeCurrent = humor > 0.5f ? InteractionType.Hurt : InteractionType.Heal;
                break;
            case 1:
                // Nothing
                break;
            case 2:
                // Nothing
                break;
            case 3:
                if (_charInteractedToday < 4) {
                    _interactionTypeCurrent = InteractionType.Neutral;
                    Follow(transform); // Change to park
                }
                else {
                    _interactionTypeCurrent = InteractionType.Hurt;
                    Follow(_movementFollowTargetHurt);
                }
                    break;
            case 4:
                if (_charInteractedToday < 4) {
                    _interactionTypeCurrent = InteractionType.Neutral;
                    Follow(transform); // Change to park
                }
                else {
                    _interactionTypeCurrent = InteractionType.Hurt;
                    Follow(_movementFollowTargetHurt);
                }
                break;
            case 5:
                if (_charInteractedToday < 7) {
                    _interactionTypeCurrent = InteractionType.Neutral;
                    Follow(transform); // Change to park
                }
                else {
                    _interactionTypeCurrent = InteractionType.Heal;
                    Follow(_movementFollowTargetHeal);
                }
                break;
            case 6:
                if (_charInteractedToday < 7) {
                    _interactionTypeCurrent = InteractionType.Neutral;
                    Follow(transform); // Change to park
                }
                else {
                    _interactionTypeCurrent = InteractionType.Heal;
                    Follow(_movementFollowTargetHeal);
                }
                break;
            case 7:

                break;
            case 8:

                break;
            case 9:

                break;
            case 10:

                break;
            case 11:

                break;

        }
    }

    private void OnTriggerEnter(Collider other) {
        _charInteractedToday++;
        if (_charInteractedToday == 4) _movementFollowTargetHurt = other.transform;
        else if (_charInteractedToday == 7) _movementFollowTargetHeal = other.transform;
        switch (_interactionTypeCurrent) {
            case InteractionType.Hurt:
                // Manipulate Charbase Stats
                other.GetComponent<CharBase>().Interact(this);
                break;
            case InteractionType.Heal:
                // Manipulate Charbase Stats
                other.GetComponent<CharBase>().Interact(this);
                break;
            case InteractionType.Neutral:
                
                break;
        }
    }

    public override void Interact(CharBase charInfo) {

    }

    public void Follow(Transform other) {
        _rb.velocity = (other.position - transform.position).normalized * _movementSpeed;
    }

}
