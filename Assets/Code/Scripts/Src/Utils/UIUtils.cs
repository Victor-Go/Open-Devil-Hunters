using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Code.Scripts.Src.Utils
{
    public static class UIUtils
    {
        public static IEnumerator SetSelectedGameObject(GameObject go)
        {
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(go);
        }
    }
}
