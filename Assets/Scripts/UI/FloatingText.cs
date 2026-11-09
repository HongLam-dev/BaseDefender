using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] Vector2 moveSpeed= new Vector2(0,1 );
    [SerializeField] float lifeTime = 1.0f;
    TMP_Text text;

    float currentLifeTime = 1.0f;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void Initialize(Vector2 position, int amount)
    {
        transform.position = position;
        currentLifeTime = lifeTime;
        if(amount>0)
        {
            text.color =Color.green ;
            text.text ="+"+ amount.ToString();
        }
        else
        {
            text.color = Color.red;
            text.text = "-" + amount.ToString();
        }
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0)
            gameObject.SetActive(false);
        transform.position = transform.position + new Vector3 (moveSpeed.x,moveSpeed.y,0) * Time.deltaTime;
    }
}
