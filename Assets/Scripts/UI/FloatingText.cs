using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] Vector3 moveSpeed= new Vector3(0,0,1 );
    [SerializeField] float lifeTime = 1.0f;

    [SerializeField] Image icon;
    [SerializeField] TMP_Text text;

    float currentLifeTime = 1.0f;

    public void Initialize(Vector3 position, int amount, Sprite icon)
    {
        transform.position = position;
        this.icon.sprite = icon;

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
        transform.position = transform.position + moveSpeed * Time.deltaTime;
    }

   // void LateUpdate()
    //{
  //      transform.forward = Camera.main.transform.forward;
   // }
}
