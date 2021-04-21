﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace EncryShare
{
    public partial class ClientForm : Form
    {
        bool receive = true;
        Thread receiveFilesThread;
        Thread receiveFileListenerThread;
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        OpenFileDialog getFileDialog = new OpenFileDialog();
        TcpClient tcpClient;
        TcpListener tcpFileListener;
        TcpClient tcpFileClient = new TcpClient();
        NetworkStream nStream;
        NetworkStream fileNStream;
        Thread receiveThread;
        public ClientForm()
        {
            InitializeComponent();
        }

        private void Connect()
        {
            try
            {
                
                tcpClient = new TcpClient(new IPEndPoint(IPAddress.Any, 60755));
                tcpClient.Connect(IPAddress.Parse(ipTextBox.Text), 60755);
                tcpClient.SendTimeout = 4000;
                nStream = tcpClient.GetStream();
                receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start();
                chatTextBox.Text+=("Установлено соединение с " + tcpClient.Client.RemoteEndPoint.ToString()+"\n");
                sendButton.Enabled = true;
                messageTextBox.Enabled = true;
                receiveFileListenerThread = new Thread(WaitFileConnection);
                receiveFileListenerThread.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void WaitFileConnection()
        {
            if (!tcpFileClient.Connected)
            {
                tcpFileListener = new TcpListener(IPAddress.Any, 60766);
                tcpFileListener.Start();
                while (!tcpFileClient.Connected)
                {
                    tcpFileClient = tcpFileListener.AcceptTcpClient();
                    if (tcpFileClient.Connected)
                    {
                        tcpFileListener.Stop();
                        fileNStream = tcpFileClient.GetStream();
                        receiveFilesThread = new Thread(ReceiveFileBytes);
                        receiveFilesThread.Start();
                        chatTextBox.Text += "!READY TO RECEIVE FILE!\n";
                    }
                }
            }
        }

        private void ReceiveFileBytes()
        {
            while (receive)
            {
                try
                {
                    byte[] data = new byte[84748364]; // буфер для получаемых данных
                    int bytes = 0;
                    do
                    {
                        try
                        {
                            bytes = fileNStream.Read(data, 0, data.Length);
                        }
                        catch { }
                    }
                    while (fileNStream.DataAvailable);
                    if (data != new byte[814748364])
                    {
                        FileStream fs = File.Create(Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads" + @"\" + DateTime.Now.Year + DateTime.Now.DayOfYear + DateTime.Now.DayOfWeek + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".encryshare", bytes);
                        fs.Write(data, 0, bytes);
                        fs.Close();
                        chatTextBox.Text += "!FILE RECEIVED!\n(saved to downloads)\n";
                        SendMessage("!FILES TRANSFERED!");
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void ReceiveMessage()
        {
            
            while (tcpClient.Connected)
            {
                try
                {
                    byte[] data = new byte[512]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        try
                        {
                            bytes = nStream.Read(data, 0, data.Length);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        catch { }
                    }
                    while (nStream.DataAvailable);

                    string message = builder.ToString();
                    chatTextBox.Text += $"{DateTime.Now.ToShortTimeString()}:{tcpClient.Client.RemoteEndPoint}:\n" + message+"\n";

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            label2.Text = new WebClient().DownloadString("http://icanhazip.com/");
            sendButton.Enabled = false;
            messageTextBox.Enabled = false;
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            try
            {
                receiveFileListenerThread.Abort();
                tcpFileClient.Client.Shutdown(SocketShutdown.Both);
                tcpFileClient.Close();
                receiveThread.Abort();
                receiveFilesThread.Abort();
                tcpClient.Client.Shutdown(SocketShutdown.Both);
                tcpFileListener.Stop();
                if (tcpClient.Connected)
                {
                    tcpClient.Close();
                }
                nStream.Close();


            }
            catch { }
            finally
            {
                Form1.CloseForm();
            }
        }

        private void SendMessage(string message)
        {
            byte[] msg = Encoding.Unicode.GetBytes(message);
            nStream.Write(msg,0,msg.Length);
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            SendMessage(messageTextBox.Text);
            chatTextBox.Text += $"me:{messageTextBox.Text}\n";
            messageTextBox.Text = "";

        }

        private void messageTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!tcpFileClient.Connected)
            {
                tcpFileClient.Connect(ipTextBox.Text, 60766);
                fileNStream = tcpFileClient.GetStream();
            }
            
            getFileDialog.ShowDialog();
            try
            {

                byte[] data = File.ReadAllBytes(getFileDialog.FileName);
                fileNStream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
