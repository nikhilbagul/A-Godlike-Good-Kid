public class Template : PageManager
{
    // This is the Start() method
    protected override void Start ()
    {
        /* DON'T TOUCH */
        base.Start();
        /* DON'T TOUCH */

        // Enter anything you would in Start() here
    }

    // Call this when next page is to be enabled
    void EnableNext()
    {
        SetSolved();
    }
}
