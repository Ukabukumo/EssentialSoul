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

        // Добавление слушателей на кнопки навыков
        for (int i = 0; i < 55; i++)
        {
            Button _skill = skills.transform.GetChild(i).GetComponent<Button>();
            _skill.onClick.AddListener(UpgradeMenuInit);
        }

        // Добавление слушателей на кнопки улучшения навыков
        damageUpgrade.GetComponent<Button>().onClick.AddListener(UpgradeDamage);
        speedUpgrade.GetComponent<Button>().onClick.AddListener(UpgradeSpeed);
        aimSpeedUpgrade.GetComponent<Button>().onClick.AddListener(UpgradeAimSpeed);
        specialUpgrade.GetComponent<Button>().onClick.AddListener(UpgradeSpecial);
        removeUpgrade.GetComponent<Button>().onClick.AddListener(RemoveUpgrade);

        // Очищаем массив состояний навыков
        skillsState = new int[55];
        for (int i = 0; i < 55; i++)
        {
            skillsState[i] = 0;
        }

        // Очищаем массив состояний специальных навыков
        specialSkills = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            specialSkills[i] = false;
        }
    }

    // Инициализация меню
    public void SkillsMenuInit()
    {
        WindowInit();
    }

    // Инициализация окна меню
    private void WindowInit()
    {
        // Активация окна
        skillsMenu.SetActive(true);

        // Вывод необходимой информации о игроке
        Player _player = player.GetComponent<Player>();
        healthInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetMaxHealth());
        soulsInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetSouls());
        damageInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetDamage());
        speedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetMiniPlayerSpeed());
        aimSpeedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetAimSpeed());

        // Подсветка первого навыка в меню
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(skills.transform.GetChild(0).gameObject);

        // Добавляем дополнительную камеру
        miniGameCamera.SetActive(true);

        lastSelectedObject = null;

        StartCoroutine(Act());
    }

    // Действия в меню
    private IEnumerator Act()
    {
        while (!Input.GetKeyDown(KeyCode.Escape))
        {
            yield return null;

            // Вывод информации о навыке
            SkillInfo();

            // Текущая зона дерева навыков
            int _ind = (Convert.ToInt32(choosenSkillName) - 1) / 11;

            // Появление специального навыка при наличии 10 улучшеных навыков
            if (upgradedSkills >= 10 && !specialSkills[_ind])
            {
                // Увеличение панели улучшения
                RectTransform _trans = upgradePanel.GetComponent<RectTransform>();
                Vector2 _newSize = new Vector2(125f, 125f);
                Vector2 oldSize = _trans.rect.size;
                Vector2 deltaSize = _newSize - oldSize;
                _trans.offsetMin = _trans.offsetMin - 
                    new Vector2(deltaSize.x * _trans.pivot.x, deltaSize.y * _trans.pivot.y);
                _trans.offsetMax = _trans.offsetMax + 
                    new Vector2(deltaSize.x * (1f - _trans.pivot.x), deltaSize.y * (1f - _trans.pivot.y));

                // Включение специального навыка
                specialUpgrade.SetActive(true);
            }

            else
            {
                // Уменьшение панели улучшения
                RectTransform _trans = upgradePanel.GetComponent<RectTransform>();
                Vector2 _newSize = new Vector2(125f, 95f);
                Vector2 oldSize = _trans.rect.size;
                Vector2 deltaSize = _newSize - oldSize;
                _trans.offsetMin = _trans.offsetMin - 
                    new Vector2(deltaSize.x * _trans.pivot.x, deltaSize.y * _trans.pivot.y);
                _trans.offsetMax = _trans.offsetMax + 
                    new Vector2(deltaSize.x * (1f - _trans.pivot.x), deltaSize.y * (1f - _trans.pivot.y));

                // Выключение специального навыка
                specialUpgrade.SetActive(false);
            }
        }

        // Деактивация окна навыков
        upgradePanel.SetActive(false);
        skills.SetActive(true);
        skillsMenu.SetActive(false);

        // Активация игрока
        player.SetActive(true);

        // Убираем дополнительную камеру
        miniGameCamera.SetActive(false);
    }

    // Улучшение навыка
    private void UpgradeMenuInit()
    {
        int _ind = Convert.ToInt32(eventSystem.currentSelectedGameObject.name);

        // Если в текущей ячейке не назначен навык
        if (skillsState[_ind - 1] == 0)
        {
            // Проверяем наличие душ
            if (player.GetComponent<Player>().GetSouls() > 0)
            {
                choosenSkill = eventSystem.currentSelectedGameObject;
                choosenSkillName = eventSystem.currentSelectedGameObject.name;

                upgradePanel.SetActive(true);
                skills.SetActive(false);

                // Подсветка первого элемента в меню
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

            // Подсветка первого элемента в меню
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(removeUpgrade);

            StartCoroutine(CheckUpgrade());
        }
    }

    // Улучшение урона
    private void UpgradeDamage()
    {
        // Изменение урона игрока
        player.GetComponent<Player>().AddDamage(1);

        // Изменение информации об уроне игрока
        Player _player = player.GetComponent<Player>();
        damageInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetDamage());

        // Забираем необходимое количество душ у игрока
        player.GetComponent<Player>().AddSouls(-1);

        // Обновляем информацию о душах
        soulsInfo.GetComponent<TextMeshProUGUI>().text =
            Convert.ToString(player.GetComponent<Player>().GetSouls());

        // Изменение иконки улучшенного навыка
        choosenSkill.GetComponent<Image>().sprite = damageSpr;
        choosenSkill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
        SpriteState _ss = choosenSkill.GetComponent<Button>().spriteState;
        _ss.selectedSprite = damageSelectedSpr;
        choosenSkill.GetComponent<Button>().spriteState = _ss;

        // Изменение состояния ячейки навыка
        skillsState[Convert.ToInt32(choosenSkillName) - 1] = 1;

        upgradedSkills++;

        ReturnToMenu();
    }

    // Улучшение скорости передвижения игрока в миниигре
    private void UpgradeSpeed()
    {
        // Изменение скорости передвижения игрока в мииниигре
        player.GetComponent<Player>().AddMiniPlayerSpeed(0.1f);

        // Изменение информации о скорости передвижения игрока в миниигре
        Player _player = player.GetComponent<Player>();
        speedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetMiniPlayerSpeed());

        // Забираем необходимое количество душ у игрока
        player.GetComponent<Player>().AddSouls(-1);

        // Обновляем информацию о душах
        soulsInfo.GetComponent<TextMeshProUGUI>().text =
            Convert.ToString(player.GetComponent<Player>().GetSouls());

        // Изменение иконки улучшенного навыка
        choosenSkill.GetComponent<Image>().sprite = speedSpr;
        choosenSkill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
        SpriteState _ss = choosenSkill.GetComponent<Button>().spriteState;
        _ss.selectedSprite = speedSelectedSpr;
        choosenSkill.GetComponent<Button>().spriteState = _ss;

        // Изменение состояния ячейки навыка
        skillsState[Convert.ToInt32(choosenSkillName) - 1] = 2;

        upgradedSkills++;

        ReturnToMenu();
    }

    // Улучшение скорости передвижения прицела
    private void UpgradeAimSpeed()
    {
        // Изменение скорости передвижения прицела
        player.GetComponent<Player>().AddAimSpeed(0.1f);

        // Изменение информации о скорости прицела
        Player _player = player.GetComponent<Player>();
        aimSpeedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetAimSpeed());

        // Забираем необходимое количество душ у игрока
        player.GetComponent<Player>().AddSouls(-1);

        // Обновляем информацию о душах
        soulsInfo.GetComponent<TextMeshProUGUI>().text =
            Convert.ToString(player.GetComponent<Player>().GetSouls());

        // Изменение иконки улучшенного навыка
        choosenSkill.GetComponent<Image>().sprite = aimSpeedSpr;
        choosenSkill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
        SpriteState _ss = choosenSkill.GetComponent<Button>().spriteState;
        _ss.selectedSprite = aimSpeedSelectedSpr;
        choosenSkill.GetComponent<Button>().spriteState = _ss;

        // Изменение состояния ячейки навыка
        skillsState[Convert.ToInt32(choosenSkillName) - 1] = 3;

        upgradedSkills++;

        ReturnToMenu();
    }

    private void UpgradeSpecial()
    {
        upgradedSkills -= 10;

        // Номер зоны дерева навыков
        int _ind = (Convert.ToInt32(choosenSkillName) - 1) / 11;

        // Помечаем, что этот навык уже получен
        specialSkills[_ind] = true;

        // Назначаем особый навык в зависимости от зоны дерева навыков
        switch (_ind)
        {
            // Особый навык "СБОРЩИК"
            case 0:
                player.GetComponent<Player>().SetCollector(true);
                break;

            // Особый навык "ИНИЦИАТОР"
            case 1:
                player.GetComponent<Player>().SetInitiator(true);
                break;

            // Особый навык "ЛОВКАЧ"
            case 2:
                player.GetComponent<Player>().SetDodger(true);
                break;

            // Особый навык "БЛАГОСЛОВЕННЫЙ"
            case 3:
                player.GetComponent<Player>().SetBlessed(true);
                break;

            // Особый навык "СТРЕЛОК"
            case 4:
                player.GetComponent<Player>().SetShooter(true);
                break;
        }

        // Забираем необходимое количество душ у игрока
        player.GetComponent<Player>().AddSouls(-1);

        // Обновляем информацию о душах
        soulsInfo.GetComponent<TextMeshProUGUI>().text =
            Convert.ToString(player.GetComponent<Player>().GetSouls());

        // Изменение иконки улучшенного навыка
        choosenSkill.GetComponent<Image>().sprite = specialSpr;
        choosenSkill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
        SpriteState _ss = choosenSkill.GetComponent<Button>().spriteState;
        _ss.selectedSprite = specialSelectedSpr;
        choosenSkill.GetComponent<Button>().spriteState = _ss;

        // Изменение состояния ячейки навыка
        int _nSpecial = (Convert.ToInt32(choosenSkillName) - 1) / 11;
        skillsState[Convert.ToInt32(choosenSkillName) - 1] = 4 + _nSpecial;

        ReturnToMenu();
    }

    // Удаление навыка
    private void RemoveUpgrade()
    {
        Player _player = player.GetComponent<Player>();

        // Определяем тип навыка
        switch (skillsState[Convert.ToInt32(choosenSkillName) - 1])
        {
            // Урон
            case 1:
                // Изменение урона игрока
                player.GetComponent<Player>().AddDamage(-1);

                // Изменение информации об уроне игрока
                damageInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetDamage());

                upgradedSkills--;
                break;

            // Скорость игрока в миниигре
            case 2:
                // Изменение скорости передвижения игрока в мииниигре
                player.GetComponent<Player>().AddMiniPlayerSpeed(-0.1f);

                // Изменение информации о скорости передвижения игрока в мииниигре
                speedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetMiniPlayerSpeed());

                upgradedSkills--;
                break;

            // Скорость прицела
            case 3:
                // Изменение скорости передвижения прицела
                player.GetComponent<Player>().AddAimSpeed(-0.1f);

                // Изменение информации о скорости прицела
                aimSpeedInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(_player.GetAimSpeed());

                upgradedSkills--;
                break;

            // Специальный навык "СБОРЩИК"
            case 4:
                player.GetComponent<Player>().SetCollector(false);
                specialSkills[0] = false;
                upgradedSkills += 10;
                break;

            // Специальный навык "ИНИЦИАТОР"
            case 5:
                player.GetComponent<Player>().SetInitiator(false);
                specialSkills[1] = false;
                upgradedSkills += 10;
                break;

            // Специальный навык "ЛОВКАЧ"
            case 6:
                player.GetComponent<Player>().SetDodger(false);
                specialSkills[2] = false;
                upgradedSkills += 10;
                break;

            // Специальный навык "БЛАГОСЛОВЕННЫЙ"
            case 7:
                player.GetComponent<Player>().SetBlessed(false);
                specialSkills[3] = false;
                upgradedSkills += 10;
                break;

            // Специальный навык "СТРЕЛОК"
            case 8:
                player.GetComponent<Player>().SetShooter(false);
                specialSkills[4] = false;
                upgradedSkills += 10;
                break;
        }

        // Помечаем ячейку навыка как пустую
        skillsState[Convert.ToInt32(choosenSkillName) - 1] = 0;

        // Изменение иконки улучшенного навыка
        choosenSkill.GetComponent<Image>().sprite = null;
        choosenSkill.GetComponent<Button>().transition = Selectable.Transition.ColorTint;

        ReturnToMenu();
    }

    // Проверка действий в меню улучшения
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

    // Возвращение в меню навыков
    private void ReturnToMenu()
    {
        upgradePanel.SetActive(false);
        cancelUpgradePanel.SetActive(false);
        skills.SetActive(true);

        // Подсветка первого навыка в меню
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(skills.transform.GetChild(0).gameObject);
    }

    // Вывод информации о навыке
    private void SkillInfo()
    {
        // Если текущая кнопка - удаление навыка
        if (eventSystem.currentSelectedGameObject.Equals(removeUpgrade))
        {
            lastSelectedObject = eventSystem.currentSelectedGameObject;
            textField.GetComponent<TextMeshProUGUI>().text = "Remove skill";
            return;
        }

        // Если произошла смена ячейки навыка
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
                // Информация об ячейках улучшения
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

            // Вывод информации об ячейке навыка
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

    // Сохранение информации о навыках в список
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

    // Загрузка информации о навыках из списка
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

            // Изменение иконки навыков
            switch (skillsState[i])
            {
                // Пустая ячейка навыка
                case 0:
                    _skill.GetComponent<Image>().sprite = null;
                    _skill.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
                    break;
                
                // Урон
                case 1:
                    _skill.GetComponent<Image>().sprite = damageSpr;
                    _skill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
                    _ss.selectedSprite = damageSelectedSpr;
                    _skill.GetComponent<Button>().spriteState = _ss;
                    break;

                // Скорость игрока в миниигре
                case 2:
                    _skill.GetComponent<Image>().sprite = speedSpr;
                    _skill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
                    _ss.selectedSprite = speedSelectedSpr;
                    _skill.GetComponent<Button>().spriteState = _ss;
                    break;
                
                // Скорость прицела
                case 3:
                    _skill.GetComponent<Image>().sprite = aimSpeedSpr;
                    _skill.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
                    _ss.selectedSprite = aimSpeedSelectedSpr;
                    _skill.GetComponent<Button>().spriteState = _ss;
                    break;
                
                // Специальный навык
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
