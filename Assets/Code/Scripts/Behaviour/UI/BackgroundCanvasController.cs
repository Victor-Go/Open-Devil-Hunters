using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCanvasController : MonoBehaviour, IStoreChangedHandler
{
  private StoreManager storeManager;
  private GameObject backgroundGameObject;

  private void Awake()
  {
    storeManager = StoreManager.Instance;

    backgroundGameObject = transform.Find("Background").gameObject;
  }

  private void Start()
  {
    storeManager.Subscribe(StoreNames.BackgroundCanvas, this);
  }

  public void OnStoreChanged(StoreNames storeName, IState state)
  {
    switch (storeName)
    {
      case StoreNames.BackgroundCanvas:
        var backgrondState = (BackgroundCanvasState)state;
        backgroundGameObject.SetActive(backgrondState.Active);
        break;
    }
  }

  private void OnDestroy()
  {
    storeManager.Unsubscribe(this);
  }
}