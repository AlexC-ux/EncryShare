﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace EncryShare
{
    public partial class ServerForm : Form
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        OpenFileDialog getFileDialog = new OpenFileDialog();
        bool receive = true;
        Thread receiveFilesThread;
        Thread receiveFileListenerThread;
        TcpListener tcpListener;
        TcpClient tcpClient;
        TcpListener tcpFileListener;
        TcpClient tcpFileClient = new TcpClient();
        NetworkStream nStream;
        NetworkStream fileNStream;
        bool listen = true;
        Thread listenThread;
        Thread receiveThread;
        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            label2.Text = new WebClient().DownloadString("http://icanhazip.com/");
            sendButton.Enabled = false;
            messageTextBox.Enabled = false;
        }

        private void StartListening()
        {
            // Устанавливаем для сокета локальную конечную точку
            tcpListener = new TcpListener(IPAddress.Any, 60755);
            tcpListener.Start();
            

            try
            {
                
                
                // Начинаем слушать соединения
                while (listen)
                {
                    tcpClient = tcpListener.AcceptTcpClient();
                    if (tcpClient.Client.RemoteEndPoint.ToString().Split(':')[0] != ipTextBox.Text)
                    {
                        MessageBox.Show("К вам желал подключиться незнакомый клиент\n"+ tcpClient.Client.RemoteEndPoint.ToString());
                    }
                    else
                    {
                        
                        tcpListener.Stop();
                        listen = false;
                        nStream = tcpClient.GetStream();
                        receiveThread = new Thread(ReceiveMessage);
                        receiveThread.Start();
                        chatTextBox.Text = ("Установлено соединение с " + tcpClient.Client.RemoteEndPoint.ToString() + "\n");
                        sendButton.Enabled = true;
                        messageTextBox.Enabled = true;
                        receiveFileListenerThread = new Thread(WaitFileConnection);
                        receiveFileListenerThread.Start();
                    }
                }
            }
            catch (Exception ex)
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
                    chatTextBox.Text += $"{DateTime.Now.ToShortTimeString()}:{tcpClient.Client.RemoteEndPoint}:\n" + message + "\n";

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            listenThread = new Thread(StartListening);
            listenThread.Start();
            listenButton.Enabled = false;
            ipTextBox.Enabled = false;
        }

        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (receiveThread!=null)
                {
                    receiveThread.Abort();
                }
                receiveFileListenerThread.Abort();
                tcpFileClient.Client.Shutdown(SocketShutdown.Both);
                tcpFileClient.Close();
                tcpClient.Client.Shutdown(SocketShutdown.Both);
                nStream.Close();
                tcpListener.Stop();
                receiveFilesThread.Abort();
                if (tcpClient.Connected)
                {
                    tcpClient.Close();
                }
                listenThread.Abort();
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
            nStream.Write(msg, 0, msg.Length);
        }
        private void sendButton_Click(object sender, EventArgs e)
        {
            SendMessage(messageTextBox.Text);
            chatTextBox.Text += $"me:{messageTextBox.Text}\n";
            messageTextBox.Text = "";
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