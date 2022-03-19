using System;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EncryShare
{
    public partial class ClientForm : Form
    {
        byte[] rsaExponentReceived;
        byte[] rsaModulusReceived;
        byte[] aesEncryptedKey;
        byte[] aesEncryptedIV;

        bool resFile;
        SoundPlayer notifySound = new SoundPlayer(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\Media\Speech On.wav");
        Thread receiveFilesThread;
        Thread receiveFileListenerThread;
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        OpenFileDialog getFileDialog = new OpenFileDialog();
        TcpClient tcpClient;
        TcpListener tcpFileListener;
        TcpClient tcpFileClient = new TcpClient();
        NetworkStream nStream;
        NetworkStream fileNStream = null;
        Thread receiveThread;
        public ClientForm()
        {
            InitializeComponent();
            toolTip1.SetToolTip(label2, "Click on your ip to copy it to the clipboard!");
        }

        private void Connect()
        {


            try
            {

                tcpClient = new TcpClient();

                chatTextBox.Text += $"Начато подключение к {IPAddress.Parse(ipTextBox.Text)}\n";

                //tcpClient.SendTimeout = 7000;
                //tcpClient.ReceiveTimeout = 7000;

                //tcpClient.Connect(ipTextBox.Text.ToString(), 60755);
                //tcpClient.ConnectAsync(ipTextBox.Text, 60755).Wait(30000);
                tcpClient.Connect(IPAddress.Parse(ipTextBox.Text), 60755);

                //tcpClient.Connect(Dns.GetHostEntry(ipTextBox.Text.ToString()).AddressList[0], 60755);
                while (!tcpClient.Connected) { continue; }
                nStream = tcpClient.GetStream();
                receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start();
                chatTextBox.Text = ("Соединение установлено.\n");


                receiveFileListenerThread = new Thread(WaitFileConnection);
                receiveFileListenerThread.Start();

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
                        resFile = true;


                    }
                }
            }
        }

        private void ReceiveFileBytes()
        {

            while (tcpFileClient.Connected)
            {

                try
                {
                    byte[] data = new byte[268435456]; // буфер для получаемых данных
                    int bytes = 0;
                    do
                    {

                        try
                        {
                            bytes = fileNStream.Read(data, 0, data.Length);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    }
                    while (fileNStream.DataAvailable);
                    if (bytes > 0)
                    {
                        string fileName = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads" + @"\" + DateTime.Now.Year + DateTime.Now.DayOfYear + DateTime.Now.DayOfWeek + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".encryshare";
                        byte[] decryptedData = CryptoTools.CryptoTools.DecryptToByte(data, CryptoTools.CryptoTools.myAes.Key, CryptoTools.CryptoTools.myAes.IV, bytes);
                        FileStream fs = File.Create(fileName, decryptedData.Length);
                        fs.Write(decryptedData, 0, decryptedData.Length);
                        fs.Close();
                        chatTextBox.Text += $"Принятый файл сохранён в загрузках как\n{fileName}\n";
                        SendMessage("\nФайл успешно доставлен.\n");
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
                    byte[] data = new byte[65536]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        try
                        {
                            if (rsaModulusReceived == null)
                            {
                                if (rsaExponentReceived == null)
                                {
                                    data = new byte[3];
                                    bytes = nStream.Read(data, 0, 3);
                                }
                                else
                                {
                                    data = new byte[256];
                                    bytes = nStream.Read(data, 0, 256);
                                }
                            }
                            else
                            {
                                bytes = nStream.Read(data, 0, data.Length);
                            }

                            //builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        catch { }
                    }
                    while (nStream.DataAvailable);

                    //string message = builder.ToString();

                    if (rsaModulusReceived == null)
                    {
                        if (rsaExponentReceived == null)
                        {
                            rsaExponentReceived = data;
                        }
                        else
                        {
                            rsaModulusReceived = data;

                            CryptoTools.CryptoTools.SetRSAOpenKeys(rsaModulusReceived, rsaExponentReceived);
                            aesEncryptedKey = CryptoTools.CryptoTools.EncryptAESKey();
                            aesEncryptedIV = CryptoTools.CryptoTools.EncryptAESIV();

                            nStream.Write(aesEncryptedKey, 0, aesEncryptedKey.Length);
                            nStream.Write(aesEncryptedIV, 0, aesEncryptedIV.Length);
                            messageTextBox.Enabled = true;
                            sendButton.Enabled = true;
                            button1.Enabled = true;


                        }

                    }
                    else
                    {
                        string message = Encoding.Default.GetString(CryptoTools.CryptoTools.DecryptToByte(data, CryptoTools.CryptoTools.myAes.Key, CryptoTools.CryptoTools.myAes.IV, bytes));
                        chatTextBox.AppendText($"\nany: " + message);
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            ipTextBox.Enabled = false;
            Thread connectThread = new Thread(Connect);
            connectThread.Start();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            label2.Text = new WebClient().DownloadString("http://icanhazip.com/");
            sendButton.Enabled = false;
            messageTextBox.Enabled = false;
            button1.Enabled = false;
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            try
            {
                SendMessage("\nСоединение принудительно разорвано!\n");

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
            if (tcpClient.Connected)
            {
                byte[] msg = CryptoTools.CryptoTools.EncryptString(message, CryptoTools.CryptoTools.myAes.Key, CryptoTools.CryptoTools.myAes.IV);
                nStream.Write(msg, 0, msg.Length);
            }
            else { MessageBox.Show(text: "Сессия завершена!", caption: "Ошибка отправки текстового сообщения", buttons: MessageBoxButtons.OK); }

        }

        private void sendButton_Click(object sender, EventArgs e)
        {

            SendMessage(messageTextBox.Text);
            chatTextBox.AppendText($"\nme: {messageTextBox.Text}");

            messageTextBox.Text = "";
        }

        private void messageTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                if (!tcpFileClient.Connected)
                {
                    tcpFileClient.Connect(ipTextBox.Text, 60766);
                    fileNStream = tcpFileClient.GetStream();

                }

                if (!resFile)
                {
                    receiveFilesThread = new Thread(ReceiveFileBytes);
                    receiveFilesThread.Start();
                }

                if (getFileDialog.ShowDialog() == DialogResult.OK)
                {

                    if (getFileDialog.FileName.Length > 0)
                    {
                        try
                        {

                            byte[] data = File.ReadAllBytes(getFileDialog.FileName);

                            byte[] encryBytes = CryptoTools.CryptoTools.EncryptFileToByte(getFileDialog.FileName, CryptoTools.CryptoTools.myAes.Key, CryptoTools.CryptoTools.myAes.IV, data.Length);
                            fileNStream.Write(encryBytes, 0, encryBytes.Length);
                            SendMessage("Вам передан файл " + getFileDialog.FileName.Split('\\')[getFileDialog.FileName.Split('\\').Length - 1]);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
            catch { MessageBox.Show(text: "Сессия завершена!", caption: "Ошибка отправки файла", buttons: MessageBoxButtons.OK); }

        }

        private void chatTextBox_TextChanged(object sender, EventArgs e)
        {
            chatTextBox.SelectionStart = chatTextBox.Text.Length;
            chatTextBox.ScrollToCaret();
            if (checkBox1.Checked) { notifySound.Play(); }
        }

        private void button1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void messageTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && messageTextBox.Enabled)
            {
                messageTextBox.Text.Remove(-1);
                sendButton.PerformClick();
                messageTextBox.Focus();
            }
            if (e.KeyCode == Keys.Down && messageTextBox.Enabled)
            {
                messageTextBox.Text = messageTextBox.Text + '\n';
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void label2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(label2.Text.Replace("\n", ""));
            MessageBox.Show("IP copied to the clipboard!");
        }
    }
}
