using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuAnimation : MonoBehaviour
{
    public GameObject energyPlanet;
    public GameObject spacePigPlanet;
    public GameObject metalPlanet;
    public GameObject waterPlanet;
    public GameObject darkMatterPlanet;

    public Transform planetsContainer;
    public GameObject title;

    // Start is called before the first frame update
    void Start()
    {
        energyPlanet.SetActive(true);
        StartCoroutine(ShowPlanets());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ShowPlanets() {

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 100; i++) {

            planetsContainer.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            if (title.GetComponent<TMP_Text>().characterSpacing < 0) title.GetComponent<TMP_Text>().characterSpacing += 1.07f;
            yield return new WaitForSeconds(0.03f);
        }
        //spacePigPlanet.SetActive(true);
        //metalPlanet.SetActive(true);
        //waterPlanet.SetActive(true);
        //darkMatterPlanet.SetActive(true);

    }
}
