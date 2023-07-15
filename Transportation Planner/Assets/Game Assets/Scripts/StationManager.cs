using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationManager : MonoBehaviour
{
    public Mode currentMode;
    [ReadOnly] public CarSpawner currentCarSelected;
    [ReadOnly] public BusSpawner currentBusSelected;
    [ReadOnly] public List<BusSpawner> busStations;
    public enum Mode { Pointer, BusEditor }

    [HorizontalLine]
    public Toggle showCarPathToggle;
    public Toggle showBusPathToggle;

    [HorizontalLine]
    public GameObject stationInfoUI;
    public Button deleteButton;
    public TMP_Text infoTitle;
    public TMP_Text carCountText;
    public TMP_Text peopleCountText;
    public Image vehicleImage;
    public Sprite carIcon;
    public Sprite busIcon;

    [Space(20)]
    public Image modeIdentifierImage;
    public Sprite pointerIcon;
    public Sprite busEditorIcon;
    public TMP_Text instructionText;
    public string pointerInstruction;
    public string busInstruction;

    Camera mainCamera;
    public static StationManager Instance;

    void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    void Start()
    {
        stationInfoUI.SetActive(false);
    }

    void Update()
    {
        busStations.RemoveAll(item => item == null);
        stationInfoUI.SetActive(currentCarSelected != null || currentBusSelected != null);
        CheckForStationClicks();
    }

    void CheckForStationClicks()
    {
        modeIdentifierImage.sprite = currentMode == Mode.Pointer? pointerIcon : busEditorIcon;
        instructionText.text = currentMode == Mode.Pointer? pointerInstruction : busInstruction;
        if (Input.GetMouseButtonDown(0) && currentMode == Mode.Pointer)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                Collider collider = hit.collider;
                CarSpawner carSpawner = collider.GetComponent<CarSpawner>();
                BusSpawner busSpawner = collider.GetComponent<BusSpawner>();
                Node node = collider.GetComponent<Node>();

                if (carSpawner != null)
                {
                    currentCarSelected = carSpawner;
                    vehicleImage.sprite = carIcon;

                    deleteButton.interactable = false;
                    currentBusSelected = null;
                }
                if (busSpawner != null)
                {
                    currentBusSelected = busSpawner;
                    vehicleImage.sprite = busIcon;

                    deleteButton.interactable = true;
                    currentCarSelected = null;
                }
            }
        }

        if(currentCarSelected != null)
        {
            carCountText.text = currentCarSelected.currCars + "/" + currentCarSelected.maxCars;
            peopleCountText.text = currentCarSelected.currPeople + "/" + currentCarSelected.maxPeople;

            infoTitle.text = "Car Station";
        }

        if(currentBusSelected != null)
        {
            carCountText.text = currentBusSelected.currBuses + "/" + currentBusSelected.maxBuses;

            if(currentBusSelected.nearestCarSpawner != null)
                peopleCountText.text = currentBusSelected.nearestCarSpawner.currPeople + "/" + currentBusSelected.nearestCarSpawner.maxPeople;

            infoTitle.text = "Bus Station";
        }
    }

    public void ResetSelected(){ currentCarSelected = null; currentBusSelected = null; }
    public void ChangeToBusEditorMode() => currentMode = Mode.BusEditor;
    public void ChangeToPointerMode() => currentMode = Mode.Pointer;

    public void DeleteSelectedBus()
    {
        if(currentBusSelected != null)
        {
            busStations.Remove(currentBusSelected);
            Destroy(currentBusSelected.gameObject);
        }
    }
}
