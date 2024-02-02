using UnityEngine;
using TMPro;

namespace Runtime
{
    public class Tooltip : MonoBehaviour
    {
        [System.Serializable]
        private enum TooltipScenes{ Menu, PreGameMenu }

        [SerializeField] private TooltipScenes _tooltipScene;
        [SerializeField] private Animator _tooltipAnimator;
        [SerializeField] private TMP_Text _tTooltip;
        [SerializeField] private UIRaycaster _uiRaycaster;
        [SerializeField] private string[] _animParams = {"classic", "settings", "stars", "reset"};
        [SerializeField] [TextArea(1, 5)]private string[] _tooltipMenuTexts = {
            "You can start the game in classic mode\nby cutting the fruit by swiping.",
            "You can turn on/off the music and the sound effects or\nyou can adjust other settings by clicking\nthe clock icon.",
            "You'll earn stars by completing daily objectives, achievements.\nAlso you will earn some amount of stars end of the each round."
        };
        private int _tooltipIndex = 0;

        private void Start() {
            switch(_tooltipScene)
            {
                case TooltipScenes.Menu:
                    if(!VGPGSManager.Instance._playerData._areMenuTipsDone)
                        ShowTooltip(_tooltipScene);
                    else
                        _uiRaycaster.enabled = true;
                    break;

                case TooltipScenes.PreGameMenu:
                    if(!VGPGSManager.Instance._playerData._arePreGameTipsDone)
                        ShowTooltip(_tooltipScene);
                    else
                        _uiRaycaster.enabled = true;
                    break;
            }
        }

        private void Update() {
            if(Input.GetMouseButtonDown(0))
            {
                switch(_tooltipScene)
                {
                    case TooltipScenes.Menu:
                        if(!VGPGSManager.Instance._playerData._areMenuTipsDone)
                            ShowTooltip(_tooltipScene);
                        break;

                    case TooltipScenes.PreGameMenu:
                        if(!VGPGSManager.Instance._playerData._arePreGameTipsDone)
                            ShowTooltip(_tooltipScene);
                        break;
                }
            }
        }
        private void ShowTooltip(TooltipScenes tooltipScene)
        {
            if(_animParams.Length > _tooltipIndex)
            {
                _tooltipAnimator.SetTrigger(_animParams[_tooltipIndex]);

                if(_tooltipIndex == _animParams.Length - 1)
                {
                    _uiRaycaster.enabled = true;

                    if(tooltipScene == TooltipScenes.Menu)
                        VGPGSManager.Instance._playerData._areMenuTipsDone = true;
                    else if(tooltipScene == TooltipScenes.PreGameMenu)
                        VGPGSManager.Instance._playerData._arePreGameTipsDone = true;
                }
            }

            if(_tooltipMenuTexts.Length > _tooltipIndex)
                _tTooltip.text = _tooltipMenuTexts[_tooltipIndex];


            if(_tooltipIndex + 1 < _animParams.Length)
                _tooltipIndex++;
        }
    }
}
