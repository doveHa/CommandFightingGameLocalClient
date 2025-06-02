using UnityEngine;

public class ShowCustomCommandHandler : MonoBehaviour
{
    private bool isShow = false;

    public void ChangeState(GameObject menu)
    {
        if (isShow)
        {
            menu.SetActive(false);
            isShow = false;
        }
        else
        {
            menu.SetActive(true);
            isShow = true;
        }
    }
}