using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pagination : MonoBehaviour
{
    public GameObject[] pages;
    public GameObject nextButton, prevButton;
    private int page;
    private void Start()
    {
        ResetPages();
    }
    private void AddButtons()
    {
        if (page < pages.Length-1)
        {
            nextButton.SetActive(true);
        }
        else
        {
            nextButton.SetActive(false);
        }
        if (page != 0)
        {
            prevButton.SetActive(true);
        }
        else
        {
            prevButton.SetActive(false);
        }
    }
    public void OnNextPage()
    {
        pages[page].SetActive(false);
        page++;
        pages[page].SetActive(true);
        AddButtons();
    }
    public void OnPrevPage()
    {
        pages[page].SetActive(false);
        page--;
        pages[page].SetActive(true);
        AddButtons();
    }
    public void ResetPages()
    {
        TurnOffPages();
        pages[0].SetActive(true);
        page = 0;
        AddButtons();
    }
    private void TurnOffPages()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
    }
}
