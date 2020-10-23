using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Pin;
    public Rotator rotator;
    [HideInInspector]
    public List<AttachPin> attachedPins;
    [HideInInspector]
    public bool Reverse;

    public Queue<AttachPin> canAttachPins;


    private void Awake()
    {
        attachedPins = new List<AttachPin>();
        canAttachPins = new Queue<AttachPin>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(canAttachPins.Count > 0)
            {
                if (Reverse)
                    rotator.RotateSpeed *= -1;

                StopAllCoroutines();

                AttachPin attachedPin = canAttachPins.Dequeue();
                attachedPins.Add(attachedPin);
                attachedPin.CanMove = true;

                StartCoroutine(SetPositionOtherPins());
            }
        }
    }

    public void SpawnPin(int id)
    {
        for(int i = id;i > 0; i--)
        {
            AttachPin newPin = Instantiate(Pin, (Vector2)transform.position + (Vector2.down * (id - i) * 3f), Quaternion.identity).GetComponent<AttachPin>();
            newPin.SetText(i);
            canAttachPins.Enqueue(newPin);
        }
    }

    IEnumerator SetPositionOtherPins()
    {
        if (canAttachPins.Count > 0)
        {
            AttachPin[] arrayPins = canAttachPins.ToArray();
            float distance;
            while ((distance = Vector3.Distance(transform.position, arrayPins[0].transform.position)) > 0.15f)
            {
                for (int i = 0; i < canAttachPins.Count; i++)
                {
                    arrayPins[i].transform.Translate(Vector2.up * 15 * (distance + 1) * Time.fixedDeltaTime);
                }

                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }
    }
}
