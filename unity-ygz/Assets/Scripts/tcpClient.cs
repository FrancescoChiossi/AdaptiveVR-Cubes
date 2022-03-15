using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class tcpClient : MonoBehaviour
{
	#region private members 	
	private TcpClient socketConnection;
	private Thread clientReceiveThread;
	#endregion


	public float NewAverage { get; private set; }

	int i = 0;

	// Use this for initialization 	
	async void Awake()
	{
		socketConnection = new TcpClient("localhost", 8052);
		/*await Task.Run(async () => {
            while (true) {
				//var average = await SendMessage("-0.010214_0.009999_0.011373_0.011115_0.010600_-0.005107_0.006480_0.005965_0.005879_0.007081_-0.010214_0.009999_0.011373_0.011115_0.010600_-0.005107_0.006480_0.005965_0.005879_0.007081_-0.010214_0.009999_0.011373_0.011115_0.010600_-0.005107_0.006480_0.005965_0.005879_0.007081_"); // sample data, remove later
				//Debug.Log(average);
			}
		});*/
	}

    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    public List<float> SendMessage(string clientMessage)
	{
		if (socketConnection == null)
		{
			Debug.Log("error");
			return null;
		}
		try
		{
			// Get a stream object for writing.
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite)
			{
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.UTF8.GetBytes(clientMessage);
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
			}

			var valueList = new List<float>();
			// Listen to new response
			while (!stream.DataAvailable)
			{
				if (socketConnection == null)
				{
					Debug.Log("socket interruption");
					break;
				}
			}

			Byte[] bytes = new Byte[1024];
			StringBuilder serverMessage = new StringBuilder();
			int length;
			// Read incomming stream into byte arrary. 	
			do
			{
				length = stream.Read(bytes, 0, bytes.Length);
				serverMessage.AppendFormat("{0}", Encoding.UTF8.GetString(bytes, 0, length));
			} while (stream.DataAvailable);
			Debug.Log("server message received as: " + serverMessage);
			// Converts the average string to floating point number
			//NewAverage = float.Parse(serverMessage.ToString());
			var listStr = serverMessage.ToString().Split('_');

			foreach (var value in listStr)
			{
				valueList.Add(float.Parse(value));
			}
			return valueList;
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
			return null;
		}
	}
}
