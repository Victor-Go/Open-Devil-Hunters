using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class GuideUI : UIAnimation
  {
    [SerializeField] private GameObject previousButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private List<GameObject> pages;

    private int page;

    protected override void Start()
    {
      base.Start();

      pages.ForEach(p => p.SetActive(false));
      pages.FirstOrDefault()?.SetActive(true);
      previousButton?.SetActive(false);
      nextButton?.SetActive(pages.Count > 1);
    }

    private void updateButtonActivity()
    {
      previousButton?.SetActive(page != 0);
      nextButton?.SetActive(page != pages.Count - 1);
    }

    private void updatePage()
    {
      pages.ForEach(p => p.SetActive(false));
      pages[page].SetActive(true);
    }

    public void GoPrevious()
    {
      if (page == 0) return;
      page--;

      updatePage();
      updateButtonActivity();
    }

    public void GoNext()
    {
      if (page == pages.Count - 1) return;
      page++;

      updatePage();
      updateButtonActivity();
    }
  }
}