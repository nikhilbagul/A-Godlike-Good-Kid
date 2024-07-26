using UnityEngine;
using System.Collections;

public class ClockHandAnimator : MonoBehaviour
{
    public enum Trigger { Oscillate, Spin };
    public Trigger trigger = 0;
    public float anglesPerSecond, spinMultiplier = 5;

    private delegate void _Behaviour();
    private _Behaviour Behaviour;

    private int osc_direction = 1;

	void Update ()
    {
        switch (trigger)
        {
            case Trigger.Oscillate:
                Behaviour = Oscillate;
                break;

            case Trigger.Spin:
                Behaviour = Spin;
                break;
        }
        Behaviour();
	}

    void Oscillate()
    {
        transform.Rotate(0, 0, anglesPerSecond * Time.deltaTime * osc_direction);
        if (transform.rotation.eulerAngles.z <= 1.0f)
            osc_direction *= -1;
    }

    void Spin()
    {
        transform.Rotate(0, 0, anglesPerSecond * Time.deltaTime * osc_direction * spinMultiplier);
    }

    public void SetTrigger(Trigger trigger)
    {
        this.trigger = trigger;
    }
}
