using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

public partial class My
{
	public class FTP
	{
		FtpWebRequest request;
		public FTP(string URL, string username, string password)
		{
			request = (FtpWebRequest)FtpWebRequest.Create("ftp://"+ URL ); //"ftp://ftp.adrive.com/" + filename);
			request.Credentials = new NetworkCredential(username,password); //"pieterpuk@gmail.com", "Bertus01");
			request.KeepAlive = false;
		}
		public void UploadFile(string filename)
		{
			request.Method = WebRequestMethods.Ftp.UploadFile;
		}
		public void DownloadFile(string filename)
		{
		}
		private void MakeDirectory(string path)
		{
		}
		private void ChangeDirectory(string path)
		{
		}
	}
}
