using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI
{
    public class PlayExitPanel : PanelBasic
    {
        public Button saveButton;
        public Button saveNewButton;
        public Button continueButton;
        public Button quitPlayButton;

        private void Start()
        {
            saveButton.onClick.AddListener(() => { Save(); });
            saveNewButton.onClick.AddListener(() => { SaveNew(); });
            continueButton.onClick.AddListener(() => { Continue(); });
            quitPlayButton.onClick.AddListener(() => { Quit(); });
        }

        private void Save()
        {
            GameManager.Instance.SaveGame(false);
        }

        private void SaveNew()
        {
            GameManager.Instance.SaveGame(true);
        }

        private void Continue()
        {
            GameManager.Instance.ContinuePlay();
        }

        private void Quit()
        {
            GameManager.Instance.QuitPlay();
        }
    }
}

