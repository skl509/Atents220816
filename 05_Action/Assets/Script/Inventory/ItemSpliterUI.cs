using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSpliterUI : MonoBehaviour
{
    uint itemSplitCount = 1;

    ItemSlot targetSlot;

    TMP_InputField inputField;
    Slider slider;
    Image image;

    bool ischange;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();

        slider = GetComponentInChildren<Slider>();

        Button increase = transform.GetChild(1).GetComponent<Button>();
        Button decrease = transform.GetChild(2).GetComponent<Button>();
        Button ok = transform.GetChild(4).GetComponent<Button>();
        Button cancel = transform.GetChild(5).GetComponent<Button>();
        image = transform.GetChild(6).GetComponent<Image>();
    }
    public bool Ischange
    {
        get => ischange;
        set
        {
            ischange = value;
            if (ischange)     // 일시 정지가 되면 무조건 상세정보 창을 닫는다.
            {
                 
            }
        }
    }
    private void Start()
    {
        Close();
    }

    private void Update()
    {
        
    }

    public void Open(ItemSlotUI target)
    {
        targetSlot = target.ItemSlot;

        Debug.Log($"{targetSlot.ItemData.itemName} : {targetSlot.ItemCount}개");
        Show(targetSlot.ItemData);
        this.gameObject.SetActive(true);
       
        
    }

    public void Show(ItemData itemData)
    {
        image.sprite = itemData.itemIcon;

    }
    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Increase() 
    {
        itemSplitCount += 1; 
    
    }

    public void Decrease()
    {
        itemSplitCount -= 1;

    }



}
