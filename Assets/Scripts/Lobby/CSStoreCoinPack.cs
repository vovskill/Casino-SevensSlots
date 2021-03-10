using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CSStoreCoinPack : MonoBehaviour {

	public Text price;
	public Text total;
    public CSIAPProduct product;

    public string productId {
        get { return product.productId; }
    }

	void Start ()
	{
		
	}

	public int Total()
	{
		return int.Parse (total.text.Replace (" ", string.Empty));
	}

	public void SetPrice(string value)
	{
		price.text = value;
	}
}
