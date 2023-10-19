using Pancake.Apex;
using Pancake.LevelSystem;
using Pancake.Monetization;
using Pancake.Scriptable;
using Pancake.Sound;
using Pancake.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Pancake.SceneFlow
{
    [EditorIcon("script_controller")]
    public class GameplayController : GameComponent
    {
        [SerializeField] private Transform canvasUI;
        [SerializeField, PagePickup] private string inGamePageName;
        
        private PopupContainer MainPopupContainer => PopupContainer.Find(Constant.MAIN_POPUP_CONTAINER);
        private PageContainer MainPageContainer => PageContainer.Find(Constant.MAIN_PAGE_CONTAINER);

        private void Start()
        {
            MainPageContainer.Push(inGamePageName, true);
        }
    }
}