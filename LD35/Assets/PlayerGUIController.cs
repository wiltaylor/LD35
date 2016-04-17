using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGUIController : MonoBehaviour
{
    public ActorController PlayerActorController;
    public Slider HPBar;
    public Text LevelText;
    public Text GoldText;
    public Text ContextText;
    public GameObject GameOverScreen;
    public GameObject GameMenuScreen;
    public GameObject GameShopScreen;
    public GameObject GameLevelSelectScreen;

    public Text SpeedText;
    public Text DamageText;
    public Text HPText;
    public Text SpeedButtonText;
    public Text DamageButtonText;
    public Text HPButtonText;
    public Button SpeedButton;
    public Button DamageButton;
    public Button HPButton;

    public Button Level1Button;
    public Button Level2Button;
    public Button Level3Button;
    public Button Level4Button;
    public Button Level5Button;
    public Button Level6Button;
    public Button Level7Button;
    public Button Level8Button;
    public Button Level9Button;
    public Button Level10Button;
    public Button CloseLevelButton;
    public Button EntereanceButton;

    private LevelLoader _levelLoader;
    private PlayerPersistData _playerPersistData;
    private PlayerControl _playerControl;
    private UpgradeTable _upgradeTable;

    public void ShowGameOver()
    {
        GameOverScreen.SetActive(true);
    }

    public void ShowGameMenuScreen()
    {
        GameMenuScreen.SetActive(true);
    }

    public void SHowLevelSelectScreen()
    {
        GameLevelSelectScreen.SetActive(true);
    }

    public void LevelSelected(int level)
    {
        _levelLoader.CreateLevel(level);
    }

    public void LevelSelectClosed()
    {
        GameLevelSelectScreen.SetActive(false);
        _playerPersistData.GamePaused = false;
    }

    public void RespawnClicked()
    {
        _levelLoader.Respawn();
    }

    public void Start()
    {
        _levelLoader = GlobalController.Instance.GetComponentInChildren<LevelLoader>();
        _playerPersistData = GlobalController.Instance.GetComponentInChildren<PlayerPersistData>();
        _upgradeTable = GlobalController.Instance.GetComponentInChildren<UpgradeTable>();
    }
        
	public void Update ()
	{
	    if (PlayerActorController == null)
	        return;

	    if (_playerControl == null)
	        _playerControl = PlayerActorController.GetComponent<PlayerControl>();


        HPBar.minValue = 0;
	    HPBar.maxValue = PlayerActorController.MaxHP;
	    HPBar.value = PlayerActorController.HP;
	    ContextText.text = _playerControl.ContextText;

        if (PlayerActorController.HP < 0)
	        HPBar.value = PlayerActorController.HP = 0;

	    LevelText.text = _levelLoader.CurrentLevel == 0 ? "Level: Enterance" : string.Format("Level: {0}", _levelLoader.CurrentLevel);

	    GoldText.text = string.Format("Gold: {0}", _playerPersistData.Gold);

	    SpeedText.text = string.Format("Speed: {0} / {1}",_playerPersistData.SpeedRank, _upgradeTable.SpeedUpgradeModifier.Length - 1);
        DamageText.text = string.Format("Damage: {0} / {1}", _playerPersistData.DamageRank, _upgradeTable.DMGUpgradeModifier.Length - 1);
        HPText.text = string.Format("HP: {0} / {1}", _playerPersistData.HPRank, _upgradeTable.HPUpgradeModifier.Length - 1);

	    if (_playerPersistData.SpeedRank >= _upgradeTable.SpeedUpgradeModifier.Length - 1)
	    {
	        SpeedButtonText.text = "Max Level";
	        SpeedButton.enabled = false;
	    }
	    else
	    {
	        SpeedButtonText.text = string.Format("Upgrade (${0})",
	            _upgradeTable.SpeedUpgradeCosts[_playerPersistData.SpeedRank + 1]);

	        SpeedButton.enabled = _playerPersistData.Gold >= _upgradeTable.SpeedUpgradeCosts[_playerPersistData.SpeedRank + 1];
	    }

        if (_playerPersistData.HPRank >= _upgradeTable.HPUpgradeModifier.Length - 1)
        {
            HPButtonText.text = "Max Level";
            HPButton.enabled = false;
        }
        else
        {
            HPButtonText.text = string.Format("Upgrade (${0})",
                _upgradeTable.HPUpgradeCosts[_playerPersistData.HPRank + 1]);

            HPButton.enabled = _playerPersistData.Gold >= _upgradeTable.HPUpgradeCosts[_playerPersistData.HPRank + 1];
        }

        if (_playerPersistData.DamageRank >= _upgradeTable.DMGUpgradeModifier.Length - 1 )
        {
            DamageButtonText.text = "Max Level";
            DamageButton.enabled = false;
        }
        else
        {
            DamageButtonText.text = string.Format("Upgrade (${0})",
                _upgradeTable.DMGUpgradeCosts[_playerPersistData.DamageRank + 1]);

            DamageButton.enabled = _playerPersistData.Gold >= _upgradeTable.DMGUpgradeCosts[_playerPersistData.DamageRank + 1];
        }


	    Level1Button.gameObject.SetActive(_playerPersistData.LowestLevelVisited >= 1 && _levelLoader.CurrentLevel != 1);
        Level2Button.gameObject.SetActive(_playerPersistData.LowestLevelVisited >= 2 && _levelLoader.CurrentLevel != 2);
        Level3Button.gameObject.SetActive(_playerPersistData.LowestLevelVisited >= 3 && _levelLoader.CurrentLevel != 3);
        Level4Button.gameObject.SetActive(_playerPersistData.LowestLevelVisited >= 4 && _levelLoader.CurrentLevel != 4);
        Level5Button.gameObject.SetActive(_playerPersistData.LowestLevelVisited >= 5 && _levelLoader.CurrentLevel != 5);
        Level6Button.gameObject.SetActive(_playerPersistData.LowestLevelVisited >= 6 && _levelLoader.CurrentLevel != 6);
        Level7Button.gameObject.SetActive(_playerPersistData.LowestLevelVisited >= 7 && _levelLoader.CurrentLevel != 7);
        Level8Button.gameObject.SetActive(_playerPersistData.LowestLevelVisited >= 8 && _levelLoader.CurrentLevel != 8);
        Level9Button.gameObject.SetActive(_playerPersistData.LowestLevelVisited >= 9 && _levelLoader.CurrentLevel != 9);
        Level10Button.gameObject.SetActive(_playerPersistData.LowestLevelVisited >= 10 && _levelLoader.CurrentLevel != 10);
    }

    public void ShowShop()
    {
        GameShopScreen.SetActive(true);
    }

    public void CloseShop()
    {
        GameShopScreen.SetActive(false);
        _playerPersistData.GamePaused = false;
    }

    public void UpgradeSpeed()
    {
        _playerPersistData.SpeedRank++;
        _playerPersistData.DirtyData = true;
        _playerPersistData.Gold -= _upgradeTable.SpeedUpgradeCosts[_playerPersistData.SpeedRank];
    }

    public void UpgradeHP()
    {
        _playerPersistData.HPRank++;
        _playerPersistData.DirtyData = true;
        _playerPersistData.Gold -= _upgradeTable.HPUpgradeCosts[_playerPersistData.HPRank];
    }

    public void UpgradeDamage()
    {
        _playerPersistData.DamageRank++;
        _playerPersistData.DirtyData = true;
        _playerPersistData.Gold -= _upgradeTable.DMGUpgradeCosts[_playerPersistData.DamageRank];
    }

    public void ReturnToMainMenuClicked()
    {
        //todo: fix main menu.
    }

    public void ExitGameClicked()
    {
        Application.Quit();
    }

    public void CloseGameMenu()
    {
        GameMenuScreen.SetActive(false);
        _playerPersistData.GamePaused = false;
    }
}
