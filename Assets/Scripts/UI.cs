using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    [SerializeField]
    public Player player;
    [SerializeField]
    public Weapon weapon;

    public FastEnemyParticle FastEnemyParticle;
    public SlowEnemyParticle SlowEnemyParticle;
    private UIDocument document;
    private VisualElement root;
    private Label ReloadingAssert; 
    private Label ScoreText;
    private Label RemainingTimeText;
    private Label UWintext;
    private Label ULosetext;


    private List<Button> weaponSlots;
    private List<Button> allButtons;
    private Button LastPressed;
    //public  BindableProperty<string> ScoreTextPoperty;
    private AudioSource audioSource;

    private VisualElement menu;
    private Button Resume;
    private Button Exit;
    public void Start()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        VisualElement WeaponSlotsCotainer = root.Q("WeaponSlots");
        weaponSlots = WeaponSlotsCotainer.Query<Button>().ToList();
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            var iv = i;
            weaponSlots[i].RegisterCallback<ClickEvent>( x => { weapon.SetBulletSlot(iv); });
        }
        allButtons = root.Query<Button>().ToList();
        for (int i = 0; i < allButtons.Count; i++)
        {
            allButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }

        audioSource = GetComponent<AudioSource>();

        weapon.WeaponSwitched.AddListener(WeaponSlotOutline);
        WeaponSlotOutline(0);

        ReloadingAssert = root.Q("ReloadingAssert") as Label;
        weapon.ReloadingAssert.AddListener(ShowReloadingAssert);
       

        ScoreText = root.Q("Score") as Label;
        player.GetComponent<ScorePointReciever>().PointRecieved.AddListener(Player_OnScoreChange);

        RemainingTimeText = root.Q("RemainingTime") as Label;
        GameController.instance.RemainingTimeChanged.AddListener(GameController_OnTimeChange);

        ReloadingAssert.SetEnabled(false);

        UWintext = root.Q("U_Win") as Label;
        UWintext.visible=false;
        ULosetext = root.Q("U_Lose") as Label;
        ULosetext.visible=false;       

        menu = root.Q("Menu") as VisualElement;
        SetMenuEnabled(false);
        Exit = menu.Q("Exit") as Button;
        Resume = menu.Q("Resume") as Button;
        Resume.RegisterCallback<ClickEvent>(x => { GameController.instance.Play(); });
        Exit.RegisterCallback<ClickEvent>(x => { GameController.instance.Exit(); });

        //ScoreTextPoperty = BindableProperty<string>.Bind( ()=>"Score: "+player.GetComponent<ScorePointReciever>().Points.ToString());

        //ScoreText.bindingPath = player.
        //    GetComponent<ScorePointReciever>()
        //    .Points.ToString();
        //ScoreText.bindingPath = player.GetComponent<ScorePointReciever>().Points.ToString();


    }
    public void SetMenuEnabled(bool b)
    {
        menu.visible= (b) ;

    }
    private void Player_OnScoreChange(int newScore)
    {
        ScoreText.text = "Score: "+newScore.ToString();
    } 
    private void GameController_OnTimeChange(float newTime)
    {
        RemainingTimeText.text = newTime.ToString("0.0");


    }
    private void Update()
    {
        //    Debug.Log(ScoreText.text); 
        //    Debug.Log(ScoreText.bindingPath);  
    }
    private void OnDisable()
    {
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            weaponSlots[i].UnregisterCallback<ClickEvent>(OnWeaponSlotClick);
        }
        allButtons = root.Query<Button>().ToList();
        for (int i = 0; i < allButtons.Count; i++)
        {
            allButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
        weapon.WeaponSwitched.RemoveListener(WeaponSlotOutline);
        weapon.ReloadingAssert.RemoveListener(ShowReloadingAssert);
        player.GetComponent<ScorePointReciever>().PointRecieved.RemoveListener(Player_OnScoreChange);
        GameController.instance.RemainingTimeChanged.RemoveListener(GameController_OnTimeChange);
    }
    public void ShowReloadingAssert()
    {
       StartCoroutine(ReloadingAnimation());
    }

    private IEnumerator ReloadingAnimation()
    {
        Debug.Log("anim start");
        ReloadingAssert.SetEnabled(true);
        yield return new WaitForSeconds(1);
        ReloadingAssert.SetEnabled(false);
    }

    private void WeaponSlotOutline(int arg0)
    {
        if (LastPressed != null)
        {
            LastPressed.style.borderLeftWidth = 0;
            LastPressed.style.borderRightWidth = 0;
            LastPressed.style.borderTopWidth = 0;
            LastPressed.style.borderBottomWidth = 0;
        }
        LastPressed = weaponSlots[arg0];
        int width = 5;
        weaponSlots[arg0].style.borderLeftWidth = width;
        weaponSlots[arg0].style.borderRightWidth = width;
        weaponSlots[arg0].style.borderTopWidth = width;
        weaponSlots[arg0].style.borderBottomWidth = width;
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
    }
    private void OnWeaponSlotClick(ClickEvent evt)
    {
        
    }

    private void OnEnable()
    {
        //Button ChooseFastButton = root.Q<Button>("FastOne");
        //Button ChooseSlowButton = root. Q<Button>("SlowOne");

        //ChooseFastButton.clicked += () => player.ObjectToInstantiate = FastEnemyParticle;
        //ChooseSlowButton.clicked += () => player.ObjectToInstantiate = SlowEnemyParticle;
    }
    void MakeGradient()
    {
        var initialValue = new Gradient();
        initialValue.colorKeys = new GradientColorKey[]
        {
        new GradientColorKey(Color.red, 0),
        new GradientColorKey(Color.blue, 10),
        new GradientColorKey(Color.green, 20)
        };

        // Get a reference to the field from UXML and assign a value to it.
        var uxmlField = weaponSlots[2].Q<GradientField>("the-uxml-field");
        uxmlField.value = initialValue;

        // Create a new field, disable it, and give it a style class.
        var csharpField = new GradientField("C# Field");
        csharpField.SetEnabled(false);
        csharpField.AddToClassList("some-styled-field");
        csharpField.value = uxmlField.value;
        weaponSlots[2].Add(csharpField);
        uxmlField.RegisterCallback<ChangeEvent<Gradient>>((evt) =>
        {
            csharpField.value = evt.newValue;
        });
    }

    internal void OnWin()
    {
        UWintext.visible=true;
        RemainingTimeText.visible = false;
    }internal void OnLose()
    {
        UWintext.visible=true;
        RemainingTimeText.visible = false;
    }
}
