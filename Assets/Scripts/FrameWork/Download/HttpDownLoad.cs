using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO;
using System.Net;
using System;


public class HttpDownLoad {
	
	public float progress{get; private set;}
	private bool isStop;
	private Thread thread;
	public bool isDone{get; private set;}


	/// <summary>
	/// 下载方法(断点续传)
	/// </summary>
	/// <param name="url">URL下载地址</param>
	/// <param name="savePath">Save path保存路径</param>
	/// <param name="callBack">Call back回调函数</param>
	public void DownLoad(string url, string savePath, Action callBack)
	{
		isStop = false;
		thread = new Thread(delegate() {			
			FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write);
			long fileLength = fs.Length;
			UnityEngine.Debug.Log(111);
			long totalLength = GetLength(url);
			UnityEngine.Debug.Log(222);
			
			
			//断点续传
			if(fileLength < totalLength)
			{
				
				//设置本地文件流的起始位置
				fs.Seek(fileLength, SeekOrigin.Begin);

				HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;

				//设置远程访问文件流的起始位置
				request.AddRange((int)fileLength);
				Stream  stream = request.GetResponse().GetResponseStream();

				byte[] buffer = new byte[1024];
				//使用流读取内容到buffer中
				int length = stream.Read(buffer, 0, buffer.Length);
				while(length > 0)
				{
					//如果Unity客户端关闭，停止下载
					if(isStop)
                        break;
					//将内容再写入本地文件中
					fs.Write(buffer, 0, length);
					fileLength += length;
					progress = (float)fileLength / (float)totalLength;
					UnityEngine.Debug.Log(progress);
					length = stream.Read(buffer, 0, buffer.Length);
				}
				stream.Close();
				stream.Dispose();

			}
			else
			{
				progress = 1;
			}
			fs.Close();
			fs.Dispose();
			//下载完毕，执行回调
			if(progress == 1)
			{
				isDone = true;
				if(callBack != null) callBack();
			}
			UnityEngine.Debug.Log (1111);	 
		});
		//开启子线程
		thread.IsBackground = true;
		thread.Start();
	}


	/// <summary>
	/// 获取下载文件的大小
	/// </summary>
	/// <returns>The length.</returns>
	/// <param name="url">URL.</param>
	long GetLength(string url)
	{
		UnityEngine.Debug.Log(url);
		
		HttpWebRequest requet = HttpWebRequest.Create(url) as HttpWebRequest;
		requet.Method = "HEAD";
		HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
		return response.ContentLength;
	}

	public void Close()
	{
		isStop = true;
	}

}
