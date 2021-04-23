using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{

    public int weaponNumber = 0;
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = weaponNumber;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(weaponNumber >= transform.childCount - 1)
                weaponNumber = 0;
            else
                weaponNumber++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (weaponNumber <= 0)
                weaponNumber = transform.childCount - 1;
            else
                weaponNumber--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponNumber = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            weaponNumber = 1;
        }
        
        if (previousSelectedWeapon != weaponNumber)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if(i == weaponNumber)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }

    }
}
