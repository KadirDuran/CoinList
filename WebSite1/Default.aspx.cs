using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Default : System.Web.UI.Page
{
    SqlConnection connection = new SqlConnection("server=89.252.181.210\\MSSQLSERVER2017;database=kadirdur_coin;UID=kadirdur_coin;pwd=Kanarya12;MultipleActiveResultSets=True");
    protected void Page_Load(object sender, EventArgs e)
    {
       
        showData(processData(getData()));
        showDataOnGridview();

    }
    string getData()
    {
        WebClient w = new WebClient();
        return w.DownloadString("https://data.messari.io/api/v1/assets?fields=id,slug,symbol,metrics/market_data/price_usd");

    }
    List<Coin> processData(string data)
    {
        JObject o = JObject.Parse(data);
        JArray array = (JArray)o["data"];
        List<Coin> c = new List<Coin>();
        for (int i = 0; i <= array.Count-1; i++)
        {
            Coin coin = new Coin();
            coin.coinId = array[i]["id"].ToString();
            coin.coinName = array[i]["symbol"].ToString();
            coin.coinPair = array[i]["slug"].ToString();
            coin.coinPrice = array[i]["metrics"]["market_data"]["price_usd"].ToString();

            c.Add(coin);

        }
        
        return c;
    }
    void showData(List<Coin> coinList)
    {
        Label1.Text = "";
        for (int i = 0; i < coinList.Count - 1; i++)
        {
            //insertData(coinList[i]); //Bir kere çalıştırıp verileri kaydet.
            Label1.Text += "Name : " + coinList[i].coinPair + "<br>Symbol : " + coinList[i].coinName + "<br>Price : " + coinList[i].coinPrice + "<hr>";
            updateDatabase(coinList[i]);
        }
    }
    void updateDatabase(Coin coin)
    {
        connection.Open();
        SqlCommand command = new SqlCommand("update dbCoin set Price=@price where Id=@id", connection);
        command.Parameters.AddWithValue("@id", coin.coinId);
        command.Parameters.AddWithValue("@price", coin.coinPrice);
        
        command.ExecuteNonQuery();
        connection.Close();
    }
    void insertData(Coin coin)
    {
        connection.Open();
        SqlCommand command = new SqlCommand("insert into dbCoin(Id,Symbol,Name,Price) values (@id,@name,@pair,@price)",connection);
        command.Parameters.AddWithValue("@id", coin.coinId);
        command.Parameters.AddWithValue("@name", coin.coinName);
        command.Parameters.AddWithValue("@pair", coin.coinPair);
        command.Parameters.AddWithValue("@price", coin.coinPrice);
        command.ExecuteNonQuery();
        connection.Close();
        
    }
    void showDataOnGridview()
    {
        connection.Open();
        DataTable dataTable = new DataTable();
        SqlDataAdapter adapter = new SqlDataAdapter("Select * from dbCoin", connection);

        adapter.Fill(dataTable);
        GridView1.DataSource = dataTable;
        DataBind();
        connection.Close();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        showData(processData(getData()));
        showDataOnGridview();
       
        
    }
    
}