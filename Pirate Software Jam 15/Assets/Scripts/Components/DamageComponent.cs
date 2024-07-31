using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEditor;
#endif
using UnityEngine;
using System;

public class DamageComponent : MonoBehaviour
{
    public int damage = 10;
    public string[] damageableTags;

    // enum for type of damage component
    [SerializeField] public DamageType damageType;

    // DOT
    [SerializeField] public float delayInterval;
    [SerializeField] public float uptime = 5f;
    private float delay = 0f, life;
    [SerializeField] public ParticleSystem dotParticles;

    // Single
    [SerializeField] public float swingDelay;

    // Impact
    [SerializeField] public Vector2 impactVector;

    //killing enemy event
    public delegate void OnKill();
    public event OnKill onKill;

    private Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        // add onKill events here

        if (uptime > 0)
            life = uptime;

        dir = Vector2.zero;

        if (damageType is DamageType.DOT)
        {
            ParticleSystem particles = Instantiate(dotParticles, transform.position, Quaternion.identity);
            particles.transform.parent = transform;
            particles.transform.localScale = new Vector3(1,1,1);

            var particleMain = particles.main;
            particleMain.duration = uptime;

            particles.Play();
        }
    }

    void Update()
    {
        if (GameManager.instance.inMenu)
            return;

        if (delay > 0)
            delay -= Time.deltaTime;

        if (damageType is DamageType.DOT)
        {
            life -= Time.deltaTime;

            if (life <= 0)
                Destroy(gameObject);
        }

        dir.x = transform.localScale.x;
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (isDamageable(damageableTags, collision.gameObject.tag))
        {
            if (damageType is DamageType.Thorns) DamageEntity(collision.collider);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isDamageable(damageableTags, other.gameObject.tag))
        {
            if (delay <= 0 && damageType is DamageType.DOT && !other.isTrigger)
            {
                DamageEntity(other);
                delay = delayInterval;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (damageType is DamageType.Single && !other.isTrigger && isDamageable(damageableTags, other.gameObject.tag)) DamageEntity(other.GetComponent<Collider2D>());
    }

    void DamageEntity(Collider2D entity)
    {
        // get object's health
        HealthComponent hpComp = entity.gameObject.GetComponent<HealthComponent>();

        // if enemy dies from attack -> call KilledEnemy
        if (hpComp != null)
        {
            hpComp.onDeath += KilledEnemy;
            entity.gameObject.GetComponent<HealthComponent>().TakeDamage(damage, new Vector2(transform.lossyScale.x, 0));
            hpComp.onDeath -= KilledEnemy;
        }
    }

    // When an enemy is killed, call the event.
    // Will be used for XP or other abilities maybe...
    void KilledEnemy(){
        onKill?.Invoke();
    }

    void OnDrawGizmos()
    {
        if (damageType is DamageType.DOT)
        {
            Gizmos.color = new Color(0, delay < 0 ? 1 : 0, 1, .3f);
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }

    bool isDamageable (string[] damageableTags, string tag)
    {
        foreach (string listTag in damageableTags){
            // if object is attackable -> get health component and damage it
            if(tag == listTag){
                return true;
            }
        }

        return false;
    }
}







#if UNITY_EDITOR // =>
/* This is a custom Editor. Instead of using the inspector's default GUI, we can make our own.
    I did this so that I can use the enum to show/hide values & make a modular damage component we
    can slap on anything -C */
[CustomEditor(typeof(DamageComponent))]
[CanEditMultipleObjects]
public class DamageComponentEditor : Editor
{

    private DamageComponent dc;

    // Grabs Script variables to set them to GUI values
    private void OnEnable()
    {
        dc = target as DamageComponent;
    }

    // bool for garbage down at the bottom -C
    private bool isOpened;

    public override void OnInspectorGUI()
    {
        dc.damageType = (DamageType)EditorGUILayout.EnumPopup("Damage Type", dc.damageType);
        // Tags entity can damage
        dc.damageableTags = StringArrayField("Damageable Tags", ref isOpened, dc.damageableTags);

        // DOT attacks
        if (dc.damageType is DamageType.DOT)
        {
            dc.delayInterval = EditorGUILayout.FloatField("Tick Delay", dc.delayInterval);
            dc.uptime = EditorGUILayout.FloatField("AOE Uptime", dc.uptime);
            dc.damage = EditorGUILayout.IntField("Damage Per Tick", dc.damage);
            dc.dotParticles = (ParticleSystem)EditorGUILayout.ObjectField("DOT Particles", dc.dotParticles, typeof(ParticleSystem));
        }

        // Slashes/single swings
        else if (dc.damageType is DamageType.Single)
        {
            dc.damage = EditorGUILayout.IntField("Damage", dc.damage);
            dc.swingDelay = EditorGUILayout.FloatField("Swing Delay", dc.swingDelay);
        }

        // Attacks with Knockback
        else if (dc.damageType is DamageType.Impact)
        {
            dc.damage = EditorGUILayout.IntField("Damage", dc.damage);
            dc.impactVector = EditorGUILayout.Vector2Field("Knockback Vector", dc.impactVector);
        }

        // Reflected Damage from Melee Attacks
        else if (dc.damageType is DamageType.Thorns)
        {
            dc.damage = EditorGUILayout.IntField("Damage", dc.damage);
        }

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    /* Don't worry about all this code down here, I copied it off the web.
       All you need to know is that you can't put arrays in EditorGUIs so some guy made stuff for that
       Therefore, the damageableTags array uses this :) -C */
    public string[] StringArrayField(string label, ref bool open, string[] array) {
        // Create a foldout
        open = EditorGUILayout.Foldout(open, label);
        int newSize = array.Length;

        // Show values if foldout was opened.
        if (open) {
            // Int-field to set array size
            newSize = EditorGUILayout.IntField("Size", newSize);
            newSize = newSize < 0 ? 0 : newSize;

            // Creates a spacing between the input for array-size, and the array values.
            EditorGUILayout.Space();

            // Resize if user input a new array length
            if (newSize != array.Length) {
                array = ResizeArray(array, newSize);
            }

            // Make multiple int-fields based on the length given
            for (var i = 0; i < newSize; ++i) {
                array[i] = EditorGUILayout.TextField($"Value-{i}", array[i]);
            }

            EditorGUILayout.Space();
        }
        return array;
    }

    private static T[] ResizeArray<T>(T[] array, int size) {
        T[] newArray = new T[size];

        for (var i = 0; i < size; i++) {
            if (i < array.Length) {
                newArray[i] = array[i];
            }
        }

        return newArray;
    }
}
#endif