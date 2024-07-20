using UnityEngine;

public class LobProjectile : MonoBehaviour
{
      //flask
      public GameObject flask;

      //flask forces
        public float throwForce, upwardPush;

      //flask restrictions
        public float recollecting, spread, recollectingTime, timeBetweenThrows;
        public int bagSize, flasksThrownOnClick;
        public bool allowButtonHold;

        int flasksLeft, flasksThrown;

        //bools
        bool throwing, ableToThrow, reloading;

        //reference
            public GameObject directionFaced;
            public Transform throwOrigion;

            public bool allowthrow = true;

            public Vector2 throwVector = new Vector2(1,1);
            public float strength = 2f;

        private void Awake()
        {
            //make sure bag is filled
                flasksLeft = bagSize;
                ableToThrow = true;
        }

        private void Update()
        {
            MyInput();

        }

        private void MyInput()
        {
            //check if user can hold the fire button and accept input
            if(allowButtonHold) throwing = Input.GetKey(KeyCode.Mouse0);
            else throwing = Input.GetKeyDown(KeyCode.Mouse0);

            //throwing
              if(ableToThrow && throwing && !reloading && flasksLeft > 0)
              {
                //Set flasks shot to 0
                    flasksThrown = 0;

                    Lob();
              }
        }

        private void Lob()
        {
            ableToThrow = true;

            flasksLeft--;
            flasksThrown++;

            GameObject obj = Instantiate(flask, throwOrigion.position, Quaternion.identity);

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            
            // get flask's rigidbody -> add force to rigidbody that is equal to: preset var times direction facing * strength
            rb.AddForce(throwVector * transform.localScale * strength * rb.mass);
            rb.AddTorque(strength/6 * -transform.localScale.x * rb.mass);

        }

}

