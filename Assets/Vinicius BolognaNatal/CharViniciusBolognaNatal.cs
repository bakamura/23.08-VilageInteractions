using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharViniciusBolognaNatal : CharBase
{
    [Header("Custom Class Variables")]
    [SerializeField, Min(.01f)] private float _movmentSpeed;
    [SerializeField, Min(0f)] private float _distanceFromTargetPlaceTolerance = .5f;
    [SerializeField, Min(1)] private int _maxActionsPossiblePerUpdate;
    [SerializeField] private RandomizationData _customRadomizerRule;
    [SerializeField] private PlaceData[] _newPlacesPrefabs;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool _debugActive;
    [SerializeField, Min(0f)] private float _debugSize;
    [SerializeField] private Color _debugColor;
    [SerializeField] private int[] _stepsDebugTest;
    private int _currentIndex;
#endif
    private Rigidbody2D _rb;
    private List<PlaceData> _currentCreatedPlaces = new List<PlaceData>();
    private List<PlaceData> _currentAvailableToCreatePlaces = new List<PlaceData>();
    private Transform _placesContainer;
    private bool _isBehaviourLoopDone = true;
    private Coroutine _isMoving = null;
    private int _changesRequested;
    private System.Random _random = new System.Random();
    private int _currentWeightValue;

    [Serializable]
    private struct PlaceData
    {
        public GameObject Prefab;
        public string Name;
        public Vector3 Location;
        [HideInInspector] public GameObject InstanceInScene;

        public PlaceData(PlaceData placeData, GameObject instanceInScene)
        {
            Prefab = placeData.Prefab;
            Name = placeData.Name;
            Location = placeData.Location;
            InstanceInScene = instanceInScene;
        }
    }

    private enum RandomizerRuleTypes
    {
        Default,
        WeightToCreation,
        WeightToDestruction
    }

    [Serializable]
    private struct RandomizationData
    {
        public RandomizerRuleTypes RandomizerRule;
        [Min(0)] public int Weight;
    }

    private void Awake()
    {
        _placesContainer = GameObject.Find("Places").transform;
        _rb = GetComponent<Rigidbody2D>();
        _currentAvailableToCreatePlaces = new List<PlaceData>(_newPlacesPrefabs.ToList());
        GameManager.onChangePeriod.AddListener(CharacterUpdate);
    }

    public override void Interact(CharBase charInfo)
    {

    }

    private void CharacterUpdate(int currentPeriod)
    {
        if (_isBehaviourLoopDone) UpdateBehaviour();
    }

    private void MoveTo(PlaceData data, Action<PlaceData> OnDestinationReached)
    {
        if (_isMoving == null)
        {
            _rb.velocity = (data.Location - transform.position).normalized * _movmentSpeed;
            _isMoving = StartCoroutine(CheckDestinationReached(data, OnDestinationReached));
        }
    }

    private IEnumerator CheckDestinationReached(PlaceData data, Action<PlaceData> OnDestinationReached)
    {
        while (Vector2.Distance(transform.position, data.Location) > _distanceFromTargetPlaceTolerance)
        {
            yield return null;
        }
        _rb.velocity = Vector2.zero;
        _isMoving = null;
        OnDestinationReached?.Invoke(data);
    }

    private void CreateNewPlace(PlaceData placeChosen)
    {
        GameObject temp = Instantiate(placeChosen.Prefab, placeChosen.Location, Quaternion.identity, _placesContainer);
        GameManager._placePosition.Add(temp.name, temp.transform.position);
        _currentAvailableToCreatePlaces.Remove(placeChosen);
        _currentCreatedPlaces.Add(new PlaceData(placeChosen, temp));
        _changesRequested--;
        if (_currentAvailableToCreatePlaces.Count > 0 && _changesRequested > 0)
            MoveTo(_currentAvailableToCreatePlaces[GetRandomNumber(0, _currentAvailableToCreatePlaces.Count)], CreateNewPlace);
        else _isBehaviourLoopDone = true;
    }

    private void DestroyCreatedPlace(PlaceData placeChosen)
    {
        GameManager._placePosition.Remove(placeChosen.Name);
        _currentAvailableToCreatePlaces.Add(placeChosen);
        _currentCreatedPlaces.Remove(placeChosen);
        Destroy(placeChosen.InstanceInScene);
        _changesRequested--;
        if (_currentCreatedPlaces.Count > 0 && _changesRequested > 0)
            MoveTo(_currentCreatedPlaces[GetRandomNumber(0, _currentCreatedPlaces.Count)], DestroyCreatedPlace);
        else _isBehaviourLoopDone = true;
    }

    private void UpdateBehaviour()
    {
        _isBehaviourLoopDone = false;
        HandleRandomizerRule();
#if UNITY_EDITOR
        if (_debugActive)
        {
            if (_stepsDebugTest.Length > 0)
            {
                _changesRequested = _stepsDebugTest[_currentIndex];
                _currentIndex++;
            }
            Debug.Log($"changes to happen {_changesRequested}");
        }
#endif
        humor += _changesRequested;
        if (_changesRequested > 0) MoveTo(_currentAvailableToCreatePlaces[GetRandomNumber(0, _currentAvailableToCreatePlaces.Count - 1)], CreateNewPlace);
        else if (_changesRequested < 0 && _currentCreatedPlaces.Count > 0)
        {
            _changesRequested = Math.Abs(_changesRequested);
            MoveTo(_currentCreatedPlaces[GetRandomNumber(0, _currentCreatedPlaces.Count - 1)], DestroyCreatedPlace);
        }
        else _isBehaviourLoopDone = true;
    }

    private void HandleRandomizerRule()
    {
        _changesRequested = GetRandomNumber(-_maxActionsPossiblePerUpdate, _maxActionsPossiblePerUpdate);
        switch (_customRadomizerRule.RandomizerRule)
        {
            case RandomizerRuleTypes.Default:
                break;
            case RandomizerRuleTypes.WeightToCreation:
                if (_changesRequested < 0)
                {
                    _currentWeightValue += _customRadomizerRule.Weight;
                    _changesRequested += _currentWeightValue;
                    if (_changesRequested > 0) _currentWeightValue = 0;
                }
                else if (_changesRequested > 0) _currentWeightValue = 0;
                break;
            case RandomizerRuleTypes.WeightToDestruction:
                if (_changesRequested > 0)
                {
                    _currentWeightValue += _customRadomizerRule.Weight;
                    _changesRequested -= _currentWeightValue;
                    if (_changesRequested < 0) _currentWeightValue = 0;
                }
                else if (_changesRequested < 0) _currentWeightValue = 0;
                break;
        }
    }

    private int GetRandomNumber(int min, int max)
    {
        //return _random.Next(min, max);
        return UnityEngine.Random.Range(min, max);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_newPlacesPrefabs != null && _debugActive)
        {
            for (int i = 0; i < _newPlacesPrefabs.Length; i++)
            {
                if (_newPlacesPrefabs[i].Prefab)
                {
                    Gizmos.color = _debugColor;
                    Gizmos.DrawWireCube(_newPlacesPrefabs[i].Location, _newPlacesPrefabs[i].Prefab.GetComponent<BoxCollider2D>().size);
                    Handles.Label(_newPlacesPrefabs[i].Location, _newPlacesPrefabs[i].Name);
                }
            }
        }
    }

    private void OnValidate()
    {
        if (_newPlacesPrefabs != null)
        {
            for (int i = 0; i < _newPlacesPrefabs.Length; i++)
            {
                if (_newPlacesPrefabs[i].Prefab)
                {
                    _newPlacesPrefabs[i].Name = _newPlacesPrefabs[i].Prefab.name;
                    _newPlacesPrefabs[i].Location = _newPlacesPrefabs[i].Prefab.transform.position;
                }
            }
            PlaceData[] validValues = _newPlacesPrefabs.Where(x => x.Prefab != null).ToArray();

            Vector3[] locations = validValues.Select(x => x.Location).ToArray();
            string[] names = validValues.Select(x => x.Name).ToArray();

            Vector3[] disticntLocations = locations.Distinct().ToArray();

            if (disticntLocations.Length != locations.Length)
            {
                Debug.LogWarning("there are Location Conflicts with the recent adition");
            }
            if (names.Distinct().Count() != names.Length)
            {
                Debug.LogWarning("there are Name Conflicts with the recent adition");
            }
        }
    }
#endif
}
