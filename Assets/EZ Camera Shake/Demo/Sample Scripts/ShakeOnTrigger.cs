using UnityEngine;
using EZCameraShake;

/*
 * This script begins shaking the camera when the player enters the trigger, and stops shaking when the player leaves.
 */
public class ShakeOnTrigger : MonoBehaviour
{
    //Our saved shake instance.
    private CameraShakeInstance _shakeInstance;

    private PlayerBehaviour scriptReference;
    private bool isHurt;
    void Start()
    {

        scriptReference = GetComponent<PlayerBehaviour>();
        isHurt = scriptReference.isHurt;

        //We make a single shake instance that we will fade in and fade out when the player enters and leaves the trigger area.
        _shakeInstance = CameraShaker.Instance.StartShake(10, 15, 10);

        //Immediately make the shake inactive.  
        _shakeInstance.StartFadeOut(0);

        //We don't want our shake to delete itself once it stops shaking.
        _shakeInstance.DeleteOnInactive = true;
    }

    //When the player enters the trigger, begin shaking.
    void Update()
    {
        //Check to make sure the object that entered the trigger was the player.
        if (isHurt == true)
            _shakeInstance.StartFadeIn(0.1f);
            _shakeInstance.StartFadeOut(0.4f);
    }
}
