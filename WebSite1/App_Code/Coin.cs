using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Coin için özet açıklama
/// </summary>
public class Coin
{
    public string coinId;
    public string coinName;
    public string coinPair;
    public string coinPrice;
    public Coin()
    {

    }
    public Coin(string cId, string cName, string cPair, string cPrice)
    {
        this.coinId = cId;
        this.coinName = cName;
        this.coinPair = cPair;
        this.coinPrice = cPrice;
    }
   
}