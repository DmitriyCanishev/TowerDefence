using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next, play, gameover, win
}

public class Manager : Loader<Manager>
{

    [SerializeField]
    int totalWave = 10;
    [SerializeField]
    Text totalGoldLabel;
    [SerializeField]
    Text currentWave;
    [SerializeField]
    Text playBtnLabel;
    [SerializeField]
    Text totalEscapedLabel;
    [SerializeField]
    Button playButton;
    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    Enemy[] enemies;
    [SerializeField]
    int totalEnemies = 1; //максимальное кол-во противников за уровень, которые появятся на карте
    [SerializeField]
    int enemiesPerSpawn; //сколько противников могут появится в начальной точке

    int waveNumber = 0;
    int totalGold = 10;
    int totalEscaped = 0;
    int roundEscaped = 0;
    int totalKilled = 0;
    int whichEnemiesToSpawn = 0;
    int enemiesToSpawn = 0;
    gameStatus currentStatus = gameStatus.play;


    public List<Enemy> EnemyList = new List<Enemy>();

    const float spawnDelay = 0.5f;

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }

        set
        {
            totalEscaped = value;
        }
    }

    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }

        set
        {
            roundEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }

        set
        {
            totalKilled = value;
        }
    }

    public int TotalGold
    {
        get
        {
            return totalGold;
        }

        set
        {
            totalGold = value;
            totalGoldLabel.text = TotalGold.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        playButton.gameObject.SetActive(false);
        ShowMenu();

    }

    void Update()
    {
        HandleEscape();
    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {

            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0, enemiesToSpawn)]) as Enemy;
                    newEnemy.transform.position = spawnPoint.transform.position;
                    
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());

        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyEnemies()
    {
        foreach (Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }

        EnemyList.Clear();
    }

    public void addGold(int amount)
    {
        TotalGold += amount;
    }

    public void subtractGold(int amount)
    {
        TotalGold -= amount;
    }

    public void IsWaveOver()
    {
        totalEscapedLabel.text = "Escaped " + TotalEscaped + "/ 10";

        if ((RoundEscaped + TotalKilled) == totalEnemies)
        {
            if (waveNumber <= enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }
            SetCurrentGameState();
            ShowMenu();
        }
    }

    public void SetCurrentGameState()
    {
        if (totalEscaped >= 10)
        {
            currentStatus = gameStatus.gameover;
        }

        else if (waveNumber == 0 && (RoundEscaped + TotalKilled) == 0)
        {
            currentStatus = gameStatus.play;
        }

        else if (waveNumber >= totalWave)
        {
            currentStatus = gameStatus.win;
        }

        else
        {
            currentStatus = gameStatus.next;
        }
    }

    public void PlayButtonPressed()
    {
        //Debug.Log("Play");
        switch (currentStatus)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                

                break;

            default:
                totalEnemies = 1;
                TotalEscaped = 0;
                TotalGold = 10;
                enemiesToSpawn = 0;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameBuildSite();
                totalGoldLabel.text = TotalGold.ToString();
                totalEscapedLabel.text = "Escaped " + totalEscaped + "/ 10";

                break;
        }

        DestroyEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWave.text = "Wave " + (waveNumber + 1);
        StartCoroutine(Spawn());
        playButton.gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        switch (currentStatus)
        {
            case gameStatus.gameover:
                playBtnLabel.text = "Play Again!";

                break;

            case gameStatus.next:
                playBtnLabel.text = "Next Wave";

                break;

            case gameStatus.play:
                playBtnLabel.text = "Play Game";

                break;

            case gameStatus.win:
                playBtnLabel.text = "New Game";

                break;
        }

        playButton.gameObject.SetActive(true);
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisactivatePicture();
            TowerManager.Instance.towerButtonPressed = null;
        }
    }

}
