using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Core;
using ZipEntry = ICSharpCode.SharpZipLib.Zip.ZipEntry;

public class ZipUtil
{
#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern void unzip (string zipFilePath, string location);

	[DllImport("__Internal")]
	private static extern void zip (string zipFilePath);

	[DllImport("__Internal")]
	private static extern void addZipFile (string addFile);

#endif

	public static void Unzip (string zipFilePath, string location)
	{
        Debug.Log(zipFilePath);
        Debug.Log(location);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
        ICSharpCode.SharpZipLib.Zip.ZipFile file = null;
            try
            {
                FileStream fs = File.OpenRead(zipFilePath);
                file = new ICSharpCode.SharpZipLib.Zip.ZipFile(fs);
        
                foreach (ZipEntry zipEntry in file)
                {
                    if (!zipEntry.IsFile)
                    {
                        // Ignore directories
                        continue;           
                    }
        
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.
        
                    // 4K is optimum
                    byte[] buffer = new byte[4096];     
                    Stream zipStream = file.GetInputStream(zipEntry);
        
                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(location, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
        
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }
        
                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (file != null)
                {
                    file.IsStreamOwner = true; // Makes close also shut the underlying stream
                    file.Close(); // Ensure we release resources
                }
            }
#elif UNITY_ANDROID
		using (AndroidJavaClass zipper = new AndroidJavaClass ("com.tsw.zipper")) {
			zipper.CallStatic ("unzip", zipFilePath, location);
		}
#elif UNITY_IPHONE
		unzip (zipFilePath, location);
#elif UNITY_WEBGL
		ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
		ICSharpCode.SharpZipLib.Zip.ZipFile file = null;
            try
            {
                FileStream fs = File.OpenRead(zipFilePath);
                file = new ICSharpCode.SharpZipLib.Zip.ZipFile(fs);
        
                foreach (ZipEntry zipEntry in file)
                {
                    if (!zipEntry.IsFile)
                    {
                        // Ignore directories
                        continue;           
                    }
        
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.
        
                    // 4K is optimum
                    byte[] buffer = new byte[4096];     
                    Stream zipStream = file.GetInputStream(zipEntry);
        
                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(location, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
        
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }
        
                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (file != null)
                {
                    file.IsStreamOwner = true; // Makes close also shut the underlying stream
                    file.Close(); // Ensure we release resources
                }
            }

#endif
	}
}
