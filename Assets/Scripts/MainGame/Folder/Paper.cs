using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Paper : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform paperObj;

    public bool isOn;

    [SerializeField] private Vector3 amtToMove;

    private FolderManager fldMngr;
    private int idx;

    public void Setup(FolderManager givenFldrMngr, int givenIdx)
    {
        fldMngr = givenFldrMngr;
        idx = givenIdx;
        transform.localPosition = Vector3.zero + amtToMove * idx;
        gameObject.SetActive(false);
    }

    public IEnumerator ResetPos()
    {
        yield return null;

        if (animator.GetBool("Moved"))
        {
            animator.SetTrigger("Reset");
            yield return null;
            animator.SetBool("Moved", false);
        }

        transform.localPosition = Vector3.zero + amtToMove * idx;
        paperObj.localPosition = Vector3.zero;
        paperObj.localEulerAngles = Vector3.zero;
    }

    public void Load(Sprite img)
    {
        isOn = true;
        image.sprite = img;
        transform.localPosition = Vector3.zero + amtToMove * idx;
        gameObject.SetActive(true);
    }

    public void Unload()
    {
        isOn = false;
        image.sprite = null;
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LMB();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            RMB();
        }
    }

    private void LMB()
    {
        fldMngr.PaperMoveNext();
    }

    private void RMB()
    {
        fldMngr.PaperMoveBack();
    }

    public void MoveAnim(bool shouldGoBack)
    {
        animator.SetBool("Moved", !shouldGoBack);
    }

    public void Move(bool shouldGoDown)
    {
        if (shouldGoDown)
            transform.localPosition += amtToMove;
        else
            transform.localPosition += -amtToMove;
    }

    public void FinishedAnim()
    {
        fldMngr.FinishedMovingPapers();
    }
}
