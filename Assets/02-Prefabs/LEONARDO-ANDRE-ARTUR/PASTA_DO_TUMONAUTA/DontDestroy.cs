using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    [SerializeField]
    public List<Radio> radios = new List<Radio>();

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach(var r in radios)
        {
            if(SceneManager.GetActiveScene().name == r.scene)
            {
                transform.position = r.posicao.position;
            }
        }
    }

}

[System.Serializable]
public class Radio
{
    public string scene;

    [SerializeField]
    public Transform posicao;
}
