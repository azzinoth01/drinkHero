using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroHolder : MonoBehaviour
{
    [SerializeField] private List<Hero> heroes = new List<Hero>();
    public List<Hero> Heroes => heroes;


    private static HeroHolder _instance;
    public static HeroHolder Instance => _instance;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {

    }

    public Hero GetHeroById(int id)
    {
        return heroes.FirstOrDefault(x => x.ID == id);
    }
}