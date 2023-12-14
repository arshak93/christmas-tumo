using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
public class Cancel : MonoBehaviour
{
    public GameObject Object_Menu_Animator;
    
    public void OnClick()
    {  
        Object_Menu_Animator.SetActive(false); 
    }
}
