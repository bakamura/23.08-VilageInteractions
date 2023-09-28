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
    [SerializeField] private float _trueHumor; // Serializefor visibility

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
                _interactionTypeCurrent = _trueHumor > 0.5f ? InteractionType.Hurt : InteractionType.Heal;
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
        gender = RandomizeGender(Random.Range(0, 3));
        age = (uint)Random.Range(0, 100);
        race = RandomizeRace(Random.Range(0, 4));
        money = RandomizePurse(Random.Range(0, 3));
        humor = Random.Range(0f, 1f);
        persona = RandomizePersona(Random.Range(0, 6));

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

    private GenderT RandomizeGender(int i) {
        switch(i % 3) {
            case 0: return GenderT.Male;
            case 1: return GenderT.Female;
            case 2: return GenderT.Other;
            default:
                Debug.LogError("RandomGender: Somehow i % 3 != {0 ~ 2}");
                return GenderT.Other;
        }
    }

    private RaceT RandomizeRace(int i) {
        switch (i % 4) {
            case 0: return RaceT.Human;
            case 1: return RaceT.Animal;
            case 2: return RaceT.Spirit;
            case 3: return RaceT.NonHuman;
            default:
                Debug.LogError("RandomRace: Somehow i % 4 != {0 ~ 3}");
                return RaceT.NonHuman;
        }
    }

    private MoneyT RandomizePurse(int i) {
        switch (i % 3) {
            case 0: return MoneyT.Rich;
            case 1: return MoneyT.Medium;
            case 2: return MoneyT.Poor;
            default:
                Debug.LogError("RandomPurse: Somehow i % 4 != {0 ~ 2}");
                return MoneyT.Rich;
        }
    }

    private PersonalityT RandomizePersona(int i) {
        switch (i % 6) {
            case 0: return PersonalityT.Sadistic;
            case 1: return PersonalityT.Grumpy;
            case 2: return PersonalityT.Loud;
            case 3: return PersonalityT.Shy;
            case 4: return PersonalityT.Kind;
            case 5: return PersonalityT.Flirty;
            default:
                Debug.LogError("RandomPersona: Somehow i % 6 != {0 ~ 5}");
                return PersonalityT.Sadistic;
        }
    }

    public override void Interact(CharBase charInfo) {

    }

    public void Follow(Transform other) {
        _rb.velocity = (other.position - transform.position).normalized * _movementSpeed;
    }

}
