using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selecting_Dictionary : MonoBehaviour
{
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    //add item to our dictionary
    public void addSelected(GameObject find)
    {
        int id = find.GetInstanceID();

        if (!(selectedTable.ContainsKey(id)))
        {
            selectedTable.Add(id, find);
            find.AddComponent<Selection_Component>();
            Debug.Log("Added " + id + " to selected dict");
        }
    }

    public void deselect(int id)
    {
        Destroy(selectedTable[id].GetComponent<Selection_Component>());
        selectedTable.Remove(id);
    }

    //in case we use drag to select troops allow for deselection
    public void deselectAll()
    {
        foreach (KeyValuePair<int, GameObject> pair in selectedTable)
        {
            if (pair.Value != null)
            {
                Destroy(selectedTable[pair.Key].GetComponent<Selection_Component>());
            }
        }
        selectedTable.Clear();
    }

    public List<GameObject> GetSelectedTroops()
    {
        return new List<GameObject>(selectedTable.Values);
    }
}