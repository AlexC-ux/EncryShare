
namespace EncryShare
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.connectButton = new System.Windows.Forms.Button();
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.chatTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(14, 48);
            this.connectButton.Margin = new System.Windows.Forms.Padding(1);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(86, 28);
            this.connectButton.TabIndex = 11;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // ipTextBox
            // 
            this.ipTextBox.Location = new System.Drawing.Point(119, 48);
            this.ipTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(106, 27);
            this.ipTextBox.TabIndex = 9;
            this.ipTextBox.Text = "127.0.0.1";
            // 
            // chatTextBox
            // 
            this.chatTextBox.Location = new System.Drawing.Point(14, 88);
            this.chatTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chatTextBox.Name = "chatTextBox";
            this.chatTextBox.ReadOnly = true;
            this.chatTextBox.Size = new System.Drawing.Size(395, 596);
            this.chatTextBox.TabIndex = 8;
            this.chatTextBox.Text = "";
            this.chatTextBox.TextChanged += new System.EventHandler(this.chatTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Client mode";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(11, 692);
            this.messageTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(397, 57);
            this.messageTextBox.TabIndex = 12;
            this.messageTextBox.Text = "";
            this.messageTextBox.TextChanged += new System.EventHandler(this.messageTextBox_TextChanged);
            this.messageTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.messageTextBox_KeyUp);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(306, 757);
            this.sendButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(103, 31);
            this.sendButton.TabIndex = 13;
            this.sendButton.Text = "SEND";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 757);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 31);
            this.button1.TabIndex = 18;
            this.button1.Text = "Send file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.button1_KeyUp);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(250, 17);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(71, 24);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "sound";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(119, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 20;
            this.label2.Text = "label2";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 795);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.ipTextBox);
            this.Controls.Add(this.chatTextBox);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ClientForm";
            this.Text = "ClientForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientForm_FormClosed);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.RichTextBox chatTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox messageTextBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}