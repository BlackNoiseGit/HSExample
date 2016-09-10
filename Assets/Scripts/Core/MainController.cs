using UnityEngine;
using System;
using System.Collections;

public class MainController : MonoBehaviour {

    public static MainController instance = null;
    [HideInInspector]
    public DelayService DelayService { get { return _DelayService; } }
    [HideInInspector]
    public PickController PickController { get { return _PickController; } }
    [HideInInspector]
    public DeckController DeckController { get { return _DeckController; } }

    [SerializeField]
    private DeckController _DeckController;
    private DelayService _DelayService;
    private PickController _PickController;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
       
        DontDestroyOnLoad(gameObject);

        _DelayService = GetComponent<DelayService>();
        if (_DelayService == null)
            _DelayService = gameObject.AddComponent<DelayService>();

        _PickController = GetComponent<PickController>();
        if (_PickController == null)
            _PickController = gameObject.AddComponent<PickController>();
    }
}
