using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TowerManager : Loader<TowerManager>
{
    public TowerButton towerButtonPressed { get; set; }

    SpriteRenderer spriteRenderer;

    private List<TowerControl> TowerList = new List<TowerControl>();
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D buildTile;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {  
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);

            if (hit.collider.tag == "TowerPlace")
            {
                buildTile = hit.collider;
                buildTile.tag = "TowerPlace_taken";
                RegisterBuildSite(buildTile);
                PlaceTower(hit);
            }           
           
        }

        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }

    public void RegisterBuildSite(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }

    public void RegisterTower(TowerControl tower)
    {
        TowerList.Add(tower);
    }

    public void RenameBuildSite()
    {
        foreach (Collider2D buildtag in BuildList)
        {
            buildtag.tag = "TowerPlace";
        }

        BuildList.Clear();
    }

    public void DestroyAllTowers()
    {
        foreach(TowerControl tower in TowerList)
        {
            Destroy(tower.gameObject);
        }

        TowerList.Clear();
    }

    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPressed != null)
        {
            //создаст башню взависимости от выбранной кнопки
            TowerControl newTower = Instantiate(towerButtonPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            BuyTower(towerButtonPressed.TowerPrice);
            RegisterTower(newTower);
            DisactivatePicture();
        }

    }

    public void BuyTower(int price)
    {
        Manager.Instance.subtractGold(price);
    }

    public void SelectedTower(TowerButton towerSelected)
    {
        if(towerSelected.TowerPrice <= Manager.Instance.TotalGold)
        {
            towerButtonPressed = towerSelected;
            ActivatePicture(towerButtonPressed.DragSprite);
        }
        

        //Debug.Log("Pressed" + towerButtonPressed.gameObject);
    }

    public void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void ActivatePicture(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void DisactivatePicture()
    {
        spriteRenderer.enabled = false;
    }

}

