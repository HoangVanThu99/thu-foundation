using Pancake.LevelSystem;
using Pancake.Monetization;
using Pancake.SceneFlow;
using Pancake.Scriptable;
using Pancake.Sound;
using Pancake.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Pancake.UI
{
    public sealed class InGameView : View
    {
        [Header("BUTTON")] [SerializeField] private Button buttonHome;
        [SerializeField] private Button buttonReplay;
        [SerializeField] private Button buttonSkipByAd;
        
        [SerializeField] private ScriptableEventString changeSceneEvent;

        [Header("LEVEL")] [SerializeField] private RewardVariable rewardVariable;
        [SerializeField] private ScriptableEventLoadLevel loadLevelEvent;
        [SerializeField] private ScriptableEventGetLevelCached getNextLevelCached;
        [SerializeField] private ScriptableEventGetGameObject getLevelRootHolder;
        [SerializeField] private IntVariable currentLevelIndex;

        private void Start()
        {
            buttonHome.onClick.AddListener(GoToMenu);
            buttonReplay.onClick.AddListener(OnButtonReplayClicked);
            buttonSkipByAd.onClick.AddListener(OnButtonSkipByAdClicked);
        }

        private void OnButtonSkipByAdClicked()
        {
            if (!Application.isMobilePlatform)
            {
                Execute();
            }
            else
            {
                rewardVariable.Context().Show().OnCompleted(Execute);
            }

            return;

            async void Execute()
            {
                currentLevelIndex.Value += 1;
                var prefabLevel = await loadLevelEvent.Raise(currentLevelIndex.Value);
                GameObject goSpawnLevel = getLevelRootHolder.Raise();
                goSpawnLevel.RemoveAllChildren();
                var instance = Instantiate(prefabLevel, goSpawnLevel.transform, false);
            }
        }

        private void OnButtonReplayClicked()
        {
            // Because the next level load has not yet been called, the cached next level is the current level being played
            var nextLevelPrefab = getNextLevelCached.Raise();
            // clear current instance
            GameObject goSpawnLevel = getLevelRootHolder.Raise();
            goSpawnLevel.RemoveAllChildren();
            var instance = Instantiate(nextLevelPrefab, goSpawnLevel.transform, false);
        }

        private void GoToMenu() { changeSceneEvent.Raise(Constant.MENU_SCENE); }
        
    	protected override UniTask Initialize()
    	{
        	return UniTask.CompletedTask;
    	}
    }
}
