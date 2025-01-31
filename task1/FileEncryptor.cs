using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace task1
{
    public class FileEncryptor
    {
        public WorkerResult ProcessFile(string filePath, string key, bool isEncryption, BackgroundWorker worker)
        {
            string outputFilePath = filePath + (isEncryption ? ".enc" : ".dec");
            byte[] buffer = new byte[8192];
            long totalBytes = new FileInfo(filePath).Length;
            long processedBytes = 0;

            var aes = Aes.Create();
            aes.Key = GenerateKey(key);
            aes.IV = GenerateIV(key);

            try
            {
                using (FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                using (CryptoStream cryptoStream = new CryptoStream(outputFileStream,
                        isEncryption ? aes.CreateEncryptor() : aes.CreateDecryptor(),
                        CryptoStreamMode.Write))
                {
                    int bytesRead;
                    while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cryptoStream.Write(buffer, 0, bytesRead);
                        processedBytes += bytesRead;
                        int progress = (int)((double)processedBytes / totalBytes * 100);
                        worker.ReportProgress(progress);
                    }
                }

                return new WorkerResult
                {
                    FilePath = outputFilePath,
                    FileSize = new FileInfo(outputFilePath).Length,
                    ElapsedTime = DateTime.Now - DateTime.Now 
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during {(isEncryption ? "encryption" : "decryption")}: {ex.Message}");
            }
        }

        private byte[] GenerateKey(string key)
        {
            return Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
        }

        private byte[] GenerateIV(string key)
        {
            return Encoding.UTF8.GetBytes(key.PadRight(16).Substring(0, 16));
        }
    }
}
