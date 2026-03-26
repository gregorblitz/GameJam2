using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Image fill;

    public void SetEnergy(float value)
    {
        fill.fillAmount = value;
    }
}