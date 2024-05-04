using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public Player player;
    public FastEnemyParticle FastEnemyParticle;
    public SlowEnemyParticle SlowEnemyParticle;
    private void OnEnable()
    {
        VisualElement root= GetComponent<UIDocument>().rootVisualElement;
        Button ChooseFastButton = root.Q<Button>("FastOne");
        Button ChooseSlowButton = root.Q<Button>("SlowOne");

        ChooseFastButton.clicked += () => player.ObjectToInstantiate = FastEnemyParticle;
        ChooseSlowButton.clicked += () => player.ObjectToInstantiate = SlowEnemyParticle;
    }

}
