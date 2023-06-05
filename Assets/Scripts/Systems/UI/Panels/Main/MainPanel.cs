using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI
{
    public class MainPanel : PanelBasic
    {
        public Button newButton;
        public Button loadButton;
        public Button quitButton;

        private void Start()
        {
            newButton.onClick.AddListener(() => { NewGame(); });
            loadButton.onClick.AddListener(() => { Load(); });
            quitButton.onClick.AddListener(() => { QuitGame(); });
        }

        private void NewGame()
        {
            Close();
            UIManager.Instance.OpenSinglePanel<ReadyPlayPanel>();
        }

        private void Load()
        {
            Close();
            UIManager.Instance.OpenSinglePanel<SavesPanel>();
        }

        private void QuitGame()
        {
            Application.Quit();
        }
    }
}

