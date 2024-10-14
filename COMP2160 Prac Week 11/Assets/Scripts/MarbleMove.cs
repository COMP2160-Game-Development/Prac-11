/**
 * Move the marble towards the target.
 *  
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MarbleMove : MonoBehaviour
{

#region Parameters
    [SerializeField] private float speed = 2f;
#endregion 

#region State
    private Vector3? target;    // null if there is no current target
#endregion 

#region Components
    private new Rigidbody rigidbody;
#endregion

#region Init & Destroy
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        // subscribe to changes in the target position from the UI Mananger
        UIManager.Instance.TargetSelected += OnTargetSelected;
    }

    void OnDisable()
    {
        // unsubscribe while the script is disabled
        UIManager.Instance.TargetSelected -= OnTargetSelected;
    }
#endregion Init

#region FixedUpdate
    void FixedUpdate()
    {
        if (target.HasValue) 
        {
            Vector3 dir = target.Value - rigidbody.position;
            dir.y = 0;  // ignore y component, as the target and marble are at different heights

            if (dir.magnitude < speed * Time.fixedDeltaTime)
            {
                // move direction to the target
                rigidbody.MovePosition(target.Value);
                target = null;

                // stop moving / rolling
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }
            else 
            {
                // move towards the target
                rigidbody.velocity = dir.normalized * speed;
            }
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
#endregion 

#region Event handlers
    public void OnTargetSelected(Vector3 worldPosition)
    {        
        target = worldPosition;
    }
#endregion FixedUpdate
}
