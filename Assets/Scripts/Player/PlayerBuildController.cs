using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBuildController : MonoBehaviour
{
    [SerializeField] private float moveTimeout = 0.1f;

    public PlayerController Player { get; private set; } = null;
    // public WorldGrid WorldGrid { get; private set; } = null;

    public GridElement CurrentGridElement { get; private set; } = null;
    public Placeable CurrentPlaceable { get; private set; } = null;
    public GameObject CurrentPlaceableVizualizer { get; private set; } = null;
    public Animator VizualizerAnimator { get; private set; } = null;
    public List<GridElement> CurrentPlaceableGridElements { get; private set; } = new List<GridElement>();

    float lastMove = 0f;

    public void Init(PlayerController _player)
    {
        this.Player = _player;
        // this.WorldGrid = WorldGrid.Instance;
    }

    public void FinalPlaceObject(InputAction.CallbackContext _context)
    {
        if (!CurrentPlaceable || !CurrentPlaceableVizualizer) return;

        if (_context.performed)
        {
            if (CurrentPlaceable &&
            CurrentGridElement &&
            WorldGrid.Instance.IsObjectPlaceable(CurrentPlaceable, CurrentPlaceableGridElements))
            {
                Vector3 objectOffset = WorldGrid.Instance.GetObjectOffset(CurrentPlaceable);

                GameObject newObject = Instantiate(CurrentPlaceable.prefab, CurrentGridElement.transform.position + objectOffset, Quaternion.identity);

                foreach (GridElement gridElement in CurrentPlaceableGridElements)
                {
                    gridElement.ObjectOnGrid = newObject;
                }

                Destroy(CurrentPlaceableVizualizer);

                CurrentPlaceableVizualizer = null;
                CurrentPlaceable = null;
                CurrentPlaceableGridElements.Clear();

                Player.Nekromancer.ResetInteractable();

                Player.StartCombatMode();

                LevelManager.Instance.StartTime();
            }
        }
    }

    public void PlaceObject(Placeable _object)
    {
        LevelManager.Instance.StopTime();

        CurrentPlaceable = _object;
        Vector3 objectOffset = WorldGrid.Instance.GetObjectOffset(CurrentPlaceable);

        CurrentGridElement = WorldGrid.Instance.GetGridElement(Player.Nekromancer.transform.position);

        CurrentPlaceableVizualizer = Instantiate(CurrentPlaceable.preview, CurrentGridElement.transform.position + objectOffset, Quaternion.identity);
        VizualizerAnimator = CurrentPlaceableVizualizer.GetComponent<Animator>();

        CurrentPlaceableVizualizer.transform.position = Player.Nekromancer.transform.position;

        Player.BuildingCamera.Follow = CurrentPlaceableVizualizer.transform;

        Player.StartBuildingMode();
    }

    public void MovePlaceable(InputAction.CallbackContext _context)
    {
        LevelManager.Instance.StopTime();

        if (!CurrentPlaceable || !CurrentPlaceableVizualizer || (Time.time - lastMove) < moveTimeout) return;

        lastMove = Time.time;

        Vector2 input = _context.ReadValue<Vector2>();

        if (input.x > 0.5f) input.x = 1;
        else if (input.x < -0.5f) input.x = -1;
        else input.x = 0;

        if (input.y > 0.5f) input.y = 1;
        else if (input.y < -0.5f) input.y = -1;
        else input.y = 0;

        CurrentGridElement = WorldGrid.Instance.GetGridElement(CurrentGridElement, input);

        if (CurrentPlaceable)
        {
            Vector3 objectOffset = WorldGrid.Instance.GetObjectOffset(CurrentPlaceable);

            CurrentPlaceableVizualizer.transform.position = CurrentGridElement.transform.position + objectOffset;

            List<GridElement> newGridElements = WorldGrid.Instance.GetGridElements(CurrentGridElement.transform.position, CurrentPlaceable.size);

            bool isPlaceable = WorldGrid.Instance.IsObjectPlaceable(CurrentPlaceable, newGridElements);

            VizualizerAnimator.SetBool("IsPlaceable", isPlaceable);

            CurrentPlaceableGridElements.Clear();

            CurrentPlaceableGridElements = newGridElements;
        }
    }

    public void Cancel(InputAction.CallbackContext _context)
    {
        if (!CurrentPlaceable || !CurrentPlaceableVizualizer) return;

        if (_context.performed)
        {
            Destroy(CurrentPlaceableVizualizer);

            CurrentPlaceableVizualizer = null;
            CurrentPlaceable = null;
            CurrentPlaceableGridElements.Clear();

            Player.Nekromancer.ResetInteractable();

            Player.StartCombatMode();

            LevelManager.Instance.StartTime();
        }
    }
}
