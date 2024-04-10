using Data;
using Gameplay.Core;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    private DungeonEnemyProvider _provider;

    [SerializeField] private BossData data;

    [SerializeField] private TMP_Text progressText;
    
    [SerializeField] private TMP_Text enemyInfo;

    [SerializeField] private int playerNum;
    
    void Start()
    {
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(new Vector2(0, 0), new Vector2(300, 300)));
        if (GUILayout.Button("初始化"))
        {
            _provider = new DungeonEnemyProvider(playerNum, data);
        }
        if (GUILayout.Button("获取下一个对象"))
        {
            var enemyCard = _provider.GetNextEnemyCard();
            enemyInfo.text = $"{enemyCard.ToReadableString()}";
            progressText.text = $"{_provider.CurProgress}/{_provider.TotalLevelNum}";
        }
        GUILayout.EndArea();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
