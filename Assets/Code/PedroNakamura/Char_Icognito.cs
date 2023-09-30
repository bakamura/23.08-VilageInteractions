using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Char_Icognito : CharBase {

    [Header("Behaviour")]

    [SerializeField] private float _trueHumor; // Serializefor visibility
    private int _charInteractedToday;
    private InteractionType _interactionTypeCurrent;
    private enum InteractionType {
        Hurtful,
        Helpful,
        Neutral
    }

    [SerializeField] private Transform _friend;
    [SerializeField] private Transform _acquaitance;
    [SerializeField] private Transform _nemesis;

    [Header("Movement")]

    [SerializeField] private float _movementSpeed;
    private Transform _followTargetCurrent;
    private Vector3 _followPlaceCurrent;
    private Transform _movementFollowTargetHurt;
    private Transform _movementFollowTargetHeal;

    [Header("Cache")]

    private Rigidbody2D _rb;
    private GameManager _gameManager;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        _gameManager = FindObjectOfType<GameManager>();
        GameManager.onChangePeriod.AddListener(ChangeBehaviuor);
    }

    private void Update() {
        Follow(_followTargetCurrent != null ? _followTargetCurrent.position : _followPlaceCurrent);
    }

    private void OnTriggerEnter(Collider other) {
        _charInteractedToday++;
        if (_charInteractedToday == 4) _movementFollowTargetHurt = other.transform;
        else if (_charInteractedToday == 7) _movementFollowTargetHeal = other.transform;

        gender = RandomizeGender(Random.Range(0, 3));
        age = (uint)Random.Range(0, 100);
        race = RandomizeRace(Random.Range(0, 4));

        switch (_interactionTypeCurrent) {
            case InteractionType.Hurtful:
                money = RandomizePurse(Random.Range(1, 3));
                humor = Random.Range(0f, 0.25f);
                persona = RandomizePersona(Random.Range(0, 3));
                break;
            case InteractionType.Helpful:
                money = RandomizePurse(Random.Range(0, 2));
                humor = Random.Range(0.75f, 1f);
                persona = RandomizePersona(Random.Range(3, 6));
                break;
            case InteractionType.Neutral:
                money = MoneyT.Medium;
                humor = 0.5f;
                persona = PersonalityT.Shy;
                break;
        }
        other.GetComponent<CharBase>().Interact(this);
    }

    private void ChangeBehaviuor(int periodCurrent) {
        switch (periodCurrent) {
            case 0:
                _interactionTypeCurrent = _trueHumor > 0.5f ? InteractionType.Hurtful : InteractionType.Helpful;
                break;
            case 1:
                _interactionTypeCurrent = InteractionType.Neutral;
                break;
            case 2:
                _interactionTypeCurrent = InteractionType.Neutral;
                break;
            case 3:
                if (_charInteractedToday < 4) {
                    _interactionTypeCurrent = InteractionType.Neutral;
                    _followTargetCurrent = null;
                    _followPlaceCurrent = _gameManager._placePosition["TownSquare"];
                }
                else {
                    _interactionTypeCurrent = InteractionType.Hurtful;
                    _followTargetCurrent = null;
                    _followPlaceCurrent = _movementFollowTargetHurt.position;
                }
                break;
            case 4:
                if (_charInteractedToday < 4) {
                    _interactionTypeCurrent = InteractionType.Neutral;
                    _followTargetCurrent = null;
                    _followPlaceCurrent = _gameManager._placePosition["TownSquare"];
                }
                else {
                    _interactionTypeCurrent = InteractionType.Hurtful;
                    _followTargetCurrent = null;
                    _followPlaceCurrent = _movementFollowTargetHurt.position;
                }
                break;
            case 5:
                if (_charInteractedToday < 7) {
                    _interactionTypeCurrent = InteractionType.Neutral;
                    _followTargetCurrent = null;
                    _followPlaceCurrent = _gameManager._placePosition["TownSquare"];
                }
                else {
                    _interactionTypeCurrent = InteractionType.Helpful;
                    _followTargetCurrent = null;
                    _followPlaceCurrent = _movementFollowTargetHeal.position;
                }
                break;
            case 6:
                if (_charInteractedToday < 7) {
                    _interactionTypeCurrent = InteractionType.Neutral;
                    _followTargetCurrent = null;
                    _followPlaceCurrent = _gameManager._placePosition["TownSquare"];
                }
                else {
                    _interactionTypeCurrent = InteractionType.Helpful;
                    _followTargetCurrent = null;
                    _followPlaceCurrent = _movementFollowTargetHeal.position;
                }
                break;
            case 7:
                _followTargetCurrent = null;
                // Will Cause unexpected behaviour
                _followPlaceCurrent = GetFarthestFrom(_movementFollowTargetHurt, _gameManager._placePosition["Bakery"], _gameManager._placePosition["Bar"]);
                break;
            case 8:
                _followTargetCurrent = null;
                _followPlaceCurrent = _gameManager._placePosition["TownSquare"];
                break;
            case 9:
                switch (Random.Range(0, 3)) {
                    case 0:
                        _followTargetCurrent = _friend;
                        _interactionTypeCurrent = InteractionType.Helpful;
                        break;
                    case 1:
                        _followTargetCurrent = _acquaitance;
                        _interactionTypeCurrent = InteractionType.Neutral;
                        break;
                    case 2:
                        _followTargetCurrent = _nemesis;
                        _interactionTypeCurrent = InteractionType.Hurtful;
                        break;
                }
                break;
            case 10:
                switch (Random.Range(0, 3)) {
                    case 0:
                        _followTargetCurrent = _friend;
                        _interactionTypeCurrent = InteractionType.Helpful;
                        break;
                    case 1:
                        _followTargetCurrent = _acquaitance;
                        _interactionTypeCurrent = InteractionType.Neutral;
                        break;
                    case 2:
                        _followTargetCurrent = _nemesis;
                        _interactionTypeCurrent = InteractionType.Hurtful;
                        break;
                }
                break;
            case 11:
                _interactionTypeCurrent = _trueHumor > 0.5f ? InteractionType.Hurtful : InteractionType.Helpful;
                break;

        }
    }

    public override void Interact(CharBase charInfo) {
        _trueHumor = Mathf.Clamp01(_trueHumor + (charInfo.Humor > 0.5f ? -0.1f : 0.1f));
    }

    private void Follow(Vector3 other) {
        _rb.velocity = (other - transform.position).normalized * _movementSpeed;
    }

    private Vector3 GetFarthestFrom(Transform from, Vector3 place1, Vector3 place2) {
        return Vector3.Distance(from.position, place1) > Vector3.Distance(from.position, place2) ? place1 : place2;
    }

    private GenderT RandomizeGender(int i) {
        switch (i % 3) {
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

}
