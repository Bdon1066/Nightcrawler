using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    public Material transparent;
    public Material blue;
    public Material gold;
    public Material brown;

    public void TransparentGraphics()
    {
        GetComponent<MeshRenderer>().material = transparent;

        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().material = transparent;
        }

    }
    public void ResetGraphics()
    {
        GetComponent<MeshRenderer>().material = blue;

        foreach (Transform child in transform)
        {
            switch (child.tag)
            {
                case("Blue"):
                    child.GetComponent<MeshRenderer>().material = blue;
                    break;
                case ("Brown"):
                    child.GetComponent<MeshRenderer>().material = brown;
                    break;
                case ("Gold"):
                    child.GetComponent<MeshRenderer>().material = gold;
                    break;
            }
        }

    }
}
