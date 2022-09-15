using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buildings;
using UnitComponents;

namespace RTS.Controller
{
    public class UnitPicker : MonoBehaviour
    {
        [SerializeField] private ResourceController resourceController;

        [SerializeField] private RectTransform unitSelectionArea = null;
        [SerializeField] private LayerMask layerMask = new LayerMask();
        [SerializeField] private Barracks barracks;

        private Vector2 startPosition;
        private List<Unit> selectedUnits;

        private void Awake()
        {
            selectedUnits = new List<Unit>();
            unitSelectionArea.gameObject.SetActive(false);
        }

        void Update()
        {
            // Interaction when mouse button is pressed down
            if (Input.GetMouseButtonDown(0))
            {
                StartUnitSelectionArea();
            }

            // Action when mouse button is held down
            if (Input.GetMouseButton(0))
            {
                DragUnitSelectionArea();
            }

            // Action when mouse button is released
            if (Input.GetMouseButtonUp(0))
            {
                EndUnitSelectionArea();
            }
        }

        private void StartUnitSelectionArea()
        {
            RaycastHit hit;
            Physics.Raycast(GetMouseRay(), out hit);
            startPosition = Input.mousePosition;

            unitSelectionArea.gameObject.SetActive(true);
            DragUnitSelectionArea();
        }

        private void DragUnitSelectionArea()
        {
            RaycastHit hit;
            Physics.Raycast(GetMouseRay(), out hit);

            Vector2 mousePosition = Input.mousePosition;

            float areaWidth = mousePosition.x - startPosition.x;
            float areaHeight = mousePosition.y - startPosition.y;

            unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
            unitSelectionArea.anchoredPosition = startPosition + new Vector2(-10, -5) + new Vector2(areaWidth / 2, areaHeight / 2);
        }

        private void EndUnitSelectionArea()
        {
            unitSelectionArea.gameObject.SetActive(false);
            foreach (Unit unit in selectedUnits)
            {
                unit.SetSelectedVisible(false);
            }
            selectedUnits.Clear();

            // Select unit if it's only single click without dragging
            if (unitSelectionArea.sizeDelta.magnitude == 0)
            {
                RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
                foreach (RaycastHit rayHit in hits)
                {
                    Unit unit = rayHit.collider.GetComponent<Unit>();
                    if (unit && unit.canBeControlled)
                    {
                        selectedUnits.Add(unit);
                        unit.SetSelectedVisible(true);
                    }
                }

                foreach (Unit selectedUnit in selectedUnits)
                {
                    selectedUnit.SetSelectedVisible(true);
                }

                return;
            }

            // Add all of the units inside the selection area to the list of selected units
            Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
            Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);

            List<Unit> listOfUnitsInWorld = resourceController.GetUnitList();

            foreach (Unit unit in listOfUnitsInWorld)
            {
                if (selectedUnits.Contains(unit)) { continue; }

                Vector3 screenPosition = Camera.main.WorldToScreenPoint(unit.transform.position);

                if (screenPosition.x > min.x &&
                    screenPosition.x < max.x &&
                    screenPosition.y > min.y &&
                    screenPosition.y < max.y &&
                    unit.canBeControlled)
                {
                    selectedUnits.Add(unit);
                    unit.SetSelectedVisible(true);
                }
            }
        }

        // Shot the raycast from the camera
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}