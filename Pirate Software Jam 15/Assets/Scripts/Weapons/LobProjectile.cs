using UnityEngine;

public class LobProjectile : MonoBehaviour
{
  //flask
  [Tooltip("The flask Prefab. The flask itself contains its own information on contact and break.")]
  public GameObject flask;

  [Header("Forces & position")]
  [Tooltip("The x component of the player's throw.")]
    public float throwForce;
  [Tooltip("The y component of the player's throw.")]
    public float upwardPush;
  [Tooltip("The point at which the flask is instantiated. This allows the player to throw the flask from a place not inside their model.")]
    public Transform throwOrigion;

  [Header("Bag & Shooting")]
  [Tooltip("Amount of flasks the player can throw.")]
    public int bagSize = 5;
  [Tooltip("How many flasks would be thrown on one throw input.")]
    public int flasksThrownOnClick = 1;
  [Tooltip("Allow the user to hold the throw button to shoot, if true.")]
    public bool allowButtonHold = false;

  //flask restrictions
  [Header("Flask Restrictions")]
  public float recollecting;
  public float spread;
  public float recollectingTime;
  public float timeBetweenThrows;



  private int flasksLeft, flasksThrown;

  //bools
  bool throwing, ableToThrow, reloading;

  //reference
  private PotionManager potionManager;

  public bool allowthrow = true;

  private void Awake()
  {
    //make sure bag is filled
        flasksLeft = bagSize;
        ableToThrow = true;

    potionManager = GameObject.Find("Potion Manager").GetComponent<PotionManager>();
  }

  void Update()
  {
    int beforeIndex = potionManager.potionIndex;

    if(Input.GetAxis("Mouse ScrollWheel") > 0f)
    {
      potionManager.potionIndex = ((potionManager.potionIndex+1)%(potionManager.potionsList.Length+1));
    }
    if(Input.GetAxis("Mouse ScrollWheel") < 0f)
    {
      potionManager.potionIndex = ((potionManager.potionIndex-1)%(potionManager.potionsList.Length+1));
    }
    if(potionManager.potionIndex < 0)
    {
      potionManager.potionIndex = 1;
    }

    if (beforeIndex != potionManager.potionIndex)
    {
      potionManager.UpdateUI();

      potionManager.UpdateUISprites();
    }

    

    if(Input.GetKeyDown(KeyCode.Mouse0))
    {
      MyInput(ref potionManager.potionsList[potionManager.potionIndex]);

      potionManager.UpdateUISprites();
    }
  }

  public void MyInput(ref Potions potion)
  {
    if(ableToThrow && !reloading && potion.amountHeld > 0)
    {
      //Set flasks shot to 0
        flasksThrown = 0;

      for (int i=flasksThrownOnClick; i>0; i--)
      {
        if (potion.amountHeld <= 0) break;

        Lob(ref potion);
      }
    }
  }

  public void Lob(ref Potions potion)
  {
    ableToThrow = true;

    potion.amountHeld--;
    flasksThrown++;

    GameObject obj = Instantiate(potion.Brew, throwOrigion.position, Quaternion.identity);

    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
    
    // get flask's rigidbody -> add force to rigidbody that is equal to: preset var times direction facing * strength
    Vector2 throwVector = new Vector2(throwForce, upwardPush);
    rb.AddForce(throwVector * transform.localScale * rb.mass);
    rb.AddTorque(throwForce/6 * -transform.localScale.x * rb.mass);

  }

}

