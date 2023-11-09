using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CharViniciusBolognaNatal : CharBase
{
    [SerializeField] private float _movmentSpeed;
    [SerializeField] private PlaceData[] _newPlacesPrefabs;
    [SerializeField] private float _distanceFromTargetPlaceTolerance = .5f;

    private List<PlaceData> _currentAvailablePlacesToGo = new List<PlaceData>();
    private Rigidbody2D _rb;
    private PlaceData _currentPlaceToGo;
    private List<PlaceData> _currentCreatedPlaces = new List<PlaceData>();
    private List<PlaceData> _currentAvailableToCreatePlaces = new List<PlaceData>();
    private const int _maxHumorChange = 2;
    private Transform _placesContainer;
    private bool _isBehaviourLoopDone = true;
    private Coroutine _isMoving = null;

    private void Awake()
    {
        _placesContainer = GameObject.Find("Places").transform;
        _rb = GetComponent<Rigidbody2D>();
        _currentAvailableToCreatePlaces = new List<PlaceData>(_newPlacesPrefabs.ToList());

        string[] placeNames = GameManager._placePosition.Keys.ToArray();
        Vector3[] placeLocations = GameManager._placePosition.Values.ToArray();
        int rand = GetRandomNumber(0, placeNames.Length);
        _currentAvailablePlacesToGo.Add(new PlaceData(placeNames[rand], placeLocations[rand]));
        GameManager.onChangePeriod.AddListener(ChangePlace);
    }

    [Serializable]
    private struct PlaceData
    {
        public GameObject Instance;
        public string Name;
        public Vector2 Location;

        public PlaceData(string name, Vector2 location, GameObject instance = null)
        {
            Name = name;
            Location = location;
            Instance = instance;
        }
    }

    public override void Interact(CharBase charInfo)
    {

    }

    private void ChangePlace(int currentPeriod)
    {
        if (_isBehaviourLoopDone) MoveTo(ChoseRandomPlace());
    }

    private void MoveTo(PlaceData data)
    {
        if(_isMoving == null)
        {
            _currentPlaceToGo = data;
            _rb.velocity = data.Location.normalized * _movmentSpeed;
            _isMoving = StartCoroutine(CheckDestinationReached());
        }
    }

    private IEnumerator CheckDestinationReached()
    {
        while (Vector2.Distance(transform.position, _currentPlaceToGo.Location) > _distanceFromTargetPlaceTolerance)
        {
            yield return null;
        }
    }

    private void CreateNewPlace()
    {
        if (_currentAvailableToCreatePlaces.Count > 0)
        {
            PlaceData prefadChosen = _currentAvailableToCreatePlaces[GetRandomNumber(0, _currentAvailableToCreatePlaces.Count)];
            GameObject temp = Instantiate(prefadChosen.Instance, prefadChosen.Location, Quaternion.identity, _placesContainer);
            GameManager._placePosition.Add(temp.name, temp.transform.position);
            _currentAvailableToCreatePlaces.Remove(prefadChosen);
            _currentCreatedPlaces.Add(prefadChosen);
            _currentAvailablePlacesToGo.Add(prefadChosen);
        }
    }

    private void DestroyCreatedPlace()
    {
        if (_currentCreatedPlaces.Count > 0)
        {
            PlaceData prefadChosen = _currentCreatedPlaces[GetRandomNumber(0, _currentCreatedPlaces.Count)];
            GameManager._placePosition.Remove(prefadChosen.Name);
            _currentAvailableToCreatePlaces.Add(prefadChosen);
            _currentCreatedPlaces.Remove(prefadChosen);
            _currentAvailablePlacesToGo.Remove(prefadChosen);
        }
    }

    private PlaceData ChoseRandomPlace()
    {
        _currentPlaceToGo = _currentAvailablePlacesToGo[GetRandomNumber(0, _currentAvailablePlacesToGo.Count)];
        return _currentPlaceToGo;
    }

    private void UpdateBehaviour()
    {
        int changeInHumor = GetRandomNumber(-_maxHumorChange, _maxHumorChange);
        for (int i = 0; i < Mathf.Abs(changeInHumor); i++)
        {
            if (changeInHumor > 0) CreateNewPlace();
            else DestroyCreatedPlace();
        }
        humor += changeInHumor;
    }

    private int GetRandomNumber(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == _currentPlaceToGo.Name && _isBehaviourLoopDone)
        {
            _isBehaviourLoopDone = false;
            _rb.velocity = Vector2.zero;
            UpdateBehaviour();
        }
    }
}
