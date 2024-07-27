using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HerbUIManager : MonoBehaviour
{

    public GameObject uiObject;

    public int yMargin = 20;

    private List<GameObject> herbUIs = new List<GameObject>();

    private float height;

    // Start is called before the first frame update
    void Start()
    {

        HerbManager herbs = GameObject.Find("Herb Manager").GetComponent<HerbManager>();

        if (herbs != null)
        {
            foreach (HerbVars herb in herbs.herbs)
            {
                // create UI component
                GameObject herbUI = Instantiate(uiObject, transform);

                //hide component till revealed
                herbUI.SetActive(false);

                // add to ui list
                herbUIs.Add(herbUI);

                // Set picture
                herbUI.GetComponent<Image>().sprite = herb.sprite;
            }
        }

        // get height of each ui element
        height = herbUIs[0].GetComponent<RectTransform>().rect.height;

        UpdateUI(herbs);
    }


    public void UpdateUI(HerbManager manager)
    {
        float yPos = yMargin;

        for (int i=0; i<herbUIs.Count; i++)
        {
            GameObject herbUI = herbUIs[i];
            Text textComp = herbUI.GetComponentInChildren<Text>();
            int newValue = (int)manager.herbs[i].inventoryNum;
            textComp.text = newValue.ToString();

            if (newValue > 0)
            {
                herbUI.SetActive(true);

                Vector3 position = herbUI.GetComponent<RectTransform>().position;
                position.y = yPos;
                herbUI.GetComponent<RectTransform>().position = position;

                yPos += yMargin + height;
            } 
        }
    }
}
