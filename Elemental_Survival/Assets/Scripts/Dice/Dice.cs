using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Dice : MonoBehaviour
{
    [SerializeField] private float angularForce = 10f;
    [SerializeField] private Vector3 forceDirection = Vector3.up;

    private Rigidbody diceObj;
    
    public int Value { get; private set; }

    private void Awake()
    {
        diceObj = GetComponent<Rigidbody>();
    }

    public YieldInstruction Roll()
    {
        StopAllCoroutines();
        diceObj.linearVelocity = forceDirection;
        diceObj.angularVelocity = Random.insideUnitSphere * angularForce;

        return StartCoroutine(RollCoroutine());
    }

    private IEnumerator RollCoroutine()
    {
        while (diceObj.linearVelocity.sqrMagnitude > 0f || diceObj.angularVelocity.sqrMagnitude > 0f)
        {
            yield return null;
        }

        var directions = new Vector3[]
        {
            -transform.up,      // 1
            -transform.right,   // 2
            -transform.forward, // 3
            transform.forward,  // 4
            transform.right,    // 5
            transform.up,       // 6
        };

        float dis = float.MaxValue;
        int value = 0;
        for (int i = 0; i < 6; i++)
        {
            // 감지된 레이의 길이는 이전에 갱신된 거리보다 짧을 수 밖에 없음
            if (Physics.Raycast(transform.position, directions[i], out RaycastHit hit, dis, 0b10000000))
            {
                dis = hit.distance;
                value = i + 1;
            }
        }
        Value = value;
    }
}
