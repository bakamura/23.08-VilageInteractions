using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

    [Header("Timer")]

    public int _periodCurrent = -1;
    [SerializeField] private int _periodTotal;
    [SerializeField] private float _periodDuration = 5.0f;
    private float _periodCurrentInterval;
    [SerializeField] private Text _periodDisplay;

    public static UnityEvent<int> onChangePeriod = new UnityEvent<int>();

    [Header("Places")]

    [SerializeField] private Transform _placesContainer;
    public static Dictionary<string, Vector3> _placePosition = new Dictionary<string, Vector3>();

    void Awake() {
        _periodDisplay.text = "Starting the Day";
        for (int i = 0; i < _placesContainer.childCount; i++) _placePosition.Add(_placesContainer.GetChild(i).name, _placesContainer.GetChild(i).position);
    }

    private void Start() {
    }

    private void Update() {
        _periodCurrentInterval += Time.deltaTime;

        if (_periodCurrentInterval >= _periodDuration) {
            _periodCurrentInterval -= _periodDuration;
            TimeSkip();
        }
    }

    private void TimeSkip() {
        if (_periodCurrent < _periodTotal) {
            _periodCurrent ++;
            onChangePeriod.Invoke(_periodCurrent);
            _periodDisplay.text = $"{_periodCurrent}:00";

        }
    }

    public float GetRealTimeSinceStart() {
        return (_periodCurrent * _periodDuration) + _periodCurrentInterval;
    }

}
