using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class SkillsMenuManager : MonoBehaviour
{
    [SerializeField] GameObject skillsMenu;
    [SerializeField] GameObject skills;
    [SerializeField] GameObject player;
    [SerializeField] GameObject healthInfo;
    [SerializeField] GameObject soulsInfo;
    [SerializeField] GameObject damageInfo;
    [SerializeField] GameObject speedInfo;
    [SerializeField] GameObject aimSpeedInfo;
    [SerializeField] GameObject upgradePanel;
    [SerializeField] GameObject cancelUpgradePanel;
    [SerializeField] GameObject damageUpgrade;
    [SerializeField] GameObject speedUpgrade;
    [SerializeField] GameObject aimSpeedUpgrade;
    [SerializeField] GameObject specialUpgrade;
    [SerializeField] GameObject removeUpgrade;
    [SerializeField] GameObject miniGameCamera;
    [SerializeField] GameObject textField;
    [SerializeField] Sprite damageSpr;
    [SerializeField] Sprite speedSpr;
    [SerializeField] Sprite aimSpeedSpr;
    [SerializeField] Sprite specialSpr;
    [SerializeField] Sprite damageSelectedSpr;
    [SerializeField] Sprite speedSelectedSpr;
    [SerializeField] Sprite aimSpeedSelectedSpr;
    [SerializeField] Sprite specialSelectedSpr;
    private EventSystem eventSystem;
    private GameObject lastSelectedObject;
    private GameObject choosenSkill;
    private string choosenSkillName;
    private int[] skillsState;
    private bool[] specialSkills;
    private int upgradedSkills = 0;

    private void Start()
    {
        eventSystem = EventSystem.current;

        // ���������� ���������� �� ������ �������
        for (int i = 0; i < 55; i++)
        {
            Button _skill = skills.transform.GetChild(i).GetComponent<Button>();
            _skill.onClick.AddListener(UpgradeMenuInit);
        }

        // ���������� ���������� �� ������ ��������� �������
        damageUpgrade.GetComponent<Button>().onClick.AddListener(UpgradeDamage);
        speedUpgrade.GetComponent<Button>().onClick.AddListener(UpgradeSpeed);
        aimSpeedUpgrade.GetComponent<Button>().onClick.AddListener(UpgradeAimSpeed);
        specialUpgrade.GetComponent<Button>().onClick.AddListener(UpgradeSpecial);
        removeUpgrade.GetComponent<Button>().onClick.AddListener(RemoveUpgrade);

        // ������� ������ ��������� �������
        skillsState = new int[55];
        for (int i = 0; i < 55; i++)
        {
            skillsState[i] = 0;
        }

        // ������� ������ ��������� ����������� �������
        specialSkills = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            specialSkills[i] = false;
        }
    }

    // ������������� ����
    public void SkillsMenuInit()
    {
        WindowInit();
    }

    // ������������� ���� ����
    private void WindowInit()
    {
        // ��������� ����
        skillsMenu.SetActive(true);

        // ����� ����������� ���������� � ������
        Player _player = player.GetComponent<Player>();
        healthInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetMaxHealth());
        soulsInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetSouls());
        damageInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetDamage());
        speedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetMiniPlayerSpeed());
        aimSpeedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetAimSpeed());

        // ��������� ������� ������ � ����
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(skills.transform.GetChild(0).gameObject);

        // ��������� �������������� ������
        miniGameCamera.SetActive(true);

        lastSelectedObject = null;

        StartCoroutine(Act());
    }

    // �������� � ����
    private IEnumerator Act()
    {
        while (!Input.GetKeyDown(KeyCode.Escape))
        {
            yield return null;

            // ����� ���������� � ������
            SkillInfo();

            // ������� ���� ������ �������
            int _ind = (Convert.ToInt32(choosenSkillName) - 1) / 11;

            // ��������� ������������ ������ ��� ������� 10 ��������� �������
            if (upgradedSkills >= 10 && !specialSkills[_ind])
            {
                // ���������� ������ ���������
                RectTransform _trans = upgradePanel.GetComponent<RectTransform>();
                Vector2 _newSize = new Vector2(125f, 125f);
                Vector2 oldSize = _trans.rect.size;
                Vector2 deltaSize = _newSize - oldSize;
                _trans.offsetMin = _trans.offsetMin - 
                    new Vector2(deltaSize.x * _trans.pivot.x, deltaSize.y * _trans.pivot.y);
                _trans.offsetMax = _trans.offsetMax + 
                    new Vector2(deltaSize.x * (1f - _trans.pivot.x), deltaSize.y * (1f - _trans.pivot.y));

                // ��������� ������������ ������
                specialUpgrade.SetActive(true);
            }

            else
            {
                // ���������� ������ ���������
                RectTransform _trans = upgradePanel.GetComponent<RectTransform>();
                Vector2 _newSize = new Vector2(125f, 95f);
                Vector2 oldSize = _trans.rect.size;
                Vector2 deltaSize = _newSize - oldSize;
                _trans.offsetMin = _trans.offsetMin - 
                    new Vector2(deltaSize.x * _trans.pivot.x, deltaSize.y * _trans.pivot.y);
                _trans.offsetMax = _trans.offsetMax + 
                    new Vector2(deltaSize.x * (1f - _trans.pivot.x), deltaSize.y * (1f - _trans.pivot.y));

                // ���������� ������������ ������
                specialUpgrade.SetActive(false);
            }
        }

        // ����������� ���� �������
        upgradePanel.SetActive(false);
        skills.SetActive(true);
        skillsMenu.SetActive(false);

        // ��������� ������
        player.SetActive(true);

        // ������� �������������� ������
        miniGameCamera.SetActive(false);
    }

    // ��������� ������
    private void UpgradeMenuInit()
    {
        int _ind = Convert.ToInt32(eventSystem.currentSelectedGameObject.name);

        // ���� � ������� ������ �� �������� �����
        if (skillsState[_ind - 1] == 0)
        {
            // ��������� ������� ���
            if (player.GetComponent<Player>().GetSouls() > 0)
            {
                choosenSkill = eventSystem.currentSelectedGameObject;
                choosenSkillName = eventSystem.currentSelectedGameObject.name;

                upgradePanel.SetActive(true);
                skills.SetActive(false);

                // ��������� ������� �������� � ����
                eventSystem.SetSelectedGameObject(null);
                eventSystem.SetSelectedGameObject(damageUpgrade);

                StartCoroutine(CheckUpgrade());
            }

            else
            {
                textField.GetComponent<TextMeshProUGUI>().text = "Not enough souls!";
            }
        }

        else
        {
            choosenSkill = eventSystem.currentSelectedGameObject;
            choosenSkillName = eventSystem.currentSelectedGameObject.name;

            cancelUpgradePanel.SetActive(true);
            skills.SetActive(false);

            // ��������� ������� �������� � ����
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(removeUpgrade);

            StartCoroutine(CheckUpgrade());
        }
    }

    // ��������� �����
    private void UpgradeDamage()
    {
        // ��������� ����� ������
        player.GetComponent<Player>().AddDamage(1);

        // ��������� ���������� �� ����� ������
        Player _player = player.GetComponent<Player>();
        damageInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetDamage());

        // �������� ����������� ���������� ��� � ������
        player.GetComponent<Player>().AddSouls(-1);

        // ��������� ���������� � �����
        soulsInfo.GetComponent<TextMeshProUGUI>().text =
            Convert.ToString(player.GetComponent<Player>().GetSouls());

        // ��������� ������ ����������� ������
        choosenSkill.GetComponent<Image>().sprite = damageSpr;
        choosenSkill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
        SpriteState _ss = choosenSkill.GetComponent<Button>().spriteState;
        _ss.selectedSprite = damageSelectedSpr;
        choosenSkill.GetComponent<Button>().spriteState = _ss;

        // ��������� ��������� ������ ������
        skillsState[Convert.ToInt32(choosenSkillName) - 1] = 1;

        upgradedSkills++;

        ReturnToMenu();
    }

    // ��������� �������� ������������ ������ � ��������
    private void UpgradeSpeed()
    {
        // ��������� �������� ������������ ������ � ���������
        player.GetComponent<Player>().AddMiniPlayerSpeed(0.1f);

        // ��������� ���������� � �������� ������������ ������ � ��������
        Player _player = player.GetComponent<Player>();
        speedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetMiniPlayerSpeed());

        // �������� ����������� ���������� ��� � ������
        player.GetComponent<Player>().AddSouls(-1);

        // ��������� ���������� � �����
        soulsInfo.GetComponent<TextMeshProUGUI>().text =
            Convert.ToString(player.GetComponent<Player>().GetSouls());

        // ��������� ������ ����������� ������
        choosenSkill.GetComponent<Image>().sprite = speedSpr;
        choosenSkill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
        SpriteState _ss = choosenSkill.GetComponent<Button>().spriteState;
        _ss.selectedSprite = speedSelectedSpr;
        choosenSkill.GetComponent<Button>().spriteState = _ss;

        // ��������� ��������� ������ ������
        skillsState[Convert.ToInt32(choosenSkillName) - 1] = 2;

        upgradedSkills++;

        ReturnToMenu();
    }

    // ��������� �������� ������������ �������
    private void UpgradeAimSpeed()
    {
        // ��������� �������� ������������ �������
        player.GetComponent<Player>().AddAimSpeed(0.1f);

        // ��������� ���������� � �������� �������
        Player _player = player.GetComponent<Player>();
        aimSpeedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetAimSpeed());

        // �������� ����������� ���������� ��� � ������
        player.GetComponent<Player>().AddSouls(-1);

        // ��������� ���������� � �����
        soulsInfo.GetComponent<TextMeshProUGUI>().text =
            Convert.ToString(player.GetComponent<Player>().GetSouls());

        // ��������� ������ ����������� ������
        choosenSkill.GetComponent<Image>().sprite = aimSpeedSpr;
        choosenSkill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
        SpriteState _ss = choosenSkill.GetComponent<Button>().spriteState;
        _ss.selectedSprite = aimSpeedSelectedSpr;
        choosenSkill.GetComponent<Button>().spriteState = _ss;

        // ��������� ��������� ������ ������
        skillsState[Convert.ToInt32(choosenSkillName) - 1] = 3;

        upgradedSkills++;

        ReturnToMenu();
    }

    private void UpgradeSpecial()
    {
        upgradedSkills -= 10;

        // ����� ���� ������ �������
        int _ind = (Convert.ToInt32(choosenSkillName) - 1) / 11;

        // ��������, ��� ���� ����� ��� �������
        specialSkills[_ind] = true;

        // ��������� ������ ����� � ����������� �� ���� ������ �������
        switch (_ind)
        {
            // ������ ����� "�������"
            case 0:
                player.GetComponent<Player>().SetCollector(true);
                break;

            // ������ ����� "���������"
            case 1:
                player.GetComponent<Player>().SetInitiator(true);
                break;

            // ������ ����� "������"
            case 2:
                player.GetComponent<Player>().SetDodger(true);
                break;

            // ������ ����� "��������������"
            case 3:
                player.GetComponent<Player>().SetBlessed(true);
                break;

            // ������ ����� "�������"
            case 4:
                player.GetComponent<Player>().SetShooter(true);
                break;
        }

        // �������� ����������� ���������� ��� � ������
        player.GetComponent<Player>().AddSouls(-1);

        // ��������� ���������� � �����
        soulsInfo.GetComponent<TextMeshProUGUI>().text =
            Convert.ToString(player.GetComponent<Player>().GetSouls());

        // ��������� ������ ����������� ������
        choosenSkill.GetComponent<Image>().sprite = specialSpr;
        choosenSkill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
        SpriteState _ss = choosenSkill.GetComponent<Button>().spriteState;
        _ss.selectedSprite = specialSelectedSpr;
        choosenSkill.GetComponent<Button>().spriteState = _ss;

        // ��������� ��������� ������ ������
        int _nSpecial = (Convert.ToInt32(choosenSkillName) - 1) / 11;
        skillsState[Convert.ToInt32(choosenSkillName) - 1] = 4 + _nSpecial;

        ReturnToMenu();
    }

    // �������� ������
    private void RemoveUpgrade()
    {
        Player _player = player.GetComponent<Player>();

        // ���������� ��� ������
        switch (skillsState[Convert.ToInt32(choosenSkillName) - 1])
        {
            // ����
            case 1:
                // ��������� ����� ������
                player.GetComponent<Player>().AddDamage(-1);

                // ��������� ���������� �� ����� ������
                damageInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetDamage());

                upgradedSkills--;
                break;

            // �������� ������ � ��������
            case 2:
                // ��������� �������� ������������ ������ � ���������
                player.GetComponent<Player>().AddMiniPlayerSpeed(-0.1f);

                // ��������� ���������� � �������� ������������ ������ � ���������
                speedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetMiniPlayerSpeed());

                upgradedSkills--;
                break;

            // �������� �������
            case 3:
                // ��������� �������� ������������ �������
                player.GetComponent<Player>().AddAimSpeed(-0.1f);

                // ��������� ���������� � �������� �������
                aimSpeedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetAimSpeed());

                upgradedSkills--;
                break;

            // ����������� ����� "�������"
            case 4:
                player.GetComponent<Player>().SetCollector(false);
                specialSkills[0] = false;
                upgradedSkills += 10;
                break;

            // ����������� ����� "���������"
            case 5:
                player.GetComponent<Player>().SetInitiator(false);
                specialSkills[1] = false;
                upgradedSkills += 10;
                break;

            // ����������� ����� "������"
            case 6:
                player.GetComponent<Player>().SetDodger(false);
                specialSkills[2] = false;
                upgradedSkills += 10;
                break;

            // ����������� ����� "��������������"
            case 7:
                player.GetComponent<Player>().SetBlessed(false);
                specialSkills[3] = false;
                upgradedSkills += 10;
                break;

            // ����������� ����� "�������"
            case 8:
                player.GetComponent<Player>().SetShooter(false);
                specialSkills[4] = false;
                upgradedSkills += 10;
                break;
        }

        // �������� ������ ������ ��� ������
        skillsState[Convert.ToInt32(choosenSkillName) - 1] = 0;

        // ��������� ������ ����������� ������
        choosenSkill.GetComponent<Image>().sprite = null;
        choosenSkill.GetComponent<Button>().transition = Selectable.Transition.ColorTint;

        ReturnToMenu();
    }

    // �������� �������� � ���� ���������
    private IEnumerator CheckUpgrade()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                ReturnToMenu();

                break;
            }

            yield return null;
        }
    }

    // ����������� � ���� �������
    private void ReturnToMenu()
    {
        upgradePanel.SetActive(false);
        cancelUpgradePanel.SetActive(false);
        skills.SetActive(true);

        // ��������� ������� ������ � ����
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(skills.transform.GetChild(0).gameObject);
    }

    // ����� ���������� � ������
    private void SkillInfo()
    {
        // ���� ������� ������ - �������� ������
        if (eventSystem.currentSelectedGameObject.Equals(removeUpgrade))
        {
            lastSelectedObject = eventSystem.currentSelectedGameObject;
            textField.GetComponent<TextMeshProUGUI>().text = "Remove skill";
            return;
        }

        // ���� ��������� ����� ������ ������
        if (lastSelectedObject != eventSystem.currentSelectedGameObject)
        {
            lastSelectedObject = eventSystem.currentSelectedGameObject;

            int _skillState;

            try
            {
                int _ind = Convert.ToInt32(eventSystem.currentSelectedGameObject.name);
                _skillState = skillsState[_ind - 1];
            }
            catch (FormatException)
            {
                // ���������� �� ������� ���������
                switch (eventSystem.currentSelectedGameObject.name)
                {
                    case "Damage":
                        _skillState = 1;
                        break;

                    case "Speed":
                        _skillState = 2;
                        break;

                    case "AimSpeed":
                        _skillState = 3;
                        break;

                    case "Special":
                        int _nSpecial = (Convert.ToInt32(choosenSkillName) - 1) / 11;
                        _skillState = 4 + _nSpecial;
                        break;

                    default:
                        _skillState = 0;
                        break;
                }
            }

            // ����� ���������� �� ������ ������
            switch (_skillState)
            {
                case 0:
                    textField.GetComponent<TextMeshProUGUI>().text = "Skill not assigned";
                    break;

                case 1:
                    textField.GetComponent<TextMeshProUGUI>().text = "Damage +1";
                    break;

                case 2:
                    textField.GetComponent<TextMeshProUGUI>().text =
                        "Player speed in mini game +0.1";
                    break;

                case 3:
                    textField.GetComponent<TextMeshProUGUI>().text =
                        "Aim speed +0.1";
                    break;

                case 4:
                    textField.GetComponent<TextMeshProUGUI>().text = "Special: Collector";
                    break;

                case 5:
                    textField.GetComponent<TextMeshProUGUI>().text = "Special: Initiator";
                    break;

                case 6:
                    textField.GetComponent<TextMeshProUGUI>().text = "Special: Dodger";
                    break;

                case 7:
                    textField.GetComponent<TextMeshProUGUI>().text = "Special: Blessed";
                    break;

                case 8:
                    textField.GetComponent<TextMeshProUGUI>().text = "Special: Shooter";
                    break;

                default:
                    textField.GetComponent<TextMeshProUGUI>().text = "Skill not assigned";
                    break;
            }
        }
    }

    // ���������� ���������� � ������� � ������
    public ArrayList GetInfo()
    {
        ArrayList _info = new ArrayList();

        for (int i = 0; i < skillsState.Length; i++)
        {
            _info.Add(skillsState[i]);
        }

        for (int i = 0; i < specialSkills.Length; i++)
        {
            _info.Add(specialSkills[i]);
        }

        _info.Add(upgradedSkills);

        return _info;
    }

    // �������� ���������� � ������� �� ������
    public void SetInfo(ArrayList _info)
    {
        for (int i = 0; i < skillsState.Length; i++)
        {
            skillsState[i] = int.Parse(_info[i].ToString());
        }

        for (int i = 0; i < specialSkills.Length; i++)
        {
            specialSkills[i] = bool.Parse(_info[skillsState.Length + i].ToString());
        }

        upgradedSkills = int.Parse(_info[skillsState.Length + specialSkills.Length].ToString());

        for (int i = 0; i < 55; i++)
        {
            Button _skill = skills.transform.GetChild(i).GetComponent<Button>();
            SpriteState _ss = _skill.GetComponent<Button>().spriteState;

            // ��������� ������ �������
            switch (skillsState[i])
            {
                // ������ ������ ������
                case 0:
                    _skill.GetComponent<Image>().sprite = null;
                    _skill.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
                    break;
                
                // ����
                case 1:
                    _skill.GetComponent<Image>().sprite = damageSpr;
                    _skill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
                    _ss.selectedSprite = damageSelectedSpr;
                    _skill.GetComponent<Button>().spriteState = _ss;
                    break;

                // �������� ������ � ��������
                case 2:
                    _skill.GetComponent<Image>().sprite = speedSpr;
                    _skill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
                    _ss.selectedSprite = speedSelectedSpr;
                    _skill.GetComponent<Button>().spriteState = _ss;
                    break;
                
                // �������� �������
                case 3:
                    _skill.GetComponent<Image>().sprite = aimSpeedSpr;
                    _skill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
                    _ss.selectedSprite = aimSpeedSelectedSpr;
                    _skill.GetComponent<Button>().spriteState = _ss;
                    break;
                
                // ����������� �����
                default:
                    _skill.GetComponent<Image>().sprite = specialSpr;
                    _skill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
                    _ss.selectedSprite = specialSelectedSpr;
                    _skill.GetComponent<Button>().spriteState = _ss;
                    break;
            }
        }
    }
}
